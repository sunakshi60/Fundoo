using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Middleware.Services.GoogleAuth
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private readonly IConfiguration _configuration;

        public GoogleTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleUserInfo?> ValidateAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    // Token must be issued for YOUR app's client ID
                    Audience = new[] { _configuration["Authentication:Google:ClientId"]! }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                if (!payload.EmailVerified)
                    return null;

                return new GoogleUserInfo(payload.Email, payload.Subject);
            }
            catch (InvalidJwtException)
            {
                return null; // bad, expired, or wrong-audience token
            }
        }
    }
}