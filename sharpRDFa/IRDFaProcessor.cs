using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpRDFa
{
    public interface IRDFaProcessor
    {
        NameSpace IsNameSpace(string attributeName);
        List<string> MatchRegEx(string attributeValue, string pattern);
        CURIE IsCURIE(string attributeValue, Dictionary<string, string> uriMappings);
        CURIE IsSafeCURIE(string attributeValue, Dictionary<string, string> uriMappings);
        string IsURI(string attributeValue, string attributeName);
        URI GetURIParsed(string attributeValue);
        object IsUriOrSafeCurie(string attributeValue, Dictionary<string, string> uriMappings, string attributeName);
        string IsReservedWord(string attributeValue);
        object IsReservedWordOrCurie(string attributeValue, Dictionary<string, string> uriMappings);
        IList<CURIE> GetCURIEs(string attributeValue, Dictionary<string, string> uriMappings);
        string CURIEtoURI(string curie, Dictionary<string, string> uriMappings);
        string SafeCURIEtoURI(string curie, Dictionary<string, string> uriMappings);
        string ResolveCURIE(string curie, string baseURI, Dictionary<string, string> uriMappings);
        string ResolveSafeCURIE(string curie, string baseURI, Dictionary<string, string> uriMappings);
    }
}
