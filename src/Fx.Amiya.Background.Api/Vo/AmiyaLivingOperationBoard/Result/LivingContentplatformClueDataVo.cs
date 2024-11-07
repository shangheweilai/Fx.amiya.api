using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Result
{
    public class LivingContentplatformClueDataVo
    {
        /// <summary>
        /// 平台获客总数
        /// </summary>
        public int ContentPlatformTotalClue { get; set; }
        /// <summary>
        /// 平台获客占比
        /// </summary>
        public List<LivingContentplatformClueDataItemVo> ContentPlatformClueRate { get; set; }
        /// <summary>
        /// 抖音获客总数
        /// </summary>
        public int TikTokTotalClue { get; set; }
        /// <summary>
        /// 抖音获客占比
        /// </summary>
        public List<LivingContentplatformClueDataItemVo> TikTokClueRate { get; set; }
        /// <summary>
        /// 视频号获客总数
        /// </summary>
        public int WechatVideoTotalClue { get; set; }
        /// <summary>
        /// 视频号获客占比
        /// </summary>
        public List<LivingContentplatformClueDataItemVo> WechatVideoClueRate { get; set; }
       

    }
    public class LivingContentplatformClueDataItemVo
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
