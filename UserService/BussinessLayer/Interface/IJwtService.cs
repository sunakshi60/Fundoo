using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email);

        string GenerateResetToken(int userId, string email);
    }
}
