using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
	[ApiController]
	public class UsersController : Controller
	{
		private readonly UserServices _userServices;
		public UsersController(UserServices userServices)
		{
			_userServices = userServices;
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] UserModel user)
		{
			var newUser = await _userServices.RegisterUser(user);
			return Ok(newUser);
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginModel login)
		{
			var user = await _userServices.LoginUser(login);
			if (user == null)
			{
				return Unauthorized();
			}
			var token = _userServices.GenerateJwtToken(user);
			return Ok(new { token });
		}
	}
}
