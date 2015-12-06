using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class NaptanStop
    {
        public int Northing { get; set; }
        public string AtcoCode { get; set; }
        public string Indicator { get; set; }
        public string Bearing { get; set; }
        public string Street { get; set; }
        public int Easting { get; set; }
        public string NatGazId { get; set; }
        public string CommonName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
