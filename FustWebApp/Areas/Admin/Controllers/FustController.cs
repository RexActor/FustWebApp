﻿using Audit.Core;

using FustWebApp.Data;
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
	public class FustController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public FustController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		[HttpGet]
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
						ViewBag.Message = $"{TempData["action"]} Failed. {TempData["reason"]} Please try again! ";
						ViewBag.Style = "alert-danger";
						break;
				}
			}

			var viewModel = await applicationDbContext.Fusts.ToListAsync();
			ViewBag.FustTypes = await applicationDbContext.FustTypes.ToListAsync();

			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			List<string> fustTypeList = new();
			await applicationDbContext.FustTypes.ForEachAsync(item => { fustTypeList.Add(item.FustTypeName); });

			ViewBag.FustTypes = fustTypeList;
			return View();
		}

		[HttpGet]
		public IActionResult AddFustType() => View();


		[HttpPost]
		public async Task<IActionResult> SaveFust(Fust fustViewModel, string Save)
		{
			if (Save != null)
			{
				var findFustType = await applicationDbContext.FustTypes.FirstOrDefaultAsync(item => item.FustTypeName == fustViewModel.FustType.FustTypeName);

				if (findFustType != null)
				{
					var fustToSave = new Fust()
					{
						FustName = fustViewModel.FustName,
						FustAmountPerPallet = fustViewModel.FustAmountPerPallet,
						FustType = findFustType,
					};

					await applicationDbContext.Fusts.AddAsync(fustToSave);


					await applicationDbContext.StockHolding.Select(item => item.StockHoldingSupplier).Distinct().ForEachAsync(item =>
					{
						var stockItemToAdd = new StockHolding()
						{
							StockholdingDate = DateTime.Now,
							StockHoldingFustItems = fustToSave,
							StockHoldingQty = 0,
							StockHoldingSupplier = item,


						};
						applicationDbContext.StockHolding.Add(stockItemToAdd);

					});


					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					TempData["result"] = "Success";
					TempData["action"] = "Fust Item Added";
				}


				return RedirectToAction("Index");
			}
			TempData["result"] = "Fail";
			TempData["action"] = "Fust Item adding";
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> SaveFustType(FustType fustTypesViewModel, string Save)
		{

			if (Save != null)
			{
				var fustTypeToSave = new FustType()
				{
					FustTypeName = fustTypesViewModel.FustTypeName,
					Comment = fustTypesViewModel.Comment,
				};
				await applicationDbContext.FustTypes.AddAsync(fustTypeToSave);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

				TempData["result"] = "Success";
				TempData["action"] = "Fust Type Added";
				return RedirectToAction("Index");
			}
			TempData["result"] = "Success";
			TempData["action"] = "Fust Type adding";
			return RedirectToAction("Index");

		}

		[HttpGet]
		public async Task<IActionResult> View(int Id)
		{

			var fust = await applicationDbContext.Fusts.Where(item => item.Id == Id).Include(item => item.FustType).FirstOrDefaultAsync();

			if (fust != null)
			{
				var fustItemView = new Fust()
				{
					Id = fust.Id,
					FustAmountPerPallet = fust.FustAmountPerPallet,
					FustName = fust.FustName,
					FustType = fust.FustType,
					baseValue = fust.baseValue

				};

				List<string> fustTypeList = new();
				await applicationDbContext.FustTypes.ForEachAsync(item => { fustTypeList.Add(item.FustTypeName); });
				ViewBag.FustTypes = fustTypeList;
				return View(fustItemView);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFust(Fust updateFustViewModel, string Edit, string Delete)
		{

			if (Delete != null)
			{

				bool referenced = applicationDbContext.StockHolding.Any(e=>e.StockHoldingFustItems.Id==updateFustViewModel.Id);

				if (referenced)
				{

					TempData["result"] = "Fail";
					TempData["action"] = "Fust Item is linked with Stockholding. Fust Item Deletion";
					return RedirectToAction("Index");
				}

				var fustItemToRemove = new Fust()
				{
					Id = updateFustViewModel.Id,
				};
				


					applicationDbContext.Fusts.Attach(fustItemToRemove);
					applicationDbContext.Fusts.Remove(fustItemToRemove);
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					TempData["result"] = "Success";
					TempData["action"] = "Fust Item Deletion";
					return RedirectToAction("Index");
				
			}

			if (Edit != null)
			{
				var fustItemToUpdate = await applicationDbContext.Fusts.FirstOrDefaultAsync(item => item.Id == updateFustViewModel.Id);
				var findFustType = await applicationDbContext.FustTypes.FirstOrDefaultAsync(item => item.FustTypeName == updateFustViewModel.FustType.FustTypeName);


				if (fustItemToUpdate != null && findFustType != null)
				{
					applicationDbContext.Entry(fustItemToUpdate).State = EntityState.Unchanged;
					fustItemToUpdate.FustName = updateFustViewModel.FustName;
					fustItemToUpdate.FustAmountPerPallet = updateFustViewModel.FustAmountPerPallet;
					fustItemToUpdate.baseValue = updateFustViewModel.baseValue;
					fustItemToUpdate.FustType = findFustType;
					applicationDbContext.Fusts.Update(fustItemToUpdate);
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					TempData["result"] = "Success";
					TempData["action"] = "Fust Item Updated";
					return RedirectToAction("Index");
				}

			}
			TempData["result"] = "Failure";
			TempData["action"] = "Item not Updated";
			return RedirectToAction("Index");

		}

		[HttpGet]
		public IActionResult ViewFustType(int Id)
		{
			var fustType = applicationDbContext.FustTypes.FirstOrDefault(x => x.FustTypeId == Id);
			if (fustType != null)
			{
				var fustView = new FustType()
				{
					FustTypeId = fustType.FustTypeId,
					FustTypeName = fustType.FustTypeName,
					Comment = fustType.Comment,
				};

				return View(fustView);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public async Task<IActionResult> EditFustType(FustType updateFustTypeViewModel, string Delete, string Edit)
		{

			if (Delete != null)
			{
				var fustTypeUsed = applicationDbContext.Fusts.FirstOrDefault(item => item.FustType.FustTypeName == updateFustTypeViewModel.FustTypeName);
				var fustTypeSupplier = applicationDbContext.Suppliers.FirstOrDefault(item => item.FustTypeList.Contains(updateFustTypeViewModel.FustTypeName));

				if (fustTypeUsed != null || fustTypeSupplier != null)
				{
					TempData["result"] = "Fail";
					TempData["action"] = "Fust Type Deletion";
					TempData["reason"] = "Due that Fust Type is being used";

					return RedirectToAction("ViewFustType");
				}




				var fustTypeToRemove = new FustType()
				{
					FustTypeId = updateFustTypeViewModel.FustTypeId,
				};

				applicationDbContext.FustTypes.Attach(fustTypeToRemove);
				applicationDbContext.FustTypes.Remove(fustTypeToRemove);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
				TempData["result"] = "Success";
				TempData["action"] = "Fust Type Deleted";

			}
			if (Edit != null)
			{
				var fustTypeToUpdate = await applicationDbContext.FustTypes.SingleOrDefaultAsync(item => item.FustTypeId == updateFustTypeViewModel.FustTypeId);



				if (fustTypeToUpdate != null)
				{
					fustTypeToUpdate.FustTypeName = updateFustTypeViewModel.FustTypeName;
					fustTypeToUpdate.Comment = updateFustTypeViewModel.Comment;
					applicationDbContext.FustTypes.Update(fustTypeToUpdate);

					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
				}
				TempData["result"] = "Success";
				TempData["action"] = "Fust Type Updated";

			}

			return RedirectToAction("Index");

		}


		[HttpGet]
		[Authorize(Roles ="Finance")]
		public async Task<IActionResult> BaseValue() => View(await applicationDbContext.StockHolding.Include(item => item.StockHoldingSupplier).ThenInclude(item => item.Currency).Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).OrderBy(item=>item.StockHoldingSupplier.SupplierName).ToListAsync());



		[HttpGet]

		public IActionResult ViewSupplier(Guid SupplierId) => View(applicationDbContext.StockHolding.Include(item => item.StockHoldingSupplier).ThenInclude(item => item.Currency).Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).Where(item => item.StockHoldingSupplier.Id == SupplierId).ToList());

		[HttpGet]
		
		public async Task<IActionResult> EditSupplier(Guid SupplierId) => View(await applicationDbContext.StockHolding.Include(item => item.StockHoldingSupplier).ThenInclude(item => item.Currency).Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).Where(item => item.StockHoldingSupplier.Id == SupplierId).ToListAsync());


		[HttpPost]
		public async Task<IActionResult> EditSupplier(Guid supplierId, int fustItemId,float baseValue, int stockholdingId)
		{

			var itemtoUpdate = applicationDbContext.StockHolding.Include(item=>item.StockHoldingSupplier).ThenInclude(item=>item.Currency).Include(item=>item.StockHoldingFustItems).ThenInclude(item=>item.FustType).Where(item=>item.StockHoldingSupplier.Id== supplierId).Where(item=>item.StockHoldingFustItems.Id== fustItemId).Where(item=>item.StockHoldingId==stockholdingId).FirstOrDefault();
			if (itemtoUpdate != null)
			{
				itemtoUpdate.baseValue = baseValue;

				applicationDbContext.StockHolding.Update(itemtoUpdate);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

				return RedirectToRoute(new
				{
					Controller = "Fust",
					Action = "ViewSupplier",
					SupplierId = supplierId
				});
			}
			else
			{

				return RedirectToAction("Index");
			}





		}

	}
}