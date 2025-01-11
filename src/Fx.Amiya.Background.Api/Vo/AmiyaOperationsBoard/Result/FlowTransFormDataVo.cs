using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Result
{
    public class FlowTransFormDataVo
    {
        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 年度月份
        /// </summary>
        public string YearAndMonth { get; set; }
        /// <summary>
        /// 线索量
        /// </summary>
        public int ClueCount { get; set; }
        /// <summary>
        /// 线索有效率
        /// </summary>
        public decimal ClueEffectiveRate { get; set; }
        /// <summary>
        /// 派单数
        /// </summary>
        public decimal SendOrderCount { get; set; }
        /// <summary>
        /// 分诊量
        /// </summary>
        public int DistributeConsulationNum { get; set; }
        /// <summary>
        /// 加v
        /// </summary>
        public int AddWechatCount { get; set; }
        /// <summary>
        /// 加v率
        /// </summary>
        public decimal AddWechatRate { get; set; }
        /// <summary>
        /// 派单率
        /// </summary>
        public decimal SendOrderRate { get; set; }
        /// <summary>
        /// 上门数
        /// </summary>
        public int ToHospitalCount { get; set; }
        /// <summary>
        /// 上门率
        /// </summary>
        public decimal ToHospitalRate { get; set; }
        /// <summary>
        /// 成交
        /// </summary>
        public int DealCount { get; set; }
        /// <summary>
        /// 新客成交量
        /// </summary>
        public int NewCustomerDealCount { get; set; }
        /// <summary>
        /// 老客成交量
        /// </summary>
        public int OldCustomerDealCount { get; set; }
        /// <summary>
        /// 成交率
        /// </summary>
        public decimal DealRate { get; set; }
        /// <summary>
        /// 新客业绩
        /// </summary>
        public decimal NewCustomerPerformance { get; set; }
        /// <summary>
        /// 老客业绩
        /// </summary>
        public decimal OldCustomerPerformance { get; set; }

        /// <summary>
        /// 总业绩
        /// </summary>
        public decimal TotalPerformance { get; set; }
        /// <summary>
        /// 新客客单价
        /// </summary>
        public decimal NewCustomerUnitPrice { get; set; }
        /// <summary>
        /// 老客客单价
        /// </summary>
        public decimal OldCustomerUnitPrice { get; set; }
        /// <summary>
        /// 客单价
        /// </summary>
        public decimal CustomerUnitPrice { get; set; }
        /// <summary>
        /// 新老客占比
        /// </summary>
        public string NewAndOldCustomerRate { get; set; }

        /// <summary>
        /// 贡献
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 当月老客复购率
        /// </summary>
        public decimal OldCustomerBuyRate { get; set; }
    }
}
