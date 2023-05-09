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
	public class LoadController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public LoadController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}
		public async Task<IActionResult> Index()
		{
			List<Loads> viewModel = new List<Loads>();
			viewModel = await applicationDbContext.Loads.OrderBy(item => item.LoadType).ThenByDescending(item => item.LoadDate).ToListAsync();
			List<LoadFusts> fustItemsList = new List<LoadFusts>();

			foreach (var item in viewModel)
			{

				await applicationDbContext.LoadFusts.Where(loadFust => loadFust.Loads.LoadId == item.LoadId).ForEachAsync(loadFust =>
				{
					fustItemsList.Add(new LoadFusts()
					{
						FustName = loadFust.FustName,
						Loads = loadFust.Loads,
						FustType = loadFust.FustType,
						LoadFustId = loadFust.LoadFustId,
						ExpectedQuantity = loadFust.ExpectedQuantity,
					});

				});

			}

			ViewBag.FustItems = fustItemsList;
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
				return RedirectToAction("CreateLoad", inboundLoad);
			}

			return View();
		}
		[HttpGet]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> CreateLoad(LoadBaseModel inboundLoad)
		{
			var supplierDetail = await applicationDbContext.Suppliers.FirstOrDefaultAsync(item => item.SupplierName == inboundLoad.LoadSupplier);

			if (supplierDetail != null)
			{
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
					LoadOrigin = load.LoadOrigin,
					PONumber = loadPo,
					LoadSupplier = load.LoadSupplier,
					LoadTrailerNumber = load.LoadTrailerNumber,
					CreatedBy = User.Identity.Name,
					ReceivedBy = "Not Received",
					UpdatedBy = "Not Updated"
				};

				using (AuditScope.Create("Add:Load", () => loadToAdd))
				{
					await applicationDbContext.Loads.AddAsync(loadToAdd);
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
				}
			}

			return RedirectToAction("Index");

		}

		[HttpGet]
		public async Task<IActionResult> ViewLoad(int Id) => View(await applicationDbContext.Loads.Where(item => item.LoadId == Id).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).FirstOrDefaultAsync());



		[HttpGet]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> EditLoad(int Id) => View(await applicationDbContext.Loads.Where(item => item.LoadId == Id).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).FirstOrDefaultAsync());



		[HttpPost]
		[Authorize(Roles = "Admin,Fust")]
		public async Task<IActionResult> EditLoad(Loads load)
		{

			var loadToUpdate = applicationDbContext.Loads.Include(item => item.LoadFustItems).SingleOrDefault(item => item.LoadId == load.LoadId);
			using (AuditScope.Create("Update:Load", () => loadToUpdate))
			{
				List<LoadFusts> fustItems = new List<LoadFusts>();

				load.LoadFustItems.ForEach(fustItem =>
				{
					applicationDbContext.Fusts.Where(item => item.FustName == fustItem.FustName).ToList().ForEach(foundFust =>
					{
						var fustTypeFound = applicationDbContext.FustTypes.FirstOrDefault(item => item.FustTypeName == fustItem.FustType.FustTypeName);
						var loadFound = applicationDbContext.Loads.FirstOrDefault(item => item.LoadId == load.LoadId);
						if (fustTypeFound != null)
						{
							fustItems.Add(new LoadFusts()
							{
								FustType = fustTypeFound,
								LoadFustId = fustItem.LoadFustId,
								Loads = load,
								FustName = foundFust.FustName,
								ReceivedQty = fustItem.ReceivedQty,
								ExpectedQuantity = fustItem.ExpectedQuantity
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
					loadToUpdate.UpdatedBy = User.Identity.Name ?? "Unknown User";
					loadToUpdate.LoadUpdatedDate = DateTime.Now;

					foreach (var fustItem in fustItems)
					{
						int currentvalue = loadToUpdate.LoadFustItems.Where(item => item.FustName == fustItem.FustName).FirstOrDefault().ReceivedQty;

						var stockholdingToUpdate = await applicationDbContext.StockHolding.Where(item => item.StockHoldingSupplier.SupplierName == load.LoadSupplier).Include(item => item.StockHoldingFustItems).SingleOrDefaultAsync(item => item.StockHoldingFustItems.FustName == fustItem.FustName);

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
						}
						stockholdingToUpdate.StockholdingDate = loadToUpdate.LoadUpdatedDate;
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


		[HttpGet]
		[Authorize(Roles = "Admin,Fust,Operations")]
		public async Task<IActionResult> ReceiveLoad(int Id) => View(await applicationDbContext.Loads.Where(item => item.LoadId == Id).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).FirstOrDefaultAsync());



		[HttpPost]
		[Authorize(Roles = "Admin,Fust,Operations")]
		public async Task<IActionResult> ReceiveLoad(Loads load)
		{

			var loadToUpdate = applicationDbContext.Loads.Include(item => item.LoadFustItems).SingleOrDefault(item => item.LoadId == load.LoadId);
			using (AuditScope.Create("Receive:Load", () => loadToUpdate))
			{
				List<LoadFusts> fustItems = new List<LoadFusts>();

				load.LoadFustItems.ForEach(fustItem =>
				{
					applicationDbContext.Fusts.Where(item => item.FustName == fustItem.FustName).ToList().ForEach(foundFust =>
					{

						var fustTypeFound = applicationDbContext.FustTypes.FirstOrDefault(item => item.FustTypeName == fustItem.FustType.FustTypeName);
						var loadFound = applicationDbContext.Loads.FirstOrDefault(item => item.LoadId == load.LoadId);
						if (fustTypeFound != null)
						{
							fustItems.Add(new LoadFusts()
							{

								FustType = fustTypeFound,
								LoadFustId = fustItem.LoadFustId,
								Loads = load,
								FustName = foundFust.FustName,
								ReceivedQty = fustItem.ReceivedQty,
								ExpectedQuantity = fustItem.ExpectedQuantity
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
						int currentvalue = loadToUpdate.LoadFustItems.Where(item => item.FustName == fustItem.FustName).FirstOrDefault().ReceivedQty;

						var stockholdingToUpdate = await applicationDbContext.StockHolding.Where(item => item.StockHoldingSupplier.SupplierName == load.LoadSupplier).Include(item => item.StockHoldingFustItems).SingleOrDefaultAsync(item => item.StockHoldingFustItems.FustName == fustItem.FustName);

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
}
