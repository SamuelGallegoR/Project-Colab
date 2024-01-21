using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;



namespace Lab1.Controllers //Controller does CRUD
{ 
    public class ProjectController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var projects = new List<Project>()
            {
                new Project {projectID = 1, Name = "Project 1", Description = "My first project"}

            };

               return View(projects);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = new Project { projectID = id, Name = "Project " + id, Description = "Description of project " + id };
            return View(project);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            return RedirectToAction("Index");
        }


    }
}

