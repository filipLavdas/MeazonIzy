using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeazonIzy.Models
{
    public interface ICookie
    {
        bool SetAppCookie(string url);

        void ClearCookies(string url);
    }
}
