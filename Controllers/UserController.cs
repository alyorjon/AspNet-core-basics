// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using api.ApplicationDbContext;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;

// namespace api.Controllers
// {
//     [Route("api/user")]
//     [ApiController]
//     public class UserController : ControllerBase
//     {
//         private readonly ApplicationDBContext _context;
//         private readonly UserManager<AppUser> _userManager1;
//         public UserController(UserManager<AppUser> userManager, ApplicationDBContext context)
//         {
//             _userManager1 = userManager;
//             _context = context;
//         }

//         [HttpPost("/register")]
//         public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
//         {

//         }
//     }
// }