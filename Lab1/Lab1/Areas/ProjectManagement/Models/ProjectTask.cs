using System;
using System.ComponentModel.DataAnnotations;

namespace Lab1.Areas.ProjectManagement.Models
{
	public class ProjectTask
	{

		[Key]
		public int ProjectTaskID { get; set; }

		[Required]
		public string? Title { get; set; }

		[Required]
		public string? Description { get; set; }

		//Foreign key from project, make sure name matches the name of PK in projects
		public int ProjectID { get; set; }

		//Navigation property
		//This property allows for easy acess to the related Project entity from the Task entity
		public Project? Project { get; set; }

	}
}

