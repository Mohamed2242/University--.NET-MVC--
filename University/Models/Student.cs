
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace University.Models
{
	public class Student
    {
		public int Id { get; set; }


        //[Display(Name= ("Name")]
        [DisplayName("Name")]
		[Required(ErrorMessage = "You have to provide a valid Full Name.")]
		[MinLength(10, ErrorMessage = "Full name mustn't be less than 10 characters.")]
		[MaxLength(70, ErrorMessage = "Full name mustn't exceepd 70 characters.")]
		public string FullName { get; set; }

		//[Display(Name="Current Year")]
		[DisplayName("Current Year")]
		[Range(1,4, ErrorMessage = "Current Year mustn't be less than 1 or more than 4.")]
		[Required(ErrorMessage = "You have to provide a valid Year number.")]
		public int YearNumber { get; set; }

		[DisplayName("CGPA")]
        [Range(0.0, 4.0, ErrorMessage = "CGPA mustn't be less than 0 or more than 4.")]
        [Required(ErrorMessage = "You have to provide a valid CGPA.")]
		public decimal CGPA { get; set; }

		[DisplayName("Date of Birth")]
		public DateTime BirthDate { get; set; }

		[DisplayName("Date of Join")]
		public DateTime JoinDate { get; set; }

		[DisplayName("Image")]
		[ValidateNever]
		public string ImageUrl { get; set; }

		//Foreign Key Property
		[Range(1, int.MaxValue, ErrorMessage = "Choose a valid department.")]
		[DisplayName("Department")]
		public int DepartmentId { get; set; }

		//Navigation Property
		[ValidateNever]
		public Department Department { get; set; }
	}
}

