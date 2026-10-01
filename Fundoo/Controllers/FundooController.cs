using Microsoft.AspNetCore.Mvc;
using BussinessLayer.Interface;
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
            var result = _userBL.LoginUserBL(loginModel);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

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
    }
}
