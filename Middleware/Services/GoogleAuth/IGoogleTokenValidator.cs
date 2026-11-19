using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Services.GoogleAuth
{
    public record GoogleUserInfo(string Email, string GoogleId);

    public interface IGoogleTokenValidator
    {
        Task<GoogleUserInfo?> ValidateAsync(string idToken);
    }
}
