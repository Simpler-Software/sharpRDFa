using System.Collections.Generic;

namespace sharpRDFa.RDFTriple
{
    public interface IRDFTripleBuilder
    {
        void CreateSubject(string resource, string baseURL, Dictionary<string, string> uriMappings);
        void CreatePredicate(string property, string baseURL, Dictionary<string, string> uriMappings);
        void CreateObject(string objectValue, string language, string dataType, TripleObjectType type, string baseURL, Dictionary<string, string> uriMappings);
        RDFTriple GetTriple();
    }
}
