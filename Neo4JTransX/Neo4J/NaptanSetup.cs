using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.Neo4J
{
    class NaptanSetup
    {
        private string baseUrl = @"http://localhost/db/data";
        private string SpatialIndexNodeEndPoint { get { return $"{baseUrl}/ext/SpatialPlugin/graphdb/addNodeToLayer"; } }
        private string AddSimplePointLayerEndPoint { get { return $"{baseUrl}/ext/SpatialPlugin/graphdb/addSimplePointLayer"; } }
        private string IndexNodeEndPoint { get { return $"{baseUrl}/index/node"; } }

        private string NodeEndPoint { get { return $"{baseUrl}/node"; } }
        private string IndexGeomEndPoint
        {
            get
            {
                return $"/index/node/geom";
            }
        }
        private string FindGeomWithinDistEndPoint
        {
            get
            {
                return $"{baseUrl}/ext/SpatialPlugin/graphdb/findGeometriesWithinDistance";
            }
        }
        private string CypherEndpoint
        {
            get
            {
                return $"{baseUrl}/db/data/cypher";
            }
        }

        public string AddSimplePointLayer()
        {
            //  :body => { :name => 'geom', :config => { :provider => 'spatial', :geometry_type => 'point', :lat => 'lat', :lon => 'lon'  } }.to_json,

            var httpClient = new HttpClient { BaseAddress = new Uri(AddSimplePointLayerEndPoint) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postData = new { lat = "Latitude", lon = "Longitude", layer = "geom" };
            var json = JsonConvert.SerializeObject(postData);
            var res =
                httpClient.PostAsync(httpClient.BaseAddress, new StringContent(json, Encoding.UTF8, "application/json"))
                    .Result;
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                return result;
            }
            return "";
        }
        public async void AddSpatialIndexNode()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(IndexNodeEndPoint) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postData = new
            {
                name = "geom",
                config = new
                {
                    provider = "spatial",
                    geometry_type = "point",
                    lat = "Latitude",
                    lon = "Longitude"
                }
            };
            var json = JsonConvert.SerializeObject(postData);
            HttpResponseMessage res =
                await
                    httpClient.PostAsync(httpClient.BaseAddress,
                        new StringContent(json, Encoding.UTF8, "application/json"));
            // var res = httpClient.PostAsync(httpClient.BaseAddress, json);   
        }

        public void AddIndexNodeGeom(string nodeUri)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(IndexGeomEndPoint);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var nodeGeom = new
            {
                key = "dummy",
                value = "dummy",
                uri = nodeUri
            };
            var json = JsonConvert.SerializeObject(nodeGeom);
            HttpResponseMessage res =
                httpClient.PostAsync(httpClient.BaseAddress, new StringContent(json, Encoding.UTF8, "application/json"))
                    .Result;
            if (res.IsSuccessStatusCode)
            {
                string jsonResult = res.Content.ReadAsStringAsync().Result;

            }
        }

        public void AddNodeToIndexLayer(string nodeUri)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(SpatialIndexNodeEndPoint);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var nodeSpatialIndex = new
            {
                layer = "geom",
                node = nodeUri
            };
            var json = JsonConvert.SerializeObject(nodeSpatialIndex);
            HttpResponseMessage res =
                httpClient.PostAsync(httpClient.BaseAddress, new StringContent(json, Encoding.UTF8, "application/json"))
                    .Result;
            if (res.IsSuccessStatusCode)
            {
                string jsonResult = res.Content.ReadAsStringAsync().Result;
                Console.WriteLine(jsonResult);
            }
        }
        //public NeoServiceResponse GetStopsWithinDistance(double lat, double lng, double radius)
        //{
        //    //{"layer":"geom","pointX":15.0,"pointY":60.0,"distanceInKm":100.0}
        //    NeoServiceResponse serviceRes = new NeoServiceResponse();
        //    var httpClient = new HttpClient();
        //    httpClient.BaseAddress = new Uri(FindGeomWithinDistEndPoint);
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var nodeGeom = new
        //    {
        //        layer = "geom",
        //        pointY = lat,
        //        pointX = lng,
        //        distanceInKm = radius
        //    };
        //    var json = JsonConvert.SerializeObject(nodeGeom);
        //    var res = httpClient.PostAsync(httpClient.BaseAddress,
        //        new StringContent(json, Encoding.UTF8, "application/json")).Result;
        //    if (res.IsSuccessStatusCode)
        //    {
        //        var data = res.Content.ReadAsStringAsync().Result;
        //        // data = (data.First().Equals('[')) ? data.Substring(1, data.Length - 2) : data;
        //        var result = JsonConvert.DeserializeObject<List<GetNodesInDistanceResponse>>(data.Trim());
        //        serviceRes.Nodes.AddRange(result);
        //    }
        //    else
        //    {
        //        serviceRes.Errors.Add(new ServiceError(res.Content.ToString()));
        //    }
        //    return serviceRes;
        //}
    }
}
