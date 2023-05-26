using FustWebApp.Data;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize(Roles = "Finance")]
	[Area("Admin")]
	public class CurrencyController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;


		public CurrencyController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		public async Task<IActionResult> Index()
		{
			if (TempData["result"] != null)
			{
				switch (TempData["result"]?.ToString())
				{
					case "Success":
						ViewBag.Message = $"{TempData["Action"]} Successfully";
						ViewBag.Style = "alert-success";
						break;
					case "Fail":
						ViewBag.Message = $"{TempData["Action"]} Failed. Please try again!";
						ViewBag.Style = "alert-danger";
						break;
				}
			}

			return View(await applicationDbContext.Currency.ToListAsync());
		} 


		[HttpGet]
		public IActionResult AddCurrency() => View();


		[HttpPost]
		public async Task<IActionResult> AddCurrency(Currency currency)
		{

			applicationDbContext.Currency.Add(currency);
			await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> View(int id) => View(await applicationDbContext.Currency.FindAsync(id));

		[HttpGet]
		public async Task<IActionResult> Edit(int id) => View(await applicationDbContext.Currency.FindAsync(id));

		[HttpPost]
		public async Task<IActionResult> Edit(Currency currency)
		{
			applicationDbContext.Currency.Update(currency);
			await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

			return RedirectToRoute(new
			{
				Controller = "Currency",
				Action = "View",
				id = currency.currencyId
			});
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{

			bool referenced = applicationDbContext.Suppliers.Any(e=>e.Currency.currencyId == id);

			if (referenced)
			{
				TempData["result"] = "Fail";
				TempData["action"] = "Currency is linked with Supplier. Currency Deletion";
				return RedirectToAction("Index");
			}



			var currencyToRemove = await applicationDbContext.Currency.FindAsync(id);
			if (currencyToRemove == null)
			{
				throw new ArgumentException($"No Currency found with ID {id}");
			}
			applicationDbContext.Currency.Remove(currencyToRemove);
			await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
			return RedirectToAction("Index");
		}

	}
}
