using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ContentPlateFormOrder.Input
{
    public class SplitContentPlatformOrderDealInfoVo
    {
        public string DealId { get; set; }
        public decimal LocalDealPirce { get; set; }
        public List<decimal> Price { get; set; }
    }
}
