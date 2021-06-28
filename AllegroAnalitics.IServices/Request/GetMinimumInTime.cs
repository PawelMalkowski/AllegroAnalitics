using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.IServices.Request
{
    public class GetMinimumInTime
    {
        public uint OrderID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
    }
}
