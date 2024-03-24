using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Areas.ProjectManagement.Models
{
    [Area("ProjectManagement")]
    public class TaskController : Controller
    {

        private readonly ApplicationDbContext _context;


        public TaskController(ApplicationDbContext context) {
            _context = context;
        }


        [HttpGet("Index/{projectID:int?}")]
        public async Task<IActionResult> Index(int? projectID) // Note the change here to make projectID nullable
        {
            var taskQuery = _context.ProjectTasks.AsQueryable();

            if (projectID.HasValue)
            {
                taskQuery = taskQuery.Where(t => t.ProjectID == projectID.Value);
            }

            var tasks = await taskQuery.ToListAsync();
            ViewBag.ProjectID = projectID;
            return View(tasks);
        }



        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id) {

            var task = await _context.ProjectTasks
                            .Include(t => t.Project)
                            .FirstOrDefaultAsync(task => task.ProjectID == id);

            if (task == null) {
                return NotFound();
            }

            return View(task);
        }


        [HttpGet("Create/{projectID:int}")]
        public async Task<IActionResult> Create(int projectID)
        {
            var project = await _context.Projects.FindAsync(projectID);
            if (project == null) {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectID = projectID
            };

            return View(task);
        }

        [HttpPost("Create/{projectID:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectID")] ProjectTask task)
        {
            if (ModelState.IsValid) {
                await _context.ProjectTasks.AddAsync(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { ProjectID = task.ProjectID });
            }

            var projects = await _context.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(projects, "ProjectID", "Name", task.ProjectID);

            return View(task);
        }


        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.ProjectTasks
                            .Include(t => t.Project)
                            .FirstOrDefaultAsync(t => t.ProjectTaskID == id);
            if (task == null) {
                return NotFound();
            }

            var projects = await _context.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(projects, "ProjectID", "Name", task.ProjectID);
            return View(task);
        }



        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskID", "Title", "Description", "ProjectID")] ProjectTask task)
        {
            if (id != task.ProjectTaskID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                _context.ProjectTasks.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { ProjectID = task.ProjectID });

            }

            var projects = await _context.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(projects, "ProjectID", "Name", task.ProjectID);
            return View(task);
        }


        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ProjectTasks
                           .Include(t => t.Project)
                           .FirstOrDefaultAsync(t => t.ProjectTaskID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }



        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int projectTaskID)
        {
            var task = await _context.ProjectTasks.FindAsync(projectTaskID);

            if (task != null) {
                _context.ProjectTasks.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { projectID = task.ProjectID });
            }

            return NotFound();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(int? projectID, string searchString) {

            var taskQuery = _context.ProjectTasks.AsQueryable();
            bool searchPerformed = !String.IsNullOrEmpty(searchString);

            //If project ID was passed
            if (projectID.HasValue) {
                taskQuery = taskQuery.Where(t => t.ProjectID == projectID.Value);
            }

            if (!searchPerformed) {
                taskQuery = taskQuery.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
            }

            var tasks = await taskQuery.ToListAsync();

            //Depending on your UI, you might want to handle the ViewBag differently if no specific project is targeted
            ViewBag.ProjectID = projectID;
            ViewData["SearchedPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", tasks);

        }



    }
}

