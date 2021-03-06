using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Hotel.BLogicLayer;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Exceptions;
using Hotel.BLogicLayer.Interfaces;
using Hotel.PRLAYER.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.PRLAYER.Controler
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IGuestService _guestService;
        private IMapperItem _mapperItem;
        private IRoleService _roleService;
        public AccountController(IMapperItem mapperItem, IRoleService roleService, IGuestService guestService)
        {
            _mapperItem = mapperItem;
            _roleService = roleService;
            _guestService = guestService;
        }



        [HttpPost]
        public IActionResult RegisterUser(GuestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string roleName = "User";
                    var userRole = _roleService.GetRoleByName(roleName);
                    model.GuestRegisterInfo.RoleId = userRole.Id;
                    model.Id = Guid.NewGuid();
                    _guestService.CreateGuest(_mapperItem.Mapper.Map<GuestDto>(model));
                    return Ok();
                }
                catch (KeyNotFoundException kf)
                {
                    Debug.WriteLine(kf.Message);
                    Debug.WriteLine("System Error. Not found Role!");
                    return StatusCode(500);
                }
                catch (GuestEmailAlreadyExistException ge)
                {
                    Debug.WriteLine(ge.Message);
                    ModelState.AddModelError("Login", "User with current e-mail are already contains");
                }
                catch (GuestUserNameAlreadyExistException gu)
                {
                    Debug.WriteLine(gu.Message);
                    ModelState.AddModelError("UserName", "User with current user name are already exist");
                }
            }

            return BadRequest(ModelState);
        }


        [HttpPost("Token")]
        public IActionResult GetToken(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var identity = GetIdentity(model.Email, model.Password);
                    if (identity != null)
                    {
                        var now = DateTime.UtcNow;

                        var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            notBefore: now,
                            claims: identity.Claims,
                            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                            signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                            );

                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                        var response = new
                        {
                            access_token = encodedJwt,
                            username = identity.Name
                        };

                        return Json(response);
                    }
                }catch(ArgumentException)
                {
                    return StatusCode(500);
                }
            }

            return BadRequest(ModelState);
        }


        [NonAction]
        public ClaimsIdentity GetIdentity(String email, String password)
        {

            try
            {
                var guest = _guestService.GetGuestByEmailAndPassword(email, password);
                var guestRole = _roleService.GetRole(guest.GuestRegisterInfo.RoleId);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, guest.GuestRegisterInfo.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, guestRole.RoleName), 
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            catch (KeyNotFoundException kf)
            {
                Debug.WriteLine(kf.Message);
                ModelState.AddModelError("", "Invalid Email or Password");
            }
            catch (ArgumentException ae)
            {
                Debug.WriteLine(ae.Message);
                throw;
            }

            return null;
        }


    }
}