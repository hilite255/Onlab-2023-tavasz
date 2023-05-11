using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using SharedProject.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var res = await userService.GetAll();
            return Ok(res);
        }
        
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterDto user)
        {
            var regex = new Regex("^\\S+@\\S+\\.\\S+$");
            if(!regex.IsMatch(user.Email)) { return BadRequest("Az email formátuma nem megfelelő"); }
            var res = await userService.Register(user.Username, user.Password, user.Email);
            if (res)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("login")]
        public async  Task<IActionResult> Login([FromBody]LoginDto user)
        {
            var res = await userService.Login(user.Username, user.Password);
            if (res == null)
                return BadRequest("Wrong username or password");
            return Ok(res);
                
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userService.GetUserById(userId);
            if (user == null) return BadRequest();
            return Ok(new {
                role = user.Permission.ToString(),
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await userService.GetUserById(id);
            if (user == null) return BadRequest();
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody]UpdateUserDto newUser)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res = await userService.UpdateUser(userId, newUser.Firstname, newUser.Lastname, newUser.Oldpassword, newUser.Password);

            return Ok(res);
        }
    }
}
