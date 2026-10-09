using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

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

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var result = _userBL.GetAllUsersBL();

            return Ok(result);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var result = _userBL.GetUserByIdBL(userId);

            if (result == null)
            {
                return NotFound("User not found");
            }

            return Ok(result);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser( int userId, RegistrationModel registrationModel)
        {
            var result = _userBL.UpdateUserBL(userId, registrationModel);
            if (result == null)
            {
                return NotFound("User not found");
            }
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var result = _userBL.DeleteUserBL(userId);

            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok("User deleted successfully");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var result = await _userBL.ForgotPassword(model);

            if (!result)
            {
                return BadRequest("Email not found");
            }

            return Ok("Password reset link sent to your email");
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

            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                return Unauthorized();
            }

            var token = authHeader.Replace("Bearer ", "");

            var result = _userBL.ResetPassword(
                token,
                model.NewPassword
            );

            if (!result)
            {
                return BadRequest("Invalid or expired reset token");
            }

            return Ok("Password reset successfully");
        }

    }
}
