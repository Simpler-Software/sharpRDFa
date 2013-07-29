using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using sharpRDFa.RDF;
using sharpRDFa.RDFa;

namespace sharpRDFa
{
    [Serializable]
    public class ParserContext : ISerializable
    {
        public ParserContext() { }

        public ParserContext(SerializationInfo info, StreamingContext context)
        {
            Base = (string) info.GetValue("Base", typeof(string));
            Language = (string) info.GetValue("Language", typeof(string));
            PrefixMappings = (IDictionary<string, string>)info.GetValue("PrefixMappings", typeof(IDictionary<string, string>));
            ParentSubject = (string) info.GetValue("ParentSubject", typeof(string));
            ParentObject = (string) info.GetValue("ParentObject", typeof(string));
            DefaultVocabulary = (string) info.GetValue("DefaultVocabulary", typeof(string));
            IncompleteRels = (IList<string>) info.GetValue("IncompleteRels", typeof(IList<string>));
            IncompleteRevs = (IList<string>) info.GetValue("IncompleteRevs", typeof(IList<string>));
            Terms = (IDictionary<string, string>) info.GetValue("Terms", typeof(IDictionary<string, string>));
            ListMapping = (IDictionary<string, IList<string>>) info.GetValue("ListMapping", typeof(IDictionary<string, IList<string>>));
            //Console.WriteLine(DefaultVocabulary + " ");
        }


        public string Base { get; set; }
        public string Language { get; set; }
        public IDictionary<string, string> PrefixMappings { get; set; }
        public string ParentSubject { get; set; }
        public string ParentObject { get; set; }
        public string DefaultVocabulary { get; set; }
        public IList<string> IncompleteRels { get; set; }
        public IList<string> IncompleteRevs { get; set; }
        public IDictionary<string, string> Terms { get; set; }
        public IDictionary<string, IList<string>> ListMapping { get; set; }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Base", Base);
            info.AddValue("Language", Language);
            info.AddValue("PrefixMappings", PrefixMappings);
            info.AddValue("ParentSubject", ParentSubject);
            info.AddValue("ParentObject", ParentObject);
            info.AddValue("DefaultVocabulary", DefaultVocabulary);
            info.AddValue("IncompleteRels", IncompleteRels);
            info.AddValue("IncompleteRevs", IncompleteRevs);
            info.AddValue("Terms", Terms);
            info.AddValue("ListMapping", ListMapping);
        }
    }
}