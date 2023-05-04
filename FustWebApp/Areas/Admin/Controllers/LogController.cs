using FustWebApp.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize]
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class LogController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public LogController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> LogReader() => View(await applicationDbContext.AuditLogs.OrderByDescending(item => item.DateTime).ToListAsync());


	}



}
