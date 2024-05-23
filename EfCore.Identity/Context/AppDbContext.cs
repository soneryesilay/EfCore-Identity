using EfCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Identity.Context
{
	public class AppDbContext : IdentityDbContext<AppUser,AppRole,Guid,IdentityUserClaim<Guid>,AppUserRole,IdentityUserLogin<Guid>,IdentityRoleClaim<Guid>,IdentityUserToken<Guid>>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	builder.Entity<AppUserRole>().has(x => new { x.UserId, x.RoleId });//composite key
		//	builder.Ignore<IdentityUserLogin<Guid>>(); //ignore the IdentityUserLogin
		//	builder.Ignore<IdentityUserClaim<Guid>>(); //ignore the IdentityUserClaim
		//	builder.Ignore<IdentityUserToken<Guid>>(); //ignore the IdentityUserToken
		//	builder.Ignore<IdentityRoleClaim<Guid>>(); //ignore the IdentityRoleClaim
		//	// vs vs veri tabanından kaldırmak ve daha sadeve projey yönelik identity hazırlamk için
		//	//kütüphaneyi bu şekilde özelleştirebilirsin.
		//}
	}
}
