using Microsoft.EntityFrameworkCore;
using University.Models;

namespace University.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

        public DbSet<Department> Departments { get; set; }
		public DbSet<Student> Students { get; set; }

	}
}
