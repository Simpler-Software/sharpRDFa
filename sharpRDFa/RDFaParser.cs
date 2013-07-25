using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using sharpRDFa.Extension;
using sharpRDFa.Processing;
using sharpRDFa.RDF;
using sharpRDFa.Resource;

namespace sharpRDFa
{
    /*
     * sharpRDFa
     * 
     * Class to parse RDFa 1.1 with no external dependancies.
     * 
     * http://www.w3.org/TR/rdfa-core/
     * 
     * @copyright  Copyright (c) 2013-2014 Ruwan Nawarathne
     * @license    http://opensource.org/licenses/BSD-3-Clause
     */

    public class RDFaParser : IRDFaParser
    {
        private const string HtmlHeadXPath = "/html[1]/head[1]";
        private const string HtmlElementName = "HTML";
        private decimal _bnodes;

        private readonly IRDFaProcessor _processor = new RDFaProcessor();

        public IList<RDFTriple> ParseRDFTriplesFromURL(string url)
        {
            HtmlDocument document = GetDocumentFromURL(url);
            return ParseRDFTriples(document);
        }

        public IList<RDFTriple> ParseRDFTriplesFromFile(string filePath)
        {
            HtmlDocument document = GetDocumentFromFile(filePath);
            return ParseRDFTriples(document);
        }

        public IList<RDFTriple> ParseRDFTriples(HtmlDocument document)
        {
            ParserContext context = InitializeContext(document);

            HtmlNode rootElement = GetDocumentRootElement(document);
            ValidateRootElement(rootElement);

            _bnodes = 0;
            Parse(context, rootElement);

            return context.ConstructedTiples;
        }

        private void ValidateRootElement(HtmlNode rootElement)
        {
            if (rootElement == null || rootElement.Name.ToUpper() != HtmlElementName)
                throw ParserErrorHtmlRootElementException();
        }

