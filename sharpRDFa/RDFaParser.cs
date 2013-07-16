using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using sharpRDFa.Extension;
using sharpRDFa.RDF;
using sharpRDFa.RDFa;
using sharpRDFa.Resource;

namespace sharpRDFa
{
    public class RDFaParser : IRDFaParser
    {
        //private readonly IDictionary<string, string> _commonPrefixes;
        private decimal _bnodes;

        //public RDFaParser()
        //{
        //    _commonPrefixes = Vocabulary.GetCommonPrefixes();
        //}

        public IList<RDFTriple> GetRDFTriplesFromURL(string url)
        {
            HtmlDocument document = GetDocumentFromURL(url);
            return GetRDFTriples(document);
        }

        public IList<RDFTriple> GetRDFTriplesFromFile(string filePath)
        {
            HtmlDocument document = GetDocumentFromFile(filePath);
            return GetRDFTriples(document);
        }

        public IList<RDFTriple> GetRDFTriples(HtmlDocument document)
        {
            var triples = new List<RDFTriple>();
            HtmlNode rootElement = GetDocumentRootElement(document);

            if (rootElement == null || rootElement.Name.ToUpper() != "HTML")
                throw ParserErrorHtmlRootElementException();
            
            ParserContext context = InitializeContext(document);
            _bnodes = 0;
            Parse(context, rootElement, ref triples);
            return triples;
        }

