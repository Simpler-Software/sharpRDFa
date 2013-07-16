using System.Collections.Generic;

namespace sharpRDFa.RDF
{
    public interface IRDFTripleBuilder
    {
        void CreateSubject(string resource, string baseURL, string vocabulary, IDictionary<string, string> uriMappings);
        void CreatePredicate(string property, string baseURL, string vocabulary, IDictionary<string, string> uriMappings);
        void CreateObject(string objectValue, string language, string dataType, TripleObjectType type, string baseURL, string vocabulary, IDictionary<string, string> uriMappings);
        RDFTriple GetTriple();
    }
}
