using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace sharpRDFa
{
    public class Vocabulary
    {
        private const string PrefixFilePath = "Resource\\all.file.json";

        public static IDictionary<string, string> GetCommonPrefixes()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(PrefixFilePath));
        }
    }
}