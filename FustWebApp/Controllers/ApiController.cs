using FustWebApp.Data;
using FustWebApp.Models.Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Returns All Suppliers</returns>
		// GET api/<action> 
		[HttpGet]
		public IEnumerable<Supplier> GetSuppliers() => applicationDbContext.Suppliers.ToList();

		// GET api/<action> 
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Returns All Fust Items including fust Types</returns>
		// GET api/<action> 
		[HttpGet]
		public IEnumerable<Fust> GetFust() => applicationDbContext.Fusts.Include(item => item.FustType).ToList();


		// GET api/<action> 
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Returns All Loads</returns>
		// GET api/<action> 
		[HttpGet]
		public IEnumerable<Loads> GetLoads() => applicationDbContext.Loads.Include(item => item.LoadFustItems).ThenInclude(item => item.FustType).ToList();


		// GET api/<action> 
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Returns Stockholding List</returns>
		[HttpGet]
		public IEnumerable<StockHolding> GetStockHolding() => applicationDbContext.StockHolding.Include(item => item.StockHoldingFustItems).ThenInclude(item => item.FustType).Include(item => item.StockHoldingSupplier).ToList();

	}
}
