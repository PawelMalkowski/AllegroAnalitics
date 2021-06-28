using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Domain.Order
{
    public class GetMinimum
    {
        public uint OrderId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime StopDate { get; private set; }
        public GetMinimum(uint orderId, DateTime startDate, DateTime stopDate)
        {
            OrderId = orderId;
            StartDate = startDate;
            StopDate = stopDate;

        }
    }
}
