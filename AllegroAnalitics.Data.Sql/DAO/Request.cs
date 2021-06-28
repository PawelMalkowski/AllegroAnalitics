using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql.DAO
{
    public class Request
    {
        public Request()
        {
            Products = new List<Product>();
        }
        public uint RequestId { get; set; }
        public DateTime Timestamp { get; set; }
        public string firstPromotedId { get; set; }
        public string firstRegularId { get; set; }
        public uint firstRegularOffset { get; set; }
        public uint CountRecords { get; set; }
        public uint OrderId { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
