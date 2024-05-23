using EfCore.Identity.Dtos;
using EfCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EfCore.Identity.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController(
		UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
		{
			AppUser appUser = new()
			{
				Email = request.Email,
				UserName = request.UserName,
				FirstName = request.FirstName,
				LastName = request.LastName,
			};

			IdentityResult result = await userManager.CreateAsync(appUser, request.Password);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors.Select(s=>s.Description));
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordDto request,CancellationToken cancellationToken)
		{
			AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());
			if (appUser is null)
			{
				return BadRequest(new {Message = "Kullanıcı Bulunamadı"});
			}

			IdentityResult result= await userManager.ChangePasswordAsync(appUser, request.CurrentPassword, request.NewPassword);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors.Select(s=>s.Description));
			}
			return NoContent();
		}

		[HttpGet]
		public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
		{
			AppUser? appUser = await userManager.FindByEmailAsync(email);
			if (appUser is null)
			{
				return BadRequest(new {Message = "Kullanıcı Bulunamadı"});
			}

			string token = await userManager.GeneratePasswordResetTokenAsync(appUser);
			return Ok(new{Token = token});
		}

		[HttpPost]
		public async Task<IActionResult> ChangePasswordUsingToken(ChangePasswordUsingTokenDto request, CancellationToken cancellationToken)
		{
			AppUser? appUser = await userManager.FindByEmailAsync(request.Email);
			if (appUser is null)
			{
				return BadRequest(new {Message = "Kullanıcı Bulunamadı"});
			}

			IdentityResult result = await userManager.ResetPasswordAsync(appUser, request.Token, request.NewPassword);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors.Select(s=>s.Description));
			}
			return NoContent();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
		{
			AppUser? appUser = 
				await userManager.
					Users.FirstOrDefaultAsync
					(p => p.Email == request.userNameOrEmail || 
					      p.UserName == request.userNameOrEmail, cancellationToken);
			if (appUser is null)
			{
		  	return BadRequest(new {Message = "Kullanıcı Bulunamadı"});
		    }

			bool result = await userManager.CheckPasswordAsync(appUser, request.Password);
			if (!result)
			{
				return BadRequest(new {Message = "Yanlış şifre veya Email"});
			}
			return Ok(new {Token = "Token"});

		}

		[HttpPost]
		public async Task<IActionResult> LoginWithSignInManager(LoginDto request, CancellationToken cancellationToken)
		{
			AppUser? appUser =
				await userManager.
					Users.FirstOrDefaultAsync
					(p => p.Email == request.userNameOrEmail ||
					      p.UserName == request.userNameOrEmail, cancellationToken);
			if (appUser is null)
			{
				return BadRequest(new { Message = "Kullanıcı Bulunamadı" });
			}

			SignInResult result  = await signInManager.CheckPasswordSignInAsync(appUser, request.Password,true);
		

			if (result.IsLockedOut)
			{
				TimeSpan? timeSpan = appUser.LockoutEnd - DateTime.Now;
				if (timeSpan is not null)
				{
					return BadRequest(new { Message = $"Hesabınız kilitlenmiştir. {timeSpan.Value.TotalMinutes} dakika sonra tekrar deneyin" });
				}
				else
				{
					return BadRequest(new { Message = "Hesabınız kilitlenmiştir. Daha sonra tekrar deneyin" });
				}
				
			}

			if (result.IsNotAllowed)
			{
				return BadRequest(new { Message = "Email adresinizi onaylayın" });
			}

			if (result.Succeeded)
			{
				return BadRequest("Şifreniz veya emailiniz yanlış");

			}

			return Ok(new { Token = "Token" });

		}


	}
}

