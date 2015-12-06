using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class Operator
    {
        public string Id { get; set; }
        public string NationalOperatorCode { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorShortName { get; set; }
        public string OperatorNameOnLicence { get; set; }
        public string TradingName { get; set; }

        public string ToString()
        {
            return string.Format("{0}, {1}, {2}", Id, NationalOperatorCode, OperatorShortName);
        }
    }
}
