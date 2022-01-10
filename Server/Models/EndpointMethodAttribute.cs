using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    class EndpointMethodAttribute : System.Attribute
    {
        public string Method;

        public EndpointMethodAttribute(string method)
        {
            this.Method = method;
        }
    }
}
