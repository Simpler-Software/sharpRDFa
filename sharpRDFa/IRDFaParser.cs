using System.Collections.Generic;
using HtmlAgilityPack;
using sharpRDFa.RDF;

namespace sharpRDFa
{
    public interface IRDFaParser
    {
        IList<RDFTriple> ParseRDFTriplesFromURL(string url);
        IList<RDFTriple> ParseRDFTriplesFromFile(string filePath);
        IList<RDFTriple> ParseRDFTriples(HtmlDocument document);
    }
}
