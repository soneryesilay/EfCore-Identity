using EfCore.Identity.Context;
using EfCore.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EfCore.Identity.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public sealed class UserRolesController(AppDbContext context) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> Create(Guid userId, Guid roleId, CancellationToken cancellationToken)
		{
			AppUserRole appUserRole = new()
			{
				UserId = userId,
				RoleId = roleId
			};
			 await context.UserRoles.AddAsync(appUserRole);
			 await context.SaveChangesAsync(cancellationToken);

			 return NoContent();
		}
	}
}
