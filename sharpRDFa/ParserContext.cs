using System.Collections.Generic;
using sharpRDFa.RDF;

namespace sharpRDFa
{
    public class ParserContext
    {
        public string Base { get; set; }
        public string Language { get; set; }
        public List<IncompleteTriple> IncompleteTriples { get; set; }
        public IDictionary<string, string> UriMappings { get; set; }
        public string ParentSubject { get; set; }
        public ObjectNode ParentObject { get; set; }
    }
}