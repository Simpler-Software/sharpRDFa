using System.Collections.Generic;
using HtmlAgilityPack;
using sharpRDFa.RDF;

namespace sharpRDFa
{
    public interface IRDFaParser
    {
        IList<RDFTriple> GetRDFTriplesFromURL(string url);
        IList<RDFTriple> GetRDFTriplesFromFile(string filePath);
        IList<RDFTriple> GetRDFTriples(HtmlDocument document);
    }
}
