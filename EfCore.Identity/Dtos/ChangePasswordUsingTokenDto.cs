﻿namespace EfCore.Identity.Dtos
{
	public sealed record ChangePasswordUsingTokenDto(string Email, 
		string Token, 
		string NewPassword);
	
}
