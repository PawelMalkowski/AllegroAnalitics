using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql.DAO
{
    public class BannedWord
    {
        public uint BannedWordId { get; set; }
        public string Word { get; set; }
        public uint OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
