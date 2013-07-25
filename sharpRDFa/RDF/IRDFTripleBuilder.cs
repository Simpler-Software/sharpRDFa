using System.Collections.Generic;

namespace sharpRDFa.RDF
{
    public interface IRDFTripleBuilder
    {
        void CreateSubject(string resource, string baseURL, string vocabulary, IDictionary<string, string> uriMappings);
        void CreatePredicate(string property, string baseURL, string vocabulary, IDictionary<string, string> prefixMappings);
        void CreateObject(string objectValue, string language, string dataType, string baseURL, string vocabulary, IDictionary<string, string> uriMappings);
        RDFTriple GetTriple();
    }

    class RDFTripleBuilderNoValidation : IRDFTripleBuilder
    {
        private readonly RDFTriple _rdfTriple;

        public RDFTripleBuilderNoValidation()
        {
            _rdfTriple = new RDFTriple();
        }

        public void CreateSubject(string resource, string baseURL, string vocabulary, IDictionary<string, string> uriMappings)
        {
            _rdfTriple.Subject = resource;
        }

        public void CreatePredicate(string property, string baseURL, string vocabulary, IDictionary<string, string> prefixMappings)
        {
            _rdfTriple.Predicate = property;
        }

        public void CreateObject(string objectValue, string language, string dataType, string baseURL, string vocabulary, IDictionary<string, string> uriMappings)
        {
            _rdfTriple.Object = new ObjectNode
                                    {
                                        DataType =  dataType,
                                        Language = language
                                    };

            if (dataType == Constants.UriDataType) _rdfTriple.Object.Uri = objectValue;
            else if (dataType == Constants.LiteralDataType) _rdfTriple.Object.Literal = objectValue;
        }

        public RDFTriple GetTriple()
        {
            return _rdfTriple;
        }
    }
}