        private void Parse(ParserContext context, HtmlNode elementNode)
        {
            // Step 1: Validating the html node which is going tp parse
            if (!ShouldParse(elementNode)) return;

            // Step 2: Establish local variables
            bool skipElement = false;
            string subject;
            string objecto = null;
            string typedResource = null;
            IList<string> rels = new List<string>();
            IList<string> revs = new List<string>();
            IList<string> incompleteRels = new List<string>();
            IList<string> incompleteRevs = new List<string>();
            IDictionary<string, IList<string>> listMapping;

            string about = elementNode.GetAttributeValue(Constants.About_RDFaAttribute, null);
            string content = elementNode.GetAttributeValue(Constants.Content_RDFaAttribute, null);
            string dataType = elementNode.GetAttributeValue(Constants.DataType_RDFaAttribute, null);
            string href = elementNode.GetAttributeValue(Constants.Href_RDFaAttribute, null);
            string inlist = elementNode.GetAttributeValue(Constants.Inlist_RDFaAttribute, null);
            string property = elementNode.GetAttributeValue(Constants.Property_RDFaAttribute, null);
            string rel = elementNode.GetAttributeValue(Constants.Rel_RDFaAttribute, null);
            string resource = elementNode.GetAttributeValue(Constants.Resource_RDFaAttribute, null);
            string rev = elementNode.GetAttributeValue(Constants.Rev_RDFaAttribute, null);
            string src = elementNode.GetAttributeValue(Constants.Src_RDFaAttribute, null);
            string typeOf = elementNode.GetAttributeValue(Constants.TypeOf_RDFaAttribute, null);

            // Step 3: Default vocabulary
            string defaultVocabulary = UpdateDefaultVocabulary(context, elementNode);

            // Step 4: Update prefix mappings
            IDictionary<string, string> prefixMappings = UpdatePrefixMappings(context, elementNode);

            // Step 5: Update Current Language
            var currentLanguage = UpdateLanguage(context, elementNode);

            // Step 5: Establish a new subject if no rel/rev
            if (!rel.IsNullOrEmpty() && !rev.IsNullOrEmpty())
            {
                if (!property.IsNullOrEmpty() && content.IsNullOrEmpty() && dataType.IsNullOrEmpty())
                {
                    subject = about;
                    if (!typeOf.IsNullOrEmpty() && subject.IsNullOrEmpty())
                    {
                        typedResource = (new List<string> { resource, href, src }).FirstNonEmptyOrDefault();
                        if (typedResource.IsNullOrEmpty()) typedResource = GetNewBNode();
                        objecto = typedResource;
                    }
                }
                else
                {
                    subject = (new List<string> { about, resource, href, src }).FirstNonEmptyOrDefault();
                }

                // Establish a subject if there isn't one
                if (subject.IsNullOrEmpty())
                {
                    if (elementNode.XPath == HtmlHeadXPath)
                        subject = context.ParentObject;
                    else if (elementNode.XPath.Split('/').Length <= 2)
                        subject = context.Base;
                    else if (typeOf.IsNotNull() && property.IsNull())
                        subject = GetNewBNode();
                    else
                    {
                        if (property.IsNullOrEmpty()) skipElement = true;
                        subject = context.ParentSubject;
                    }
                }
            }
            else
            {
                // Step 6
                // If the current element does contain a @rel or @rev attribute, then the next step is to
                // establish both a value for new subject and a value for current object resource:

                subject = about;
                objecto = (new List<string> { resource, href, src }).FirstNonEmptyOrDefault();


                if (!typeOf.IsNullOrEmpty())
                {
                    if (objecto.IsNullOrEmpty() && subject.IsNullOrEmpty())
                        objecto = GetNewBNode();
                    typedResource = !subject.IsNullOrEmpty() ? subject : objecto;
                }

                // FIXME: if the element is the root element of the document
                // then act as if there is an empty @about present
                if (subject.IsNullOrEmpty())
                    subject = context.ParentSubject;

                if (!rel.IsNullOrEmpty())
                    rels = Regex.Split(rel, @"/(?:$|^|\s+)/");

                if (!rev.IsNullOrEmpty())
                    revs = Regex.Split(rev, @"/(?:$|^|\s+)/");
            }

            // FIXME: better place for this?
            if (!typeOf.IsNullOrEmpty() && !subject.IsNullOrEmpty() && typedResource.IsNull())
                typedResource = subject;

            // Step 7: Process @typeof if there is a subject
            if (!typedResource.IsNullOrEmpty())
            {
                var typeAttributeList = _processor.GetCURIEs(typedResource, defaultVocabulary, prefixMappings);

                if (!typeAttributeList.IsEmpty() && typeAttributeList.Count > 0)
                {
                    foreach (var item in typeAttributeList)
                    {
                        var dto = new RDFConstructDto
                                      {
                                          Subject = typedResource,
                                          Predicate = "rdf:type",
                                          Object = item.Curie,
                                          DataType = Constants.UriDataType,
                                      };

                        ConstructTriple(context, dto, context.Base, defaultVocabulary, prefixMappings);
                    }
                }
            }

            // Step 8: Create new List mapping if the subject has changed
            if (subject.IsNotNull() && !string.Equals(subject, context.ParentSubject))
            {
                listMapping = new Dictionary<string, IList<string>>();
            }
            else
            {
                listMapping = context.ListMapping;
            }

            // Step 9: Generate triples with given object
            if (subject.IsNotNull() && objecto.IsNotNull())
            {
                foreach (var prop in rels)
                {
                    var dto = new RDFConstructDto
                                  {
                                      Subject = subject,
                                      Predicate = prop,
                                      Object = objecto,
                                      DataType = Constants.UriDataType,
                                  };
                    if (inlist.IsNotNull())
                    {
                        UpdateListMapping(listMapping, prop, objecto);
                    }
                    else
                    {
                        ConstructTriple(context, dto, context.Base, defaultVocabulary, prefixMappings);
                    }
                }

                foreach (var prop in revs)
                {
                    var dto = new RDFConstructDto
                                  {
                                      Subject = objecto,
                                      Predicate = prop,
                                      Object = subject,
                                      DataType = Constants.UriDataType,
                                  };

                    ConstructTriple(context, dto, context.Base, defaultVocabulary, prefixMappings);
                }
            }
            else if ((rels.IsNotNull() && rels.Count > 0) || (revs.IsNotNull() && revs.Count > 0))
            {
                // Step 10: Incomplete triples and bnode creation
                //currentObjectResource = GetNewBNode();
                if (rels.IsNotNull() && rels.Count > 0)
                {
                    if (inlist.IsNotNull())
                    {
                        foreach (var prop in rels)
                        {
                            // FIXME: add support for incomplete lists
                            if (!listMapping.ContainsKey(prop))
                            {
                                listMapping.Add(prop, new List<string>());
                            }
                        }
                    }
                    else
                    {
                        incompleteRels = rels;
                    }
                }
                if (revs.IsNotNull() && revs.Count > 0)
                {
                    incompleteRevs = revs;
                }
            }

            // Step 11: establish current property value
            if (subject.IsNotNull() && property.IsNotNull())
            {
                bool dateTime = false;
                var dto = new RDFConstructDto();

                if (dataType.IsNotNull())
                    dataType = _processor.ResolveCURIE(dataType, context.Base, prefixMappings);

                if (elementNode.Name == "data" && elementNode.HasAttribute("value"))
                {
                    dto.Object = elementNode.GetAttributeValue("value", null);
                }
                else if (elementNode.HasAttribute("datetime"))
                {
                    dto.Object = elementNode.GetAttributeValue("datetime", null);
                    dateTime = true;
                }
                else if (dataType == "")
                {
                    dto.Object = elementNode.InnerText;
                    dto.DataType = Constants.LiteralDataType;
                }

                //elseif ($datatype === self::RDF_XML_LITERAL) {
                //    $value['value'] = '';
                //    foreach ($node->childNodes as $child) {
                //        $value['value'] .= $child->C14N();
                //    }
                //} 

                else if (content.IsNotNull())
                {
                    dto.Object = content;
                }
                else if (dataType.IsNull() && rels.IsEmpty() && revs.IsEmpty())
                {
                    dto.Object = (new List<string> { resource, href, src }).FirstNonEmptyOrDefault();
                    dto.DataType = Constants.UriDataType;
                }

                if (dto.IsNull() && typedResource.IsNotNull() && about.IsNull())
                {
                    dto.DataType = Constants.UriDataType;
                    dto.Object = typedResource;
                }

                if (dto.Object.IsNull())
                {
                    dto.Object = elementNode.InnerText;
                    dto.DataType = Constants.LiteralDataType;
                }

                if (dto.DataType.IsNull())
                {
                    dto.DataType = Constants.LiteralDataType;
                    if (dataType.IsNotNull())
                    {
                        dto.DataType = dataType;
                    }
                    else if (dateTime || elementNode.Name == "time")
                    {
                        dto.DataType = "time";
                    }

                    if (dto.DataType.IsNull() && currentLanguage.IsNotNull())
                    {
                        dto.Language = currentLanguage;
                    }
                }

                // Add each of the properties
                IList<string> propertyAttributeList = Regex.Split(property, @"/(?:$|^|\s+)/");

                foreach (var prop in propertyAttributeList)
                {
                    dto.Subject = subject;
                    dto.Predicate = prop;
                    if (inlist.IsNotNull())
                    {
                        UpdateListMapping(listMapping, prop, objecto);
                    }
                    else if (subject.IsNotNull())
                    {
                        ConstructTriple(context, dto, context.Base, defaultVocabulary, prefixMappings);
                    }
                }
            }

            // Step 12: Complete the incomplete triples from the evaluation context
            if (!skipElement && subject.IsNotNull() && (incompleteRels.IsNotNull() || incompleteRevs.IsNotNull()))
            {
                foreach (var prop in incompleteRels)
                {
                    var dto = new RDFConstructDto()
                                  {
                                      Subject = context.ParentSubject,
                                      Predicate = prop,
                                      Object = subject,
                                      DataType = Constants.UriDataType
                                  };
                    ConstructTriple(context, dto, context.Base, defaultVocabulary, prefixMappings);
                }

                foreach (var prop in incompleteRevs)
                {
                    var dto = new RDFConstructDto()
                                  {
                                      Subject = subject,
                                      Predicate = prop,
                                      Object = context.ParentSubject,
                                      DataType = Constants.UriDataType
                                  };

                    ConstructTriple(context, dto, context.Base, defaultVocabulary, prefixMappings);
                }
            }

            //Step 11  create a new evaluation context and proceed recursively
            if (elementNode.HasChildNodes)
            {
                for (var currentElement = elementNode.FirstChild; currentElement != null; currentElement = currentElement.NextSibling)
                {
                    if (!skipElement)
                    {
                        if (subject.IsNotNull())
                        {
                            context.ParentSubject = subject;
                        }

                        if (objecto.IsNotNull())
                        {
                            context.ParentObject = objecto;
                        }


                        if (incompleteRels.IsNotNull())
                        {
                            context.IncompleteRels = incompleteRels;
                        }

                        if (incompleteRevs.IsNotNull())
                        {
                            context.IncompleteRevs = incompleteRevs;
                        }
                    }

                    Parse(context, currentElement);
                }
            }

            // Step 14: create triples for lists
            if (listMapping.IsNotNull())
            {
                foreach (var prop in listMapping.Keys)
                {

                    GenerateList(subject, prop, listMapping[prop]);

                }
            }
        }

