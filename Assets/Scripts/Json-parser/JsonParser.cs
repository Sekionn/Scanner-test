using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Json_parser
{
    public class JsonParser
    {
        public Information LoadJson()
        {
            var jsonfile = Resources.Load<TextAsset>(
#if DEBUG
            "Text/dev.info"
#elif DEVELOPMENT_BUILD
                "Text/info"
#else
                "Text/info"
#endif
            );
            
            Information items = JsonConvert.DeserializeObject<Information>(jsonfile.text);

            return items;
        }
    }

    public class Information
    {
        public string URL;
    }
}