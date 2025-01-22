using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaOperationsBoardService.Input
{
    public class QueryTransformDataDto
    {
        /// <summary>
        /// 基础主播id
        /// </summary>
        public string BaseLiveAnchorId { get; set; }
        /// <summary>
        /// 平台id
        /// </summary>
        public List<string> ContentPlatFormIds { get; set; }
        /// <summary>
        /// 抖音
        /// </summary>
        public bool ShowTikTok { get; set; }
        /// <summary>
        /// 小红书
        /// </summary>
        public bool ShowXiaoHongShu { get; set; }
        /// <summary>
        /// 视频号
        /// </summary>
        public bool ShowWechatVideo { get; set; }
        /// <summary>
        /// 私域
        /// </summary>
        public bool ShowPrivateDomain { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 是否是当月数据
        /// </summary>
        public bool? IsCurrentMonth { get; set; }

        /// <summary>
        /// 助理id
        /// </summary>
        public int? AssistantId { get; set; }
    }
}
