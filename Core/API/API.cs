using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.API
{
    public static class API
    {
        static HttpListener httpListener = new HttpListener();
        public static string URL => String.Join("", httpListener.Prefixes);
    }
}