        private void Parse(ParserContext context, HtmlNode elementNode, ref List<RDFTriple> triples)
        {
            if (!ShouldParse(elementNode)) return;

            IList<CURIE> propertyAttributeList = new List<CURIE>();
            IList<CURIE> typeAttributeList = new List<CURIE>();
            IList<string> relAttributeList = new List<string>();
            IList<string> revAttributeList = new List<string>();

            string subject = null;
            string predicate = null;
            var objecto = new ObjectNode();

            // Step 1: Establish local variables
            bool recurse = true;
            bool skipElement = false;
            string newSubject = context.ParentSubject;
            string currentObjectLiteral = null;
            string currentObjectResource = null;
            string currentObjectLiteralDataType = null;
            string currentObjectLiteralLanguage = null;
            var localIncompleteTriples = new List<IncompleteTriple>();
            IDictionary<string, string> prefixMappings = null;
            string defaultVocabulary = null;
            string currentLanguage = null;

            defaultVocabulary = UpdateDefaultVocabulary(elementNode);

            //Step 2 update uri mappings
            prefixMappings = UpdatePrefixMappings(context, elementNode);

            //Step 3 set current language
            currentLanguage = UpdateLanguage(context, elementNode); 
            
            //string content = elementNode.GetAttributeValue("content", null);
            //string dataType = elementNode.GetAttributeValue("datatype", null);
            //string property = elementNode.GetAttributeValue("property", null);
            //string typeOf = elementNode.GetAttributeValue("typeof", null);
 
            //Step 4 of the processing sequence
            if (!elementNode.HasAttribute("rel") && !elementNode.HasAttribute("rev"))
            {

                var value = elementNode.GetAttributeValue(new[]{"about","src", "resource", "href"});
                
                if(!string.IsNullOrEmpty(value))
                {
                    newSubject = value;
                }
                    /*
                if (elementNode.HasAttribute("about") && elementNode.GetAttribute("about").IsUriOrSafeCurie(prefixMappings).IsNotNull())
                {
                    newSubject = elementNode.GetAttribute("about").Value;
                }
                else if (elementNode.HasAttribute("src") && elementNode.GetAttribute("src").IsUri().IsNotNull())
                {
                    newSubject = elementNode.GetAttribute("src").Value;
                }
                else if (elementNode.HasAttribute("resource") && elementNode.GetAttribute("resource").IsUriOrSafeCurie(prefixMappings).IsNotNull())
                {
                    newSubject = elementNode.GetAttribute("resource").Value;
                }
                else if (elementNode.HasAttribute("href") && elementNode.GetAttribute("href").IsUri().IsNotNull())
                {
                    newSubject = elementNode.GetAttribute("href").Value;
                }
                 */
                else if ((elementNode.Name.ToUpper() == "HEAD") || (elementNode.Name.ToUpper() == "BODY"))
                {
                    newSubject = "";
                }
                else if (elementNode.HasAttribute("typeof") && (typeAttributeList = elementNode.GetAttribute("typeof").GetCURIEs(defaultVocabulary, prefixMappings)).IsNotNull() && typeAttributeList.Count > 0)
                {
                    newSubject = "[_:" + Constants.BnodePrefix + _bnodes++ + "]";
                }
                else
                {
                    if (!string.IsNullOrEmpty((context.ParentObject.Value)))
                    {
                        newSubject = context.ParentObject.Value;
                    }
                    if (elementNode.HasAttribute("property") && (propertyAttributeList = elementNode.GetAttribute("property").GetCURIEs(defaultVocabulary, prefixMappings)).IsNull())
                        skipElement = true;
                }
            }
            else //Step 5 of the processing sequence
            {
                var subjectValue = elementNode.GetAttributeValue(new[] { "about"});

                if (!string.IsNullOrEmpty(subjectValue))
                {
                    newSubject = subjectValue;
                }
                /*
                if (elementNode.GetAttribute("about").IsUriOrSafeCurie(prefixMappings).IsNotNull())
                {
                    newSubject = elementNode.GetAttribute("about").Value;
                }
                else if (elementNode.GetAttribute("src").IsUri().IsNotNull())
                {
                    newSubject = elementNode.GetAttribute("src").Value;
                }
                 */
                else if ((elementNode.Name.ToUpper() == "HEAD") || (elementNode.Name.ToUpper() == "BODY"))
                {
                    newSubject = "";
                }
                else if ((typeAttributeList = elementNode.GetAttribute("typeof").GetCURIEs(defaultVocabulary, prefixMappings)).IsNotNull() && typeAttributeList.Count > 0)
                {
                    newSubject = "[_:" + Constants.BnodePrefix + _bnodes++ + "]";
                }
                else if (!string.IsNullOrEmpty(context.ParentObject.Value))
                {
                    newSubject = context.ParentObject.Value;
                }

                var objectResourceValue = elementNode.GetAttributeValue(new[] { "src", "href", "resource" });

                if (!string.IsNullOrEmpty(objectResourceValue))
                {
                    currentObjectResource = objectResourceValue;
                }

                /*
                if (elementNode.GetAttribute("resource").IsUriOrSafeCurie(prefixMappings).IsNotNull())
                {
                    currentObjectResource = elementNode.GetAttribute("resource").Value;
                }
                else if (elementNode.GetAttribute("href").IsUri().IsNotNull())
                {
                    currentObjectResource = elementNode.GetAttribute("href").Value;
                }*/
            }

            // Paso 6 de la secuencia de procesamiento 
            if (!string.IsNullOrEmpty(newSubject))
            {
                if (typeAttributeList.IsNull() || typeAttributeList.Count == 0)
                {
                    if (elementNode.HasAttribute("typeof"))
                        typeAttributeList = elementNode.GetAttribute("typeof").GetCURIEs(defaultVocabulary, prefixMappings);
                }
                if (typeAttributeList.IsNotNull() && typeAttributeList.Count > 0)
                {
                    foreach (var item in typeAttributeList)
                    {
                        subject = newSubject;
                        predicate = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type";
                        objecto.Value = "[" + item.Curie + "]";
                        objecto.Type = TripleObjectType.URIorSafeCURIE;

                        triples.Add(ConstructTriple(subject, predicate, objecto, context.Base, defaultVocabulary, prefixMappings));
                    }
                }
            }

            // Paso 7 de la secuencia de procesamiento
            if (currentObjectResource != null)
            {
                if (relAttributeList.Count == 0)
                {
                    if (elementNode.HasAttribute("rel"))
                    {
                        var item = elementNode.GetAttribute("rel").GetReservedWordOrCURIE(prefixMappings);
                        if(item.IsNotNull()) relAttributeList.Add(item);
                    }
                }
                if (relAttributeList.Count > 0)
                {
                    foreach (var item in relAttributeList)
                    {
                        subject = newSubject;
                        if (!string.IsNullOrEmpty(item))
                        {
                            predicate = "[" + item + "]";
                        }
                        else
                        {
                            predicate = "http://www.w3.org/1999/xhtml/vocab#" + item;
                        }
                        objecto.Value = currentObjectResource;
                        objecto.Type = TripleObjectType.URIorSafeCURIE;

                        triples.Add(ConstructTriple(subject, predicate, objecto, context.Base, defaultVocabulary, prefixMappings));
                    }
                }
                if (revAttributeList.Count == 0)
                {
                    if (elementNode.HasAttribute("rev"))
                    {
                        var item = elementNode.GetAttribute("rev").GetReservedWordOrCURIE(prefixMappings);
                        if (item.IsNotNull()) revAttributeList.Add(item);
                    }
                }
                if (revAttributeList.Count > 0)
                {
                    foreach (var item in revAttributeList)
                    {
                        objecto.Value = newSubject;
                        objecto.Type = TripleObjectType.URIorSafeCURIE; 
                        if (!string.IsNullOrEmpty(item))
                        {
                            predicate = "[" + item + "]";
                        }
                        else
                        {
                            predicate = "http://www.w3.org/1999/xhtml/vocab#" + item;
                        }

                        subject = currentObjectResource;

                        triples.Add(ConstructTriple(subject, predicate, objecto, context.Base, defaultVocabulary, prefixMappings));
                    }
                }
            }


            // Paso 8 de la secuencia de procesamiento 
            if (currentObjectResource == null)
            {
                if (relAttributeList.Count == 0)
                {
                    if (elementNode.HasAttribute("rel"))
                        relAttributeList.Add(elementNode.GetAttribute("rel").GetReservedWordOrCURIE(prefixMappings));
                }
                if (relAttributeList.Count > 0)
                {
                    foreach (var item in relAttributeList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            localIncompleteTriples.Add(new IncompleteTriple("[" + item + "]", "forward", elementNode));
                        }
                        //else
                        //{
                        //  local_list_of_incomplete_triples.push(new incomplete_triple("http://www.w3.org/1999/xhtml/vocab#" + relAttributeList[i].reserved_word, "forward", elemento));
                        //}

                    }
                }

                if (revAttributeList.Count == 0)
                {
                    if (elementNode.HasAttribute("rev"))
                        revAttributeList.Add(elementNode.GetAttribute("rev").GetReservedWordOrCURIE(prefixMappings));
                }
                if (revAttributeList.Count > 0)
                {
                    foreach (var item in revAttributeList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            localIncompleteTriples.Add(new IncompleteTriple("[" + item + "]", "reverse", elementNode));
                        }
                        //else
                        //{
                        //  local_list_of_incomplete_triples.push(new incomplete_triple("http://www.w3.org/1999/xhtml/vocab#" + revAttributeList[i].reserved_word, "reverse", elemento));
                        //}

                    }
                }

