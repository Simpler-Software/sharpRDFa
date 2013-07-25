using System.Collections.Generic;
using VDS.RDF;
using VDS.RDF.Writing;
using sharpRDFa.Extension;
using sharpRDFa.RDF;


namespace sharpRDFa
{
    public class RDFWriter : IRDFWriter
    {
        public void WriteToXML(IList<RDFTriple> triples, string outputFile)
        {
            Graph graph = CreateGraph(triples);
            var rdfxmlwriter = new RdfXmlWriter();
            rdfxmlwriter.Save(graph, outputFile);
        }

        public void WriteToNTriples(IList<RDFTriple> triples, string outputFile)
        {
            Graph graph = CreateGraph(triples);
            var ntwriter = new NTriplesWriter();
            ntwriter.Save(graph, outputFile);
        }

        private Graph CreateGraph(IEnumerable<RDFTriple> triples)
        {
            var g = new Graph();
            foreach (var triple in triples)
            {
                if(triple.Subject.IsNull() || triple.Predicate.IsNull() || triple.Object.IsNull()) continue;
                IUriNode subject = g.CreateUriNode(UriFactory.Create(triple.Subject)); 
                IUriNode predicate = g.CreateUriNode(UriFactory.Create(triple.Predicate));

                switch (triple.Object.DataType)
                {
                    case Constants.UriDataType:
                        IUriNode objectUri = g.CreateUriNode(UriFactory.Create(triple.Object.Uri));
                        g.Assert(new Triple(subject, predicate, objectUri));
                        break;
                    case Constants.LiteralDataType:
                        ILiteralNode objecto = g.CreateLiteralNode(triple.Object.Literal);
                        g.Assert(new Triple(subject, predicate, objecto));
                        break;
                }
            }

            return g;
        }

        
    }
}