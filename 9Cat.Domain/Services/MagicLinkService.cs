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

		public MagicLinkService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<Response> SendMagicLinkEmailAsync(string emailTo)
		{
			var apiKey = _configuration.GetSection("SendGridApiKeys")?.Value;
			if (apiKey == null)
			{
				throw new Exception("Can't find SendGridApiKey value in configuration.");
			}

			var client = new SendGridClient(apiKey);

			var msg = new SendGridMessage();
			msg.SetFrom("login@9cat.com");
			msg.AddTo(emailTo);
			msg.SetSubject("Login Magic Link for 9Cat.com");
			msg.AddContent(MimeType.Text, $"Hello! You can login to 9Cat.com with {emailTo}");

			var result = await client.SendEmailAsync(msg);
			return result;
		}
	}
}
