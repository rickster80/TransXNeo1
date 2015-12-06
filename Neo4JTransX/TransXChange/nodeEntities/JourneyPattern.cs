using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class RouteLink
    {
        public string FromStopPointRef { get; set; }
        public string ToStopPointRef { get; set; }
        public string Direction { get; set; }
    }
    public class JourneyPatternSection
    {
        public string Id { get; set; }
        public List<JourneyPatternTimingLink> JourneyPatternTimingLinks { get; set; }
    }
    public class JourneyPatternTimingLink
    {
        public string Id { get; set; }
        public JourneyPatternTimingLinkPoint From { get; set; }
        public JourneyPatternTimingLinkPoint To { get; set; }
        public string RouteLinkRef { get; set; }
        public string RunTime { get; set; }

        public string ToString()
        {
            return string.Format("JPTL ID:{0}, from:{1}, to:{2}, rlref: {3}", Id, From.StopPointRef, To.StopPointRef, RouteLinkRef);

        }
    }
    public class JourneyPatternTimingLinkPoint
    {
        public string Activity { get; set; }
        public string StopPointRef { get; set; }
        public string TimingStatus { get; set; }
    }
}
