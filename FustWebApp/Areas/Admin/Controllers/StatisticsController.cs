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
			List<Models.DataPoint> loadsStats = new List<Models.DataPoint>();


			var loads = applicationDbContext.Loads.Select(item => item.LoadType).Distinct().ToList();



			foreach (var item in loads)
			{
				int loadCount = applicationDbContext.Loads.Where(load => load.LoadType == item).ToList().Count();

				loadsStats.Add(new Models.DataPoint
				{
					chartlabel = item,
					chartvalue= loadCount
				}) ;
			}





			ViewBag.LoadsStats = loadsStats;




			return View();
		}



	}
}
