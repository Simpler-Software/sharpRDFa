using System.Collections.Generic;

namespace sharpRDFa
{
    public class Constants
    {
        /* a SafeCURIEorCURIEorIRI, used for stating what
         * the data is about (a 'subject' in RDF terminology) */
        public const string About_RDFaAttribute = "about";

        /* a CDATA string, for supplying machine-readable
         * content for a literal (a 'literal object', in RDF terminology) */
        public const string Content_RDFaAttribute = "content";

        /* a TERMorCURIEorAbsIRI representing a datatype,
         * to express the datatype of a literal */
        public const string DataType_RDFaAttribute = "datatype";

        /* a traditionally navigable IRI for expressing
         * the partner resource of a relationship (a 'resource object', in RDF terminology) */
        public const string Href_RDFaAttribute = "href";

        /* An attribute used to indicate that the object associated 
         * with a rel or property attribute on the same element is to be added to 
         * the list for that predicate. The value of this attribute must be ignored. 
         * Presence of this attribute causes a list to be created if it does not already exist. */          
        public const string Inlist_RDFaAttribute = "inlist";

        /* a white space separated list of prefix-name IRI pairs of the form 
         * NCName ':' ' '+ xsd:anyURI */
        public const string Prefix_RDFaAttribute = "prefix";

        /* a white space separated list of TERMorCURIEorAbsIRIs,
         * used for expressing relationships between a subject 
         * and either a resource object if given or some literal text (also a 'predicate') */
        public const string Property_RDFaAttribute = "property";

        /* a white space separated list of TERMorCURIEorAbsIRIs, 
         * used for expressing relationships between two resources ('predicates' in RDF terminology); */
        public const string Rel_RDFaAttribute = "rel";

        /* a SafeCURIEorCURIEorIRI for expressing the partner resource of a relationship 
         * that is not intended to be navigable (e.g., a 'clickable' link) (also an 'object'); */
        public const string Resource_RDFaAttribute = "resource";

        /* a white space separated list of TERMorCURIEorAbsIRIs, used for expressing
         * reverse relationships between two resources (also 'predicates');*/
        public const string Rev_RDFaAttribute = "rev";

        /* an IRI for expressing the partner resource of a relationship 
         * when the resource is embedded (also a 'resource object'); */
        public const string Src_RDFaAttribute = "src";

        /* a white space separated list of TERMorCURIEorAbsIRIs 
         * that indicate the RDF type(s) to associate with a subject; */
        public const string TypeOf_RDFaAttribute = "typeof";

        /* an IRI that defines the mapping to use when a TERM is referenced in an attribute value. */
        public const string Vocab_RDFaAttribute = "vocab";


        /* Declaring Namespaces: http://www.w3.org/TR/1999/REC-xml-names-19990114/#ns-decl */
        public const string CombiningCharRegex = "[\\u0300-\\u0345]"
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

        public const string ExtenderRegex = "\\u00B7 | "
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

        public const string NCNameCharRegex = "(?:\\w|\\d|\\.|-|_" + CombiningCharRegex + "|" + ExtenderRegex + ")";
        public const string NCNameRegex = "(?:\\w|_)(?:" + NCNameCharRegex + ")*";
        public const string PrefixedAttNameRegex = "xmlns:(" + NCNameRegex + ")";

        /* Uniform Resource Identifiers (URI): http://www.ietf.org/rfc/rfc2396.txt */
        /* '[' and ']' characters are excluded to distingish between safe curies and uris */
        public const string UriReferenceRegex = "(?:(?:[^\\[\\]\\:\\/\\?\\#]+):)?(?:\\/\\/(?:[^\\[\\]\\/\\?\\#]*))?(?:[^\\[\\]\\?\\#]*)(?:\\?(?:[^\\[\\]\\#]*))?(?:\\#(?:[^\\[\\]]*))?";
        public const string UriReferenceParsedRegex = "(?:([^\\[\\]\\:\\/\\?\\#]+):)?(?:\\/\\/([^\\[\\]\\/\\?\\#]*))?([^\\[\\]\\?\\#]*)(?:\\?([^\\[\\]\\#]*))?(?:\\#([^\\[\\]]*))?";
        public const string URISchemaRegex   = @"(?:([^\\:\\/\\?\\#]+):)?(?:\\/\\/(?:[^\\/\\?\#]*))?(?:[^\\?\\#]*)(?:\\?(?:[^\\#]*))?(?:\\#(?:.*))?";
        public const string ErrorHtmlAttributeNull = "HtmlAttribute can be null";

        /* Internationalized Resource Identifiers (IRI): http://www.ietf.org/rfc/rfc3987.txt */
        public const string IrelativeRefRegex = "(?:\\/\\/(?:[^\\/\\?\\#]*))?(?:[^\\?\\#]*)(?:\\?(?:[^\\#]*))?(?:\\#(?:.*))?";

        /* CURIE Syntax Definition: http://www.w3.org/TR/rdfa-syntax/#s_curies */
        public const string PrefixRegex = NCNameRegex;
        public const string ReferenceRegex = IrelativeRefRegex;
        public const string CurieRegex = "(?:(" + PrefixRegex + ")" + "?:)" + "(" + ReferenceRegex + ")";

        // list of reserved values for @rel and @rev
        public const string ReservedWordRegex = "(?:alternate|appendix|bookmark|cite|chapter|contents|copyright|first|glossary|help|icon|index|last|license|meta|next|p3pv1|prev|role|section|stylesheet|subsection|start|top|up)";

        public const string UriDataType = "Uri";
        public const string LiteralDataType = "Literal";

        public const string BnodePrefix = "rdfadevBnode";
        public const string EmptyBnodePrefix = "rdfadevBnodeEmpty";       
    }
}
