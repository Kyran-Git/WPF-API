using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPF.RoleLogic
{
    public static class Permissions
    {
        public static bool AdminAccess(string role)
        {
            return role == "Admin";
        }

        public static bool NormalAccess(string role)
        {
            return role == "Normal";
        }
    }
}