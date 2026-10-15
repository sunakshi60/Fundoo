using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using Middleware.Services.Email;
using ModelLayer.Model.User.UserRequest;

namespace FundooPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundooController : ControllerBase
    {
        private IUserBL _userBL;

        public FundooController(IUserBL _userBL)
        {
            this._userBL = _userBL;
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser(RegistrationModel registrationModel)
        {
            var result = _userBL.RegisterUserBL(registrationModel);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
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