                if ((elementNode.HasAttribute("rel") || elementNode.HasAttribute("rev")))
                {
                    /* XXX: sometimes a bnode is created without being used at any moment */
                    currentObjectResource = "[_:" + Constants.BnodePrefix + _bnodes++ + "]";
                    //current_object_resource.elemento = elemento;
                }
            }


            // Paso 9 de la secuencia de procesamiento
            if (propertyAttributeList.IsNull() || propertyAttributeList.Count == 0)
            {
                if (elementNode.HasAttribute("property") && elementNode.HasAttribute("content"))
                    propertyAttributeList = elementNode.GetAttribute("property").GetCURIEs(defaultVocabulary, prefixMappings);
            }
            if (propertyAttributeList.IsNotNull() && propertyAttributeList.Count > 0)
            {
                // typed literal 
                if (elementNode.HasAttribute("datatype") &&
                   !string.IsNullOrEmpty(elementNode.GetAttribute("datatype").Value) &&
                   elementNode.GetAttribute("datatype").IsCurie(prefixMappings).IsNotNull() &&
                   elementNode.GetAttribute("datatype").ResolveCURIE(context.Base as string, prefixMappings) != "http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral")
                {
                    if (elementNode.HasAttribute("content"))
                    {
                        currentObjectLiteral = elementNode.GetAttribute("content").Value;
                        currentObjectLiteralDataType = "[" + elementNode.GetAttribute("datatype").Value + "]";
                    }
                    else
                    {
                        currentObjectLiteral = elementNode.GetChildrenText();
                        currentObjectLiteralDataType = "[" + elementNode.GetAttribute("datatype").Value + "]";
                    }
                }
                // plain literal 
                else if (elementNode.HasAttribute("content"))
                {
                    currentObjectLiteral = elementNode.GetAttribute("content").Value;
                    currentObjectLiteralLanguage = currentLanguage;
                }
                else if (elementNode.IsTextNode())
                {
                    currentObjectLiteral = elementNode.GetChildrenText();
                    currentObjectLiteralLanguage = currentLanguage;
                }
                else if (elementNode.ChildNodes.Count == 0)
                {
                    currentObjectLiteral = "";
                    currentObjectLiteralLanguage = currentLanguage;
                }
                else if (elementNode.HasAttribute("datatype") && elementNode.GetAttribute("datatype").Value == "")
                {
                    currentObjectLiteral = elementNode.GetChildrenText();
                    currentObjectLiteralLanguage = currentLanguage;
                }
                // XML literal 
                else if (!elementNode.IsTextNode() &&
                        elementNode.HasAttribute("datatype") ||
                        elementNode.GetAttribute("datatype").IsCurie(prefixMappings).IsNotNull())
                {
                    currentObjectLiteral = elementNode.WriteContentTo();
                    currentObjectLiteralDataType = "http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral";
                    recurse = false;
                }
                else if (!elementNode.IsTextNode() &&
                    elementNode.HasAttribute("datatype") &&
                        elementNode.GetAttribute("datatype").IsCurie(prefixMappings).IsNotNull() &&
                        elementNode.GetAttribute("datatype").ResolveCURIE(context.Base, prefixMappings) == "http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral")
                {
                    currentObjectLiteral = elementNode.WriteContentTo(); ;
                    currentObjectLiteralDataType = "[" + elementNode.GetAttribute("datatype").Value + "]";
                    recurse = false;
                }

                if (currentObjectLiteral != null)
                {
                    foreach (var item in propertyAttributeList)
                    {
                        subject = newSubject;
                        predicate = "[" + item.Curie + "]";
                        objecto.Value = currentObjectLiteral;
                        objecto.DataType = currentObjectLiteralDataType;
                        objecto.Language = currentObjectLiteralLanguage;
                        objecto.Type = TripleObjectType.Literal;

                        triples.Add(ConstructTriple(subject, predicate, objecto, context.Base, defaultVocabulary, prefixMappings));
                    }
                }
            }


