using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaMingSuoOperationBoard.Input
{
    public class QueryMingSuoFilterDataVo
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
    public class QueryMingSuoCompleteDataVo
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


}
