using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPOS.Shared.POS
{
    public class reqLogin
    {
        public string Username { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class respLogin
    {
        public string Names { get; set; }    = string.Empty;
        public string Usernames { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
