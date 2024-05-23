namespace EfCore.Identity.Dtos
{
	public sealed record LoginDto(string userNameOrEmail, string Password);
	
}
