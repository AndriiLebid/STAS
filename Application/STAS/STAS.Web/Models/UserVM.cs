using Microsoft.AspNetCore.Mvc.Rendering;

namespace STAS.Web.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public int UserType { get; set; }
        public IEnumerable<SelectListItem>? UserTypesList { get; set; }
    }
}
