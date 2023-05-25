using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using FustWebApp.Data;
using FustWebApp.Models.Domain;
using Microsoft.EntityFrameworkCore;
using FustWebApp.Models;
using System.Globalization;
using System.Dynamic;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]

	public class StatisticsController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public StatisticsController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		public IActionResult Index()
		{
			List<Models.StatisticsDataPoint> loadsStats = new List<Models.StatisticsDataPoint>();

				var loads = applicationDbContext.Loads.Select(load => load.LoadType).Distinct().ToList();

				foreach (var item in loads)
				{
					int loadCount = applicationDbContext.Loads.Where(load => load.LoadType == item).ToList().Count;
					loadsStats.Add(new Models.StatisticsDataPoint
					{
						loadType = item,
						loadCount = loadCount,
						

					});
				}


			ViewBag.LoadsStats = loadsStats;


			

			List<Models.SupplierStatistics> supplierLoads = new List<Models.SupplierStatistics>();
			var suppliers = applicationDbContext.Suppliers.Select(supplier=>supplier.SupplierName).Distinct().ToList();


			foreach (var item in suppliers)
			{
				int loadCount = applicationDbContext.Loads.Where(load=>load.LoadSupplier.SupplierName==item).ToList().Count;
				supplierLoads.Add(new Models.SupplierStatistics
				{
					SupplierName=item,
					loadCount = loadCount,
				});
			}

			ViewData["SuppliersList"] = supplierLoads;


			List<Models.StatisticsDataPoint> countryList=new List<Models.StatisticsDataPoint>();

			var countries = applicationDbContext.Origins.ToList();

			foreach (var item in countries)
			{
				int supplierCount = applicationDbContext.Suppliers.Where(supplier => supplier.SupplierOrigin == item.OriginName).Count();
				countryList.Add(new StatisticsDataPoint
				{
					loadType = item.OriginName,
					loadCount = supplierCount
				});
			}


			ViewBag.CountryList = countryList;



			return View();
		}



	}
}
