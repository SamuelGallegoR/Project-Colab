using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Controllers
{
    public class TaskController : Controller
    {

        private readonly ApplicationDbContext _context;


        public TaskController(ApplicationDbContext context) {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index(int projectID)
        {
            var tasks = _context.ProjectTasks
                .Where(t => t.ProjectID == projectID)
                .ToList();

            ViewBag.ProjectID = projectID;
            return View(tasks);
        }


        [HttpGet]
        public IActionResult Details(int id) {
            var task = _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(task => task.ProjectID == id);

            if (task == null) {
                return NotFound();
            }

            return View(task);
        }


        [HttpGet]
        public IActionResult Create(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            if (project == null) {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectID = projectID
            };

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectID")] ProjectTask task)
        {
            if (ModelState.IsValid) {
                _context.ProjectTasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { ProjectID = task.ProjectID });
            }

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectID", "Name", task.ProjectID);

            return View(task);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskID == id);
            if (task == null) {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectID", "Name", task.ProjectID);
            return View(task);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskID", "Title", "Description", "ProjectID")] ProjectTask task)
        {
            if (id != task.ProjectTaskID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                _context.ProjectTasks.Update(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { ProjectID = task.ProjectID });

            }


            ViewBag.Projects = new SelectList(_context.Projects, "ProjectID", "Name", task.ProjectID);
            return View(task);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _context.ProjectTasks
                           .Include(t => t.Project)
                           .FirstOrDefault(t => t.ProjectTaskID == id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectID", "Name", task.ProjectID);
            return View(task);
        }



        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectTaskID)
        {
            var task = _context.ProjectTasks.Find(projectTaskID);

            if (task != null) {
                _context.ProjectTasks.Remove(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { ProjectID = task.ProjectID });
            }

            return NotFound();
        }

        public async Task<IActionResult> Search(int? projectID, string searchString) {

            var taskQuery = _context.ProjectTasks.AsQueryable();

            //If project ID was passed
            if (projectID.HasValue) {
                taskQuery = taskQuery.Where(t => t.ProjectID == projectID.Value);
            }
            if (!string.IsNullOrEmpty(searchString)) {
                taskQuery = taskQuery.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
            }

            var tasks = await taskQuery.ToListAsync();
            ViewBag.Tasks = tasks;
            return View("Index", tasks);

        }



    }
}