        private void GenerateList(string subject, string property, IEnumerable<string> list)
        {
            var current = subject;
            var prop = property;

            // Output a blank node for each item in the list
            foreach (var item in list)
            {
                var newNode = GetNewBNode();

                ConstructTriple(new RDFConstructDto
                              {
                                  Subject = current,
                                  Predicate = prop,
                                  Object = newNode,
                                  DataType = Constants.BnodeDataType,
                              });

                ConstructTriple(new RDFConstructDto
                                    {
                                        Subject = newNode,
                                        Predicate = "rdf:first",
                                        Object = item
                                    });
                current = newNode;
                prop = "rdf:rest";
            }
            // Finally, terminate the list
            ConstructTriple(new RDFConstructDto
                                {
                                    Subject = current,
                                    Predicate = prop,
                                    Object = "rdf:nil"
                                });
        }

        private void ConstructTriple(RDFConstructDto context)
        {
            
        }

        private void UpdateListMapping(IDictionary<string, IList<string>> listMapping, string prop, string objecto)
        {
            if (!listMapping.ContainsKey(prop))
                listMapping.Add(prop, new List<string>());
            listMapping[prop].Add(objecto);
        }

        private string GetNewBNode()
        {
            return "_:" + Constants.BnodePrefix + _bnodes++;
        }

