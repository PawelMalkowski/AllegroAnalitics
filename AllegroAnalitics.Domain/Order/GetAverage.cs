using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Domain.Order
{
    public class GetAverage
    {
       public uint OrderId { get; private set; }
       public DateTime StartDate { get; private set; }
        public GetAverage(uint orderId, DateTime startDate)
        {
            OrderId = orderId;
            StartDate = startDate;
            
        }
    }
}
