using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.PRLAYER.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.PRLAYER.Controler
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class RoomsController : Controller
    {
        private IRoomService _roomService;
        private IMapperItem _mapper;
        private ICategoryService _categoryService;
        
        public RoomsController(IRoomService roomService, IMapperItem mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetRooms()
        {
            var rooms = _roomService.GetRooms();
            if (!rooms.Any())
            {
                return NoContent();
            }
            return Json(_mapper.Mapper.Map<IEnumerable<RoomDto>, IEnumerable<RoomModel>>(rooms));
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateRoom([Bind("RoomNumber", "CategoryId")]RoomModel model)
        {
            if (ModelState.IsValid)
            {
                if(TryValidateRoomCategoryAndMakeDbAction(model, room =>
                {
                    _roomService.CreateRoom(room);
                }))
                {
                    return Created(HttpContext.Request.Path, model);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateRoom([Bind("Id", "RoomNumber","CategoryId" )] RoomModel room)
        {
            if (ModelState.IsValid)
            {
                
                if (_roomService.GetRoom(room.Id) != null && TryValidateRoomCategoryAndMakeDbAction(room, roomDto => { _roomService.UpdateRoom(roomDto); }))
                {
                    return Ok();
                } 
            }
            return BadRequest(ModelState);
        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteRoom(Guid Id)
        {
            if (ModelState.IsValid)
            {
                if (_roomService.GetRoom(Id) != null)
                {
                    _roomService.DeleteRoom(Id);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Can't delete room, because it not fount with current Id");
                }

            }

            return BadRequest(ModelState);
        }

        private bool TryValidateRoomCategoryAndMakeDbAction(RoomModel room, Action<RoomDto> dbRequest)
        {
            if (_categoryService.GetCategory(room.CategoryId) != null)
            {
                room.Id = Guid.NewGuid();
                var mappedRoom = _mapper.Mapper.Map<RoomDto>(room);
                dbRequest.Invoke(mappedRoom);
                return true;
            }
            ModelState.AddModelError("CategoryId","Can't find category with given Id");
            
            return false;
        }
        
    }
        
           
}
    
