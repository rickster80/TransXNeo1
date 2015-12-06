using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.Neo4J.Models
{
    class AddNodesToLayerResponse
    {
    }

    public class SpatialData
    {
        public List<double> bbox { get; set; }
        public int gtype { get; set; }
    }

    //public class Data2
    //{
    //    public string Name { get; set; }
    //    public string NatGazId { get; set; }
    //    public string Street { get; set; }
    //    public string Type { get; set; }
        
    //    public double Latitude { get; set; }
    //    public double Longitude { get; set; }
        
    //    public string AtcoCode { get; set; }
    //}

    public class Extensions
    {
    }

    public class Metadata2
    {
        public int id { get; set; }
        public List<string> labels { get; set; }
    }

    public class SpatialRootObject<T> where T : SpatialData
    {
        public string labels { get; set; }
        public string outgoing_relationships { get; set; }
        public T data { get; set; }
        public string traverse { get; set; }
        public string all_typed_relationships { get; set; }
        public string property { get; set; }
        public string self { get; set; }
        public string outgoing_typed_relationships { get; set; }
        public string properties { get; set; }
        public string incoming_relationships { get; set; }
        public Extensions extensions { get; set; }
        public string create_relationship { get; set; }
        public string paged_traverse { get; set; }
        public string all_relationships { get; set; }
        public string incoming_typed_relationships { get; set; }
        public Metadata2 metadata { get; set; }
    }

}
