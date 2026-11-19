using BussinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.User.ResponseModel;
using RepositoryLayer.Interface;

namespace BussinessLayer.User.Query
{
    public class GoogleLoginQueryHandler : IGoogleLoginQueryHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly ILogger<GoogleLoginQueryHandler> _logger;

        public GoogleLoginQueryHandler(IUserRL userRL, IJwtService jwtService, ILogger<GoogleLoginQueryHandler> logger)
        {
            _userRL = userRL;
            _jwtService = jwtService;
            _logger = logger;
        }

        public GoogleLoginResponseModel Handle(GoogleLoginQuery query)
        {
            _logger.LogInformation("Google login process started.");

            var response = new GoogleLoginResponseModel();

            // Validate the values received from Google.
            if (string.IsNullOrWhiteSpace(query.Email) ||
                string.IsNullOrWhiteSpace(query.GoogleId))
            {
                _logger.LogWarning("Google login failed because email or Google ID was missing.");

                response.IsSuccess = false;
                response.Message = "Google account details were not provided.";
                return response;
            }

            // Find the user by Google ID first, then by email.
            var user = _userRL.GetUserByGoogleId(query.GoogleId)
                    ?? _userRL.GetUserByEmail(query.Email);

            // Do not register users automatically.
            if (user == null)
            {
                _logger.LogWarning("Google login attempted for an email not registered in Fundoo.");

                response.IsSuccess = false;
                response.Message = "User not registered in Fundoo. Please register first.";
                return response;
            }

            if (user.GoogleId == null)
            {
                // First Google login: link this Google account to the user.
                _userRL.LinkGoogleId(user.UserId, query.GoogleId);
            }
            else if (user.GoogleId != query.GoogleId)
            {
                // Email matches, but the account is linked to a different Google account.
                _logger.LogWarning(
                    "Google login rejected: email matches UserId {UserId} but Google ID differs.",
                    user.UserId);

                response.IsSuccess = false;
                response.Message = "This email is linked to a different Google account.";
                return response;
            }

            // Generate Fundoo's JWT for the user.
            response.IsSuccess = true;
            response.Message = "Google login successful.";
            response.Data = new LoginResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = _jwtService.GenerateToken(user.UserId, user.Email)
            };

            _logger.LogInformation(
                "Google login completed successfully for UserId: {UserId}",
                user.UserId);

            return response;
        }
    }
}
