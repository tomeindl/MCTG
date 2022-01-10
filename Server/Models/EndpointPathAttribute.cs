using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)
    ]
    public class EndpointPathAttribute : System.Attribute
    {
        public string Path;

        public EndpointPathAttribute(string path)
        {
            this.Path = path; 
        }
    }
}
