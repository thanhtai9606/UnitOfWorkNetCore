using FE.Advanture.Models.HSSE;
using System;
using System.Collections.Generic;
using System.Text;

namespace FE.Advanture.Contract
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
