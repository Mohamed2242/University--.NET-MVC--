using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using University.Data;
using University.Models;

namespace University.Controllers
{
	public class StudentsController : Controller
	{
		ApplicationDbContext _context;
		IWebHostEnvironment _webHostEnvironment;

		public StudentsController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
		{
			_webHostEnvironment = webHostEnvironment;
			_context = context;
		}

		public IActionResult Index(string Name)
		{
			var students = from s in _context.Students
							select s;
			
            if (string.IsNullOrEmpty(Name))
            {
                return View(students.ToList());
            }
            else
            {
                students = students.Where(s => s.FullName.Contains(Name));
                return View(students.ToList());
            }
        }
		

		[HttpGet]
		public IActionResult GetIndexView()
		{
			return View("Index", _context.Students.ToList());
		}

		[HttpGet]
		public IActionResult GetDetailsView(int id)
		{
            Student student = _context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);

			return View("Details", student);
		}

		[HttpGet]
		public IActionResult GetCreateView()
		{
			ViewBag.DeptSelectItems = new SelectList(_context.Departments.ToList(), "Id", "Description");
			return View("Create");
		}

		[HttpPost]
		public IActionResult AddNew(Student stu, IFormFile? imageFormFile) // FolanAlfolani.png
		{
			// GUID -> Globally Unique Identifier
			if (imageFormFile != null)
			{
				string imgExtension = Path.GetExtension(imageFormFile.FileName); // .png
				Guid imgGuid = Guid.NewGuid(); // xm789-f07li-624yn-uvx98
				string imgName = imgGuid + imgExtension; // xm789-f07li-624yn-uvx98.png
				string imgUrl = "\\images\\" + imgName; //  \images\xm789-f07li-624yn-uvx98.png
                stu.ImageUrl = imgUrl;

				string imgPath = _webHostEnvironment.WebRootPath + imgUrl;

				// FileStream 
				FileStream imgStream = new FileStream(imgPath, FileMode.Create);
				imageFormFile.CopyTo(imgStream);
				imgStream.Dispose();
			}
			else
			{
                stu.ImageUrl = "\\images\\No_Image.png";
			}

            if (((stu.JoinDate - stu.BirthDate).Days / 365) < 17 || ((stu.JoinDate - stu.BirthDate).Days / 365) > 24)
            {
                ModelState.AddModelError(string.Empty, "Illegal Age (Under 17 years old) or (Over 24 years old).");
            }
			
            
            if (ModelState.IsValid == true)
			{
				_context.Students.Add(stu);
				_context.SaveChanges();
				return RedirectToAction("GetIndexView");
			}
			else
			{
				ViewBag.DeptSelectItems = new SelectList(_context.Departments.ToList(), "Id", "Description");
				return View("Create");
			}
		}


		[HttpGet]
		public IActionResult GetEditView(int id)
		{
            Student student = _context.Students.FirstOrDefault(s => s.Id == id);

			if (student == null)
			{
				return NotFound();
			}
			else
			{
				ViewBag.DeptSelectItems = new SelectList(_context.Departments.ToList(), "Id", "Description");
				return View("Edit", student);
			}
		}


		[HttpPost]
		public IActionResult EditCurrent(Student stu, IFormFile? imageFormFile)
		{
			// GUID -> Globally Unique Identifier
			if (imageFormFile != null)
			{
				if (stu.ImageUrl != "\\images\\No_Image.png")
				{
					string oldImgPath = _webHostEnvironment.WebRootPath + stu.ImageUrl;

					if (System.IO.File.Exists(oldImgPath) == true)
					{
						System.IO.File.Delete(oldImgPath);
					}
				}


				string imgExtension = Path.GetExtension(imageFormFile.FileName); // .png
				Guid imgGuid = Guid.NewGuid(); // xm789-f07li-624yn-uvx98
				string imgName = imgGuid + imgExtension; // xm789-f07li-624yn-uvx98.png
				string imgUrl = "\\images\\" + imgName; //  \images\xm789-f07li-624yn-uvx98.png
				stu.ImageUrl = imgUrl;

				string imgPath = _webHostEnvironment.WebRootPath + imgUrl;

				// FileStream 
				FileStream imgStream = new FileStream(imgPath, FileMode.Create);
				imageFormFile.CopyTo(imgStream);
				imgStream.Dispose();
			}


            if (((stu.JoinDate - stu.BirthDate).Days / 365) < 17 || ((stu.JoinDate - stu.BirthDate).Days / 365) > 24)
            {
                ModelState.AddModelError(string.Empty, "Illegal Age (Under 17 years old) or (Over 24 years old).");
            }

           

            if (ModelState.IsValid == true)
			{
				_context.Students.Update(stu);
				_context.SaveChanges();
				return RedirectToAction("GetIndexView");
			}
			else
			{
				ViewBag.DeptSelectItems = new SelectList(_context.Departments.ToList(), "Id", "Description");
				return View("Edit");
			}
		}


		[HttpGet]
		public IActionResult GetDeleteView(int id)
		{
            Student student = _context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);

			if (student == null)
			{
				return NotFound();
			}
			else
			{
				return View("Delete", student);
			}
		}


		[HttpPost]
		public IActionResult DeleteCurrent(int id)
		{
            Student student = _context.Students.FirstOrDefault(s => s.Id == id);
			if (student == null)
			{
				return NotFound();
			}
			else
			{
				if (student.ImageUrl != "\\images\\No_Image.png")
				{
					string imgPath = _webHostEnvironment.WebRootPath + student.ImageUrl;

					if (System.IO.File.Exists(imgPath))
					{
						System.IO.File.Delete(imgPath);
					}
				}


				_context.Students.Remove(student);
				_context.SaveChanges();
				return RedirectToAction("GetIndexView");
			}
		}
	}
}
