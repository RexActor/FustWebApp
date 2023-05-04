using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FustWebApp.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using FustWebApp.Data;

using Audit.Core;

using System.Security.Claims;

namespace FustWebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{

		private RoleManager<IdentityRole> roleManager;
		private UserManager<IdentityUser> userManager;
		private readonly ApplicationDbContext applicationDbContext;
		private IPasswordHasher<IdentityUser> _passwordHasher;

		public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr, ApplicationDbContext applicationDbContext, IPasswordHasher<IdentityUser> passwordHasher)
		{
			roleManager = roleMgr;
			userManager = userMgr;
			this.applicationDbContext = applicationDbContext;
			_passwordHasher = passwordHasher;
		}
		public IActionResult Index()
		{
			ViewBag.Users = userManager.Users;
			return View(roleManager.Roles);
		}
		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{
			IdentityRole role = await roleManager.FindByIdAsync(id);
			List<IdentityUser> members = new List<IdentityUser>();
			List<IdentityUser> nonMembers = new List<IdentityUser>();
			foreach (IdentityUser user in userManager.Users)
			{
				var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
				list.Add(user);
			}
			return View(new RoleEdit
			{
				Members = members,
				NonMembers = nonMembers,
				Role = role
			});

		}

		[HttpPost]
		public async Task<IActionResult> Update(RoleModification model)
		{
			using (AuditScope.Create("Update:Role", () => model))
			{
				IdentityResult result;
				if (ModelState.IsValid)
				{
					foreach (string userId in model.AddIds ?? new string[] { })
					{
						IdentityUser user = await userManager.FindByIdAsync(userId);
						if (user != null)
						{
							result = await userManager.AddToRoleAsync(user, model.RoleName);
							if (!result.Succeeded)
							{
								Errors(result);
							}
						}
					}

					foreach (string userId in model.DeleteIds ?? new string[] { })
					{
						IdentityUser user = await userManager.FindByIdAsync(userId);
						if (user != null)
						{
							result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
							if (!result.Succeeded)
							{
								Errors(result);
							}
						}
					}
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

				}
				if (ModelState.IsValid)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return await Update(model.RoleId);
				}
			}
		}

		[HttpGet]
		public IActionResult Create() => View();

		[HttpGet]
		public IActionResult CreateUser() => View();


		[HttpPost]
		public async Task<IActionResult> CreateUser([Required] string email, [Required] string password)
		{
			if (ModelState.IsValid)
			{
				IdentityUser userToCreate = new IdentityUser();
				using (AuditScope.Create("Create:User", () => userToCreate))
				{
					Guid guid = Guid.NewGuid();
					userToCreate.Id = guid.ToString();
					userToCreate.UserName = email;
					userToCreate.Email = email;
					userToCreate.NormalizedUserName = email;
					userToCreate.EmailConfirmed = true;
					userToCreate.NormalizedEmail = email;
					userToCreate.LockoutEnabled = true;
					applicationDbContext.Users.Add(userToCreate);

					var hashedPassword = _passwordHasher.HashPassword(userToCreate, password);
					userToCreate.SecurityStamp = Guid.NewGuid().ToString();
					userToCreate.PasswordHash = hashedPassword;

					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					return RedirectToAction("Index");
				}

			}
			else
			{
				return View();
			}

		}

		private void Errors(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create([Required] string name)
		{
			using (AuditScope.Create("Create:Role", () => name))
			{
				if (ModelState.IsValid)
				{
					IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));

					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						Errors(result);
					}
				}
				return View();
			}
		}

		public async Task<IActionResult> Delete(string id)
		{

			IdentityRole role = await roleManager.FindByIdAsync(id);
			using (AuditScope.Create("Delete:Role", () => role))
			{
				if (role != null)
				{
					IdentityResult result = await roleManager.DeleteAsync(role);
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						Errors(result);
					}

				}
				else
				{
					ModelState.AddModelError("", "No role found");

				}
				return View("Index", roleManager.Roles);
			}
		}

		public async Task<ActionResult> DeleteUser(bool confirm, string id)
		{

			IdentityUser user = await userManager.FindByIdAsync(id);

			using (AuditScope.Create("Delete:User", () => user))
			{
				if (user != null && confirm)
				{
					IdentityResult result = await userManager.DeleteAsync(user);

					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						Errors(result);
					}

				}
				else
				{
					ModelState.AddModelError("", "No role found");

				}
				return View("Index", userManager.Users);
			}
		}


		public async Task<IActionResult> UpdateUser(string id)
		{
			IdentityUser user = await userManager.FindByIdAsync(id);


			return View(user);

		}


		[HttpPost]
		public async Task<IActionResult> UpdateUser(string id, string email, string password)
		{
			IdentityResult result;
			IdentityUser user = await userManager.FindByIdAsync(id);

			using (AuditScope.Create("Update:User", () => user))
			{
				if (ModelState["password"] != null) ModelState["password"].Errors.Clear();

				if (ModelState.IsValid)
				{




					user.Email = email;
					user.UserName = email;


					await userManager.UpdateAsync(user);

					if (password != null)
					{
						var hashedPassword = _passwordHasher.HashPassword(user, password);
						user.SecurityStamp = Guid.NewGuid().ToString();
						user.PasswordHash = hashedPassword;
					}
					await applicationDbContext.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

				}
				if (ModelState.IsValid)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return await UpdateUser(user.Id);
				}

			}
		}


	}
}