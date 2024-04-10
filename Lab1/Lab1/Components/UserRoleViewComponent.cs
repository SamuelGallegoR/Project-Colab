using System;
using Lab1.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Components
{
	public class UserRoleViewComponent : ViewComponent
	{

		private readonly UserManager<ApplicationUser> _userManager;

		public UserRoleViewComponent(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			var model = new
			{
				IsSuperAdmin = user != null && await _userManager.IsInRoleAsync(user, "SuperAdmin"),
				IsAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin")
			};

			return View(model);
		}
    }
}

