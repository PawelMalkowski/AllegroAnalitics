using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Common.Object
{
    public class RequestsResult
    {
        public string firstPromotedId { get; set; }
        public string firstRegularId { get; set; }
        public uint firstRegularOffset { get; set; }
        public List<IteamsAtribute> iteamList = new List<IteamsAtribute>();
    }
}
