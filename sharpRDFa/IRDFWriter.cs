using System.Collections.Generic;
using sharpRDFa.RDF;

namespace sharpRDFa
{
    public interface IRDFWriter
    {
        void WriteToXML(IList<RDFTriple> triples, string outputFile);
        void WriteToNTriples(IList<RDFTriple> triples, string outputFile);
    }
}