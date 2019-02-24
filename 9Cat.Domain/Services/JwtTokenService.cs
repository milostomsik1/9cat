using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace _9Cat.Domain.Services
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly IConfiguration _configuration;

		public JwtTokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(string email)
		{
			var claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Iss, _configuration.GetSection("Hostname")?.Value),
				new Claim(JwtRegisteredClaimNames.Aud, _configuration.GetSection("Hostname")?.Value),
				new Claim(JwtRegisteredClaimNames.Sub, email),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("IssuerSigningKey")?.Value));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration.GetSection("Hostname")?.Value,
				audience: _configuration.GetSection("Hostname")?.Value,
				claims: claims,
				expires: DateTime.UtcNow.AddDays(30),
				signingCredentials: credentials
			);

			var result = new JwtSecurityTokenHandler().WriteToken(token);
			return result;
		}
	}
}