        public string UpdateDefaultVocabulary(ParserContext context, HtmlNode elementNode, bool isRecursive = false)
        {
            if (elementNode.HasAttribute("vocab"))
            {
                string vocab = elementNode.GetAttributeValue("vocab", string.Empty);
                if (!string.IsNullOrEmpty(vocab))
                {
                    if (!isRecursive)
                    ConstructTriple(new RDFConstructDto
                                        {
                                            Subject = context.Base,
                                            Predicate = "rdfa:usesVocabulary",
                                            Object = vocab,
                                            DataType = Constants.UriDataType,
                                        });
                    return vocab;
                }
            }
            else if (elementNode.ParentNode != null)
            {
                return UpdateDefaultVocabulary(context, elementNode.ParentNode, true);
            }

            return null;
        }

        private string UpdateLanguage(ParserContext context, HtmlNode elementNode)
        {
            var language = string.IsNullOrEmpty(elementNode.GetLanguage())
                               ? context.Language
                               : elementNode.GetLanguage();
            context.Language = language;
            return language;
        }

        private HtmlNode GetDocumentRootElement(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//html");
        }

        private HtmlDocument GetDocumentFromURL(string url)
        {
            var webGet = new HtmlWeb();
            return webGet.Load(url);
        }

        private HtmlDocument GetDocumentFromFile(string filePath)
        {
            var document = new HtmlDocument();
            document.Load(filePath);
            return document;
        }

