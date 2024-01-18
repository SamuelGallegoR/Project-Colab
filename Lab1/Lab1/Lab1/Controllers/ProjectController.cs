using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;



namespace Lab1.Controllers
{
    public class ProjectController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var projects = new List<Project>()
            {
                new Project {projectID = 1, Name = "Project 1", Description = "My first project"}

            };

               return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    } 
}

