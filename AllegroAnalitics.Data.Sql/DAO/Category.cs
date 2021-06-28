using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql.DAO
{
    public class Category
    {
       public uint CategoryId { get; set; }
       public uint CategoryAllegroId { get; set; } 
       public uint OrderId { get; set; } 
       public virtual Order Order { get; set; } 
    }
}
