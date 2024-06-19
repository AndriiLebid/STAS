using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace STAS.Web.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "User type is required.")]
        public int UserType { get; set; }
        public IEnumerable<SelectListItem>? UserTypesList { get; set; }
    }
}
