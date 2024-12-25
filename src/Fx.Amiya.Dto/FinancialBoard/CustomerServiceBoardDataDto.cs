using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.FinancialBoard
{
    public class CustomerServiceBoardDataDto
    {
        public int CustomerServiceId { get; set; }
        public string CustomerServiceName { get; set; }
        public decimal? VisitNumRatio { get; set; }
        public decimal DealPrice { get; set; }
        public decimal TotalServicePrice { get; set; }
        public decimal NewCustomerPrice { get; set; }
        public decimal NewCustomerServicePrice { get; set; }
        public decimal OldCustomerPrice { get; set; }
        public decimal OldCustomerServicePrice { get; set; }

    }

    /// <summary>
    /// 企业微信客服业绩相关功能展示
    /// </summary>
    public class CustomerServiceDetailsPerformanceDto
    {

        public int CustomerServiceId { get; set; }
        public string CustomerServiceName { get; set; }
        /// <summary>
        /// 当月派单+历史派单当月上门率
        /// </summary>
        public decimal? VisitNumRatio { get; set; }
        /// <summary>
        /// 当月派单当月上门率
        /// </summary>
        public decimal? ThisMonthSendThisMonthVisitNumRatio { get; set; }
        public decimal DealPrice { get; set; }
        public decimal TotalServicePrice { get; set; }
        public decimal SupportPrice { get; set; }
        public decimal NewCustomerPrice { get; set; }
        public decimal NewCustomerServicePrice { get; set; }
        public decimal OldCustomerPrice { get; set; }
        public decimal OldCustomerServicePrice { get; set; }

        public int DealPerformanceType { get; set; }
        /// <summary>
        /// 视频业绩
        /// </summary>
        public decimal VideoPerformance { get; set; }
        /// <summary>
        /// 照片业绩
        /// </summary>
        public decimal PicturePerformance { get; set; }

        /// <summary>
        /// 照片/视频业绩占比
        /// </summary>
        public string VideoAndPictureCompare { get; set; }

        /// <summary>
        /// 陪诊业绩
        /// </summary>
        public decimal AcompanyingPerformance { get; set; }
        /// <summary>
        /// 独立业绩
        /// </summary>
        public decimal NotAcompanyingPerformance { get; set; }

        /// <summary>
        /// 独立/陪诊业绩占比
        /// </summary>
        public string IsAcompanyingCompare { get; set; }
        /// <summary>
        /// 0元业绩
        /// </summary>
        public decimal ZeroPerformance { get; set; }
        /// <summary>
        /// 199业绩
        /// </summary>
        public decimal HavingPricePerformance { get; set; }

        /// <summary>
        /// 0元/199业绩占比
        /// </summary>
        public string ZeroAndHavingPriceCompare { get; set; }

        /// <summary>
        /// 历史派单当月成交业绩
        /// </summary>
        public decimal HistorySendThisMonthDealPerformance { get; set; }

        /// <summary>
        /// 当月派单当月成交业绩
        /// </summary>
        public decimal ThisMonthSendThisMonthDealPerformance { get; set; }
        /// <summary>
        /// 历史/当月派单当月成交业绩占比
        /// </summary>
        public string HistoryAndThisMonthCompare { get; set; } 
    }


    /// <summary>
    /// 企业微信客服简单业绩
    /// </summary>
    public class CustomerServiceSimplePerformanceDto
    {

        public int CustomerServiceId { get; set; }
        public string CustomerServiceName { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public string Rank { get; set; }
        /// <summary>
        /// 总业绩
        /// </summary>
        public decimal TotaPrice { get; set; }
        /// <summary>
        /// 辅助业绩
        /// </summary>
        public decimal SupportPrice { get; set; }

        /// <summary>
        /// 新客业绩
        /// </summary>
        public decimal NewCustomerPrice { get; set; }
        /// <summary>
        /// 老客业绩
        /// </summary>
        public decimal OldCustomerPrice { get; set; }
        /// <summary>
        /// 新老客业绩占比
        /// </summary>
        public string NewOrOldCustomerRate { get; set; }
        /// <summary>
        /// 当月派单+历史派单当月上门率
        /// </summary>
        public decimal? VisitRate { get; set; }
        /// <summary>
        /// 当月派单当月上门率
        /// </summary>
        public decimal? ThisMonthSendThisMonthVisitNumRatio { get; set; }

        /// <summary>
        /// 新诊人数
        /// </summary>
        public int NewCustomerNum { get; set; }

        /// <summary>
        /// 复诊人数
        /// </summary>
        public int SequentCustomerNum { get; set; }

        /// <summary>
        /// 成交人数
        /// </summary>
        public int DealNum { get; set; }

        /// <summary>
        /// 老客人数
        /// </summary>
        public int OldCustomerNum { get; set; }

        /// <summary>
        /// 排名集合
        /// </summary>
        public List<CustomerServiceRankDto> CustomerServiceRankDtoList { get; set; }
    }
    /// <summary>
    /// 客服排名类
    /// </summary>

    public class CustomerServiceRankDto
    {
        public int RankId { get; set; }
        public int CustomerServiceId { get; set; }
        public string CustomerServiceName { get; set; }

        public decimal TotalAchievement { get; set; }
    }
}
