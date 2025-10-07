using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SampleApp.Controllers;

public class AccountController : Controller
{
	private readonly IConfiguration configuration;

	public AccountController(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

	[HttpGet]
	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync();
		return Redirect("~/");
	}

	[HttpPost]
	public async Task<IActionResult> Login(string password)
	{
		var adminPassword = configuration["adminPassword"];
		if (!string.Equals(adminPassword, password, StringComparison.CurrentCultureIgnoreCase))
		{
			return Forbid();
		}

		var props = new AuthenticationProperties
		{
			RedirectUri = "~/hangfire"
		};

		var claims = new[]
		{
			new Claim("sub","admin")
		};

		var idenity = new ClaimsIdentity(claims, "pwd", "name", "role");
		await HttpContext.SignInAsync(new ClaimsPrincipal(idenity), props);

		return Redirect("~/hangfire");
	}
}
