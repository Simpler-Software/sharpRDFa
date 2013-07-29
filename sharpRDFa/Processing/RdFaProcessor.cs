using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using sharpRDFa.Extension;
using sharpRDFa.RDFa;

namespace sharpRDFa.Processing
{
    public class RDFaProcessor : IRDFaProcessor
    {
        private const string TermRegexp = "^([a-zA-Z_])([0-9a-zA-Z_\\.-]*)$";
        private const string SafeCurieRegexp = "^\\[(.*)\\]$";

        public NameSpace IsNameSpace(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName)) return null;
            const string pattern = "^" + Constants.PrefixedAttNameRegex + "$";

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

        public CURIE IsCURIE(string input, string vocabulary, IDictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string curieExp = "^" + Constants.CurieRegex + "$";
            //const string curieExp = "^(\\w*?):(.*)$";
            var regExResult = MatchRegEx(input, curieExp);

            if ((regExResult != null) && (regExResult[2] != null) && (regExResult[2] != "_"))
            {
                if (uriMappings.IsNull() && vocabulary.IsNull()) return null;
                
                if (!uriMappings.ContainsKey(regExResult[1]) && vocabulary.IsNull()) return null;

                var result = new CURIE { Curie = regExResult[0] };

                if (regExResult[1] != null)
                    result.Prefix = regExResult[1];

                if (regExResult[2] != null)
                    result.Reference = regExResult[2];

                return result;

            }

            return null;
        }

