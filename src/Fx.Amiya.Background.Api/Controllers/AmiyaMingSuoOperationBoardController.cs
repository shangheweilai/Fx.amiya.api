using Fx.Amiya.Background.Api.Vo;
using Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Result;
using Fx.Amiya.Background.Api.Vo.AmiyaMingSuoOperationBoard.Input;
using Fx.Amiya.Background.Api.Vo.AmiyaMingSuoOperationBoard.Result;
using Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Input;
using Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Result;
using Fx.Amiya.Background.Api.Vo.Performance.AmiyaPerformance2.Result;
using Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Input;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Input;
using Fx.Amiya.IService;
using Fx.Authorization.Attributes;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 名索运营看板
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class AmiyaMingSuoOperationBoardController : ControllerBase
    {
        private readonly IAmiyaOperationsBoardService amiyaOperationsBoardService;
        private readonly IAmiyaMingSuoOperationBoardService amiyaMingSuoOperationBoardService;

        public AmiyaMingSuoOperationBoardController(
            IAmiyaOperationsBoardService amiyaOperationsBoardService,
            IAmiyaMingSuoOperationBoardService amiyaMingSuoOperationBoardService
            )
        {
            this.amiyaOperationsBoardService = amiyaOperationsBoardService;
            this.amiyaMingSuoOperationBoardService = amiyaMingSuoOperationBoardService;
        }


        #region 【名索数据运营看板】
        /// <summary>
        /// 根据条件获取名索数据运营看板
        /// </summary>
        /// <returns></returns>
        [HttpGet("getMingSuoAchievementAndDateSchedule")]
        public async Task<ResultData<OperationMingSuoAchievementDataVo>> GetMingSuoAchievementAndDateScheduleAsync([FromQuery] QueryOperationDataVo query)
        {
            OperationMingSuoAchievementDataVo result = new OperationMingSuoAchievementDataVo();
            QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
            queryOperationDataVo.startDate = query.startDate;
            queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryOperationDataVo.keyWord = query.keyWord;
            var data = await amiyaMingSuoOperationBoardService.GetTotalAchievementAndDateScheduleAsync(queryOperationDataVo);

            result.NewCustomerPerformance = data.NewCustomerPerformance;
            result.NewCustomerPerformanceCompleteRate = data.NewCustomerPerformanceCompleteRate;
            result.NewCustomerPerformanceYearOnYear = data.NewCustomerPerformanceYearOnYear;
            result.NewCustomerPerformanceChainRatio = data.NewCustomerPerformanceChainRatio;
            result.TodayNewCustomerPerformance = data.TodayNewCustomerPerformance;

            result.OldCustomerPerformance = data.OldCustomerPerformance;
            result.OldCustomerPerformanceCompleteRate = data.OldCustomerPerformanceCompleteRate;
            result.OldCustomerPerformanceYearOnYear = data.OldCustomerPerformanceYearOnYear;
            result.OldCustomerPerformanceChainRatio = data.OldCustomerPerformanceChainRatio;
            result.TodayOldCustomerPerformance = data.TodayOldCustomerPerformance;

            result.TotalPerformance = data.TotalPerformance;
            result.TotalPerformanceCompleteRate = data.TotalPerformanceCompleteRate;
            result.TotalPerformanceYearOnYear = data.TotalPerformanceYearOnYear;
            result.TotalPerformanceChainRatio = data.TotalPerformanceChainRatio;
            result.TodayTotalPerformance = data.TodayTotalPerformance;

            result.TotalPerformanceBrokenLineList = data.TotalPerformanceBrokenLineList.Select(x => new PerformanceBrokenLineListInfoVo
            {
                date = x.date,
                Performance = x.Performance
            }).ToList();

            return ResultData<OperationMingSuoAchievementDataVo>.Success().AddData("data", result);
        }


        /// <summary>
        /// 获取名索漏斗图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getMingSuoFilterData")]
        public async Task<ResultData<MingSuoOperationDataVo>> GetMingSuoFilterDataAsync([FromQuery] QueryMingSuoFilterDataVo query)
        {
            QueryMingSuoFilterDataDto queryDto = new QueryMingSuoFilterDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.LiveAnchorBaseId = query.LiveAnchorBaseId;
            queryDto.Current = query.Current;
            queryDto.History = query.History;
            var res = await amiyaMingSuoOperationBoardService.GetMingSuoFilterDataAsync(queryDto);
            MingSuoOperationDataVo data = new MingSuoOperationDataVo();
            data.NewCustomerData = new AssistantNewCustomerOperationDataVo(); 
            data.OldCustomerData =new AssistantOldCustomerOperationDataVo ();
            data.NewCustomerData.AddWeChatRate = res.NewCustomerData.AddWeChatRate;
            data.NewCustomerData.AddWeChatRateHealthValueThisMonth = res.NewCustomerData.AddWeChatRateHealthValueThisMonth;
            data.NewCustomerData.SendOrderRate = res.NewCustomerData.SendOrderRate;
            data.NewCustomerData.SendOrderRateHealthValueThisMonth = res.NewCustomerData.SendOrderRateHealthValueThisMonth;
            data.NewCustomerData.ToHospitalRateHealthValueThisMonth = res.NewCustomerData.ToHospitalRateHealthValueThisMonth;
           
            data.NewCustomerData.ToHospitalRate = res.NewCustomerData.ToHospitalRate;
            data.NewCustomerData.DealRate = res.NewCustomerData.DealRate;
            data.NewCustomerData.DealRateHealthValueThisMonth = res.NewCustomerData.DealRateHealthValueThisMonth;
            data.NewCustomerData.newCustomerOperationDataDetails = res.NewCustomerData.newCustomerOperationDataDetails.Select(e => new AssistantNewCustomerOperationDataDetailsVo
            {
                Key = e.Key,
                Value = e.Value,
                Name = e.Name
            }).ToList();

            data.OldCustomerData.TotalDealPeople = res.OldCustomerData.TotalDealPeople;
            data.OldCustomerData.SecondDealPeople = res.OldCustomerData.SecondDealPeople;
            data.OldCustomerData.ThirdDealPeople = res.OldCustomerData.ThirdDealPeople;
            data.OldCustomerData.FourthDealCustomer = res.OldCustomerData.FourthDealCustomer;
            data.OldCustomerData.FifThOrMoreOrMoreDealCustomer = res.OldCustomerData.FifThOrMoreOrMoreDealCustomer;
            data.OldCustomerData.SecondTimeBuyRate = res.OldCustomerData.SecondTimeBuyRate;
            data.OldCustomerData.SecondTimeBuyRateProportion = res.OldCustomerData.SecondTimeBuyRateProportion;
            data.OldCustomerData.ThirdTimeBuyRate = res.OldCustomerData.ThirdTimeBuyRate;
            data.OldCustomerData.ThirdTimeBuyRateProportion = res.OldCustomerData.ThirdTimeBuyRateProportion;
            data.OldCustomerData.FourthTimeBuyRate = res.OldCustomerData.FourthTimeBuyRate;
            data.OldCustomerData.FourthTimeBuyRateProportion = res.OldCustomerData.FourthTimeBuyRateProportion;
            data.OldCustomerData.FifthTimeOrMoreBuyRate = res.OldCustomerData.FifthTimeOrMoreBuyRate;
            data.OldCustomerData.FifthTimeOrMoreBuyRateProportion = res.OldCustomerData.FifthTimeOrMoreBuyRateProportion;
            data.OldCustomerData.BuyRate = res.OldCustomerData.BuyRate;
            data.OldCustomerData.SecondDealCycle = res.OldCustomerData.SecondDealCycle;
            data.OldCustomerData.ThirdDealCycle = res.OldCustomerData.ThirdDealCycle;
            data.OldCustomerData.FourthDealCycle = res.OldCustomerData.FourthDealCycle;
            data.OldCustomerData.FifthDealCycle = res.OldCustomerData.FifthDealCycle;
            return ResultData<MingSuoOperationDataVo>.Success().AddData("data", data);
        }


        /// <summary>
        /// 名索运营看板周期转化
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getMingSuoTransformCycleData")]
        public async Task<ResultData<MingSuoTransformCycleDataVo>> GetMingSuoTransformCycleDataAsync([FromQuery] QueryLiveAnchorPerformanceVo query)
        {
            MingSuoTransformCycleDataVo data = new MingSuoTransformCycleDataVo();
            QueryOperationDataDto queryDto = new QueryOperationDataDto();
            queryDto.startDate = query.StartDate;
            queryDto.endDate = query.EndDate;
            queryDto.keyWord = query.LiveAnchorBaseId;
            var res = await amiyaMingSuoOperationBoardService.GetLiveAnchorTransformCycleDataAsync(queryDto);
            data.ThisMonthSendCycle = res.ThisMonthSendCycle;
            data.ThisMonthToHospitalCycle = res.ThisMonthToHospitalCycle;
            data.HistorySendCycle = res.HistorySendCycle;
            data.HistoryToHospitalCycle = res.HistoryToHospitalCycle;
            //data.TotalSendCycle = res.TotalSendCycle;
            //data.TotalToHospitalCycle = res.TotalToHospitalCycle;
            data.SendCycleData = res.SendCycleData;
            data.ToHospitalCycleData = res.ToHospitalCycleData;

            return ResultData<MingSuoTransformCycleDataVo>.Success().AddData(data);
        }


        /// <summary>
        /// 名索线索和业绩目标完成率
        /// </summary>
        /// <returns></returns>
        [HttpGet("getMingSuoClueAndPerformanceTargetData")]
        public async Task<ResultData<MingSuoClueTargetDataVo>> GetMingSuoClueTargetDataAsync([FromQuery] QueryMingSuoCompleteDataVo query)
        {
            MingSuoClueTargetDataVo data = new MingSuoClueTargetDataVo();
            QueryMingSuoCompleteDataDto queryDto = new QueryMingSuoCompleteDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaMingSuoOperationBoardService.GetMingSuoClueAndPerformanceTargetDataAsync(queryDto);
            data.ClueTargetComplete = res.ClueTargetComplete;
            data.PerformanceTargetComplete = new List<BaseIdAndNameVo<string, decimal>>();
            foreach (var x in res.PerformanceTargetComplete)
            {
                BaseIdAndNameVo<string, decimal> performanceTarget = new BaseIdAndNameVo<string, decimal>();
                performanceTarget.Id = x.Key;
                performanceTarget.Name = x.Value;
                data.PerformanceTargetComplete.Add(performanceTarget);
            }
            return ResultData<MingSuoClueTargetDataVo>.Success().AddData("data", data);
        }

        /// <summary>
        /// 助理目标完成率和助理业绩占比柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("mingsuoAssiatantTargetCompleteAndPerformanceRateData")]
        public async Task<ResultData<AssiatantTargetCompleteAndPerformanceRateVo>> GetMingSuoAssiatantTargetCompleteAndPerformanceRateDataAsync([FromQuery] QueryMingSuoCompleteDataVo query)
        {
            AssiatantTargetCompleteAndPerformanceRateVo result = new AssiatantTargetCompleteAndPerformanceRateVo();
            QueryMingSuoAssistantPerformanceDto queryDto = new QueryMingSuoAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.LiveAnchorBaseId = query.BaseLiveAnchorId;
            //queryDto.IsCurrent = query.IsCurrent;
            var res = await amiyaMingSuoOperationBoardService.GetAssiatantTargetCompleteAndPerformanceRateDataAsync(queryDto);
            result.PerformanceRateData = res.PerformanceRateData.OrderByDescending(e => e.Value).Select(e => new BaseIdAndNameVo<string, decimal>
            {
                Id = e.Key,
                Name = e.Value,
            }).ToList();
            result.TargetCompleteData = res.TargetCompleteData.OrderByDescending(e => e.Value).Select(e => new BaseIdAndNameVo<string, decimal>
            {
                Id = e.Key,
                Name = e.Value,
            }).ToList();
            result.PerformanceRateData.RemoveAll(e => e.Name == 0m);
            result.TargetCompleteData.RemoveAll(e => e.Name == 0m);
            return ResultData<AssiatantTargetCompleteAndPerformanceRateVo>.Success().AddData("data", result);
        }

        /// <summary>
        /// 名索账号获客占比
        /// </summary>
        /// <returns></returns>
        [HttpGet("getMingSuoContentplatformClueData")]
        public async Task<ResultData<MingSuoContentplatformClueDataVo>> GetMingSuoContentplatformClueDataAsync([FromQuery] QueryMingSuoCompleteDataVo query)
        {
            MingSuoContentplatformClueDataVo data = new MingSuoContentplatformClueDataVo();
            QueryMingSuoAssistantPerformanceDto queryDto = new QueryMingSuoAssistantPerformanceDto();
            queryDto.LiveAnchorBaseId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaMingSuoOperationBoardService.GetMingSuoContentplatformClueDataAsync(queryDto);
            data.ContentPlatformTotalClue = res.ContentPlatformTotalClue;
            data.ContentPlatformClueRate = res.ContentPlatformClueRate.Select(e => new LivingContentplatformClueDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            return ResultData<MingSuoContentplatformClueDataVo>.Success().AddData("data", data);
        }

        /// <summary>
        /// 名索账号业绩占比
        /// </summary>
        /// <returns></returns>
        [HttpGet("getMingSuoContentplatformPerformanceData")]
        public async Task<ResultData<MingSuoContentplatformPerformanceDataVo>> GetMingSuoContentplatformPerformanceDataAsync([FromQuery] QueryMingSuoCompleteDataVo query)
        {
            MingSuoContentplatformPerformanceDataVo data = new MingSuoContentplatformPerformanceDataVo();
            QueryMingSuoAssistantPerformanceDto queryDto = new QueryMingSuoAssistantPerformanceDto();
            queryDto.LiveAnchorBaseId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaMingSuoOperationBoardService.GetMingSuoContentplatformPerformanceDataAsync(queryDto);
            data.ContentPlatformTotalPerformance = res.ContentPlatformTotalPerformance;
            data.ContentPlatformPerformanceRate = res.ContentPlatformPerformanceRate.Select(e => new LivingContentplatformPerformanceDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            return ResultData<MingSuoContentplatformPerformanceDataVo>.Success().AddData("data", data);
        }

        #endregion
    }
}
