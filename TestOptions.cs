using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IserviceColections_MapWhen
{
    public class TestOptions
    {
        public string option_key1 { get; set; }
        public SubTestOptions option_key2 { get; set; }

    }

    public class SubTestOptions
    {
        public string k1 { get; set; }
        public string k2 { get; set; }

    }
}
