using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ContentPlateFormOrder
{
    public class SplitContentPlatFormOrderDealInfoDto
    {
        public string DealId { get; set; }
        public decimal LocalDealPirce { get; set; }
        public List<decimal> DealPrice { get; set; }
    }
}
