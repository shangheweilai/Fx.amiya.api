using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingCustomerAndPerformanceDataDto
    {
        /// <summary>
        /// 累计客资数
        /// </summary>
        public decimal ClueCount { get; set; }
        /// <summary>
        /// 当日客资数
        /// </summary>
        public decimal CurrentClueCount { get; set; }
        /// <summary>
        /// 客资目标完成率
        /// </summary>
        public decimal ClueTargetCompleteRate { get; set; }
        /// <summary>
        /// 客资环比
        /// </summary>
        public decimal ClueChain { get; set; }
        /// <summary>
        /// 客资同比
        /// </summary>
        public decimal ClueYearOnYear { get; set; }
        /// <summary>
        /// 累计业绩
        /// </summary>
        public decimal Performance { get; set; }
        /// <summary>
        /// 当日业绩
        /// </summary>
        public decimal CurrentPerformance { get; set; }
        /// <summary>
        /// 当月业绩
        /// </summary>
        public decimal CurrentMontPerformance { get; set; }
        /// <summary>
        /// 业绩环比
        /// </summary>
        public decimal PerformanceChain { get; set; }
        /// <summary>
        /// 业绩同比
        /// </summary>
        public decimal PerformanceYearOnYear { get; set; }
    }
}
