using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Services.Email
{
    public interface IEmailService
    {
         Task SendEmailAsync(string email, string subject, string body);
    }
}
