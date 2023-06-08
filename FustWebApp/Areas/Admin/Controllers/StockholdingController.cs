

using FustWebApp.Data;
using FustWebApp.Migrations;
using FustWebApp.Models;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize]
	[Authorize(Roles = ("Admin,Fust"))]
	[Area("Admin")]
	public class StockholdingController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;




		public StockholdingController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		[HttpGet]
		public async Task<IActionResult> Index() => View(await applicationDbContext.StockHolding.Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).Include(item => item.StockHoldingSupplier).ThenInclude(item => item.Currency).ToListAsync());

		[HttpGet]
		public async Task<IActionResult> EditStock(int stockId, Guid Supplier)
		{

			StockHolding model = await applicationDbContext.StockHolding.Include(item => item.StockHoldingSupplier).ThenInclude(item => item.Currency).Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).FirstOrDefaultAsync(item => item.StockHoldingId == stockId);

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Adjustments() => View(await applicationDbContext.Adjustments.Include(item=>item.AdjustmentCode).Include(item=>item.stockHolding).ThenInclude(item=>item.StockHoldingFustItems).ThenInclude(item=>item.FustType).Include(item=>item.stockHolding.StockHoldingSupplier).ThenInclude(item=>item.Currency).ToListAsync());


		[HttpGet]
		public async Task<IActionResult> EditAdjustment(int adjustmentId)
		{
			


			return View(await applicationDbContext.Adjustments.Include(item => item.stockHolding).ThenInclude(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).Include(item => item.stockHolding.StockHoldingSupplier).ThenInclude(item => item.Currency).FirstOrDefaultAsync(item=>item.Id==adjustmentId));
		}



		[HttpPost]
		public async Task<IActionResult> EditStock(int stockId, string reason, string reasonCode, int from, int to)
		{

			var locatedStock = await applicationDbContext.StockHolding.Include(item=>item.StockHoldingSupplier).ThenInclude(item=>item.Currency).Include(item=>item.StockHoldingFustItems).ThenInclude(item=>item.FustType).FirstOrDefaultAsync(item=>item.StockHoldingId==stockId);
			var adjusmentCode = await applicationDbContext.AdjustmentCodes.FirstOrDefaultAsync(item => item.AdjustmentCode == reasonCode);



			Stockadjustments adjustment = new Stockadjustments()
			{

				stockHolding = locatedStock,
				FromQuantity = from,
				ToQuantity = to,
				Reason = reason,
				AdjustmentCode = adjusmentCode,
				adjustmentDate = DateTime.Now,

			};

			locatedStock.StockHoldingQty = to;


			applicationDbContext.StockHolding.Update(locatedStock);
			await applicationDbContext.Adjustments.AddAsync(adjustment);
			await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);


			return RedirectToAction("Index");
		}


		[HttpGet]
		public async Task<IActionResult> AdjustmentCodes()
		{
			return View();
		}


		[HttpPost]

		public async Task<IActionResult> AddCode(AdjustmentCodes adjusmentCode)
		{
			if (ModelState.IsValid)
			{

				await applicationDbContext.AdjustmentCodes.AddAsync(adjusmentCode);

				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
			}


			return RedirectToAction("AdjustmentCodes");
		}


		[HttpGet]
		public async Task<IActionResult> Delete(int Id)
		{
			var codeToRemove = await applicationDbContext.AdjustmentCodes.FindAsync(Id);
			if (codeToRemove == null)
			{
				throw new ArgumentException($"Can't locate adjusment code with ID {Id}");
			}

			applicationDbContext.AdjustmentCodes.Remove(codeToRemove);
			await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

			return RedirectToAction("AdjustmentCodes");
		}


	}
}
