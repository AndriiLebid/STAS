using STAS.Model;

namespace STAS.Model
{
    public class User : BaseEntities
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }
        public int UserType { get; set; }
    }
}
