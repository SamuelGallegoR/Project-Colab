using System;
using System.ComponentModel.DataAnnotations;

namespace Lab1.Areas.ProjectManagement.Models
{
	public class ProjectComment
	{

		[Key]
		public int ProjectCommentID { get; set; }

		[Required]
        [Display(Name = "Comment")]

        [StringLength(500, ErrorMessage = "Project Comment cannot exceed 500 characters")]
		public string? Content { get; set; }


		[Display(Name ="Posted Date")]
		[DisplayFormat(DataFormatString ="{0:yyy-MM-dd}", ApplyFormatInEditMode =true)]
		[DataType(DataType.Date)]
		public DateTime DatePosted { get; set; }


		//Foreign key
		public int ProjectID { get; set; }


		//Navigation Property
		public Project? Project { get; set; }


	}
}

