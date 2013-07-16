using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using HtmlAgilityPack;
using sharpRDFa.Processing;
using sharpRDFa.RDFa;

namespace sharpRDFa.Extension
{
    public static class HtmlAttributeExtensions
    {
        private static readonly IRDFaProcessor Processor = new RDFaProcessor();

        public static NameSpace IsNamespace(this HtmlAttribute attribute)
        {
            if (attribute.IsNull()) return null;
            return Processor.IsNameSpace(attribute.Name);
        }

        public static object IsUriOrSafeCurie(this HtmlAttribute attribute, string vocabulary, IDictionary<string, string> uriMappings)
        {
            if (attribute.IsNull()) return null;
            return Processor.IsUriOrSafeCurie(attribute.Value, vocabulary, uriMappings, attribute.Name);
        }

        public static CURIE IsSafeCurie(this HtmlAttribute attribute, string vocabulary, IDictionary<string, string> uriMappings)
        {
            if (attribute.IsNull()) return null;
            return Processor.IsSafeCURIE(attribute.Value, vocabulary, uriMappings);
        }

        public static CURIE IsCurie(this HtmlAttribute attribute, IDictionary<string, string> uriMappings)
        {
            if (attribute.IsNull()) return null;
            return Processor.IsCURIE(attribute.Value, null, uriMappings);
        }

        public static string IsUri(this HtmlAttribute attribute)
        {
            if (attribute.IsNull()) return null;
            return Processor.IsURI(attribute.Value, attribute.Name);
        }

        public static IList<CURIE> GetCURIEs(this HtmlAttribute attribute, string vocabulary, IDictionary<string, string> mappings)
        {
            if (attribute.IsNull()) return null;
            return Processor.GetCURIEs(attribute.Value, vocabulary, mappings);
        }

        private static CURIE GetCURIE(string attributeValue, IDictionary<string, string> mappings)
        {
            return Processor.IsCURIE(attributeValue, null, mappings);
        }

        public static string ResolveCURIE(this HtmlAttribute attribute, string baseUri, IDictionary<string, string> mappings)
        {
            if (attribute.IsNull()) return null;
            return Processor.ResolveCURIE(attribute.Value, baseUri, mappings);
        }

        public static string CURIEtoURI(this HtmlAttribute attribute, IDictionary<string, string> mappings)
        {
            if (attribute.IsNull()) return null;
            return Processor.CURIEtoURI(attribute.Value, mappings);
        }

        public static string GetReservedWordOrCURIE(this HtmlAttribute attribute, IDictionary<string, string> mappings)
        {
            if (attribute.IsNull()) return null;
            var result =  Processor.IsReservedWordOrCurie(attribute.Value, mappings);
            if(result is CURIE)
            {
                return (result as CURIE).Curie;
            }
            if(result is string)
            {
                return (result as string);
            }

            return null;
        }
    }
}
