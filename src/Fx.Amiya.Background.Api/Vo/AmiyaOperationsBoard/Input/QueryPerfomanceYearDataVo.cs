using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Input
{
    public class QueryPerfomanceYearDataVo
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 新/老客（可传空）
        /// </summary>
        public bool? IsOldCustomer { get; set; }
    }
}
