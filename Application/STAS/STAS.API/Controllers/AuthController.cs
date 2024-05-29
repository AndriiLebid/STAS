using Microsoft.AspNetCore.Mvc;
using STAS.API.Interfaces;
using STAS.Model;
using STAS.Services;

namespace STAS.API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginService service = new LoginService();
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpPost]
        public async Task<ActionResult<LoginOutputDTO>> Login(LoginDTO credentials)
        {
            LoginUser? user = await service.Login(credentials);

            if (user == null)
            {
                return Unauthorized("Invalid login");
            }

            return new LoginOutputDTO()
            {
                UserName = user.LoginUserName,
                Token = _tokenService.CreateToken(user),
                ExpiresIn = 7 * 24 * 60 * 60
            };
        }

    }
}
