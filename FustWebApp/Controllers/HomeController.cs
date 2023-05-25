using FustWebApp.Data;
using FustWebApp.Models;
using FustWebApp.Models.Domain;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;


namespace FustWebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext applicationDbContext;
		private readonly UserManager<IdentityUser> userManager;


		public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userMgr)
		{
			_logger = logger;
			this.applicationDbContext = applicationDbContext;
			userManager = userMgr;
		}


		[Authorize]
		public async Task<IActionResult> Index()
		{

			List<StockholdingViewModel> stockHoldingList = new List<StockholdingViewModel>();
			

			List<Loads>  InboundList = await applicationDbContext.Loads.Include(item=>item.LoadSupplier).ThenInclude(item=>item.Currency).OrderBy(item => item.LoadDate).ToListAsync();

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


			ViewBag.InboundList = InboundList;
			ViewBag.StockHoldingList = stockHoldingList.OrderBy(item => item.StockHoldingFustItems.FustType.FustTypeName);

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });


		public IActionResult AccessDenied() => View();

	}
}