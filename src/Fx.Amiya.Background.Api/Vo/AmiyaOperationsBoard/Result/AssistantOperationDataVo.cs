using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Result
{
    public class AssistantOperationDataVo
    {

        /// <summary>
        /// 新客业绩
        /// </summary>
        public AssistantNewCustomerOperationDataVo NewCustomerData { get; set; }
        /// <summary>
        /// 老客业绩
        /// </summary>

        public AssistantOldCustomerOperationDataVo OldCustomerData { get; set; }
    }


    /// <summary>
    /// 新客业绩输出类
    /// </summary>
    public class AssistantNewCustomerOperationDataVo
    {
        /// <summary>
        /// 线索有效率
        /// </summary>
        public decimal? ClueEffectiveRate { get; set; }
        /// <summary>
        /// 线索有效率健康值
        /// </summary>
        public decimal ClueEffectiveRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 加v率
        /// </summary>
        public decimal? AddWeChatRate { get; set; }
        /// <summary>
        /// 加v率健康值(累计)
        /// </summary>
        public decimal AddWeChatRateHealthValueSum { get; set; }
        /// <summary>
        /// 加v率健康值(当月)
        /// </summary>
        public decimal AddWeChatRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 派单率
        /// </summary>
        public decimal? SendOrderRate { get; set; }
        /// <summary>
        /// 派单率健康值(累计)
        /// </summary>
        public decimal SendOrderRateHealthValueSum { get; set; }
        /// <summary>
        /// 派单率健康值(当月)
        /// </summary>
        public decimal SendOrderRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 上门率
        /// </summary>
        public decimal? ToHospitalRate { get; set; }
        /// <summary>
        /// 上门率健康值(累计)
        /// </summary>
        public decimal ToHospitalRateHealthValueSum { get; set; }
        /// <summary>
        /// 上门率健康值(当月)
        /// </summary>
        public decimal ToHospitalRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 成交率
        /// </summary>
        public decimal? DealRate { get; set; }
        /// <summary>
        /// 成交率健康值(累计）
        /// </summary>
        public decimal DealRateHealthValueSum { get; set; }
        /// <summary>
        /// 成交率健康值(当月)
        /// </summary>
        public decimal DealRateHealthValueThisMonth { get; set; }

        /// <summary>
        /// 分诊成交转化率
        /// </summary>
        public decimal? AllocationConsulationToDealRate { get; set; }

        /// <summary>
        /// 派单成交转化率
        /// </summary>
        public decimal? SendOrderToDealRate { get; set; }

        /// <summary>
        /// 漏斗图详情数据
        /// </summary>
        public List<AssistantNewCustomerOperationDataDetailsVo> newCustomerOperationDataDetails { get; set; }

    }

    /// <summary>
    /// 老客业绩输出类
    /// </summary>
    public class AssistantOldCustomerOperationDataVo
    {
        /// <summary>
        /// 总成交人数
        /// </summary>
        public int TotalDealPeople { get; set; }

        /// <summary>
        /// 二次复购人数
        /// </summary>
        public int SecondDealPeople { get; set; }


        /// <summary>
        /// 三次复购人数
        /// </summary>
        public int ThirdDealPeople { get; set; }
        /// <summary>
        /// 四次复购人数
        /// </summary>
        public int FourthDealCustomer { get; set; }
        /// <summary>
        /// 五次及以上复购人数
        /// </summary>
        public int FifThOrMoreOrMoreDealCustomer { get; set; }


        /// <summary>
        /// 二次转换率
        /// </summary>
        public decimal SecondTimeBuyRate { get; set; }
        /// <summary>
        /// 二次复购占比
        /// </summary>
        public decimal SecondTimeBuyRateProportion { get; set; }
        /// <summary>
        /// 三次转换率
        /// </summary>
        public decimal ThirdTimeBuyRate { get; set; }
        /// <summary>
        /// 三次复购占比
        /// </summary>
        public decimal ThirdTimeBuyRateProportion { get; set; }
        /// <summary>
        /// 四次转换率
        /// </summary>
        public decimal FourthTimeBuyRate { get; set; }
        /// <summary>
        /// 四次复购占比
        /// </summary>
        public decimal FourthTimeBuyRateProportion { get; set; }
        /// <summary>
        /// 五次转换率
        /// </summary>
        public decimal FifthTimeOrMoreBuyRate { get; set; }
        /// <summary>
        /// 五次及以上复购占比
        /// </summary>
        public decimal FifthTimeOrMoreBuyRateProportion { get; set; }
        /// <summary>
        /// 复购率
        /// </summary>
        public decimal BuyRate { get; set; }
        /// <summary>
        /// 二次复购周期
        /// </summary>
        public decimal SecondDealCycle { get; set; }
        /// <summary>
        /// 三次复购周期
        /// </summary>
        public decimal ThirdDealCycle { get; set; }
        /// <summary>
        /// 四次复购周期
        /// </summary>
        public decimal FourthDealCycle { get; set; }
        /// <summary>
        /// 五次复购周期
        /// </summary>
        public decimal FifthDealCycle { get; set; }

    }
    /// <summary>
    /// 业绩输出详情
    /// </summary>
    public class AssistantNewCustomerOperationDataDetailsVo
    {
        /// <summary>
        /// 标识码
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 目标完成率
        /// </summary>
        public decimal? TargetCompleteRate { get; set; }

        /// <summary>
        /// 同比值
        /// </summary>
        public decimal? YearToYearValue { get; set; }

        /// <summary>
        /// 环比值
        /// </summary>
        public decimal? ChainRatioValue { get; set; }
    }
}
