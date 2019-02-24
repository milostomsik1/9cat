using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using _9Cat.Domain.Services;
using _9Cat.Domain.Models;

namespace _9Cat.Api.Controllers
{
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/login")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IMagicLinkService _magicLinkService;

		public LoginController(IConfiguration configuration, IMagicLinkService magicLinkService)
		{
			_configuration = configuration;
			_magicLinkService = magicLinkService;
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("magic-link")]
		public async Task<ActionResult> SendMagicLinkEmailAsync([FromBody] LoginCredentials loginCredentials)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var result = await _magicLinkService.SendMagicLinkEmailAsync(loginCredentials.Email);

			return StatusCode((int)result.StatusCode);
		}

		//[Authorize(Policy = "IsMyself")]
		[HttpGet]
		[Route("magic-link")]
		public ActionResult VerifyLogin()
		{
			var me = User;
			return Ok();
		}
	}
}
