using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuth.Context;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public EmployeeController(AppDbContext context,
                                 UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
           
        }
        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Employees.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetByName")]
        public async Task<IActionResult> GetByName([FromQuery] string fullname)
        {
            return Ok(await _context.Employees.Where(x => x.FullName.Contains(fullname)).ToListAsync());
        }
    }
}
