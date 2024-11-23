using Fx.Amiya.Dto.AmiyaLivingOperationBoard;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Input
{

    public class QueryMingSuoFilterDataDto
    {
        /// <summary>
        /// 当月
        /// </summary>
        public bool Current { get; set; }
        /// <summary>
        /// 历史
        /// </summary>
        public bool History { get; set; }

        /// <summary>
        /// 新老客（false为新客，true为老客）
        /// </summary>
        public bool IsOldCustomer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string LiveAnchorBaseId { get; set; }
    }
    public class QueryMingSuoCompleteDataDto
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
        /// 基础主播id
        /// </summary>
        public string? BaseLiveAnchorId { get; set; }
    }
    public class QueryMingSuoAssistantPerformanceDto
    {
        /// <summary>
        /// 开始时间(开始时间结束时间都为null时查询当日数据)
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 基础主播id
        /// </summary>
        public string LiveAnchorBaseId{ get; set; }
        /// <summary>
        /// 是否为当月
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}
