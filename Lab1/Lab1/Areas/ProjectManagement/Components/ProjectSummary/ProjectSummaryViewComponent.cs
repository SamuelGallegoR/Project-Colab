using System;
using Lab1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Areas.ProjectManagement.Components.ProjectSummary
{
	public class ProjectSummaryViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;

		public ProjectSummaryViewComponent(ApplicationDbContext context) {
			_context = context;
		}

		//Async --> non blocking
		public async Task<IViewComponentResult> InvokeAsync(int projectID) {

			var project = await _context.Projects
				.Include(p => p.Task)
				.FirstOrDefaultAsync(p => p.ProjectID);

			if (project == null) {
				//Handle case when the project is not found, return html content
				return Content("Project Not Found");
			}

			return View(project);

		}

	}
}

