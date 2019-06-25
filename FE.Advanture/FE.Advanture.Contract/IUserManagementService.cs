using System;
using System.Collections.Generic;
using System.Text;

namespace FE.Advanture.Contract
{
    public interface IUserManagementService
    {
        bool IsValidUser(string username, string password);
    }
}
