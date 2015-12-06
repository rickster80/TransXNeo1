using CsvHelper;
using Neo4JTransX.Extensions;
using Neo4JTransX.Neo4J;
using Neo4JTransX.Neo4J.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX
{
    class Program
    {
        public class StopCode
        {
            public string AtcoCode { get; set; }
        }
        static void Main(string[] args)
        {

        }
        public static void ParseStopCodes()
        {
            var filePath = @"C:\Users\Ricky\Documents\visual studio 2015\Projects\Neo4JTransX\Neo4JTransX\Data\StopCodes2.csv";
            var tr = File.OpenText(filePath);
            var csv = new CsvReader(tr);
            var setup = new NaptanSetup();
            //var stopCodes = csv.GetRecords<StopCode>();
            var codes = new List<string>();
            while (csv.Read())
            {
                var stopCode = csv.GetRecord<StopCode>();
                codes.Add(stopCode.AtcoCode);
                if (codes.Count >= 10)
                {
                    var cypher = $"MATCH n WHERE n.AtcoCode in {codes.ToJsonString()} RETURN n";
                    var nodes = Match(cypher).Result;

                    var nodeInfo = nodes.results.FirstOrDefault()?.data.Select(d => d.rest.FirstOrDefault()?.self).ToList();
                    var spatialRes = AddNodesToSpatialIndex(nodeInfo).Result;
                    codes.Clear();
                }
                Console.WriteLine(stopCode.AtcoCode);
            }
            Console.Read();
        }
        public static async Task<NeoRootObject<Data>> Match(string cypher, bool rest = true)
        {
            var http = new HttpClient();
            http.BaseAddress = new Uri("http://localhost:7474");
            var payload = JsonConvert.SerializeObject(new
            {
                statements = new object[] { new { statement = cypher, resultDataContents = new string[] { "REST" } } }
            });

            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Add("Authorization", "Basic bmVvNGo6bDBmdGdyMDB2ZXI=");
            var response = await http.PostAsync("/db/data/transaction/commit", new StringContent(payload, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<NeoRootObject<Data>>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
            return null;
        }

        public static async Task<IEnumerable<SpatialRootObject<Data>>> AddNodesToSpatialIndex(List<string> nodes)
        {
            var http = new HttpClient();
            http.BaseAddress = new Uri("http://localhost:7474");
            var payload = JsonConvert.SerializeObject(new { nodes = nodes, layer = "geom" });

            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Add("Authorization", "Basic bmVvNGo6bDBmdGdyMDB2ZXI=");
            var response = await http.PostAsync("/db/data/ext/SpatialPlugin/graphdb/addNodesToLayer", new StringContent(payload, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<IEnumerable<SpatialRootObject<Data>>>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
            return null;
        }
    }
}
