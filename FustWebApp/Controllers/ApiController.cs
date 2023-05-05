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



		public IEnumerable<Loads> GetLoads()
		{
			List<Loads> load = new List<Loads>();



			load = applicationDbContext.Loads.Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).ToList();

			return load;
		}

		public IEnumerable<StockHolding> GetStockHolding()
		{
			List<StockHolding> stockHolding = new List<StockHolding>();
			stockHolding = applicationDbContext.StockHolding.Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).Include(item => item.StockHoldingSupplier).ToList();

			return stockHolding;
		}

	}
}
