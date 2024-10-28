using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.CustomerServiceCheckPerformance.Input
{
    public class QueryCustomerServiceCheckPerformanceVo : BaseQueryVo
    {
        public bool? Valid { get; set; }
        /// <summary>
        /// 归属客服
        /// </summary>
        public int? BelongEmpId { get; set; }

        /// <summary>
        /// 稽查客服
        /// </summary>
        public int? CheckEmpId { get; set; }

        /// <summary>
        /// 业绩类型合集
        /// </summary>
        public string PerformanceTypeList { get; set; }
        /// <summary>
        /// 薪资单id
        /// </summary>
        public string customerServiceCompensationId { get; set; }
    }
}
