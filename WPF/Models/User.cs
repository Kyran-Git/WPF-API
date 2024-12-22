using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Models
{
    internal class User
    {
        public int Id {  get; set; }
        public string UserName { get; set; } = string.Empty;
        public SecureString Password { get; set; }
    }
}
