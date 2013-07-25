using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using sharpRDFa.Extension;

namespace sharpRDFa
{
    public class Vocabulary
    {
        private const string PrefixFilePath = "Resource\\all.file.json";
        private IDictionary<string, string> _knownPrefixes;

        private static Vocabulary _instance;

        public static Vocabulary Instance
        {
            get
            {
                if (_instance.IsNotNull())
                    return _instance;
                _instance = new Vocabulary();
                return _instance;
            }
        }

        public IDictionary<string, string> KnownPrefixes
        {
            get
            {
                if (_knownPrefixes.IsNotNull())
                    return _knownPrefixes;
                _knownPrefixes = GetKnownPrefixes();
                return _knownPrefixes;
            }
        }

        public IDictionary<string, string> GetKnownPrefixes()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(PrefixFilePath));
        }
    }
}