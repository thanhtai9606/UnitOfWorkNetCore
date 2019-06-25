using System;
using System.Collections.Generic;
using System.Text;

namespace FE.Advanture.Contract
{
    public class UserManagementService : IUserManagementService
    {
        public bool IsValidUser(string userName, string password)
        {
            return true;
        }
    }
}
