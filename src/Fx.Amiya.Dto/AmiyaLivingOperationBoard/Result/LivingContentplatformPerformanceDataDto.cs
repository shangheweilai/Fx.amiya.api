using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingContentplatformPerformanceDataDto
    {
        /// <summary>
        /// 平台总业绩
        /// </summary>
        public decimal ContentPlatformTotalPerformance { get; set; }
        /// <summary>
        /// 平台业绩占比
        /// </summary>
        public List<LivingContentplatformPerformanceDataItemDto> ContentPlatformPerformanceRate { get; set; }
        /// <summary>
        /// 账号总业绩
        /// </summary>
        public decimal AccountTotalPerformance { get; set; }
        /// <summary>
        /// 账号业绩占比
        /// </summary>
        public List<LivingContentplatformPerformanceDataItemDto> AccountPerformanceRate { get; set; }
    }
    public class LivingContentplatformPerformanceDataItemDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
