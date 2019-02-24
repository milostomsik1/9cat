using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace _9Cat.Domain.Services
{
	public class MagicLinkService : IMagicLinkService
	{
		private readonly IConfiguration _configuration;
		private readonly IJwtTokenService _jwtTokenService;

		public MagicLinkService(IConfiguration configuration, IJwtTokenService jwtTokenService)
		{
			_configuration = configuration;
			_jwtTokenService = jwtTokenService;
		}

		public async Task<Response> SendMagicLinkEmailAsync(string emailTo)
		{
			var apiKey = _configuration.GetSection("SendGridApiKey")?.Value;
			if (apiKey == null)
			{
				throw new Exception("Can't find SendGridApiKey value in configuration.");
			}

			var client = new SendGridClient(apiKey);

			var token = _jwtTokenService.GenerateToken(emailTo);

			var msg = new SendGridMessage();
			msg.SetFrom("login@9cat.world");
			msg.AddTo(emailTo);
			msg.SetSubject("Login Magic Link for 9Cat.world");
			msg.AddContent(MimeType.Text, $"Hello! You can login to 9Cat.world with {emailTo}. Your token is: {token} and is valid for 30 days.");

			var result = await client.SendEmailAsync(msg);
			return result;
		}
	}
}
