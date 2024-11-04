using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard.Input
{
    public class QueryLivingDataDto
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
        /// 是否当月
        /// </summary>
        public bool IsCurrent { get; set; } = false;
        /// <summary>
        /// 基础主播id(传空查所有)
        /// </summary>
        public string? BaseLiveAnchorId { get; set; }
    }
}
