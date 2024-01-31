using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Lab1.Controllers //Controller does CRUD
{
    public class ProjectController : Controller
    {
        // GET: /<controller>/

        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            this._context = context;
        }



        [HttpGet]
        public IActionResult Index()
        {
            /*
            var projects = new List<Project>()
            {
                new Project {projectID = 1, Name = "Project 1", Description = "My first project"}

            };
            */

            var projects = _context.Projects.ToList();
            return View(projects);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            //var project = new Project { projectID = id, Name = "Project " + id, Description = "Description of project " + id };
            var project = _context.Projects.FirstOrDefault(p => p.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //DO THIS VALIDATE FOR EVERY POST
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid) //Check is the state is healthy or valid, Ex. If a project (in our model) has a name. it should, cause thats the model we wrote for out project.
            {
                _context.Projects.Add(project); //Saves project to memmory, its not in the db just yet
                _context.SaveChanges(); //"Commits the projects to the db
                return RedirectToAction("Index"); //Redirects to the method "Index()" above
            }
            return View(project);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectID, Name, Description")]Project project)
        {
            if (id != project.ProjectID) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                try
                {
                    _context.Update(project);
                    _context.SaveChanges();
                }
                catch(DbUpdateConcurrencyException) {
                    if (!ProjectExists(project.ProjectID))
                    {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(project);

        }

        public bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }


        [HttpGet]
        public IActionResult Delete(int id) {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectID == id);
            if (project == null) {
                return NotFound();
            }
            return View(project);

        }


    }
}

