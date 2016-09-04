using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public class MSniperClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var w = base.GetWebRequest(uri);
            w.Timeout = 12345;
            return w;
        }
    }
}
