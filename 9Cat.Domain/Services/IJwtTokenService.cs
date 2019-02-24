namespace _9Cat.Domain.Services
{
	public interface IJwtTokenService
	{
		/// <summary>
		/// Generates a JWT token for a given email address
		/// </summary>
		/// <param name="email"></param>
		/// <returns>JWT Token in string format</returns>
		string GenerateToken(string email);
	}
}
