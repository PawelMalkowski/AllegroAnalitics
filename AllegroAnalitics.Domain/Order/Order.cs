using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Domain.Order
{
    public class Order
    {
        public uint Id { get; set; }
        public string Name { get; private set; }
        public List<int> CattegoryId { get; private set; }
        public List<KeyValuePair<string, string>> ParametrList { get; private set; }
        public List<string> BannedWords { get; private set; }
        public List<string> NecessaryWords { get; private set; }

        public Order(string name, List<int> cattegoryId, 
            List<KeyValuePair<string, string>> parametrList, List<string> bannedWords, List<string> necessaryWords)
        {
            Name = name;
            CattegoryId = cattegoryId;
            ParametrList = parametrList;
            BannedWords = bannedWords;
            NecessaryWords = necessaryWords;
        }
    }
}
