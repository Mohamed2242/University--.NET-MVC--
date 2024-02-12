using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace University.Models
{
	public class Department
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		//Navigation Property
		[ValidateNever]
		public List<Student> Students { get; set; }

    }
}
