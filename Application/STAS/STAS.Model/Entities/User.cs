using STAS.Model;
using System.ComponentModel.DataAnnotations;

namespace STAS.Model
{
    public class User : BaseEntities
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }
        [Required(ErrorMessage = "User Type is required.")]
        public int UserType { get; set; }
    }
}
