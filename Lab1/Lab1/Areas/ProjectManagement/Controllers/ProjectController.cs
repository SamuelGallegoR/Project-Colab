using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Lab1.Areas.ProjectManagement.Models//Controller does CRUD
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            this._context = context;
        }


        //List all projects
        [HttpGet("")]
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


        //list a specific project using an ID
        [HttpGet("Details/{id:int}")]
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


        //Create Get, response to user request to get the form to create a project
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        //Actually insert a new project in the DataBase
        [HttpPost("Create")]
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


        //Getting form to Edit
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }


        //Editing the project itself
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectID, Name, Description, StartDate, EndDate, Status")] Project project)
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
                catch (DbUpdateConcurrencyException) {
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


        //Get the delete page
        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id) {

            var project = _context.Projects.FirstOrDefault(p => p.ProjectID == id);
            if (project == null) {
                return NotFound();
            }
            return View(project);

        }


        //Submitting the actual delete
        [HttpPost("DeleteConfirmed/{id:int}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString) {
            var projectQuery = from p in _context.Projects select p;

            bool searchPerformed = !String.IsNullOrEmpty(searchString);

            if (searchPerformed) {
                projectQuery = projectQuery.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));

            }
            var projects = await projectQuery.ToListAsync();

            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;

            return View("Index", projects);
        }
        
    }
}

