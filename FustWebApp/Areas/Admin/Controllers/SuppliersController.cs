using Audit.Core;

using FustWebApp.Data;
using FustWebApp.Models;
using FustWebApp.Models.Domain;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize]
	[Area("Admin")]

	public class SuppliersController : Controller
	{

		private readonly ApplicationDbContext applicationDbContext;

		[BindProperty]
		public List<string> FustTypeChecked { get; set; }

		[BindProperty]
		public List<CheckBoxModel> FustTypeCheckBoxModel { get; set; } = new List<CheckBoxModel>();

		[BindProperty]
		public List<string> GroupChecked { get; set; }

		[BindProperty]
		public List<CheckBoxModel> GroupCheckBoxModel { get; set; } = new List<CheckBoxModel>();

		public SuppliersController(ApplicationDbContext applicationDbContext)
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
						ViewBag.Message = $"{TempData["Action"]} Successfully";
						ViewBag.Style = "alert-success";
						break;
					case "Fail":
						ViewBag.Message = $"{TempData["Action"]} Failed. Please try again!";
						ViewBag.Style = "alert-danger";
						break;
				}
			}

			var suppliers = await applicationDbContext.Suppliers.ToListAsync();
			return View(suppliers);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateSupplier()
		{
			int fustTypes = 0;
			await applicationDbContext.FustTypes.ForEachAsync(item =>
			{
				FustTypeCheckBoxModel.Add(
						new CheckBoxModel()
						{
							Id = fustTypes + 1,
							CheckBoxName = item.FustTypeName,
							IsChecked = false
						});

			});

			ViewBag.FustTypeCheckboxes = FustTypeCheckBoxModel;

			List<string> originList = new();
			await applicationDbContext.Origins.ForEachAsync(item => { originList.Add(item.OriginName); });

			ViewBag.Origins = originList;

			int groups = 0;
			await applicationDbContext.Groups.ForEachAsync(item =>
			{
				GroupCheckBoxModel.Add(new CheckBoxModel()
				{
					Id = groups + 1,
					CheckBoxName = item.GroupName,
					IsChecked = false
				});
			});

			List<string> CurrencyList = new List<string>();
			await applicationDbContext.Currency.ForEachAsync(item => { CurrencyList.Add(item.currencyAbrevation); });

			ViewBag.Currencies = CurrencyList;

			ViewBag.GroupsCheckBoxes = GroupCheckBoxModel;
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add(Supplier addSupplierRequest, string Save, List<string> FustTypeChecked, List<string> GroupChecked)
		{

			if (Save != null)
			{

				string fustTypeList = string.Join(",", FustTypeChecked);
				string groupList = string.Join(",", GroupChecked);

				var currency = await applicationDbContext.Currency.FirstOrDefaultAsync(item => item.currencyAbrevation == addSupplierRequest.Currency.currencyAbrevation);

				var supplier = new Supplier()
				{
					Id = Guid.NewGuid(),
					SupplierName = addSupplierRequest.SupplierName,
					SupplierAddress = addSupplierRequest.SupplierAddress,
					FustTypeList = fustTypeList,
					Currency = currency,
					SupplierGroup = groupList,
					SupplierOrigin = addSupplierRequest.SupplierOrigin
				};

				List<StockHolding> stockHoldingList = new List<StockHolding>();

				await applicationDbContext.Fusts.ForEachAsync(item =>
				{
					var stockholding = new StockHolding()
					{
						StockholdingDate = DateTime.Now,
						StockHoldingQty = 0,
						StockHoldingSupplier = supplier,
						baseValue = 0,
						StorageQuantity = 0,
						StockHoldingFustItems = item
					};

					stockHoldingList.Add(stockholding);
				});

				await applicationDbContext.AddRangeAsync(stockHoldingList);
				await applicationDbContext.Suppliers.AddAsync(supplier);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

				TempData["result"] = "Success";
				TempData["action"] = "Supplier Creation";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["result"] = "Fail";
				TempData["action"] = "Supplier Creation";
				return RedirectToAction("CreateSupplier");
			}


		}

		[Authorize(Roles = "Fust,Admin")]
		[HttpGet]
		public async Task<IActionResult> View(Guid Id, string editSupplier)
		{


			List<Loads> supplierLoadList = new List<Loads>();
			var supplier = await applicationDbContext.Suppliers.Include(item => item.Currency).FirstOrDefaultAsync(x => x.Id == Id);

			await applicationDbContext.Loads.Where(load => load.LoadSupplier == supplier.SupplierName).ForEachAsync(load =>
			{
				supplierLoadList.Add(load);
			});


			if (editSupplier != null && !User.IsInRole("Admin"))
			{
				return RedirectToRoute(
					new
					{
						Controller = "Suppliers",
						Action = "View",
						Id = supplier.Id,
					});
			}


			if (supplier != null)
			{
				var currency = await applicationDbContext.Currency.FirstOrDefaultAsync(item => item.currencyAbrevation == supplier.Currency.currencyAbrevation);
				var viewModel = new Supplier()
				{
					Id = supplier.Id,
					SupplierAddress = supplier.SupplierAddress,
					FustTypeList = supplier.FustTypeList,
					SupplierGroup = supplier.SupplierGroup,
					Currency = currency,
					
					SupplierName = supplier.SupplierName,
					SupplierOrigin = supplier.SupplierOrigin
				};

				int fustTypes = 0;
				await applicationDbContext.FustTypes.ForEachAsync(item =>
				{
					FustTypeCheckBoxModel.Add(
							new CheckBoxModel()
							{
								Id = fustTypes + 1,
								CheckBoxName = item.FustTypeName,
								IsChecked = false
							});
				});

				ViewBag.FustTypeCheckboxes = FustTypeCheckBoxModel;

				List<string> originList = new();
				await applicationDbContext.Origins.ForEachAsync(item => { originList.Add(item.OriginName); });

				ViewBag.Origins = originList;


				List<string> CurrencyList = new List<string>();
				await applicationDbContext.Currency.ForEachAsync(item => { CurrencyList.Add(item.currencyAbrevation); });

				ViewBag.Currencies = CurrencyList;


				int groups = 0;
				await applicationDbContext.Groups.ForEachAsync(item =>
				{
					GroupCheckBoxModel.Add(new CheckBoxModel()
					{
						Id = groups + 1,
						CheckBoxName = item.GroupName,
						IsChecked = false
					});
				});

				ViewBag.GroupsCheckBoxes = GroupCheckBoxModel;
				ViewBag.SupplierName = viewModel.SupplierName;
				List<StockHolding> stockHoldingList = new List<StockHolding>();

				await applicationDbContext.StockHolding.Where(item => item.StockHoldingSupplier == supplier).Include(item => item.StockHoldingFustItems).ForEachAsync(item =>
				{
					int outboundQuantity = applicationDbContext.LoadFusts.Where(loadfust => loadfust.FustName == item.StockHoldingFustItems.FustName).Where(loadfust => loadfust.Loads.LoadType == "Outbound").Where(loadfust => loadfust.Loads.LoadSupplier == item.StockHoldingSupplier.SupplierName).Sum(loadfust => loadfust.ReceivedQty);
					int inboundQuantity = applicationDbContext.LoadFusts.Where(loadfust => loadfust.FustName == item.StockHoldingFustItems.FustName).Where(loadfust => loadfust.Loads.LoadType == "Inbound").Where(loadfust => loadfust.Loads.LoadSupplier == item.StockHoldingSupplier.SupplierName).Sum(loadfust => loadfust.ReceivedQty);

					int stockHolding = -outboundQuantity + inboundQuantity;

					stockHoldingList.Add(new StockHolding()
					{

						StockholdingDate = item.StockholdingDate,
						StockHoldingFustItems = item.StockHoldingFustItems,
						StockHoldingQty = stockHolding,
						baseValue = item.baseValue,
						StorageQuantity=item.StorageQuantity,
						StockHoldingSupplier = item.StockHoldingSupplier,
						StockHoldingId = item.StockHoldingId
					});
				});

				ViewBag.StockHoldingList = stockHoldingList;
				ViewBag.editSupplier = editSupplier;
				ViewBag.SupplierLoads = supplierLoadList;
				return View(viewModel);
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(Supplier updateSupplierViewModel, string Save, string Delete, List<string> FustTypeChecked, List<string> GroupChecked)
		{


			if (Delete != null)
			{
				var supplierToRemvoe = new Supplier()
				{
					Id = updateSupplierViewModel.Id,
				};

				applicationDbContext.Update(supplierToRemvoe);
				applicationDbContext.Suppliers.Attach(supplierToRemvoe);

				applicationDbContext.Suppliers.Remove(supplierToRemvoe);

				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
				TempData["result"] = "Success";
				TempData["action"] = "Supplier Deletion";

			}
			if (Save != null)
			{

				var supplierToUpdate = applicationDbContext.Suppliers.Include(item => item.Currency).SingleOrDefault(item => item.Id == updateSupplierViewModel.Id);

				string fustTypeList = string.Join(",", FustTypeChecked);
				string groupList = string.Join(",", GroupChecked);
				var currency = await applicationDbContext.Currency.FirstOrDefaultAsync(item => item.currencyAbrevation == updateSupplierViewModel.Currency.currencyAbrevation);
				if (supplierToUpdate != null)
				{
					if (applicationDbContext.StockHolding.Where(item => item.StockHoldingSupplier == supplierToUpdate).Count() == 0)
					{

						List<StockHolding> stockHoldingList = new List<StockHolding>();
						await applicationDbContext.Fusts.ForEachAsync(item =>
						{
							var stockholding = new StockHolding()
							{
								StockholdingDate = DateTime.Now,
								StockHoldingQty = 0,
								StockHoldingSupplier = supplierToUpdate,
								StockHoldingFustItems = item
							};

							stockHoldingList.Add(stockholding);
						});
						await applicationDbContext.AddRangeAsync(stockHoldingList);
					}
					supplierToUpdate.Id = updateSupplierViewModel.Id;
					supplierToUpdate.Currency = currency;
					supplierToUpdate.SupplierGroup = groupList;
					supplierToUpdate.SupplierName = updateSupplierViewModel.SupplierName;
					supplierToUpdate.SupplierOrigin = updateSupplierViewModel.SupplierOrigin;
					supplierToUpdate.SupplierAddress = updateSupplierViewModel.SupplierAddress;
					supplierToUpdate.FustTypeList = fustTypeList;

					applicationDbContext.Suppliers.Update(supplierToUpdate);

					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

				}
				TempData["result"] = "Success";
				TempData["action"] = "Supplier Update";


			}
			return RedirectToAction("Index");
		}
	}



}