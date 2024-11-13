using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Input
{
    public class QueryAssistantPerformanceVo
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 助理id
        /// </summary>
        public int AssistantId { get; set; }
        /// <summary>
        /// 是否为当月
        /// </summary>
        public bool IsCurrent { get; set; }
    }
    /// <summary>
    /// 运营看板查询主播周期转化数据
    /// </summary>
    public class QueryLiveAnchorPerformanceVo
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 主播基础id(可空)
        /// </summary>
        public string LiveAnchorBaseId { get; set; }
    }
}
