using System.ComponentModel.DataAnnotations;

namespace STAS.Web.Models
{
    public class LoginVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee Name is required.")]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        public string? Password { get; set; }

    }
}
