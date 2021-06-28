using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql.DAO
{
    public class Parameter
    {
        public uint ParameterId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public uint OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