        private ParserContext InitializeContext(HtmlDocument document)
        {
            var parserContext = new ParserContext();

            var baseURI = GetBaseURI(document);
            parserContext.Base = baseURI;
            parserContext.ParentSubject = baseURI;
            parserContext.ParentObject = null;
            parserContext.PrefixMappings = GetDocNamespaces(document);
            parserContext.Terms = GetDefaultTerms();
            parserContext.IncompleteRels = new List<string>();
            parserContext.IncompleteRevs = new List<string>();
            parserContext.Language = null;
            parserContext.ConstructedTiples = new List<RDFTriple>();
            parserContext.ListMapping = new Dictionary<string, IList<string>>();

            // Set the default prefix
            SetDefaultPrefix(parserContext);

            return parserContext;
        }

        private void SetDefaultPrefix(ParserContext context)
        {
            context.PrefixMappings.Add(Constants.DefaultPrefix, "http://www.w3.org/1999/xhtml/vocab#");
        }

        private IDictionary<string, string> GetDefaultTerms()
        {
            // RDFa 1.1 default term mapping
            var terms = new Dictionary<string, string>
                            {
                                {"describedby", "http://www.w3.org/2007/05/powder-s#describedby"},
                                {"license", "http://www.w3.org/1999/xhtml/vocab#license"},
                                {"role", "http://www.w3.org/1999/xhtml/vocab#role"}
                            };

            return terms;
        }

        public IDictionary<string, string> GetDocNamespaces(HtmlDocument document)
        {
            var mappings = new Dictionary<string, string>();
            var rootElement = GetDocumentRootElement(document);
            foreach (var source in rootElement.GetNameSpaceMappings())
            {
                if (!mappings.ContainsKey(source.Key))
                    mappings.Add(source.Key, source.Value);
            }
            return mappings;
        }

        public string GetBaseURI(HtmlDocument document)
        {
            var rootElement = GetDocumentRootElement(document);
            if (rootElement.IsNull()) return string.Empty;

            string baseURI = null;
            var xpaths = new[] { "//html", "//head", "//base" };

            foreach (var xpath in xpaths)
            {
                baseURI = rootElement.GetXPATHAttributeValue(xpath, "href");
                if (!string.IsNullOrEmpty(baseURI)) return baseURI;
            }

            if (string.IsNullOrEmpty(baseURI)) baseURI = GetNewBNode();

            return baseURI;
        }

        private Exception ParserErrorHtmlRootElementException()
        {
            return new Exception(CommonResource.ParserError_InvalidHtmlRootElement);
        }

        public IDictionary<string, string> UpdatePrefixMappings(ParserContext context, HtmlNode elementNode)
        {
            if (elementNode.HasAttribute(Constants.Prefix_RDFaAttribute))
            {
                var mappings = new Dictionary<string, string>(context.PrefixMappings);

                string[] splits = Regex.Split(elementNode.GetAttribute(Constants.Prefix_RDFaAttribute).Value, "\\s+");
                int i = 0;
                while (i < splits.Length)
                {
                    var prefix = splits[i].Split(':')[0];
                    var url = splits[i + 1];

                    if (prefix == "_")
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(prefix) && !mappings.ContainsKey(prefix))
                    {
                        mappings.Add(prefix, url);
                    }

                    i += 2;
                }

                if (mappings.Count > context.PrefixMappings.Count)
                    context.PrefixMappings = mappings;

            }
            return context.PrefixMappings;
        }

        private bool ShouldParse(HtmlNode elementNode)
        {
            if (elementNode.IsNull() || !elementNode.NodeType.Equals(HtmlNodeType.Element)) return false;
            //Console.WriteLine(elementNode.XPath);
            return true;
        }

        private void ConstructTriple(ParserContext context, RDFConstructDto dto, string baseURL, string vocabulary, IDictionary<string, string> uriMappings)
        {
            IRDFTripleBuilder builder = new RDFTripleBuilder();
            //IRDFTripleBuilder builder = new RDFTripleBuilderNoValidation();
            builder.CreateSubject(dto.Subject, baseURL, vocabulary, uriMappings);
            builder.CreatePredicate(dto.Predicate, baseURL, vocabulary, uriMappings);
            builder.CreateObject(dto.Object, dto.Language, dto.DataType, baseURL, vocabulary, uriMappings);
            context.ConstructedTiples.Add(builder.GetTriple());
        }
    }
}