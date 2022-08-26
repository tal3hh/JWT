using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuth.Context;
using JwtAuth.Dtos.Account;
using JwtAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,
                                 UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new AppUser()
            {
                FullName = dto.Fullname,
                Email = dto.Email,
            };
            await _userManager.CreateAsync(user, dto.Password);
            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok();
        }



        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromQuery] string role)
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = role,
            });
            return Ok();
        }


    }
}
