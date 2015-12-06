using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Neo4JTransX.TransXChange.Extensions
{
    public static class TransXTensions
    {
        private static XNamespace ns = "http://www.transxchange.org.uk/";

        public static XName AsXName(this string name)
        {
            return ns + name;
        }
    }
}
