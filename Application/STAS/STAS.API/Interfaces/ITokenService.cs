using STAS.Model;


namespace STAS.API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(LoginUser user);
    }
}
