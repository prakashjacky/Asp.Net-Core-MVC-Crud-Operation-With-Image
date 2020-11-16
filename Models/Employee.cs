using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EmployeeReg.Models
{
	public class Employee
	{
		[Key]
		public int EmployeeId { get; set; }

		[Column(TypeName = "nvarchar(250)")]
		[Required(ErrorMessage = "This field is required")]
		[DisplayName("Full Name")]
		public string FullName { get; set; }

		[Column(TypeName = "varchar(10)")]
		[DisplayName("Emp. Code")]
		public string EmpCode { get; set; }

		[Required(ErrorMessage = "Please Enter DOB...")]
		[Display(Name = "Date of Birth")]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "Please Enter Email...")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Select the Gender...")]
		[Display(Name = "Gender")]
		public string Gender { get; set; }

		[Required(ErrorMessage = "Select the Marital Status...")]
		[Display(Name = "Marital Status")]
		public string MaritalStatus { get; set; }

		[Column(TypeName = "nvarchar(250)")]
		[Display(Name = "Image Name")]
		public string ImageName { get; set; }

		[NotMapped]
		[Display(Name = "Upload File")]
		public IFormFile ImageFile { get; set; }
	}
}
