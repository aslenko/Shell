using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Core.Security
{
    public static class AuthHeaderSchemes
    {
        public const string MAC = "MAC";
    }

    public static class HttpCookieNames
    {      
        public const string UserName = "UserName";
        public const string UserToken = "UserToken";       
    }
}
