using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql.DAO
{
    public class Order
    {
       public Order()
       {
            Categories = new List<Category>();
            Parameters = new List<Parameter>();
            BannedWords = new List<BannedWord>();
            NecessaryWords = new List<NecessaryWord>();
            Requests = new List<Request>();
       } 
       public uint OrderId { get; set; }
       public string UserId { get; set; } 
       public string SerchedPhrase { get; set; }
       public virtual ICollection<Category> Categories { get; set; }
       public virtual ICollection<Parameter> Parameters { get; set; }
       public virtual ICollection<BannedWord> BannedWords { get; set; }
       public virtual ICollection<NecessaryWord> NecessaryWords { get; set; }
       public virtual ICollection<Request> Requests { get; set; }
    }
}
