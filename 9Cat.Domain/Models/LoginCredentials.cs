using System.ComponentModel.DataAnnotations;

namespace _9Cat.Domain.Models
{
	public class LoginCredentials
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
