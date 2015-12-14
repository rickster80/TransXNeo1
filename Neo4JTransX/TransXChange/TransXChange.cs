using Neo4JTransX.TransXChange.Extensions;
using Neo4JTransX.TransXChange.nodeEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Neo4JTransX.TransXChange
{
    public class TransXChangeParser
    {
        
        private List<Route> _routes;
        private List<Operator> _operators;
        private List<VehicleJourney> _vehicleJourneys;
        private List<JourneyPatternSection> _journeyPatternSections;
        private List<Service> _services;
        private List<RouteSection> _routeSections;
        private List<AnnotatedStopPoint> _stopPoints;
        public TransXChangeParser(string filePath)
        {
            _routes = new List<Route>();
            _operators = new List<Operator>();
            _vehicleJourneys = new List<VehicleJourney>();
            _journeyPatternSections = new List<JourneyPatternSection>();
            _services = new List<Service>();
            _routeSections = new List<RouteSection>();
            _stopPoints = new List<AnnotatedStopPoint>();

            //xDoc = XDocument.Load(filePath);
        }
        public void ParseTransXSchedule()
        {
            //var path = @"C:\Users\Ricky_2\Documents\TNDS\eataken";
            //string[] files = Directory.GetFiles(path, "*.xml");
            //foreach (var file in files)
            //{
            // var fpSplit = file.Split('-');
            //var timetable = fpSplit.ElementAt(0);
            //var timetableDate = fpSplit.ElementAt(1);
            //var timetableNo = fpSplit.ElementAt(2);
            var file = @"C:\Users\Ricky\Documents\visual studio 2015\Projects\Neo4JTransX\Neo4JTransX\Data\ea_20-1-_-y08-1.xml";
                XDocument xDoc = XDocument.Load(file);

                processJourneyPatternSections(xDoc);
                processOperators(xDoc);
                processRoutes(xDoc);
                processRouteSections(xDoc);
                processServices(xDoc);
                processStopPoints(xDoc);
                processVehicleJourneys(xDoc);

                Console.WriteLine("done");
           // }
        }
        private void processOperators(XDocument xDoc)
        {
            var operators = xDoc.Descendants("Operators".AsXName()).Elements("Operator".AsXName()).ToList()
                .Select(o => new Operator()
                {
                    Id = o.Attribute("id").Value,
                    NationalOperatorCode = o.Element("NationalOperatorCode".AsXName()).Value,
                    OperatorCode = o.Element("OperatorCode".AsXName()).Value,
                    TradingName = o.Element("TradingName".AsXName()).Value,
                    OperatorShortName = o.Element("OperatorShortName".AsXName()).Value,
                    OperatorNameOnLicence = o.Element("OperatorNameOnLicence".AsXName()).Value
                }).ToList();
            _operators.AddRange(operators);
            operators.ForEach(o =>
            {
                Console.WriteLine(o.ToString());
            });

        }
        private void processVehicleJourneys(XDocument xDoc)
        {
            var parsed = new List<VehicleJourney>();
            var vehicleJourneys = xDoc.Descendants("VehicleJourneys".AsXName()).Elements("VehicleJourney".AsXName()).ToList();
            vehicleJourneys.ForEach(vj =>
            {
                var privateCode = vj.Element("PrivateCode".AsXName());
                var operatoringProfile = vj.Element("OperatingProfile".AsXName());
                var oRegularDayType = operatoringProfile.Element("RegularDayType".AsXName());
                var daysOfWeek = oRegularDayType.Element("DaysOfWeek".AsXName())?.Elements()?.Select(o => o.Name.LocalName)?.ToList();

                var specialDaysOp = operatoringProfile.Element("SpecialDaysOperation".AsXName());
                //if (specialDaysOp != null)
                //{
                var daysNoneOp = specialDaysOp?.Element("DaysOfNonOperation".AsXName()).Element("DateRange".AsXName());
                var nonOpPeriod = new OperatingPeriod()
                {
                    StartDate = daysNoneOp?.Element("StartDate".AsXName()).Value,
                    EndDate = daysNoneOp?.Element("EndDate".AsXName()).Value
                };
                //}
                var bankHolOp = operatoringProfile?.Element("BankHolidayOperation".AsXName());
                var bhNonOp = bankHolOp?.Element("DaysOfNonOperation".AsXName())?.Elements().Select(o => o.Name.LocalName)?.ToList();

                var opObj = new OperatingProfile()
                {
                    BankHolidayNonOperation = bhNonOp,
                    SpecialDaysNonOperation = nonOpPeriod,
                    RegularDaysOfWeek = daysOfWeek
                };

                var vjCode = vj.Element("VehicleJourneyCode".AsXName()).Value;
                var serviceRef = vj.Element("ServiceRef".AsXName()).Value;
                var lineRef = vj.Element("LineRef".AsXName()).Value;
                var journeyPatternRef = vj.Element("JourneyPatternRef".AsXName()).Value;
                var departureTime = vj.Element("DepartureTime".AsXName()).Value;

                parsed.Add(new VehicleJourney()
                {
                    ServiceRef = serviceRef,
                    VehicleJourneyCode = vjCode,
                    LineRef = lineRef,
                    OperatingProfile = opObj,
                    DepartureTime = departureTime
                });

            });
            _vehicleJourneys.AddRange(parsed);
            var sel = parsed;
        }
        private void processServices(XDocument xDoc)
        {
            var parsed = new List<Service>();
            var services = xDoc.Descendants("Services".AsXName()).Elements("Service".AsXName())?.ToList();
            services.ForEach(srv =>
            {
                var srvCode = srv.Element("ServiceCode".AsXName()).Value;
                var prvCode = srv.Element("PrivateCode".AsXName()).Value;
                var lines = srv.Element("Lines".AsXName()).Descendants("Line".AsXName())?.ToList()
                .Select(l => new Line()
                {
                    Id = l.Attribute("id").Value,
                    LineName = l.Element("LineName".AsXName()).Value
                }).ToList();
                var op = new OperatingPeriod()
                {
                    StartDate = srv.Element("OperatingPeriod".AsXName()).Element("StartDate".AsXName()).Value,
                    EndDate = srv.Element("OperatingPeriod".AsXName()).Element("EndDate".AsXName()).Value
                };
                var regOpRef = srv.Element("RegisteredOperatorRef".AsXName()).Value;
                var stopReq = srv.Element("StopRequirements".AsXName()).Value;
                var mode = srv.Element("Mode".AsXName()).Value;
                var desc = srv.Element("Description".AsXName()).Value;
                var ssEl = srv.Element("StandardService".AsXName());
                var ssJP = ssEl.Elements("JourneyPattern".AsXName())?.ToList()
                    .Select(jp => new JourneyPattern()
                    {
                        Id = jp.Attribute("id").Value,
                        RouteRef = jp.Element("RouteRef".AsXName()).Value,
                        Direction = jp.Element("Direction".AsXName()).Value,
                        JourneyPatternSectionRefs = jp.Element("JourneyPatternSectionRefs".AsXName()).Value
                    }).ToList();
                var standardService = new StandardService()
                {
                    Origin = ssEl.Element("Origin".AsXName()).Value,
                    Destination = ssEl.Element("Destination".AsXName()).Value,
                    JourneyPatterns = ssJP
                };

                var service = new Service()
                {
                    Description = desc,
                    Lines = lines,
                    Mode = mode,
                    OperatingPeriod = op,
                    PrivateCode = prvCode,
                    RegisteredOperatorRef = regOpRef,
                    ServiceCode = srvCode,
                    StandardService = standardService,
                    StopRequirements = stopReq
                };
                parsed.Add(service);
                Console.WriteLine(service.ToString());
            });
            _services.AddRange(parsed);

        }
        private void processJourneyPatternSections(XDocument xDoc)
        {
            var parsedJPS = new List<JourneyPatternSection>();
            var journeyPatternSections = xDoc.Descendants("JourneyPatternSections".AsXName()).Elements("JourneyPatternSection".AsXName()).ToList();
            journeyPatternSections.ForEach(jps =>
            {
                var jpsId = jps.Attribute("id").Value;
                //Console.WriteLine("--------route section id: {0}------------", rsId);
                var journeyPatternTimingLinks = jps.Descendants("JourneyPatternTimingLink".AsXName()).ToList()
                    .Select(jptl => new JourneyPatternTimingLink
                    {
                        Id = jptl.Attribute("id").Value,
                        From = new JourneyPatternTimingLinkPoint()
                        {
                            Activity = jptl.Element("From".AsXName()).Element("Activity".AsXName()).Value,
                            StopPointRef = jptl.Element("From".AsXName()).Element("StopPointRef".AsXName()).Value,
                            TimingStatus = jptl.Element("From".AsXName()).Element("TimingStatus".AsXName()).Value
                        },
                        To = new JourneyPatternTimingLinkPoint()
                        {
                            Activity = jptl.Element("To".AsXName()).Element("Activity".AsXName()).Value,
                            StopPointRef = jptl.Element("To".AsXName()).Element("StopPointRef".AsXName()).Value,
                            TimingStatus = jptl.Element("To".AsXName()).Element("TimingStatus".AsXName()).Value
                        },
                        RouteLinkRef = jptl.Element("RouteLinkRef".AsXName()).Value,
                        RunTime = jptl.Element("RunTime".AsXName()).Value
                    }).ToList();
                journeyPatternTimingLinks.ForEach(jptl => Console.WriteLine(jptl.ToString()));
                parsedJPS.Add(new JourneyPatternSection()
                {
                    Id = jpsId,
                    JourneyPatternTimingLinks = journeyPatternTimingLinks
                });
                _journeyPatternSections.AddRange(parsedJPS);
            });

        }
        private void processRouteSections(XDocument xDoc)
        {
            var parsedRS = new List<RouteSection>();

            var routeSections = xDoc.Descendants("RouteSections".AsXName()).Elements("RouteSection".AsXName()).ToList();
            routeSections.ForEach(rs =>
            {
                var rsId = rs.Attribute("id").Value;
                Console.WriteLine("--------route section id: {0}------------", rsId);
                var routeLinks = rs.Descendants("RouteLink".AsXName())
                    .Select(rl => new RouteLink
                    {
                        FromStopPointRef = rl.Element("From".AsXName()).Element("StopPointRef".AsXName()).Value,
                        ToStopPointRef = rl.Element("To".AsXName()).Element("StopPointRef".AsXName()).Value,
                        Direction = rl.Element("Direction".AsXName()).Value
                    }).ToList();
                routeLinks.ForEach(rl => {
                    Console.WriteLine("{0}, {1}, {2}", rl.FromStopPointRef, rl.ToStopPointRef, rl.Direction);
                });
                parsedRS.Add(new RouteSection()
                {
                    Id = rsId,
                    RouteLinks = routeLinks
                });
            });
            _routeSections.AddRange(parsedRS);

        }
        private void processRoutes(XDocument xDoc)
        {
            var parsedRoutes = new List<Route>();
            var routes = xDoc.Descendants("Routes".AsXName()).Descendants("Route".AsXName()).ToList();
            routes.ForEach(r =>
            {
                //var aspr = sp.Element("AnnotatedStopPointRef".AsXName());
                var pc = r.Element("PrivateCode".AsXName()).Value;
                var d = r.Element("Description".AsXName()).Value;
                var rsr = r.Element("RouteSectionRef".AsXName()).Value;
                var rId = r.Attribute("id").Value;

                parsedRoutes.Add(new Route()
                {
                    Description = d,
                    id = rId,
                    PrivateCode = pc,
                    RouteSectionRef = rsr
                });
                Console.WriteLine("{0}, {1}, {2}, {3}", rId, pc, d, rsr);
            });
            _routes.AddRange(parsedRoutes);

        }
        private void processStopPoints(XDocument xDoc)
        {
            var parsedStopPoints = new List<AnnotatedStopPoint>();
            var stopPoints = xDoc.Descendants("StopPoints".AsXName()).Elements("AnnotatedStopPointRef".AsXName()).ToList();
            stopPoints.ForEach(sp =>
            {
                //var aspr = sp.Element("AnnotatedStopPointRef".AsXName());
                var spr = sp.Element("StopPointRef".AsXName()).Value;
                var cn = sp.Element("CommonName".AsXName()).Value;
                var ln = sp.Element("LocalityName".AsXName()).Value;
                var lq = sp.Element("LocalityQualifier".AsXName()).Value;
                parsedStopPoints.Add(new AnnotatedStopPoint()
                {
                    CommonName = cn,
                    LocalityName = ln,
                    LocalityQualifier = lq,
                    StopPointRef = spr
                });
                Console.WriteLine("{0}, {1}, {2}, {3}", spr, cn, ln, lq);
            });
            _stopPoints.AddRange(parsedStopPoints);

        }
    }
}
