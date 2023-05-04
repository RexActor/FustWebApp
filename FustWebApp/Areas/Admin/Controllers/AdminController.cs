using FustWebApp.Data;
using FustWebApp.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FustWebApp.Controllers
{
	[Authorize]

	[Area("Admin")]
	[Route("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public AdminController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Returns StockHolding Viewmodel into page</returns>

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			List<StockholdingViewModel> stockHoldingList = new List<StockholdingViewModel>();
			await applicationDbContext.StockHolding.Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).ForEachAsync(item =>
			{

				if (stockHoldingList.FirstOrDefault(holding => holding.StockHoldingFustItems == item.StockHoldingFustItems) == null)
				{

					stockHoldingList.Add(new StockholdingViewModel()
					{
						StockHoldingSupplier = item.StockHoldingSupplier,
						StockholdingDate = item.StockholdingDate,
						StockHoldingFustItems = item.StockHoldingFustItems,
						StockHoldingId = item.StockHoldingId,
						StockHoldingQty = applicationDbContext.StockHolding.Where(fust => fust.StockHoldingFustItems == item.StockHoldingFustItems).Sum(x => x.StockHoldingQty)
					});
				}
			});

			ViewBag.StockHoldingList = stockHoldingList.OrderBy(item => item.StockHoldingFustItems.FustType.FustTypeName);
			return View();
		}
	}
}
