using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Common.Object
{
    public class IteamsAtribute
    {
        public string id { get; set; }
        public string name { get; set; }
        public Seller seller { get; set; }
        public Promotion promotion { get; set; }
        public Delivery delivery { get; set; }
        public SellingMode sellingMode { get; set; }
        public Stock stock { get; set; }
        public Category category { get; set; }
        public Publication publication { get; set; }
    }
}
