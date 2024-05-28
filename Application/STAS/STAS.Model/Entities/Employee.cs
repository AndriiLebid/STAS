using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Model
{
    public class Employee : BaseEntities
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Number is required.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Employee Number must be 4 characters long.")]
        [Display(Name = "First Name")]
        public string? EmployeeNumber { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 30 characters long.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Initial (optional)")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Initial should be 1 character long.")]
        public string? MiddleInitial { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 30 characters long.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Type Employee")]
        public int TypeEmployeeId { get; set; }


    }
}
