using ExpenseTrackerAPI.DataAccess;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTrackerAPI.Services
{
	public class UserServices
	{
		private readonly EFDbContext _dbContext;
		private readonly IConfiguration _config;

		public UserServices(EFDbContext dbContext, IConfiguration config)
		{
			_dbContext = dbContext;
			_config = config;
		}


		public async Task<UserModel> RegisterUser(UserModel user)
		{
			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();
			return user;
		}
		public async Task<UserModel> LoginUser(LoginModel login)
		{
			var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == login.Email);
			if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
			{
				return null;
			}
			return user;
		}

		public string GenerateJwtToken(UserModel user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentialls = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			};

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: credentialls
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


	}
}
