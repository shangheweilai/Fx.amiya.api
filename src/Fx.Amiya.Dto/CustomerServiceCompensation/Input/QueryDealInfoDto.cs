using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.CustomerServiceCompensation.Input
{
    public class QueryDealInfoDto:BaseQueryDto
    {
        /// <summary>
        /// 上传人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 归属客服
        /// </summary>
        public int? BelongEmpId { get; set; }

        /// <summary>
        /// 业绩类型（1-成交；2-退款；3-稽查）
        /// </summary>
        public string PerformanceType { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }
    }
}
