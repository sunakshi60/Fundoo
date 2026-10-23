using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.User.UserRequest;

namespace FundooPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundooController : ControllerBase
    {
        private IUserBL _userBL;
        private readonly ILogger<FundooController> _logger;

        public FundooController(IUserBL _userBL, ILogger<FundooController> logger)
        {
            this._userBL = _userBL;
            _logger = logger;
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser(RegistrationModel registrationModel)
        {
            _logger.LogInformation("User registration request received.");

            try
            {
                var result = _userBL.RegisterUserBL(registrationModel);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("User registration failed. Reason: {Message}", result.Message);

                    return BadRequest(result);
                }

                _logger.LogInformation("User registered successfully.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during user registration.");

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("Login")]
        public IActionResult LoginUser(LoginModel loginModel)
        {
            var response = _userBL.LoginUserBL(loginModel);

                if (!response.IsSuccess)
                {
                    return Unauthorized(response);
                }

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var result = await _userBL.ForgotPassword(model);
            return Ok(result);
        }


        [Authorize]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            var tokenType = User.FindFirst("TokenType")?.Value;

            if (tokenType != "PasswordReset")
            {
                return Forbid();
            }

            var result = _userBL.ResetPassword(model);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}