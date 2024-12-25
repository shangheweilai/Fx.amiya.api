using Fx.Amiya.Background.Api.Vo;
using Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Input;
using Fx.Amiya.Background.Api.Vo.AmiyaOperationsBoard.Result;
using Fx.Amiya.Background.Api.Vo.Performance.AmiyaPerformance2.Result;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Input;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.IService;
using Fx.Authorization.Attributes;
using Fx.Common.Extensions;
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
    /// 啊美雅运营看板
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class AmiyaOperationsBoardController : ControllerBase
    {
        private readonly IAmiyaOperationsBoardService amiyaOperationsBoardService;
        private readonly ILiveAnchorMonthlyTargetLivingService liveAnchorMonthlyTargetLivingService;
        private readonly ILiveAnchorBaseInfoService liveAnchorBaseInfoService;
        public AmiyaOperationsBoardController(IAmiyaOperationsBoardService amiyaOperationsBoardService, ILiveAnchorMonthlyTargetLivingService liveAnchorMonthlyTargetLivingService, ILiveAnchorBaseInfoService liveAnchorBaseInfoService)
        {
            this.amiyaOperationsBoardService = amiyaOperationsBoardService;
            this.liveAnchorMonthlyTargetLivingService = liveAnchorMonthlyTargetLivingService;
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
        }
        /// <summary>
        /// 根据结束时间获取时间进度
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getTimeSpan")]
        public async Task<ResultData<decimal>> GetTimeSpanAsync([FromQuery] QueryOperationDataVo query)
        {
            decimal result = 0.00M;
            var date = DateTimeExtension.GetDatetimeSchedule(query.endDate.Value).FirstOrDefault();
            result = date.Value;
            return ResultData<decimal>.Success().AddData("data", result);
        }
        #region  业绩


        /// <summary>
        /// 根据条件获取业绩数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("getTotalAchievementAndDateSchedule")]
        public async Task<ResultData<OperationTotalAchievementDataVo>> GetTotalAchievementAndDateScheduleAsync([FromQuery] QueryOperationDataVo query)
        {
            OperationTotalAchievementDataVo result = new OperationTotalAchievementDataVo();
            QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
            queryOperationDataVo.startDate = query.startDate;
            queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryOperationDataVo.keyWord = query.keyWord;
            var data = await amiyaOperationsBoardService.GetTotalAchievementAndDateScheduleAsync(queryOperationDataVo);

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
            result.NewCustomerPerformanceBrokenLineList = data.NewCustomerPerformanceBrokenLineList.Select(x => new PerformanceBrokenLineListInfoVo
            {
                date = x.date,
                Performance = x.Performance
            }).ToList();
            result.OldCustomerPerformanceBrokenLineList = data.OldCustomerPerformanceBrokenLineList.Select(x => new PerformanceBrokenLineListInfoVo
            {
                date = x.date,
                Performance = x.Performance
            }).ToList();
            return ResultData<OperationTotalAchievementDataVo>.Success().AddData("data", result);
        }

        /// <summary>
        /// 根据条件获取新老客业绩占比（分组）
        /// </summary>
        /// <returns></returns>
        [HttpGet("getNewOrOldCustomerCompare")]
        public async Task<ResultData<GetNewOrOldCustomerCompareDataVo>> GetNewOrOldCustomerCompareDataAsync([FromQuery] QueryOperationDataVo query)
        {
            GetNewOrOldCustomerCompareDataVo result = new GetNewOrOldCustomerCompareDataVo();
            QueryOperationDataDto queryOperationDataDto = new QueryOperationDataDto();
            queryOperationDataDto.startDate = query.startDate;
            queryOperationDataDto.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryOperationDataDto.keyWord = query.keyWord;
            var data = await amiyaOperationsBoardService.GetNewOrOldCustomerCompareDataAsync(queryOperationDataDto);


            #region【面诊类型】
            #region{业绩}

            result.TotalConsulationType = new OperationBoardConsulationTypeVo();
            result.GroupDaoDaoConsulationType = new OperationBoardConsulationTypeVo();
            result.GroupJiNaConsulationType = new OperationBoardConsulationTypeVo();

            result.TotalConsulationType.OtherRate = data.TotalConsulationType.OtherRate;
            result.TotalConsulationType.UnConsulationRate = data.TotalConsulationType.UnConsulationRate;
            result.TotalConsulationType.PictureConsulationRate = data.TotalConsulationType.PictureConsulationRate;
            result.TotalConsulationType.VideoConsulationRate = data.TotalConsulationType.VideoConsulationRate;
            result.TotalConsulationType.AudioConsulationRate = data.TotalConsulationType.AudioConsulationRate;
            result.TotalConsulationType.OtherNumber = data.TotalConsulationType.OtherNumber;
            result.TotalConsulationType.UnConsulationNumber = data.TotalConsulationType.UnConsulationNumber;
            result.TotalConsulationType.PictureConsulationNumber = data.TotalConsulationType.PictureConsulationNumber;
            result.TotalConsulationType.VideoConsulationNumber = data.TotalConsulationType.VideoConsulationNumber;
            result.TotalConsulationType.AudioConsulationNumber = data.TotalConsulationType.AudioConsulationNumber;
            result.TotalConsulationType.TotalPerformanceNumber = data.TotalConsulationType.TotalPerformanceNumber;


            #endregion
            #region{人数}

            result.TotalConsulationTypeNumber = new OperationBoardConsulationTypeVo();
            result.GroupDaoDaoConsulationTypeNumber = new OperationBoardConsulationTypeVo();
            result.GroupJiNaConsulationTypeNumber = new OperationBoardConsulationTypeVo();

            result.TotalConsulationTypeNumber.OtherRate = data.TotalConsulationType.OtherRate;
            result.TotalConsulationTypeNumber.UnConsulationRate = data.TotalConsulationTypeNumber.UnConsulationRate;
            result.TotalConsulationTypeNumber.PictureConsulationRate = data.TotalConsulationTypeNumber.PictureConsulationRate;
            result.TotalConsulationTypeNumber.VideoConsulationRate = data.TotalConsulationTypeNumber.VideoConsulationRate;
            result.TotalConsulationTypeNumber.AudioConsulationRate = data.TotalConsulationTypeNumber.AudioConsulationRate;
            result.TotalConsulationTypeNumber.OtherNumber = data.TotalConsulationTypeNumber.OtherNumber;
            result.TotalConsulationTypeNumber.UnConsulationNumber = data.TotalConsulationTypeNumber.UnConsulationNumber;
            result.TotalConsulationTypeNumber.PictureConsulationNumber = data.TotalConsulationTypeNumber.PictureConsulationNumber;
            result.TotalConsulationTypeNumber.VideoConsulationNumber = data.TotalConsulationTypeNumber.VideoConsulationNumber;
            result.TotalConsulationTypeNumber.AudioConsulationNumber = data.TotalConsulationTypeNumber.AudioConsulationNumber;
            result.TotalConsulationTypeNumber.TotalPerformanceNumber = data.TotalConsulationTypeNumber.TotalPerformanceNumber;

            #endregion

            #endregion

            #region【平台】

            result.TotalFlowRateByContentPlatForm = new OperationBoardContentPlatFormDataVo();
            result.GroupDaoDaoFlowRateByContentPlatForm = new OperationBoardContentPlatFormDataVo();
            result.GroupJiNaFlowRateByContentPlatForm = new OperationBoardContentPlatFormDataVo();

            result.TotalFlowRateByContentPlatForm.DouYinNumber = data.TotalFlowRateByContentPlatForm.DouYinNumber;
            result.TotalFlowRateByContentPlatForm.DouYinRate = data.TotalFlowRateByContentPlatForm.DouYinRate;
            result.TotalFlowRateByContentPlatForm.VideoNumberNumber = data.TotalFlowRateByContentPlatForm.VideoNumberNumber;
            result.TotalFlowRateByContentPlatForm.VideoNumberRate = data.TotalFlowRateByContentPlatForm.VideoNumberRate;
            result.TotalFlowRateByContentPlatForm.XiaoHongShuNumber = data.TotalFlowRateByContentPlatForm.XiaoHongShuNumber;
            result.TotalFlowRateByContentPlatForm.XiaoHongShuRate = data.TotalFlowRateByContentPlatForm.XiaoHongShuRate;
            result.TotalFlowRateByContentPlatForm.PrivateDataNumber = data.TotalFlowRateByContentPlatForm.PrivateDataNumber;
            result.TotalFlowRateByContentPlatForm.PrivateDataRate = data.TotalFlowRateByContentPlatForm.PrivateDataRate;
            result.TotalFlowRateByContentPlatForm.TotalFlowRateNumber = data.TotalFlowRateByContentPlatForm.TotalFlowRateNumber;


            #region{刀刀吉娜数据}

            //result.GroupDaoDaoFlowRateByContentPlatForm.DouYinNumber = data.GroupDaoDaoFlowRateByContentPlatForm.DouYinNumber;
            //result.GroupDaoDaoFlowRateByContentPlatForm.DouYinRate = data.GroupDaoDaoFlowRateByContentPlatForm.DouYinRate;
            //result.GroupDaoDaoFlowRateByContentPlatForm.VideoNumberNumber = data.GroupDaoDaoFlowRateByContentPlatForm.VideoNumberNumber;
            //result.GroupDaoDaoFlowRateByContentPlatForm.VideoNumberRate = data.GroupDaoDaoFlowRateByContentPlatForm.VideoNumberRate;
            //result.GroupDaoDaoFlowRateByContentPlatForm.XiaoHongShuNumber = data.GroupDaoDaoFlowRateByContentPlatForm.XiaoHongShuNumber;
            //result.GroupDaoDaoFlowRateByContentPlatForm.XiaoHongShuRate = data.GroupDaoDaoFlowRateByContentPlatForm.XiaoHongShuRate;
            //result.GroupDaoDaoFlowRateByContentPlatForm.PrivateDataNumber = data.GroupDaoDaoFlowRateByContentPlatForm.PrivateDataNumber;
            //result.GroupDaoDaoFlowRateByContentPlatForm.PrivateDataRate = data.GroupDaoDaoFlowRateByContentPlatForm.PrivateDataRate;
            //result.GroupDaoDaoFlowRateByContentPlatForm.TotalFlowRateNumber = data.GroupDaoDaoFlowRateByContentPlatForm.TotalFlowRateNumber;

            //result.GroupJiNaFlowRateByContentPlatForm.DouYinNumber = data.GroupJiNaFlowRateByContentPlatForm.DouYinNumber;
            //result.GroupJiNaFlowRateByContentPlatForm.DouYinRate = data.GroupJiNaFlowRateByContentPlatForm.DouYinRate;
            //result.GroupJiNaFlowRateByContentPlatForm.VideoNumberNumber = data.GroupJiNaFlowRateByContentPlatForm.VideoNumberNumber;
            //result.GroupJiNaFlowRateByContentPlatForm.VideoNumberRate = data.GroupJiNaFlowRateByContentPlatForm.VideoNumberRate;
            //result.GroupJiNaFlowRateByContentPlatForm.XiaoHongShuNumber = data.GroupJiNaFlowRateByContentPlatForm.XiaoHongShuNumber;
            //result.GroupJiNaFlowRateByContentPlatForm.XiaoHongShuRate = data.GroupJiNaFlowRateByContentPlatForm.XiaoHongShuRate;
            //result.GroupJiNaFlowRateByContentPlatForm.PrivateDataNumber = data.GroupJiNaFlowRateByContentPlatForm.PrivateDataNumber;
            //result.GroupJiNaFlowRateByContentPlatForm.PrivateDataRate = data.GroupJiNaFlowRateByContentPlatForm.PrivateDataRate;
            //result.GroupJiNaFlowRateByContentPlatForm.TotalFlowRateNumber = data.GroupJiNaFlowRateByContentPlatForm.TotalFlowRateNumber;
            #endregion
            #endregion

            #region【部门】

            result.TotalBelongChannelPerformance = new OperationBoardBelongChannelPerformanceDataVo();
            result.GroupDaoDaoBelongChannelPerformance = new OperationBoardBelongChannelPerformanceDataVo();
            result.GroupJiNaBelongChannelPerformance = new OperationBoardBelongChannelPerformanceDataVo();

            result.TotalBelongChannelPerformance.BeforeLivingNumber = data.TotalBelongChannelPerformance.BeforeLivingNumber;
            result.TotalBelongChannelPerformance.BeforeLivingRate = data.TotalBelongChannelPerformance.BeforeLivingRate;
            result.TotalBelongChannelPerformance.LivingNumber = data.TotalBelongChannelPerformance.LivingNumber;
            result.TotalBelongChannelPerformance.LivingRate = data.TotalBelongChannelPerformance.LivingRate;
            result.TotalBelongChannelPerformance.AfterLivingNumber = data.TotalBelongChannelPerformance.AfterLivingNumber;
            result.TotalBelongChannelPerformance.AfterLivingRate = data.TotalBelongChannelPerformance.AfterLivingRate;
            result.TotalBelongChannelPerformance.OtherNumber = data.TotalBelongChannelPerformance.OtherNumber;
            result.TotalBelongChannelPerformance.OtherRate = data.TotalBelongChannelPerformance.OtherRate;
            result.TotalBelongChannelPerformance.TotalPerformanceNumber = data.TotalBelongChannelPerformance.TotalPerformanceNumber;

            #region{刀刀吉娜数据}

            //result.GroupDaoDaoBelongChannelPerformance.BeforeLivingNumber = data.GroupDaoDaoBelongChannelPerformance.BeforeLivingNumber;
            //result.GroupDaoDaoBelongChannelPerformance.BeforeLivingRate = data.GroupDaoDaoBelongChannelPerformance.BeforeLivingRate;
            //result.GroupDaoDaoBelongChannelPerformance.LivingNumber = data.GroupDaoDaoBelongChannelPerformance.LivingNumber;
            //result.GroupDaoDaoBelongChannelPerformance.LivingRate = data.GroupDaoDaoBelongChannelPerformance.LivingRate;
            //result.GroupDaoDaoBelongChannelPerformance.AfterLivingNumber = data.GroupDaoDaoBelongChannelPerformance.AfterLivingNumber;
            //result.GroupDaoDaoBelongChannelPerformance.AfterLivingRate = data.GroupDaoDaoBelongChannelPerformance.AfterLivingRate;
            //result.GroupDaoDaoBelongChannelPerformance.OtherNumber = data.GroupDaoDaoBelongChannelPerformance.OtherNumber;
            //result.GroupDaoDaoBelongChannelPerformance.OtherRate = data.GroupDaoDaoBelongChannelPerformance.OtherRate;
            //result.GroupDaoDaoBelongChannelPerformance.TotalPerformanceNumber = data.GroupDaoDaoBelongChannelPerformance.TotalPerformanceNumber;

            //result.GroupJiNaBelongChannelPerformance.BeforeLivingNumber = data.GroupJiNaBelongChannelPerformance.BeforeLivingNumber;
            //result.GroupJiNaBelongChannelPerformance.BeforeLivingRate = data.GroupJiNaBelongChannelPerformance.BeforeLivingRate;
            //result.GroupJiNaBelongChannelPerformance.LivingNumber = data.GroupJiNaBelongChannelPerformance.LivingNumber;
            //result.GroupJiNaBelongChannelPerformance.LivingRate = data.GroupJiNaBelongChannelPerformance.LivingRate;
            //result.GroupJiNaBelongChannelPerformance.AfterLivingNumber = data.GroupJiNaBelongChannelPerformance.AfterLivingNumber;
            //result.GroupJiNaBelongChannelPerformance.AfterLivingRate = data.GroupJiNaBelongChannelPerformance.AfterLivingRate;
            //result.GroupJiNaBelongChannelPerformance.OtherNumber = data.GroupJiNaBelongChannelPerformance.OtherNumber;
            //result.GroupJiNaBelongChannelPerformance.OtherRate = data.GroupJiNaBelongChannelPerformance.OtherRate;
            //result.GroupJiNaBelongChannelPerformance.TotalPerformanceNumber = data.GroupJiNaBelongChannelPerformance.TotalPerformanceNumber;
            #endregion
            #endregion

            #region【新老客】
            #region {业绩}

            result.TotalNewOrOldCustomer = new OperationBoardGetNewOrOldCustomerCompareDataDetailsVo();
            result.GroupDaoDaoNewOrOldCustomer = new OperationBoardGetNewOrOldCustomerCompareDataDetailsVo();
            result.GroupJiNaNewOrOldCustomer = new OperationBoardGetNewOrOldCustomerCompareDataDetailsVo();

            result.TotalNewOrOldCustomer.TotalPerformanceNewCustomerNumber = data.TotalNewOrOldCustomer.TotalPerformanceNewCustomerNumber;
            result.TotalNewOrOldCustomer.TotalPerformanceNewCustomerRate = data.TotalNewOrOldCustomer.TotalPerformanceNewCustomerRate;
            result.TotalNewOrOldCustomer.TotalPerformanceOldCustomerNumber = data.TotalNewOrOldCustomer.TotalPerformanceOldCustomerNumber;
            result.TotalNewOrOldCustomer.TotalPerformanceOldCustomerRate = data.TotalNewOrOldCustomer.TotalPerformanceOldCustomerRate;
            result.TotalNewOrOldCustomer.TotalPerformanceNumber = data.TotalNewOrOldCustomer.TotalPerformanceNumber;

            #region{刀刀吉娜数据}
            //result.GroupDaoDaoNewOrOldCustomer.TotalPerformanceNewCustomerNumber = data.GroupDaoDaoNewOrOldCustomer.TotalPerformanceNewCustomerNumber;
            //result.GroupDaoDaoNewOrOldCustomer.TotalPerformanceNewCustomerRate = data.GroupDaoDaoNewOrOldCustomer.TotalPerformanceNewCustomerRate;
            //result.GroupDaoDaoNewOrOldCustomer.TotalPerformanceOldCustomerNumber = data.GroupDaoDaoNewOrOldCustomer.TotalPerformanceOldCustomerNumber;
            //result.GroupDaoDaoNewOrOldCustomer.TotalPerformanceOldCustomerRate = data.GroupDaoDaoNewOrOldCustomer.TotalPerformanceOldCustomerRate;
            //result.GroupDaoDaoNewOrOldCustomer.TotalPerformanceNumber = data.GroupDaoDaoNewOrOldCustomer.TotalPerformanceNumber;

            //result.GroupJiNaNewOrOldCustomer.TotalPerformanceNewCustomerNumber = data.GroupJiNaNewOrOldCustomer.TotalPerformanceNewCustomerNumber;
            //result.GroupJiNaNewOrOldCustomer.TotalPerformanceNewCustomerRate = data.GroupJiNaNewOrOldCustomer.TotalPerformanceNewCustomerRate;
            //result.GroupJiNaNewOrOldCustomer.TotalPerformanceOldCustomerNumber = data.GroupJiNaNewOrOldCustomer.TotalPerformanceOldCustomerNumber;
            //result.GroupJiNaNewOrOldCustomer.TotalPerformanceOldCustomerRate = data.GroupJiNaNewOrOldCustomer.TotalPerformanceOldCustomerRate;
            //result.GroupJiNaNewOrOldCustomer.TotalPerformanceNumber = data.GroupJiNaNewOrOldCustomer.TotalPerformanceNumber;
            #endregion
            #endregion

            #region{人数}

            result.TotalNewOrOldCustomerNum = new OperationBoardGetNewOrOldCustomerCompareDataDetailsVo();
            result.GroupDaoDaoNewOrOldCustomerNum = new OperationBoardGetNewOrOldCustomerCompareDataDetailsVo();
            result.GroupJiNaNewOrOldCustomerNum = new OperationBoardGetNewOrOldCustomerCompareDataDetailsVo();

            result.TotalNewOrOldCustomerNum.TotalPerformanceNewCustomerNumber = data.TotalNewOrOldCustomerNum.TotalPerformanceNewCustomerNumber;
            result.TotalNewOrOldCustomerNum.TotalPerformanceNewCustomerRate = data.TotalNewOrOldCustomerNum.TotalPerformanceNewCustomerRate;
            result.TotalNewOrOldCustomerNum.TotalPerformanceOldCustomerNumber = data.TotalNewOrOldCustomerNum.TotalPerformanceOldCustomerNumber;
            result.TotalNewOrOldCustomerNum.TotalPerformanceOldCustomerRate = data.TotalNewOrOldCustomerNum.TotalPerformanceOldCustomerRate;
            result.TotalNewOrOldCustomerNum.TotalPerformanceNumber = data.TotalNewOrOldCustomerNum.TotalPerformanceNumber;

            #region{刀刀吉娜数据}

            //result.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceNewCustomerNumber = data.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceNewCustomerNumber;
            //result.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceNewCustomerRate = data.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceNewCustomerRate;
            //result.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceOldCustomerNumber = data.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceOldCustomerNumber;
            //result.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceOldCustomerRate = data.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceOldCustomerRate;
            //result.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceNumber = data.GroupDaoDaoNewOrOldCustomerNum.TotalPerformanceNumber;

            //result.GroupJiNaNewOrOldCustomerNum.TotalPerformanceNewCustomerNumber = data.GroupJiNaNewOrOldCustomerNum.TotalPerformanceNewCustomerNumber;
            //result.GroupJiNaNewOrOldCustomerNum.TotalPerformanceNewCustomerRate = data.GroupJiNaNewOrOldCustomerNum.TotalPerformanceNewCustomerRate;
            //result.GroupJiNaNewOrOldCustomerNum.TotalPerformanceOldCustomerNumber = data.GroupJiNaNewOrOldCustomerNum.TotalPerformanceOldCustomerNumber;
            //result.GroupJiNaNewOrOldCustomerNum.TotalPerformanceOldCustomerRate = data.GroupJiNaNewOrOldCustomerNum.TotalPerformanceOldCustomerRate;
            //result.GroupJiNaNewOrOldCustomerNum.TotalPerformanceNumber = data.GroupJiNaNewOrOldCustomerNum.TotalPerformanceNumber;
            #endregion

            #endregion
            #endregion

            #region【有效/潜在】

            result.TotalIsEffictivePerformance = new OperationBoardGetIsEffictivePerformanceVo();
            result.GroupDaoDaoIsEffictivePerformance = new OperationBoardGetIsEffictivePerformanceVo();
            result.GroupJiNaIsEffictivePerformance = new OperationBoardGetIsEffictivePerformanceVo();

            result.TotalIsEffictivePerformance.EffictivePerformanceNumber = data.TotalIsEffictivePerformance.EffictivePerformanceNumber;
            result.TotalIsEffictivePerformance.EffictivePerformanceRate = data.TotalIsEffictivePerformance.EffictivePerformanceRate;
            result.TotalIsEffictivePerformance.NotEffictivePerformanceNumber = data.TotalIsEffictivePerformance.NotEffictivePerformanceNumber;
            result.TotalIsEffictivePerformance.NotEffictivePerformanceRate = data.TotalIsEffictivePerformance.NotEffictivePerformanceRate;
            result.TotalIsEffictivePerformance.TotalPerformanceNumber = data.TotalIsEffictivePerformance.TotalPerformanceNumber;

            #region{刀刀吉娜数据}

            //result.GroupDaoDaoIsEffictivePerformance.EffictivePerformanceNumber = data.GroupDaoDaoIsEffictivePerformance.EffictivePerformanceNumber;
            //result.GroupDaoDaoIsEffictivePerformance.EffictivePerformanceRate = data.GroupDaoDaoIsEffictivePerformance.EffictivePerformanceRate;
            //result.GroupDaoDaoIsEffictivePerformance.NotEffictivePerformanceNumber = data.GroupDaoDaoIsEffictivePerformance.NotEffictivePerformanceNumber;
            //result.GroupDaoDaoIsEffictivePerformance.NotEffictivePerformanceRate = data.GroupDaoDaoIsEffictivePerformance.NotEffictivePerformanceRate;
            //result.GroupDaoDaoIsEffictivePerformance.TotalPerformanceNumber = data.GroupDaoDaoIsEffictivePerformance.TotalPerformanceNumber;

            //result.GroupJiNaIsEffictivePerformance.EffictivePerformanceNumber = data.GroupJiNaIsEffictivePerformance.EffictivePerformanceNumber;
            //result.GroupJiNaIsEffictivePerformance.EffictivePerformanceRate = data.GroupJiNaIsEffictivePerformance.EffictivePerformanceRate;
            //result.GroupJiNaIsEffictivePerformance.NotEffictivePerformanceNumber = data.GroupJiNaIsEffictivePerformance.NotEffictivePerformanceNumber;
            //result.GroupJiNaIsEffictivePerformance.NotEffictivePerformanceRate = data.GroupJiNaIsEffictivePerformance.NotEffictivePerformanceRate;
            //result.GroupJiNaIsEffictivePerformance.TotalPerformanceNumber = data.GroupJiNaIsEffictivePerformance.TotalPerformanceNumber;
            #endregion

            #endregion

            #region【当月/历史】
            #region{业绩}

            result.TotalIsHistoryPerformance = new OperationBoardGetIsHistoryPerformanceVo();
            result.GroupDaoDaoIsHistoryPerformance = new OperationBoardGetIsHistoryPerformanceVo();
            result.GroupJiNaIsHistoryPerformance = new OperationBoardGetIsHistoryPerformanceVo();

            result.TotalIsHistoryPerformance.HistoryPerformanceNumber = data.TotalIsHistoryPerformance.HistoryPerformanceNumber;
            result.TotalIsHistoryPerformance.HistoryPerformanceRate = data.TotalIsHistoryPerformance.HistoryPerformanceRate;
            result.TotalIsHistoryPerformance.ThisMonthPerformanceNumber = data.TotalIsHistoryPerformance.ThisMonthPerformanceNumber;
            result.TotalIsHistoryPerformance.ThisMonthPerformanceRate = data.TotalIsHistoryPerformance.ThisMonthPerformanceRate;
            result.TotalIsHistoryPerformance.TotalPerformanceNumber = data.TotalIsHistoryPerformance.TotalPerformanceNumber;

            #region{刀刀吉娜数据}
            //result.GroupDaoDaoIsHistoryPerformance.HistoryPerformanceNumber = data.GroupDaoDaoIsHistoryPerformance.HistoryPerformanceNumber;
            //result.GroupDaoDaoIsHistoryPerformance.HistoryPerformanceRate = data.GroupDaoDaoIsHistoryPerformance.HistoryPerformanceRate;
            //result.GroupDaoDaoIsHistoryPerformance.ThisMonthPerformanceNumber = data.GroupDaoDaoIsHistoryPerformance.ThisMonthPerformanceNumber;
            //result.GroupDaoDaoIsHistoryPerformance.ThisMonthPerformanceRate = data.GroupDaoDaoIsHistoryPerformance.ThisMonthPerformanceRate;
            //result.GroupDaoDaoIsHistoryPerformance.TotalPerformanceNumber = data.GroupDaoDaoIsHistoryPerformance.TotalPerformanceNumber;

            //result.GroupJiNaIsHistoryPerformance.HistoryPerformanceNumber = data.GroupJiNaIsHistoryPerformance.HistoryPerformanceNumber;
            //result.GroupJiNaIsHistoryPerformance.HistoryPerformanceRate = data.GroupJiNaIsHistoryPerformance.HistoryPerformanceRate;
            //result.GroupJiNaIsHistoryPerformance.ThisMonthPerformanceNumber = data.GroupJiNaIsHistoryPerformance.ThisMonthPerformanceNumber;
            //result.GroupJiNaIsHistoryPerformance.ThisMonthPerformanceRate = data.GroupJiNaIsHistoryPerformance.ThisMonthPerformanceRate;
            //result.GroupJiNaIsHistoryPerformance.TotalPerformanceNumber = data.GroupJiNaIsHistoryPerformance.TotalPerformanceNumber;
            #endregion

            #endregion
            #region{人数}

            result.TotalIsHistoryPerformanceNum = new OperationBoardGetIsHistoryPerformanceVo();
            result.GroupDaoDaoIsHistoryPerformanceNum = new OperationBoardGetIsHistoryPerformanceVo();
            result.GroupJiNaIsHistoryPerformanceNum = new OperationBoardGetIsHistoryPerformanceVo();

            result.TotalIsHistoryPerformanceNum.HistoryPerformanceNumber = data.TotalIsHistoryPerformanceNum.HistoryPerformanceNumber;
            result.TotalIsHistoryPerformanceNum.HistoryPerformanceRate = data.TotalIsHistoryPerformanceNum.HistoryPerformanceRate;
            result.TotalIsHistoryPerformanceNum.ThisMonthPerformanceNumber = data.TotalIsHistoryPerformanceNum.ThisMonthPerformanceNumber;
            result.TotalIsHistoryPerformanceNum.ThisMonthPerformanceRate = data.TotalIsHistoryPerformanceNum.ThisMonthPerformanceRate;
            result.TotalIsHistoryPerformanceNum.TotalPerformanceNumber = data.TotalIsHistoryPerformanceNum.TotalPerformanceNumber;
            #region{刀刀吉娜数据}

            //result.GroupDaoDaoIsHistoryPerformanceNum.HistoryPerformanceNumber = data.GroupDaoDaoIsHistoryPerformanceNum.HistoryPerformanceNumber;
            //result.GroupDaoDaoIsHistoryPerformanceNum.HistoryPerformanceRate = data.GroupDaoDaoIsHistoryPerformanceNum.HistoryPerformanceRate;
            //result.GroupDaoDaoIsHistoryPerformanceNum.ThisMonthPerformanceNumber = data.GroupDaoDaoIsHistoryPerformanceNum.ThisMonthPerformanceNumber;
            //result.GroupDaoDaoIsHistoryPerformanceNum.ThisMonthPerformanceRate = data.GroupDaoDaoIsHistoryPerformanceNum.ThisMonthPerformanceRate;
            //result.GroupDaoDaoIsHistoryPerformanceNum.TotalPerformanceNumber = data.GroupDaoDaoIsHistoryPerformanceNum.TotalPerformanceNumber;

            //result.GroupJiNaIsHistoryPerformanceNum.HistoryPerformanceNumber = data.GroupJiNaIsHistoryPerformanceNum.HistoryPerformanceNumber;
            //result.GroupJiNaIsHistoryPerformanceNum.HistoryPerformanceRate = data.GroupJiNaIsHistoryPerformanceNum.HistoryPerformanceRate;
            //result.GroupJiNaIsHistoryPerformanceNum.ThisMonthPerformanceNumber = data.GroupJiNaIsHistoryPerformanceNum.ThisMonthPerformanceNumber;
            //result.GroupJiNaIsHistoryPerformanceNum.ThisMonthPerformanceRate = data.GroupJiNaIsHistoryPerformanceNum.ThisMonthPerformanceRate;
            //result.GroupJiNaIsHistoryPerformanceNum.TotalPerformanceNumber = data.GroupJiNaIsHistoryPerformanceNum.TotalPerformanceNumber;
            #endregion
            #endregion

            #endregion
            return ResultData<GetNewOrOldCustomerCompareDataVo>.Success().AddData("data", result);
        }


        /// <summary>
        /// 根据条件获取新老客业绩占比（助理与机构）
        /// </summary>
        /// <returns></returns>
        [HttpGet("getNewOrOldCustomerCompareByEmployeeAndHospital")]
        public async Task<ResultData<NewOrOldCustomerPerformanceDataListVo>> GetNewOrOldCustomerCompareByEmployeeAndHospitalDataAsync([FromQuery] QueryOperationDataVo query)
        {
            QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
            queryOperationDataVo.startDate = query.startDate;
            queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            var data = await amiyaOperationsBoardService.GetNewOrOldCustomerCompareByEmployeeAndHospitalAsync(queryOperationDataVo);

            NewOrOldCustomerPerformanceDataListVo result = new NewOrOldCustomerPerformanceDataListVo();
            result.HospitalPerformance = new List<CustomerPerformanceDataVo>();
            result.EmployeePerformance = new List<CustomerPerformanceDataVo>();
            foreach (var x in data.EmployeePerformance)
            {
                CustomerPerformanceDataVo employeePerformanceVo = new CustomerPerformanceDataVo();
                employeePerformanceVo.NewCustomerPerformance = x.NewCustomerPerformance;
                employeePerformanceVo.OldCustomerPerformance = x.OldCustomerPerformance;
                employeePerformanceVo.TotalCustomerPerformance = x.TotalPerformance;
                employeePerformanceVo.Name = x.Name;
                result.EmployeePerformance.Add(employeePerformanceVo);
            }
            foreach (var x in data.HospitalPerformance)
            {
                CustomerPerformanceDataVo hospitalPerformanceVo = new CustomerPerformanceDataVo();
                hospitalPerformanceVo.NewCustomerPerformance = x.NewCustomerPerformance;
                hospitalPerformanceVo.OldCustomerPerformance = x.OldCustomerPerformance;
                hospitalPerformanceVo.TotalCustomerPerformance = x.TotalPerformance;
                hospitalPerformanceVo.Name = x.Name;
                result.HospitalPerformance.Add(hospitalPerformanceVo);
            }
            return ResultData<NewOrOldCustomerPerformanceDataListVo>.Success().AddData("data", result);
        }

        #endregion


        #region 【流量】
        /// <summary>
        /// 根据条件获取流量数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("getTotalFlowRateAndDateSchedule")]
        public async Task<ResultData<OperationTotalFlowRateDataVo>> GetTotalFlowRateAndDateScheduleAsync([FromQuery] QueryOperationDataVo query)
        {
            OperationTotalFlowRateDataVo result = new OperationTotalFlowRateDataVo();
            QueryOperationDataDto queryOperationDataDto = new QueryOperationDataDto();
            queryOperationDataDto.startDate = query.startDate;
            queryOperationDataDto.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryOperationDataDto.keyWord = query.keyWord;
            var data = await amiyaOperationsBoardService.GetCustomerDataAsync(queryOperationDataDto);


            result.TotalBeforeLivingClue = data.TotalBeforeLivingClue;
            result.TodayBeforeLivingClue = data.TodayBeforeLivingClue;
            result.BeforeLivingClueCompleteRate = data.BeforeLivingClueCompleteRate;
            result.BeforeLivingClueYearOnYear = data.BeforeLivingClueYearOnYear;
            result.BeforeLivingClueChainRatio = data.BeforeLivingClueChainRatio;

            result.TotalLivingClue = data.TotalLivingClue;
            result.TodayLivingClue = data.TodayLivingClue;
            result.LivingClueCompleteRate = data.LivingClueCompleteRate;
            result.LivingClueYearOnYear = data.LivingClueYearOnYear;
            result.LivingClueChainRatio = data.LivingClueChainRatio;

            result.TotalAfterLivingClue = data.TotalAfterLivingClue;
            result.TodayTotalAfterLivingClue = data.TodayTotalAfterLivingClue;
            result.TotalAfterLivingClueCompleteRate = data.TotalAfterLivingClueCompleteRate;
            result.TotalAfterLivingClueYearOnYear = data.TotalAfterLivingClueYearOnYear;
            result.TotalAfterLivingClueChainRatio = data.TotalAfterLivingClueChainRatio;

            result.TotalClue = data.TotalClue;
            result.TodayClue = data.TodayClue;
            result.ClueCompleteRate = data.ClueCompleteRate;
            result.ClueYearOnYear = data.ClueYearOnYear;
            result.ClueChainRatio = data.ClueChainRatio;

            result.BeforeLivingClueBrokenLineList = data.BeforeLivingClueBrokenLineList.Select(x => new PerformanceBrokenLineListInfoVo
            {
                date = x.date,
                Performance = x.Performance
            }).ToList();
            result.LivingClueBrokenLineList = data.LivingClueBrokenLineList.Select(x => new PerformanceBrokenLineListInfoVo
            {
                date = x.date,
                Performance = x.Performance
            }).ToList();

            result.AfterLivingClueBrokenLineList = data.AfterLivingClueBrokenLineList.Select(x => new PerformanceBrokenLineListInfoVo
            {
                date = x.date,
                Performance = x.Performance
            }).ToList();
            return ResultData<OperationTotalFlowRateDataVo>.Success().AddData("data", result);
        }

        /// <summary>
        /// 根据条件获取流量分析占比（分组）
        /// </summary>
        /// <returns></returns>
        [HttpGet("getGroupFlowRateCompare")]
        public async Task<ResultData<GetGroupFlowRateCompareDataVo>> GetGroupFlowRateCompareDataAsync([FromQuery] QueryOperationDataVo query)
        {
            GetGroupFlowRateCompareDataVo result = new GetGroupFlowRateCompareDataVo();
            QueryOperationDataDto queryOperationDataDto = new QueryOperationDataDto();
            queryOperationDataDto.startDate = query.startDate;
            queryOperationDataDto.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryOperationDataDto.keyWord = query.keyWord;
            var data = await amiyaOperationsBoardService.GetFlowRateByContentPlatFormCompareDataAsync(queryOperationDataDto);
            #region 【平台线索分析】
            OperationBoardContentPlatFormDataVo totalResultDataByContentPlatFormVo = new OperationBoardContentPlatFormDataVo();
            totalResultDataByContentPlatFormVo.DouYinNumber = data.TotalFlowRateByContentPlatForm.DouYinNumber;
            totalResultDataByContentPlatFormVo.DouYinRate = data.TotalFlowRateByContentPlatForm.DouYinRate;
            totalResultDataByContentPlatFormVo.VideoNumberNumber = data.TotalFlowRateByContentPlatForm.VideoNumberNumber;
            totalResultDataByContentPlatFormVo.VideoNumberRate = data.TotalFlowRateByContentPlatForm.VideoNumberRate;
            totalResultDataByContentPlatFormVo.XiaoHongShuNumber = data.TotalFlowRateByContentPlatForm.XiaoHongShuNumber;
            totalResultDataByContentPlatFormVo.XiaoHongShuRate = data.TotalFlowRateByContentPlatForm.XiaoHongShuRate;
            totalResultDataByContentPlatFormVo.PrivateDataNumber = data.TotalFlowRateByContentPlatForm.PrivateDataNumber;
            totalResultDataByContentPlatFormVo.PrivateDataRate = data.TotalFlowRateByContentPlatForm.PrivateDataRate;
            totalResultDataByContentPlatFormVo.TotalFlowRateNumber = data.TotalFlowRateByContentPlatForm.TotalFlowRateNumber;
            result.TotalFlowRateByContentPlatForm = totalResultDataByContentPlatFormVo;
            #region{刀刀吉娜组}

            //OperationBoardContentPlatFormDataVo groupDaoDaoResultDataByContentPlatFormVo = new OperationBoardContentPlatFormDataVo();
            //groupDaoDaoResultDataByContentPlatFormVo.DouYinNumber = data.GroupDaoDaoFlowRateByContentPlatForm.DouYinNumber;
            //groupDaoDaoResultDataByContentPlatFormVo.DouYinRate = data.GroupDaoDaoFlowRateByContentPlatForm.DouYinRate;
            //groupDaoDaoResultDataByContentPlatFormVo.VideoNumberNumber = data.GroupDaoDaoFlowRateByContentPlatForm.VideoNumberNumber;
            //groupDaoDaoResultDataByContentPlatFormVo.VideoNumberRate = data.GroupDaoDaoFlowRateByContentPlatForm.VideoNumberRate;
            //groupDaoDaoResultDataByContentPlatFormVo.XiaoHongShuNumber = data.GroupDaoDaoFlowRateByContentPlatForm.XiaoHongShuNumber;
            //groupDaoDaoResultDataByContentPlatFormVo.XiaoHongShuRate = data.GroupDaoDaoFlowRateByContentPlatForm.XiaoHongShuRate;
            //groupDaoDaoResultDataByContentPlatFormVo.PrivateDataNumber = data.GroupDaoDaoFlowRateByContentPlatForm.PrivateDataNumber;
            //groupDaoDaoResultDataByContentPlatFormVo.PrivateDataRate = data.GroupDaoDaoFlowRateByContentPlatForm.PrivateDataRate;
            //groupDaoDaoResultDataByContentPlatFormVo.TotalFlowRateNumber = data.GroupDaoDaoFlowRateByContentPlatForm.TotalFlowRateNumber;
            //result.GroupDaoDaoFlowRateByContentPlatForm = groupDaoDaoResultDataByContentPlatFormVo;

            //OperationBoardContentPlatFormDataVo groupJiNaResultDataByContentPlatFormVo = new OperationBoardContentPlatFormDataVo();
            //groupJiNaResultDataByContentPlatFormVo.DouYinNumber = data.GroupJiNaFlowRateByContentPlatForm.DouYinNumber;
            //groupJiNaResultDataByContentPlatFormVo.DouYinRate = data.GroupJiNaFlowRateByContentPlatForm.DouYinRate;
            //groupJiNaResultDataByContentPlatFormVo.VideoNumberNumber = data.GroupJiNaFlowRateByContentPlatForm.VideoNumberNumber;
            //groupJiNaResultDataByContentPlatFormVo.VideoNumberRate = data.GroupJiNaFlowRateByContentPlatForm.VideoNumberRate;
            //groupJiNaResultDataByContentPlatFormVo.XiaoHongShuNumber = data.GroupJiNaFlowRateByContentPlatForm.XiaoHongShuNumber;
            //groupJiNaResultDataByContentPlatFormVo.XiaoHongShuRate = data.GroupJiNaFlowRateByContentPlatForm.XiaoHongShuRate;
            //groupJiNaResultDataByContentPlatFormVo.PrivateDataNumber = data.GroupJiNaFlowRateByContentPlatForm.PrivateDataNumber;
            //groupJiNaResultDataByContentPlatFormVo.PrivateDataRate = data.GroupJiNaFlowRateByContentPlatForm.PrivateDataRate;
            //groupJiNaResultDataByContentPlatFormVo.TotalFlowRateNumber = data.GroupJiNaFlowRateByContentPlatForm.TotalFlowRateNumber;
            //result.GroupJiNaFlowRateByContentPlatForm = groupJiNaResultDataByContentPlatFormVo;
            #endregion
            #endregion

            #region 【部门线索分析】
            OperationBoardDepartmentDataVo totalResultDataByDepartmentVo = new OperationBoardDepartmentDataVo();
            totalResultDataByDepartmentVo.BeforeLivingNumber = data.TotalFlowRateByDepartment.BeforeLivingNumber;
            totalResultDataByDepartmentVo.BeforeLivingRate = data.TotalFlowRateByDepartment.BeforeLivingRate;
            totalResultDataByDepartmentVo.LivingNumber = data.TotalFlowRateByDepartment.LivingNumber;
            totalResultDataByDepartmentVo.LivingRate = data.TotalFlowRateByDepartment.LivingRate;
            totalResultDataByDepartmentVo.AfterLivingNumber = data.TotalFlowRateByDepartment.AfterLivingNumber;
            totalResultDataByDepartmentVo.AftereLivingRate = data.TotalFlowRateByDepartment.AftereLivingRate;
            totalResultDataByDepartmentVo.OtherRate = data.TotalFlowRateByDepartment.OtherRate;
            totalResultDataByDepartmentVo.OtherNumber = data.TotalFlowRateByDepartment.OtherNumber;
            totalResultDataByDepartmentVo.TotalFlowRateNumber = data.TotalFlowRateByDepartment.TotalFlowRateNumber;
            result.TotalFlowRateByDepartment = totalResultDataByDepartmentVo;
            #region{刀刀吉娜组}

            //OperationBoardDepartmentDataVo groupDaoDaoResultDataByDepartmentVo = new OperationBoardDepartmentDataVo();
            //groupDaoDaoResultDataByDepartmentVo.BeforeLivingNumber = data.GroupDaoDaoFlowRateByDepartment.BeforeLivingNumber;
            //groupDaoDaoResultDataByDepartmentVo.BeforeLivingRate = data.GroupDaoDaoFlowRateByDepartment.BeforeLivingRate;
            //groupDaoDaoResultDataByDepartmentVo.LivingNumber = data.GroupDaoDaoFlowRateByDepartment.LivingNumber;
            //groupDaoDaoResultDataByDepartmentVo.LivingRate = data.GroupDaoDaoFlowRateByDepartment.LivingRate;
            //groupDaoDaoResultDataByDepartmentVo.AfterLivingNumber = data.GroupDaoDaoFlowRateByDepartment.AfterLivingNumber;
            //groupDaoDaoResultDataByDepartmentVo.AftereLivingRate = data.GroupDaoDaoFlowRateByDepartment.AftereLivingRate;
            //groupDaoDaoResultDataByDepartmentVo.OtherRate = data.GroupDaoDaoFlowRateByDepartment.OtherRate;
            //groupDaoDaoResultDataByDepartmentVo.OtherNumber = data.GroupDaoDaoFlowRateByDepartment.OtherNumber;
            //groupDaoDaoResultDataByDepartmentVo.TotalFlowRateNumber = data.GroupDaoDaoFlowRateByDepartment.TotalFlowRateNumber;
            //result.GroupDaoDaoFlowRateByDepartment = groupDaoDaoResultDataByDepartmentVo;

            //OperationBoardDepartmentDataVo groupJiNaResultDataByDepartmentVo = new OperationBoardDepartmentDataVo();
            //groupJiNaResultDataByDepartmentVo.BeforeLivingNumber = data.GroupJiNaFlowRateByDepartment.BeforeLivingNumber;
            //groupJiNaResultDataByDepartmentVo.BeforeLivingRate = data.GroupJiNaFlowRateByDepartment.BeforeLivingRate;
            //groupJiNaResultDataByDepartmentVo.LivingNumber = data.GroupJiNaFlowRateByDepartment.LivingNumber;
            //groupJiNaResultDataByDepartmentVo.LivingRate = data.GroupJiNaFlowRateByDepartment.LivingRate;
            //groupJiNaResultDataByDepartmentVo.AfterLivingNumber = data.GroupJiNaFlowRateByDepartment.AfterLivingNumber;
            //groupJiNaResultDataByDepartmentVo.AftereLivingRate = data.GroupJiNaFlowRateByDepartment.AftereLivingRate;
            //groupJiNaResultDataByDepartmentVo.OtherRate = data.GroupJiNaFlowRateByDepartment.OtherRate;
            //groupJiNaResultDataByDepartmentVo.OtherNumber = data.GroupJiNaFlowRateByDepartment.OtherNumber;
            //groupJiNaResultDataByDepartmentVo.TotalFlowRateNumber = data.GroupJiNaFlowRateByDepartment.TotalFlowRateNumber;
            //result.GroupJiNaFlowRateByDepartment = groupJiNaResultDataByDepartmentVo;
            #endregion

            #endregion

            #region【分组线索分析】
            result.TotalFlowRate = data.TotalFlowRate;
            result.GroupJiNaFlowRate = data.GroupJiNaFlowRate;
            result.GroupDaoDaoFlowRate = data.GroupDaoDaoFlowRate;
            #endregion

            #region 【有效/潜在线索分析】
            OperationBoardIsEffictiveDataVo totalResultDataByIsEffictiveVo = new OperationBoardIsEffictiveDataVo();
            totalResultDataByIsEffictiveVo.EffictiveNumber = data.TotalFlowRateByIsEffictive.EffictiveNumber;
            totalResultDataByIsEffictiveVo.EffictiveRate = data.TotalFlowRateByIsEffictive.EffictiveRate;
            totalResultDataByIsEffictiveVo.NotEffictiveNumber = data.TotalFlowRateByIsEffictive.NotEffictiveNumber;
            totalResultDataByIsEffictiveVo.NotEffictiveRate = data.TotalFlowRateByIsEffictive.NotEffictiveRate;
            totalResultDataByIsEffictiveVo.TotalFlowRateNumber = data.TotalFlowRateByIsEffictive.TotalFlowRateNumber;
            result.TotalFlowRateByIsEffictive = totalResultDataByIsEffictiveVo;

            #region{刀刀吉娜数据}
            //OperationBoardIsEffictiveDataVo groupDaoDaoResultDataByIsEffictiveVo = new OperationBoardIsEffictiveDataVo();
            //groupDaoDaoResultDataByIsEffictiveVo.EffictiveNumber = data.GroupDaoDaoFlowRateByIsEffictive.EffictiveNumber;
            //groupDaoDaoResultDataByIsEffictiveVo.EffictiveRate = data.GroupDaoDaoFlowRateByIsEffictive.EffictiveRate;
            //groupDaoDaoResultDataByIsEffictiveVo.NotEffictiveNumber = data.GroupDaoDaoFlowRateByIsEffictive.NotEffictiveNumber;
            //groupDaoDaoResultDataByIsEffictiveVo.NotEffictiveRate = data.GroupDaoDaoFlowRateByIsEffictive.NotEffictiveRate;
            //groupDaoDaoResultDataByIsEffictiveVo.TotalFlowRateNumber = data.GroupDaoDaoFlowRateByIsEffictive.TotalFlowRateNumber;
            //result.GroupDaoDaoFlowRateByIsEffictive = groupDaoDaoResultDataByIsEffictiveVo;

            //OperationBoardIsEffictiveDataVo groupJiNaResultDataByIsEffictiveVo = new OperationBoardIsEffictiveDataVo();
            //groupJiNaResultDataByIsEffictiveVo.EffictiveNumber = data.GroupJiNaFlowRateByIsEffictive.EffictiveNumber;
            //groupJiNaResultDataByIsEffictiveVo.EffictiveRate = data.GroupJiNaFlowRateByIsEffictive.EffictiveRate;
            //groupJiNaResultDataByIsEffictiveVo.NotEffictiveNumber = data.GroupJiNaFlowRateByIsEffictive.NotEffictiveNumber;
            //groupJiNaResultDataByIsEffictiveVo.NotEffictiveRate = data.GroupJiNaFlowRateByIsEffictive.NotEffictiveRate;
            //groupJiNaResultDataByIsEffictiveVo.TotalFlowRateNumber = data.GroupJiNaFlowRateByIsEffictive.TotalFlowRateNumber;
            //result.GroupJiNaFlowRateByIsEffictive = groupJiNaResultDataByIsEffictiveVo;
            #endregion

            #endregion

            return ResultData<GetGroupFlowRateCompareDataVo>.Success().AddData("data", result);
        }


        /// <summary>
        /// 根据条件获取线索分析柱状图（助理与机构）
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCustomerFlowRateByEmployeeAndHospital")]
        public async Task<ResultData<CustomerFlowRateDataListVo>> GetCustomerFlowRateByEmployeeAndHospitalAsync([FromQuery] QueryCustomerFlowRateWithEmployeeAndHospitalVo query)
        {
            QueryCustomerFlowRateWithEmployeeAndHospitalDto queryDto = new QueryCustomerFlowRateWithEmployeeAndHospitalDto();
            queryDto.startDate = query.startDate;
            queryDto.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryDto.CurrentMonth = query.CurrentMonth;
            queryDto.History = query.History;
            var data = await amiyaOperationsBoardService.GetCustomerFlowRateByEmployeeAndHospitalAsync(queryDto);

            CustomerFlowRateDataListVo result = new CustomerFlowRateDataListVo();
            result.HospitalFlowRate = new List<CustomerFlowRateDataVo>();
            result.EmployeeFlowRate = new List<CustomerFlowRateDataVo>();
            foreach (var x in data.EmployeeFlowRate)
            {
                CustomerFlowRateDataVo employeePerformanceVo = new CustomerFlowRateDataVo();
                employeePerformanceVo.DistributeConsulationNum = x.DistributeConsulationNum;
                employeePerformanceVo.SendOrderNum = x.SendOrderNum;
                employeePerformanceVo.VisitNum = x.VisitNum;
                employeePerformanceVo.Name = x.Name;
                result.EmployeeFlowRate.Add(employeePerformanceVo);
            }
            foreach (var x in data.HospitalFlowRate)
            {
                CustomerFlowRateDataVo hospitalPerformanceVo = new CustomerFlowRateDataVo();
                hospitalPerformanceVo.SendOrderNum = x.SendOrderNum;
                hospitalPerformanceVo.VisitNum = x.VisitNum;
                hospitalPerformanceVo.NewCustomerDealNum = x.NewCustomerDealNum;
                hospitalPerformanceVo.Name = x.Name;
                result.HospitalFlowRate.Add(hospitalPerformanceVo);
            }
            result.TotalDistributeConsulationByEmployee = result.EmployeeFlowRate.Sum(x => x.DistributeConsulationNum);
            result.TotalSendOrderByEmployee = result.EmployeeFlowRate.Sum(x => x.SendOrderNum);
            result.TotalVisitByEmployee = result.EmployeeFlowRate.Sum(x => x.VisitNum);
            result.TotalSendOrderByHospital = result.HospitalFlowRate.Sum(x => x.SendOrderNum);
            result.TotalVisitByHospital = result.HospitalFlowRate.Sum(x => x.VisitNum);
            result.TotalDealByHospital = result.HospitalFlowRate.Sum(x => x.NewCustomerDealNum);
            return ResultData<CustomerFlowRateDataListVo>.Success().AddData("data", result);
        }

        /// <summary>
        /// 根据条件获取平台流量分析
        /// </summary>
        /// <returns></returns>
        [HttpGet("getFlowRateByContentPlatform")]
        public async Task<ResultData<GetFlowRateByContentPlatformDataVo>> GetFlowRateByContentPlatformAsync([FromQuery] QueryOperationDataVo query)
        {
            GetFlowRateByContentPlatformDataVo result = new GetFlowRateByContentPlatformDataVo();
            QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
            queryOperationDataVo.startDate = query.startDate;
            queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            var data = await amiyaOperationsBoardService.GetFlowRateByContentPlatformAsync(queryOperationDataVo);

            result.DouYinFolwRateAnalize = data.DouYinFolwRateAnalize.Select(e => new Vo.BaseIdNameAndRateVo
            {
                Name = e.Value,
                Rate = e.Rate
            }).ToList();

            result.VideoNumberFolwRateAnalize = data.VideoNumberFolwRateAnalize.Select(e => new Vo.BaseIdNameAndRateVo
            {
                Name = e.Value,
                Rate = e.Rate
            }).ToList();

            result.XiaoHongShuFolwRateAnalize = data.XiaoHongShuFolwRateAnalize.Select(e => new Vo.BaseIdNameAndRateVo
            {
                Name = e.Value,
                Rate = e.Rate
            }).ToList();

            result.PrivateDataFolwRateAnalize = data.PrivateDataFolwRateAnalize.Select(e => new Vo.BaseIdNameAndRateVo
            {
                Name = e.Value,
                Rate = e.Rate
            }).ToList();
            return ResultData<GetFlowRateByContentPlatformDataVo>.Success().AddData("data", result);
        }

        /// <summary>
        /// 根据平台获取详细流量分析
        /// </summary>
        /// <returns></returns>
        [HttpGet("getFlowRateDetailsByContentPlatform")]
        public async Task<ResultData<GetFlowRateDetailsByContentPlatformDataVo>> GetFlowRateDetailsByContentPlatformAsync([FromQuery] QueryOperationDataVo query)
        {
            GetFlowRateDetailsByContentPlatformDataVo result = new GetFlowRateDetailsByContentPlatformDataVo();
            result.FolwRateDetailsAnalize = new List<Vo.BaseIdNameAndRateVo>();
            QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
            queryOperationDataVo.startDate = query.startDate;
            queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
            queryOperationDataVo.keyWord = query.keyWord;
            var data = await amiyaOperationsBoardService.GetFlowRateDetailsByContentPlatformAsync(queryOperationDataVo);

            result.FolwRateDetailsAnalize = data.FolwRateDetailsAnalize.Select(e => new Vo.BaseIdNameAndRateVo
            {
                Name = e.Value,
                Rate = e.Rate
            }).ToList();
            return ResultData<GetFlowRateDetailsByContentPlatformDataVo>.Success().AddData("data", result);
        }
        #endregion


        #region 转化

        /// <summary>
        /// 获取助理业绩目标达成情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantTargetCompleteData")]
        public async Task<ResultData<List<AssistantTargetCompleteVo>>> GetAssistantTargetCompleteDataAsync([FromQuery] QueryAssistantTargetCompleteDataVo query)
        {
            QueryAssistantTargetCompleteDataDto queryDto = new QueryAssistantTargetCompleteDataDto();
            queryDto.ShowPrivateDomain = query.ShowPrivateDomain;
            queryDto.ShowTikTok = query.ShowTikTok;
            queryDto.ShowWechatVideo = query.ShowWechatVideo;
            queryDto.ShowXiaoHongShu = query.ShowXiaoHongShu;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            var dataList = await amiyaOperationsBoardService.GetAssitantTargetCompleteAsync(queryDto);
            var res = dataList.Select(e => new AssistantTargetCompleteVo
            {
                Sort = e.Sort,
                Name = e.Name,
                NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                CurrentMonthNewCustomerPerformance = e.CurrentMonthNewCustomerPerformance,
                HistoryMonthNewCustomerPerformance = e.HistoryMonthNewCustomerPerformance,
                NewCustomerTargetComplete = e.NewCustomerTargetComplete,
                NewCustomerChainRatio = e.NewCustomerChainRatio,
                OldCustomerPerformanceTarget = e.OldCustomerPerformanceTarget,
                CurrentMonthOldCustomerPerformance = e.CurrentMonthOldCustomerPerformance,
                HistoryMonthOldCustomerPerformance = e.HistoryMonthOldCustomerPerformance,
                OldCustomerTargetComplete = e.OldCustomerTargetComplete,
                OldCustomerChainRatio = e.OldCustomerChainRatio,
                TotalCustomerPerformanceTarget = e.TotalCustomerPerformanceTarget,
                CurrentMonthTotalCustomerPerformance = e.CurrentMonthTotalCustomerPerformance,
                HistoryMonthTotalCustomerPerformance = e.HistoryMonthTotalCustomerPerformance,
                TotalCustomerTargetComplete = e.TotalCustomerTargetComplete,
                TotalCustomerChainRatio = e.TotalCustomerChainRatio,
                NewAndOldCustomerRate = e.NewAndOldCustomerRate,
                PerformanceRate = e.PerformanceRate
            }).ToList();
            return ResultData<List<AssistantTargetCompleteVo>>.Success().AddData("data", res);
        }

        /// <summary>
        /// 流量和客户转化情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("companyTransformData")]
        public async Task<ResultData<List<FlowTransFormDataVo>>> GetFlowTransformDataAsync([FromQuery] QueryTransformDataVo query)
        {
            QueryTransformDataDto queryDto = new QueryTransformDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.ShowTikTok = query.ShowTikTok;
            queryDto.ShowWechatVideo = query.ShowWechatVideo;
            queryDto.ShowXiaoHongShu = query.ShowXiaoHongShu;
            queryDto.ShowPrivateDomain = query.ShowPrivateDomain;
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            var result = await amiyaOperationsBoardService.GetFlowTransFormDataAsync(queryDto);
            var res = result.Select(e => new FlowTransFormDataVo
            {
                GroupName = e.GroupName,
                ClueCount = e.ClueCount,
                ClueEffectiveRate = e.ClueEffectiveRate,
                SendOrderCount = e.SendOrderCount,
                DistributeConsulationNum = e.DistributeConsulationNum,
                AddWechatCount = e.AddWechatCount,
                AddWechatRate = e.AddWechatRate,
                SendOrderRate = e.SendOrderRate,
                ToHospitalCount = e.ToHospitalCount,
                ToHospitalRate = e.ToHospitalRate,
                DealCount = e.DealCount,
                NewCustomerDealCount = e.NewCustomerDealCount,
                OldCustomerDealCount = e.OldCustomerDealCount,
                DealRate = e.DealRate,
                NewCustomerPerformance = e.NewCustomerPerformance,
                NewAndOldCustomerRate = e.NewAndOldCustomerRate,
                OldCustomerPerformance = e.OldCustomerPerformance,
                NewCustomerUnitPrice = e.NewCustomerUnitPrice,
                OldCustomerUnitPrice = e.OldCustomerUnitPrice,
                CustomerUnitPrice = e.CustomerUnitPrice,
                Rate = e.Rate
            }).ToList();
            return ResultData<List<FlowTransFormDataVo>>.Success().AddData("data", res);
        }

        /// <summary>
        /// 助理流量和客户转化情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantTransformData")]
        public async Task<ResultData<List<FlowTransFormDataVo>>> GetAssistantFlowTransformDataAsync([FromQuery] QueryTransformDataVo query)
        {
            QueryTransformDataDto queryDto = new QueryTransformDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.ShowTikTok = query.ShowTikTok;
            queryDto.ShowWechatVideo = query.ShowWechatVideo;
            queryDto.ShowXiaoHongShu = query.ShowXiaoHongShu;
            queryDto.ShowPrivateDomain = query.ShowPrivateDomain;
            queryDto.IsCurrentMonth = query.IsCurrentMonth;
            queryDto.BaseLiveAnchorId = query.BaseLiveAnchorId;
            var result = await amiyaOperationsBoardService.GetAssistantFlowTransFormDataAsync(queryDto);
            var res = result.Select(e => new FlowTransFormDataVo
            {
                GroupName = e.GroupName,
                ClueCount = e.ClueCount,
                ClueEffectiveRate = e.ClueEffectiveRate,
                SendOrderCount = e.SendOrderCount,
                DistributeConsulationNum = e.DistributeConsulationNum,
                AddWechatCount = e.AddWechatCount,
                AddWechatRate = e.AddWechatRate,
                SendOrderRate = e.SendOrderRate,
                ToHospitalCount = e.ToHospitalCount,
                ToHospitalRate = e.ToHospitalRate,
                DealCount = e.DealCount,
                NewCustomerDealCount = e.NewCustomerDealCount,
                OldCustomerDealCount = e.OldCustomerDealCount,
                DealRate = e.DealRate,
                NewCustomerPerformance = e.NewCustomerPerformance,
                NewAndOldCustomerRate = e.NewAndOldCustomerRate,
                OldCustomerPerformance = e.OldCustomerPerformance,
                NewCustomerUnitPrice = e.NewCustomerUnitPrice,
                OldCustomerUnitPrice = e.OldCustomerUnitPrice,
                CustomerUnitPrice = e.CustomerUnitPrice,
                Rate = e.Rate
            }).ToList();
            return ResultData<List<FlowTransFormDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 机构转化情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("hospitalTransformData")]
        public async Task<ResultData<List<HospitalTransformDataVo>>> GetHospitalTransformDataAsync([FromQuery] QueryHospitalTransformDataVo query)
        {
            QueryHospitalTransformDataDto queryDto = new QueryHospitalTransformDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.ShowDaoDao = query.ShowDaoDao;
            queryDto.ShowJiNa = query.ShowJiNa;
            queryDto.ShowLuLu = query.ShowLuLu;
            queryDto.ShowCooperate = query.ShowCooperate;
            var result = await amiyaOperationsBoardService.GetHospitalPerformanceByDateAsync(queryDto);
            var res = result.Select(e => new HospitalTransformDataVo
            {
                City = e.City,
                HospitalName = e.HospitalName,
                SendNum = e.SendNum,
                VisitNum = e.VisitNum,
                VisitRate = e.VisitRate,
                NewCustomerDealNum = e.NewCustomerDealNum,
                NewCustomerDealRate = e.NewCustomerDealRate,
                NewCustomerAchievement = e.NewCustomerAchievement,
                NewCustomerUnitPrice = e.NewCustomerUnitPrice,
                OldCustomerDealNum = e.OldCustomerDealNum,
                OldCustomerAchievement = e.OldCustomerAchievement,
                OldCustomerUnitPrice = e.OldCustomerUnitPrice,
                TotalAchievement = e.TotalAchievement,
                NewOrOldCustomerRate = e.NewOrOldCustomerRate,
                Rate = e.Rate
            }).ToList();
            return ResultData<List<HospitalTransformDataVo>>.Success().AddData("data", res);
        }

        /// <summary>
        /// 医美业绩趋势
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getTotalAchievementByYear")]
        public async Task<ResultData<PerformanceYearDataListVo>> GetTotalAchievementByYearAsync([FromQuery] QueryPerfomanceYearDataVo query)
        {
            QueryPerfomanceYearDataDto queryDto = new QueryPerfomanceYearDataDto();
            queryDto.Year = query.Year;
            queryDto.IsOldCustomer = query.IsOldCustomer;
            var result = await amiyaOperationsBoardService.GetTotalAchievementByYearAsync(queryDto);
            PerformanceYearDataListVo resultData = new PerformanceYearDataListVo();
            var res1 = result.TotalPerformanceData.Select(e => new PerformanceYearDataVo
            {
                GroupName = e.GroupName,
                SortName = e.SortName,
                JanuaryPerformance = e.JanuaryPerformance,
                FebruaryPerformance = e.FebruaryPerformance,
                MarchPerformance = e.MarchPerformance,
                AprilPerformance = e.AprilPerformance,
                MayPerformance = e.MayPerformance,
                JunePerformance = e.JunePerformance,
                JulyPerformance = e.JulyPerformance,
                AugustPerformance = e.AugustPerformance,
                SeptemberPerformance = e.SeptemberPerformance,
                OctoberPerformance = e.OctoberPerformance,
                NovemberPerformance = e.NovemberPerformance,
                DecemberPerformance = e.DecemberPerformance,
                SumPerformance = e.SumPerformance,
                AveragePerformance = e.AveragePerformance,
            }).ToList();
            resultData.TotalPerformanceData = res1;
            var res2 = result.DaoDaoPerformanceData.Select(e => new PerformanceYearDataVo
            {
                GroupName = e.GroupName,
                SortName = e.SortName,
                JanuaryPerformance = e.JanuaryPerformance,
                FebruaryPerformance = e.FebruaryPerformance,
                MarchPerformance = e.MarchPerformance,
                AprilPerformance = e.AprilPerformance,
                MayPerformance = e.MayPerformance,
                JunePerformance = e.JunePerformance,
                JulyPerformance = e.JulyPerformance,
                AugustPerformance = e.AugustPerformance,
                SeptemberPerformance = e.SeptemberPerformance,
                OctoberPerformance = e.OctoberPerformance,
                NovemberPerformance = e.NovemberPerformance,
                DecemberPerformance = e.DecemberPerformance,
                SumPerformance = e.SumPerformance,
                AveragePerformance = e.AveragePerformance,
            }).ToList();
            resultData.DaoDaoPerformanceData = res2;

            var res3 = result.JiNaPerformanceData.Select(e => new PerformanceYearDataVo
            {
                GroupName = e.GroupName,
                SortName = e.SortName,
                JanuaryPerformance = e.JanuaryPerformance,
                FebruaryPerformance = e.FebruaryPerformance,
                MarchPerformance = e.MarchPerformance,
                AprilPerformance = e.AprilPerformance,
                MayPerformance = e.MayPerformance,
                JunePerformance = e.JunePerformance,
                JulyPerformance = e.JulyPerformance,
                AugustPerformance = e.AugustPerformance,
                SeptemberPerformance = e.SeptemberPerformance,
                OctoberPerformance = e.OctoberPerformance,
                NovemberPerformance = e.NovemberPerformance,
                DecemberPerformance = e.DecemberPerformance,
                SumPerformance = e.SumPerformance,
                AveragePerformance = e.AveragePerformance,
            }).ToList();
            resultData.JiNaPerformanceData = res3;
            return ResultData<PerformanceYearDataListVo>.Success().AddData("data", resultData);
        }

        /// <summary>
        /// 医美流量趋势
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getTotalCluesByYear")]
        public async Task<ResultData<PerformanceYearDataListVo>> GetTotalCluesByYearAsync([FromQuery] QueryPerfomanceYearDataVo query)
        {
            QueryPerfomanceYearDataDto queryDto = new QueryPerfomanceYearDataDto();
            queryDto.Year = query.Year;
            queryDto.BelongChannel = query.BelongChannel;
            var result = await amiyaOperationsBoardService.GetTotalCluesByYearAsync(queryDto);
            PerformanceYearDataListVo resultData = new PerformanceYearDataListVo();
            var res2 = result.DaoDaoPerformanceData.Select(e => new PerformanceYearDataVo
            {
                GroupName = e.GroupName,
                SortName = e.SortName,
                JanuaryPerformance = e.JanuaryPerformance,
                FebruaryPerformance = e.FebruaryPerformance,
                MarchPerformance = e.MarchPerformance,
                AprilPerformance = e.AprilPerformance,
                MayPerformance = e.MayPerformance,
                JunePerformance = e.JunePerformance,
                JulyPerformance = e.JulyPerformance,
                AugustPerformance = e.AugustPerformance,
                SeptemberPerformance = e.SeptemberPerformance,
                OctoberPerformance = e.OctoberPerformance,
                NovemberPerformance = e.NovemberPerformance,
                DecemberPerformance = e.DecemberPerformance,
                SumPerformance = e.SumPerformance,
                AveragePerformance = e.AveragePerformance,
            }).ToList();
            resultData.DaoDaoPerformanceData = res2;

            var res3 = result.JiNaPerformanceData.Select(e => new PerformanceYearDataVo
            {
                GroupName = e.GroupName,
                SortName = e.SortName,
                JanuaryPerformance = e.JanuaryPerformance,
                FebruaryPerformance = e.FebruaryPerformance,
                MarchPerformance = e.MarchPerformance,
                AprilPerformance = e.AprilPerformance,
                MayPerformance = e.MayPerformance,
                JunePerformance = e.JunePerformance,
                JulyPerformance = e.JulyPerformance,
                AugustPerformance = e.AugustPerformance,
                SeptemberPerformance = e.SeptemberPerformance,
                OctoberPerformance = e.OctoberPerformance,
                NovemberPerformance = e.NovemberPerformance,
                DecemberPerformance = e.DecemberPerformance,
                SumPerformance = e.SumPerformance,
                AveragePerformance = e.AveragePerformance,
            }).ToList();
            resultData.JiNaPerformanceData = res3;
            return ResultData<PerformanceYearDataListVo>.Success().AddData("data", resultData);
        }



        #endregion

        #region 助理医美数据运营看板
        /// <summary>
        /// 助理业绩
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantPerformance")]
        public async Task<ResultData<AssistantPerformanceVo>> GetAssitantPerformanceAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssitantPerformanceAsync(queryDto);
            AssistantPerformanceVo performanceVo = new AssistantPerformanceVo();
            performanceVo.NewCustomerPerformance = res.NewCustomerPerformance;
            performanceVo.OldCustomerPerformance = res.OldCustomerPerformance;
            performanceVo.TotalPerformance = res.TotalPerformance;
            performanceVo.TodayNewCustomerPerformance = res.TodayNewCustomerPerformance;
            performanceVo.TodayOldCustomerPerformance = res.TodayOldCustomerPerformance;
            performanceVo.TodayTotalPerformance = res.TodayTotalPerformance;
            performanceVo.LastMonthNewCustomerPerformance = res.LastMonthNewCustomerPerformance;
            performanceVo.LastMonthOldCustomerPerformance = res.LastMonthOldCustomerPerformance;
            performanceVo.LastMonthTotalPerformance = res.LastMonthTotalPerformance;
            performanceVo.LastYearNewCustomerPerformance = res.LastYearNewCustomerPerformance;
            performanceVo.LastYearOldCustomerPerformance = res.LastYearOldCustomerPerformance;
            performanceVo.LastYearTotalPerformance = res.LastYearTotalPerformance;
            performanceVo.NewCustomerPerformanceChain = res.NewCustomerPerformanceChain;
            performanceVo.OldCustomerPerformanceChain = res.OldCustomerPerformanceChain;
            performanceVo.TotalPerformanceChain = res.TotalPerformanceChain;
            performanceVo.NewCustomerPerformanceYearOnYear = res.NewCustomerPerformanceYearOnYear;
            performanceVo.OldCustomerPerformanceYearOnYear = res.OldCustomerPerformanceYearOnYear;
            performanceVo.TotalPerformanceYearOnYear = res.TotalPerformanceYearOnYear;
            performanceVo.NewCustomerPerformanceTarget = res.NewCustomerPerformanceTarget;
            performanceVo.OldCustomerPerformanceTarget = res.OldCustomerPerformanceTarget;
            performanceVo.TotalPerformanceTarget = res.TotalPerformanceTarget;
            performanceVo.NewCustomerPerformanceTargetCompleteRate = res.NewCustomerPerformanceTargetCompleteRate;
            performanceVo.OldCustomerPerformanceTargetCompleteRate = res.OldCustomerPerformanceTargetCompleteRate;
            performanceVo.TotalPerformanceTargetCompleteRate = res.TotalPerformanceTargetCompleteRate;
            performanceVo.NewCustomerPerformanceTargetSchedule = res.NewCustomerPerformanceTargetSchedule;
            performanceVo.OldCustomerPerformanceTargetSchedule = res.OldCustomerPerformanceTargetSchedule;
            performanceVo.TotalPerformanceTargetSchedule = res.TotalPerformanceTargetSchedule;
            return ResultData<AssistantPerformanceVo>.Success().AddData("data", performanceVo);
        }
        /// <summary>
        /// 助理业绩折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("brokenLineData")]
        public async Task<ResultData<AssistantPerformanceBrokenLineVo>> GetAssistantPerformanceBrokenLineAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssistantPerformanceBrokenLineVo performanceBroken = new AssistantPerformanceBrokenLineVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantPerformanceBrokenLineDto(queryDto);
            performanceBroken.NewCustomerPerformance = res.NewCustomerPerformance.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            performanceBroken.OldCustomerPerformance = res.OldCustomerPerformance.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            performanceBroken.TotalPerformance = res.TotalPerformance.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            return ResultData<AssistantPerformanceBrokenLineVo>.Success().AddData("data", performanceBroken);
        }
        /// <summary>
        /// 助理新老客业绩漏斗图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantPerformanceFilterData")]
        public async Task<ResultData<AssistantOperationDataVo>> GetAssistantPerformanceFilterDataAsync([FromQuery] QueryAssistantPerformanceFilterDataVo query)
        {
            AssistantOperationDataVo result = new AssistantOperationDataVo();
            QueryAssistantPerformanceFilterDataDto queryDto = new QueryAssistantPerformanceFilterDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            queryDto.IsEffectiveCustomerData = query.IsEffectiveCustomerData;
            var performance = await amiyaOperationsBoardService.GetAssistantPerformanceFilterDataAsync(queryDto);

            AssistantNewCustomerOperationDataVo newCustomerOperationDataVo = new AssistantNewCustomerOperationDataVo();
            //newCustomerOperationDataVo.RefundCardRate = performance.NewCustomerData.RefundCardRate.HasValue ? performance.NewCustomerData.RefundCardRate.Value : 0.00M;
            //newCustomerOperationDataVo.RefundCardRateHealthValueSum = performance.NewCustomerData.RefundCardRateHealthValueSum;
            //newCustomerOperationDataVo.RefundCardRateHealthValueThisMonth = performance.NewCustomerData.RefundCardRateHealthValueThisMonth;
            newCustomerOperationDataVo.AddWeChatRate = performance.NewCustomerData.AddWeChatRate.HasValue ? performance.NewCustomerData.AddWeChatRate.Value : 0.00M;
            newCustomerOperationDataVo.AddWeChatRateHealthValueSum = performance.NewCustomerData.AddWeChatRateHealthValueSum;
            newCustomerOperationDataVo.AddWeChatRateHealthValueThisMonth = performance.NewCustomerData.AddWeChatRateHealthValueThisMonth;
            newCustomerOperationDataVo.SendOrderRate = performance.NewCustomerData.SendOrderRate.HasValue ? performance.NewCustomerData.SendOrderRate.Value : 0.00M;
            newCustomerOperationDataVo.SendOrderRateHealthValueSum = performance.NewCustomerData.SendOrderRateHealthValueSum;
            newCustomerOperationDataVo.SendOrderRateHealthValueThisMonth = performance.NewCustomerData.SendOrderRateHealthValueThisMonth;
            newCustomerOperationDataVo.ToHospitalRate = performance.NewCustomerData.ToHospitalRate.HasValue ? performance.NewCustomerData.ToHospitalRate.Value : 0.00M;
            newCustomerOperationDataVo.ToHospitalRateHealthValueSum = performance.NewCustomerData.ToHospitalRateHealthValueSum;
            newCustomerOperationDataVo.ToHospitalRateHealthValueThisMonth = performance.NewCustomerData.ToHospitalRateHealthValueThisMonth;
            newCustomerOperationDataVo.DealRate = performance.NewCustomerData.DealRate.HasValue ? performance.NewCustomerData.DealRate.Value : 0.00M;
            newCustomerOperationDataVo.DealRateHealthValueSum = performance.NewCustomerData.DealRateHealthValueSum;
            newCustomerOperationDataVo.DealRateHealthValueThisMonth = performance.NewCustomerData.DealRateHealthValueThisMonth;
            newCustomerOperationDataVo.AllocationConsulationToDealRate = performance.NewCustomerData.AllocationConsulationToDealRate.HasValue ? performance.NewCustomerData.AllocationConsulationToDealRate.Value : 0.00M;
            newCustomerOperationDataVo.SendOrderToDealRate = performance.NewCustomerData.SendOrderToDealRate.HasValue ? performance.NewCustomerData.SendOrderToDealRate.Value : 0.00M;
            newCustomerOperationDataVo.newCustomerOperationDataDetails = new List<AssistantNewCustomerOperationDataDetailsVo>();
            foreach (var x in performance.NewCustomerData.newCustomerOperationDataDetails)
            {
                AssistantNewCustomerOperationDataDetailsVo newCustomerOperationDataDetails = new AssistantNewCustomerOperationDataDetailsVo();
                newCustomerOperationDataDetails.Key = x.Key;
                newCustomerOperationDataDetails.Name = x.Name;
                newCustomerOperationDataDetails.Value = x.Value;
                newCustomerOperationDataDetails.YearToYearValue = x.YearToYearValue.HasValue ? x.YearToYearValue.Value : 0.00M;
                newCustomerOperationDataDetails.ChainRatioValue = x.ChainRatioValue.HasValue ? x.ChainRatioValue.Value : 0.00M;
                newCustomerOperationDataDetails.TargetCompleteRate = x.TargetCompleteRate.HasValue ? x.TargetCompleteRate.Value : 0.00M;
                newCustomerOperationDataVo.newCustomerOperationDataDetails.Add(newCustomerOperationDataDetails);
            }

            AssistantOldCustomerOperationDataVo oldCustomerOperationDataVo = new AssistantOldCustomerOperationDataVo();
            oldCustomerOperationDataVo.TotalDealPeople = performance.OldCustomerData.TotalDealPeople;
            oldCustomerOperationDataVo.SecondDealPeople = performance.OldCustomerData.SecondDealPeople;
            oldCustomerOperationDataVo.ThirdDealPeople = performance.OldCustomerData.ThirdDealPeople;
            oldCustomerOperationDataVo.FourthDealCustomer = performance.OldCustomerData.FourthDealCustomer;
            oldCustomerOperationDataVo.FifThOrMoreOrMoreDealCustomer = performance.OldCustomerData.FifThOrMoreOrMoreDealCustomer;
            oldCustomerOperationDataVo.SecondTimeBuyRate = performance.OldCustomerData.SecondTimeBuyRate;
            oldCustomerOperationDataVo.SecondTimeBuyRateProportion = performance.OldCustomerData.SecondTimeBuyRateProportion;
            oldCustomerOperationDataVo.ThirdTimeBuyRate = performance.OldCustomerData.ThirdTimeBuyRate;
            oldCustomerOperationDataVo.ThirdTimeBuyRateProportion = performance.OldCustomerData.ThirdTimeBuyRateProportion;
            oldCustomerOperationDataVo.FourthTimeBuyRate = performance.OldCustomerData.FourthTimeBuyRate;
            oldCustomerOperationDataVo.FourthTimeBuyRateProportion = performance.OldCustomerData.FourthTimeBuyRateProportion;
            oldCustomerOperationDataVo.FifthTimeOrMoreBuyRate = performance.OldCustomerData.FifthTimeOrMoreBuyRate;
            oldCustomerOperationDataVo.FifthTimeOrMoreBuyRateProportion = performance.OldCustomerData.FifthTimeOrMoreBuyRateProportion;
            oldCustomerOperationDataVo.BuyRate = performance.OldCustomerData.BuyRate;
            oldCustomerOperationDataVo.SecondDealCycle = performance.OldCustomerData.SecondDealCycle;
            oldCustomerOperationDataVo.ThirdDealCycle = performance.OldCustomerData.ThirdDealCycle;
            oldCustomerOperationDataVo.FourthDealCycle = performance.OldCustomerData.FourthDealCycle;
            oldCustomerOperationDataVo.FifthDealCycle = performance.OldCustomerData.FifthDealCycle;
            result.NewCustomerData = newCustomerOperationDataVo;
            result.OldCustomerData = oldCustomerOperationDataVo;

            return ResultData<AssistantOperationDataVo>.Success().AddData("data", result);


        }
        /// <summary>
        /// 助理业绩分析数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("analysisData")]
        public async Task<ResultData<AssiatantPerformanceAnalysisDataVo>> GetPerformanceAnalysisDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssiatantPerformanceAnalysisDataVo analysisData = new AssiatantPerformanceAnalysisDataVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantPerformanceAnalysisDataAsync(queryDto);
            analysisData.TypeCount = new CustomerTypePerformanceDataVo();
            analysisData.TypeCount.Total = res.TypeCount.TotalCount;
            analysisData.TypeCount.Data = res.TypeCount.Data.Select(e => new CustomerTypePerformanceDataItemVo
            {
                Name = e.Key,
                Value = e.Value,
                Rate = e.Rate
            }).ToList();
            analysisData.TypePerformance = new CustomerTypePerformanceDataVo();
            analysisData.TypePerformance.Total = DecimalExtension.ChangePriceToTenThousand(res.TypeCount.TotalCount);
            analysisData.TypePerformance.Data = res.TypePerformance.Data.Select(e => new CustomerTypePerformanceDataItemVo
            {
                Name = e.Key,
                Value = e.Value,
                Rate = e.Rate
            }).ToList();

            analysisData.DistributeConsulationData.EffictiveRate = res.DistributeConsulationData.EffictiveRate;
            analysisData.DistributeConsulationData.NotEffictiveRate = res.DistributeConsulationData.NotEffictiveRate;
            analysisData.DistributeConsulationData.EffictiveNumber = res.DistributeConsulationData.EffictiveNumber;
            analysisData.DistributeConsulationData.NotEffictiveNumber = res.DistributeConsulationData.NotEffictiveNumber;
            analysisData.DistributeConsulationData.TotalFlowRateNumber = res.DistributeConsulationData.TotalFlowRateNumber;

            analysisData.PerformanceEffictiveOrNoData.EffictivePerformanceNumber = res.PerformanceEffictiveOrNoData.EffictivePerformanceNumber;
            analysisData.PerformanceEffictiveOrNoData.EffictivePerformanceRate = res.PerformanceEffictiveOrNoData.EffictivePerformanceRate;
            analysisData.PerformanceEffictiveOrNoData.NotEffictivePerformanceNumber = res.PerformanceEffictiveOrNoData.NotEffictivePerformanceNumber;
            analysisData.PerformanceEffictiveOrNoData.NotEffictivePerformanceRate = res.PerformanceEffictiveOrNoData.NotEffictivePerformanceRate;
            analysisData.PerformanceEffictiveOrNoData.TotalPerformanceNumber = res.PerformanceEffictiveOrNoData.TotalPerformanceNumber;

            analysisData.SendOrderData.ThisMonthPerformanceNumber = res.SendOrderData.ThisMonthPerformanceNumber;
            analysisData.SendOrderData.ThisMonthPerformanceRate = res.SendOrderData.ThisMonthPerformanceRate;
            analysisData.SendOrderData.HistoryPerformanceNumber = res.SendOrderData.HistoryPerformanceNumber;
            analysisData.SendOrderData.HistoryPerformanceRate = res.SendOrderData.HistoryPerformanceRate;
            analysisData.SendOrderData.TotalPerformanceNumber = res.SendOrderData.TotalPerformanceNumber;

            analysisData.PerformanceHistoryOrNoData.ThisMonthPerformanceNumber = res.PerformanceHistoryOrNoData.ThisMonthPerformanceNumber;
            analysisData.PerformanceHistoryOrNoData.ThisMonthPerformanceRate = res.PerformanceHistoryOrNoData.ThisMonthPerformanceRate;
            analysisData.PerformanceHistoryOrNoData.HistoryPerformanceNumber = res.PerformanceHistoryOrNoData.HistoryPerformanceNumber;
            analysisData.PerformanceHistoryOrNoData.HistoryPerformanceRate = res.PerformanceHistoryOrNoData.HistoryPerformanceRate;
            analysisData.PerformanceHistoryOrNoData.TotalPerformanceNumber = res.PerformanceHistoryOrNoData.TotalPerformanceNumber;

            analysisData.CustomerDealData.TotalPerformanceNewCustomerRate = res.CustomerDealData.TotalPerformanceNewCustomerRate;
            analysisData.CustomerDealData.TotalPerformanceOldCustomerRate = res.CustomerDealData.TotalPerformanceOldCustomerRate;
            analysisData.CustomerDealData.TotalPerformanceNewCustomerNumber = res.CustomerDealData.TotalPerformanceNewCustomerNumber;
            analysisData.CustomerDealData.TotalPerformanceOldCustomerNumber = res.CustomerDealData.TotalPerformanceOldCustomerNumber;
            analysisData.CustomerDealData.TotalPerformanceNumber = res.CustomerDealData.TotalPerformanceNumber;

            analysisData.PerformanceNewCustonerOrNoData.TotalPerformanceNewCustomerRate = res.PerformanceNewCustonerOrNoData.TotalPerformanceNewCustomerRate;
            analysisData.PerformanceNewCustonerOrNoData.TotalPerformanceOldCustomerRate = res.PerformanceNewCustonerOrNoData.TotalPerformanceOldCustomerRate;
            analysisData.PerformanceNewCustonerOrNoData.TotalPerformanceNewCustomerNumber = res.PerformanceNewCustonerOrNoData.TotalPerformanceNewCustomerNumber;
            analysisData.PerformanceNewCustonerOrNoData.TotalPerformanceOldCustomerNumber = res.PerformanceNewCustonerOrNoData.TotalPerformanceOldCustomerNumber;
            analysisData.PerformanceNewCustonerOrNoData.TotalPerformanceNumber = res.PerformanceNewCustonerOrNoData.TotalPerformanceNumber;

            analysisData.Consulation = new CustomerTypePerformanceDataVo();
            analysisData.Consulation.Total = res.Consulation.TotalCount;
            analysisData.Consulation.Data = res.Consulation.Data.Select(e => new CustomerTypePerformanceDataItemVo
            {
                Name = e.Key,
                Value = e.Value,
                Rate = e.Rate
            }).ToList();

            analysisData.ConsulationPerformance = new CustomerTypePerformanceDataVo();
            analysisData.ConsulationPerformance.Total = res.ConsulationPerformance.TotalCount;
            analysisData.ConsulationPerformance.Data = res.ConsulationPerformance.Data.Select(e => new CustomerTypePerformanceDataItemVo
            {
                Name = e.Key,
                Value = e.Value,
                Rate = e.Rate
            }).ToList();

            return ResultData<AssiatantPerformanceAnalysisDataVo>.Success().AddData("data", analysisData);
        }
        /// <summary>
        /// 助理机构业绩数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantHospitalPerformanceData")]
        public async Task<ResultData<List<AssistantHospitalPerformanceVo>>> GetAssistantHospitalPerformanceDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantHospitalPerformanceDataAsync(queryDto);
            var result = res.Select(e => new AssistantHospitalPerformanceVo
            {
                Name = e.Name,
                NewCustomerPerformance = e.NewCustomerPerformance,
                OldCustomerPerformance = e.OldCustomerPerformance,
                TotalPerformance = e.TotalPerformance
            }).OrderByDescending(e => e.TotalPerformance).ToList();
            return ResultData<List<AssistantHospitalPerformanceVo>>.Success().AddData("data", result);
        }
        /// <summary>
        /// 助理机构线索数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantHospitalCluesData")]
        public async Task<ResultData<AssistantHospitalCluesDataVo>> GetAssistantHospitalCluesDataAsync([FromQuery] QueryAssistantHospitalCluesDataVo query)
        {
            AssistantHospitalCluesDataVo dataVo = new AssistantHospitalCluesDataVo();
            QueryAssistantHospitalCluesDataDto queryDto = new QueryAssistantHospitalCluesDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            queryDto.CurrentMonth = query.CurrentMonth;
            queryDto.History = query.History;
            var res = await amiyaOperationsBoardService.GetAssistantHospitalCluesDataAsync(queryDto);
            dataVo.TotalSendOrderCount = res.TotalSendOrderCount;
            dataVo.TotalVisitCount = res.TotalVisitCount;
            dataVo.TotalDealCount = res.TotalDealCount;
            dataVo.ToHospitalRate = res.ToHospitalRate;
            dataVo.DealRate = res.DealRate;
            dataVo.Items = res.Items.OrderByDescending(e => e.SendOrderCount).Select(e => new AssistantCluesDataItemVo
            {
                Name = e.Name,
                SendOrderCount = e.SendOrderCount,
                VisitCount = e.VisitCount,
                DealCount = e.DealCount,
                ToHospitalRate = e.ToHospitalRate,
                DealRate = e.DealRate
            }).ToList();
            return ResultData<AssistantHospitalCluesDataVo>.Success().AddData("data", dataVo);
        }
        /// <summary>
        /// 助理目标完成率和助理业绩占比柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assiatantTargetCompleteAndPerformanceRateData")]
        public async Task<ResultData<AssiatantTargetCompleteAndPerformanceRateVo>> GetAssiatantTargetCompleteAndPerformanceRateDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssiatantTargetCompleteAndPerformanceRateVo result = new AssiatantTargetCompleteAndPerformanceRateVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssiatantTargetCompleteAndPerformanceRateDataAsync(queryDto);
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
        /// 助理分诊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("distributeConsulationData")]
        public async Task<ResultData<AssistantDistributeConsulationVo>> GetDistributeConsulationDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssistantDistributeConsulationVo data = new AssistantDistributeConsulationVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantDistributeConsulationDataAsync(queryDto);
            data.FirstTypeCurrentDay = res.FirstType.CurrentDay;
            data.FirstTypeTotal = res.FirstType.Total;
            data.FirstTypeChainRate = res.FirstType.ChainRate;
            data.FirstTypeYearOnYear = res.FirstType.YearOnYear;

            data.SecondTypeCurrentDay = res.SecondType.CurrentDay;
            data.SecondTypeTotal = res.SecondType.Total;
            data.SecondTypeChainRate = res.SecondType.ChainRate;
            data.SecondTypeYearOnYear = res.SecondType.YearOnYear;

            data.ThirdTypeCurrentDay = res.ThirdType.CurrentDay;
            data.ThirdTypeTotal = res.ThirdType.Total;
            data.ThirdTypeChainRate = res.ThirdType.ChainRate;
            data.ThirdTypeYearOnYear = res.ThirdType.YearOnYear;

            data.TotalTypeCurrentDay = res.TotalType.CurrentDay;
            data.TotalTypeTotal = res.TotalType.Total;
            data.TotalTypeChainRate = res.TotalType.ChainRate;
            data.TotalTypeYearOnYear = res.TotalType.YearOnYear;

            return ResultData<AssistantDistributeConsulationVo>.Success().AddData("data", data);

        }
        /// <summary>
        /// 助理有效/潜在分诊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("effOrPotDistributeConsulationData")]
        public async Task<ResultData<AssistantEffOrPotDistributeConsulationVo>> GetEffOrPotDistributeConsulationDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssistantEffOrPotDistributeConsulationVo data = new AssistantEffOrPotDistributeConsulationVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantEffOrPotDistributeConsulationDataAsync(queryDto);
            data.EffctiveCurrentDayData = res.EffctiveData.CurrentDay;
            data.EffctiveTotalData = res.EffctiveData.Total;
            data.EffctiveChainRateData = res.EffctiveData.ChainRate;
            data.EffctiveYearOnYearData = res.EffctiveData.YearOnYear;

            data.PotentialCurrentDayData = res.PotentialData.CurrentDay;
            data.PotentialTotalData = res.PotentialData.Total;
            data.PotentialChainRateData = res.PotentialData.ChainRate;
            data.PotentialYearOnYearData = res.PotentialData.YearOnYear;

            data.TotalCurrentDayData = res.TotalData.CurrentDay;
            data.TotalTotalData = res.TotalData.Total;
            data.TotalChainRateData = res.TotalData.ChainRate;
            data.TotalYearOnYearData = res.TotalData.YearOnYear;

            return ResultData<AssistantEffOrPotDistributeConsulationVo>.Success().AddData("data", data);

        }
        /// <summary>
        /// 助理分诊折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantDistributeConsulationBrokenLineData")]
        public async Task<ResultData<AssistantDistributeConsulationBrokenLineVo>> GetAssistantDistributeConsulationBrokenLineDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssistantDistributeConsulationBrokenLineVo data = new AssistantDistributeConsulationBrokenLineVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantDistributeConsulationBrokenLineDataAsync(queryDto);
            data.FirstType = res.FirstType.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            data.SencondType = res.SencondType.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            data.ThirdType = res.ThirdType.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            data.TotalType = res.TotalType.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            return ResultData<AssistantDistributeConsulationBrokenLineVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 助理有效/潜在客资折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("assistantEffOrPotBrokenLineData")]
        public async Task<ResultData<AdminCustomerServiceEffOrPotBrokenLineDataVo>> GetAdminCustomerServiceEffOrPotBrokenLineDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAssistantEffOrPotBrokenLineDataAsync(queryDto);

            AdminCustomerServiceEffOrPotBrokenLineDataVo data = new AdminCustomerServiceEffOrPotBrokenLineDataVo();
            data.Effective = res.Effective.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();
            data.Potential = res.Potential.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();
            data.Total = res.Total.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();

            return ResultData<AdminCustomerServiceEffOrPotBrokenLineDataVo>.Success().AddData("data", data);

        }
        /// <summary>
        /// 周期转化
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getAssistantTransformCycleData")]
        public async Task<ResultData<AssistantTransformCycleDataVo>> GetAssistantTransformCycleDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            AssistantTransformCycleDataVo data = new AssistantTransformCycleDataVo();
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            queryDto.IsCurrent = query.IsCurrent;
            var res = await amiyaOperationsBoardService.GetAssistantTransformCycleDataAsync(queryDto);
            data.EffectiveSendCycle = res.EffectiveSendCycle;
            data.EffectiveToHospitalCycle = res.EffectiveToHospitalCycle;
            data.PotionelSendCycle = res.PotionelSendCycle;
            data.PotionelToHospitalCycle = res.PotionelToHospitalCycle;
            data.TotalSendCycle = res.TotalSendCycle;
            data.TotalToHospitalCycle = res.TotalToHospitalCycle;
            data.SendCycleData = res.SendCycleData;
            data.ToHospitalCycleData = res.ToHospitalCycleData;
            data.OldCustomerRePurcheData = res.OldCustomerRePurcheData;
            return ResultData<AssistantTransformCycleDataVo>.Success().AddData(data);
        }

        /// <summary>
        /// 运营看板周期转化
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getTransformCycleData")]
        public async Task<ResultData<AssistantTransformCycleDataVo>> GetLiveAnchorTransformCycleDataAsync([FromQuery] QueryLiveAnchorPerformanceVo query)
        {
            AssistantTransformCycleDataVo data = new AssistantTransformCycleDataVo();
            QueryLiveAnchorPerformanceDto queryDto = new QueryLiveAnchorPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.LiveAnchorBaseId = query.LiveAnchorBaseId;
            var res = await amiyaOperationsBoardService.GetLiveAnchorTransformCycleDataAsync(queryDto);
            data.EffectiveSendCycle = res.EffectiveSendCycle;
            data.EffectiveToHospitalCycle = res.EffectiveToHospitalCycle;
            data.PotionelSendCycle = res.PotionelSendCycle;
            data.PotionelToHospitalCycle = res.PotionelToHospitalCycle;
            data.TotalSendCycle = res.TotalSendCycle;
            data.TotalToHospitalCycle = res.TotalToHospitalCycle;
            data.SendCycleData = res.SendCycleData;
            data.ToHospitalCycleData = res.ToHospitalCycleData;
            data.OldCustomerRePurcheData = res.OldCustomerRePurcheData;
            return ResultData<AssistantTransformCycleDataVo>.Success().AddData(data);
        }

        #endregion

        #region 行政客服运营看板

        /// <summary>
        /// 行政客服客资数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("adminCustomerServiceCustomerTypeData")]
        public async Task<ResultData<AdminCustomerServiceCustomerTypeVo>> GetAdminCustomerServiceCustomerTypeDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAdminCustomerServiceCustomerTypeDataAsync(queryDto);

            AdminCustomerServiceCustomerTypeVo data = new AdminCustomerServiceCustomerTypeVo();
            data.FirstTypeTotal = res.FirstTypeTotal;
            data.FirstTypeChainRate = res.FirstTypeChainRate;
            data.FirstTypeYearOnYear = res.FirstTypeYearOnYear;
            data.SecondTypeTotal = res.SecondTypeTotal;
            data.SecondTypeChainRate = res.SecondTypeChainRate;
            data.SecondTypeYearOnYear = res.SecondTypeYearOnYear;
            data.ThirdTypeTotal = res.ThirdTypeTotal;
            data.ThirdTypeChainRate = res.ThirdTypeChainRate;
            data.ThirdTypeYearOnYear = res.ThirdTypeYearOnYear;
            data.TotalTypeTotal = res.TotalTypeTotal;
            data.TotalTypeChainRate = res.TotalTypeChainRate;
            data.TotalTypeYearOnYear = res.TotalTypeYearOnYear;

            return ResultData<AdminCustomerServiceCustomerTypeVo>.Success().AddData("data", data);
        }

        /// <summary>
        /// 行政客服个人客资数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("adminCustomerServiceCustomerTypeAddWechatData")]
        public async Task<ResultData<AdminCustomerServiceCustomerTypeVo>> GetAdminCustomerServiceCustomerTypeAddWechatDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAdminCustomerServiceCustomerTypeAddWechatDataAsync(queryDto);
            AdminCustomerServiceCustomerTypeVo data = new AdminCustomerServiceCustomerTypeVo();
            data.FirstTypeTotal = res.FirstTypeTotal;
            data.FirstTypeChainRate = res.FirstTypeChainRate;
            data.FirstTypeYearOnYear = res.FirstTypeYearOnYear;
            data.SecondTypeTotal = res.SecondTypeTotal;
            data.SecondTypeChainRate = res.SecondTypeChainRate;
            data.SecondTypeYearOnYear = res.SecondTypeYearOnYear;
            data.ThirdTypeTotal = res.ThirdTypeTotal;
            data.ThirdTypeChainRate = res.ThirdTypeChainRate;
            data.ThirdTypeYearOnYear = res.ThirdTypeYearOnYear;
            data.TotalTypeTotal = res.TotalTypeTotal;
            data.TotalTypeChainRate = res.TotalTypeChainRate;
            data.TotalTypeYearOnYear = res.TotalTypeYearOnYear;

            return ResultData<AdminCustomerServiceCustomerTypeVo>.Success().AddData("data", data);
        }

        /// <summary>
        /// 行政客服客资折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("adminCustomerServiceCustomerTypeBrokenLineData")]
        public async Task<ResultData<AdminCustomerServiceCustomerTypeBrokenLineDataVo>> GetAdminCustomerServiceCustomerTypeBrokenLineDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAdminCustomerServiceCustomerTypeBrokenLineDataAsync(queryDto);

            AdminCustomerServiceCustomerTypeBrokenLineDataVo data = new AdminCustomerServiceCustomerTypeBrokenLineDataVo();
            data.FirstType = res.FirstType.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();
            data.SencondType = res.SencondType.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();
            data.ThirdType = res.ThirdType.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();
            data.TotalType = res.TotalType.Select(e => new PerformanceBrokenLineListInfoVo { date = e.date, Performance = e.Performance }).ToList();
            return ResultData<AdminCustomerServiceCustomerTypeBrokenLineDataVo>.Success().AddData("data", data);

        }


        /// <summary>
        /// 行政客服漏斗图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("adminCustomerFilterData")]
        public async Task<ResultData<AdminCustomerFilterDataVo>> GetAdminCustomerFilterDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAdminCustomerFilterDataAsync(queryDto);
            AdminCustomerFilterDataVo data = new AdminCustomerFilterDataVo();
            data.GroupData = new AdminCustomerFilterDataItemVo();
            data.GroupData.AddWeChatRate = res.GroupData.AddWeChatRate;
            data.GroupData.AddWeChatRateHealthValueThisMonth = res.GroupData.AddWeChatRateHealthValueThisMonth;
            data.GroupData.SendOrderRate = res.GroupData.SendOrderRate;
            data.GroupData.SendOrderRateHealthValueThisMonth = res.GroupData.SendOrderRateHealthValueThisMonth;
            data.GroupData.ToHospitalRate = res.GroupData.ToHospitalRate;
            data.GroupData.ToHospitalRateHealthValueThisMonth = res.GroupData.ToHospitalRateHealthValueThisMonth;
            data.GroupData.SendCycle = res.GroupData.SendCycle;
            data.GroupData.HospitalCycle = res.GroupData.HospitalCycle;
            data.GroupData.DataList = res.GroupData.DataList.Select(e => new AdminCustomerFilterDetailDataVo
            {
                Key = e.Key,
                Name = e.Name,
                Value = e.Value
            }).ToList();

            data.AddwechatData = new AdminCustomerFilterDataItemVo();
            data.AddwechatData.AddWeChatRate = res.AddwechatData.AddWeChatRate;
            data.AddwechatData.AddWeChatRateHealthValueThisMonth = res.AddwechatData.AddWeChatRateHealthValueThisMonth;
            data.AddwechatData.SendOrderRate = res.AddwechatData.SendOrderRate;
            data.AddwechatData.SendOrderRateHealthValueThisMonth = res.AddwechatData.SendOrderRateHealthValueThisMonth;
            data.AddwechatData.ToHospitalRate = res.AddwechatData.ToHospitalRate;
            data.AddwechatData.ToHospitalRateHealthValueThisMonth = res.AddwechatData.ToHospitalRateHealthValueThisMonth;
            data.AddwechatData.SendCycle = res.AddwechatData.SendCycle;
            data.AddwechatData.HospitalCycle = res.AddwechatData.HospitalCycle;
            data.AddwechatData.DataList = res.AddwechatData.DataList.Select(e => new AdminCustomerFilterDetailDataVo
            {
                Key = e.Key,
                Name = e.Name,
                Value = e.Value
            }).ToList();

            return ResultData<AdminCustomerFilterDataVo>.Success().AddData("data", data);
        }

        /// <summary>
        /// 行政客服饼状图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("adminCustomerAnalysisData")]
        public async Task<ResultData<AdminCustomerAnalysisDataVo>> GetAdminCustomerAnalysisDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAdminCustomerAnalysisDataAsync(queryDto);

            AdminCustomerAnalysisDataVo data = new AdminCustomerAnalysisDataVo();
            data.DistributeConsulationDataList = res.DistributeConsulationDataList.Select(e => new ItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Rate = e.Rate
            }).ToList();
            data.DistributeConsulationAddWechatDataList = res.DistributeConsulationAddWechatDataList.Select(e => new ItemVo
            {
                Name = e.Name,
                Value = 1,
                Rate = e.Rate
            }).ToList();
            data.EffAndPotDataList = res.EffAndPotDataList.Select(e => new ItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Rate = e.Rate
            }).ToList();
            data.EffAndPotAddWechatDataList = res.EffAndPotAddWechatDataList.Select(e => new ItemVo
            {
                Name = e.Name,
                Value = 1,
                Rate = e.Rate
            }).ToList();

            return ResultData<AdminCustomerAnalysisDataVo>.Success().AddData("data", data);
        }

        /// <summary>
        /// 行政客服分诊加v率柱状图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("adminCustomerAssistantDisAndAddVData")]
        public async Task<ResultData<AdminCustomerAssistantDisAndAddVDataVo>> GetAdminCustomerAssistantDisAndAddVDataAsync([FromQuery] QueryAssistantPerformanceVo query)
        {
            QueryAssistantPerformanceDto queryDto = new QueryAssistantPerformanceDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetAdminCustomerAssistantDisAndAddVDataAsync(queryDto);

            AdminCustomerAssistantDisAndAddVDataVo data = new AdminCustomerAssistantDisAndAddVDataVo();
            data.AssistantDistributeData = res.AssistantDistributeData.Select(e => new DataItemVo
            {
                Name = e.Name,
                Value = e.Value,
            }).ToList();
            data.AssistantDistributeDataDetail = res.AssistantDistributeDataDetail.Select(e => new DataDetailItemVo
            {
                Name = e.Name,
                BeforeLiveValue = e.BeforeLiveValue,
                LivingValue = e.LivingValue,
                AfterLiveValue = e.AfterLiveValue
            }).ToList();
            data.AssistantAddWechatData = res.AssistantAddWechatData.Select(e => new DataItemVo
            {
                Name = e.Name,
                Value = e.Value,
            }).ToList();
            data.AssistantAddWechatDataDetail = res.AssistantAddWechatDataDetail.Select(e => new DataDetailItemVo
            {
                Name = e.Name,
                BeforeLiveValue = e.BeforeLiveValue,
                LivingValue = e.LivingValue,
                AfterLiveValue = e.AfterLiveValue
            }).ToList();
            return ResultData<AdminCustomerAssistantDisAndAddVDataVo>.Success().AddData("data", data);
        }

        #endregion

        #region 直播前数据运营看板

        /// <summary>
        /// 获取直播前客资和业绩数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveClueAndPerformanceData")]
        public async Task<ResultData<BeforeLiveClueAndPerformanceDataVo>> GetBeforeLiveClueAndPerformanceDataAsync([FromQuery] QueryBeforeLiveDataVo query)
        {
            QueryBeforeLiveDataDto queryDto = new QueryBeforeLiveDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetBeforeLiveClueAndPerformanceDataAsync(queryDto);
            BeforeLiveClueAndPerformanceDataVo data = new BeforeLiveClueAndPerformanceDataVo();
            data.DepartmentData = new BeforeLiveClueAndPerformanceDataItemVo();
            data.EmployeeData = new BeforeLiveClueAndPerformanceDataItemVo();
            data.DepartmentData.CurrentDayCustomerCount = res.DepartmentData.CurrentDayCustomerCount;
            data.DepartmentData.CurrentDayPerformance = res.DepartmentData.CurrentDayPerformance;
            data.DepartmentData.CustomerCount = res.DepartmentData.CustomerCount;
            data.DepartmentData.Performance = res.DepartmentData.Performance;
            data.EmployeeData.CustomerCount = res.EmployeeData.CustomerCount;
            data.EmployeeData.Performance = res.EmployeeData.Performance;
            data.EmployeeData.CurrentDayCustomerCount = res.EmployeeData.CurrentDayCustomerCount;
            data.EmployeeData.CurrentDayPerformance = res.EmployeeData.CurrentDayPerformance;
            return ResultData<BeforeLiveClueAndPerformanceDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前线索和业绩折线图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveClueAndPerformanceBrokenData")]
        public async Task<ResultData<BeforeLiveClueAndPerformanceBrokenDataVo>> GetBeforeLiveClueAndPerformanceBrokenDataAsync([FromQuery] QueryBeforeLiveBrokenDataVo query)
        {
            QueryBeforeLiveBrokenDataDto queryDto = new QueryBeforeLiveBrokenDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            queryDto.Department = query.Department;
            queryDto.Employee = query.Employee;
            var res = await amiyaOperationsBoardService.GetBeforeLiveClueAndPerformanceBrokenDataAsync(queryDto);
            BeforeLiveClueAndPerformanceBrokenDataVo data = new BeforeLiveClueAndPerformanceBrokenDataVo();
            data.ClueData = res.ClueData.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            data.PerformanceData = res.PerformanceData.Select(e => new PerformanceBrokenLineListInfoVo
            {
                date = e.date,
                Performance = e.Performance
            }).ToList();
            return ResultData<BeforeLiveClueAndPerformanceBrokenDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前漏斗图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveFilterData")]
        public async Task<ResultData<BeforeLiveFilterDataVo>> GetBeforeLiveFilterDataAsync([FromQuery] QueryBeforeLiveFilterDataVo query)
        {
            QueryBeforeLiveFilterDataDto queryDto = new QueryBeforeLiveFilterDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            queryDto.Current = query.Current;
            queryDto.History = query.History;
            var res = await amiyaOperationsBoardService.GetBeforeLiveFilterDataAsync(queryDto);
            BeforeLiveFilterDataVo data = new BeforeLiveFilterDataVo();
            data.DepartData = new BeforeLiveFilterDataItemVo();
            data.EmployeeData = new BeforeLiveFilterDataItemVo();
            data.DepartData.AddWeChatRate = res.DepartData.AddWeChatRate;
            data.DepartData.AddWeChatRateHealthValueThisMonth = res.DepartData.AddWeChatRateHealthValueThisMonth;
            data.DepartData.SendOrderRate = res.DepartData.SendOrderRate;
            data.DepartData.SendOrderRateHealthValueThisMonth = res.DepartData.SendOrderRateHealthValueThisMonth;
            data.DepartData.ToHospitalRateHealthValueThisMonth = res.DepartData.ToHospitalRateHealthValueThisMonth;
            data.DepartData.SendCycle = res.DepartData.SendCycle;
            data.DepartData.HospitalCycle = res.DepartData.HospitalCycle;
            data.DepartData.ToHospitalRate = res.DepartData.ToHospitalRate;
            data.DepartData.DealRate = res.DepartData.DealRate;
            data.DepartData.DealRateHealthValueThisMonth = res.DepartData.DealRateHealthValueThisMonth;
            data.DepartData.DataList = res.DepartData.DataList.Select(e => new BeforeLiveFilterDetailDataVo
            {
                Key = e.Key,
                Value = e.Value,
                Name = e.Name
            }).ToList();

            data.EmployeeData.AddWeChatRate = res.EmployeeData.AddWeChatRate;
            data.EmployeeData.AddWeChatRateHealthValueThisMonth = res.EmployeeData.AddWeChatRateHealthValueThisMonth;
            data.EmployeeData.SendOrderRate = res.EmployeeData.SendOrderRate;
            data.EmployeeData.SendOrderRateHealthValueThisMonth = res.EmployeeData.SendOrderRateHealthValueThisMonth;
            data.EmployeeData.ToHospitalRateHealthValueThisMonth = res.EmployeeData.ToHospitalRateHealthValueThisMonth;
            data.EmployeeData.SendCycle = res.EmployeeData.SendCycle;
            data.EmployeeData.HospitalCycle = res.EmployeeData.HospitalCycle;
            data.EmployeeData.ToHospitalRate = res.EmployeeData.ToHospitalRate;
            data.EmployeeData.DealRate = res.EmployeeData.DealRate;
            data.EmployeeData.DealRateHealthValueThisMonth = res.EmployeeData.DealRateHealthValueThisMonth;
            data.EmployeeData.DataList = res.EmployeeData.DataList.Select(e => new BeforeLiveFilterDetailDataVo
            {
                Key = e.Key,
                Value = e.Value,
                Name = e.Name
            }).ToList();
            return ResultData<BeforeLiveFilterDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前转化周期
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveTransformCycleData")]
        public async Task<ResultData<BeforeLiveTransformCycleDataVo>> GetBeforeLiveTransformCycleDataAsync([FromQuery] QueryBeforeLiveDataVo query)
        {
            QueryBeforeLiveDataDto queryDto = new QueryBeforeLiveDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetBeforeLiveTransformCycleDataAsync(queryDto);
            BeforeLiveTransformCycleDataVo data = new BeforeLiveTransformCycleDataVo();
            data.SendCycleData = res.SendCycleData;
            data.ToHospitalCycleData = res.ToHospitalCycleData.OrderBy(e => e.Value).ToList();
            return ResultData<BeforeLiveTransformCycleDataVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前业绩占比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLivePerformanceRate")]
        public async Task<ResultData<BeforeLiveTargetCompleteAndPerformanceRateVo>> GetBeforeLivePerformanceRateAsync([FromQuery] QueryBeforeLiveDataVo query)
        {
            QueryBeforeLiveDataDto queryDto = new QueryBeforeLiveDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetBeforeLivePerformanceRateAsync(queryDto);
            BeforeLiveTargetCompleteAndPerformanceRateVo data = new BeforeLiveTargetCompleteAndPerformanceRateVo();
            data.PerformanceRate = res.PerformanceRate.OrderByDescending(e => e.Value).ToList();
            return ResultData<BeforeLiveTargetCompleteAndPerformanceRateVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前线索目标完成率数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveTargetCompleteRate")]
        public async Task<ResultData<BeforeLiveTargetCompleteRateVo>> GetBeforeLiveTargetCompleteRateAsync([FromQuery] QueryBeforeLiveDataVo query)
        {
            QueryBeforeLiveDataDto queryDto = new QueryBeforeLiveDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetBeforeLiveTargetCompleteRateAsync(queryDto);
            BeforeLiveTargetCompleteRateVo data = new BeforeLiveTargetCompleteRateVo();
            data.TargetComplete = res.TargetComplete.OrderByDescending(e => e.Value).ToList();
            return ResultData<BeforeLiveTargetCompleteRateVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前部门平台线索占比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveDepartmentContentPlatformClueRate")]
        public async Task<ResultData<BeforeLiveDepartmentContentPlatformClueRateVo>> GetBeforeLiveDepartmentContentPlatformClueRateAsync([FromQuery] QueryBeforeLiveDataVo query)
        {
            QueryBeforeLiveDataDto queryDto = new QueryBeforeLiveDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var res = await amiyaOperationsBoardService.GetBeforeLiveDepartmentContentPlatformClueRateAsync(queryDto);
            BeforeLiveDepartmentContentPlatformClueRateVo data = new BeforeLiveDepartmentContentPlatformClueRateVo();
            data.DepartmentPerformance = res.DepartmentClue;
            data.DepartmentContentPlatformClueRate = res.DepartmentContentPlatformClueRate.Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            data.TikTokPerformance = res.TikTokClue;
            data.TikTokClueRate = res.TikTokClueRate.Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            data.WechatVideoPerformance = res.WechatVideoClue;
            data.WechatVideoClueRate = res.WechatVideoClueRate.Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            data.XiaohongshuPerformance = res.XiaoHongShuClue;
            data.XiaohongshuClueRate = res.XiaohongshuClueRate.Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            return ResultData<BeforeLiveDepartmentContentPlatformClueRateVo>.Success().AddData("data", data);
        }
        /// <summary>
        /// 获取直播前部门平台业绩占比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveDepartmentContentPlatformPerformanceRate")]
        public async Task<ResultData<BeforeLiveDepartmentContentPlatformPerformanceRateVo>> GetBeforeLiveDepartmentContentPlatformPerformanceRateAsync([FromQuery] QueryBeforeLiveFilterDataVo query)
        {
            QueryBeforeLiveFilterDataDto queryDto = new QueryBeforeLiveFilterDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            queryDto.Current = query.Current;
            queryDto.History = query.History;
            var res = await amiyaOperationsBoardService.GetBeforeLiveDepartmentContentPlatformPerformanceRateAsync(queryDto);
            BeforeLiveDepartmentContentPlatformPerformanceRateVo data = new BeforeLiveDepartmentContentPlatformPerformanceRateVo();
            data.DepartmentPerformance = res.DepartmentPerformance;
            data.DepartmentContentPlatformPerformanceRate = res.DepartmentContentPlatformPerformanceRate.Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            data.TikTokPerformance = res.TikTokPerformance;
            data.TikTokPerformanceRate = res.TikTokPerformanceRate.Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            data.WechatVideoPerformance = res.WechatVideoPerformance;
            data.WechatVideoPerformanceRate = res.WechatVideoPerformanceRate.Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            data.XiaohongshuPerformance = data.XiaohongshuPerformance;
            data.XiaohongshuPerformanceRate = res.XiaohongshuPerformanceRate.Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemVo
            {
                Name = e.Name,
                Value = e.Value,
                Performance = e.Performance
            }).ToList();
            return ResultData<BeforeLiveDepartmentContentPlatformPerformanceRateVo>.Success().AddData(data);
        }
        /// <summary>
        /// 部门线索同/环比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getBeforeLiveLiveanchorIPData")]
        public async Task<ResultData<List<BeforeLiveLiveanchorIPDataVo>>> GetBeforeLiveLiveanchorIPDataAsync([FromQuery] QueryBeforeLiveDataVo query)
        {
            QueryBeforeLiveDataDto queryDto = new QueryBeforeLiveDataDto();
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.AssistantId = query.AssistantId;
            var data = await amiyaOperationsBoardService.GetBeforeLiveLiveanchorIPDataAsync(queryDto);
            var res = data.Select(e => new BeforeLiveLiveanchorIPDataVo
            {
                LiveanchorIP = e.LiveanchorIP,
                ClueCount = e.ClueCount,
                YearOnYear = e.YearOnYear,
                Chain = e.Chain,
                TargetComplete = e.TargetComplete,
            }).OrderByDescending(e => e.TargetComplete).ToList();
            return ResultData<List<BeforeLiveLiveanchorIPDataVo>>.Success().AddData("data", res);
        }

        #endregion


        #region【历史版本】

        //#region 运营主看板

        ///// <summary>
        ///// 获取获客情况数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("getCustomerData")]
        //public async Task<ResultData<GetCustomerDataVo>> GetCustomerDataAsync([FromQuery] QueryOperationDataVo query)
        //{
        //    QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
        //    queryOperationDataVo.startDate = query.startDate;
        //    queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
        //    var data = await amiyaOperationsBoardService.GetCustomerDataAsync(queryOperationDataVo);
        //    GetCustomerDataVo result = new GetCustomerDataVo();
        //    result.AddCardNum = data.AddCardNum;
        //    result.RefundCardNum = data.RefundCardNum;
        //    result.DistributeConsulationNum = data.DistributeConsulationNum;
        //    result.AddWechatNum = data.AddWechatNum;
        //    return ResultData<GetCustomerDataVo>.Success().AddData("data", result);
        //}

        ///// <summary>
        ///// 获取客户运营情况数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("getCustomerAnalizeData")]
        //public async Task<ResultData<GetCustomerAnalizeDataVo>> GetCustomerAnalizeDataAsync([FromQuery] QueryOperationDataVo query)
        //{
        //    QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
        //    queryOperationDataVo.startDate = query.startDate;
        //    queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
        //    var data = await amiyaOperationsBoardService.GetCustomerAnalizeDataAsync(queryOperationDataVo);
        //    GetCustomerAnalizeDataVo result = new GetCustomerAnalizeDataVo();
        //    CustomerAnalizeByGroupVo sendNum = new CustomerAnalizeByGroupVo();
        //    sendNum.TotalNum = data.SendNum.TotalNum;
        //    sendNum.GroupDaoDao = data.SendNum.GroupDaoDao;
        //    sendNum.GroupJiNa = data.SendNum.GroupJiNa;
        //    result.SendNum = sendNum;

        //    CustomerAnalizeByGroupVo visitNum = new CustomerAnalizeByGroupVo();
        //    visitNum.TotalNum = data.VisitNum.TotalNum;
        //    visitNum.GroupDaoDao = data.VisitNum.GroupDaoDao;
        //    visitNum.GroupJiNa = data.VisitNum.GroupJiNa;
        //    result.VisitNum = visitNum;

        //    CustomerAnalizeByGroupVo dealNum = new CustomerAnalizeByGroupVo();
        //    dealNum.TotalNum = data.DealNum.TotalNum;
        //    dealNum.GroupDaoDao = data.DealNum.GroupDaoDao;
        //    dealNum.GroupJiNa = data.DealNum.GroupJiNa;
        //    result.DealNum = dealNum;
        //    return ResultData<GetCustomerAnalizeDataVo>.Success().AddData("data", result);
        //}

        ///// <summary>
        ///// 获取客户指标转化数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("getCustomerIndexTransformationData")]
        //public async Task<ResultData<List<GetCustomerIndexTransformationResultVo>>> GetCustomerIndexTransformationDataAsync([FromQuery] QueryOperationDataVo query)
        //{
        //    QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
        //    queryOperationDataVo.startDate = query.startDate;
        //    queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
        //    var data = await amiyaOperationsBoardService.GetCustomerIndexTransformationDataAsync(queryOperationDataVo);

        //    List<GetCustomerIndexTransformationResultVo> result = new List<GetCustomerIndexTransformationResultVo>();
        //    //参数转换赋值
        //    GetCustomerIndexTransformationResultVo getAddCardNum = new GetCustomerIndexTransformationResultVo();

        //    getAddCardNum.Name = "下卡量";
        //    getAddCardNum.Value = data.AddCardNum;
        //    result.Add(getAddCardNum);

        //    GetCustomerIndexTransformationResultVo getRefundCardNum = new GetCustomerIndexTransformationResultVo();
        //    getRefundCardNum.Name = "退卡量";
        //    getRefundCardNum.Value = data.RefundCardNum;
        //    result.Add(getRefundCardNum);

        //    GetCustomerIndexTransformationResultVo getDistributeConsulationNum = new GetCustomerIndexTransformationResultVo();
        //    getDistributeConsulationNum.Name = "分诊量";
        //    getDistributeConsulationNum.Value = data.DistributeConsulationNum;
        //    result.Add(getDistributeConsulationNum);

        //    GetCustomerIndexTransformationResultVo getAddWechatNum = new GetCustomerIndexTransformationResultVo();
        //    getAddWechatNum.Name = "加v量";
        //    getAddWechatNum.Value = data.AddWechatNum;
        //    result.Add(getAddWechatNum);

        //    GetCustomerIndexTransformationResultVo getSendOrderNum = new GetCustomerIndexTransformationResultVo();
        //    getSendOrderNum.Name = "派单量";
        //    getSendOrderNum.Value = data.SendOrderNum;
        //    result.Add(getSendOrderNum);

        //    GetCustomerIndexTransformationResultVo getVisitNum = new GetCustomerIndexTransformationResultVo();
        //    getVisitNum.Name = "上门量";
        //    getVisitNum.Value = data.VisitNum;
        //    result.Add(getVisitNum);

        //    GetCustomerIndexTransformationResultVo getDealNum = new GetCustomerIndexTransformationResultVo();
        //    getDealNum.Name = "成交量";
        //    getDealNum.Value = data.DealNum;
        //    result.Add(getDealNum);

        //    return ResultData<List<GetCustomerIndexTransformationResultVo>>.Success().AddData("data", result);
        //}

        ///// <summary>
        ///// 获取助理业绩分析数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("getEmployeePerformanceAnalizeData")]
        //public async Task<ResultData<GetEmployeePerformanceAnalizeDataVo>> GetEmployeePerformanceAnalizeDataAsync([FromQuery] QueryOperationDataVo query)
        //{
        //    QueryOperationDataDto queryOperationDataVo = new QueryOperationDataDto();
        //    queryOperationDataVo.startDate = query.startDate;
        //    queryOperationDataVo.endDate = query.endDate.Value.AddDays(1).AddMilliseconds(-1);
        //    var data = await amiyaOperationsBoardService.GetEmployeePerformanceAnalizeDataAsync(queryOperationDataVo);
        //    GetEmployeePerformanceAnalizeDataVo result = new GetEmployeePerformanceAnalizeDataVo();
        //    result.EmployeeDatas = data.EmployeeDatas.Select(x => new GetEmployeePerformanceDataVo
        //    {
        //        EmployeeName = x.EmployeeName,
        //        Performance = x.Performance,
        //        CompleteRate = x.CompleteRate,
        //    }).ToList();
        //    result.EmployeeDistributeConsulationNumAndAddWechats = data.EmployeeDistributeConsulationNumAndAddWechats.Select(x => new GetEmployeeDistributeConsulationNumAndAddWechatVo
        //    {
        //        EmployeeName = x.EmployeeName,
        //        DistributeConsulationNum = x.DistributeConsulationNum,
        //        AddWechatNum = x.AddWechatNum,
        //    }).OrderByDescending(x => x.DistributeConsulationNum).ToList();
        //    result.GetEmployeeCustomerAnalizes = data.GetEmployeeCustomerAnalizes.Select(x => new GetEmployeeCustomerAnalizeVo
        //    {
        //        EmployeeName = x.EmployeeName,
        //        SendOrderNum = x.SendOrderNum,
        //        VisitNum = x.VisitNum,
        //        DealNum = x.DealNum
        //    }).ToList();
        //    result.GetEmployeePerformanceRankings = data.GetEmployeePerformanceRankings.Select(x => new GetEmployeePerformanceRankingVo
        //    {
        //        EmployeeName = x.EmployeeName,
        //        Performance = x.Performance,
        //    }).ToList();
        //    return ResultData<GetEmployeePerformanceAnalizeDataVo>.Success().AddData("data", result);
        //}

        //#endregion

        //#region 公司看板
        /// <summary>
        /// 获取公司看板业绩情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("companyPerformanceData")]
        public async Task<ResultData<List<CompanyPerformanceDataVo>>> GetCompanyPerformanceDataAsync([FromQuery] QueryAmiyaCompanyOperationsDataVo query)
        {
            QueryAmiyaCompanyOperationsDataDto querDto = new QueryAmiyaCompanyOperationsDataDto();
            querDto.StartDate = query.StartDate;
            querDto.EndDate = query.EndDate;
            querDto.Unit = query.Unit;
            querDto.LiveAnchorIds = query.LiveAnchorIds;
            var data = await amiyaOperationsBoardService.GetCompanyPerformanceDataAsync(querDto);
            var res = data.Select(e => new CompanyPerformanceDataVo
            {
                GroupName = e.GroupName,
                CurrentMonthNewCustomerPerformance = e.CurrentMonthNewCustomerPerformance,
                NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                NewCustomerPerformanceTargetComplete = e.NewCustomerPerformanceTargetComplete,
                CurrentMonthOldCustomerPerformance = e.CurrentMonthOldCustomerPerformance,
                OldCustomerTarget = e.OldCustomerTarget,
                OldCustomerTargetComplete = e.OldCustomerTargetComplete,
                TotalPerformance = e.TotalPerformance,
                TotalPerformanceTarget = e.TotalPerformanceTarget,
                TotalPerformanceTargetComplete = e.TotalPerformanceTargetComplete
            }).OrderBy(e => e.GroupName).ToList();
            return ResultData<List<CompanyPerformanceDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 获取公司看板获客情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("companyCustomerAcquisition")]
        public async Task<ResultData<List<CompanyCustomerAcquisitionDataVo>>> GetCompanyCustomerAcquisitionDataAsync([FromQuery] QueryAmiyaCompanyOperationsDataVo query)
        {
            QueryAmiyaCompanyOperationsDataDto querDto = new QueryAmiyaCompanyOperationsDataDto();
            querDto.StartDate = query.StartDate;
            querDto.EndDate = query.EndDate;
            querDto.Unit = query.Unit;
            querDto.LiveAnchorIds = query.LiveAnchorIds;
            var data = await amiyaOperationsBoardService.GetCompanyCustomerAcquisitionDataAsync(querDto);
            var res = data.Select(e => new CompanyCustomerAcquisitionDataVo
            {
                GroupName = e.GroupName,
                OrderCard = e.OrderCard,
                OrderCardTarget = e.OrderCardTarget,
                OrderCardTargetComplete = e.OrderCardTargetComplete,
                RefundCard = e.RefundCard,
                OrderCardError = e.OrderCardError,
                AllocationConsulation = e.AllocationConsulation,
                AllocationConsulationTarget = e.AllocationConsulationTarget,
                AllocationConsulationTargetComplete = e.AllocationConsulationTargetComplete,
                AddWechat = e.AddWechat,
                AddWechatTarget = e.AddWechatTarget,
                AddWechatTargetComplete = e.AddWechatTargetComplete,
            }).OrderBy(e => e.GroupName).ToList();
            return ResultData<List<CompanyCustomerAcquisitionDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 获取公司看板运营情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("companyOperationsData")]
        public async Task<ResultData<List<CompanyOperationsDataVo>>> GetCompanyOperationsDataAsync([FromQuery] QueryAmiyaCompanyOperationsDataVo query)
        {
            QueryAmiyaCompanyOperationsDataDto querDto = new QueryAmiyaCompanyOperationsDataDto();
            querDto.StartDate = query.StartDate;
            querDto.EndDate = query.EndDate;
            querDto.Unit = query.Unit;
            querDto.LiveAnchorIds = query.LiveAnchorIds;
            querDto.IsOldCustomer = query.IsOldCustomer;
            var data = await amiyaOperationsBoardService.GetCompanyOperationsDataAsync(querDto);
            var res = data.Select(e => new CompanyOperationsDataVo
            {
                GroupName = e.GroupName,
                SendOrder = e.SendOrder,
                SendOrderTarget = e.SendOrderTarget,
                SendOrderTargetComplete = e.SendOrderTargetComplete,
                ToHospital = e.ToHospital,
                ToHospitalTarget = e.ToHospitalTarget,
                ToHospitalTargetComplete = e.ToHospitalTargetComplete,
                Deal = e.Deal,
                DealTarget = e.DealTarget,
                DealTargetComplete = e.DealTargetComplete,
            }).OrderBy(e => e.GroupName).ToList();
            return ResultData<List<CompanyOperationsDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 获取公司看板指标转化情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("companyIndicatorConversionData")]
        public async Task<ResultData<List<CompanyIndicatorConversionDataVo>>> GetCompanyIndicatorConversionDataAsync([FromQuery] QueryAmiyaCompanyOperationsDataVo query)
        {
            QueryAmiyaCompanyOperationsDataDto querDto = new QueryAmiyaCompanyOperationsDataDto();
            querDto.StartDate = query.StartDate;
            querDto.EndDate = query.EndDate;
            querDto.Unit = query.Unit;
            querDto.LiveAnchorIds = query.LiveAnchorIds;
            querDto.IsEffective = query.IsEffective;
            var data = await amiyaOperationsBoardService.GetCompanyIndicatorConversionDataAsync(querDto);
            var res = data.Select(e => new CompanyIndicatorConversionDataVo
            {
                GroupName = e.GroupName,
                SevenDaySendOrderRate = e.SevenDaySendOrderRate,
                FifteenDaySendOrderRate = e.FifteenDaySendOrderRate,
                OldCustomerToHospitalRate = e.OldCustomerToHospitalRate,
                RePurchaseRate = e.RePurchaseRate,
                AddWechatRate = e.AddWechatRate,
                SendOrderRate = e.SendOrderRate,
                ToHospitalRate = e.ToHospitalRate,
                NewCustomerDealRate = e.NewCustomerDealRate,
                NewCustomerUnitPrice = e.NewCustomerUnitPrice,
                OldCustomerUnitPrice = e.OldCustomerUnitPrice,
            }).OrderBy(e => e.GroupName).ToList();
            return ResultData<List<CompanyIndicatorConversionDataVo>>.Success().AddData("data", res);
        }

        /// <summary>
        /// 获取公司当月/历史新客分诊转换情况
        /// </summary>
        /// <returns></returns>
        [HttpGet("companyNewCustomerConversionData")]
        public async Task<ResultData<List<CompanyNewCustomerConversionDataVo>>> GetCompanyNewCustomerConversionDataAsync([FromQuery] QueryAmiyaCompanyOperationsDataVo query)
        {
            List<CompanyNewCustomerConversionDataDto> res = new List<CompanyNewCustomerConversionDataDto>();
            QueryAmiyaCompanyOperationsDataDto querDto = new QueryAmiyaCompanyOperationsDataDto();
            querDto.StartDate = query.StartDate;
            querDto.EndDate = query.EndDate;
            querDto.Unit = query.Unit;
            querDto.LiveAnchorIds = query.LiveAnchorIds;
            querDto.IsEffective = query.IsEffective;
            querDto.IsCurrentMonth = query.IsCurrentMonth;
            if (querDto.IsCurrentMonth.Value)
            {
                res = await amiyaOperationsBoardService.GetCompanyNewCustomerConversionDataAsync(querDto);
            }
            else
            {
                res = await amiyaOperationsBoardService.GetHistoryNewCustomerConversionDataAsync(querDto);
            }
            var data = res.Select(e => new CompanyNewCustomerConversionDataVo
            {
                GroupName = e.GroupName,
                SendOrderCount = e.SendOrderCount,
                DistributeConsulationNum = e.DistributeConsulationNum,
                AddWechatCount = e.AddWechatCount,
                AddWechatRate = e.AddWechatRate,
                SendOrderRate = e.SendOrderRate,
                ToHospitalCount = e.ToHospitalCount,
                ToHospitalRate = e.ToHospitalRate,
                DealCount = e.DealCount,
                DealRate = e.DealRate,
                Performance = e.Performance,
            }).OrderBy(e => e.GroupName).ToList();
            return ResultData<List<CompanyNewCustomerConversionDataVo>>.Success().AddData("data", data);
        }
        //#endregion

        //#region 助理看板
        /// <summary>
        /// 获取助理看板业绩情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("assistantPerformanceData")]
        public async Task<ResultData<List<AssistantPerformanceDataVo>>> GetAssistantPerformanceDataAsync([FromQuery] QueryAmiyaAssistantOperationsDataVo query)
        {
            QueryAmiyaAssistantOperationsDataDto queryDto = new QueryAmiyaAssistantOperationsDataDto();
            queryDto.Unit = query.Unit;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            if (string.IsNullOrEmpty(query.LiveAnchorIds))
            {
                queryDto.LiveAnchorIds = (await liveAnchorBaseInfoService.GetValidAsync(true)).Select(e => e.Id).ToList();
            }
            else
            {
                queryDto.LiveAnchorIds = query.LiveAnchorIds.Split(",").ToList();
            }

            queryDto.IsOldCustomer = query.IsOldCustomer;
            queryDto.IsEffective = query.IsEffective;

            var data = await amiyaOperationsBoardService.GetAssistantPerformanceDataAsync(queryDto);
            var res = data.Select(e => new AssistantPerformanceDataVo
            {
                AssistantName = e.AssistantName,
                CurrentMonthNewCustomerPerformance = e.CurrentMonthNewCustomerPerformance,
                NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                NewCustomerPerformanceTargetComplete = e.NewCustomerPerformanceTargetComplete,
                CurrentMonthOldCustomerPerformance = e.CurrentMonthOldCustomerPerformance,
                OldCustomerTarget = e.OldCustomerTarget,
                OldCustomerTargetComplete = e.OldCustomerTargetComplete,
                TotalPerformance = e.TotalPerformance,
                TotalPerformanceTarget = e.TotalPerformanceTarget,
                TotalPerformanceTargetComplete = e.TotalPerformanceTargetComplete,
            }).OrderBy(e => e.TotalPerformance).ToList();
            return ResultData<List<AssistantPerformanceDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 获取助理看板获客情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("assistantCustomerAcquisition")]
        public async Task<ResultData<List<AssistantCustomerAcquisitionDataVo>>> GetAssistantCustomerAcquisitionDataAsync([FromQuery] QueryAmiyaAssistantOperationsDataVo query)
        {
            QueryAmiyaAssistantOperationsDataDto queryDto = new QueryAmiyaAssistantOperationsDataDto();
            queryDto.Unit = query.Unit;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            if (string.IsNullOrEmpty(query.LiveAnchorIds))
            {
                queryDto.LiveAnchorIds = (await liveAnchorBaseInfoService.GetValidAsync(true)).Select(e => e.Id).ToList();
            }
            else
            {
                queryDto.LiveAnchorIds = query.LiveAnchorIds.Split(",").ToList();
            }
            queryDto.IsOldCustomer = query.IsOldCustomer;
            queryDto.IsEffective = query.IsEffective;
            var data = await amiyaOperationsBoardService.GetAssistantCustomerAcquisitionDataAsync(queryDto);
            var res = data.Select(e => new AssistantCustomerAcquisitionDataVo
            {
                AssistantName = e.AssistantName,
                PotentialAllocationConsulation = e.PotentialAllocationConsulation,
                PotentialAllocationConsulationTarget = e.PotentialAllocationConsulationTarget,
                PotentialAllocationConsulationTargetComplete = e.PotentialAllocationConsulationTargetComplete,
                PotentialAddWechat = e.PotentialAddWechat,
                PotentialAddWechatTarget = e.PotentialAddWechatTarget,
                PotentialAddWechatTargetComplete = e.PotentialAddWechatTargetComplete,
                EffectiveAllocationConsulation = e.EffectiveAllocationConsulation,
                EffectiveAllocationConsulationTarget = e.EffectiveAllocationConsulationTarget,
                EffectiveAllocationConsulationTargetComplete = e.EffectiveAllocationConsulationTargetComplete,
                EffectiveAddWechat = e.EffectiveAddWechat,
                EffectiveAddWechatTarget = e.EffectiveAddWechatTarget,
                EffectiveAddWechatTargetComplete = e.EffectiveAddWechatTargetComplete,
            }).ToList();
            return ResultData<List<AssistantCustomerAcquisitionDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 获取助理看板运营情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("assistantOperationsData")]
        public async Task<ResultData<List<AssistantOperationsDataVo>>> GetAssistantOperationsDataAsync([FromQuery] QueryAmiyaAssistantOperationsDataVo query)
        {
            QueryAmiyaAssistantOperationsDataDto queryDto = new QueryAmiyaAssistantOperationsDataDto();
            queryDto.Unit = query.Unit;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            if (string.IsNullOrEmpty(query.LiveAnchorIds))
            {
                queryDto.LiveAnchorIds = (await liveAnchorBaseInfoService.GetValidAsync(true)).Select(e => e.Id).ToList();
            }
            else
            {
                queryDto.LiveAnchorIds = query.LiveAnchorIds.Split(",").ToList();
                queryDto.LiveAnchorIds = query.LiveAnchorIds.Split(",").ToList();
            }
            queryDto.IsOldCustomer = query.IsOldCustomer;
            queryDto.IsEffective = query.IsEffective;
            var data = await amiyaOperationsBoardService.GetAssistantOperationsDataAsync(queryDto);
            var res = data.Select(e => new AssistantOperationsDataVo
            {
                AssistantName = e.AssistantName,
                SendOrder = e.SendOrder,
                SendOrderTarget = e.SendOrderTarget,
                SendOrderTargetComplete = e.SendOrderTargetComplete,
                ToHospital = e.ToHospital,
                ToHospitalTarget = e.ToHospitalTarget,
                ToHospitalTargetComplete = e.ToHospitalTargetComplete,
                Deal = e.Deal,
                DealTarget = e.DealTarget,
                DealTargetComplete = e.DealTargetComplete,
            }).ToList();
            return ResultData<List<AssistantOperationsDataVo>>.Success().AddData("data", res);
        }
        /// <summary>
        /// 获取助理看板指标转化情况数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("assistantIndicatorConversionData")]
        public async Task<ResultData<List<AssistantIndicatorConversionDataVo>>> GetAssistantIndicatorConversionDataAsync([FromQuery] QueryAmiyaAssistantOperationsDataVo query)
        {
            QueryAmiyaAssistantOperationsDataDto queryDto = new QueryAmiyaAssistantOperationsDataDto();
            queryDto.Unit = query.Unit;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            if (string.IsNullOrEmpty(query.LiveAnchorIds))
            {
                queryDto.LiveAnchorIds = (await liveAnchorBaseInfoService.GetValidAsync(true)).Select(e => e.Id).ToList();
            }
            else
            {
                queryDto.LiveAnchorIds = query.LiveAnchorIds.Split(",").ToList();
            }
            queryDto.IsOldCustomer = query.IsOldCustomer;
            queryDto.IsEffective = query.IsEffective;
            var data = await amiyaOperationsBoardService.GetAssistantIndicatorConversionDataAsync(queryDto);
            var res = data.Select(e => new AssistantIndicatorConversionDataVo
            {
                AssistantName = e.AssistantName,
                SevenDaySendOrderRate = e.SevenDaySendOrderRate,
                FifteenDaySendOrderRate = e.FifteenDaySendOrderRate,
                OldCustomerToHospitalRate = e.OldCustomerToHospitalRate,
                RePurchaseRate = e.RePurchaseRate,
                AddWechatRate = e.AddWechatRate,
                SendOrderRate = e.SendOrderRate,
                ToHospitalRate = e.ToHospitalRate,
                NewCustomerDealRate = e.NewCustomerDealRate,
                NewCustomerUnitPrice = e.NewCustomerUnitPrice,
                OldCustomerUnitPrice = e.OldCustomerUnitPrice,
            }).ToList();
            return ResultData<List<AssistantIndicatorConversionDataVo>>.Success().AddData("data", res);
        }
        //#endregion

        #endregion
    }
}
