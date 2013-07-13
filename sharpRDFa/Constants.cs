using System.Collections.Generic;

namespace sharpRDFa
{
    public class Constants
    {
        // about – a URI or CURIE specifying the resource the metadata is about
        // rel and rev – specifying a relationship and reverse-relationship with another resource, respectively
        // src, href and resource – specifying the partner resource
        // property – specifying a property for the content of an element or the partner resource
        // content – optional attribute that overrides the content of the element when using the property attribute
        // datatype – optional attribute that specifies the datatype of text specified for use with the property attribute
        // typeof – optional attribute that specifies the RDF type(s) of the subject or the partner resource (the resource that the metadata is about).
        public static IList<string> RDFAttributes = new List<string>
                                                        {
                                                            "about",
                                                            "rel",
                                                            "rev",
                                                            "src",
                                                            "href",
                                                            "resource",
                                                            "property",
                                                            "content",
                                                            "typeof"
                                                        };


        /* Declaring Namespaces: http://www.w3.org/TR/1999/REC-xml-names-19990114/#ns-decl */
        public const string CombiningChar = "[\\u0300-\\u0345]"
                                        + "[\\u0360-\\u0361] | "
                                        + "[\\u0483-\\u0486] | "
                                        + "[\\u0591-\\u05A1] | "
                                        + "[\\u05A3-\\u05B9] | "
                                        + "[\\u05BB-\\u05BD] | "
                                        + "\\u05BF | "
                                        + "[\\u05C1-\\u05C2] | "
                                        + "[\\u05C4 | "
                                        + "[\\u064B-\\u0652] | "
                                        + "\\u0670 | "
                                        + "[\\u06D6-\\u06DC] | "
                                        + "[\\u06DD-\\u06DF] | "
                                        + "[\\u06E0-\\u06E4] | "
                                        + "[\\u06E7-\\u06E8] | "
                                        + "[\\u06EA-\\u06ED] | "
                                        + "[\\u0901-\\u0903] | "
                                        + "\\u093C | "
                                        + "[\\u093E-\\u094C] | "
                                        + "\\u094D | "
                                        + "[\\u0951-\\u0954] | "
                                        + "[\\u0962-\\u0963] | "
                                        + "[\\u0981-\\u0983] | "
                                        + "\\u09BC | "
                                        + "\\u09BE | "
                                        + "\\u09BF | "
                                        + "[\\u09C0-\\u09C4] | "
                                        + "[\\u09C7-\\u09C8] | "
                                        + "[\\u09CB-\\u09CD] | "
                                        + "\\u09D7 | "
                                        + "[\\u09E2-\\u09E3] | "
                                        + "\\u0A02 | "
                                        + "\\u0A3C | "
                                        + "\\u0A3E | "
                                        + "\\u0A3F | "
                                        + "[\\u0A40-\\u0A42] | "
                                        + "[\\u0A47-\\u0A48] | "
                                        + "[\\u0A4B-\\u0A4D] | "
                                        + "[\\u0A70-\\u0A71] | "
                                        + "[\\u0A81-\\u0A83] | "
                                        + "\\u0ABC | "
                                        + "[\\u0ABE-\\u0AC5] | "
                                        + "[\\u0AC7-\\u0AC9] | "
                                        + "[\\u0ACB-\\u0ACD] | "
                                        + "[\\u0B01-\\u0B03] | "
                                        + "\\u0B3C | "
                                        + "[\\u0B3E-\\u0B43] | "
                                        + "[\\u0B47-\\u0B48] | "
                                        + "[\\u0B4B-\\u0B4D] | "
                                        + "[\\u0B56-\\u0B57] | "
                                        + "[\\u0B82-\\u0B83] | "
                                        + "[\\u0BBE-\\u0BC2] | "
                                        + "[\\u0BC6-\\u0BC8] | "
                                        + "[\\u0BCA-\\u0BCD] | "
                                        + "\\u0BD7 | "
                                        + "[\\u0C01-\\u0C03] | "
                                        + "[\\u0C3E-\\u0C44] | "
                                        + "[\\u0C46-\\u0C48] | "
                                        + "[\\u0C4A-\\u0C4D] | "
                                        + "[\\u0C55-\\u0C56] | "
                                        + "[\\u0C82-\\u0C83] | "
                                        + "[\\u0CBE-\\u0CC4] | "
                                        + "[\\u0CC6-\\u0CC8] | "
                                        + "[\\u0CCA-\\u0CCD] | "
                                        + "[\\u0CD5-\\u0CD6] | "
                                        + "[\\u0D02-\\u0D03] | "
                                        + "[\\u0D3E-\\u0D43] | "
                                        + "[\\u0D46-\\u0D48] | "
                                        + "[\\u0D4A-\\u0D4D] | "
                                        + "\\u0D57 | "
                                        + "\\u0E31 | "
                                        + "[\\u0E34-\\u0E3A] | "
                                        + "[\\u0E47-\\u0E4E] | "
                                        + "\\u0EB1 | "
                                        + "[\\u0EB4-\\u0EB9] | "
                                        + "[\\u0EBB-\\u0EBC] | "
                                        + "[\\u0EC8-\\u0ECD] | "
                                        + "[\\u0F18-\\u0F19] | "
                                        + "\\u0F35 | "
                                        + "\\u0F37 | "
                                        + "\\u0F39 | "
                                        + "\\u0F3E | "
                                        + "\\u0F3F | "
                                        + "[\\u0F71-\\u0F84] | "
                                        + "[\\u0F86-\\u0F8B] | "
                                        + "[\\u0F90-\\u0F95] | "
                                        + "\\u0F97 | "
                                        + "[\\u0F99-\\u0FAD] | "
                                        + "[\\u0FB1-\\u0FB7] | "
                                        + "\\u0FB9 | "
                                        + "[\\u20D0-\\u20DC] | "
                                        + "\\u20E1 | "
                                        + "[\\u302A-\\u302F] | "
                                        + "\\u3099 | "
                                        + "\\u309A";

