using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assets.Scripts.Json_parser
{
    public class JsonParser
    {
        public Information LoadJson()
        {
            
            using (StreamReader r = new StreamReader(
#if DEBUG
            "dev.info.json"
#else
                "info.json"
#endif
            ))
            {
                string json = r.ReadToEnd();
                Information items = JsonConvert.DeserializeObject<Information>(json);

                return items;
            }

        }
    }

    public class Information
    {
        public string URL;
    }
}