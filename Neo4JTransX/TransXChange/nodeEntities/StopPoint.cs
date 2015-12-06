using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TransXTesting.Models.TransXToNeo4j.relationshipEntities;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class StopPoint
    {
    }
    public class AnnotatedStopPoint
    {
        public string StopPointRef { get; set; }
        public string CommonName { get; set; }
        public string LocalityName { get; set; }
        public string LocalityQualifier { get; set; }
    }
    public class Route
    {
        public string id { get; set; }
        public string PrivateCode { get; set; }
        public string Description { get; set; }
        public string RouteSectionRef { get; set; }
    }
    public class RouteSection
    {
        public string Id { get; set; }
        public List<RouteLink> RouteLinks { get; set; }
    }
}
