using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using sharpRDFa.RDFa;

namespace sharpRDFa.Processing
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

        public CURIE IsCURIE(string input, IDictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string curieExp = "^" + Constants.Curie + "$";
            //const string curieExp = "^(\\w*?):(.*)$";
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

        public CURIE IsSafeCURIE(string input, IDictionary<string, string> uriMappings)
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

                //if (regExResult.Count > 4)
                //    result.Query = regExResult[4];

                if (regExResult.Count > 4)
                    result.Fragment = regExResult[4];

                return result;
            }

            return null;
        }

        public object IsUriOrSafeCurie(string input, IDictionary<string, string> uriMappings, string attributeName)
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

        public object IsReservedWordOrCurie(string input, IDictionary<string, string> uriMappings)
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

        public IList<CURIE> GetCURIEs(string attributeValue, IDictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(attributeValue)) return null;

            string[] listOfAttributes = Regex.Split(attributeValue, @"/(?:$|^|\s+)/");

            IList<CURIE> result = listOfAttributes
                .Select(attrib => IsCURIE(attrib, uriMappings))
                .Where(curie => curie != null).ToList();

            if (result.Count > 0) return result;
            return null;
        }

        public string CURIEtoURI(string curie, IDictionary<string, string> uriMappings)
        {
            var resCurie = IsCURIE(curie, uriMappings);
            var resURI = "";

            if (resCurie != null)
            {

                if ((resCurie.Prefix != null) && ((uriMappings == null) || (uriMappings[resCurie.Prefix] == null)))
                    return null;

                if (resCurie.Prefix != null)
                {
                    resURI = resURI + uriMappings[resCurie.Prefix];
                }
                /* default prefix mapping in RDFa */
                else
                {
                    resURI = resURI + "http://www.w3.org/1999/xhtml/vocab#";
                }

                if (resCurie.Reference != null)
                    resURI = resURI + resCurie.Reference;

                return resURI;

            }

            return null;
        }

        public string SafeCURIEtoURI(string aSafeCurie, IDictionary<string, string> uriMappings)
        {
            var resSafeCurie = IsSafeCURIE(aSafeCurie, uriMappings);
            var resURI = "";

            if (resSafeCurie != null)
            {

                if ((resSafeCurie.Prefix != null) && ((uriMappings == null) || (uriMappings[resSafeCurie.Prefix] == null)))
                    return null;

                if (resSafeCurie.Prefix != null)
                {
                    resURI = resURI + uriMappings[resSafeCurie.Prefix];
                }
                /* default prefix mapping in RDFa */
                else
                {
                    resURI = resURI + "http://www.w3.org/1999/xhtml/vocab#";
                }

                if (resSafeCurie.Reference != null)
                    resURI = resURI + resSafeCurie.Reference;

                return resURI;

            }

            return null;
        }

        public string ResolveCURIE(string aCurie, string baseURI, IDictionary<string, string> uriMappings)
        {
            var anURI = CURIEtoURI(aCurie, uriMappings);
            return anURI != null ? ResolveURI(anURI, baseURI) : null;
        }

        public string ResolveSafeCURIE(string aCurie, string baseURI, IDictionary<string, string> uriMappings)
        {
            var anURI = SafeCURIEtoURI(aCurie, uriMappings);
            return anURI != null ? ResolveURI(anURI, baseURI) : null;
        }

        public string ResolveURI(string anURI, string baseURI)
        {
            if (anURI == null) return null;

            var parsedURI = GetURIParsed(anURI);
            var parsedBaseURI = GetURIParsed(baseURI);

            if (parsedURI != null && parsedBaseURI != null)
            {
                if (parsedURI.Scheme == parsedBaseURI.Scheme)
                    parsedURI.Scheme = null;

                string scheme;
                string authority;
                string path;
                string query;
                if (parsedURI.Scheme != null)
                {
                    scheme = parsedURI.Scheme;
                    authority = parsedURI.Authority;
                    path = RemoveDotSegments(parsedURI.Path);
                    query = parsedURI.Query;
                }
                else
                {
                    if (parsedURI.Authority != null)
                    {
                        authority = parsedURI.Authority;
                        path = RemoveDotSegments(parsedURI.Path);
                        query = parsedURI.Query;
                    }
                    else
                    {
                        if (parsedURI.Path == "")
                        {
                            path = parsedBaseURI.Path;
                            query = !string.IsNullOrEmpty(parsedURI.Query) ? parsedURI.Query : parsedBaseURI.Query;
                        }
                        else
                        {
                            if (parsedURI.Path.ToCharArray()[0] == '/')
                            {
                                path = RemoveDotSegments(parsedURI.Path);
                            }
                            else
                            {
                                path = MergePaths(parsedBaseURI.Path, parsedURI.Path);
                                path = RemoveDotSegments(path);
                            }
                            query = parsedURI.Query;
                        }
                        authority = parsedBaseURI.Authority;
                    }
                    scheme = parsedBaseURI.Scheme;
                }
                string fragment = parsedURI.Fragment;

                return RecomposeURIComponents(scheme, authority, path, query, fragment);

            }

            return null;
        }

        public string GetURISchema(string uri)
        {
            const string uriExp = "^" + Constants.URISchema + "$";
            if (uri == null) return null;
            var resUriExp = MatchRegEx(uri, uriExp);
            return resUriExp != null ? resUriExp[1] : null;
        }

        private static string RecomposeURIComponents(string scheme, string authority, string path, string query, string fragment)
        {
            var result = "";

            if (scheme != null)
                result = result + scheme + ":";

            if (authority != null)
                result = result + "//" + authority;

            if (path != null)
                result = result + path;

            if (query != null)
                result = result + "?" + query;

            if (fragment != null)
                result = result + "#" + fragment;

            return result;
        }

        private static string MergePaths(string path1, string path2)
        {
            if (path1 == "")
            {
                return "/" + path2;
            }

            return Regex.Replace(path1, @"/[^\/]*$/", "") + path2;
        }

        private static string RemoveDotSegments(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            var result = path;
            string previousResult;

            do
            {
                previousResult = result;
                result = Regex.Replace(result, @"/^(\.\.\/|\.\/)/", "");
            } while (previousResult != result);

            result = Regex.Replace(result, @"/(\/\.\/|\/\.$)/g", "/");

            do
            {
                previousResult = result;
                result = Regex.Replace(result, @"/(\/?[^\/]*)?(\/\.\.\/|\/\.\.$)/", "/");
            } while (previousResult != result);

            result = Regex.Replace(result, @"/^(\.\.|\.)$/g", "");

            return result;
        }
    }
}