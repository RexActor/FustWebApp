using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FustWebApp.Models.Domain;
using FustWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System.Security.Claims;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Authorize]
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class GroupController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;

		public GroupController(ApplicationDbContext applicationDbContext)
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
						ViewBag.Message = $"{TempData["Action"]} Successfully";
						ViewBag.Style = "alert-success";
						break;
					case "Fail":
						ViewBag.Message = $"{TempData["Action"]} Failed. {TempData["reason"]} Please try again!";
						ViewBag.Style = "alert-danger";
						break;
				}
			}

			var groupsToDisplay = await applicationDbContext.Groups.ToListAsync();
			return View(groupsToDisplay);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid Id) => View(await applicationDbContext.Groups.SingleOrDefaultAsync(item => item.Id == Id));


		[HttpPost]
		public async Task<IActionResult> View(Group group, string Delete, string Edit)
		{

			if (Delete != null)
			{
				var groupFound = applicationDbContext.Suppliers.FirstOrDefault(item => item.SupplierGroup == group.GroupName);
				
					if (groupFound != null)
					{
						TempData["result"] = "Fail";
						TempData["reason"] = "Group is being used";
						return RedirectToAction("Index");
					}

					var itemToDelete = new Group() { Id = group.Id };
					applicationDbContext.Groups.Attach(itemToDelete);
					applicationDbContext.Groups.Remove(itemToDelete);
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					TempData["result"] = "Success";
					TempData["action"] = "Delete";
					return RedirectToAction("Index");
				
			}

			if (Edit != null)
			{

				var itemToUpdate = await applicationDbContext.Groups.FirstOrDefaultAsync(item => item.Id == group.Id);
				

					if (itemToUpdate != null)
					{
						itemToUpdate.GroupName = group.GroupName;
						itemToUpdate.GroupComment = group.GroupComment;

						applicationDbContext.Groups.Update(itemToUpdate);
						await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					}
					TempData["result"] = "Success";
					TempData["action"] = "Update";
					return RedirectToAction("Index");
				

			}
			TempData["result"] = "Fail";
			TempData["action"] = "Update";

			return RedirectToAction("Index");

		}


		[HttpGet]
		public IActionResult Add() => View();


		[HttpPost]
		public async Task<IActionResult> Add(Group group, string Save)
		{
			
				if (Save != null)
				{
					var groupToadd = new Group()
					{
						Id = Guid.NewGuid(),
						GroupComment = group.GroupComment,
						GroupName = group.GroupName,
					};

					await applicationDbContext.Groups.AddAsync(groupToadd);
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

					TempData["result"] = "Success";
					TempData["action"] = "Upload";
					return RedirectToAction("Index");
				}
				else
				{
					TempData["result"] = "Fail";
					TempData["action"] = "Upload";
					return RedirectToAction("Add");
				}
			
		}
	}
}
