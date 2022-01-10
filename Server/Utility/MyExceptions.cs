using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utility
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException() { }

        public InvalidInputException(string text) : base(text) { }
    }

    class NotLoggedInException : Exception
    {
        public NotLoggedInException() { }

        public NotLoggedInException(string text) : base(text) { }
    }
}
