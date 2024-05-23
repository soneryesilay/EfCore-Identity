using EfCore.Identity.Context;
using EfCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connect to the database
builder.Services.AddDbContext<AppDbContext>(opt =>
{
   opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

//add identity
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
	opt.Password.RequireNonAlphanumeric=false;
	opt.Password.RequireDigit=false;
	opt.Password.RequireLowercase=false;
	opt.Password.RequireUppercase=false;
	opt.Password.RequiredLength=1;
	opt.User.RequireUniqueEmail=true;
	
	opt.SignIn.RequireConfirmedEmail= true;
	opt.Lockout.MaxFailedAccessAttempts= 5; //5. giriþte hesabý
	opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(1); //1 dakika kitle

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
