using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, List<string> roles);
    }
}
