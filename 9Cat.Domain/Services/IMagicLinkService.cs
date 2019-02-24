using SendGrid;
using System.Threading.Tasks;

namespace _9Cat.Domain.Services
{
	public interface IMagicLinkService
	{
		/// <summary>
		/// Sends an email containing login URL to the given address asynchronously
		/// </summary>
		/// <param name="mailTo"></param>
		/// <returns></returns>
		Task<Response> SendMagicLinkEmailAsync(string mailTo);
	}
}
