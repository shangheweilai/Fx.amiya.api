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
        /// 抖音账号总业绩
        /// </summary>
        public decimal TikTokAccountTotalPerformance { get; set; }
        /// <summary>
        /// 抖音账号业绩占比
        /// </summary>
        public List<LivingContentplatformPerformanceDataItemDto> TikTokAccountPerformanceRate { get; set; }
        /// <summary>
        /// 视频号账号总业绩
        /// </summary>
        public decimal WechatVideoAccountTotalPerformance { get; set; }
        /// <summary>
        /// 视频号账号业绩占比
        /// </summary>
        public List<LivingContentplatformPerformanceDataItemDto> WechatVideoAccountPerformanceRate { get; set; }
    }
    public class LivingContentplatformPerformanceDataItemDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
