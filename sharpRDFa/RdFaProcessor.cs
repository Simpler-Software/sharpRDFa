using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace sharpRDFa
{
    public class RDFaProcessor : IRDFaProcessor
    {
        public NameSpace IsNameSpace(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName)) return null;
            const string pattern = "^" + Constants.PrefixedAttName + "$";

            var regExResult = MatchRegEx(attributeName, pattern);

            if (regExResult != null)
            {
                var result = new NameSpace { Prefix = "xmlns", NCName = regExResult[1] };
                return result;
            }

            return null;
        }

        public List<string> MatchRegEx(string input, string pattern)
        {
            if (!Regex.IsMatch(input, pattern)) return null;
            var matches = Regex.Split(input, pattern).Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (!matches.Any()) matches = new List<string> { Regex.Match(input, pattern).Value };
            if (!matches.Any()) return null;
            var result = new List<string> { input };
            result.AddRange(matches);
            return result;
        }

        public CURIE IsCURIE(string input, Dictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string curieExp = "^" + Constants.Curie + "$";
            var regExResult = MatchRegEx(input, curieExp);

            if ((regExResult != null) && (regExResult[2] != null) && (regExResult[2] != "_"))
            {
                if (uriMappings == null) return null;
                if (uriMappings[regExResult[1]] == null) return null;

                var result = new CURIE { Curie = regExResult[0] };

                if (regExResult[1] != null)
                    result.Prefix = regExResult[1];

                if (regExResult[2] != null)
                    result.Reference = regExResult[2];

                return result;

            }

            return null;
        }

        public CURIE IsSafeCURIE(string input, Dictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string curieExp = "^\\[(" + Constants.Curie + ")\\]$";
            var regExResult = MatchRegEx(input, curieExp);

            if (regExResult != null)
            {
                if ((regExResult[1] != null) && (regExResult[2] != null) && (regExResult[2] != "_"))
                {
                    if (uriMappings == null) return null;
                    if (uriMappings[regExResult[2]] == null) return null;
                }

                var result = new CURIE { Curie = regExResult[1] };

                if (regExResult[2] != null)
                    result.Prefix = regExResult[2];

                if (regExResult[3] != null)
                    result.Reference = regExResult[3];

                return result;

            }

            return null;
        }

        public string IsURI(string input, string attributeName)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string uriExp = "^" + Constants.UriReference + "$";
            var regExResult = MatchRegEx(input, uriExp);

            return regExResult != null ? regExResult[0] : null;
        }

        public URI GetURIParsed(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string uriExp = "^" + Constants.UriReferenceParsed + "$";
            var regExResult = MatchRegEx(input, uriExp);

            if (regExResult != null)
            {
                var result = new URI { Uri = regExResult[0] };

                if (regExResult.Count > 1)
                    result.Scheme = regExResult[1];

                if (regExResult.Count > 2)
                    result.Authority = regExResult[2];

                if (regExResult.Count > 3)
                    result.Path = regExResult[3];

                if (regExResult.Count > 4)
                    result.Query = regExResult[4];

                if (regExResult.Count > 5)
                    result.Fragment = regExResult[5];

                return result;
            }

            return null;
        }

        public object IsUriOrSafeCurie(string input, Dictionary<string, string> uriMappings, string attributeName)
        {
            if (input == null)
            {
                return null;
            }

            var resSafeCURIE = IsSafeCURIE(input, uriMappings);

            if (resSafeCURIE != null)
            {
                return resSafeCURIE;
            }

            var resURI = IsURI(input, attributeName);

            if (resURI != null)
            {
                return resURI;
            }

            return null;
        }

        public string IsReservedWord(string input)
        {
            if (input == null)
                return null;

            const string reservedWordExp = "^" + Constants.ReservedWord + "$";
            var resReservedWordExp = MatchRegEx(input, reservedWordExp);

            if (resReservedWordExp != null)
            {
                return resReservedWordExp[0];
            }

            return null;
        }

        public object IsReservedWordOrCurie(string input, Dictionary<string, string> uriMappings)
        {
            if (input == null)
            {
                return null;
            }

            var resReservedWord = IsReservedWord(input);

            if (resReservedWord != null)
            {
                return resReservedWord;
            }

            var resURI = IsCURIE(input, uriMappings);

            if (resURI != null)
            {
                return resURI;
            }

            return null;
        }

        public void GetCURIEs(string input, Dictionary<string, string> uriMappings)
        {

        }

    }
}