        public const string Extender = "\\u00B7 | "
                                        + "\\u02D0 | "
                                        + "\\u02D1 | "
                                        + "\\u0387 | "
                                        + "\\u0640 | "
                                        + "\\u0E46 | "
                                        + "\\u0EC6 | "
                                        + "\\u3005 | "
                                        + "[\\u3031-\\u3035] | "
                                        + "[\\u309D-\\u309E] | "
                                        + "[\\u30FC-\\u30FE]   ";

        public const string NCNameChar = "(?:\\w|\\d|\\.|-|_" + CombiningChar + "|" + Extender + ")";
        public const string NCName = "(?:\\w|_)(?:" + NCNameChar + ")*";
        public const string PrefixedAttName = "xmlns:(" + NCName + ")";

        /* Uniform Resource Identifiers (URI): http://www.ietf.org/rfc/rfc2396.txt */
        /* '[' and ']' characters are excluded to distingish between safe curies and uris */
        public const string UriReference = "(?:(?:[^\\[\\]\\:\\/\\?\\#]+):)?(?:\\/\\/(?:[^\\[\\]\\/\\?\\#]*))?(?:[^\\[\\]\\?\\#]*)(?:\\?(?:[^\\[\\]\\#]*))?(?:\\#(?:[^\\[\\]]*))?";
        public const string UriReferenceParsed = "(?:([^\\[\\]\\:\\/\\?\\#]+):)?(?:\\/\\/([^\\[\\]\\/\\?\\#]*))?([^\\[\\]\\?\\#]*)(?:\\?([^\\[\\]\\#]*))?(?:\\#([^\\[\\]]*))?";
        public const string URISchema   = @"(?:([^\\:\\/\\?\\#]+):)?(?:\\/\\/(?:[^\\/\\?\#]*))?(?:[^\\?\\#]*)(?:\\?(?:[^\\#]*))?(?:\\#(?:.*))?";
        public const string ErrorHtmlAttributeNull = "HtmlAttribute can be null";

        public const string BnodePrefix = "rdfadevBnode";
        public const string EmptyBnodePrefix = "rdfadevBnodeEmpty";

        /* Internationalized Resource Identifiers (IRI): http://www.ietf.org/rfc/rfc3987.txt */
        public const string IrelativeRef = "(?:\\/\\/(?:[^\\/\\?\\#]*))?(?:[^\\?\\#]*)(?:\\?(?:[^\\#]*))?(?:\\#(?:.*))?";

        /* CURIE Syntax Definition: http://www.w3.org/TR/rdfa-syntax/#s_curies */
        public const string Prefix = NCName;
        public const string Reference = IrelativeRef;
        public const string Curie = "(?:(" + Prefix + ")" + "?:)" + "(" + Reference + ")";

        // list of reserved values for @rel and @rev
        public const string ReservedWord = "(?:alternate|appendix|bookmark|cite|chapter|contents|copyright|first|glossary|help|icon|index|last|license|meta|next|p3pv1|prev|role|section|stylesheet|subsection|start|top|up)";

    }
}
