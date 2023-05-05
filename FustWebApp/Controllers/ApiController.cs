using FustWebApp.Data;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol;

namespace FustWebApp.Controllers
{
	[Route("api/[action]")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		private readonly ApplicationDbContext applicationDbContext;

		public ApiController(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		// GET api/<action> 
		public IEnumerable<Supplier> GetSuppliers()
		{
			List<Supplier> suppliers = new List<Supplier>();
			suppliers = applicationDbContext.Suppliers.ToList();
			return suppliers;
		}


		public IEnumerable<FustType> GetFustTypes()
		{
			List<FustType> fustTypes = new List<FustType>();
			fustTypes = applicationDbContext.FustTypes.ToList();
			return fustTypes;
		}


		[HttpGet("{loadId:int}")]
		public IEnumerable<Loads> GetLoad(int loadId)
		{
			List<Loads> load = new List<Loads>();

			if (loadId != null)
			{

				load.Add(applicationDbContext.Loads.Where(item => item.LoadId == loadId).Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).FirstOrDefault());
			}
			else
			{
				applicationDbContext.Loads.ForEachAsync(item =>
				{
					load.Add(item);
				});
			}
			return load;
		}
	}
}
