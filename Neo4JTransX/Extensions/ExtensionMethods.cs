using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.Extensions
{
    public static class ExtensionMethods
    {
        public static string ToJsonString(this List<string> els)
        {
            var itemStr = string.Join(",",els.Select(e => $"'{e}'"));
            return $"[{itemStr}]";
        }
    }
}
