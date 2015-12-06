using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class Service
    {
        public string ServiceCode { get; set; }
        public string PrivateCode { get; set; }
        public List<Line> Lines { get; set; }
        public OperatingPeriod OperatingPeriod { get; set; }
        public string RegisteredOperatorRef { get; set; }
        public string StopRequirements { get; set; }
        public string Mode { get; set; }
        public string Description { get; set; }
        public StandardService StandardService { get; set; }

        public string ToString()
        {
            return string.Format("srvcode: {0}, prvcode: {1}, mode: {2}", ServiceCode, PrivateCode, Mode);
        }
    }
    public class StandardService
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public List<JourneyPattern> JourneyPatterns { get; set; }
    }
    public class Line
    {
        public string Id { get; set; }
        public string LineName { get; set; }
    }
    public class OperatingPeriod
    {
        public string StartDate{ get; set; }
        public string EndDate{ get; set; }
    }
    public enum StopRequirements {
        NoNewStopsRequired
    }
    public class JourneyPattern
    {
        public string Id { get; set; }
        public string Direction { get; set; }
        public string RouteRef { get; set; }
        public string JourneyPatternSectionRefs { get; set; }
    }
}
