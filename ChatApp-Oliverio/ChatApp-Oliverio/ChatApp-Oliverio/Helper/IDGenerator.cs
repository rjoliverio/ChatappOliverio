using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatApp_Oliverio { 
    public class IDGenerator
    {
        public static string generateID()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
    }
}
