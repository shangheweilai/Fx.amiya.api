using Fx.Amiya.Dto.AmiyaLivingOperationBoard;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Result
{
    /// <summary>
    /// 名索运营管理总业绩接口返回类
    /// </summary>
    public class OperationMingSuoAchievementDataDto
    {
        /// <summary>
        /// 总线索
        /// </summary>
        public decimal TotalClues { get; set; }
        /// <summary>
        /// 当日总线索
        /// </summary>
        public decimal TodayTotalClues { get; set; }

        /// <summary>
        /// 总线索目标完成率
        /// </summary>
        public decimal? TotalCluesCompleteRate { get; set; }

        /// <summary>
        /// 总线索同比
        /// </summary>
        public decimal? TotalCluesYearOnYear { get; set; }

        /// <summary>
        /// 总线索环比
        /// </summary>
        public decimal? TotalCluesChainRatio { get; set; }
        /// <summary>
        /// 总业绩
        /// </summary>
        public decimal TotalPerformance { get; set; }
        /// <summary>
        /// 当日总业绩
        /// </summary>
        public decimal TodayTotalPerformance { get; set; }

        /// <summary>
        /// 总业绩目标完成率
        /// </summary>
        public decimal? TotalPerformanceCompleteRate { get; set; }

        /// <summary>
        /// 总业绩同比
        /// </summary>
        public decimal? TotalPerformanceYearOnYear { get; set; }

        /// <summary>
        /// 总业绩环比
        /// </summary>
        public decimal? TotalPerformanceChainRatio { get; set; }

        /// <summary>
        /// 新客业绩
        /// </summary>
        public decimal NewCustomerPerformance { get; set; }
        /// <summary>
        /// 当日新客业绩
        /// </summary>
        public decimal TodayNewCustomerPerformance { get; set; }


        /// <summary>
        /// 新客业绩目标完成率
        /// </summary>
        public decimal? NewCustomerPerformanceCompleteRate { get; set; }

        /// <summary>
        /// 新客业绩同比
        /// </summary>
        public decimal? NewCustomerPerformanceYearOnYear { get; set; }

        /// <summary>
        /// 新客业绩环比
        /// </summary>
        public decimal? NewCustomerPerformanceChainRatio { get; set; }

        /// <summary>
        /// 老客业绩
        /// </summary>
        public decimal OldCustomerPerformance { get; set; }
        /// <summary>
        /// 当日老客业绩
        /// </summary>
        public decimal TodayOldCustomerPerformance { get; set; }


        /// <summary>
        /// 老客业绩目标完成率
        /// </summary>
        public decimal? OldCustomerPerformanceCompleteRate { get; set; }

        /// <summary>
        /// 老客业绩同比
        /// </summary>
        public decimal? OldCustomerPerformanceYearOnYear { get; set; }

        /// <summary>
        /// 老客业绩环比
        /// </summary>
        public decimal? OldCustomerPerformanceChainRatio { get; set; }
        /// <summary>
        /// 总业绩折线图
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> TotalPerformanceBrokenLineList { get; set; }
        /// <summary>
        /// 总线索折线图
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> TotalCluesBrokenLineList { get; set; }

    }

    /// <summary>
    /// 名索漏斗图返回类
    /// </summary>
    public class ResultMingSuoOperationDataDto
    {
        /// <summary>
        /// 新客业绩
        /// </summary>
        public AssistantNewCustomerOperationDataDto NewCustomerData { get; set; }
        /// <summary>
        /// 老客业绩
        /// </summary>

        public AssistantOldCustomerOperationDataDto OldCustomerData { get; set; }
    }

    /// <summary>
    /// 名索转化周期返回类
    /// </summary>
    public class MingSuoTransformCycleDataDto
    {

        /// <summary>
        /// 当前医生IP当月分诊派单转化周期
        /// </summary>
        public int ThisMonthSendCycle { get; set; }
        /// <summary>
        /// 当前医生IP当月分诊上门转化周期
        /// </summary>
        public int ThisMonthToHospitalCycle { get; set; }
        /// <summary>
        /// 当前助历史分诊派单转化周期
        /// </summary>
        public int HistorySendCycle { get; set; }
        /// <summary>
        /// 当前医生IP历史分诊上门转化周期
        /// </summary>
        public int HistoryToHospitalCycle { get; set; }
        /// <summary>
        /// 当前医生IP总分诊派单转化周期
        /// </summary>
        public int TotalSendCycle { get; set; }
        /// <summary>
        /// 当前医生IP总分诊上门转化周期
        /// </summary>
        public int TotalToHospitalCycle { get; set; }
        /// <summary>
        /// 分诊派单转化周期柱状图数据
        /// </summary>
        public List<KeyValuePair<string, int>> SendCycleData { get; set; }
        /// <summary>
        /// 分诊上门转化周期柱状图数据
        /// </summary>
        public List<KeyValuePair<string, int>> ToHospitalCycleData { get; set; }
    }

    /// <summary>
    /// 名索目标完成率返回类
    /// </summary>
    public class MingSuoClueTargetDataDto
    {
        /// <summary>
        /// 线索目标完成率
        /// </summary>
        public List<KeyValuePair<string, decimal>> ClueTargetComplete { get; set; }
        /// <summary>
        /// 业绩目标完成率
        /// </summary>
        public List<BaseKeyValueDto<string, decimal>> PerformanceTargetComplete { get; set; }
    }

    public class MingSuoContentplatformClueDataDto
    {
        /// <summary>
        /// 平台获客总数
        /// </summary>
        public int ContentPlatformTotalClue { get; set; }
        /// <summary>
        /// 平台获客占比
        /// </summary>
        public List<LivingContentplatformClueDataItemDto> ContentPlatformClueRate { get; set; }
    }

    public class MingSuoContentplatformPerformanceDataDto
    {
        /// <summary>
        /// 平台总业绩
        /// </summary>
        public decimal ContentPlatformTotalPerformance { get; set; }
        /// <summary>
        /// 平台业绩占比
        /// </summary>
        public List<LivingContentplatformPerformanceDataItemDto> ContentPlatformPerformanceRate { get; set; }
    }
}
