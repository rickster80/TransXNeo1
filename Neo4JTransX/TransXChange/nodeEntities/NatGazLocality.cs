using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class NatGazLocality
    {
        public string NptgLocalityCode { get; set; }
        public string LocalityName { get; set; }
        public int Easting { get; set; }
        public int Northing { get; set; }
        public int NptgDistrictCode { get; set; }
    }
}
