using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeReg.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EmployeeReg.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly EmployeeContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public EmployeeController(EmployeeContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: Employee
		public async Task<IActionResult> Index()
		{
			return View(await _context.Employees.ToListAsync());
		}

		// GET: Employee/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.FirstOrDefaultAsync(m => m.EmployeeId == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// GET: Employee/Create
		public IActionResult AddOrEdit(int id = 0)
		{
			if (id == 0)
				return View(new Employee());
			else
				return View(_context.Employees.Find(id));
		}

		// POST: Employee/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddOrEdit([Bind("EmployeeId,FullName,EmpCode,DateOfBirth,Email,Gender,MaritalStatus,ImageFile")] Employee employee)
		{
			if (ModelState.IsValid)
			{
				var files = HttpContext.Request.Form.Files;
				if (files.Count > 0)
				{
					string wwwRootPath = _webHostEnvironment.WebRootPath;
					string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
					string extension = Path.GetExtension(employee.ImageFile.FileName);

					employee.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
					string path = Path.Combine(wwwRootPath + "/images/", fileName);

					using (var filestream = new FileStream(path, FileMode.Create))
					{
						await employee.ImageFile.CopyToAsync(filestream);
					}
				}
				else
				{
					if (employee.EmployeeId != 0)
					{
						
					}
				}

				if (employee.EmployeeId == 0)
					_context.Add(employee);
				else
					_context.Update(employee);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(employee);
		}

		// GET: Employee/Edit/5
		//public async Task<IActionResult> Edit(int? id)
		//{
		//	if (id == null)
		//	{
		//		return NotFound();
		//	}

		//	var employee = await _context.Employees.FindAsync(id);
		//	if (employee == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(employee);
		//}

		//// POST: Employee/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FullName,EmpCode,DateOfBirth,Email,Gender,MaritalStatus,ImageFile")] Employee employee)
		//{
		//	if (id != employee.EmployeeId)
		//	{
		//		return NotFound();
		//	}

		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			var files = HttpContext.Request.Form.Files;
		//			if (files.Count > 0)
		//			{
		//				string wwwRootPath = _webHostEnvironment.WebRootPath;
		//				string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
		//				string extension = Path.GetExtension(employee.ImageFile.FileName);

		//				employee.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
		//				string path = Path.Combine(wwwRootPath + "/images/", fileName);

		//				using (var filestream = new FileStream(path, FileMode.Create))
		//				{
		//					await employee.ImageFile.CopyToAsync(filestream);
		//				}
		//			}

		//			_context.Update(employee);
		//			await _context.SaveChangesAsync();
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!EmployeeExists(employee.EmployeeId))
		//			{
		//				return NotFound();
		//			}
		//			else
		//			{
		//				throw;
		//			}
		//		}
		//		return RedirectToAction(nameof(Index));
		//	}
		//	return View(employee);
		//}

		// GET: Employee/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			var employee = await _context.Employees.FindAsync(id);

			//delete
			if (employee.ImageName != string.Empty && employee.ImageName != null)
			{
				var imagepath = Path.Combine(_webHostEnvironment.WebRootPath, "images", employee.ImageName);
				if (System.IO.File.Exists(imagepath))
					System.IO.File.Delete(imagepath);
			}

			_context.Employees.Remove(employee);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
