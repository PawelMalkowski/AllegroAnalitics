using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.IServices.Request
{
    public class GetAverage
    {

        public uint OrderID { get; set; }
        public DateTime StartDate { get; set; }
    }
}
