using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface IMessageClient
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
