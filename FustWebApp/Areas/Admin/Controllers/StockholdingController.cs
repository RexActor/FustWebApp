using FustWebApp.Data;
using FustWebApp.Models;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> AdjustmentCodes()
		{
			return View();
		}


		[HttpPost]

		public async Task<IActionResult> AddCode(AdjustmentCodes adjusmentCode)
		{
			if (ModelState.IsValid)
			{

				await applicationDbContext.Adjustments.AddAsync(adjusmentCode);

				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
			}


			return RedirectToAction("AdjustmentCodes");
		}


		[HttpGet]
		public async Task<IActionResult> Delete(int Id)
		{
			var codeToRemove = await applicationDbContext.Adjustments.FindAsync(Id);
			if (codeToRemove == null)
			{
				throw new ArgumentException($"Can't locate adjusment code with ID {Id}");
			}

			applicationDbContext.Adjustments.Remove(codeToRemove);
			await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

			return RedirectToAction("AdjustmentCodes");
		}


	}
}
