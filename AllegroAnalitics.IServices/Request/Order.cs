using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.IServices.Request
{
    public class Order
    {
        public uint id { get; set; }
        public string Name { get; set; }
        public List<uint> CattegoryId { get; set; }
        public List<KeyValuePair<string, string>> ParametrList { get; set; }
        public List<string> BannedWords { get; set; }
        public List<string> NecessaryWords { get; set; }
    }
}
