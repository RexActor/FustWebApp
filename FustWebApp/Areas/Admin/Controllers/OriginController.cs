using Audit.Core;

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
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OriginController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public OriginController(ApplicationDbContext applicationDbContext)
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
						ViewBag.Message = $"{TempData["action"]} Successfully";
						ViewBag.Style = "alert-success";
						break;
					case "Fail":
						ViewBag.Message = $"{TempData["action"]} Failed. {TempData["reason"]} Please try again!";
						ViewBag.Style = "alert-danger";
						break;
				}
			}

			var originViewModel = await applicationDbContext.Origins.ToListAsync();
			return View(originViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateOrigin(Origin origin, string Edit, string Delete)
		{
			if (Delete != null)
			{
				var originFound = await applicationDbContext.Suppliers.FirstOrDefaultAsync(item => item.SupplierOrigin == origin.OriginName);


				if (originFound != null)
				{
					TempData["result"] = "Fail";
					TempData["reason"] = "Origin is being used";
					return RedirectToAction("Index");
				}

				var originToDelete = new Origin()
				{
					Id = origin.Id,
				};

				applicationDbContext.Origins.Attach(originToDelete);
				applicationDbContext.Origins.Remove(originToDelete);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
				TempData["result"] = "Success";
				TempData["action"] = "Delete";

			}

			if (Edit != null)
			{
				var originToUpdate = await applicationDbContext.Origins.FirstOrDefaultAsync(item => item.Id == origin.Id);

				if (originToUpdate != null)
				{
					originToUpdate.OriginName = origin.OriginName;
					applicationDbContext.Origins.Update(originToUpdate);
					await applicationDbContext.SaveChangesAsync();
				}

				TempData["result"] = "Success";
				TempData["action"] = "Update";

			}
			return RedirectToAction("Index");

		}

		[HttpGet]
		public async Task<IActionResult> View(Guid Id) => View(await applicationDbContext.Origins.FirstOrDefaultAsync(item => item.Id == Id));

		[HttpGet]
		public IActionResult Add() => View();

		[HttpPost]
		public async Task<IActionResult> AddOrigin(AddOriginViewModel addOriginViewModel, string Save)
		{

			if (Save != null)
			{
				var originToAdd = new Origin()
				{
					Id = Guid.NewGuid(),
					OriginName = addOriginViewModel.OriginName,
				};

				await applicationDbContext.Origins.AddAsync(originToAdd);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
				TempData["result"] = "Success";
				TempData["action"] = "Origin Added";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["result"] = "Fail";
				TempData["action"] = "Origin Add";
				return RedirectToAction("Add");
			}

		}
	}
}
