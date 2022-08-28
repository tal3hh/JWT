using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuth.Context;
using JwtAuth.Dtos.Account;
using JwtAuth.Models;
using JwtAuth.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountController(AppDbContext context,
                                 UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager, 
                                 ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new AppUser()
            {
                FullName = dto.Fullname,
                Email = dto.Email,
                UserName = dto.Fullname.Substring(0,5) + "_00",
            };
            await _userManager.CreateAsync(user, dto.Password);
            //await _userManager.AddToRoleAsync(user, "Admin");

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


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user is null) return NotFound();
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            string token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

            return Ok(token);
        }


    }
}
