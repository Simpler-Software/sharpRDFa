namespace sharpRDFa.RDF
{
    public class ObjectNode
    {
        public string Curie { get; set; }
        public string Uri { get; set; }
        public string UriSchema { get; set; }
        public string Literal { get; set; }
        public string Language { get; set; }
        public string DataType { get; set; }

        public string Value { get; set; }
    }
}