            //Step 10 of the processing sequence
            if (!skipElement && !string.IsNullOrEmpty(newSubject))
            {
                var incompleteTriples = context.IncompleteTriples;

                if (incompleteTriples.IsNotNull())
                    foreach (var incompleteTriple in incompleteTriples)
                    {
                        if (incompleteTriple.Direction == "forward")
                        {
                            subject = context.ParentSubject;
                            predicate = incompleteTriple.Predicate;
                            objecto.Value = newSubject;
                            objecto.Type = TripleObjectType.URIorSafeCURIE;

                            triples.Add(ConstructTriple(subject, predicate, objecto, context.Base, defaultVocabulary, prefixMappings));
                            
                        }
                        else if (incompleteTriple.Direction == "reverse")
                        {
                            objecto.Value = context.ParentSubject;
                            objecto.Type = TripleObjectType.URIorSafeCURIE;
                            predicate = incompleteTriple.Predicate;
                            subject = newSubject;

                            triples.Add(ConstructTriple(subject, predicate, objecto, context.Base, defaultVocabulary, prefixMappings));

                        }
                    }
            }

            //Step 11 of the processing sequence
            if (recurse)
            {
                for (var currentElement = elementNode.FirstChild; currentElement != null; currentElement = currentElement.NextSibling)
                {
                    if (skipElement)
                    {
                        //context.Language = currentLanguage;
                        //context.PrefixMappings = prefixMappings;
                    }
                    else
                    {
                        //nuevo_contexto = new context();
                        // Nuevos valores del contexto de evaluación
                        //nuevo_contexto.base = contexto.base;
                        if (!string.IsNullOrEmpty(newSubject))
                        {
                            context.ParentSubject = newSubject;
                        }

                        if (!string.IsNullOrEmpty(currentObjectLiteral))
                        {
                            context.ParentObject.Literal = currentObjectLiteral;
                        }
                        else if (newSubject != null)
                        {
                            context.ParentObject.Value = newSubject;
                        }
                        else
                        {
                            //nuevo_contexto.parent_object   = contexto.parent_subject;
                        }
                        //context.PrefixMappings = prefixMappings;
                        //nuevo_contexto.list_of_incomplete_triples = local_list_of_incomplete_triples;
                        //context.Language = currentLanguage;
                    }

                    Parse(context, currentElement, ref triples);

                }
            }
        }

        public string UpdateDefaultVocabulary(HtmlNode elementNode)
        {
            if(elementNode.HasAttribute("vocab"))
            {
                string vocab = elementNode.GetAttributeValue("vocab", string.Empty);
                if (!string.IsNullOrEmpty(vocab)) return vocab;
            }
            else if(elementNode.ParentNode != null)
            {
                return UpdateDefaultVocabulary(elementNode.ParentNode);
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
            parserContext.ParentObject = new ObjectNode();
            parserContext.PrefixMappings = GetDocNamespaces(document);
            parserContext.IncompleteTriples = new List<IncompleteTriple>();
            parserContext.Language = null;

            return parserContext;
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
            var xpaths = new[]{"//html", "//head", "//base"};

            foreach (var xpath in xpaths)
            {
                baseURI = rootElement.GetXPATHAttributeValue(xpath, "href");
                if (!string.IsNullOrEmpty(baseURI)) return baseURI;
            }

            return baseURI;
        }

        private Exception ParserErrorHtmlRootElementException()
        {
            return new Exception(CommonResource.ParserError_InvalidHtmlRootElement);
        }

        public IDictionary<string, string> UpdatePrefixMappings(ParserContext context, HtmlNode elementNode)
        {      
            if(elementNode.HasAttribute("prefix"))
            {
                var mappings = new Dictionary<string, string>(context.PrefixMappings);

                string[] splits = Regex.Split(elementNode.GetAttribute("prefix").Value, "\\s+");
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

                if(mappings.Count > context.PrefixMappings.Count)
                    context.PrefixMappings = mappings;

            }
            return context.PrefixMappings;
        }

        private bool ShouldParse(HtmlNode elementNode)
        {
            if (elementNode.IsNull() || !elementNode.NodeType.Equals(HtmlNodeType.Element)) return false;
            if (elementNode.Name.ToUpper() == "SCRIPT") return false;
            return true;
        }

        private RDFTriple ConstructTriple(string subject, string predicate, ObjectNode objectNode, string baseURL, string vocabulary, IDictionary<string, string> uriMappings)
        {
            IRDFTripleBuilder builder = new RDFTripleBuilder();
            builder.CreateSubject(subject, baseURL, vocabulary, uriMappings);
            builder.CreatePredicate(predicate, baseURL, vocabulary, uriMappings);
            builder.CreateObject(objectNode.Value, objectNode.Language, objectNode.DataType, objectNode.Type, baseURL, vocabulary, uriMappings);
            return builder.GetTriple();
        }
    }
}