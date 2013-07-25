using System.Collections.Generic;
using sharpRDFa.RDF;
using sharpRDFa.RDFa;

namespace sharpRDFa
{
    public class ParserContext
    {
        public string Base { get; set; }
        public string Language { get; set; }
        //public List<IncompleteTriple> IncompleteTriples { get; set; }
        public IDictionary<string, string> PrefixMappings { get; set; }
        public string ParentSubject { get; set; }
        public string ParentObject { get; set; }
        public string DefaultVocabulary { get; set; }
        public IList<string> IncompleteRels { get; set; }
        public IList<string> IncompleteRevs { get; set; }
        public List<RDFTriple> ConstructedTiples { get; set; }
        public IDictionary<string, string> Terms { get; set; }
        public IDictionary<string, IList<string>> ListMapping { get; set; }
    }
}