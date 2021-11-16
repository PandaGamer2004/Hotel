using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.Exceptions;
using Hotel.BLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.PRLAYER.Controler
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingAndChekInController : Controller
    {
        private IStayService _stayService;
        private IBookingAndCheckInService _bookingService;
        private IGuestService _guestService;
        public BookingAndChekInController(IBookingAndCheckInService bookingService, IStayService stayService, IGuestService guestService)
        {
            _bookingService = bookingService;
            _stayService = stayService;
            _guestService = guestService;
        }


        
        [HttpGet]
        [Route("Bookings")]
        public IActionResult GetBookings()
        {
            var bookings = _stayService.GetBookings();
            if (!bookings.Any())
            {
                return NoContent();
            }

            return Json(bookings);
        }
        
        
        
        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("RoomsOnDate")]
        public IActionResult GetFreeRomsOnDate(DateStartEndPair datePair)
        {
            if (ModelState.IsValid)
            {
                var roomsOnDate = _bookingService.GetFreeRoomsOnDate(datePair);
                if (!roomsOnDate.Any())
                {
                    return NoContent();
                }

                return Json(roomsOnDate);
            }

            return BadRequest(ModelState);
        }
        
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("CreateBooking")]
        public IActionResult CreateBooking(Guid roomId,DateStartEndPair datePair )
        {
            return CreateStay((Guid guestId) =>
                _bookingService.CreateBooking(guestId, roomId, datePair), "Can't create Booking on this date");
        }


        [Authorize(Roles = "User")]
        private Guid GetAuthorizedGuestId()
        {
            var guestNameClaim = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Name);
            if (guestNameClaim != null)
            { 
                return _guestService.GetGuestByUserName(guestNameClaim.Value).Id;
            }

            throw new AuthenticationException("Claim not found");
        }
        
        [Authorize(Roles = "User")]
        private IActionResult CreateStay(Action<Guid> bookingServiceAction, String modelErrorMessage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var guestId = GetAuthorizedGuestId();
                    bookingServiceAction.Invoke(guestId);
                }
                catch (UserNotExistException ue)
                {
                    Debug.WriteLine(ue.Message);
                    return StatusCode(500);
                }
                catch (StayAreAlreadyExistException ae)
                {
                    Debug.WriteLine(ae.Message);
                    ModelState.AddModelError("", modelErrorMessage);
                }
                catch (AuthenticationException ae)
                {
                    return StatusCode(401);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCheckIn(Guid roomId,DateStartEndPair checkInDate)
        {
            if (ModelState.IsValid)
            {
                CreateStay(
                    (Guid guestId) => _bookingService.CreateCheckIn(guestId, roomId, checkInDate),
                    
                    "Can't create chekIn on given date");
            }

            return BadRequest(ModelState);
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult CheckOut(Guid roomId, DateTime chekOutDate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bookingService.CheckOut(roomId, chekOutDate);
                }
                catch (ArgumentException ae)
                {
                    Debug.WriteLine(ae.Message);
                    ModelState.AddModelError("","Can not foun stay for current room on date");
                }
            }

            return BadRequest(ModelState);
        }
        
    }
}