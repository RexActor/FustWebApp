﻿using Audit.Core;

using FustWebApp.Data;
using FustWebApp.Models;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System.Security.Claims;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize]
	[Area("Admin")]
	public class LoadController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public LoadController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}
		public async Task<IActionResult> Index()
		{
			
			List<Loads> viewModel = await applicationDbContext.Loads.Include(item => item.LoadSupplier).ThenInclude(item => item.Currency).OrderBy(item => item.LoadType).ThenByDescending(item => item.LoadDate).ToListAsync();
			



			return View(viewModel);
		}
		[HttpGet]
		public IActionResult ViewLoads(string date)
		{
		

			List<Loads> viewModel = applicationDbContext.Loads.AsEnumerable().OrderBy(item => item.LoadType).ThenByDescending(item => item.LoadDate).Where(item => item.LoadDate.ToShortDateString().Equals(date)).ToList();

			return View(viewModel);
		}


		[Authorize(Roles = "Admin,Fust")]
		[HttpGet]
		public async Task<IActionResult> AddLoad()
		{
			List<string> SupplierList = new List<string>();

			await applicationDbContext.Suppliers.ForEachAsync(item =>
			{
				SupplierList.Add(item.SupplierName);
			});

			ViewBag.Suppliers = SupplierList;

			return View();
		}

		[Authorize(Roles = "Admin,Fust")]
		[HttpPost]
		public IActionResult AddLoad(LoadBaseModel inboundLoad, string Next)
		{
			


			if (Next != null)
			{
				




				return RedirectToAction("CreateLoad", new { supplier = inboundLoad.LoadSupplier.SupplierName });
			}

			return View();
		}
		[HttpGet]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> CreateLoad(LoadBaseModel inboundLoad, string supplier, string date)
		{
			var supplierDetail = await applicationDbContext.Suppliers.Include(item => item.Currency).FirstOrDefaultAsync(item => item.SupplierName == supplier);

			if (supplierDetail != null)
			{
				inboundLoad.LoadSupplier = supplierDetail;
				inboundLoad.LoadDate = DateTime.Now;

				List<string> GroupsList = new List<string>();
				List<Fust> fustItems = new List<Fust>();
				List<FustType> fustTypeObjectList = new List<FustType>();
				List<string> fustTypesforSupplier = new List<string>();


				supplierDetail.FustTypeList.Split(',').ToList().ForEach(fustTypename =>
				{
					fustTypesforSupplier.Add(fustTypename);

				});

				fustTypesforSupplier.ForEach(fustTypeName =>
				{
					applicationDbContext.FustTypes.Where(item => item.FustTypeName == fustTypeName).ToList().ForEach(item =>
				   {
					   fustTypeObjectList.Add(item);
				   });
				});

				fustTypeObjectList.ForEach(fustType =>
				{

					applicationDbContext.Fusts.Where(item => item.FustType == fustType).ToList().ForEach(item =>
					{
						fustItems.Add(item);
					});
				});

				supplierDetail.SupplierGroup.Split(',').ToList().ForEach(item =>
				{
					GroupsList.Add(item);
				});

				var viewModel = new LoadViewModel()
				{
					LoadSupplier = inboundLoad.LoadSupplier,
					LoadOrigin = supplierDetail.SupplierOrigin,
					LoadDate = inboundLoad.LoadDate,
					InvoiceNumber= inboundLoad.InvoiceNumber,
					LoadFustItems = fustItems,
				};


				List<string> LoadTypes = new List<string>()
				{
					"Inbound",
					"Outbound"
				};

				ViewBag.TodaysDate = DateTime.Now;

				ViewBag.LoadType = LoadTypes;
				ViewBag.Groups = GroupsList;

				ViewBag.FustItems = fustItems;

				return View(viewModel);
			}
			else
			{
				return RedirectToAction("AddLoad");
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> CreateLoad(LoadBaseModel load, string Save)
		{

			List<LoadFusts> fustItems = new List<LoadFusts>();

			load.LoadFustItems.ForEach(fustItem =>
			{
				applicationDbContext.Fusts.Where(item => item.FustName == fustItem.FustName).ToList().ForEach(foundFust =>
			   {

				   var fustTypeFound = applicationDbContext.FustTypes.FirstOrDefault(item => item.FustTypeName == fustItem.FustType.FustTypeName);
				 


				   if (fustTypeFound != null)
				   {
					   fustItems.Add(new LoadFusts()
					   {
						   FustType = fustTypeFound,
						   Currency = load.LoadSupplier.Currency,
						   FustName = foundFust.FustName,
						   ReceivedQty = 0,
						   ExpectedQuantity = fustItem.ExpectedQuantity
					   });
				   }
			   });

			});

			if (Save != null)
			{
				int loadPo = 0;
				if (load.LoadType == "Inbound")
				{
					loadPo = load.PONumber;
				}

				var loadToAdd = new Loads()
				{
					LoadComment = load.LoadComment,
					LoadDate = load.LoadDate,
					LoadType = load.LoadType,
					LoadFustItems = fustItems,
					CreatedDate = DateTime.Now,
					LoadGroup = load.LoadGroup,
					InvoiceNumber = load.InvoiceNumber,
					LoadOrigin = load.LoadOrigin,
					PONumber = loadPo,
					LoadSupplier = load.LoadSupplier,
					LoadTrailerNumber = load.LoadTrailerNumber,
					CreatedBy = User.Identity.Name,
					ReceivedBy = "Not Received",
					UpdatedBy = "Not Updated"
				};

				applicationDbContext.Loads.Attach(loadToAdd);
				await applicationDbContext.Loads.AddAsync(loadToAdd);
				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

			}

			return RedirectToAction("Index");

		}

		[HttpGet]
		public async Task<IActionResult> ViewLoad(int Id) => View(await applicationDbContext.Loads.Where(item => item.LoadId == Id).Include(item => item.LoadSupplier).ThenInclude(item => item.Currency).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).FirstOrDefaultAsync());



		[HttpGet]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> EditLoad(int Id) => View(await applicationDbContext.Loads.Where(item => item.LoadId == Id).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).Include(item => item.LoadSupplier).ThenInclude(item => item.Currency).FirstOrDefaultAsync());



		[HttpPost]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> EditLoad(Loads load)
		{

			var loadToUpdate = applicationDbContext.Loads.Include(item => item.LoadFustItems).SingleOrDefault(item => item.LoadId == load.LoadId);



			List<LoadFusts> fustItems = new List<LoadFusts>();

			load.LoadFustItems.ForEach(fustItem =>
			{



				applicationDbContext.Fusts.Where(item => item.FustName == fustItem.FustName).ToList().ForEach(foundFust =>
				{
					var fustTypeFound = applicationDbContext.FustTypes.FirstOrDefault(item => item.FustTypeName == fustItem.FustType.FustTypeName);
					
					
					if (fustTypeFound != null)
					{
						fustItems.Add(new LoadFusts()
						{
							FustType = fustTypeFound,
							LoadFustId = fustItem.LoadFustId,
							Currency = load.LoadSupplier.Currency,
							Loads = load,
							FustName = foundFust.FustName,
							ReceivedQty = fustItem.ReceivedQty,
							ExpectedQuantity = fustItem.ExpectedQuantity,
							StorageQty = fustItem.StorageQty
						});
					}
				});

			});

			if (loadToUpdate != null)
			{

				int loadPo = 0;
				if (load.LoadType == "Inbound")
				{
					loadPo = load.PONumber;
				}
				loadToUpdate.InvoiceNumber = load.InvoiceNumber;
				loadToUpdate.LoadId = load.LoadId;
				loadToUpdate.PONumber = loadPo;
				loadToUpdate.LoadSupplier = load.LoadSupplier;
				loadToUpdate.LoadGroup = load.LoadGroup;
				loadToUpdate.LoadTrailerNumber = load.LoadTrailerNumber;
				loadToUpdate.CreatedDate = load.CreatedDate;
				loadToUpdate.LoadComment = load.LoadComment;
				loadToUpdate.LoadDate = load.LoadDate;
				loadToUpdate.LoadOrigin = load.LoadOrigin;
				loadToUpdate.UpdatedBy = User.Identity.Name ?? "Unknown User";
				loadToUpdate.LoadUpdatedDate = DateTime.Now;

				foreach (var fustItem in fustItems)
				{
					var selectedFustItem = loadToUpdate.LoadFustItems.FirstOrDefault(item => item.FustName == fustItem.FustName);

					if (selectedFustItem == null)
					{
						throw new ArgumentException($"Cannot find item with fustname {fustItem.FustName}");
					}
					int currentvalue = selectedFustItem.ReceivedQty;
					int currentStorage = selectedFustItem.StorageQty;


					var stockholdingToUpdate = await applicationDbContext.StockHolding.Where(item => item.StockHoldingSupplier.SupplierName == load.LoadSupplier.SupplierName).Include(item => item.StockHoldingFustItems).SingleOrDefaultAsync(item => item.StockHoldingFustItems.FustName == fustItem.FustName);

					if (stockholdingToUpdate == null)
					{
						throw new ArgumentException($"Cannot find item with Supplier name {load.LoadSupplier.SupplierName}");
					}


					if (load.LoadType == "Inbound")
					{
						if (fustItem.ReceivedQty > currentvalue)
						{
							stockholdingToUpdate.StockHoldingQty += (fustItem.ReceivedQty - currentvalue);
						}
						else
						{
							stockholdingToUpdate.StockHoldingQty += fustItem.ReceivedQty - currentvalue;
						}

						if (fustItem.StorageQty > currentStorage)
						{
							stockholdingToUpdate.StorageQuantity += (fustItem.StorageQty - currentStorage);
						}
						else
						{
							stockholdingToUpdate.StorageQuantity += fustItem.StorageQty - currentStorage;
						}


					}
					else
					{
						if (fustItem.ReceivedQty > currentvalue)
						{
							stockholdingToUpdate.StockHoldingQty -= (fustItem.ReceivedQty - currentvalue);
						}
						else
						{
							stockholdingToUpdate.StockHoldingQty -= fustItem.ReceivedQty - currentvalue;
						}

						if (fustItem.StorageQty > currentStorage)
						{
							stockholdingToUpdate.StorageQuantity -= (fustItem.StorageQty - currentStorage);
						}
						else
						{
							stockholdingToUpdate.StorageQuantity -= fustItem.StorageQty - currentStorage;
						}


					}
					stockholdingToUpdate.StockholdingDate = loadToUpdate.LoadUpdatedDate;
					applicationDbContext.StockHolding.Update(stockholdingToUpdate);

				}
				loadToUpdate.LoadFustItems = fustItems;
				applicationDbContext.Loads.Update(loadToUpdate);

				if (User == null)
				{
					throw new ArgumentException($"");
				}

				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

			}

			return RedirectToRoute(
				new
				{
					Controller = "Load",
					Action = "ViewLoad",
					Id = load.LoadId,
				});


		}


		[HttpGet]
		[Authorize(Roles = "Admin,Fust,Operations")]
		public async Task<IActionResult> ReceiveLoad(int Id) => View(await applicationDbContext.Loads.Where(item => item.LoadId == Id).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).Include(item => item.LoadSupplier).ThenInclude(item => item.Currency).FirstOrDefaultAsync());



		[HttpPost]
		[Authorize(Roles = "Admin,Fust,Operations")]
		public async Task<IActionResult> ReceiveLoad(Loads load)
		{

			var loadToUpdate = applicationDbContext.Loads.Include(item => item.LoadFustItems).SingleOrDefault(item => item.LoadId == load.LoadId);

			List<LoadFusts> fustItems = new List<LoadFusts>();

			load.LoadFustItems.ForEach(fustItem =>
			{


				applicationDbContext.Fusts.Where(item => item.FustName == fustItem.FustName).ToList().ForEach(foundFust =>
				{

					var fustTypeFound = applicationDbContext.FustTypes.FirstOrDefault(item => item.FustTypeName == fustItem.FustType.FustTypeName);


					if (fustTypeFound != null)
					{

						fustItems.Add(new LoadFusts()
						{

							FustType = fustTypeFound,
							LoadFustId = fustItem.LoadFustId,
							Loads = load,
							Currency = load.LoadSupplier.Currency,
							FustName = foundFust.FustName,
							ReceivedQty = fustItem.ReceivedQty,
							ExpectedQuantity = fustItem.ExpectedQuantity,
							StorageQty = fustItem.StorageQty
						});
					}
				});

			});


			if (loadToUpdate != null)
			{

				int loadPo = 0;
				if (load.LoadType == "Inbound")
				{
					loadPo = load.PONumber;
				}

				loadToUpdate.LoadId = load.LoadId;
				loadToUpdate.PONumber = loadPo;
				loadToUpdate.LoadSupplier = load.LoadSupplier;
				loadToUpdate.LoadGroup = load.LoadGroup;
				loadToUpdate.LoadTrailerNumber = load.LoadTrailerNumber;
				loadToUpdate.CreatedDate = load.CreatedDate;
				loadToUpdate.LoadComment = load.LoadComment;
				loadToUpdate.LoadDate = load.LoadDate;
				loadToUpdate.LoadOrigin = load.LoadOrigin;
				loadToUpdate.ReceivedBy = User.Identity.Name ?? "Unknown User";
				loadToUpdate.ReceivedDate = DateTime.Now;


				foreach (var fustItem in fustItems)
				{



					var selectedFustItem = loadToUpdate.LoadFustItems.FirstOrDefault(item => item.FustName == fustItem.FustName);

					if (selectedFustItem == null)
					{
						throw new ArgumentException($"Cannot find item with fustname {fustItem.FustName}");
					}
					int currentvalue = selectedFustItem.ReceivedQty;
					int currentStorage = selectedFustItem.StorageQty;


					var stockholdingToUpdate = await applicationDbContext.StockHolding.Where(item => item.StockHoldingSupplier.SupplierName == load.LoadSupplier.SupplierName).Include(item => item.StockHoldingFustItems).SingleOrDefaultAsync(item => item.StockHoldingFustItems.FustName == fustItem.FustName);

					if (stockholdingToUpdate == null)
					{
						throw new ArgumentException($"Cannot find item with Supplier name {load.LoadSupplier.SupplierName}");
					}


					if (load.LoadType == "Inbound")
					{

						if (fustItem.ReceivedQty > currentvalue)
						{
							stockholdingToUpdate.StockHoldingQty += (fustItem.ReceivedQty - currentvalue);
						}
						else
						{
							stockholdingToUpdate.StockHoldingQty += fustItem.ReceivedQty - currentvalue;
						}



						if (fustItem.StorageQty > currentStorage)
						{
							stockholdingToUpdate.StorageQuantity += (fustItem.StorageQty - currentStorage);
						}
						else
						{
							stockholdingToUpdate.StorageQuantity += (fustItem.StorageQty - currentStorage);
						}


					}
					else
					{
						if (fustItem.ReceivedQty > currentvalue)
						{
							stockholdingToUpdate.StockHoldingQty -= (fustItem.ReceivedQty - currentvalue);
						}
						else
						{
							stockholdingToUpdate.StockHoldingQty += currentvalue - fustItem.ReceivedQty;
						}
						if (fustItem.StorageQty > currentStorage)
						{
							stockholdingToUpdate.StorageQuantity -= (fustItem.StorageQty - currentStorage);
						}
						else
						{
							stockholdingToUpdate.StorageQuantity += currentStorage - fustItem.StorageQty;
						}
					}

					stockholdingToUpdate.StockholdingDate = loadToUpdate.ReceivedDate;

					applicationDbContext.StockHolding.Update(stockholdingToUpdate);

				}
				loadToUpdate.LoadFustItems = fustItems;

				applicationDbContext.Loads.Update(loadToUpdate);

				await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

			}



			return RedirectToRoute(
				new
				{
					Controller = "Load",
					Action = "ViewLoad",
					Id = load.LoadId,
				});


		}

	}
}
