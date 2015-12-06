using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4JTransX.Neo4J.Models
{
    class Neo4JDataResponse
    {
    }

    public class Metadata
    {
        public int id { get; set; }
        public List<string> labels { get; set; }
    }

    public class Data : SpatialData
    {
        public string Type { get; set; }
        public string AtcoCode { get; set; }
        public string Street { get; set; }
        public string NatGazId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Name { get; set; }
    }

    public class Rest<T>
    {
        public string labels { get; set; }
        public string outgoing_relationships { get; set; }
        public string traverse { get; set; }
        public string all_typed_relationships { get; set; }
        public string property { get; set; }
        public string self { get; set; }
        public string outgoing_typed_relationships { get; set; }
        public string properties { get; set; }
        public string incoming_relationships { get; set; }
        public string create_relationship { get; set; }
        public string paged_traverse { get; set; }
        public string all_relationships { get; set; }
        public string incoming_typed_relationships { get; set; }
        public Metadata metadata { get; set; }
        public T data { get; set; }
    }

    public class Datum<T>
    {
        public List<Rest<T>> rest { get; set; }
    }

    public class Result<T>
    {
        public List<string> columns { get; set; }
        public List<Datum<T>> data { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class NeoRootObject<T>
    {
        public List<Result<T>> results { get; set; }
        public List<Error> errors { get; set; }
    }

}
