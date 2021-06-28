using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Common.Object
{
    public class RequestData
    {
        public List<KeyValuePair<string, string>> parametrLists { get; set; }
        public string name { get; set; }
        public string firstPromotedId { get; set; }
        public string firstRegularId { get; set; }
        public uint firstRegularOffset { get; set; }
        public List<uint> cattegoriesId { get; set; }
    }
}
