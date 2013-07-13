using System.Collections.Generic;
using sharpRDFa.Processing;
using sharpRDFa.RDFa;

namespace sharpRDFa.RDFTriple
{
    public class RDFTripleBuilder : IRDFTripleBuilder
    {
        private readonly RDFTriple _rdfTriple;
        private readonly IRDFaProcessor _processor;

        public RDFTripleBuilder()
        {
            _rdfTriple = new RDFTriple();
            _processor = new RDFaProcessor();
        }

        public void CreateSubject(string resource, string baseURL, Dictionary<string, string> uriMappings)
        {
            string subject = null;

            var subjectSafeCURIE = _processor.IsSafeCURIE(resource, uriMappings);
            
            if (subjectSafeCURIE != null)
            {
                if (subjectSafeCURIE.Prefix == "_")
                {
                    if (subjectSafeCURIE.Reference == "")
                        /* "_:" CURIE */
                        subject = "_:" + Constants.EmptyBnodePrefix;
                    else
                        /* do not resolve bnode */
                        subject = resource;
                }
                else
                {
                    /* resolve Safe CURIE */
                    subject = _processor.ResolveSafeCURIE(resource, baseURL, uriMappings);
                }
            }
            else
            {
                /* resolve URI */
                subject = _processor.ResolveURI(resource, baseURL);
            }

            _rdfTriple.Subject = subject;
        }

        public void CreatePredicate(string property, string baseURL, Dictionary<string, string> uriMappings)
        {
            string predicate = null;
            var predicateSafeCURIE = _processor.IsSafeCURIE(property, uriMappings);

            if (predicateSafeCURIE != null)
            {
                if (predicateSafeCURIE.Prefix == "_")
                {
                    if (predicateSafeCURIE.Reference == "")
                        /* "_:" CURIE */
                    {
                        string strSubject = "_:" + Constants.EmptyBnodePrefix;
                    }
                    else
                        /* do not resolve bnode */
                        predicate = property;
                }
                else
                {
                    /* resolve Safe CURIE */
                    predicate = _processor.ResolveSafeCURIE(property, baseURL, uriMappings);
                }
            }
            else
            {
                /* resolve URI */
                predicate = _processor.ResolveURI(property, baseURL);
            }

            _rdfTriple.Predicate = predicate;
        }

        public void CreateObject(string objectValue, string language, string dataType, TripleObjectType type, string baseURL, Dictionary<string, string> uriMappings)
        {
            var newObject = new ObjectNode();

            if (type == TripleObjectType.URIorSafeCURIE)
            {
                {
                    var objectSafeCURIE = _processor.IsSafeCURIE(objectValue, uriMappings);

                    if (objectSafeCURIE != null)
                    {
                        /* bnode */
                        if (objectSafeCURIE.Prefix == "_")
                        {
                            newObject.Curie = objectSafeCURIE.Curie;
                        }
                            /* safe curie */
                        else
                        {
                            string uri = _processor.ResolveSafeCURIE(objectValue, baseURL, uriMappings);
                            newObject.Uri = uri;
                            newObject.UriSchema = _processor.GetURISchema(uri);
                            newObject.Curie = objectSafeCURIE.Curie;
                        }
                    }
                        /* uri */
                    else
                    {
                        string uri = _processor.ResolveURI(objectValue, baseURL);
                        newObject.Uri = uri;
                        newObject.UriSchema = _processor.GetURISchema(uri);
                    }
                }
            }
            else if (type == TripleObjectType.Literal)
            {
                newObject.Literal = objectValue;
                if (language != null)
                {
                    newObject.Language = language;
                }
                if (dataType != null)
                {
                    CURIE datatypeSafeCURIE = _processor.IsSafeCURIE(dataType, uriMappings);

                    newObject.DataType = datatypeSafeCURIE != null
                                             ? _processor.ResolveSafeCURIE(dataType, baseURL, uriMappings)
                                             : _processor.ResolveURI(dataType, baseURL);
                }
            }

            _rdfTriple.Object = newObject;
        }

        public RDFTriple GetTriple()
        {
            return _rdfTriple;
        }
    }
}