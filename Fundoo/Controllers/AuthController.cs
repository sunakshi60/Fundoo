using BussinessLayer.User.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Services.GoogleAuth;
using ModelLayer.Model.User.RequestModel;

namespace FundooPractice.Controllers
{
    [ApiController]
    [Route("api/google")]
    public class AuthController : ControllerBase
    {
        private readonly IGoogleTokenValidator _tokenValidator;
        private readonly IGoogleLoginQueryHandler _googleLoginHandler;

        public AuthController(IGoogleTokenValidator tokenValidator,
                              IGoogleLoginQueryHandler googleLoginHandler)
        {
            _tokenValidator = tokenValidator;
            _googleLoginHandler = googleLoginHandler;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] GoogleTokenModel request)
        {
            var googleUser = await _tokenValidator.ValidateAsync(request.TokenId);

            if (googleUser == null)
            {
                return Unauthorized(new { message = "Invalid or expired Google token." });
            }

            var response = _googleLoginHandler.Handle(
                new GoogleLoginQuery(googleUser.Email, googleUser.GoogleId));

            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}