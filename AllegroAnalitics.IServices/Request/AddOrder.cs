using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.IServices.Request
{
    public class AddOrder
    {
        public string Name { get; set; }
        public List<int> CattegoryId { get; set; }
        public List<KeyValuePair<string, string>> ParametrList { get; set; }
        public List<string> BannedWords { get; set; }
        public List<string> NecessaryWords { get; set; }
    }
}
