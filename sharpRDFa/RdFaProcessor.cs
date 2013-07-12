using System;
using System.Collections.Generic;

namespace sharpRDFa
{
    class RdFaProcessor : IRDFaProcessor
    {
        public NameSpace IsNameSpace(string attributeName)
        {
            throw new NotImplementedException();
        }

        public List<string> MatchRegEx(string attributeValue, string pattern)
        {
            throw new NotImplementedException();
        }

        public CURIE IsCURIE(string attributeValue, Dictionary<string, string> uriMappings)
        {
            throw new NotImplementedException();
        }

        public CURIE IsSafeCURIE(string attributeValue, Dictionary<string, string> uriMappings)
        {
            throw new NotImplementedException();
        }

        public string IsURI(string attributeValue, string attributeName)
        {
            throw new NotImplementedException();
        }

        public URI GetURIParsed(string attributeValue)
        {
            throw new NotImplementedException();
        }

        public object IsUriOrSafeCurie(string attributeValue, Dictionary<string, string> uriMappings, string attributeName)
        {
            throw new NotImplementedException();
        }

        public string IsReservedWord(string attributeValue)
        {
            throw new NotImplementedException();
        }

        public object IsReservedWordOrCurie(string attributeValue, Dictionary<string, string> uriMappings)
        {
            throw new NotImplementedException();
        }

        public void GetCURIEs(string attributeValue, Dictionary<string, string> uriMappings)
        {
            throw new NotImplementedException();
        }
    }
}