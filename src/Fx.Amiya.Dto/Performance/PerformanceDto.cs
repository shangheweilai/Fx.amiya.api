using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.Performance
{
    public class PerformanceDto
    {
        public string Id { get; set; }
        public int? LiveAnchorId { get; set; }
        public decimal Price { get; set; }
        public int ToHospitalType  { get; set; }
        public DateTime CreateDate { get; set; }

        public bool IsOldCustomer { get; set; }
        public int BelongChannel { get; set; }
        public DateTime SendDate { get; set; }

        public decimal AddOrderPrice { get; set; }

        public string ContentPlatFormId { get; set; }
        public int ConsulationType { get; set; }
        /// <summary>
        /// 上一条成交情况id（当业绩类型为“助理补单时有值）
        /// </summary>
        public string LastDealInfoId { get; set; }
    }
}
