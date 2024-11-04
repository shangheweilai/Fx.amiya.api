using Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Input;
using Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Result;
using Fx.Amiya.Background.Api.Vo.Performance.AmiyaPerformance2.Result;
using Fx.Amiya.Dto.AmiyaLivingOperationBoard.Input;
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
    /// 直播中运营看板
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class AmiyaLivingOperationBoardController : ControllerBase
    {
        private readonly IAmiyaLivingOperationBoardService amiyaLivingOperationBoardService;

        public AmiyaLivingOperationBoardController(IAmiyaLivingOperationBoardService amiyaLivingOperationBoardService)
        {
            this.amiyaLivingOperationBoardService = amiyaLivingOperationBoardService;
        }
        /// <summary>
        /// 直播中客资和新客业绩
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingCustomerAndPerformanceData")]
        public async Task<ResultData<LivingCustomerAndPerformanceDataVo>> GetLivingCustomerAndPerformanceDataAsync([FromQuery]QueryLivingDataVo query){
            LivingCustomerAndPerformanceDataVo data = new LivingCustomerAndPerformanceDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            
            var res =await amiyaLivingOperationBoardService.GetLivingCustomerAndPerformanceDataAsync(queryDto);
            data.ClueCount = res.ClueCount;
            data.Performance = res.Performance;
            return ResultData<LivingCustomerAndPerformanceDataVo>.Success().AddData("data",data);
        }
        /// <summary>
        /// 直播中客资和业绩折线图
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingCustomerAndPerformanceBrokenLineData")]
        public async Task<ResultData<LivingCustomerAndPerformanceBrokenLineDataVo>> GetLivingCustomerAndPerformanceBrokenLineDataAsync([FromQuery]QueryLivingDataVo query){
            LivingCustomerAndPerformanceBrokenLineDataVo data = new LivingCustomerAndPerformanceBrokenLineDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            
            var res = await amiyaLivingOperationBoardService.GetLivingCustomerAndPerformanceBrokenLineDataAsync(queryDto);
            data.ClueData = res.ClueData.Select(e => new PerformanceBrokenLineListInfoVo {
                date=e.date,
                Performance=e.Performance
            }).ToList();
            data.PerformanceData = res.PerformanceData.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            return ResultData<LivingCustomerAndPerformanceBrokenLineDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 直播中漏斗图数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingFilterData")]
        public async Task<ResultData<LivingFilterDataVo>> GetLivingFilterDataAsync([FromQuery]QueryLivingDataVo query){
            LivingFilterDataVo data = new LivingFilterDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.IsCurrent = query.IsCurrent;
            var res = await amiyaLivingOperationBoardService.GetLivingFilterDataAsync(queryDto);
            data.CurrentGroup = new LivingFilterDataItemVo();
            data.CurrentGroup.AddWeChatRate = res.CurrentGroup.AddWeChatRate;
            data.CurrentGroup.AddWeChatRateHealthValueThisMonth = res.CurrentGroup.AddWeChatRateHealthValueThisMonth;
            data.CurrentGroup.SendOrderRate = res.CurrentGroup.SendOrderRate;
            data.CurrentGroup.SendOrderRateHealthValueThisMonth = res.CurrentGroup.SendOrderRateHealthValueThisMonth;
            data.CurrentGroup.ToHospitalRate = res.CurrentGroup.ToHospitalRate;
            data.CurrentGroup.ToHospitalRateHealthValueThisMonth = res.CurrentGroup.ToHospitalRateHealthValueThisMonth;
            data.CurrentGroup.DealRate = res.CurrentGroup.DealRate;
            data.CurrentGroup.DealRateHealthValueThisMonth = res.CurrentGroup.DealRateHealthValueThisMonth;
            data.CurrentGroup.SendCycle = res.CurrentGroup.SendCycle;
            data.CurrentGroup.HospitalCycle = res.CurrentGroup.HospitalCycle;
            data.CurrentGroup.DataList = res.CurrentGroup.DataList.Select(e=>new LivingFilterDetailDataVo { 
                Name=e.Name,
                Key=e.Key,
                Value=e.Value
            }).ToList();
            data.Company = new LivingFilterDataItemVo();
            data.Company.AddWeChatRate = res.Company.AddWeChatRate;
            data.Company.AddWeChatRateHealthValueThisMonth = res.Company.AddWeChatRateHealthValueThisMonth;
            data.Company.SendOrderRate = res.Company.SendOrderRate;
            data.Company.SendOrderRateHealthValueThisMonth = res.Company.SendOrderRateHealthValueThisMonth;
            data.Company.ToHospitalRate = res.Company.ToHospitalRate;
            data.Company.ToHospitalRateHealthValueThisMonth = res.Company.ToHospitalRateHealthValueThisMonth;
            data.Company.DealRate = res.Company.DealRate;
            data.Company.DealRateHealthValueThisMonth = res.Company.DealRateHealthValueThisMonth;
            data.Company.SendCycle = res.Company.SendCycle;
            data.Company.HospitalCycle = res.Company.HospitalCycle;
            data.Company.DataList = res.Company.DataList.Select(e => new LivingFilterDetailDataVo
            {
                Name = e.Name,
                Key = e.Key,
                Value = e.Value
            }).ToList();
            return ResultData<LivingFilterDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 直播中转化周期数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingCycleData")]
        public async Task<ResultData<LivingCycleDataVo>> GetLivingCycleDataAsync([FromQuery]QueryLivingDataVo query){
            LivingCycleDataVo data = new LivingCycleDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.IsCurrent = query.IsCurrent;
            var res = await amiyaLivingOperationBoardService.GetLivingCycleDataAsync(queryDto);
            data.SendCycleData = res.SendCycleData;
            data.ToHospitalCycleData = res.ToHospitalCycleData;
            return ResultData<LivingCycleDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 直播中线索目标完成率
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingClueTargetData")]
        public async Task<ResultData<LivingClueTargetDataVo>> GetLivingClueTargetDataAsync([FromQuery]QueryLivingDataVo query){
            LivingClueTargetDataVo data = new LivingClueTargetDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaLivingOperationBoardService.GetLivingClueTargetDataAsync(queryDto);
            data.ClueTargetComplete = res.ClueTargetComplete;
            return ResultData<LivingClueTargetDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 直播中业绩贡献占比
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingPerformanceRate")]
        public async Task<ResultData<LivingPerformanceRateVo>> GetLivingPerformanceRateAsync([FromQuery]QueryLivingDataVo query){
            LivingPerformanceRateVo data = new LivingPerformanceRateVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaLivingOperationBoardService.GetLivingPerformanceRateAsync(queryDto);
            data.PerformanceRate = res.PerformanceRate;
            return ResultData<LivingPerformanceRateVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 直播中平台账号获客占比
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingContentplatformClueData")]
        public async Task<ResultData<LivingContentplatformClueDataVo>> GetLivingContentplatformClueDataAsync([FromQuery]QueryLivingDataVo query){
            LivingContentplatformClueDataVo data = new LivingContentplatformClueDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaLivingOperationBoardService.GetLivingContentplatformClueDataAsync(queryDto);
            data.ContentPlatformTotalClue = res.ContentPlatformTotalClue;
            data.ContentPlatformClueRate = res.ContentPlatformClueRate.Select(e => new LivingContentplatformClueDataItemVo { 
                Name=e.Name,
                Performance=e.Performance,
                Value=e.Value
            }).ToList();

            data.AccountTotalClue = res.AccountTotalClue;
            data.AccountClueRate = res.AccountClueRate.Select(e => new LivingContentplatformClueDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            data.TikTokTotalClue = res.TikTokTotalClue;
            data.TikTokClueRate = res.TikTokClueRate.Select(e => new LivingContentplatformClueDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            data.WechatVideoTotalClue = res.WechatVideoTotalClue;
            data.WechatVideoClueRate = res.WechatVideoClueRate.Select(e => new LivingContentplatformClueDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            data.XiaoHongShuTotalClue = res.XiaoHongShuTotalClue;
            data.XiaoHongShuClueRate = res.XiaoHongShuClueRate.Select(e => new LivingContentplatformClueDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            data.RiBuLuoTotalClue = res.RiBuLuoTotalClue;
            data.RiBuLuoClueRate = res.RiBuLuoClueRate.Select(e => new LivingContentplatformClueDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            return ResultData<LivingContentplatformClueDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 直播中平台账号业绩占比
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLivingContentplatformPerformanceData")]
        public async Task<ResultData<LivingContentplatformPerformanceDataVo>> GetLivingContentplatformPerformanceDataAsync([FromQuery]QueryLivingDataVo query){
            LivingContentplatformPerformanceDataVo data = new LivingContentplatformPerformanceDataVo();
            QueryLivingDataDto queryDto = new QueryLivingDataDto();
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            var res = await amiyaLivingOperationBoardService.GetLivingContentplatformPerformanceDataAsync(queryDto);
            data.ContentPlatformTotalPerformance = res.ContentPlatformTotalPerformance;
            data.ContentPlatformPerformanceRate = res.ContentPlatformPerformanceRate.Select(e => new LivingContentplatformPerformanceDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();

            data.AccountTotalPerformance = res.AccountTotalPerformance;
            data.AccountPerformanceRate = res.AccountPerformanceRate.Select(e => new LivingContentplatformPerformanceDataItemVo
            {
                Name = e.Name,
                Performance = e.Performance,
                Value = e.Value
            }).ToList();
            return ResultData<LivingContentplatformPerformanceDataVo>.Success().AddData("data", data);
        }

    }
}