        public CURIE IsSafeCURIE(string input, string vocabulary, IDictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string curieExp = "^\\[(" + Constants.CurieRegex + ")\\]$";
            var regExResult = MatchRegEx(input, curieExp);

            if (regExResult != null)
            {
                if ((regExResult[1] != null) && (regExResult[2] != null) && (regExResult[2] != "_"))
                {
                    if (uriMappings.IsNull() && vocabulary.IsNull()) return null;
                    if (!uriMappings.ContainsKey(regExResult[2]) && vocabulary.IsNull()) return null;
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
            const string uriExp = "^" + Constants.UriReferenceRegex + "$";
            var regExResult = MatchRegEx(input, uriExp);

            return regExResult != null ? regExResult[0] : null;
        }

        public URI GetURIParsed(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            const string uriExp = "^" + Constants.UriReferenceParsedRegex + "$";
            var regExResult = MatchRegEx(input, uriExp);

            if (regExResult != null)
            {
                var result = new URI { Uri = regExResult[0] };

                switch (regExResult.Count)
                {
                    case 2 :
                        if (regExResult[0].StartsWith("#")) result.Fragment = regExResult[1];
                        else result.Path = regExResult[1];
                        break;
                    case 4:
                        result.Scheme = regExResult[1];
                        result.Authority = regExResult[2];
                        result.Path = regExResult[3];
                        break; 
                    case 5:
                        result.Scheme = regExResult[1];
                        result.Authority = regExResult[2];
                        result.Path = regExResult[3];
                        result.Fragment = regExResult[4];
                        break;
                }

                return result;
            }

            return null;
        }

        public object IsUriOrSafeCurie(string input, string vocabulary, IDictionary<string, string> uriMappings, string attributeName)
        {
            if (input == null)
            {
                return null;
            }

            var resSafeCURIE = IsSafeCURIE(input, vocabulary, uriMappings);

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

            const string reservedWordExp = "^" + Constants.ReservedWordRegex + "$";
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

            var resURI = IsCURIE(input, null, uriMappings);

            if (resURI != null)
            {
                return resURI;
            }

            return null;
        }

        public IList<CURIE> GetCURIEs(string attributeValue, string vocabulary, IDictionary<string, string> uriMappings)
        {
            if (string.IsNullOrEmpty(attributeValue)) return null;

            string[] listOfAttributes = Regex.Split(attributeValue, @"/(?:$|^|\s+)/");

            IList<CURIE> result = listOfAttributes
                .Select(attrib => IsCURIE(attrib, vocabulary, uriMappings))
                .Where(curie => curie != null).ToList();

            if (result.Count > 0) return result;
            return null;
        }

        public string CURIEtoURI(string curie, IDictionary<string, string> uriMappings)
        {
            var resCurie = IsCURIE(curie, null, uriMappings);
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

        public string SafeCURIEtoURI(string aSafeCurie, string vocabulary, IDictionary<string, string> uriMappings)
        {
            var resSafeCurie = IsSafeCURIE(aSafeCurie, vocabulary, uriMappings);
            var resURI = "";

            if (resSafeCurie != null)
            {
                if (resSafeCurie.Prefix != null && uriMappings.ContainsKey(resSafeCurie.Prefix))
                {
                    resURI = resURI + uriMappings[resSafeCurie.Prefix];
                }
                /* default prefix mapping in RDFa */
                else if(vocabulary.IsNotNull())
                {
                    resURI = resURI + vocabulary;
                }
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

        public string ResolveSafeCURIE(string aCurie, string baseURI, string vocabulary, IDictionary<string, string> uriMappings)
        {
            var anURI = SafeCURIEtoURI(aCurie, vocabulary, uriMappings);
            return anURI != null ? ResolveURI(anURI, baseURI) : null;
        }

        public string ResolveURI(string anURI, string baseURI)
        {
            if (anURI == null) return null;

            var parsedURI = GetURIParsed(anURI);
            var parsedBaseURI = GetURIParsed(baseURI);

            if (parsedURI != null && parsedBaseURI != null)
            {
                if (parsedURI.Scheme == null && parsedURI.Authority == null && parsedURI.Path == null && parsedURI.Query == null && parsedURI.Fragment == null)
                    return parsedURI.Uri;

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
                        if (parsedURI.Path != null)
                        {
                            path = parsedURI.Path;
                            query = !string.IsNullOrEmpty(parsedURI.Query) ? parsedURI.Query : parsedBaseURI.Query;
                        }
                        else
                        {
                            path = parsedBaseURI.Path;
                            query = parsedBaseURI.Query;
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
            const string uriExp = "^" + Constants.URISchemaRegex + "$";
            if (uri == null) return null;
            var resUriExp = MatchRegEx(uri, uriExp);
            return resUriExp != null ? resUriExp[1] : null;
        }

        public string ExpandCurie(IDictionary<string, string> prefixMappings, string vocab, string value)
        {
            IList<string> matches = MatchRegEx(value, "^(\\w*?):(.*)$");
            if (matches.IsNull()) return null;
            
            string prefix = matches[1];
            string referrence = matches[2];

            if(string.Equals(prefix, "_"))
            {
                // It is a bnode
                // return $this->remapBnode(substr($value, 2));
            }
            else if (prefix.IsNullOrEmpty() && !vocab.IsNullOrEmpty()) {
                // Empty prefix
                return vocab + referrence;
            } 
            else if (prefixMappings.ContainsKey(prefix)) {
                return prefixMappings[prefix] + referrence;
            } 
            else if (!prefix.IsNullOrEmpty() && Vocabulary.Instance.KnownPrefixes.ContainsKey(prefix)) {
                // Expand using well-known prefixes
                return Vocabulary.Instance.KnownPrefixes[prefix] + referrence;
            }
            return null;
        }

        // todo need to rewrite this method
        public string ResolveURI(IDictionary<string, string> prefixMappings, string vocab, IDictionary<string, string> terms, string value)
        {
            if (Regex.IsMatch(value, SafeCurieRegexp))
            {
                // Safe CURIE
                IList<string> matches = MatchRegEx(value,SafeCurieRegexp);
                return ExpandCurie(prefixMappings, vocab, matches[1]);
            }
            if(Regex.IsMatch(value, TermRegexp))
            {
                if(!vocab.IsNullOrEmpty()) return vocab + value;
                if(terms.IsNotNull() && terms.ContainsKey(value)) return terms[value];
                return prefixMappings[Constants.DefaultPrefix] + value;
            }
            if(string.Equals(value.Substring(0, 2), "_:"))
            {
                return null;
            }
            else
            {
                string uri = ExpandCurie(prefixMappings, vocab, value);
                if (!uri.IsNullOrEmpty()) return uri;

                // todo
                /*else {
                $parsed = new EasyRdf_ParsedUri($value);
                if ($parsed->isAbsolute()) {
                    return $value;
                } elseif ($isProp) {
                    // Properties can't be relative URIs
                    return null;
                } elseif ($this->baseUri) {
                    return $this->baseUri->resolve($parsed);
                }
                }*/
            }

            return value;
        }

        public IList<string> ProcessUriList(string typedResource, string defaultVocabulary, IDictionary<string, string> prefixMappings)
        {
            var result = new List<string>();
            if (typedResource.IsNullOrEmpty()) return result;
            var items = Regex.Split(typedResource, "\\s+");
            foreach(var item in items)
            {
                var uri = ResolveURI(prefixMappings, defaultVocabulary, null, item);
                if(!uri.IsNullOrEmpty()) result.Add(uri);
            }

            return result;
        }

        private static string RecomposeURIComponents(string scheme, string authority, string path, string query, string fragment)
        {
            var result = "";

            if (scheme.IsNotNull())
                result = result + scheme + ":";
            
            if (authority.IsNotNull() && scheme.ToLower() != "urn")
                result = result + "//" + authority;
            
            if (authority.IsNotNull() && scheme.ToLower() == "urn")
                result = result + authority;

            if (path != null)
                result = result + path;

            if (query != null)
                result = result + "?" + query;

            if (fragment != null)
                result = result + "#" + fragment;

            return result;
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