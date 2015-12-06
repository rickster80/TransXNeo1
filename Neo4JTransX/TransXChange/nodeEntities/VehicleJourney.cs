using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.TransXChange.nodeEntities
{
    public class VehicleJourney
    {
        public string ServiceRef { get; set; }
        public OperatingProfile OperatingProfile { get; set; }
        public string VehicleJourneyCode { get; set; }
        public string LineRef { get; set; }
        public string DepartureTime { get; set; }
        
    }
    public class OperatingProfile
    {
        public List<string> RegularDaysOfWeek { get; set; }
        public OperatingPeriod SpecialDaysNonOperation { get; set; }
        public List<string> BankHolidayNonOperation { get; set; }
    }
}
