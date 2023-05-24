using FustWebApp.Data;
using FustWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize]
	[Authorize(Roles =("Admin,Fust"))]
	[Area("Admin")]
	public class StockholdingController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public StockholdingController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		public async Task<IActionResult> Index() => View(await applicationDbContext.StockHolding.Include(item=>item.StockHoldingFustItems).ThenInclude(item=>item.FustType).Include(item=>item.StockHoldingSupplier).ThenInclude(item=>item.Currency).ToListAsync());
		
	}
}
