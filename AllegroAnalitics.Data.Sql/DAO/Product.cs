using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql.DAO
{
    public class Product
    {
        public uint ProductId { get; set; }
        public string AllegroProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public double DeliveryPrice { get; set; }
        public bool BuyNow { get; set; }
        public uint AvilableIteams { get; set; }
        public uint CategorryId { get; set; }
        public uint RequestId { get; set; }
        public virtual Request Request { get; set; }
    }
}
