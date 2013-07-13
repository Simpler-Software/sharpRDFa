using System.Collections.Generic;

namespace sharpRDFa.RDF
{
    public interface IRDFTripleBuilder
    {
        void CreateSubject(string resource, string baseURL, IDictionary<string, string> uriMappings);
        void CreatePredicate(string property, string baseURL, IDictionary<string, string> uriMappings);
        void CreateObject(string objectValue, string language, string dataType, TripleObjectType type, string baseURL, IDictionary<string, string> uriMappings);
        RDFTriple GetTriple();
    }
}
