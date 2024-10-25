using Fx.Amiya.Dto;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Input;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.HospitalPerformance;
using Fx.Amiya.Dto.Performance;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.Dto.ShoppingCartRegistration;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class AmiyaOperationsBoardServiceService : IAmiyaOperationsBoardService
    {
        private readonly ILiveAnchorMonthlyTargetBeforeLivingService liveAnchorMonthlyTargetBeforeLivingService;
        private readonly ILiveAnchorMonthlyTargetLivingService liveAnchorMonthlyTargetLivingService;
        private readonly ILiveAnchorBaseInfoService liveAnchorBaseInfoService;
        private readonly IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService;
        private readonly ILiveAnchorService liveAnchorService;
        private readonly IHospitalInfoService hospitalInfoService;
        private readonly IShoppingCartRegistrationService shoppingCartRegistrationService;
        private readonly IContentPlateFormOrderService contentPlateFormOrderService;
        private readonly IAmiyaEmployeeService amiyaEmployeeService;
        private readonly IEmployeePerformanceTargetService employeePerformanceTargetService;
        private readonly IContentPlatformOrderSendService contentPlatformOrderSendService;
        private readonly ILiveAnchorMonthlyTargetAfterLivingService liveAnchorMonthlyTargetAfterLivingService;
        private readonly IDalEmployeePerformanceTarget dalEmployeePerformanceTarget;
        private readonly IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo;
        private readonly IHealthValueService _healthValueService;
        private readonly IDalContentPlatformOrderSend _dalContentPlatformOrderSend;
        private readonly IDalShoppingCartRegistration _dalShoppingCartRegistration;
        private readonly IDalLiveAnchorMonthlyTargetBeforeLiving dalLiveAnchorMonthlyTargetBeforeLiving;
        public AmiyaOperationsBoardServiceService(
            ILiveAnchorMonthlyTargetBeforeLivingService liveAnchorMonthlyTargetBeforeLivingService,
            ILiveAnchorMonthlyTargetLivingService liveAnchorMonthlyTargetLivingService,
            ILiveAnchorBaseInfoService liveAnchorBaseInfoService,
            IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService,
            ILiveAnchorService liveAnchorService,
            IShoppingCartRegistrationService shoppingCartRegistrationService,
            IContentPlateFormOrderService contentPlateFormOrderService,
            IHospitalInfoService hospitalInfoService,
            IAmiyaEmployeeService amiyaEmployeeService,
            IEmployeePerformanceTargetService employeePerformanceTargetService,
            IContentPlatformOrderSendService contentPlatformOrderSendService,
            ILiveAnchorMonthlyTargetAfterLivingService liveAnchorMonthlyTargetAfterLivingService, IDalEmployeePerformanceTarget dalEmployeePerformanceTarget, IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo, IHealthValueService healthValueService, IDalContentPlatformOrderSend dalContentPlatformOrderSend, IDalShoppingCartRegistration dalShoppingCartRegistration, IDalLiveAnchorMonthlyTargetBeforeLiving dalLiveAnchorMonthlyTargetBeforeLiving)
        {
            this.liveAnchorMonthlyTargetLivingService = liveAnchorMonthlyTargetLivingService;
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
            this.contentPlatFormOrderDealInfoService = contentPlatFormOrderDealInfoService;
            this.liveAnchorService = liveAnchorService;
            this.hospitalInfoService = hospitalInfoService;
            this.shoppingCartRegistrationService = shoppingCartRegistrationService;
            this.liveAnchorMonthlyTargetBeforeLivingService = liveAnchorMonthlyTargetBeforeLivingService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.amiyaEmployeeService = amiyaEmployeeService;
            this.employeePerformanceTargetService = employeePerformanceTargetService;
            this.contentPlatformOrderSendService = contentPlatformOrderSendService;
            this.liveAnchorMonthlyTargetAfterLivingService = liveAnchorMonthlyTargetAfterLivingService;
            this.dalEmployeePerformanceTarget = dalEmployeePerformanceTarget;
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            this._healthValueService = healthValueService;
            _dalContentPlatformOrderSend = dalContentPlatformOrderSend;
            _dalShoppingCartRegistration = dalShoppingCartRegistration;
            this.dalLiveAnchorMonthlyTargetBeforeLiving = dalLiveAnchorMonthlyTargetBeforeLiving;
        }

        #region  运营主看板
        #region 【业绩】
        /// <summary>
        /// 获取时间进度和总业绩
        /// </summary>
        /// <returns></returns>
        public async Task<OperationTotalAchievementDataDto> GetTotalAchievementAndDateScheduleAsync(QueryOperationDataDto query)
        {
            OperationTotalAchievementDataDto result = new OperationTotalAchievementDataDto();
            var dateSchedule = DateTimeExtension.GetDatetimeSchedule(query.endDate.Value).FirstOrDefault();
            //获取各个平台的主播ID
            List<int> LiveAnchorInfo = new List<int>();
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                var liveAnchorTotal = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync(query.keyWord);
                LiveAnchorInfo = liveAnchorTotal.Select(x => x.Id).ToList();
            }
            else
            {
                var liveAnchorDaoDao = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("f0a77257-c905-4719-95c4-ad2c4f33855c");
                LiveAnchorInfo = liveAnchorDaoDao.Select(x => x.Id).ToList();

                var liveAnchorJina = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("af69dcf5-f749-41ea-8b50-fe685facdd8b");
                foreach (var x in liveAnchorJina)
                {
                    LiveAnchorInfo.Add(x.Id);
                }

                var liveAnchorZhenLu = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("fed06778-06f2-4c92-afee-f098b77ac81c");
                foreach (var x in liveAnchorZhenLu)
                {
                    LiveAnchorInfo.Add(x.Id);
                }
            }
            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);

            //获取目标
            var target = await liveAnchorMonthlyTargetAfterLivingService.GetPerformanceTargetAsync(query.endDate.Value.Year, query.endDate.Value.Month, LiveAnchorInfo);
            #region 总业绩
            //总业绩
            var order = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(sequentialDate.StartDate, sequentialDate.EndDate, LiveAnchorInfo);
            var todayOrder = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day), DateTime.Now, LiveAnchorInfo);
            var curTotalPerformance = order.Sum(o => o.Price);
            //同比业绩
            var orderYearOnYear = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(sequentialDate.LastYearThisMonthStartDate, sequentialDate.LastYearThisMonthEndDate, LiveAnchorInfo);
            //环比业绩
            var orderChain = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(sequentialDate.LastMonthStartDate, sequentialDate.LastMonthEndDate, LiveAnchorInfo);
            result.TodayTotalPerformance = todayOrder.Sum(x => x.Price);
            result.TotalPerformanceCompleteRate = DecimalExtension.CalculateTargetComplete(curTotalPerformance, target.TotalPerformanceTarget);
            result.TotalPerformanceChainRatio = DecimalExtension.CalculateChain(curTotalPerformance, orderChain.Sum(e => e.Price));
            result.TotalPerformanceYearOnYear = DecimalExtension.CalculateChain(curTotalPerformance, orderYearOnYear.Sum(e => e.Price));

            #endregion

            #region 新客业绩
            var curNewCustomer = order.Where(o => o.IsOldCustomer == false).Sum(o => o.Price);
            var newOrderYearOnYear = orderYearOnYear.Where(x => x.IsOldCustomer == false).Sum(o => o.Price);
            var newOrderChainRatio = orderChain.Where(x => x.IsOldCustomer == false).Sum(o => o.Price);
            result.TodayNewCustomerPerformance = todayOrder.Where(x => x.IsOldCustomer = false).Sum(x => x.Price);
            result.NewCustomerPerformanceCompleteRate = DecimalExtension.CalculateTargetComplete(curNewCustomer, target.NewCustomerPerformanceTarget);
            result.NewCustomerPerformanceChainRatio = DecimalExtension.CalculateChain(curNewCustomer, newOrderChainRatio);
            result.NewCustomerPerformanceYearOnYear = DecimalExtension.CalculateChain(curNewCustomer, newOrderYearOnYear);
            #endregion
            #region 老客业绩
            var curOldCustomer = order.Where(o => o.IsOldCustomer == true).Sum(o => o.Price);
            var OldOrderYearOnYear = orderYearOnYear.Where(x => x.IsOldCustomer == true).Sum(o => o.Price);
            var OldOrderChainRatio = orderChain.Where(x => x.IsOldCustomer == true).Sum(o => o.Price);
            result.TodayNewCustomerPerformance = todayOrder.Where(x => x.IsOldCustomer = true).Sum(x => x.Price);
            result.OldCustomerPerformanceCompleteRate = DecimalExtension.CalculateTargetComplete(curOldCustomer, target.OldCustomerPerformanceTarget);
            result.OldCustomerPerformanceChainRatio = DecimalExtension.CalculateChain(curOldCustomer, OldOrderChainRatio);
            result.OldCustomerPerformanceYearOnYear = DecimalExtension.CalculateChain(curOldCustomer, OldOrderYearOnYear);
            #endregion
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                if (query.keyWord == "f0a77257-c905-4719-95c4-ad2c4f33855c")
                {
                    var liveAnchorDaoDao = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("f0a77257-c905-4719-95c4-ad2c4f33855c");
                    order = order.Where(x => liveAnchorDaoDao.Select(x => x.Id).ToList().Contains(x.LiveAnchorId.Value)).ToList();
                }
                else if (query.keyWord == "af69dcf5-f749-41ea-8b50-fe685facdd8b")
                {
                    var liveAnchorJina = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("af69dcf5-f749-41ea-8b50-fe685facdd8b");
                    order = order.Where(x => liveAnchorJina.Select(x => x.Id).ToList().Contains(x.LiveAnchorId.Value)).ToList();
                }
                else
                {
                    var liveAnchorJina = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("fed06778-06f2-4c92-afee-f098b77ac81c");
                    order = order.Where(x => liveAnchorJina.Select(x => x.Id).ToList().Contains(x.LiveAnchorId.Value)).ToList();
                }
            }
            var dateList = order.GroupBy(x => x.CreateDate.Day).Select(x => new OerationTotalAchievementBrokenLineListDto
            {
                Time = x.Key,
                TotalCustomerPerformance = x.Sum(e => e.Price),
                NewCustomerPerformance = x.Where(e => e.IsOldCustomer == false).Sum(e => e.Price),
                OldCustomerPerformance = x.Where(e => e.IsOldCustomer == true).Sum(e => e.Price),
            });
            List<OerationTotalAchievementBrokenLineListDto> GroupList = new List<OerationTotalAchievementBrokenLineListDto>();
            for (int i = 1; i < dateSchedule.Key + 1; i++)
            {
                OerationTotalAchievementBrokenLineListDto item = new OerationTotalAchievementBrokenLineListDto();
                item.Time = i;
                item.TotalCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.TotalCustomerPerformance).SingleOrDefault();
                item.NewCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.NewCustomerPerformance).SingleOrDefault();
                item.OldCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.OldCustomerPerformance).SingleOrDefault();

                GroupList.Add(item);
            }
            result.TotalPerformanceBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = DecimalExtension.ChangePriceToTenThousand(e.TotalCustomerPerformance) }).OrderBy(e => Convert.ToInt32(e.date)).ToList();
            result.NewCustomerPerformanceBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = DecimalExtension.ChangePriceToTenThousand(e.NewCustomerPerformance) }).OrderBy(e => Convert.ToInt32(e.date)).ToList();
            result.OldCustomerPerformanceBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = DecimalExtension.ChangePriceToTenThousand(e.OldCustomerPerformance) }).OrderBy(e => Convert.ToInt32(e.date)).ToList();

            if (query.startDate.Value.Year == query.endDate.Value.Year && query.startDate.Value.Month == query.endDate.Value.Month)
            {
                result.TotalPerformance = curTotalPerformance;
                result.NewCustomerPerformance = curNewCustomer;
                result.OldCustomerPerformance = curOldCustomer;
            }
            else
            {
                //非本月数据总业绩取累计数据
                var sumOrder = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(query.startDate.Value, query.endDate.Value, LiveAnchorInfo);
                result.TotalPerformance = sumOrder.Sum(x => x.Price);
                result.NewCustomerPerformance = sumOrder.Where(x => x.IsOldCustomer == false).Sum(s => s.Price);
                result.OldCustomerPerformance = sumOrder.Where(x => x.IsOldCustomer == true).Sum(s => s.Price);
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取业绩分组
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<GetNewOrOldCustomerCompareDataDto> GetNewOrOldCustomerCompareDataAsync(QueryOperationDataDto query)
        {
            GetNewOrOldCustomerCompareDataDto result = new GetNewOrOldCustomerCompareDataDto();
            //获取各个平台的主播ID
            List<int> LiveAnchorInfo = new List<int>();
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                var liveAnchorTotal = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync(query.keyWord);
                LiveAnchorInfo = liveAnchorTotal.Select(x => x.Id).ToList();
            }
            else
            {
                var liveAnchorDaoDao = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("f0a77257-c905-4719-95c4-ad2c4f33855c");
                LiveAnchorInfo = liveAnchorDaoDao.Select(x => x.Id).ToList();

                var liveAnchorJina = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("af69dcf5-f749-41ea-8b50-fe685facdd8b");
                foreach (var x in liveAnchorJina)
                {
                    LiveAnchorInfo.Add(x.Id);
                }
                var liveAnchorZhenLu = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("fed06778-06f2-4c92-afee-f098b77ac81c");
                foreach (var x in liveAnchorZhenLu)
                {
                    LiveAnchorInfo.Add(x.Id);
                }
            }
            var order = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(query.startDate.Value, query.endDate.Value, LiveAnchorInfo);
            var curTotalAchievement = order.Where(x => LiveAnchorInfo.Contains(x.LiveAnchorId.Value)).ToList();
            //总业绩
            var curTotalAchievementPrice = curTotalAchievement.Sum(x => x.Price);
            var curTotalCount = curTotalAchievement.Count();

            #region 【面诊类型】
            //var unConsulationCount = curTotalAchievement.Where(x => x.ConsulationType == (int)ContentPlateFormOrderConsultationType.UnConsulation).ToList();
            //var curUnConsulation = unConsulationCount.Sum(x => x.Price);
            var videoCount = curTotalAchievement.Where(x => x.ConsulationType == (int)ContentPlateFormOrderConsultationType.Collaboration).ToList();
            var curVideo = videoCount.Sum(x => x.Price);
            var pictureCount = curTotalAchievement.Where(x => x.ConsulationType == (int)ContentPlateFormOrderConsultationType.IndependentFollowUp).ToList();
            var curPicture = pictureCount.Sum(x => x.Price);
            var audioCount = curTotalAchievement.Where(x => x.ConsulationType == (int)ContentPlateFormOrderConsultationType.Voice).ToList();
            var curAudio = audioCount.Sum(x => x.Price);
            var otherConsulationCount = curTotalAchievement.Where(x => x.ConsulationType == (int)ContentPlateFormOrderConsultationType.OTHER).ToList();
            var curConsulationOther = otherConsulationCount.Sum(x => x.Price);

            OperationBoardConsulationTypeDto totalPerformanceConsulationTypeData = new OperationBoardConsulationTypeDto();

            totalPerformanceConsulationTypeData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievementPrice);
            //totalPerformanceConsulationTypeData.UnConsulationNumber = DecimalExtension.ChangePriceToTenThousand(curUnConsulation);
            //totalPerformanceConsulationTypeData.UnConsulationRate = DecimalExtension.CalculateTargetComplete(curUnConsulation, curTotalAchievementPrice);
            totalPerformanceConsulationTypeData.VideoConsulationNumber = DecimalExtension.ChangePriceToTenThousand(curVideo);
            totalPerformanceConsulationTypeData.VideoConsulationRate = DecimalExtension.CalculateTargetComplete(curVideo, curTotalAchievementPrice);
            totalPerformanceConsulationTypeData.PictureConsulationNumber = DecimalExtension.ChangePriceToTenThousand(curPicture);
            totalPerformanceConsulationTypeData.PictureConsulationRate = DecimalExtension.CalculateTargetComplete(curPicture, curTotalAchievementPrice);
            totalPerformanceConsulationTypeData.AudioConsulationNumber = DecimalExtension.ChangePriceToTenThousand(curAudio);
            totalPerformanceConsulationTypeData.AudioConsulationRate = DecimalExtension.CalculateTargetComplete(curAudio, curTotalAchievementPrice);
            totalPerformanceConsulationTypeData.OtherNumber = DecimalExtension.ChangePriceToTenThousand(curConsulationOther);
            totalPerformanceConsulationTypeData.OtherRate = DecimalExtension.CalculateTargetComplete(curConsulationOther, curTotalAchievementPrice);
            result.TotalConsulationType = totalPerformanceConsulationTypeData;

            //人数
            OperationBoardConsulationTypeDto totalPerformanceConsulationTypeDataNumber = new OperationBoardConsulationTypeDto();
            totalPerformanceConsulationTypeDataNumber.TotalPerformanceNumber = curTotalCount;
            //totalPerformanceConsulationTypeDataNumber.UnConsulationNumber = unConsulationCount.Count();
            //totalPerformanceConsulationTypeDataNumber.UnConsulationRate = DecimalExtension.CalculateTargetComplete(unConsulationCount.Count(), curTotalCount);
            totalPerformanceConsulationTypeDataNumber.VideoConsulationNumber = videoCount.Count();
            totalPerformanceConsulationTypeDataNumber.VideoConsulationRate = DecimalExtension.CalculateTargetComplete(videoCount.Count(), curTotalCount);
            totalPerformanceConsulationTypeDataNumber.PictureConsulationNumber = pictureCount.Count();
            totalPerformanceConsulationTypeDataNumber.PictureConsulationRate = DecimalExtension.CalculateTargetComplete(pictureCount.Count(), curTotalCount);
            totalPerformanceConsulationTypeDataNumber.AudioConsulationNumber = audioCount.Count();
            totalPerformanceConsulationTypeDataNumber.AudioConsulationRate = DecimalExtension.CalculateTargetComplete(audioCount.Count(), curTotalCount);
            totalPerformanceConsulationTypeDataNumber.OtherNumber = otherConsulationCount.Count();
            totalPerformanceConsulationTypeDataNumber.OtherRate = DecimalExtension.CalculateTargetComplete(otherConsulationCount.Count(), curTotalCount);
            result.TotalConsulationTypeNumber = totalPerformanceConsulationTypeDataNumber;

            #region 刀刀组业绩-平台
            //totalPerformanceContentPlatFormGroupDaoDaoData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoDouYin); ;
            //totalPerformanceContentPlatFormGroupDaoDaoData.DouYinRate = DecimalExtension.CalculateTargetComplete(curDaoDaoDouYin, curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoVideoNumber); 
            //totalPerformanceContentPlatFormGroupDaoDaoData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curDaoDaoVideoNumber, curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoXiaoHongShu);
            //totalPerformanceContentPlatFormGroupDaoDaoData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curDaoDaoXiaoHongShu, curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoPrivateDomain);
            //totalPerformanceContentPlatFormGroupDaoDaoData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curDaoDaoPrivateDomain, curDaoDaoTotalAchievementPrice);

            //result.GroupDaoDaoFlowRateByContentPlatForm = totalPerformanceContentPlatFormGroupDaoDaoData;
            #endregion

            #region 吉娜组业绩-平台
            //OperationBoardContentPlatFormDataDetailsDto totalPerformanceContentPlatFormGroupJiNaData = new OperationBoardContentPlatFormDataDetailsDto();
            //var curJiNaDouYin = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Sum(x=>x.Price);
            //var curJiNaVideoNumber = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Sum(x => x.Price);
            //var curJiNaXiaoHongShu = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Sum(x => x.Price);
            //var curJiNaPrivateDomain = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Sum(x => x.Price);

            //totalPerformanceContentPlatFormGroupJiNaData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaDouYin); ;
            //totalPerformanceContentPlatFormGroupJiNaData.DouYinRate = DecimalExtension.CalculateTargetComplete(curJiNaDouYin, curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaVideoNumber);
            //totalPerformanceContentPlatFormGroupJiNaData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curJiNaVideoNumber, curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaXiaoHongShu);
            //totalPerformanceContentPlatFormGroupJiNaData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curJiNaXiaoHongShu, curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaPrivateDomain);
            //totalPerformanceContentPlatFormGroupJiNaData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curJiNaPrivateDomain, curJiNaTotalAchievementPrice);

            //result.GroupJiNaFlowRateByContentPlatForm = totalPerformanceContentPlatFormGroupJiNaData;
            #endregion

            #region 总业绩（优化：根据刀刀和吉娜组业绩累加）-平台
            //OperationBoardContentPlatFormDataDetailsDto totalPerformanceContentPlatFormData = new OperationBoardContentPlatFormDataDetailsDto();
            //var curDouYin = curJiNaDouYin + curDaoDaoDouYin;
            //var curVideoNumber = curJiNaVideoNumber + curDaoDaoVideoNumber;
            //var curXiaoHongShu = curJiNaXiaoHongShu + curDaoDaoXiaoHongShu;
            //var curPrivateDomain = curJiNaPrivateDomain + curDaoDaoPrivateDomain;

            //totalPerformanceContentPlatFormData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievement);
            //totalPerformanceContentPlatFormData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curDouYin);
            //totalPerformanceContentPlatFormData.DouYinRate = DecimalExtension.CalculateTargetComplete(curDouYin, curTotalAchievement);
            //totalPerformanceContentPlatFormData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curXiaoHongShu);
            //totalPerformanceContentPlatFormData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curXiaoHongShu, curTotalAchievement);
            //totalPerformanceContentPlatFormData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curVideoNumber);
            //totalPerformanceContentPlatFormData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curVideoNumber, curTotalAchievement);
            //totalPerformanceContentPlatFormData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curPrivateDomain);
            //totalPerformanceContentPlatFormData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curPrivateDomain, curTotalAchievement);
            //result.TotalFlowRateByContentPlatForm = totalPerformanceContentPlatFormData;
            #endregion
            #endregion

            #region 【平台】
            var curDouYin = curTotalAchievement.Where(x => x.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Sum(x => x.Price);
            var curVideoNumber = curTotalAchievement.Where(x => x.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Sum(x => x.Price);
            var curXiaoHongShu = curTotalAchievement.Where(x => x.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Sum(x => x.Price);
            var curPrivateDomain = curTotalAchievement.Where(x => x.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Sum(x => x.Price);

            OperationBoardContentPlatFormDataDetailsDto totalPerformanceContentPlatFormData = new OperationBoardContentPlatFormDataDetailsDto();

            totalPerformanceContentPlatFormData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceContentPlatFormData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curDouYin);
            totalPerformanceContentPlatFormData.DouYinRate = DecimalExtension.CalculateTargetComplete(curDouYin, curTotalAchievementPrice);
            totalPerformanceContentPlatFormData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curXiaoHongShu);
            totalPerformanceContentPlatFormData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curXiaoHongShu, curTotalAchievementPrice);
            totalPerformanceContentPlatFormData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curVideoNumber);
            totalPerformanceContentPlatFormData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curVideoNumber, curTotalAchievementPrice);
            totalPerformanceContentPlatFormData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curPrivateDomain);
            totalPerformanceContentPlatFormData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curPrivateDomain, curTotalAchievementPrice);
            result.TotalFlowRateByContentPlatForm = totalPerformanceContentPlatFormData;

            #region 刀刀组业绩-平台
            //totalPerformanceContentPlatFormGroupDaoDaoData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoDouYin); ;
            //totalPerformanceContentPlatFormGroupDaoDaoData.DouYinRate = DecimalExtension.CalculateTargetComplete(curDaoDaoDouYin, curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoVideoNumber); 
            //totalPerformanceContentPlatFormGroupDaoDaoData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curDaoDaoVideoNumber, curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoXiaoHongShu);
            //totalPerformanceContentPlatFormGroupDaoDaoData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curDaoDaoXiaoHongShu, curDaoDaoTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupDaoDaoData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoPrivateDomain);
            //totalPerformanceContentPlatFormGroupDaoDaoData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curDaoDaoPrivateDomain, curDaoDaoTotalAchievementPrice);

            //result.GroupDaoDaoFlowRateByContentPlatForm = totalPerformanceContentPlatFormGroupDaoDaoData;
            #endregion

            #region 吉娜组业绩-平台
            //OperationBoardContentPlatFormDataDetailsDto totalPerformanceContentPlatFormGroupJiNaData = new OperationBoardContentPlatFormDataDetailsDto();
            //var curJiNaDouYin = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Sum(x=>x.Price);
            //var curJiNaVideoNumber = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Sum(x => x.Price);
            //var curJiNaXiaoHongShu = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Sum(x => x.Price);
            //var curJiNaPrivateDomain = curJiNaTotalAchievement.Where(x => x.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Sum(x => x.Price);

            //totalPerformanceContentPlatFormGroupJiNaData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaDouYin); ;
            //totalPerformanceContentPlatFormGroupJiNaData.DouYinRate = DecimalExtension.CalculateTargetComplete(curJiNaDouYin, curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaVideoNumber);
            //totalPerformanceContentPlatFormGroupJiNaData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curJiNaVideoNumber, curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaXiaoHongShu);
            //totalPerformanceContentPlatFormGroupJiNaData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curJiNaXiaoHongShu, curJiNaTotalAchievementPrice);
            //totalPerformanceContentPlatFormGroupJiNaData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaPrivateDomain);
            //totalPerformanceContentPlatFormGroupJiNaData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curJiNaPrivateDomain, curJiNaTotalAchievementPrice);

            //result.GroupJiNaFlowRateByContentPlatForm = totalPerformanceContentPlatFormGroupJiNaData;
            #endregion

            #region 总业绩（优化：根据刀刀和吉娜组业绩累加）-平台
            //OperationBoardContentPlatFormDataDetailsDto totalPerformanceContentPlatFormData = new OperationBoardContentPlatFormDataDetailsDto();
            //var curDouYin = curJiNaDouYin + curDaoDaoDouYin;
            //var curVideoNumber = curJiNaVideoNumber + curDaoDaoVideoNumber;
            //var curXiaoHongShu = curJiNaXiaoHongShu + curDaoDaoXiaoHongShu;
            //var curPrivateDomain = curJiNaPrivateDomain + curDaoDaoPrivateDomain;

            //totalPerformanceContentPlatFormData.TotalFlowRateNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievement);
            //totalPerformanceContentPlatFormData.DouYinNumber = DecimalExtension.ChangePriceToTenThousand(curDouYin);
            //totalPerformanceContentPlatFormData.DouYinRate = DecimalExtension.CalculateTargetComplete(curDouYin, curTotalAchievement);
            //totalPerformanceContentPlatFormData.XiaoHongShuNumber = DecimalExtension.ChangePriceToTenThousand(curXiaoHongShu);
            //totalPerformanceContentPlatFormData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curXiaoHongShu, curTotalAchievement);
            //totalPerformanceContentPlatFormData.VideoNumberNumber = DecimalExtension.ChangePriceToTenThousand(curVideoNumber);
            //totalPerformanceContentPlatFormData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curVideoNumber, curTotalAchievement);
            //totalPerformanceContentPlatFormData.PrivateDataNumber = DecimalExtension.ChangePriceToTenThousand(curPrivateDomain);
            //totalPerformanceContentPlatFormData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curPrivateDomain, curTotalAchievement);
            //result.TotalFlowRateByContentPlatForm = totalPerformanceContentPlatFormData;
            #endregion
            #endregion

            #region 【部门】


            OperationBoardBelongChannelPerformanceDataDto totalPerformanceDepartmentData = new OperationBoardBelongChannelPerformanceDataDto();
            var curBeforeLiving = curTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).Sum(x => x.Price);
            var curLiving = curTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.Living).Sum(x => x.Price);
            var curAfterLiving = curTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).Sum(x => x.Price);
            var curOther = curTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.Other).Sum(x => x.Price);

            totalPerformanceDepartmentData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceDepartmentData.BeforeLivingNumber = DecimalExtension.ChangePriceToTenThousand(curBeforeLiving);
            totalPerformanceDepartmentData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curBeforeLiving, curTotalAchievementPrice);
            totalPerformanceDepartmentData.LivingNumber = DecimalExtension.ChangePriceToTenThousand(curLiving);
            totalPerformanceDepartmentData.LivingRate = DecimalExtension.CalculateTargetComplete(curLiving, curTotalAchievementPrice);
            totalPerformanceDepartmentData.AfterLivingNumber = DecimalExtension.ChangePriceToTenThousand(curAfterLiving);
            totalPerformanceDepartmentData.AfterLivingRate = DecimalExtension.CalculateTargetComplete(curAfterLiving, curTotalAchievementPrice);
            totalPerformanceDepartmentData.OtherNumber = DecimalExtension.ChangePriceToTenThousand(curOther);
            totalPerformanceDepartmentData.OtherRate = DecimalExtension.CalculateTargetComplete(curOther, curTotalAchievementPrice);
            result.TotalBelongChannelPerformance = totalPerformanceDepartmentData;
            #region 刀刀组业绩-部门
            //totalPerformanceDepartmentGroupDaoDaoData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoTotalAchievementPrice);
            //totalPerformanceDepartmentGroupDaoDaoData.BeforeLivingNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoBeforeLiving);
            //totalPerformanceDepartmentGroupDaoDaoData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curDaoDaoBeforeLiving, curDaoDaoTotalAchievementPrice);
            //totalPerformanceDepartmentGroupDaoDaoData.LivingNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoLiving);
            //totalPerformanceDepartmentGroupDaoDaoData.LivingRate = DecimalExtension.CalculateTargetComplete(curDaoDaoLiving, curDaoDaoTotalAchievementPrice);
            //totalPerformanceDepartmentGroupDaoDaoData.AfterLivingNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoAfterLiving);
            //totalPerformanceDepartmentGroupDaoDaoData.AfterLivingRate = DecimalExtension.CalculateTargetComplete(curDaoDaoAfterLiving, curDaoDaoTotalAchievementPrice);
            //totalPerformanceDepartmentGroupDaoDaoData.OtherNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoOther);
            //totalPerformanceDepartmentGroupDaoDaoData.OtherRate = DecimalExtension.CalculateTargetComplete(curDaoDaoOther, curDaoDaoTotalAchievementPrice);

            //result.GroupDaoDaoBelongChannelPerformance = totalPerformanceDepartmentGroupDaoDaoData;
            #endregion

            #region 吉娜组业绩-部门
            //OperationBoardBelongChannelPerformanceDataDto totalPerformanceDepartmentGroupJiNaData = new OperationBoardBelongChannelPerformanceDataDto();
            //var curJiNaBeforeLiving = curJiNaTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).Sum(x => x.Price);
            //var curJiNaLiving = curJiNaTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.Living).Sum(x => x.Price);
            //var curJiNaAfterLiving = curJiNaTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).Sum(x => x.Price);
            //var curJiNaOther = curJiNaTotalAchievement.Where(x => x.BelongChannel == (int)BelongChannel.Other).Sum(x => x.Price);

            //totalPerformanceDepartmentGroupJiNaData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaTotalAchievementPrice);
            //totalPerformanceDepartmentGroupJiNaData.BeforeLivingNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaBeforeLiving);
            //totalPerformanceDepartmentGroupJiNaData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curJiNaBeforeLiving, curJiNaTotalAchievementPrice);
            //totalPerformanceDepartmentGroupJiNaData.LivingNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaLiving);
            //totalPerformanceDepartmentGroupJiNaData.LivingRate = DecimalExtension.CalculateTargetComplete(curJiNaLiving, curJiNaTotalAchievementPrice);
            //totalPerformanceDepartmentGroupJiNaData.AfterLivingNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaAfterLiving);
            //totalPerformanceDepartmentGroupJiNaData.AfterLivingRate = DecimalExtension.CalculateTargetComplete(curJiNaAfterLiving, curJiNaTotalAchievementPrice);
            //totalPerformanceDepartmentGroupJiNaData.OtherNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaOther);
            //totalPerformanceDepartmentGroupJiNaData.OtherRate = DecimalExtension.CalculateTargetComplete(curJiNaOther, curJiNaTotalAchievementPrice);

            //result.GroupJiNaBelongChannelPerformance = totalPerformanceDepartmentGroupJiNaData;
            #endregion

            #region 总业绩（优化：根据刀刀和吉娜组业绩累加）-部门
            //OperationBoardBelongChannelPerformanceDataDto totalPerformanceDepartmentData = new OperationBoardBelongChannelPerformanceDataDto();
            //var curBeforeLiving = curJiNaBeforeLiving + curDaoDaoBeforeLiving;
            //var curLiving = curJiNaLiving + curDaoDaoLiving;
            //var curAfterLiving = curJiNaAfterLiving + curDaoDaoAfterLiving;
            //var curOther = curJiNaOther + curDaoDaoOther;

            //totalPerformanceDepartmentData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievement);
            //totalPerformanceDepartmentData.BeforeLivingNumber = DecimalExtension.ChangePriceToTenThousand(curBeforeLiving);
            //totalPerformanceDepartmentData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curBeforeLiving, curTotalAchievement);
            //totalPerformanceDepartmentData.LivingNumber = DecimalExtension.ChangePriceToTenThousand(curLiving);
            //totalPerformanceDepartmentData.LivingRate = DecimalExtension.CalculateTargetComplete(curLiving, curTotalAchievement);
            //totalPerformanceDepartmentData.AfterLivingNumber = DecimalExtension.ChangePriceToTenThousand(curAfterLiving);
            //totalPerformanceDepartmentData.AfterLivingRate = DecimalExtension.CalculateTargetComplete(curAfterLiving, curTotalAchievement);
            //totalPerformanceDepartmentData.OtherNumber = DecimalExtension.ChangePriceToTenThousand(curOther);
            //totalPerformanceDepartmentData.OtherRate = DecimalExtension.CalculateTargetComplete(curOther, curTotalAchievement);
            //result.TotalBelongChannelPerformance = totalPerformanceDepartmentData;
            #endregion
            #endregion

            #region 【新老客】

            OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            var NewCount = order.Where(o => o.IsOldCustomer == false).Where(x => LiveAnchorInfo.Contains(x.LiveAnchorId.Value)).ToList();
            var curNewCustomer = NewCount.Sum(o => o.Price);
            var OldCount = order.Where(o => o.IsOldCustomer == true).Where(x => LiveAnchorInfo.Contains(x.LiveAnchorId.Value)).ToList();
            var curOldCustomer = OldCount.Sum(o => o.Price);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceNewCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curNewCustomer);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(curNewCustomer, curTotalAchievementPrice);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceOldCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curOldCustomer);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(curOldCustomer, curTotalAchievementPrice);
            result.TotalNewOrOldCustomer = totalPerformanceNewOrOldCustomerData;


            //人数
            OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerNumData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNumber = curTotalCount;
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNewCustomerNumber = NewCount.Count();
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(NewCount.Count(), curTotalCount);
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceOldCustomerNumber = OldCount.Count();
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(OldCount.Count(), curTotalCount);
            result.TotalNewOrOldCustomerNum = totalPerformanceNewOrOldCustomerNumData;

            #region 刀刀组业绩-新/老客
            //OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerGroupDaoDaoData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            //var daoDaoNewCount = order.Where(o => o.IsOldCustomer == false).Where(x => LiveAnchorInfoDaoDaoResult.Contains(x.LiveAnchorId.Value)).ToList();
            //var curDaoDaoNewCustomer = daoDaoNewCount.Sum(o => o.Price);
            //var daoDaoOldCount = order.Where(o => o.IsOldCustomer == true).Where(x => LiveAnchorInfoDaoDaoResult.Contains(x.LiveAnchorId.Value)).ToList();
            //var curDaoDaoOldCustomer = daoDaoOldCount.Sum(o => o.Price);

            //totalPerformanceNewOrOldCustomerGroupDaoDaoData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoTotalAchievementPrice);

            //totalPerformanceNewOrOldCustomerGroupDaoDaoData.TotalPerformanceNewCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoNewCustomer);
            //totalPerformanceNewOrOldCustomerGroupDaoDaoData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(curDaoDaoNewCustomer, curDaoDaoTotalAchievementPrice);
            //totalPerformanceNewOrOldCustomerGroupDaoDaoData.TotalPerformanceOldCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoOldCustomer);
            //totalPerformanceNewOrOldCustomerGroupDaoDaoData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(curDaoDaoOldCustomer, curDaoDaoTotalAchievementPrice);
            //result.GroupDaoDaoNewOrOldCustomer = totalPerformanceNewOrOldCustomerGroupDaoDaoData;

            ////人数
            //OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerNumGroupDaoDaoData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            //totalPerformanceNewOrOldCustomerNumGroupDaoDaoData.TotalPerformanceNumber = curDaoDaoTotalCount;
            //totalPerformanceNewOrOldCustomerNumGroupDaoDaoData.TotalPerformanceNewCustomerNumber = daoDaoNewCount.Count();
            //totalPerformanceNewOrOldCustomerNumGroupDaoDaoData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(daoDaoNewCount.Count(), curDaoDaoTotalCount);
            //totalPerformanceNewOrOldCustomerNumGroupDaoDaoData.TotalPerformanceOldCustomerNumber = daoDaoOldCount.Count();
            //totalPerformanceNewOrOldCustomerNumGroupDaoDaoData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(daoDaoOldCount.Count(), curDaoDaoTotalCount);
            //result.GroupDaoDaoNewOrOldCustomerNum = totalPerformanceNewOrOldCustomerNumGroupDaoDaoData;
            #endregion

            #region 吉娜组业绩-新/老客
            //var JiNaNewCount = order.Where(o => o.IsOldCustomer == false).Where(x => LiveAnchorInfoJinaResult.Contains(x.LiveAnchorId.Value)).ToList();
            //var curJinaNewCustomer = JiNaNewCount.Sum(o => o.Price);
            //var JiNaOldCount = order.Where(o => o.IsOldCustomer == true).Where(x => LiveAnchorInfoJinaResult.Contains(x.LiveAnchorId.Value)).ToList();
            //var curJinaOldCustomer = JiNaOldCount.Sum(o => o.Price);

            //OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerGroupJiNaData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            //totalPerformanceNewOrOldCustomerGroupJiNaData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaTotalAchievementPrice);
            //totalPerformanceNewOrOldCustomerGroupJiNaData.TotalPerformanceNewCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curJinaNewCustomer);
            //totalPerformanceNewOrOldCustomerGroupJiNaData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(curJinaNewCustomer, curJiNaTotalAchievementPrice);
            //totalPerformanceNewOrOldCustomerGroupJiNaData.TotalPerformanceOldCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curJinaOldCustomer);
            //totalPerformanceNewOrOldCustomerGroupJiNaData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(curJinaOldCustomer, curJiNaTotalAchievementPrice);
            //result.GroupJiNaNewOrOldCustomer = totalPerformanceNewOrOldCustomerGroupJiNaData;

            ////人数
            //OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerNumGroupJiNaData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            //totalPerformanceNewOrOldCustomerNumGroupJiNaData.TotalPerformanceNumber = curJiNaTotalCount;
            //totalPerformanceNewOrOldCustomerNumGroupJiNaData.TotalPerformanceNewCustomerNumber = JiNaNewCount.Count();
            //totalPerformanceNewOrOldCustomerNumGroupJiNaData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(JiNaNewCount.Count(), curJiNaTotalCount);
            //totalPerformanceNewOrOldCustomerNumGroupJiNaData.TotalPerformanceOldCustomerNumber = JiNaOldCount.Count();
            //totalPerformanceNewOrOldCustomerNumGroupJiNaData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(JiNaOldCount.Count(), curJiNaTotalCount);
            //result.GroupJiNaNewOrOldCustomerNum = totalPerformanceNewOrOldCustomerNumGroupJiNaData;
            #endregion

            #region 总业绩（优化：根据刀刀和吉娜组业绩累加）-新/老客
            //OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            //var curNewCustomer = curDaoDaoNewCustomer + curJinaNewCustomer;
            //var curOldCustomer = curDaoDaoOldCustomer + curJinaOldCustomer;
            //totalPerformanceNewOrOldCustomerData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievement);
            //totalPerformanceNewOrOldCustomerData.TotalPerformanceNewCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curNewCustomer);
            //totalPerformanceNewOrOldCustomerData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(curNewCustomer, curTotalAchievement);
            //totalPerformanceNewOrOldCustomerData.TotalPerformanceOldCustomerNumber = DecimalExtension.ChangePriceToTenThousand(curOldCustomer);
            //totalPerformanceNewOrOldCustomerData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(curOldCustomer, curTotalAchievement);
            //result.TotalNewOrOldCustomer = totalPerformanceNewOrOldCustomerData;


            ////人数
            //OperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerNumData = new OperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            //totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNumber = curTotalCount;
            //totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNewCustomerNumber = JiNaNewCount.Count() + daoDaoNewCount.Count();
            //totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(JiNaNewCount.Count() + daoDaoNewCount.Count(), curTotalCount);
            //totalPerformanceNewOrOldCustomerNumData.TotalPerformanceOldCustomerNumber = JiNaOldCount.Count() + daoDaoNewCount.Count();
            //totalPerformanceNewOrOldCustomerNumData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(JiNaNewCount.Count() + daoDaoNewCount.Count(), curTotalCount);
            //result.TotalNewOrOldCustomerNum = totalPerformanceNewOrOldCustomerNumData;
            #endregion
            #endregion

            #region 【有效/潜在】


            OperationBoardGetIsEffictivePerformanceDto totalPerformanceIsEffictiveGroupData = new OperationBoardGetIsEffictivePerformanceDto();
            var curEffictive = curTotalAchievement.Where(x => x.AddOrderPrice > 0).Sum(x => x.Price);
            var curNotEffictive = curTotalAchievement.Where(x => x.AddOrderPrice == 0).Sum(x => x.Price);

            totalPerformanceIsEffictiveGroupData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceIsEffictiveGroupData.EffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curEffictive);
            totalPerformanceIsEffictiveGroupData.EffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curEffictive, curTotalAchievementPrice);
            totalPerformanceIsEffictiveGroupData.NotEffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curNotEffictive);
            totalPerformanceIsEffictiveGroupData.NotEffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curNotEffictive, curTotalAchievementPrice);

            result.TotalIsEffictivePerformance = totalPerformanceIsEffictiveGroupData;

            #region 刀刀组业绩-有效/潜在
            //totalPerformanceIsEffictiveGroupDaoDaoData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoTotalAchievementPrice);
            //totalPerformanceIsEffictiveGroupDaoDaoData.EffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoEffictive);
            //totalPerformanceIsEffictiveGroupDaoDaoData.EffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curDaoDaoEffictive, curDaoDaoTotalAchievementPrice);
            //totalPerformanceIsEffictiveGroupDaoDaoData.NotEffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoNotEffictive);
            //totalPerformanceIsEffictiveGroupDaoDaoData.NotEffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curDaoDaoNotEffictive, curDaoDaoTotalAchievementPrice);

            //result.GroupDaoDaoIsEffictivePerformance = totalPerformanceIsEffictiveGroupDaoDaoData;
            #endregion

            #region 吉娜组业绩-有效/潜在
            //OperationBoardGetIsEffictivePerformanceDto totalPerformanceIsEffictiveGroupJiNaData = new OperationBoardGetIsEffictivePerformanceDto();
            //var curJiNaEffictive = curJiNaTotalAchievement.Where(x => x.AddOrderPrice > 0).Sum(x => x.Price);
            //var curJiNaNotEffictive = curJiNaTotalAchievement.Where(x => x.AddOrderPrice == 0).Sum(x => x.Price);

            //totalPerformanceIsEffictiveGroupJiNaData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaTotalAchievementPrice);
            //totalPerformanceIsEffictiveGroupJiNaData.EffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaEffictive);
            //totalPerformanceIsEffictiveGroupJiNaData.EffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curJiNaEffictive, curJiNaTotalAchievementPrice);
            //totalPerformanceIsEffictiveGroupJiNaData.NotEffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaNotEffictive);
            //totalPerformanceIsEffictiveGroupJiNaData.NotEffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curJiNaNotEffictive, curJiNaTotalAchievementPrice);

            //result.GroupJiNaIsEffictivePerformance = totalPerformanceIsEffictiveGroupJiNaData;
            #endregion

            #region 总业绩（优化：根据刀刀和吉娜组业绩累加）-有效/潜在
            //OperationBoardGetIsEffictivePerformanceDto totalPerformanceIsEffictiveGroupData = new OperationBoardGetIsEffictivePerformanceDto();

            //var curEffictive = curJiNaEffictive + curDaoDaoEffictive;
            //var curNotEffictive = curJiNaNotEffictive + curDaoDaoNotEffictive;

            //totalPerformanceIsEffictiveGroupData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievement);
            //totalPerformanceIsEffictiveGroupData.EffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curEffictive);
            //totalPerformanceIsEffictiveGroupData.EffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curEffictive, curTotalAchievement);
            //totalPerformanceIsEffictiveGroupData.NotEffictivePerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curNotEffictive);
            //totalPerformanceIsEffictiveGroupData.NotEffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curNotEffictive, curTotalAchievement);

            //result.TotalIsEffictivePerformance = totalPerformanceIsEffictiveGroupData;
            #endregion
            #endregion

            #region 【当月/历史】
            OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryGroupData = new OperationBoardGetIsHistoryPerformanceDto();

            var HistoryCount = curTotalAchievement.Where(x => x.SendDate < query.startDate).ToList();
            var curHistory = HistoryCount.Sum(x => x.Price);
            var ThisMonthCount = curTotalAchievement.Where(x => x.SendDate >= query.startDate).ToList();
            var curThisMonth = ThisMonthCount.Sum(x => x.Price);

            totalPerformanceIsHistoryGroupData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceIsHistoryGroupData.HistoryPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curHistory);
            totalPerformanceIsHistoryGroupData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(curHistory, curTotalAchievementPrice);
            totalPerformanceIsHistoryGroupData.ThisMonthPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curThisMonth);
            totalPerformanceIsHistoryGroupData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(curThisMonth, curTotalAchievementPrice);

            result.TotalIsHistoryPerformance = totalPerformanceIsHistoryGroupData;


            //人数
            OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryNumData = new OperationBoardGetIsHistoryPerformanceDto();
            totalPerformanceIsHistoryNumData.TotalPerformanceNumber = curTotalCount;
            totalPerformanceIsHistoryNumData.ThisMonthPerformanceNumber = ThisMonthCount.Count();
            totalPerformanceIsHistoryNumData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(ThisMonthCount.Count(), curTotalCount);
            totalPerformanceIsHistoryNumData.HistoryPerformanceNumber = HistoryCount.Count();
            totalPerformanceIsHistoryNumData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(HistoryCount.Count(), curTotalCount);
            result.TotalIsHistoryPerformanceNum = totalPerformanceIsHistoryNumData;


            #region 刀刀组业绩-当月/历史
            //OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryGroupDaoDaoData = new OperationBoardGetIsHistoryPerformanceDto();
            //var DaoDaoHistoryCount = curDaoDaoTotalAchievement.Where(x => x.SendDate < query.startDate).ToList();
            //var curDaoDaoHistory = DaoDaoHistoryCount.Sum(x => x.Price);
            //var DaoDaoThisMonthCount = curDaoDaoTotalAchievement.Where(x => x.SendDate >= query.startDate).ToList();
            //var curDaoDaoThisMonth = DaoDaoThisMonthCount.Sum(x => x.Price);



            //totalPerformanceIsHistoryGroupDaoDaoData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoTotalAchievementPrice);
            //totalPerformanceIsHistoryGroupDaoDaoData.HistoryPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoHistory);
            //totalPerformanceIsHistoryGroupDaoDaoData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(curDaoDaoHistory, curDaoDaoTotalAchievementPrice);
            //totalPerformanceIsHistoryGroupDaoDaoData.ThisMonthPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curDaoDaoThisMonth);
            //totalPerformanceIsHistoryGroupDaoDaoData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(curDaoDaoThisMonth, curDaoDaoTotalAchievementPrice);

            //result.GroupDaoDaoIsHistoryPerformance = totalPerformanceIsHistoryGroupDaoDaoData;


            ////人数
            //OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryNumGroupDaoDaoData = new OperationBoardGetIsHistoryPerformanceDto();
            //totalPerformanceIsHistoryNumGroupDaoDaoData.TotalPerformanceNumber = curDaoDaoTotalCount;
            //totalPerformanceIsHistoryNumGroupDaoDaoData.ThisMonthPerformanceNumber = DaoDaoThisMonthCount.Count();
            //totalPerformanceIsHistoryNumGroupDaoDaoData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(DaoDaoThisMonthCount.Count(), curDaoDaoTotalCount);
            //totalPerformanceIsHistoryNumGroupDaoDaoData.HistoryPerformanceNumber = DaoDaoHistoryCount.Count();
            //totalPerformanceIsHistoryNumGroupDaoDaoData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(DaoDaoHistoryCount.Count(), curDaoDaoTotalCount);
            //result.GroupDaoDaoIsHistoryPerformanceNum = totalPerformanceIsHistoryNumGroupDaoDaoData;
            #endregion

            #region 吉娜组业绩-当月/历史
            //OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryGroupJiNaData = new OperationBoardGetIsHistoryPerformanceDto();
            //var JiNaHistoryCount = curJiNaTotalAchievement.Where(x => x.SendDate < query.startDate).ToList();
            //var curJiNaHistory = JiNaHistoryCount.Sum(x => x.Price);
            //var JiNaThisMonthCount = curJiNaTotalAchievement.Where(x => x.SendDate >= query.startDate).ToList();
            //var curJiNaThisMonth = JiNaThisMonthCount.Sum(x => x.Price);

            //totalPerformanceIsHistoryGroupJiNaData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaTotalAchievementPrice);
            //totalPerformanceIsHistoryGroupJiNaData.HistoryPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaHistory);
            //totalPerformanceIsHistoryGroupJiNaData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(curJiNaHistory, curJiNaTotalAchievementPrice);
            //totalPerformanceIsHistoryGroupJiNaData.ThisMonthPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curJiNaThisMonth);
            //totalPerformanceIsHistoryGroupJiNaData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(curJiNaThisMonth, curJiNaTotalAchievementPrice);

            //result.GroupJiNaIsHistoryPerformance = totalPerformanceIsHistoryGroupJiNaData;


            ////人数
            //OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryNumGroupJiNaData = new OperationBoardGetIsHistoryPerformanceDto();
            //totalPerformanceIsHistoryNumGroupJiNaData.TotalPerformanceNumber = curJiNaTotalCount;
            //totalPerformanceIsHistoryNumGroupJiNaData.ThisMonthPerformanceNumber = JiNaThisMonthCount.Count();
            //totalPerformanceIsHistoryNumGroupJiNaData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(JiNaThisMonthCount.Count(), curJiNaTotalCount);
            //totalPerformanceIsHistoryNumGroupJiNaData.HistoryPerformanceNumber = JiNaHistoryCount.Count();
            //totalPerformanceIsHistoryNumGroupJiNaData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(JiNaHistoryCount.Count(), curJiNaTotalCount);
            //result.GroupJiNaIsHistoryPerformanceNum = totalPerformanceIsHistoryNumGroupJiNaData;
            #endregion

            #region 总业绩（优化：根据刀刀和吉娜组业绩累加）-当月/历史
            //OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryGroupData = new OperationBoardGetIsHistoryPerformanceDto();

            //var curHistory = curJiNaHistory + curDaoDaoHistory;
            //var curThisMonth = curJiNaThisMonth + curDaoDaoThisMonth;

            //totalPerformanceIsHistoryGroupData.TotalPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curTotalAchievement);
            //totalPerformanceIsHistoryGroupData.HistoryPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curHistory);
            //totalPerformanceIsHistoryGroupData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(curHistory, curTotalAchievement);
            //totalPerformanceIsHistoryGroupData.ThisMonthPerformanceNumber = DecimalExtension.ChangePriceToTenThousand(curThisMonth);
            //totalPerformanceIsHistoryGroupData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(curThisMonth, curTotalAchievement);

            //result.TotalIsHistoryPerformance = totalPerformanceIsHistoryGroupData;


            ////人数
            //OperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryNumData = new OperationBoardGetIsHistoryPerformanceDto();
            //totalPerformanceIsHistoryNumData.TotalPerformanceNumber = curTotalCount;
            //totalPerformanceIsHistoryNumData.ThisMonthPerformanceNumber = JiNaNewCount.Count() + daoDaoNewCount.Count();
            //totalPerformanceIsHistoryNumData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(JiNaNewCount.Count() + daoDaoNewCount.Count(), curTotalCount);
            //totalPerformanceIsHistoryNumData.HistoryPerformanceNumber = JiNaOldCount.Count() + daoDaoNewCount.Count();
            //totalPerformanceIsHistoryNumData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(JiNaNewCount.Count() + daoDaoNewCount.Count(), curTotalCount);
            //result.TotalIsHistoryPerformanceNum = totalPerformanceIsHistoryNumData;
            #endregion
            #endregion
            return result;
        }


        /// <summary>
        /// 根据条件获取助理与机构新老客业绩对比情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<NewOrOldCustomerPerformanceDataListDto> GetNewOrOldCustomerCompareByEmployeeAndHospitalAsync(QueryOperationDataDto query)
        {
            List<int> LiveAnchorInfo = new List<int>();
            NewOrOldCustomerPerformanceDataListDto result = new NewOrOldCustomerPerformanceDataListDto();
            result.EmployeePerformance = new List<CustomerPerformanceDataDto>();
            result.HospitalPerformance = new List<CustomerPerformanceDataDto>();
            var orderDealInfo = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAsync(query.startDate.Value, query.endDate.Value, LiveAnchorInfo);
            #region 助理业绩（包含行政客服）
            var employeeInfo = await amiyaEmployeeService.GetCustomerServiceByBaseLiveAnchorid("f0a77257-c905-4719-95c4-ad2c4f33855c");
            var employeeInfo2 = await amiyaEmployeeService.GetCustomerServiceByBaseLiveAnchorid("af69dcf5-f749-41ea-8b50-fe685facdd8b");
            foreach (var x in employeeInfo2)
            {
                employeeInfo.Add(x);
            }
            foreach (var empInfo in employeeInfo)
            {
                CustomerPerformanceDataDto customerPerformanceDataDto = new CustomerPerformanceDataDto();
                customerPerformanceDataDto.Name = empInfo.Name;
                var newPerformance = orderDealInfo.Where(x => x.IsSupportOrder == false && x.IsOldCustomer == false && x.BelongEmployeeId == empInfo.Id).Sum(x => x.Price);
                newPerformance += orderDealInfo.Where(x => x.IsSupportOrder == true && x.IsOldCustomer == false && x.SupportEmpId == empInfo.Id).Sum(x => x.Price);
                customerPerformanceDataDto.NewCustomerPerformance = DecimalExtension.ChangePriceToTenThousand(newPerformance);


                var oldPerformance = orderDealInfo.Where(x => x.IsSupportOrder == false && x.IsOldCustomer == true && x.BelongEmployeeId == empInfo.Id).Sum(x => x.Price);
                oldPerformance += orderDealInfo.Where(x => x.IsSupportOrder == true && x.IsOldCustomer == true && x.SupportEmpId == empInfo.Id).Sum(x => x.Price);
                customerPerformanceDataDto.OldCustomerPerformance = DecimalExtension.ChangePriceToTenThousand(oldPerformance);
                customerPerformanceDataDto.TotalPerformance = customerPerformanceDataDto.NewCustomerPerformance + customerPerformanceDataDto.OldCustomerPerformance;

                result.EmployeePerformance.Add(customerPerformanceDataDto);
            }
            #endregion


            #region 机构业绩
            var hospitalIdList = orderDealInfo.GroupBy(x => x.LastDealHospitalId);
            foreach (var hospitalInfo in hospitalIdList)
            {
                CustomerPerformanceDataDto hospitalPerformanceDto = new CustomerPerformanceDataDto();
                var lastHospitalId = hospitalInfo.Key;

                var hospital = await hospitalInfoService.GetByIdAsync(lastHospitalId.Value);
                hospitalPerformanceDto.Name = hospital.Name;
                var newPerformance = orderDealInfo.Where(x => x.IsOldCustomer == false && x.LastDealHospitalId == lastHospitalId.Value).Sum(x => x.Price);
                hospitalPerformanceDto.NewCustomerPerformance = DecimalExtension.ChangePriceToTenThousand(newPerformance);


                var oldPerformance = orderDealInfo.Where(x => x.IsOldCustomer == true && x.LastDealHospitalId == lastHospitalId.Value).Sum(x => x.Price);
                hospitalPerformanceDto.OldCustomerPerformance = DecimalExtension.ChangePriceToTenThousand(oldPerformance);
                hospitalPerformanceDto.TotalPerformance = hospitalPerformanceDto.NewCustomerPerformance + hospitalPerformanceDto.OldCustomerPerformance;
                result.HospitalPerformance.Add(hospitalPerformanceDto);
            }
            #endregion
            result.EmployeePerformance = result.EmployeePerformance.OrderByDescending(x => x.TotalPerformance).Take(15).ToList();
            result.HospitalPerformance = result.HospitalPerformance.OrderByDescending(x => x.TotalPerformance).Take(15).ToList();
            return result;
        }
        #endregion

        #region 【流量】
        /// <summary>
        /// 获取获客情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<OperationTotalFlowRateDataDto> GetCustomerDataAsync(QueryOperationDataDto query)
        {
            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);
            List<int> LiveAnchorInfo = new List<int>();
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                var liveAnchorTotal = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync(query.keyWord);
                LiveAnchorInfo = liveAnchorTotal.Select(x => x.Id).ToList();
            }
            else
            {
                var liveAnchorDaoDao = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("f0a77257-c905-4719-95c4-ad2c4f33855c");
                LiveAnchorInfo = liveAnchorDaoDao.Select(x => x.Id).ToList();

                var liveAnchorJina = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("af69dcf5-f749-41ea-8b50-fe685facdd8b");
                foreach (var x in liveAnchorJina)
                {
                    LiveAnchorInfo.Add(x.Id);
                }

                var liveAnchorZhenLu = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("fed06778-06f2-4c92-afee-f098b77ac81c");
                foreach (var x in liveAnchorZhenLu)
                {
                    LiveAnchorInfo.Add(x.Id);
                }
            }
            OperationTotalFlowRateDataDto result = new OperationTotalFlowRateDataDto();
            var dateSchedule = DateTimeExtension.GetDatetimeSchedule(query.endDate.Value).FirstOrDefault();

            //获取目标
            var targetBeforeLiving = await liveAnchorMonthlyTargetBeforeLivingService.GetCluePerformanceTargetAsync(query.endDate.Value.Year, query.endDate.Value.Month, LiveAnchorInfo);
            var targetLiving = await liveAnchorMonthlyTargetLivingService.GetBasePerformanceTargetAsync(query.endDate.Value.Year, query.endDate.Value.Month, LiveAnchorInfo);
            var targetAfterLiving = await liveAnchorMonthlyTargetAfterLivingService.GetPerformanceTargetAsync(query.endDate.Value.Year, query.endDate.Value.Month, LiveAnchorInfo);


            var shoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(sequentialDate.StartDate, sequentialDate.EndDate, query.keyWord);

            var todayshoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day), DateTime.Now, query.keyWord);

            var shoppingCartRegistionYearOnYear = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(sequentialDate.LastYearThisMonthStartDate, sequentialDate.LastYearThisMonthEndDate, query.keyWord);

            var shoppingCartRegistionChain = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(sequentialDate.LastMonthStartDate, sequentialDate.LastMonthEndDate, query.keyWord);


            #region 直播前
            var curBeforeLivingClues = shoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            var BeforeLivingCluesYearOnYear = shoppingCartRegistionYearOnYear.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            var BeforeLivingCluesChainRatio = shoppingCartRegistionChain.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            result.TodayBeforeLivingClue = todayshoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            result.BeforeLivingClueCompleteRate = DecimalExtension.CalculateTargetComplete(curBeforeLivingClues, targetBeforeLiving.CluesTarget);
            result.BeforeLivingClueYearOnYear = DecimalExtension.CalculateChain(curBeforeLivingClues, BeforeLivingCluesYearOnYear);
            result.BeforeLivingClueChainRatio = DecimalExtension.CalculateChain(curBeforeLivingClues, BeforeLivingCluesChainRatio);
            #endregion

            #region 直播中
            var curLivingClue = shoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Living).Count();
            var LivingClueYearOnYear = shoppingCartRegistionYearOnYear.Where(x => x.BelongChannel == (int)BelongChannel.Living).Count();
            var LivingClueChainRatio = shoppingCartRegistionChain.Where(x => x.BelongChannel == (int)BelongChannel.Living).Count();
            result.TodayLivingClue = todayshoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.Living).Count();
            result.LivingClueCompleteRate = DecimalExtension.CalculateTargetComplete(curLivingClue, targetLiving.ConsulationCardTarget);
            result.LivingClueYearOnYear = DecimalExtension.CalculateChain(curLivingClue, LivingClueYearOnYear);
            result.LivingClueChainRatio = DecimalExtension.CalculateChain(curLivingClue, LivingClueChainRatio);
            #endregion

            #region 直播后
            var AfterLivingClueList = shoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveAfter).ToList();
            var curAfterLivingClue = AfterLivingClueList.Count();
            var historyYearAfterLivingClueList = shoppingCartRegistionYearOnYear.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).ToList();
            var AfterLivingClueYearOnYear = historyYearAfterLivingClueList.Count();
            var lastMonthAfterLivingClue = shoppingCartRegistionChain.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).ToList();
            var AfterLivingClueChainRatio = lastMonthAfterLivingClue.Count();
            var todayAfterLivingClue = todayshoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).ToList();
            result.TodayTotalAfterLivingClue = todayAfterLivingClue.Count();
            result.TotalAfterLivingClueCompleteRate = DecimalExtension.CalculateTargetComplete(curAfterLivingClue, targetAfterLiving.CluesTarget);
            result.TotalAfterLivingClueYearOnYear = DecimalExtension.CalculateChain(curAfterLivingClue, AfterLivingClueYearOnYear);
            result.TotalAfterLivingClueChainRatio = DecimalExtension.CalculateChain(curAfterLivingClue, AfterLivingClueChainRatio);
            #endregion

            #region 总线索

            var curClue = shoppingCartRegistionData.Count();
            var ClueYearOnYear = shoppingCartRegistionYearOnYear.Count();
            var ClueChainRatio = shoppingCartRegistionChain.Count();
            result.TodayClue = todayshoppingCartRegistionData.Count();
            result.ClueCompleteRate = DecimalExtension.CalculateTargetComplete(curClue, targetAfterLiving.CluesTarget + targetLiving.ConsulationCardTarget + targetBeforeLiving.CluesTarget);
            result.ClueYearOnYear = DecimalExtension.CalculateChain(curClue, ClueYearOnYear);
            result.ClueChainRatio = DecimalExtension.CalculateChain(curClue, ClueChainRatio);
            #endregion
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                shoppingCartRegistionData = shoppingCartRegistionData.Where(x => x.BaseLiveAnchorId == query.keyWord).ToList();
            }
            var dateList = shoppingCartRegistionData.GroupBy(x => x.RecordDate.Day).Select(x => new OerationTotalAchievementBrokenLineListDto
            {
                Time = x.Key,
                TotalCustomerPerformance = x.Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore).Count(),
                NewCustomerPerformance = x.Where(e => e.BelongChannel == (int)BelongChannel.Living).Count(),
                OldCustomerPerformance = x.Where(e => e.BelongChannel == (int)BelongChannel.LiveAfter).Count(),
            });
            List<OerationTotalAchievementBrokenLineListDto> GroupList = new List<OerationTotalAchievementBrokenLineListDto>();
            for (int i = 1; i < dateSchedule.Key + 1; i++)
            {
                OerationTotalAchievementBrokenLineListDto item = new OerationTotalAchievementBrokenLineListDto();
                item.Time = i;
                item.TotalCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.TotalCustomerPerformance).SingleOrDefault();
                item.NewCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.NewCustomerPerformance).SingleOrDefault();
                item.OldCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.OldCustomerPerformance).SingleOrDefault();

                GroupList.Add(item);
            }

            result.LivingClueBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = e.NewCustomerPerformance }).OrderBy(e => Convert.ToInt32(e.date)).ToList();
            result.AfterLivingClueBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = e.OldCustomerPerformance }).OrderBy(e => Convert.ToInt32(e.date)).ToList();
            result.BeforeLivingClueBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = e.TotalCustomerPerformance }).OrderBy(e => Convert.ToInt32(e.date)).ToList();

            if (query.startDate.Value.Year == query.endDate.Value.Year && query.startDate.Value.Month == query.endDate.Value.Month)
            {
                result.TotalBeforeLivingClue = curBeforeLivingClues;
                result.TotalLivingClue = curLivingClue;
                result.TotalAfterLivingClue = curAfterLivingClue;
                result.TotalClue = curClue;
            }
            else
            {
                //非本月数据总业绩取累计数据
                var sumShoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(query.startDate.Value, query.endDate.Value, query.keyWord);
                result.TotalBeforeLivingClue = sumShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).Count();
                result.TotalLivingClue = sumShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.Living).Count();
                result.TotalAfterLivingClue = sumShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).Count();
                result.TotalClue = sumShoppingCartRegistionData.Count();
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取流量分析--分组
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<OperationBoardContentPlatFormDataDto> GetFlowRateByContentPlatFormCompareDataAsync(QueryOperationDataDto query)
        {
            OperationBoardContentPlatFormDataDto result = new OperationBoardContentPlatFormDataDto();

            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);

            //总线索
            var totalShoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(query.startDate.Value, query.endDate.Value, query.keyWord);
            //var groupDaoDaoShoppingCartRegistionData = totalShoppingCartRegistionData.Where(x => x.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c");
            //var curGroupDaoDaoFlowRate = groupDaoDaoShoppingCartRegistionData.Count();
            //var groupJiNaShoppingCartRegistionData = totalShoppingCartRegistionData.Where(x => x.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b");
            //var curGroupJiNaFlowRate = groupJiNaShoppingCartRegistionData.Count();

            var curTotalFlowRate = totalShoppingCartRegistionData.Count();

            ////分组线索分析
            //result.TotalFlowRate = curTotalFlowRate;
            //result.GroupDaoDaoFlowRate = curGroupDaoDaoFlowRate;
            //result.GroupJiNaFlowRate = curGroupJiNaFlowRate;

            #region【平台线索】


            #region 总线索
            OperationBoardContentPlatFormDataDetailsDto totalFlowRateData = new OperationBoardContentPlatFormDataDetailsDto();

            var curTotalDouYinNumber = totalShoppingCartRegistionData.Where(o => o.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").ToList();
            var curTotalXiaoHongShuNumber = totalShoppingCartRegistionData.Where(o => o.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").ToList();
            var curTotalWeChatVideoNumber = totalShoppingCartRegistionData.Where(o => o.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").ToList();
            var curTotalPrivateDomainNumber = totalShoppingCartRegistionData.Where(o => o.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").ToList();
            totalFlowRateData.DouYinNumber = curTotalDouYinNumber.Count();
            totalFlowRateData.DouYinRate = DecimalExtension.CalculateTargetComplete(curTotalDouYinNumber.Count(), curTotalFlowRate);
            totalFlowRateData.XiaoHongShuNumber = curTotalXiaoHongShuNumber.Count();
            totalFlowRateData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curTotalXiaoHongShuNumber.Count(), curTotalFlowRate);
            totalFlowRateData.VideoNumberNumber = curTotalWeChatVideoNumber.Count();
            totalFlowRateData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curTotalWeChatVideoNumber.Count(), curTotalFlowRate);
            totalFlowRateData.PrivateDataNumber = curTotalPrivateDomainNumber.Count();
            totalFlowRateData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curTotalPrivateDomainNumber.Count(), curTotalFlowRate);
            totalFlowRateData.TotalFlowRateNumber = curTotalFlowRate;
            result.TotalFlowRateByContentPlatForm = totalFlowRateData;
            #endregion

            #region 刀刀组线索
            //OperationBoardContentPlatFormDataDetailsDto groupDaoDaoFlowRateData = new OperationBoardContentPlatFormDataDetailsDto();

            //var curGroupDaoDaoDouYinNumber = curTotalDouYinNumber.Where(o => o.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c").Count();
            //var curGroupDaoDaoXiaoHongShuNumber = curTotalXiaoHongShuNumber.Where(o => o.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c").Count();
            //var curGroupDaoDaoWeChatVideoNumber = curTotalWeChatVideoNumber.Where(o => o.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c").Count();
            //var curGroupDaoDaoPrivateDomainNumber = curTotalPrivateDomainNumber.Where(o => o.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c").Count();
            //groupDaoDaoFlowRateData.DouYinNumber = curGroupDaoDaoDouYinNumber;
            //groupDaoDaoFlowRateData.DouYinRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoDouYinNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateData.XiaoHongShuNumber = curGroupDaoDaoXiaoHongShuNumber;
            //groupDaoDaoFlowRateData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoXiaoHongShuNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateData.VideoNumberNumber = curGroupDaoDaoWeChatVideoNumber;
            //groupDaoDaoFlowRateData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoWeChatVideoNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateData.PrivateDataNumber = curGroupDaoDaoPrivateDomainNumber;
            //groupDaoDaoFlowRateData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoPrivateDomainNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateData.TotalFlowRateNumber = curGroupDaoDaoFlowRate;

            //result.GroupDaoDaoFlowRateByContentPlatForm = groupDaoDaoFlowRateData;
            #endregion

            #region 吉娜组线索
            //OperationBoardContentPlatFormDataDetailsDto groupJiNaFlowRateData = new OperationBoardContentPlatFormDataDetailsDto();


            //var curGroupJiNaDouYinNumber = curTotalDouYinNumber.Where(o => o.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b").Count();
            //var curGroupJiNaXiaoHongShuNumber = curTotalXiaoHongShuNumber.Where(o => o.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b").Count();
            //var curGroupJiNaWeChatVideoNumber = curTotalWeChatVideoNumber.Where(o => o.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b").Count();
            //var curGroupJiNaPrivateDomainNumber = curTotalPrivateDomainNumber.Where(o => o.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b").Count();
            //groupJiNaFlowRateData.DouYinNumber = curGroupJiNaDouYinNumber;
            //groupJiNaFlowRateData.DouYinRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaDouYinNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateData.XiaoHongShuNumber = curGroupJiNaXiaoHongShuNumber;
            //groupJiNaFlowRateData.XiaoHongShuRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaXiaoHongShuNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateData.VideoNumberNumber = curGroupJiNaWeChatVideoNumber;
            //groupJiNaFlowRateData.VideoNumberRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaWeChatVideoNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateData.PrivateDataNumber = curGroupJiNaPrivateDomainNumber;
            //groupJiNaFlowRateData.PrivateDataRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaPrivateDomainNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateData.TotalFlowRateNumber = curGroupJiNaFlowRate;
            //result.GroupJiNaFlowRateByContentPlatForm = groupJiNaFlowRateData;
            #endregion
            #endregion

            #region【部门线索】


            #region 总线索
            OperationBoardDepartmentDataDto totalFlowRateByDepartmentData = new OperationBoardDepartmentDataDto();

            var curTotalBeforeLivingNumber = totalShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveBefore).ToList();
            var curTotalLivingNumber = totalShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Living).ToList();
            var curTotalAfterLivingNumber = totalShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveAfter).ToList();
            var curTotalOtherNumber = totalShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Other).ToList();
            totalFlowRateByDepartmentData.BeforeLivingNumber = curTotalBeforeLivingNumber.Count();
            totalFlowRateByDepartmentData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curTotalBeforeLivingNumber.Count(), curTotalFlowRate);
            totalFlowRateByDepartmentData.LivingNumber = curTotalLivingNumber.Count();
            totalFlowRateByDepartmentData.LivingRate = DecimalExtension.CalculateTargetComplete(curTotalLivingNumber.Count(), curTotalFlowRate);
            totalFlowRateByDepartmentData.AfterLivingNumber = curTotalAfterLivingNumber.Count();
            totalFlowRateByDepartmentData.AftereLivingRate = DecimalExtension.CalculateTargetComplete(curTotalAfterLivingNumber.Count(), curTotalFlowRate);
            totalFlowRateByDepartmentData.OtherNumber = curTotalOtherNumber.Count();
            totalFlowRateByDepartmentData.OtherRate = DecimalExtension.CalculateTargetComplete(curTotalOtherNumber.Count(), curTotalFlowRate);
            totalFlowRateByDepartmentData.TotalFlowRateNumber = curTotalFlowRate;
            result.TotalFlowRateByDepartment = totalFlowRateByDepartmentData;
            #endregion

            #region 刀刀组线索
            //OperationBoardDepartmentDataDto groupDaoDaoFlowRateByDepartmentData = new OperationBoardDepartmentDataDto();

            //var curGroupDaoDaoBeforeLivingNumber = groupDaoDaoShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            //var curGroupDaoDaoLivingNumber = groupDaoDaoShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Living).Count();
            //var curGroupDaoDaoAfterLivingNumber = groupDaoDaoShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveAfter).Count();
            //var curGroupDaoDaoOtherNumber = groupDaoDaoShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Other).Count();
            //groupDaoDaoFlowRateByDepartmentData.BeforeLivingNumber = curGroupDaoDaoBeforeLivingNumber;
            //groupDaoDaoFlowRateByDepartmentData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoBeforeLivingNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateByDepartmentData.LivingNumber = curGroupDaoDaoLivingNumber;
            //groupDaoDaoFlowRateByDepartmentData.LivingRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoLivingNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateByDepartmentData.AfterLivingNumber = curGroupDaoDaoAfterLivingNumber;
            //groupDaoDaoFlowRateByDepartmentData.AftereLivingRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoAfterLivingNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateByDepartmentData.OtherNumber = curGroupDaoDaoOtherNumber;
            //groupDaoDaoFlowRateByDepartmentData.OtherRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoOtherNumber, curGroupDaoDaoFlowRate);
            //groupDaoDaoFlowRateByDepartmentData.TotalFlowRateNumber = curGroupDaoDaoFlowRate;

            //result.GroupDaoDaoFlowRateByDepartment = groupDaoDaoFlowRateByDepartmentData;
            #endregion

            #region 吉娜组线索
            //OperationBoardDepartmentDataDto groupJiNaFlowRateByDepartmentData = new OperationBoardDepartmentDataDto();

            //var curGroupJiNaBeforeLivingNumber = groupJiNaShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            //var curGroupJiNaLivingNumber = groupJiNaShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Living).Count();
            //var curGroupJiNaAfterLivingNumber = groupJiNaShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.LiveAfter).Count();
            //var curGroupJiNaOtherNumber = groupJiNaShoppingCartRegistionData.Where(o => o.BelongChannel == (int)BelongChannel.Other).Count();
            //groupJiNaFlowRateByDepartmentData.BeforeLivingNumber = curGroupJiNaBeforeLivingNumber;
            //groupJiNaFlowRateByDepartmentData.BeforeLivingRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaBeforeLivingNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateByDepartmentData.LivingNumber = curGroupJiNaLivingNumber;
            //groupJiNaFlowRateByDepartmentData.LivingRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaLivingNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateByDepartmentData.AfterLivingNumber = curGroupJiNaAfterLivingNumber;
            //groupJiNaFlowRateByDepartmentData.AftereLivingRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaAfterLivingNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateByDepartmentData.OtherNumber = curGroupJiNaOtherNumber;
            //groupJiNaFlowRateByDepartmentData.OtherRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaOtherNumber, curGroupJiNaFlowRate);
            //groupJiNaFlowRateByDepartmentData.TotalFlowRateNumber = curGroupJiNaFlowRate;

            //result.GroupJiNaFlowRateByDepartment = groupJiNaFlowRateByDepartmentData;
            #endregion
            #endregion

            #region【有效/潜在线索】


            #region 总线索
            OperationBoardIsEffictiveDataDto totalFlowRateByIsEffictiveData = new OperationBoardIsEffictiveDataDto();

            var curEffictiveNumber = totalShoppingCartRegistionData.Where(o => o.AddPrice > 0).ToList();
            var curNotEffictiveNumber = totalShoppingCartRegistionData.Where(o => o.AddPrice == 0).ToList();
            totalFlowRateByIsEffictiveData.EffictiveNumber = curEffictiveNumber.Count();
            totalFlowRateByIsEffictiveData.EffictiveRate = DecimalExtension.CalculateTargetComplete(curEffictiveNumber.Count(), curTotalFlowRate);
            totalFlowRateByIsEffictiveData.NotEffictiveNumber = curNotEffictiveNumber.Count();
            totalFlowRateByIsEffictiveData.NotEffictiveRate = DecimalExtension.CalculateTargetComplete(curNotEffictiveNumber.Count(), curTotalFlowRate);
            totalFlowRateByIsEffictiveData.TotalFlowRateNumber = curTotalFlowRate;
            result.TotalFlowRateByIsEffictive = totalFlowRateByIsEffictiveData;
            #endregion

            #region 刀刀组线索
            //OperationBoardIsEffictiveDataDto groupDaoDaoFlowRateByIsEffictiveData = new OperationBoardIsEffictiveDataDto();

            //var curGroupDaoDaoEffictiveNumber = curEffictiveNumber.Where(o => o.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b").Count();
            //var curGroupDaoDaoNotEffictiveNumber = curNotEffictiveNumber.Where(o => o.BaseLiveAnchorId == "af69dcf5-f749-41ea-8b50-fe685facdd8b").Count();
            //var totalDaoDaoIsEffectiveNumber = curGroupDaoDaoEffictiveNumber + curGroupDaoDaoNotEffictiveNumber;
            //groupDaoDaoFlowRateByIsEffictiveData.EffictiveNumber = curGroupDaoDaoEffictiveNumber;
            //groupDaoDaoFlowRateByIsEffictiveData.EffictiveRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoEffictiveNumber, totalDaoDaoIsEffectiveNumber);
            //groupDaoDaoFlowRateByIsEffictiveData.NotEffictiveNumber = curGroupDaoDaoNotEffictiveNumber;
            //groupDaoDaoFlowRateByIsEffictiveData.NotEffictiveRate = DecimalExtension.CalculateTargetComplete(curGroupDaoDaoNotEffictiveNumber, totalDaoDaoIsEffectiveNumber);
            //groupDaoDaoFlowRateByIsEffictiveData.TotalFlowRateNumber = totalDaoDaoIsEffectiveNumber;

            //result.GroupDaoDaoFlowRateByIsEffictive = groupDaoDaoFlowRateByIsEffictiveData;
            #endregion

            #region 吉娜组线索
            //OperationBoardIsEffictiveDataDto groupJiNaFlowRateByIsEffictiveData = new OperationBoardIsEffictiveDataDto();

            //var curGroupJiNaEffictiveNumber = curEffictiveNumber.Where(o => o.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c").Count();
            //var curGroupJiNaNotEffictiveNumber = curNotEffictiveNumber.Where(o => o.BaseLiveAnchorId == "f0a77257-c905-4719-95c4-ad2c4f33855c").Count();
            //var totalJiNaIsEffectiveNumber = curGroupJiNaEffictiveNumber + curGroupJiNaNotEffictiveNumber;
            //groupJiNaFlowRateByIsEffictiveData.EffictiveNumber = curGroupJiNaEffictiveNumber;
            //groupJiNaFlowRateByIsEffictiveData.EffictiveRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaEffictiveNumber, totalJiNaIsEffectiveNumber);
            //groupJiNaFlowRateByIsEffictiveData.NotEffictiveNumber = curGroupJiNaNotEffictiveNumber;
            //groupJiNaFlowRateByIsEffictiveData.NotEffictiveRate = DecimalExtension.CalculateTargetComplete(curGroupJiNaNotEffictiveNumber, totalJiNaIsEffectiveNumber);
            //groupJiNaFlowRateByIsEffictiveData.TotalFlowRateNumber = totalJiNaIsEffectiveNumber;


            //result.GroupJiNaFlowRateByIsEffictive = groupJiNaFlowRateByIsEffictiveData;
            #endregion
            #endregion

            return result;
        }

        /// <summary>
        /// 根据条件获取助理与机构获客对比情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<CustomerFlowRateDataListDto> GetCustomerFlowRateByEmployeeAndHospitalAsync(QueryCustomerFlowRateWithEmployeeAndHospitalDto query)
        {
            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);
            var shoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(sequentialDate.StartDate, sequentialDate.EndDate, query.keyWord);
            var sendInfo = shoppingCartRegistionData.ToList();
            //所有派单手机号
            var totalSendPhoneList = await _dalContentPlatformOrderSend.GetAll()
                .Where(e => e.IsMainHospital == true && e.SendDate >= sequentialDate.StartDate && e.SendDate < sequentialDate.EndDate)
                .Select(e => new KeyValuePair<int?, string>
                (
                    e.ContentPlatformOrder.IsSupportOrder ? e.ContentPlatformOrder.SupportEmpId : e.ContentPlatformOrder.BelongEmpId,
                    e.ContentPlatformOrder.Phone
                )).Distinct().ToListAsync();
            //当月派单手机号
            var currentSendPhoneList = sendInfo.Select(e => new KeyValuePair<int?, string>
            (
                e.AssignEmpId,
                e.Phone
            )).Where(e => totalSendPhoneList.Select(e => e.Value).Contains(e.Value)).ToList();
            var emp = currentSendPhoneList.Where(e => !totalSendPhoneList.Contains(e)).ToList();
            //历史派单手机号
            var historySendPhoneList = totalSendPhoneList.Where(e => !currentSendPhoneList.Select(e => e.Value).Contains(e.Value)).ToList();
            List<KeyValuePair<int?, string>> queryPhoneList = null;
            if (!query.CurrentMonth && !query.History)
            {
                query.CurrentMonth = true;
                query.History = true;
            }
            if (query.CurrentMonth && query.History)
            {
                queryPhoneList = totalSendPhoneList;
            }
            else
            {
                if (query.CurrentMonth)
                {
                    queryPhoneList = currentSendPhoneList;
                }
                if (query.History)
                {
                    queryPhoneList = historySendPhoneList;
                }
            }


            List<int> LiveAnchorInfo = new List<int>();
            CustomerFlowRateDataListDto result = new CustomerFlowRateDataListDto();
            result.EmployeeFlowRate = new List<CustomerFlowRateDataDto>();
            result.HospitalFlowRate = new List<CustomerFlowRateDataDto>();
            #region 助理业绩（包含行政客服）
            var employeeInfo = await amiyaEmployeeService.GetCustomerServiceByBaseLiveAnchorid("f0a77257-c905-4719-95c4-ad2c4f33855c");
            var employeeInfo2 = await amiyaEmployeeService.GetCustomerServiceByBaseLiveAnchorid("af69dcf5-f749-41ea-8b50-fe685facdd8b");
            foreach (var x in employeeInfo2)
            {
                employeeInfo.Add(x);
            }
            foreach (var empInfo in employeeInfo)
            {
                CustomerFlowRateDataDto customerPerformanceDataDto = new CustomerFlowRateDataDto();
                customerPerformanceDataDto.Name = empInfo.Name;
                customerPerformanceDataDto.DistributeConsulationNum = shoppingCartRegistionData.Where(x => x.AssignEmpId == empInfo.Id).Count();
                var sendList = queryPhoneList.Where(x => x.Key == empInfo.Id).ToList();
                customerPerformanceDataDto.SendOrderNum = sendList.Count();
                var visitList = await contentPlateFormOrderService.GetToHospitalCountDataAsync(sequentialDate.StartDate, sequentialDate.EndDate, sendList.Select(x => x.Value).ToList());
                customerPerformanceDataDto.VisitNum = visitList;

                result.EmployeeFlowRate.Add(customerPerformanceDataDto);
            }
            #endregion


            #region 机构业绩
            var sendOrderHospitalList = await contentPlateFormOrderService.GetDealCountDataByPhoneListAsync(sequentialDate.StartDate, sequentialDate.EndDate, queryPhoneList.Select(x => x.Value).ToList());
            var hospitalIdList = sendOrderHospitalList.Distinct();
            foreach (var hospitalInfo in hospitalIdList)
            {
                CustomerFlowRateDataDto hospitalFlowRateDto = new CustomerFlowRateDataDto();
                var lastHospitalId = hospitalInfo;

                var hospital = await hospitalInfoService.GetByIdAsync(lastHospitalId);
                hospitalFlowRateDto.Name = hospital.Name;
                hospitalFlowRateDto.SendOrderNum = sendOrderHospitalList.Where(x => x == lastHospitalId).Count();
                List<int> hospitalIds = new List<int>();
                hospitalIds.Add(lastHospitalId);
                var toHospitalData = await contentPlatFormOrderDealInfoService.GeVisitAndDealNumByHospitalIdAndPhoneListAsync(hospitalIds, sequentialDate.StartDate, sequentialDate.EndDate, queryPhoneList.Select(x => x.Value).ToList());
                hospitalFlowRateDto.VisitNum = toHospitalData.Where(x => x.IsToHospital == true).Count();
                hospitalFlowRateDto.NewCustomerDealNum = toHospitalData.Where(x => x.IsDeal == true).Count();
                result.HospitalFlowRate.Add(hospitalFlowRateDto);
            }
            #endregion
            result.EmployeeFlowRate = result.EmployeeFlowRate.OrderByDescending(x => x.DistributeConsulationNum).Take(15).ToList();
            result.HospitalFlowRate = result.HospitalFlowRate.OrderByDescending(x => x.SendOrderNum).Take(15).ToList();
            return result;
        }

        /// <summary>
        /// 根据条件获取流量分析-部门
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<GetFlowRateByContentPlatformDataDto> GetFlowRateByContentPlatformAsync(QueryOperationDataDto query)
        {
            GetFlowRateByContentPlatformDataDto result = new GetFlowRateByContentPlatformDataDto();
            result.DouYinFolwRateAnalize = new List<BaseKeyValueAndPercentDto>();
            result.VideoNumberFolwRateAnalize = new List<BaseKeyValueAndPercentDto>();
            result.XiaoHongShuFolwRateAnalize = new List<BaseKeyValueAndPercentDto>();
            result.PrivateDataFolwRateAnalize = new List<BaseKeyValueAndPercentDto>();
            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);
            OperationBoardContentPlatFormDataDetailsDto totalFlowRateData = new OperationBoardContentPlatFormDataDetailsDto();
            //总线索
            var totalShoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(sequentialDate.StartDate, sequentialDate.EndDate, query.keyWord);

            //部门拆分
            var beforeLivingData = totalShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.LiveBefore).ToList();
            var livingData = totalShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.Living).ToList();
            var afterLivingData = totalShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.LiveAfter).ToList();
            var otherLivingData = totalShoppingCartRegistionData.Where(x => x.BelongChannel == (int)BelongChannel.Other).ToList();

            string beforeLivingName = "直播前";
            string livingName = "直播中";
            string afterLivingName = "直播后";
            string otherName = "其他";

            #region【抖音流量分析】
            BaseKeyValueAndPercentDto beforeLivingTikTokBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            beforeLivingTikTokBaseKeyValueAndPercentDto.Value = beforeLivingName;
            beforeLivingTikTokBaseKeyValueAndPercentDto.Rate = beforeLivingData.Where(o => o.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count();
            result.DouYinFolwRateAnalize.Add(beforeLivingTikTokBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto LivingTikTokBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            LivingTikTokBaseKeyValueAndPercentDto.Value = livingName;
            LivingTikTokBaseKeyValueAndPercentDto.Rate = livingData.Where(o => o.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count();
            result.DouYinFolwRateAnalize.Add(LivingTikTokBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto afterLivingTikTokBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            afterLivingTikTokBaseKeyValueAndPercentDto.Value = afterLivingName;
            afterLivingTikTokBaseKeyValueAndPercentDto.Rate = afterLivingData.Where(o => o.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count();
            result.DouYinFolwRateAnalize.Add(afterLivingTikTokBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto otherTikTokBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            otherTikTokBaseKeyValueAndPercentDto.Value = otherName;
            otherTikTokBaseKeyValueAndPercentDto.Rate = otherLivingData.Where(o => o.ContentPlatFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count();
            result.DouYinFolwRateAnalize.Add(otherTikTokBaseKeyValueAndPercentDto);
            #endregion

            #region【视频号流量分析】
            BaseKeyValueAndPercentDto beforeLivingWechatVideoBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            beforeLivingWechatVideoBaseKeyValueAndPercentDto.Value = beforeLivingName;
            beforeLivingWechatVideoBaseKeyValueAndPercentDto.Rate = beforeLivingData.Where(o => o.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count();
            result.VideoNumberFolwRateAnalize.Add(beforeLivingWechatVideoBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto LivingWechatVideoBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            LivingWechatVideoBaseKeyValueAndPercentDto.Value = livingName;
            LivingWechatVideoBaseKeyValueAndPercentDto.Rate = livingData.Where(o => o.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count();
            result.VideoNumberFolwRateAnalize.Add(LivingWechatVideoBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto afterLivingWechatVideoBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            afterLivingWechatVideoBaseKeyValueAndPercentDto.Value = afterLivingName;
            afterLivingWechatVideoBaseKeyValueAndPercentDto.Rate = afterLivingData.Where(o => o.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count();
            result.VideoNumberFolwRateAnalize.Add(afterLivingWechatVideoBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto otherWechatVideoBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            otherWechatVideoBaseKeyValueAndPercentDto.Value = otherName;
            otherWechatVideoBaseKeyValueAndPercentDto.Rate = otherLivingData.Where(o => o.ContentPlatFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count();
            result.VideoNumberFolwRateAnalize.Add(otherWechatVideoBaseKeyValueAndPercentDto);
            #endregion

            #region【小红书流量分析】
            BaseKeyValueAndPercentDto beforeLivingXiaoHongShuBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            beforeLivingXiaoHongShuBaseKeyValueAndPercentDto.Value = beforeLivingName;
            beforeLivingXiaoHongShuBaseKeyValueAndPercentDto.Rate = beforeLivingData.Where(o => o.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Count();
            result.XiaoHongShuFolwRateAnalize.Add(beforeLivingXiaoHongShuBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto LivingXiaoHongShuBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            LivingXiaoHongShuBaseKeyValueAndPercentDto.Value = livingName;
            LivingXiaoHongShuBaseKeyValueAndPercentDto.Rate = livingData.Where(o => o.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Count();
            result.XiaoHongShuFolwRateAnalize.Add(LivingXiaoHongShuBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto afterLivingXiaoHongShuBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            afterLivingXiaoHongShuBaseKeyValueAndPercentDto.Value = afterLivingName;
            afterLivingXiaoHongShuBaseKeyValueAndPercentDto.Rate = afterLivingData.Where(o => o.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Count();
            result.XiaoHongShuFolwRateAnalize.Add(afterLivingXiaoHongShuBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto otherXiaoHongShuBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            otherXiaoHongShuBaseKeyValueAndPercentDto.Value = otherName;
            otherXiaoHongShuBaseKeyValueAndPercentDto.Rate = otherLivingData.Where(o => o.ContentPlatFormId == "317c03b8-aff9-4961-8392-fc44d04b1725").Count();
            result.XiaoHongShuFolwRateAnalize.Add(otherXiaoHongShuBaseKeyValueAndPercentDto);
            #endregion

            #region【私域流量分析】
            BaseKeyValueAndPercentDto beforeLivingPrivateDomainBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            beforeLivingPrivateDomainBaseKeyValueAndPercentDto.Value = beforeLivingName;
            beforeLivingPrivateDomainBaseKeyValueAndPercentDto.Rate = beforeLivingData.Where(o => o.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Count();
            result.PrivateDataFolwRateAnalize.Add(beforeLivingPrivateDomainBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto LivingPrivateDomainBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            LivingPrivateDomainBaseKeyValueAndPercentDto.Value = livingName;
            LivingPrivateDomainBaseKeyValueAndPercentDto.Rate = livingData.Where(o => o.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Count();
            result.PrivateDataFolwRateAnalize.Add(LivingPrivateDomainBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto afterLivingPrivateDomainBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            afterLivingPrivateDomainBaseKeyValueAndPercentDto.Value = afterLivingName;
            afterLivingPrivateDomainBaseKeyValueAndPercentDto.Rate = afterLivingData.Where(o => o.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Count();
            result.PrivateDataFolwRateAnalize.Add(afterLivingPrivateDomainBaseKeyValueAndPercentDto);

            BaseKeyValueAndPercentDto otherPrivateDomainBaseKeyValueAndPercentDto = new BaseKeyValueAndPercentDto();
            otherPrivateDomainBaseKeyValueAndPercentDto.Value = otherName;
            otherPrivateDomainBaseKeyValueAndPercentDto.Rate = otherLivingData.Where(o => o.ContentPlatFormId == "22a0b287-232d-4373-a9dd-c372aaae57dc").Count();
            result.PrivateDataFolwRateAnalize.Add(otherPrivateDomainBaseKeyValueAndPercentDto);
            #endregion

            return result;
        }



        /// <summary>
        /// 根据条件获取流量分析-部门-详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<GetFlowRateDetailsByContentPlatformDataDto> GetFlowRateDetailsByContentPlatformAsync(QueryOperationDataDto query)
        {
            GetFlowRateDetailsByContentPlatformDataDto result = new GetFlowRateDetailsByContentPlatformDataDto();
            result.FolwRateDetailsAnalize = new List<BaseKeyValueAndPercentDto>();
            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);
            OperationBoardContentPlatFormDataDetailsDto totalFlowRateData = new OperationBoardContentPlatFormDataDetailsDto();
            //总线索
            var totalShoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDate(sequentialDate.StartDate, sequentialDate.EndDate, query.keyWord);
            totalShoppingCartRegistionData = totalShoppingCartRegistionData.Where(x => x.ContentPlatFormId == query.keyWord).ToList();
            var groupData = totalShoppingCartRegistionData.GroupBy(x => x.Source).Select(e =>
            {
                BaseKeyValueAndPercentDto data = new BaseKeyValueAndPercentDto();
                data.Value = ServiceClass.GetTiktokCustomerSourceText(e.Key);
                data.Rate = e.Count();
                return data;
            }).OrderByDescending(x => x.Rate).ToList();
            result.FolwRateDetailsAnalize = groupData;
            return result;
        }
        #endregion

        #region 【转化】
        /// <summary>
        /// 获取流量转化和客户转化情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<FlowTransFormDataDto>> GetFlowTransFormDataAsync(QueryTransformDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var liveAnchorIds = new List<string>();
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            if (string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                liveAnchorIds = nameList.Where(e => e.LiveAnchorName.Contains("刀刀") || e.LiveAnchorName.Contains("吉娜") || e.LiveAnchorName.Contains("璐璐")).Select(e => e.Id).ToList();
            }
            else
            {
                liveAnchorIds = new List<string>() { query.BaseLiveAnchorId };
            }

            query.ContentPlatFormIds = GetContentPlatformIdList(query);
            List<FlowTransFormDataDto> dataList = new List<FlowTransFormDataDto>();
            foreach (var liveanchorId in liveAnchorIds)
            {
                var liveanchorName = nameList.Where(e => e.Id == liveanchorId).Select(e => e.LiveAnchorName).FirstOrDefault();
                var groupBaseData = await shoppingCartRegistrationService.GetFlowAndCustomerTransformDataAsync(selectDate.StartDate, selectDate.EndDate, liveanchorId, query.ContentPlatFormIds);
                FlowTransFormDataDto groupData = new FlowTransFormDataDto();
                groupData.GroupName = $"{liveanchorName}组";
                groupData.ClueCount = groupBaseData.ClueCount;
                groupData.SendOrderCount = groupBaseData.SendOrderCount;
                groupData.DistributeConsulationNum = groupBaseData.TotalCount;
                groupData.ClueEffectiveRate = DecimalExtension.CalculateTargetComplete(groupData.DistributeConsulationNum, groupData.ClueCount).Value;
                groupData.AddWechatCount = groupBaseData.AddWechatCount;
                groupData.AddWechatRate = DecimalExtension.CalculateTargetComplete(groupBaseData.AddWechatCount, groupData.DistributeConsulationNum).Value;
                groupData.SendOrderRate = DecimalExtension.CalculateTargetComplete(groupBaseData.SendOrderCount, groupBaseData.AddWechatCount).Value;
                groupData.ToHospitalCount = groupBaseData.ToHospitalCount;
                groupData.ToHospitalRate = DecimalExtension.CalculateTargetComplete(groupBaseData.ToHospitalCount, groupBaseData.SendOrderCount).Value;
                groupData.DealCount = groupBaseData.NewCustomerDealCount;
                groupData.NewCustomerDealCount = groupBaseData.NewCustomerDealCount;
                groupData.OldCustomerDealCount = groupBaseData.OldCustomerDealCount;
                groupData.DealRate = DecimalExtension.CalculateTargetComplete(groupData.DealCount, groupBaseData.ToHospitalCount).Value;
                groupData.NewCustomerPerformance = groupBaseData.NewCustomerTotalPerformance;
                groupData.OldCustomerPerformance = groupBaseData.OldCustomerTotalPerformance;
                groupData.OldCustomerUnitPrice = DecimalExtension.Division(groupData.OldCustomerPerformance, groupBaseData.OldCustomerDealCount).Value;
                groupData.NewCustomerUnitPrice = DecimalExtension.Division(groupData.NewCustomerPerformance, groupBaseData.NewCustomerDealCount).Value;
                groupData.CustomerUnitPrice = DecimalExtension.Division(groupData.NewCustomerPerformance + groupData.OldCustomerPerformance, groupBaseData.OldCustomerDealCount + groupBaseData.NewCustomerDealCount).Value;
                groupData.NewAndOldCustomerRate = DecimalExtension.CalculateAccounted(groupData.NewCustomerPerformance, groupData.OldCustomerPerformance);
                dataList.Add(groupData);
            }
            foreach (var item in dataList)
            {
                item.Rate = DecimalExtension.CalculateTargetComplete(item.NewCustomerPerformance + item.OldCustomerPerformance, dataList.Sum(e => e.NewCustomerPerformance) + dataList.Sum(e => e.OldCustomerPerformance)).Value;
            }
            if (string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                FlowTransFormDataDto data = new FlowTransFormDataDto();
                data.GroupName = "总计";
                data.ClueCount = dataList.Sum(e => e.ClueCount);
                data.SendOrderCount = dataList.Sum(e => e.SendOrderCount);
                data.DistributeConsulationNum = dataList.Sum(e => e.DistributeConsulationNum);
                data.ClueEffectiveRate = DecimalExtension.CalculateTargetComplete(data.DistributeConsulationNum, data.ClueCount).Value;
                data.AddWechatCount = dataList.Sum(e => e.AddWechatCount);
                data.AddWechatRate = DecimalExtension.CalculateTargetComplete(data.AddWechatCount, data.DistributeConsulationNum).Value;
                data.SendOrderRate = DecimalExtension.CalculateTargetComplete(data.SendOrderCount, data.AddWechatCount).Value;
                data.ToHospitalCount = dataList.Sum(e => e.ToHospitalCount);
                data.ToHospitalRate = DecimalExtension.CalculateTargetComplete(data.ToHospitalCount, data.SendOrderCount).Value;
                data.DealCount = dataList.Sum(e => e.DealCount);
                data.DealRate = DecimalExtension.CalculateTargetComplete(data.DealCount, data.ToHospitalCount).Value;
                data.NewCustomerPerformance = dataList.Sum(e => e.NewCustomerPerformance);
                data.OldCustomerPerformance = dataList.Sum(e => e.OldCustomerPerformance);
                data.OldCustomerUnitPrice = DecimalExtension.Division(data.OldCustomerPerformance, dataList.Sum(e => e.OldCustomerDealCount)).Value;
                data.NewCustomerUnitPrice = DecimalExtension.Division(data.NewCustomerPerformance, dataList.Sum(e => e.NewCustomerDealCount)).Value;
                data.CustomerUnitPrice = DecimalExtension.Division(data.NewCustomerPerformance + data.OldCustomerPerformance, data.DealCount).Value;
                data.NewAndOldCustomerRate = DecimalExtension.CalculateAccounted(data.NewCustomerPerformance, data.OldCustomerPerformance);
                data.Rate = 100;
                dataList.Add(data);
            }
            return dataList;
        }
        /// <summary>
        /// 获取流量转化和客户转化情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<FlowTransFormDataDto>> GetAssistantFlowTransFormDataAsync(QueryTransformDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                nameList = nameList.Where(e => e.Id == query.BaseLiveAnchorId).ToList();
            }
            var assistantNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(nameList.Select(e => e.Id).ToList());
            query.ContentPlatFormIds = GetContentPlatformIdList(query);
            var baseData = await shoppingCartRegistrationService.GetAssitantFlowAndCustomerTransformDataAsync(selectDate.StartDate, selectDate.EndDate, query.IsCurrentMonth, query.BaseLiveAnchorId, query.ContentPlatFormIds);
            var list = baseData.GroupBy(e => e.EmpId).Select(e =>
            {
                var name = assistantNameList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其他";
                FlowTransFormDataDto data = new FlowTransFormDataDto();
                data.GroupName = name;
                data.SendOrderCount = e.Sum(e => e.SendOrderCount);
                data.DistributeConsulationNum = e.Sum(e => e.TotalCount);
                data.AddWechatCount = e.Sum(e => e.AddWechatCount);
                data.AddWechatRate = DecimalExtension.CalculateTargetComplete(data.AddWechatCount, data.DistributeConsulationNum).Value;
                data.SendOrderRate = DecimalExtension.CalculateTargetComplete(data.SendOrderCount, data.AddWechatCount).Value;
                data.ToHospitalCount = e.Sum(e => e.ToHospitalCount);
                data.ToHospitalRate = DecimalExtension.CalculateTargetComplete(data.ToHospitalCount, data.SendOrderCount).Value;
                data.NewCustomerDealCount = e.Sum(e => e.NewCustomerDealCount);
                data.OldCustomerDealCount = e.Sum(e => e.OldCustomerDealCount);
                //data.DealCount = data.NewCustomerDealCount + data.OldCustomerDealCount;
                data.DealCount = data.NewCustomerDealCount;
                data.DealRate = DecimalExtension.CalculateTargetComplete(data.DealCount, data.ToHospitalCount).Value;
                data.NewCustomerPerformance = e.Sum(e => e.NewCustomerTotalPerformance);
                data.OldCustomerPerformance = e.Sum(e => e.OldCustomerTotalPerformance);
                data.OldCustomerUnitPrice = DecimalExtension.Division(data.OldCustomerPerformance, data.OldCustomerDealCount).Value;
                data.NewCustomerUnitPrice = DecimalExtension.Division(data.NewCustomerPerformance, data.NewCustomerDealCount).Value;
                data.CustomerUnitPrice = DecimalExtension.Division(data.NewCustomerPerformance + data.OldCustomerPerformance, data.DealCount).Value;
                data.NewAndOldCustomerRate = DecimalExtension.CalculateAccounted(data.NewCustomerPerformance, data.OldCustomerPerformance);
                return data;
            }).ToList();
            FlowTransFormDataDto otherData = new FlowTransFormDataDto();
            otherData.GroupName = "其他";
            otherData.SendOrderCount = list.Where(e => e.GroupName == "其他").Sum(e => e.SendOrderCount);
            otherData.DistributeConsulationNum = list.Where(e => e.GroupName == "其他").Sum(e => e.DistributeConsulationNum);
            otherData.AddWechatCount = list.Where(e => e.GroupName == "其他").Sum(e => e.AddWechatCount);
            otherData.AddWechatRate = DecimalExtension.CalculateTargetComplete(otherData.AddWechatCount, otherData.DistributeConsulationNum).Value;
            otherData.SendOrderRate = DecimalExtension.CalculateTargetComplete(otherData.SendOrderCount, otherData.AddWechatCount).Value;
            otherData.ToHospitalCount = list.Where(e => e.GroupName == "其他").Sum(e => e.ToHospitalCount);
            otherData.ToHospitalRate = DecimalExtension.CalculateTargetComplete(otherData.ToHospitalCount, otherData.SendOrderCount).Value;
            otherData.NewCustomerDealCount = list.Where(e => e.GroupName == "其他").Sum(e => e.NewCustomerDealCount);
            otherData.OldCustomerDealCount = list.Where(e => e.GroupName == "其他").Sum(e => e.OldCustomerDealCount);
            otherData.DealCount = otherData.NewCustomerDealCount;
            otherData.DealRate = DecimalExtension.CalculateTargetComplete(otherData.DealCount, otherData.ToHospitalCount).Value;
            otherData.NewCustomerPerformance = list.Where(e => e.GroupName == "其他").Sum(e => e.NewCustomerPerformance);
            otherData.OldCustomerPerformance = list.Where(e => e.GroupName == "其他").Sum(e => e.OldCustomerPerformance);
            otherData.OldCustomerUnitPrice = DecimalExtension.Division(otherData.OldCustomerPerformance, otherData.OldCustomerDealCount).Value;
            otherData.NewCustomerUnitPrice = DecimalExtension.Division(otherData.NewCustomerPerformance, otherData.NewCustomerDealCount).Value;
            otherData.CustomerUnitPrice = DecimalExtension.Division(otherData.NewCustomerPerformance + otherData.OldCustomerPerformance, otherData.DealCount).Value;
            otherData.NewAndOldCustomerRate = DecimalExtension.CalculateAccounted(otherData.NewCustomerPerformance, otherData.OldCustomerPerformance);
            list.RemoveAll(e => e.GroupName == "其他");
            list.Add(otherData);
            foreach (var item in list)
            {
                item.Rate = DecimalExtension.CalculateTargetComplete(item.NewCustomerPerformance + item.OldCustomerPerformance, list.Sum(e => e.NewCustomerPerformance) + list.Sum(e => e.OldCustomerPerformance)).Value;
            }
            FlowTransFormDataDto data = new FlowTransFormDataDto();
            data.GroupName = "总计";
            data.SendOrderCount = list.Sum(e => e.SendOrderCount);
            data.DistributeConsulationNum = list.Sum(e => e.DistributeConsulationNum);
            data.AddWechatCount = list.Sum(e => e.AddWechatCount);
            data.AddWechatRate = DecimalExtension.CalculateTargetComplete(data.AddWechatCount, data.DistributeConsulationNum).Value;
            data.SendOrderRate = DecimalExtension.CalculateTargetComplete(data.SendOrderCount, data.AddWechatCount).Value;
            data.ToHospitalCount = list.Sum(e => e.ToHospitalCount);
            data.ToHospitalRate = DecimalExtension.CalculateTargetComplete(data.ToHospitalCount, data.SendOrderCount).Value;
            data.DealCount = list.Sum(e => e.DealCount);
            data.OldCustomerDealCount = list.Sum(e => e.OldCustomerDealCount);
            data.DealRate = DecimalExtension.CalculateTargetComplete(data.DealCount, data.ToHospitalCount).Value;
            data.NewCustomerPerformance = list.Sum(e => e.NewCustomerPerformance);
            data.OldCustomerPerformance = list.Sum(e => e.OldCustomerPerformance);
            data.OldCustomerUnitPrice = DecimalExtension.Division(data.OldCustomerPerformance, list.Sum(e => e.OldCustomerDealCount)).Value;
            data.NewCustomerUnitPrice = DecimalExtension.Division(data.NewCustomerPerformance, list.Sum(e => e.NewCustomerDealCount)).Value;
            data.CustomerUnitPrice = DecimalExtension.Division(data.NewCustomerPerformance + data.OldCustomerPerformance, data.DealCount).Value;
            data.NewAndOldCustomerRate = DecimalExtension.CalculateAccounted(data.NewCustomerPerformance, data.OldCustomerPerformance);
            data.Rate = 100;
            var res = list.OrderByDescending(e => e.DistributeConsulationNum).ToList();
            res.Add(data);
            return res;
        }

        /// <summary>
        /// 根据时间获取全国机构运营数据概况
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isCity"></param>
        /// <returns></returns>
        public async Task<List<HospitalPerformanceDto>> GetHospitalPerformanceByDateAsync(QueryHospitalTransformDataDto query)
        {

            List<HospitalPerformanceDto> resultList = new List<HospitalPerformanceDto>();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            query.StartDate = selectDate.StartDate;
            query.EndDate = selectDate.EndDate;
            query.LiveAnchorIds = await GetBaseLiveAnchorIdListAsync(query);
            var contentPlatFormOrderSendList = await contentPlatformOrderSendService.GetTodayOrderSendDataAsync(query);
            foreach (var x in contentPlatFormOrderSendList)
            {

                var isExist = resultList.Where(z => z.HospitalId == x.SendHospitalId).Count();
                if (isExist > 0)
                {
                    continue;
                }
                HospitalPerformanceDto hospitalPerformanceDto = new HospitalPerformanceDto();
                hospitalPerformanceDto.HospitalId = x.SendHospitalId;
                hospitalPerformanceDto.HospitalName = x.SendHospital;
                hospitalPerformanceDto.City = x.City;
                List<int> hospitalIds = new List<int>();
                hospitalIds.Add(x.SendHospitalId);
                query.HospitalId = hospitalIds;
                hospitalPerformanceDto.SendNum = contentPlatFormOrderSendList.Where(z => hospitalIds.Contains(z.SendHospitalId)).Count();
                var contentPlatFormOrderDealInfoList = await contentPlatFormOrderDealInfoService.GetSendPerformanceByHospitalIdAndDateTimeAsync(query);
                hospitalPerformanceDto.VisitNum = contentPlatFormOrderDealInfoList.Count();
                hospitalPerformanceDto.VisitRate = DecimalExtension.CalculateTargetComplete(hospitalPerformanceDto.VisitNum, hospitalPerformanceDto.SendNum).Value;
                var dealInfoList = contentPlatFormOrderDealInfoList.Where(x => x.IsDeal == true && x.DealDate.HasValue == true);
                hospitalPerformanceDto.NewCustomerDealNum = dealInfoList.Where(x => x.IsOldCustomer == false).Count();
                hospitalPerformanceDto.NewCustomerDealRate = DecimalExtension.CalculateTargetComplete(hospitalPerformanceDto.NewCustomerDealNum, hospitalPerformanceDto.VisitNum).Value;
                hospitalPerformanceDto.NewCustomerAchievement = dealInfoList.Where(x => x.IsOldCustomer == false).Sum(x => x.Price);
                hospitalPerformanceDto.NewCustomerUnitPrice = DecimalExtension.Division(hospitalPerformanceDto.NewCustomerAchievement, hospitalPerformanceDto.NewCustomerDealNum).Value;
                hospitalPerformanceDto.OldCustomerDealNum = dealInfoList.Where(x => x.IsOldCustomer == true).Count();
                hospitalPerformanceDto.OldCustomerAchievement = dealInfoList.Where(x => x.IsOldCustomer == true).Sum(x => x.Price);
                hospitalPerformanceDto.OldCustomerUnitPrice = DecimalExtension.Division(hospitalPerformanceDto.OldCustomerAchievement, hospitalPerformanceDto.OldCustomerDealNum).Value;
                hospitalPerformanceDto.TotalAchievement = dealInfoList.Sum(x => x.Price);
                hospitalPerformanceDto.NewOrOldCustomerRate = DecimalExtension.CalculateAccounted(hospitalPerformanceDto.NewCustomerAchievement, hospitalPerformanceDto.OldCustomerAchievement);
                resultList.Add(hospitalPerformanceDto);
            }
            var res = resultList.OrderByDescending(e => e.SendNum).Skip(0).Take(10).ToList();
            var totalPerformance = resultList.Sum(e => e.TotalAchievement);
            foreach (var item in res)
            {
                item.Rate = DecimalExtension.CalculateTargetComplete(item.TotalAchievement, totalPerformance).Value;
            }
            return res;
        }
        #endregion

        #endregion


        #region 公司看板
        /// <summary>
        /// 获取公司看板业绩情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyPerformanceDataDto>> GetCompanyPerformanceDataAsync(QueryAmiyaCompanyOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            query.LiveAnchorIds = nameList.Select(e => e.Id).ToList();
            var liveanchorIds = (await liveAnchorService.GetLiveAnchorListByBaseInfoIdListAsync(query.LiveAnchorIds)).Select(e => e.Id).ToList();
            var targetList = await liveAnchorMonthlyTargetAfterLivingService.GetPerformanceTargetByBaseLiveAnchorIdAsync(selectDate.StartDate.Year, selectDate.StartDate.Month, query.LiveAnchorIds);
            var order = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAsync(selectDate.StartDate, selectDate.EndDate, liveanchorIds);
            var dataList = order.GroupBy(e => e.BaseLiveAnchorId).Select(e =>
            {
                var liveanchorName = nameList.Where(a => a.Id == e.Key).Select(e => e.LiveAnchorName).FirstOrDefault();
                var target = targetList.Where(t => t.BaseLiveAbchorId == e.Key).FirstOrDefault();
                CompanyPerformanceDataDto data = new CompanyPerformanceDataDto();
                data.GroupName = $"{liveanchorName}组";
                data.CurrentMonthNewCustomerPerformance = e.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
                data.NewCustomerPerformanceTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.NewCustomerPerformanceTarget ?? 0m);
                data.NewCustomerPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthNewCustomerPerformance, data.NewCustomerPerformanceTarget ?? 0m);
                data.CurrentMonthOldCustomerPerformance = e.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
                data.OldCustomerTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.OldCustomerPerformanceTarget ?? 0m);
                data.OldCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthOldCustomerPerformance, data.OldCustomerTarget ?? 0m);
                data.TotalPerformance = e.Sum(e => e.Price);
                data.TotalPerformanceTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.TotalPerformanceTarget ?? 0m);
                data.TotalPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(data.TotalPerformance, data.TotalPerformanceTarget);
                return data;
            }).ToList();
            var total = new CompanyPerformanceDataDto();
            total.GroupName = "总计";
            total.CurrentMonthNewCustomerPerformance = dataList.Sum(e => e.CurrentMonthNewCustomerPerformance);
            total.NewCustomerPerformanceTarget = dataList.Sum(e => e.NewCustomerPerformanceTarget);
            total.NewCustomerPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(total.CurrentMonthNewCustomerPerformance, total.NewCustomerPerformanceTarget ?? 0m);
            total.CurrentMonthOldCustomerPerformance = dataList.Sum(e => e.CurrentMonthOldCustomerPerformance);
            total.OldCustomerTarget = dataList.Sum(e => e.OldCustomerTarget);
            total.OldCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(total.CurrentMonthOldCustomerPerformance, total.OldCustomerTarget ?? 0m);
            total.TotalPerformance = dataList.Sum(e => e.TotalPerformance);
            total.TotalPerformanceTarget = dataList.Sum(e => e.TotalPerformanceTarget);
            total.TotalPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(total.TotalPerformance, total.TotalPerformanceTarget);
            dataList.Add(total);
            return dataList;
        }
        /// <summary>
        /// 获取公司看板获客情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyCustomerAcquisitionDataDto>> GetCompanyCustomerAcquisitionDataAsync(QueryAmiyaCompanyOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            query.LiveAnchorIds = nameList.Select(e => e.Id).ToList();
            var livingTarget = await liveAnchorMonthlyTargetLivingService.GetConsulationCardAddTargetByDateAsync(query.StartDate.Value.Year, query.StartDate.Value.Month, query.LiveAnchorIds);
            var dataList = new List<CompanyCustomerAcquisitionDataDto>();
            foreach (var liveanchor in nameList)
            {
                var assistantTarget = await employeePerformanceTargetService.GetEmployeeTargetByBaseLiveAnchorIdAsync(query.StartDate.Value.Year, query.StartDate.Value.Month, liveanchor.Id);
                var data = await shoppingCartRegistrationService.GetPerformanceByBaseLiveAnchorIdAsync(selectDate.StartDate, selectDate.EndDate, true, liveanchor.Id);
                var liveanchorName = nameList.Where(e => e.Id == liveanchor.Id).Select(e => e.LiveAnchorName).FirstOrDefault();
                CompanyCustomerAcquisitionDataDto dataItem = new CompanyCustomerAcquisitionDataDto();
                dataItem.GroupName = $"{liveanchorName}组-有效业绩";
                dataItem.OrderCard = data.Where(e => e.IsReturnBackPrice == false).Count();
                dataItem.OrderCardTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, livingTarget?.Sum(e => e.ConsulationCardTarget) ?? 0);
                dataItem.OrderCardTargetComplete = DecimalExtension.CalculateTargetComplete(dataItem.OrderCard, dataItem.OrderCardTarget).Value;
                dataItem.RefundCard = data.Where(x => x.IsReturnBackPrice == true).Count();
                dataItem.OrderCardError = 0;
                dataItem.AllocationConsulationTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, assistantTarget?.EffectiveConsulationCardTarget ?? 0);
                dataItem.AllocationConsulation = data.Select(e => e.Phone).Distinct().Count();
                dataItem.AllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(dataItem.AllocationConsulation, dataItem.AllocationConsulationTarget).Value;
                dataItem.AddWechat = data.Where(e => e.IsAddWeChat).Select(e => e.Phone).Distinct().Count();
                dataItem.AddWechatTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, assistantTarget?.EffectiveAddWechatTarget ?? 0);
                dataItem.AddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(dataItem.AddWechat, dataItem.AddWechatTarget).Value;
                dataItem.BaseLiveAnchorId = liveanchor.Id;
                dataItem.IsEffective = true;
                dataItem.LiveAnchorName = liveanchor.LiveAnchorName;
                dataList.Add(dataItem);
            }
            foreach (var liveanchor in nameList)
            {
                var assistantTarget = await employeePerformanceTargetService.GetEmployeeTargetByBaseLiveAnchorIdAsync(query.StartDate.Value.Year, query.StartDate.Value.Month, liveanchor.Id);
                var data = await shoppingCartRegistrationService.GetPerformanceByBaseLiveAnchorIdAsync(selectDate.StartDate, selectDate.EndDate, false, liveanchor.Id);
                var liveanchorName = nameList.Where(e => e.Id == liveanchor.Id).Select(e => e.LiveAnchorName).FirstOrDefault();
                CompanyCustomerAcquisitionDataDto dataItem = new CompanyCustomerAcquisitionDataDto();
                dataItem.GroupName = $"{liveanchorName}组-潜在业绩";
                dataItem.OrderCard = data.Count();
                dataItem.OrderCardTarget = livingTarget?.Sum(e => e.ConsulationCardTarget) ?? 0;
                dataItem.OrderCardTargetComplete = DecimalExtension.CalculateTargetComplete(dataItem.OrderCard, dataItem.OrderCardTarget).Value;
                dataItem.RefundCard = data.Where(x => x.IsReturnBackPrice == true).Count();
                dataItem.OrderCardError = 0;
                dataItem.AllocationConsulationTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, assistantTarget?.PotentialConsulationCardTarget ?? 0);
                dataItem.AllocationConsulation = data.Count();
                dataItem.AllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(dataItem.AllocationConsulation, dataItem.AllocationConsulationTarget).Value;
                dataItem.AddWechat = data.Where(e => e.IsAddWeChat && e.IsReturnBackPrice == false).Count();
                dataItem.AddWechatTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, assistantTarget?.PotentialAddWechatTarget ?? 0);
                dataItem.AddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(dataItem.AddWechat, dataItem.AddWechatTarget).Value;
                dataItem.BaseLiveAnchorId = liveanchor.Id;
                dataItem.IsEffective = false;
                dataItem.LiveAnchorName = liveanchor.LiveAnchorName;
                dataList.Add(dataItem);
            }
            var groupTotalData = dataList.GroupBy(e => e.LiveAnchorName).Select(e => new CompanyCustomerAcquisitionDataDto
            {
                GroupName = $"{e.Key}组",
                OrderCard = e.Sum(e => e.OrderCard),
                OrderCardTarget = e.Sum(e => e.OrderCardTarget),
                OrderCardTargetComplete = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.OrderCard), e.Sum(e => e.OrderCardTarget)).Value,
                RefundCard = e.Sum(e => e.RefundCard),
                OrderCardError = e.Sum(e => e.OrderCardError),
                AllocationConsulation = e.Sum(e => e.AllocationConsulation),
                AllocationConsulationTarget = e.Sum(e => e.AllocationConsulationTarget),
                AllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.AllocationConsulation), e.Sum(e => e.AllocationConsulationTarget)).Value,
                AddWechat = e.Sum(e => e.AddWechat),
                AddWechatTarget = e.Sum(e => e.AddWechatTarget),
                AddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.AddWechat), e.Sum(e => e.AddWechatTarget)).Value
            }).ToList();
            dataList.AddRange(groupTotalData);
            CompanyCustomerAcquisitionDataDto totalData = new CompanyCustomerAcquisitionDataDto();
            totalData.GroupName = "总计";
            totalData.OrderCard = groupTotalData.Sum(e => e.OrderCard);
            totalData.OrderCardTarget = groupTotalData.Sum(e => e.OrderCardTarget);
            totalData.OrderCardTargetComplete = DecimalExtension.CalculateTargetComplete(groupTotalData.Sum(e => e.OrderCard), groupTotalData.Sum(e => e.OrderCardTarget)).Value;
            totalData.RefundCard = groupTotalData.Sum(e => e.RefundCard);
            totalData.OrderCardError = groupTotalData.Sum(e => e.OrderCardError);
            totalData.AllocationConsulation = groupTotalData.Sum(e => e.AllocationConsulation);
            totalData.AllocationConsulationTarget = groupTotalData.Sum(e => e.AllocationConsulationTarget);
            totalData.AllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(groupTotalData.Sum(e => e.AllocationConsulation), groupTotalData.Sum(e => e.AllocationConsulationTarget)).Value;
            totalData.AddWechat = groupTotalData.Sum(e => e.AddWechat);
            totalData.AddWechatTarget = groupTotalData.Sum(e => e.AddWechatTarget);
            totalData.AddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(groupTotalData.Sum(e => e.AddWechat), groupTotalData.Sum(e => e.AddWechatTarget)).Value;
            dataList.Add(totalData);
            return dataList;
        }
        /// <summary>
        /// 获取公司当月新客分诊转化情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<CompanyNewCustomerConversionDataDto>> GetCompanyNewCustomerConversionDataAsync(QueryAmiyaCompanyOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            query.LiveAnchorIds = nameList.Select(e => e.Id).ToList();
            List<CompanyNewCustomerConversionDataDto> dataList = new List<CompanyNewCustomerConversionDataDto>();
            List<CompanyNewCustomerConversionDataDto> groupDataList = new List<CompanyNewCustomerConversionDataDto>();
            int newCustomerCount = 0;
            foreach (var liveanchorId in query.LiveAnchorIds)
            {
                var liveanchorName = nameList.Where(e => e.Id == liveanchorId).Select(e => e.LiveAnchorName).FirstOrDefault();
                var effectiveBaseData = await shoppingCartRegistrationService.GetCurrentMonthNewCustomerConversionDataAsync(selectDate.StartDate, selectDate.EndDate, liveanchorId, true, false);
                var potentialBaseData = await shoppingCartRegistrationService.GetCurrentMonthNewCustomerConversionDataAsync(selectDate.StartDate, selectDate.EndDate, liveanchorId, false, false);

                CompanyNewCustomerConversionDataDto effectiveData = new CompanyNewCustomerConversionDataDto();
                effectiveData.GroupName = $"{liveanchorName}组-有效";
                effectiveData.SendOrderCount = effectiveBaseData.SendOrderCount;
                effectiveData.DistributeConsulationNum = effectiveBaseData.TotalCount;
                effectiveData.AddWechatCount = effectiveBaseData.AddWechatCount;
                effectiveData.AddWechatRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.AddWechatCount, effectiveData.DistributeConsulationNum).Value;
                effectiveData.SendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.SendOrderCount, effectiveData.AddWechatCount).Value;
                effectiveData.ToHospitalCount = effectiveBaseData.ToHospitalCount;
                effectiveData.ToHospitalRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.ToHospitalCount, effectiveData.SendOrderCount).Value;
                effectiveData.DealCount = effectiveBaseData.NewCustomerDealCount;
                effectiveData.DealRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.NewCustomerDealCount, effectiveBaseData.ToHospitalCount).Value;
                effectiveData.Performance = effectiveBaseData.NewCustomerTotalPerformance;
                dataList.Add(effectiveData);

                CompanyNewCustomerConversionDataDto potentialData = new CompanyNewCustomerConversionDataDto();
                potentialData.GroupName = $"{liveanchorName}组-潜在";
                potentialData.SendOrderCount = potentialBaseData.SendOrderCount;
                potentialData.DistributeConsulationNum = potentialBaseData.TotalCount;
                potentialData.AddWechatCount = potentialBaseData.AddWechatCount;
                potentialData.AddWechatRate = DecimalExtension.CalculateTargetComplete(potentialBaseData.AddWechatCount, potentialData.DistributeConsulationNum).Value;
                potentialData.SendOrderRate = DecimalExtension.CalculateTargetComplete(potentialBaseData.SendOrderCount, potentialData.AddWechatCount).Value;
                potentialData.ToHospitalCount = potentialBaseData.ToHospitalCount;
                potentialData.ToHospitalRate = DecimalExtension.CalculateTargetComplete(potentialBaseData.ToHospitalCount, potentialData.SendOrderCount).Value;
                potentialData.DealCount = potentialBaseData.NewCustomerDealCount;
                potentialData.DealRate = DecimalExtension.CalculateTargetComplete(potentialBaseData.NewCustomerDealCount, potentialBaseData.ToHospitalCount).Value;
                potentialData.Performance = potentialBaseData.NewCustomerTotalPerformance;
                dataList.Add(potentialData);

                var totalCount = effectiveData.DistributeConsulationNum + potentialData.DistributeConsulationNum;
                var totalSendCount = effectiveData.SendOrderCount + potentialData.SendOrderCount;
                var totalAddwechatCount = effectiveData.AddWechatCount + potentialData.AddWechatCount;
                var totalTohospitalCount = effectiveData.ToHospitalCount + potentialData.ToHospitalCount;
                var totalDealCount = effectiveData.DealCount + potentialData.DealCount;
                var totalNewCustomerCount = effectiveBaseData.NewCustomerCount + potentialBaseData.NewCustomerCount;
                CompanyNewCustomerConversionDataDto totalData = new CompanyNewCustomerConversionDataDto();
                totalData.GroupName = $"{liveanchorName}组";
                totalData.SendOrderCount = totalSendCount;
                totalData.SendOrderRate = DecimalExtension.CalculateTargetComplete(totalSendCount, totalAddwechatCount).Value;
                totalData.DistributeConsulationNum = totalCount;
                totalData.AddWechatCount = totalAddwechatCount;
                totalData.AddWechatRate = DecimalExtension.CalculateTargetComplete(totalAddwechatCount, totalCount).Value;
                totalData.ToHospitalCount = totalTohospitalCount;
                totalData.ToHospitalRate = DecimalExtension.CalculateTargetComplete(totalTohospitalCount, totalData.SendOrderCount).Value;
                totalData.DealCount = totalDealCount;
                totalData.DealRate = DecimalExtension.CalculateTargetComplete(totalDealCount, totalData.ToHospitalCount).Value;
                totalData.Performance = potentialData.Performance + effectiveData.Performance;
                newCustomerCount += totalNewCustomerCount;
                dataList.Add(totalData);
                groupDataList.Add(totalData);
            }
            CompanyNewCustomerConversionDataDto data = new CompanyNewCustomerConversionDataDto();

            data.GroupName = "总计";
            data.SendOrderCount = groupDataList.Sum(e => e.SendOrderCount);
            data.DistributeConsulationNum = groupDataList.Sum(e => e.DistributeConsulationNum);
            data.AddWechatCount = groupDataList.Sum(e => e.AddWechatCount);
            data.AddWechatRate = DecimalExtension.CalculateTargetComplete(data.AddWechatCount, data.DistributeConsulationNum).Value;
            data.SendOrderRate = DecimalExtension.CalculateTargetComplete(data.SendOrderCount, data.AddWechatCount).Value;
            data.ToHospitalCount = groupDataList.Sum(e => e.ToHospitalCount);
            data.ToHospitalRate = DecimalExtension.CalculateTargetComplete(data.ToHospitalCount, data.SendOrderCount).Value;
            data.DealCount = groupDataList.Sum(e => e.DealCount);
            data.DealRate = DecimalExtension.CalculateTargetComplete(data.DealCount, data.ToHospitalCount).Value;
            data.Performance = groupDataList.Sum(e => e.Performance);
            dataList.Add(data);
            return dataList;
        }
        /// <summary>
        /// 获取公司看板运营情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyOperationsDataDto>> GetCompanyOperationsDataAsync(QueryAmiyaCompanyOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            query.LiveAnchorIds = nameList.Select(e => e.Id).ToList();
            var targetList = await liveAnchorMonthlyTargetAfterLivingService.GetPerformanceTargetByBaseLiveAnchorIdAsync(selectDate.StartDate.Year, selectDate.EndDate.Month, query.LiveAnchorIds);
            List<CompanyOperationsDataDto> dataList = new List<CompanyOperationsDataDto>();
            foreach (var liveanchorId in query.LiveAnchorIds)
            {
                var liveanchorName = nameList.Where(e => e.Id == liveanchorId).Select(e => e.LiveAnchorName).FirstOrDefault();
                var baseOrderPerformance = await contentPlateFormOrderService.GetOrderSendAndDealDataByMonthAndBaseLiveAnchorIdAsync(selectDate.StartDate, selectDate.EndDate, query.IsOldCustomer.Value, liveanchorId);
                var target = targetList.Where(e => e.BaseLiveAbchorId == liveanchorId).FirstOrDefault();
                CompanyOperationsDataDto operationsData = new CompanyOperationsDataDto();
                operationsData.GroupName = $"{liveanchorName}组";
                operationsData.SendOrderTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.SendOrderTarget ?? 0);
                operationsData.SendOrder = baseOrderPerformance.SendOrderNum;
                operationsData.SendOrderTargetComplete = DecimalExtension.CalculateTargetComplete(operationsData.SendOrder, operationsData.SendOrderTarget).Value;
                operationsData.ToHospitalTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, query.IsOldCustomer.Value ? target?.OldCustomerVisitTarget ?? 0 : target?.OldCustomerVisitTarget ?? 0);
                operationsData.ToHospital = baseOrderPerformance.VisitNum;
                operationsData.ToHospitalTargetComplete = DecimalExtension.CalculateTargetComplete(operationsData.ToHospital, operationsData.ToHospitalTarget).Value;
                operationsData.Deal = baseOrderPerformance.DealNum;
                operationsData.DealTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, query.IsOldCustomer.Value ? target?.OldCustomerDealTarget ?? 0m : target?.NewCustomerDealTarget ?? 0m);
                operationsData.DealTargetComplete = DecimalExtension.CalculateTargetComplete(operationsData.Deal, operationsData.DealTarget).Value;
                dataList.Add(operationsData);
            }
            CompanyOperationsDataDto total = new CompanyOperationsDataDto();
            total.GroupName = "总计";
            total.SendOrderTarget = dataList.Sum(e => e.SendOrderTarget);
            total.SendOrder = dataList.Sum(e => e.SendOrder);
            total.SendOrderTargetComplete = DecimalExtension.CalculateTargetComplete(total.SendOrder, total.SendOrderTarget).Value;
            total.ToHospitalTarget = dataList.Sum(e => e.ToHospitalTarget);
            total.ToHospital = dataList.Sum(e => e.ToHospital);
            total.ToHospitalTargetComplete = DecimalExtension.CalculateTargetComplete(total.ToHospital, total.ToHospitalTarget).Value;
            total.Deal = dataList.Sum(e => e.Deal);
            total.DealTarget = dataList.Sum(e => e.DealTarget);
            total.DealTargetComplete = DecimalExtension.CalculateTargetComplete(total.Deal, total.DealTarget).Value;
            dataList.Add(total);
            return dataList;
        }
        /// <summary>
        /// 获取公司看板指标转化情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyIndicatorConversionDataDto>> GetCompanyIndicatorConversionDataAsync(QueryAmiyaCompanyOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            query.LiveAnchorIds = nameList.Select(e => e.Id).ToList();
            List<CompanyIndicatorConversionDataDto> dataList = new List<CompanyIndicatorConversionDataDto>();
            List<ShoppingCartRegistrationIndicatorBaseDataDto> effectiveBaseData = new List<ShoppingCartRegistrationIndicatorBaseDataDto>();
            List<ShoppingCartRegistrationIndicatorBaseDataDto> potentialBaseData = new List<ShoppingCartRegistrationIndicatorBaseDataDto>();
            foreach (var liveanchorId in query.LiveAnchorIds)
            {
                var liveanchorName = nameList.Where(e => e.Id == liveanchorId).Select(e => e.LiveAnchorName).FirstOrDefault();
                CompanyIndicatorConversionDataDto data = new CompanyIndicatorConversionDataDto();
                var effectiveData = await shoppingCartRegistrationService.GetIndicatorConversionDataAsync(selectDate.StartDate, selectDate.EndDate, liveanchorId, query.IsEffective);
                effectiveBaseData.Add(effectiveData);
                data.GroupName = $"{liveanchorName}组";
                data.SevenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveData.SevenDaySendOrderCount, effectiveData.TotalCount).Value;
                data.FifteenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveData.FifteenToHospitalCount, effectiveData.TotalCount).Value;
                data.OldCustomerToHospitalRate = DecimalExtension.CalculateTargetComplete(effectiveData.OldCustomerDealCount, effectiveData.OldCustomerCountEndLastMonth).Value;
                data.RePurchaseRate = DecimalExtension.CalculateTargetComplete(effectiveData.OldCustomerCount, effectiveData.NewCustomerCount).Value;
                data.AddWechatRate = DecimalExtension.CalculateTargetComplete(effectiveData.AddWechatCount, effectiveData.TotalCount).Value;
                data.SendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveData.SendOrderCount, effectiveData.AddWechatCount).Value;
                data.ToHospitalRate = DecimalExtension.CalculateTargetComplete(effectiveData.ToHospitalCount, effectiveData.SendOrderCount).Value;
                data.NewCustomerDealRate = DecimalExtension.CalculateTargetComplete(effectiveData.NewCustomerDealCount, effectiveData.NewCustomerToHospitalCount).Value;
                data.NewCustomerUnitPrice = DecimalExtension.Division(effectiveData.NewCustomerTotalPerformance, effectiveData.NewCustomerDealCount).Value;
                data.OldCustomerUnitPrice = DecimalExtension.Division(effectiveData.OldCustomerTotalPerformance, effectiveData.OldCustomerDealCount).Value;
                dataList.Add(data);
            }
            CompanyIndicatorConversionDataDto totalEffectiveData = new CompanyIndicatorConversionDataDto();
            totalEffectiveData.GroupName = "总业绩";
            totalEffectiveData.SevenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.SevenDaySendOrderCount), effectiveBaseData.Sum(e => e.TotalCount)).Value;
            totalEffectiveData.FifteenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.FifteenToHospitalCount), effectiveBaseData.Sum(e => e.TotalCount)).Value;
            totalEffectiveData.OldCustomerToHospitalRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.OldCustomerToHospitalCount), effectiveBaseData.Sum(e => e.OldCustomerCountEndLastMonth)).Value;
            totalEffectiveData.RePurchaseRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.OldCustomerCount), effectiveBaseData.Sum(e => e.NewCustomerCount)).Value;
            totalEffectiveData.AddWechatRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.AddWechatCount), effectiveBaseData.Sum(e => e.TotalCount)).Value;
            totalEffectiveData.SendOrderRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.SendOrderCount), effectiveBaseData.Sum(e => e.AddWechatCount)).Value;
            totalEffectiveData.ToHospitalRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.ToHospitalCount), effectiveBaseData.Sum(e => e.SendOrderCount)).Value;
            totalEffectiveData.NewCustomerDealRate = DecimalExtension.CalculateTargetComplete(effectiveBaseData.Sum(e => e.NewCustomerDealCount), effectiveBaseData.Sum(e => e.NewCustomerToHospitalCount)).Value;
            totalEffectiveData.NewCustomerUnitPrice = DecimalExtension.Division(effectiveBaseData.Sum(e => e.NewCustomerTotalPerformance), effectiveBaseData.Sum(e => e.NewCustomerDealCount)).Value;
            totalEffectiveData.OldCustomerUnitPrice = DecimalExtension.Division(effectiveBaseData.Sum(e => e.OldCustomerTotalPerformance), effectiveBaseData.Sum(e => e.OldCustomerDealCount)).Value;
            dataList.Add(totalEffectiveData);
            return dataList;
        }
        /// <summary>
        /// 获取公司历史分诊新客转化情况数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<CompanyNewCustomerConversionDataDto>> GetHistoryNewCustomerConversionDataAsync(QueryAmiyaCompanyOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            query.LiveAnchorIds = nameList.Select(e => e.Id).ToList();
            List<CompanyNewCustomerConversionDataDto> dataList = new List<CompanyNewCustomerConversionDataDto>();
            List<CompanyNewCustomerConversionDataDto> groupDataList = new List<CompanyNewCustomerConversionDataDto>();
            int toHospitalTotalCount = 0;
            foreach (var liveanchorId in query.LiveAnchorIds)
            {
                var liveanchorName = nameList.Where(e => e.Id == liveanchorId).Select(e => e.LiveAnchorName).FirstOrDefault();
                var effectiveBaseData = await shoppingCartRegistrationService.GetHistoryNewCustomerConversionDataAsync(selectDate.StartDate, selectDate.EndDate, liveanchorId, false, true);
                var potentialBaseData = await shoppingCartRegistrationService.GetHistoryNewCustomerConversionDataAsync(selectDate.StartDate, selectDate.EndDate, liveanchorId, false, false);

                CompanyNewCustomerConversionDataDto effectiveData = new CompanyNewCustomerConversionDataDto();
                effectiveData.GroupName = $"{liveanchorName}组-有效";
                effectiveData.SendOrderCount = effectiveBaseData.SendOrderCount;
                effectiveData.ToHospitalCount = effectiveBaseData.ToHospitalCount;
                effectiveData.DealCount = effectiveBaseData.DealCount;
                effectiveData.DealRate = DecimalExtension.CalculateTargetComplete(effectiveData.DealCount, effectiveBaseData.ToHospitalCount).Value;
                effectiveData.Performance = effectiveBaseData.DealPrice;
                dataList.Add(effectiveData);

                CompanyNewCustomerConversionDataDto potentialData = new CompanyNewCustomerConversionDataDto();
                potentialData.GroupName = $"{liveanchorName}组-潜在";
                potentialData.SendOrderCount = potentialBaseData.SendOrderCount;
                potentialData.ToHospitalCount = potentialBaseData.ToHospitalCount;
                potentialData.DealCount = potentialBaseData.DealCount;
                potentialData.DealRate = DecimalExtension.CalculateTargetComplete(potentialData.DealCount, potentialBaseData.ToHospitalCount).Value;
                potentialData.Performance = potentialBaseData.DealPrice;
                dataList.Add(potentialData);

                var totalCount = effectiveBaseData.TotalCount + potentialBaseData.TotalCount;
                var totalSendCount = effectiveData.SendOrderCount + potentialData.SendOrderCount;
                var totalTohospitalCount = effectiveData.ToHospitalCount + potentialData.ToHospitalCount;
                var totalDealCount = effectiveData.DealCount + potentialData.DealCount;

                CompanyNewCustomerConversionDataDto totalData = new CompanyNewCustomerConversionDataDto();
                totalData.GroupName = $"{liveanchorName}组";
                totalData.SendOrderCount = totalSendCount;
                totalData.ToHospitalCount = totalTohospitalCount;
                totalData.DealCount = totalDealCount;
                totalData.DealRate = DecimalExtension.CalculateTargetComplete(totalDealCount, totalTohospitalCount).Value;
                totalData.Performance = potentialData.Performance + effectiveData.Performance;
                dataList.Add(totalData);
                groupDataList.Add(totalData);
                toHospitalTotalCount += totalTohospitalCount;
            }
            CompanyNewCustomerConversionDataDto data = new CompanyNewCustomerConversionDataDto();
            data.GroupName = "总计";
            data.SendOrderCount = groupDataList.Sum(e => e.SendOrderCount);
            data.ToHospitalCount = groupDataList.Sum(e => e.ToHospitalCount);
            data.DealCount = groupDataList.Sum(e => e.DealCount);
            data.DealRate = DecimalExtension.CalculateTargetComplete(data.DealCount, toHospitalTotalCount).Value;
            data.Performance = groupDataList.Sum(e => e.Performance);
            dataList.Add(data);
            return dataList;
        }
        #endregion

        #region 助理看板
        /// <summary>
        /// 获取助理看板业绩情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<AssistantPerformanceDataDto>> GetAssistantPerformanceDataAsync(QueryAmiyaAssistantOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var assistantNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(query.LiveAnchorIds);
            var assistantTarget = await employeePerformanceTargetService.GetEmployeeTargetByAssistantIdListAsync(query.StartDate.Value.Year, query.StartDate.Value.Month, assistantNameList.Select(e => e.Id).ToList());
            var order = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantNameList.Select(e => e.Id).ToList());
            var resList = order.GroupBy(e => e.BelongEmployeeId).Select(e =>
            {
                var target = assistantTarget.Where(a => a.EmployeeId == e.Key).FirstOrDefault();
                var name = assistantNameList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其他";
                AssistantPerformanceDataDto data = new AssistantPerformanceDataDto();
                data.AssistantName = name;
                data.NewCustomerPerformanceTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.NewCustomerPerformanceTarget ?? 0);
                data.CurrentMonthNewCustomerPerformance = e.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
                data.NewCustomerPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthNewCustomerPerformance, data.NewCustomerPerformanceTarget).Value;
                data.OldCustomerTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.OldCustomerPerformanceTarget ?? 0);
                data.CurrentMonthOldCustomerPerformance = e.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
                data.OldCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthOldCustomerPerformance, data.OldCustomerTarget).Value;
                data.TotalPerformanceTarget = data.NewCustomerPerformanceTarget + data.OldCustomerTarget;
                data.TotalPerformance = data.CurrentMonthNewCustomerPerformance + data.CurrentMonthOldCustomerPerformance;
                data.TotalPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(data.TotalPerformance, data.TotalPerformanceTarget).Value;
                return data;
            }).ToList();
            AssistantPerformanceDataDto data = new AssistantPerformanceDataDto();
            data.AssistantName = "其他";
            data.NewCustomerPerformanceTarget = 0;
            data.CurrentMonthNewCustomerPerformance = resList.Where(e => e.AssistantName == "其他").Sum(e => e.CurrentMonthNewCustomerPerformance);
            data.NewCustomerPerformanceTargetComplete = 0;
            data.OldCustomerTarget = 0;
            data.CurrentMonthOldCustomerPerformance = resList.Where(e => e.AssistantName == "其他").Sum(e => e.CurrentMonthOldCustomerPerformance);
            data.OldCustomerTargetComplete = 0;
            data.TotalPerformanceTarget = 0;
            data.TotalPerformance = data.CurrentMonthNewCustomerPerformance + data.CurrentMonthOldCustomerPerformance;
            data.TotalPerformanceTargetComplete = 0;
            resList.RemoveAll(e => e.AssistantName == "其他");
            resList.Add(data);
            AssistantPerformanceDataDto total = new AssistantPerformanceDataDto();
            total.AssistantName = "总计";
            total.NewCustomerPerformanceTarget = resList.Sum(e => e.NewCustomerPerformanceTarget);
            total.CurrentMonthNewCustomerPerformance = resList.Sum(e => e.CurrentMonthNewCustomerPerformance);
            total.NewCustomerPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(total.CurrentMonthNewCustomerPerformance, total.NewCustomerPerformanceTarget).Value;
            total.OldCustomerTarget = resList.Sum(e => e.OldCustomerTarget);
            total.CurrentMonthOldCustomerPerformance = resList.Sum(e => e.CurrentMonthOldCustomerPerformance);
            total.OldCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(total.CurrentMonthOldCustomerPerformance, total.OldCustomerTarget).Value;
            total.TotalPerformanceTarget = total.OldCustomerTarget + total.NewCustomerPerformanceTarget;
            total.TotalPerformance = total.CurrentMonthNewCustomerPerformance + total.CurrentMonthOldCustomerPerformance;
            total.TotalPerformanceTargetComplete = DecimalExtension.CalculateTargetComplete(total.TotalPerformance, total.TotalPerformanceTarget).Value;
            resList.Add(total);
            return resList;
        }
        /// <summary>
        /// 获取助理看板获客情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<AssistantCustomerAcquisitionDataDto>> GetAssistantCustomerAcquisitionDataAsync(QueryAmiyaAssistantOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var assistantNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(query.LiveAnchorIds);
            var assistantTarget = await employeePerformanceTargetService.GetEmployeeTargetByAssistantIdListAsync(query.StartDate.Value.Year, query.StartDate.Value.Month, assistantNameList.Select(e => e.Id).ToList());
            var data = await shoppingCartRegistrationService.GetPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantNameList.Select(e => e.Id).ToList());
            var resList = data.GroupBy(e => e.AssignEmpId).Select(e =>
            {
                var target = assistantTarget.Where(a => a.EmployeeId == e.Key).FirstOrDefault();
                var name = assistantNameList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其他";
                AssistantCustomerAcquisitionDataDto data = new AssistantCustomerAcquisitionDataDto();
                data.AssistantName = name;
                data.PotentialAllocationConsulation = e.Where(e => (e.AddPrice != 29.9m && e.AddPrice != 199m)).Count();
                data.PotentialAllocationConsulationTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.PotentialConsulationCardTarget ?? 0);
                data.PotentialAllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(data.PotentialAllocationConsulation, data.PotentialAllocationConsulationTarget).Value;
                data.PotentialAddWechat = e.Where(e => (e.AddPrice != 29.9m && e.AddPrice != 199m) && e.IsAddWeChat == true).Count();
                data.PotentialAddWechatTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.PotentialAddWechatTarget ?? 0);
                data.PotentialAddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(data.PotentialAddWechat, data.PotentialAddWechatTarget).Value;
                data.EffectiveAllocationConsulation = e.Where(e => (e.AddPrice == 29.9m || e.AddPrice == 199m)).Count();
                data.EffectiveAllocationConsulationTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.EffectiveConsulationCardTarget ?? 0);
                data.EffectiveAllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(data.EffectiveAllocationConsulation, data.EffectiveAllocationConsulationTarget).Value;
                data.EffectiveAddWechat = e.Where(e => (e.AddPrice == 29.9m || e.AddPrice == 199m) && e.IsAddWeChat == true).Count();
                data.EffectiveAddWechatTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.EffectiveAddWechatTarget ?? 0);
                data.EffectiveAddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(data.EffectiveAddWechat, data.EffectiveAddWechatTarget).Value;
                return data;
            }).ToList();
            AssistantCustomerAcquisitionDataDto other = new AssistantCustomerAcquisitionDataDto();
            other.AssistantName = "其他";
            other.PotentialAllocationConsulation = resList.Where(e => e.AssistantName == "其他").Sum(e => e.PotentialAllocationConsulation);
            other.PotentialAllocationConsulationTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.PotentialAllocationConsulationTarget);
            other.PotentialAllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(other.PotentialAllocationConsulation, other.PotentialAllocationConsulationTarget).Value;
            other.PotentialAddWechat = resList.Where(e => e.AssistantName == "其他").Sum(e => e.PotentialAddWechat);
            other.PotentialAddWechatTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.PotentialAddWechatTarget);
            other.PotentialAddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(other.PotentialAddWechat, other.PotentialAddWechatTarget).Value;
            other.EffectiveAllocationConsulation = resList.Where(e => e.AssistantName == "其他").Sum(e => e.EffectiveAllocationConsulation);
            other.EffectiveAllocationConsulationTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.EffectiveAllocationConsulationTarget);
            other.EffectiveAllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(other.EffectiveAllocationConsulation, other.EffectiveAllocationConsulationTarget).Value;
            other.EffectiveAddWechat = resList.Where(e => e.AssistantName == "其他").Sum(e => e.EffectiveAddWechat);
            other.EffectiveAddWechatTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.EffectiveAddWechatTarget);
            other.EffectiveAddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(other.EffectiveAddWechat, other.EffectiveAddWechatTarget).Value;
            resList.RemoveAll(e => e.AssistantName == "其他");
            resList.Add(other);
            AssistantCustomerAcquisitionDataDto total = new AssistantCustomerAcquisitionDataDto();
            total.AssistantName = "总计";
            total.PotentialAllocationConsulation = resList.Sum(e => e.PotentialAllocationConsulation);
            total.PotentialAllocationConsulationTarget = resList.Sum(e => e.PotentialAllocationConsulationTarget);
            total.PotentialAllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(total.PotentialAllocationConsulation, total.PotentialAllocationConsulationTarget).Value;
            total.PotentialAddWechat = resList.Sum(e => e.PotentialAddWechat);
            total.PotentialAddWechatTarget = resList.Sum(e => e.PotentialAddWechatTarget);
            total.PotentialAddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(total.PotentialAddWechat, total.PotentialAddWechatTarget).Value;
            total.EffectiveAllocationConsulation = resList.Sum(e => e.EffectiveAllocationConsulation);
            total.EffectiveAllocationConsulationTarget = resList.Sum(e => e.EffectiveAllocationConsulationTarget);
            total.EffectiveAllocationConsulationTargetComplete = DecimalExtension.CalculateTargetComplete(total.EffectiveAllocationConsulation, total.EffectiveAllocationConsulationTarget).Value;
            total.EffectiveAddWechat = resList.Sum(e => e.EffectiveAddWechat);
            total.EffectiveAddWechatTarget = resList.Sum(e => e.EffectiveAddWechatTarget);
            total.EffectiveAddWechatTargetComplete = DecimalExtension.CalculateTargetComplete(total.EffectiveAddWechat, total.EffectiveAddWechatTarget).Value;
            resList.Add(total);
            return resList;
        }
        /// <summary>
        /// 获取助理看板运营情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<AssistantOperationsDataDto>> GetAssistantOperationsDataAsync(QueryAmiyaAssistantOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var assistantNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(query.LiveAnchorIds);
            var assistantTarget = await employeePerformanceTargetService.GetEmployeeTargetByAssistantIdListAsync(query.StartDate.Value.Year, query.StartDate.Value.Month, assistantNameList.Select(e => e.Id).ToList());
            var baseOrderPerformance = await contentPlateFormOrderService.GetOrderSendAndDealDataByMonthAndBaseLiveAnchorIdAsync(selectDate.StartDate, selectDate.EndDate, query.IsOldCustomer.Value, assistantNameList.Select(e => e.Id).ToList());
            var resList = baseOrderPerformance.Select(e =>
            {
                var target = assistantTarget.Where(a => a.EmployeeId == e.BelongEmpId).FirstOrDefault();
                var name = assistantNameList.Where(a => a.Id == e.BelongEmpId).FirstOrDefault()?.Name ?? "其他";
                AssistantOperationsDataDto data = new AssistantOperationsDataDto();
                data.AssistantName = name;
                data.SendOrder = e.SendOrderNum;
                data.SendOrderTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, target?.SendOrderTarget ?? 0);
                data.SendOrderTargetComplete = DecimalExtension.CalculateTargetComplete(data.SendOrder, data.SendOrderTarget).Value;
                data.ToHospital = e.VisitNum;
                data.ToHospitalTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, query.IsOldCustomer.Value ? target?.OldCustomerVisitTarget ?? 0 : target?.NewCustomerVisitTarget ?? 0);
                data.ToHospitalTargetComplete = DecimalExtension.CalculateTargetComplete(data.ToHospital, data.ToHospitalTarget).Value;
                data.Deal = e.DealNum;
                data.DealTarget = CalTarget(query.StartDate.Value, query.EndDate.Value, query.IsOldCustomer.Value ? target?.OldCustomerDealNumTarget ?? 0 : target?.NewCustomerDealNumTarget ?? 0);
                data.DealTargetComplete = DecimalExtension.CalculateTargetComplete(data.Deal, data.DealTarget).Value;
                return data;
            }).ToList();
            AssistantOperationsDataDto other = new AssistantOperationsDataDto();
            other.AssistantName = "其他";
            other.SendOrder = resList.Where(e => e.AssistantName == "其他").Sum(e => e.SendOrder);
            other.SendOrderTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.SendOrderTarget);
            other.SendOrderTargetComplete = DecimalExtension.CalculateTargetComplete(other.SendOrder, other.SendOrderTarget).Value;
            other.ToHospital = resList.Where(e => e.AssistantName == "其他").Sum(e => e.ToHospital);
            other.ToHospitalTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.ToHospitalTarget);
            other.ToHospitalTargetComplete = DecimalExtension.CalculateTargetComplete(other.ToHospital, other.ToHospitalTarget).Value;
            other.Deal = resList.Where(e => e.AssistantName == "其他").Sum(e => e.Deal);
            other.DealTarget = resList.Where(e => e.AssistantName == "其他").Sum(e => e.DealTarget);
            other.DealTargetComplete = DecimalExtension.CalculateTargetComplete(other.Deal, other.DealTarget).Value;
            resList.RemoveAll(e => e.AssistantName == "其他");
            resList.Add(other);
            AssistantOperationsDataDto total = new AssistantOperationsDataDto();
            total.AssistantName = "总计";
            total.SendOrder = resList.Sum(e => e.SendOrder);
            total.SendOrderTarget = resList.Sum(e => e.SendOrderTarget);
            total.SendOrderTargetComplete = DecimalExtension.CalculateTargetComplete(total.SendOrder, total.SendOrderTarget).Value;
            total.ToHospital = resList.Sum(e => e.ToHospital);
            total.ToHospitalTarget = resList.Sum(e => e.ToHospitalTarget);
            total.ToHospitalTargetComplete = DecimalExtension.CalculateTargetComplete(total.ToHospital, total.ToHospitalTarget).Value;
            total.Deal = resList.Sum(e => e.Deal);
            total.DealTarget = resList.Sum(e => e.DealTarget);
            total.DealTargetComplete = DecimalExtension.CalculateTargetComplete(total.Deal, total.DealTarget).Value;
            resList.Add(total);
            return resList;
        }
        /// <summary>
        /// 获取助理看板指标转化情况数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<AssistantIndicatorConversionDataDto>> GetAssistantIndicatorConversionDataAsync(QueryAmiyaAssistantOperationsDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate.Value, query.EndDate.Value);
            var assistantNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(query.LiveAnchorIds);
            var baseData = await shoppingCartRegistrationService.GetIndicatorConversionDataByAssistantIdsAsync(selectDate.StartDate, selectDate.EndDate, assistantNameList.Select(e => e.Id).ToList(), query.IsEffective);
            var resList = baseData.GroupBy(e => e.EmpId).Select(e =>
            {
                var name = assistantNameList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其他";
                AssistantIndicatorConversionDataDto data = new AssistantIndicatorConversionDataDto();
                data.AssistantName = name;
                data.SevenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.SevenDaySendOrderCount), e.Sum(e => e.TotalCount)).Value;
                data.FifteenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.FifteenToHospitalCount), e.Sum(e => e.TotalCount)).Value;
                data.OldCustomerToHospitalRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.OldCustomerDealCount), e.Sum(e => e.OldCustomerCountEndLastMonth)).Value;
                data.RePurchaseRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.OldCustomerCount), e.Sum(e => e.NewCustomerCount)).Value;
                data.AddWechatRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.AddWechatCount), e.Sum(e => e.TotalCount)).Value;
                data.SendOrderRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.SendOrderCount), e.Sum(e => e.AddWechatCount)).Value;
                data.ToHospitalRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.ToHospitalCount), e.Sum(e => e.SendOrderCount)).Value;
                data.NewCustomerDealRate = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.NewCustomerDealCount), e.Sum(e => e.NewCustomerToHospitalCount)).Value;
                data.NewCustomerUnitPrice = DecimalExtension.Division(e.Sum(e => e.NewCustomerTotalPerformance), e.Sum(e => e.NewCustomerDealCount)).Value;
                data.OldCustomerUnitPrice = DecimalExtension.Division(e.Sum(e => e.OldCustomerTotalPerformance), e.Sum(e => e.OldCustomerDealCount)).Value;
                return data;
            }).ToList();
            var otherList = baseData.Where(e => !assistantNameList.Select(e => e.Id).Contains(e.EmpId));
            AssistantIndicatorConversionDataDto other = new AssistantIndicatorConversionDataDto();
            other.AssistantName = "其他";
            other.SevenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.SevenDaySendOrderCount), otherList.Sum(e => e.TotalCount)).Value;
            other.FifteenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.FifteenToHospitalCount), otherList.Sum(e => e.TotalCount)).Value;
            other.OldCustomerToHospitalRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.OldCustomerDealCount), otherList.Sum(e => e.OldCustomerCountEndLastMonth)).Value;
            other.RePurchaseRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.OldCustomerCount), otherList.Sum(e => e.NewCustomerCount)).Value;
            other.AddWechatRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.AddWechatCount), otherList.Sum(e => e.TotalCount)).Value;
            other.SendOrderRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.SendOrderCount), otherList.Sum(e => e.AddWechatCount)).Value;
            other.ToHospitalRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.ToHospitalCount), otherList.Sum(e => e.SendOrderCount)).Value;
            other.NewCustomerDealRate = DecimalExtension.CalculateTargetComplete(otherList.Sum(e => e.NewCustomerDealCount), otherList.Sum(e => e.NewCustomerToHospitalCount)).Value;
            other.NewCustomerUnitPrice = DecimalExtension.Division(otherList.Sum(e => e.NewCustomerTotalPerformance), otherList.Sum(e => e.NewCustomerDealCount)).Value;
            other.OldCustomerUnitPrice = DecimalExtension.Division(otherList.Sum(e => e.OldCustomerTotalPerformance), otherList.Sum(e => e.OldCustomerDealCount)).Value;
            resList.Add(other);
            AssistantIndicatorConversionDataDto total = new AssistantIndicatorConversionDataDto();
            total.AssistantName = "总计";
            total.SevenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.SevenDaySendOrderCount), baseData.Sum(e => e.TotalCount)).Value;
            total.FifteenDaySendOrderRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.FifteenToHospitalCount), baseData.Sum(e => e.TotalCount)).Value;
            total.OldCustomerToHospitalRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.OldCustomerDealCount), baseData.Sum(e => e.OldCustomerCountEndLastMonth)).Value;
            total.RePurchaseRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.OldCustomerCount), baseData.Sum(e => e.NewCustomerCount)).Value;
            total.AddWechatRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.AddWechatCount), baseData.Sum(e => e.TotalCount)).Value;
            total.SendOrderRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.SendOrderCount), baseData.Sum(e => e.AddWechatCount)).Value;
            total.ToHospitalRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.ToHospitalCount), baseData.Sum(e => e.SendOrderCount)).Value;
            total.NewCustomerDealRate = DecimalExtension.CalculateTargetComplete(baseData.Sum(e => e.NewCustomerDealCount), baseData.Sum(e => e.NewCustomerToHospitalCount)).Value;
            total.NewCustomerUnitPrice = DecimalExtension.Division(baseData.Sum(e => e.NewCustomerTotalPerformance), baseData.Sum(e => e.NewCustomerDealCount)).Value;
            total.OldCustomerUnitPrice = DecimalExtension.Division(baseData.Sum(e => e.OldCustomerTotalPerformance), baseData.Sum(e => e.OldCustomerDealCount)).Value;
            resList.Add(total);
            return resList;
        }
        #endregion

        #region
        private int CalTarget(DateTime datetime1, DateTime datetime2, decimal target)
        {
            int day = 0;
            if (datetime1 == datetime2)
            {
                day = DateTime.DaysInMonth(datetime1.Year, datetime1.Month);
            }
            else
            {
                day = 1;
            }
            return (int)Math.Ceiling(DecimalExtension.Division(target, day).Value);
        }

        private List<string> GetContentPlatformIdList(QueryTransformDataDto query)
        {
            List<string> idList = new List<string>();
            if (query.ShowTikTok)
            {
                idList.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
            }
            if (query.ShowWechatVideo)
            {
                idList.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
            }
            if (query.ShowXiaoHongShu)
            {
                idList.Add("317c03b8-aff9-4961-8392-fc44d04b1725");
            }
            if (query.ShowPrivateDomain)
            {
                idList.Add("22a0b287-232d-4373-a9dd-c372aaae57dc");
            }
            return idList.Any() ? idList : null;
        }
        private List<string> TargetGetContentPlatformIdList(QueryAssistantTargetCompleteDataDto query)
        {
            List<string> idList = new List<string>();
            if (query.ShowTikTok)
            {
                idList.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
            }
            if (query.ShowWechatVideo)
            {
                idList.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
            }
            if (query.ShowXiaoHongShu)
            {
                idList.Add("317c03b8-aff9-4961-8392-fc44d04b1725");
            }
            if (query.ShowPrivateDomain)
            {
                idList.Add("22a0b287-232d-4373-a9dd-c372aaae57dc");
            }
            return idList.Any() ? idList : null;
        }
        private async Task<List<string>> GetBaseLiveAnchorIdListAsync(QueryHospitalTransformDataDto query)
        {
            List<string> idList = new List<string>();
            if (query.ShowDaoDao)
            {
                idList.Add("f0a77257-c905-4719-95c4-ad2c4f33855c");
            }
            if (query.ShowJiNa)
            {
                idList.Add("af69dcf5-f749-41ea-8b50-fe685facdd8b");
            }
            if (query.ShowLuLu)
            {
                idList.Add("fed06778-06f2-4c92-afee-f098b77ac81c");
            }
            if (query.ShowCooperate)
            {
                var thridLiveanchor = await liveAnchorBaseInfoService.GetCooperateLiveAnchorAsync(true);
                idList.AddRange(thridLiveanchor.Select(e => e.Id));
            }

            if (idList.Count == 0)
            {
                var thridLiveanchor = await liveAnchorBaseInfoService.GetCooperateLiveAnchorAsync(true);
                var selfLiveanchor = await liveAnchorBaseInfoService.GetValidAsync(true);
                idList.AddRange(thridLiveanchor.Select(e => e.Id));
                idList.AddRange(selfLiveanchor.Select(e => e.Id));
            }

            return idList;
        }

        #endregion
        /// <summary>
        /// 获取助理新客上门人数和目标完成率
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<NewCustomerToHospiatlAndTargetCompleteDto> GetNewCustomerToHospiatlAndTargetCompleteAsync(QueryNewCustomerToHospiatlAndTargetCompleteDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var target = dalEmployeePerformanceTarget.GetAll().Where(e => e.EmployeeId == query.EmpId && e.BelongYear == query.StartDate.Year && e.BelongMonth == query.StartDate.Month).FirstOrDefault();
            if (target == null)
                throw new Exception($"请先填写{query.StartDate.Year}-{query.StartDate.Month}月份的目标");
            var totalTarget = target.PerformanceTarget;
            var data = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId == query.EmpId : e.ContentPlatFormOrder.BelongEmpId == query.EmpId)
                .Select(e => new
                {
                    IsOldCustomer = e.IsOldCustomer,
                    IsToHospital = e.IsToHospital,
                    IsDeal = e.IsDeal,
                    DealPrice = e.Price,
                    Phone = e.ContentPlatFormOrder.Phone,
                    CustomerSource = e.ContentPlatFormOrder.CustomerSource,
                    ConsumptionType = e.ConsumptionType,
                }).ToListAsync();
            NewCustomerToHospiatlAndTargetCompleteDto dataDto = new NewCustomerToHospiatlAndTargetCompleteDto();
            dataDto.NewCustomerToHospitalCount = data.Where(e => e.ConsumptionType != (int)ConsumptionType.Refund).Where(e => e.IsOldCustomer == false && e.IsToHospital == true).Select(e => e.Phone).Distinct().Count();
            dataDto.OldTakeNewCustomerNum = data.Where(e => e.ConsumptionType != (int)ConsumptionType.Refund).Where(x => x.IsDeal == true && x.IsOldCustomer == false && x.CustomerSource == (int)TiktokCustomerSource.OldTakeNewCustomer).Select(e => e.Phone).Distinct().Count();
            dataDto.TargetComplete = DecimalExtension.CalculateTargetComplete(data.Where(e => e.IsDeal == true).Sum(e => e.DealPrice), totalTarget).Value;
            return dataDto;
        }
        /// <summary>
        /// 根据助理医院获取上门人数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ToHospitalDto> GetToHospiatlByAssistantAndHospitalIdListAsync(QueryToHospiatlByAssistantAndHospitalIdListDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var data = dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                .Where(e => e.ConsumptionType != (int)ConsumptionType.Refund)
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId == query.AssistantId : e.ContentPlatFormOrder.BelongEmpId == query.AssistantId)
                .Where(e => query.HospitalIdList.Contains(e.LastDealHospitalId.Value))
                .Where(e => e.IsToHospital == true && e.IsOldCustomer == false)
                .Select(e => e.ContentPlatFormOrder.Phone).Distinct();
            ToHospitalDto dataDto = new ToHospitalDto();
            dataDto.ToHospitalCount = await data.CountAsync();
            return dataDto;
        }
        /// <summary>
        /// 助理业绩目标达成情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<AssitantTargetCompleteDto>> GetAssitantTargetCompleteAsync(QueryAssistantTargetCompleteDataDto query)
        {

            List<AssitantTargetCompleteDto> assitantTargetCompletes = new List<AssitantTargetCompleteDto>();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var liveanchorIds = new List<string>();
            if (string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
                liveanchorIds = nameList.Where(e => e.LiveAnchorName.Contains("刀刀") || e.LiveAnchorName.Contains("吉娜") || e.LiveAnchorName.Contains("璐璐")).Select(e => e.Id).ToList();
            }
            else
            {
                liveanchorIds = new List<string> { query.BaseLiveAnchorId };
            }
            var assistantNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(liveanchorIds);
            var assistantTarget = await dalEmployeePerformanceTarget.GetAll()
                .Where(e => e.Valid == true)
                .Where(e => e.BelongYear == selectDate.StartDate.Year && e.BelongMonth == selectDate.StartDate.Month)
                .Where(e => assistantNameList.Select(e => e.Id).ToList().Contains(e.EmployeeId))
                .Select(e => new
                {
                    EmpId = e.EmployeeId,
                    NewCustomerTarget = e.NewCustomerPerformanceTarget,
                    OldCustomerTarget = e.OldCustomerPerformanceTarget
                }).ToListAsync();
            query.ContentPlatFormIds = TargetGetContentPlatformIdList(query);
            var currentContentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
               .Where(e => e.IsDeal == true)
               .Where(o => query.ContentPlatFormIds == null || query.ContentPlatFormIds.Contains(o.ContentPlatFormOrder.ContentPlateformId))
               .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate && liveanchorIds.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
               .Select(e => new
               {
                   AssignEmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                   DealPrice = e.Price,
                   IsOldCustomer = e.IsOldCustomer
               }).ToList();
            var historyStartDate = query.StartDate.AddMonths(-1);
            var historyEndDate = query.EndDate.AddMonths(-1);
            var historyContentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
               .Where(e => e.IsDeal == true)
               .Where(o => query.ContentPlatFormIds == null || query.ContentPlatFormIds.Contains(o.ContentPlatFormOrder.ContentPlateformId))
               .Where(e => e.CreateDate >= historyStartDate && e.CreateDate < historyEndDate && liveanchorIds.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
               .Select(e => new
               {
                   AssignEmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                   DealPrice = e.Price,
                   IsOldCustomer = e.IsOldCustomer
               }).ToList();
            foreach (var assistant in assistantNameList)
            {
                AssitantTargetCompleteDto data = new AssitantTargetCompleteDto();
                data.Name = assistant.Name;
                data.NewCustomerPerformanceTarget = assistantTarget.Where(e => e.EmpId == assistant.Id).Select(e => e.NewCustomerTarget).FirstOrDefault();
                data.CurrentMonthNewCustomerPerformance = currentContentOrderList.Where(e => e.IsOldCustomer == false && e.AssignEmpId == assistant.Id).Sum(e => e.DealPrice);
                data.HistoryMonthNewCustomerPerformance = historyContentOrderList.Where(e => e.IsOldCustomer == false && e.AssignEmpId == assistant.Id).Sum(e => e.DealPrice);
                data.NewCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthNewCustomerPerformance, data.NewCustomerPerformanceTarget).Value;
                data.NewCustomerChainRatio = DecimalExtension.CalculateChain(data.CurrentMonthNewCustomerPerformance, data.HistoryMonthNewCustomerPerformance).Value;
                data.OldCustomerPerformanceTarget = assistantTarget.Where(e => e.EmpId == assistant.Id).Select(e => e.OldCustomerTarget).FirstOrDefault();
                data.CurrentMonthOldCustomerPerformance = currentContentOrderList.Where(e => e.IsOldCustomer == true && e.AssignEmpId == assistant.Id).Sum(e => e.DealPrice);
                data.HistoryMonthOldCustomerPerformance = historyContentOrderList.Where(e => e.IsOldCustomer == true && e.AssignEmpId == assistant.Id).Sum(e => e.DealPrice);
                data.OldCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthOldCustomerPerformance, data.OldCustomerPerformanceTarget).Value;
                data.OldCustomerChainRatio = DecimalExtension.CalculateChain(data.CurrentMonthOldCustomerPerformance, data.HistoryMonthOldCustomerPerformance).Value;
                data.TotalCustomerPerformanceTarget = data.NewCustomerPerformanceTarget + data.OldCustomerPerformanceTarget;
                data.CurrentMonthTotalCustomerPerformance = data.CurrentMonthNewCustomerPerformance + data.CurrentMonthOldCustomerPerformance;
                data.HistoryMonthTotalCustomerPerformance = data.HistoryMonthNewCustomerPerformance + data.HistoryMonthOldCustomerPerformance;
                data.TotalCustomerTargetComplete = DecimalExtension.CalculateTargetComplete(data.CurrentMonthTotalCustomerPerformance, data.TotalCustomerPerformanceTarget).Value;
                data.TotalCustomerChainRatio = DecimalExtension.CalculateChain(data.CurrentMonthTotalCustomerPerformance, data.HistoryMonthTotalCustomerPerformance).Value;
                data.NewAndOldCustomerRate = DecimalExtension.CalculateAccounted(data.CurrentMonthNewCustomerPerformance, data.CurrentMonthOldCustomerPerformance);
                assitantTargetCompletes.Add(data);
            }
            var sumPerformance = assitantTargetCompletes.Sum(e => e.CurrentMonthTotalCustomerPerformance);
            assitantTargetCompletes = assitantTargetCompletes.OrderByDescending(e => e.CurrentMonthTotalCustomerPerformance).ToList();
            for (int i = 0; i < assitantTargetCompletes.Count; i++)
            {
                assitantTargetCompletes[i].Sort = i + 1;
                assitantTargetCompletes[i].PerformanceRate = DecimalExtension.CalculateTargetComplete(assitantTargetCompletes[i].CurrentMonthTotalCustomerPerformance, sumPerformance).Value;
            }
            return assitantTargetCompletes;
        }

        #region 助理数据运营看板

        /// <summary>
        /// 助理业绩运营情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantPerformanceDto> GetAssitantPerformanceAsync(QueryAssistantPerformanceDto query)
        {
            List<AssitantTargetCompleteDto> assitantTargetCompletes = new List<AssitantTargetCompleteDto>();

            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month == 0 ? 1 : query.EndDate.Month);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            var assistantTarget = await dalEmployeePerformanceTarget.GetAll()
                .Where(e => e.Valid == true)
                .Where(e => e.BelongYear == query.EndDate.Year && e.BelongMonth == query.EndDate.Month)
                .Where(e => assistantIdList.Contains(e.EmployeeId))
                .Select(e => new
                {
                    NewCustomerTarget = e.NewCustomerPerformanceTarget,
                    OldCustomerTarget = e.OldCustomerPerformanceTarget
                }).ToListAsync();

            //var todayPerformance = dalContentPlatFormOrderDealInfo.GetAll()
            //   .Where(e => e.IsDeal == true && e.ContentPlatFormOrderId != null)
            //   .Where(e => e.CreateDate >= DateTime.Now.Date && e.CreateDate < DateTime.Now.AddDays(1).Date)
            //   .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value) || assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value))
            //   .Select(e => new
            //   {
            //       DealPrice = e.Price,
            //       IsOldCustomer = e.IsOldCustomer
            //   }).ToList();
            var todayPerformance = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(DateTime.Now.Date, DateTime.Now.AddDays(1).Date, assistantIdList);
            //var currentContentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
            //   .Where(e => e.IsDeal == true&& e.ContentPlatFormOrderId != null)
            //   .Where(e => e.CreateDate >= sequentialDate.StartDate && e.CreateDate < sequentialDate.EndDate)
            //   .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value) || assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value))
            //   .Select(e => new
            //   {
            //       DealPrice = e.Price,
            //       IsOldCustomer = e.IsOldCustomer
            //   }).ToList();
            var currentContentOrderList = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(sequentialDate.StartDate, sequentialDate.EndDate, assistantIdList);
            //var lastMonthContentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
            //   .Where(e => e.IsDeal == true && e.ContentPlatFormOrderId != null)
            //   .Where(e => e.CreateDate >= sequentialDate.LastMonthStartDate && e.CreateDate < sequentialDate.LastMonthEndDate)
            //   .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value) || assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value))
            //   .Select(e => new
            //   {
            //       DealPrice = e.Price,
            //       IsOldCustomer = e.IsOldCustomer
            //   }).ToList();
            var lastMonthContentOrderList = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(sequentialDate.LastMonthStartDate, sequentialDate.LastMonthEndDate, assistantIdList);
            //var lastYearContentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
            //   .Where(e => e.IsDeal == true && e.ContentPlatFormOrderId != null)
            //   .Where(e => e.CreateDate >= sequentialDate.LastYearThisMonthStartDate && e.CreateDate < sequentialDate.LastYearThisMonthEndDate)
            //   .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value) || assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value))
            //   .Select(e => new
            //   {
            //       DealPrice = e.Price,
            //       IsOldCustomer = e.IsOldCustomer
            //   }).ToList();
            var lastYearContentOrderList = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(sequentialDate.LastYearThisMonthStartDate, sequentialDate.LastYearThisMonthEndDate, assistantIdList);
            AssistantPerformanceDto assistantPerformance = new AssistantPerformanceDto();
            assistantPerformance.NewCustomerPerformance = currentContentOrderList.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
            assistantPerformance.OldCustomerPerformance = currentContentOrderList.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
            assistantPerformance.TotalPerformance = assistantPerformance.NewCustomerPerformance + assistantPerformance.OldCustomerPerformance;
            assistantPerformance.TodayNewCustomerPerformance = todayPerformance.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
            assistantPerformance.TodayOldCustomerPerformance = todayPerformance.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
            assistantPerformance.TodayTotalPerformance = assistantPerformance.TodayNewCustomerPerformance + assistantPerformance.TodayOldCustomerPerformance;
            assistantPerformance.LastMonthNewCustomerPerformance = lastMonthContentOrderList.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
            assistantPerformance.LastMonthOldCustomerPerformance = lastMonthContentOrderList.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
            assistantPerformance.LastMonthTotalPerformance = assistantPerformance.LastMonthNewCustomerPerformance + assistantPerformance.LastMonthOldCustomerPerformance;
            assistantPerformance.LastYearNewCustomerPerformance = lastYearContentOrderList.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
            assistantPerformance.LastYearOldCustomerPerformance = lastYearContentOrderList.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
            assistantPerformance.LastYearTotalPerformance = assistantPerformance.LastYearNewCustomerPerformance + assistantPerformance.LastYearOldCustomerPerformance;
            assistantPerformance.NewCustomerPerformanceChain = DecimalExtension.CalculateChain(assistantPerformance.NewCustomerPerformance, assistantPerformance.LastMonthNewCustomerPerformance).Value;
            assistantPerformance.OldCustomerPerformanceChain = DecimalExtension.CalculateChain(assistantPerformance.OldCustomerPerformance, assistantPerformance.LastMonthOldCustomerPerformance).Value;
            assistantPerformance.TotalPerformanceChain = DecimalExtension.CalculateChain(assistantPerformance.TotalPerformance, assistantPerformance.LastMonthTotalPerformance).Value;
            assistantPerformance.NewCustomerPerformanceYearOnYear = DecimalExtension.CalculateChain(assistantPerformance.NewCustomerPerformance, assistantPerformance.LastYearNewCustomerPerformance).Value;
            assistantPerformance.OldCustomerPerformanceYearOnYear = DecimalExtension.CalculateChain(assistantPerformance.OldCustomerPerformance, assistantPerformance.LastYearOldCustomerPerformance).Value;
            assistantPerformance.TotalPerformanceYearOnYear = DecimalExtension.CalculateChain(assistantPerformance.TotalPerformance, assistantPerformance.LastYearTotalPerformance).Value;
            assistantPerformance.NewCustomerPerformanceTarget = assistantTarget.Sum(e => e.NewCustomerTarget);
            assistantPerformance.OldCustomerPerformanceTarget = assistantTarget.Sum(e => e.OldCustomerTarget);
            assistantPerformance.TotalPerformanceTarget = assistantPerformance.NewCustomerPerformanceTarget + assistantPerformance.OldCustomerPerformanceTarget;
            assistantPerformance.NewCustomerPerformanceTargetCompleteRate = DecimalExtension.CalculateTargetComplete(assistantPerformance.NewCustomerPerformance, assistantPerformance.NewCustomerPerformanceTarget).Value;
            assistantPerformance.OldCustomerPerformanceTargetCompleteRate = DecimalExtension.CalculateTargetComplete(assistantPerformance.OldCustomerPerformance, assistantPerformance.OldCustomerPerformanceTarget).Value;
            assistantPerformance.TotalPerformanceTargetCompleteRate = DecimalExtension.CalculateTargetComplete(assistantPerformance.TotalPerformance, assistantPerformance.TotalPerformanceTarget).Value;
            assistantPerformance.NewCustomerPerformanceTargetSchedule = this.CalculateSchedule(assistantPerformance.NewCustomerPerformanceTarget, assistantPerformance.NewCustomerPerformance, query.StartDate.Year, query.StartDate.Month);
            assistantPerformance.OldCustomerPerformanceTargetSchedule = this.CalculateSchedule(assistantPerformance.OldCustomerPerformanceTarget, assistantPerformance.OldCustomerPerformance, query.StartDate.Year, query.StartDate.Month);
            assistantPerformance.TotalPerformanceTargetSchedule = this.CalculateSchedule(assistantPerformance.TotalPerformanceTarget, assistantPerformance.TotalPerformance, query.StartDate.Year, query.StartDate.Month);
            return assistantPerformance;
        }
        /// <summary>
        /// 获取助理业绩折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantPerformanceBrokenLineDto> GetAssistantPerformanceBrokenLineDto(QueryAssistantPerformanceDto query)
        {
            AssistantPerformanceBrokenLineDto brokenLineDto = new AssistantPerformanceBrokenLineDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            var currentContentOrderList = await dalContentPlatFormOrderDealInfo.GetAll()
               .Where(e => e.IsDeal == true)
               .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
               .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId.Value))
               .Select(e => new
               {
                   DealPrice = e.Price,
                   IsOldCustomer = e.IsOldCustomer,
                   CreateDate = e.CreateDate
               }).ToListAsync();
            var newCustomerData = currentContentOrderList
                .Where(e => e.IsOldCustomer == false)
                .GroupBy(e => e.CreateDate.Date)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key.Day.ToString(),
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.DealPrice))
                })
                .ToList();
            var oldCustomerData = currentContentOrderList
                .Where(e => e.IsOldCustomer == true)
                .GroupBy(e => e.CreateDate.Date)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key.Day.ToString(),
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.DealPrice))
                })
                .ToList();
            var totalData = currentContentOrderList
               .GroupBy(e => e.CreateDate.Date)
               .Select(e => new PerformanceBrokenLineListInfoDto
               {
                   date = e.Key.Day.ToString(),
                   Performance = ChangePriceToTenThousand(e.Sum(e => e.DealPrice))
               })
               .ToList();
            brokenLineDto.NewCustomerPerformance = this.FillDate(query.EndDate.Year, query.EndDate.Month, newCustomerData);
            brokenLineDto.OldCustomerPerformance = this.FillDate(query.EndDate.Year, query.EndDate.Month, oldCustomerData);
            brokenLineDto.TotalPerformance = this.FillDate(query.EndDate.Year, query.EndDate.Month, totalData);
            return brokenLineDto;
        }

        /// <summary>
        /// 获取助理漏斗图业绩
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantOperationDataDto> GetAssistantPerformanceFilterDataAsync(QueryAssistantPerformanceFilterDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            AssistantOperationDataDto amiyaOperationDataDto = new AssistantOperationDataDto();
            AssistantNewCustomerOperationDataDto newCustomerOperationDataDto = new AssistantNewCustomerOperationDataDto();
            newCustomerOperationDataDto.newCustomerOperationDataDetails = new List<AssistantNewCustomerOperationDataDetails>();
            AssistantOldCustomerOperationDataDto oldCustomerOperationDataDto = new AssistantOldCustomerOperationDataDto();
            var healthValueList = await _healthValueService.GetValidListAsync();
            #region【小黄车数据】
            //小黄车数据
            var baseBusinessPerformance = await shoppingCartRegistrationService.GetShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, query.IsEffectiveCustomerData, assistantIdList);
            #endregion
            #region【订单数据】
            var baseOrderPerformance = await contentPlateFormOrderService.GetOrderSendAndDealDataByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, query.IsEffectiveCustomerData, assistantIdList);
            #endregion

            #region 【线索】
            //线索
            //AssistantNewCustomerOperationDataDetails addCarddetails = new AssistantNewCustomerOperationDataDetails();
            //addCarddetails.Key = "AddCard";
            //addCarddetails.Name = "线索量";
            //addCarddetails.Value = baseBusinessPerformance.Count();
            //newCustomerOperationDataDto.newCustomerOperationDataDetails.Add(addCarddetails);
            #endregion

            #region 【分诊】
            //分诊
            AssistantNewCustomerOperationDataDetails consulationdetails = new AssistantNewCustomerOperationDataDetails();
            consulationdetails.Key = "Consulation";
            consulationdetails.Name = "分诊量";
            consulationdetails.Value = baseBusinessPerformance.Where(x => x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            newCustomerOperationDataDto.newCustomerOperationDataDetails.Add(consulationdetails);

            //线索有效率
            //newCustomerOperationDataDto.ClueEffictiveRate = DecimalExtension.CalculateTargetComplete(consulationdetails.Value, addCarddetails.Value);
            //newCustomerOperationDataDto.ClueEffictiveRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ClueEffictiveRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【加v】
            AssistantNewCustomerOperationDataDetails addWechatdetails = new AssistantNewCustomerOperationDataDetails();
            //加v
            addWechatdetails.Key = "AddWeChat";
            addWechatdetails.Name = "加v量";
            addWechatdetails.Value = baseBusinessPerformance.Where(x => x.IsAddWeChat == true && x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            newCustomerOperationDataDto.newCustomerOperationDataDetails.Add(addWechatdetails);

            //加v率
            newCustomerOperationDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails.Value, consulationdetails.Value);
            newCustomerOperationDataDto.AddWeChatRateHealthValueSum = healthValueList.Where(e => e.Key == "AddWeChatHealthValueSum").Select(e => e.Rate).FirstOrDefault();
            newCustomerOperationDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【派单】
            AssistantNewCustomerOperationDataDetails sendOrderdetails = new AssistantNewCustomerOperationDataDetails();
            //派单
            sendOrderdetails.Key = "AddWeChat";
            sendOrderdetails.Name = "派单量";
            sendOrderdetails.Value = baseOrderPerformance.SendOrderNum;
            newCustomerOperationDataDto.newCustomerOperationDataDetails.Add(sendOrderdetails);

            //派单率
            newCustomerOperationDataDto.SendOrderRate = DecimalExtension.CalculateTargetComplete(sendOrderdetails.Value, addWechatdetails.Value);
            newCustomerOperationDataDto.SendOrderRateHealthValueSum = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueSum").Select(e => e.Rate).FirstOrDefault();
            newCustomerOperationDataDto.SendOrderRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【上门】
            AssistantNewCustomerOperationDataDetails visitdetails = new AssistantNewCustomerOperationDataDetails();
            //上门
            visitdetails.Key = "AddWeChat";
            visitdetails.Name = "上门量";
            visitdetails.Value = baseOrderPerformance.VisitNum;
            newCustomerOperationDataDto.newCustomerOperationDataDetails.Add(visitdetails);

            //上门率
            newCustomerOperationDataDto.ToHospitalRate = DecimalExtension.CalculateTargetComplete(visitdetails.Value, sendOrderdetails.Value);
            newCustomerOperationDataDto.ToHospitalRateHealthValueSum = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueSum").Select(e => e.Rate).FirstOrDefault();
            newCustomerOperationDataDto.ToHospitalRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【成交】
            AssistantNewCustomerOperationDataDetails dealdetails = new AssistantNewCustomerOperationDataDetails();
            //成交
            dealdetails.Key = "AddWeChat";
            dealdetails.Name = "成交量";
            dealdetails.Value = baseOrderPerformance.DealNum;
            newCustomerOperationDataDto.newCustomerOperationDataDetails.Add(dealdetails);

            //成交率
            newCustomerOperationDataDto.DealRate = DecimalExtension.CalculateTargetComplete(dealdetails.Value, visitdetails.Value);
            newCustomerOperationDataDto.DealRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "DealRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            newCustomerOperationDataDto.DealRateHealthValueSum = healthValueList.Where(e => e.Key == "DealRateHealthValueSum").Select(e => e.Rate).FirstOrDefault();
            #endregion

            //if (baseOrderPerformance.DealPrice.HasValue)
            //{
            //    //if (addCarddetails.Value != 0)
            //    //{
            //    //    //线索成交能效
            //    //    newCustomerOperationDataDto.FlowClueToDealPrice = Math.Round(baseOrderPerformance.DealPrice.Value / addCarddetails.Value, 2, MidpointRounding.AwayFromZero);
            //    //}
            //    if (consulationdetails.Value != 0)
            //        //分诊成交能效
            //        newCustomerOperationDataDto.AllocationConsulationToDealPrice = Math.Round(baseOrderPerformance.DealPrice.Value / consulationdetails.Value, 2, MidpointRounding.AwayFromZero);
            //    if (addWechatdetails.Value != 0)
            //        //加v成交能效
            //        newCustomerOperationDataDto.AddWeChatToDealPrice = Math.Round(baseOrderPerformance.DealPrice.Value / addWechatdetails.Value, 2, MidpointRounding.AwayFromZero);

            //    if (sendOrderdetails.Value != 0)
            //        //派单成交能效
            //        newCustomerOperationDataDto.SendOrderToDealPrice = Math.Round(baseOrderPerformance.DealPrice.Value / sendOrderdetails.Value, 2, MidpointRounding.AwayFromZero);
            //    if (visitdetails.Value != 0)
            //        //上门成交能效
            //        newCustomerOperationDataDto.VisitToDealPrice = Math.Round(baseOrderPerformance.DealPrice.Value / visitdetails.Value, 2, MidpointRounding.AwayFromZero);
            //    //成交能效
            //    if (dealdetails.Value != 0)
            //    {
            //        newCustomerOperationDataDto.DealToPrice = Math.Round(baseOrderPerformance.DealPrice.Value / dealdetails.Value, 2, MidpointRounding.AwayFromZero);
            //    }
            //}
            //else
            //{
            //    //线索成交能效
            //    //newCustomerOperationDataDto.FlowClueToDealPrice = 0.00M;
            //    //分诊成交能效
            //    newCustomerOperationDataDto.AllocationConsulationToDealPrice = 0.00M;
            //    //加v成交能效
            //    newCustomerOperationDataDto.AddWeChatToDealPrice = 0.00M;
            //    //派单成交能效
            //    newCustomerOperationDataDto.SendOrderToDealPrice = 0.00M;
            //    //上门成交能效
            //    newCustomerOperationDataDto.VisitToDealPrice = 0.00M;
            //    //成交能效
            //    newCustomerOperationDataDto.DealToPrice = 0.00M;
            //}

            amiyaOperationDataDto.NewCustomerData = newCustomerOperationDataDto;
            //老客数据
            var oldCustomerData = await contentPlateFormOrderService.GetAssistantOldCustomerBuyAgainByMonthAsync(selectDate.EndDate, query.IsEffectiveCustomerData, assistantIdList);
            oldCustomerOperationDataDto.TotalDealPeople = oldCustomerData.TotalDealCustomer;
            oldCustomerOperationDataDto.SecondDealPeople = oldCustomerData.SecondDealCustomer;
            oldCustomerOperationDataDto.ThirdDealPeople = oldCustomerData.ThirdDealCustomer;
            oldCustomerOperationDataDto.FourthDealCustomer = oldCustomerData.FourthDealCustomer;
            oldCustomerOperationDataDto.FifThOrMoreOrMoreDealCustomer = oldCustomerData.FifThOrMoreOrMoreDealCustomer;
            oldCustomerOperationDataDto.SecondDealCycle = oldCustomerData.SecondDealCycle;
            oldCustomerOperationDataDto.ThirdDealCycle = oldCustomerData.ThirdDealCycle;
            oldCustomerOperationDataDto.FourthDealCycle = oldCustomerData.FourthDealCycle;
            oldCustomerOperationDataDto.FifthDealCycle = oldCustomerData.FifthDealCycle;
            oldCustomerOperationDataDto.SecondTimeBuyRate = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.SecondDealPeople), Convert.ToDecimal(oldCustomerOperationDataDto.TotalDealPeople)).Value;
            oldCustomerOperationDataDto.SecondTimeBuyRateProportion = oldCustomerOperationDataDto.SecondTimeBuyRate;
            oldCustomerOperationDataDto.ThirdTimeBuyRate = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.ThirdDealPeople), Convert.ToDecimal(oldCustomerOperationDataDto.SecondDealPeople)).Value;
            oldCustomerOperationDataDto.ThirdTimeBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.ThirdDealPeople), Convert.ToDecimal(oldCustomerOperationDataDto.TotalDealPeople)).Value;
            oldCustomerOperationDataDto.FourthTimeBuyRate = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.FourthDealCustomer), Convert.ToDecimal(oldCustomerOperationDataDto.ThirdDealPeople)).Value;
            oldCustomerOperationDataDto.FourthTimeBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.FourthDealCustomer), Convert.ToDecimal(oldCustomerOperationDataDto.TotalDealPeople)).Value;
            oldCustomerOperationDataDto.FifthTimeOrMoreBuyRate = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.FifThOrMoreOrMoreDealCustomer), Convert.ToDecimal(oldCustomerOperationDataDto.FourthDealCustomer)).Value;
            oldCustomerOperationDataDto.FifthTimeOrMoreBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.FifThOrMoreOrMoreDealCustomer), Convert.ToDecimal(oldCustomerOperationDataDto.TotalDealPeople)).Value;
            oldCustomerOperationDataDto.BuyRate = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(oldCustomerOperationDataDto.FifThOrMoreOrMoreDealCustomer + oldCustomerOperationDataDto.FourthDealCustomer + oldCustomerOperationDataDto.ThirdDealPeople + oldCustomerOperationDataDto.SecondDealPeople), Convert.ToDecimal(oldCustomerOperationDataDto.TotalDealPeople)).Value;

            amiyaOperationDataDto.OldCustomerData = oldCustomerOperationDataDto;
            return amiyaOperationDataDto;
        }

        /// <summary>
        /// 获取助理业绩分析数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantPerformanceAnalysisDataDto> GetAssistantPerformanceAnalysisDataAsync(QueryAssistantPerformanceDto query)
        {
            AssistantPerformanceAnalysisDataDto result = new AssistantPerformanceAnalysisDataDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }

            //成交数据
            var order = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            //小黄车数据
            var shoppingCartRegistionData = await shoppingCartRegistrationService.GetPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            //总业绩
            var curTotalAchievementPrice = order.Sum(e => e.Price);

            var totalSendPhoneList = await _dalContentPlatformOrderSend.GetAll()
                .Where(e => e.IsMainHospital == true && e.SendDate >= selectDate.StartDate && e.SendDate < selectDate.EndDate)
                .Where(e => e.ContentPlatformOrder.IsSupportOrder ? e.ContentPlatformOrder.SupportEmpId == query.AssistantId : e.ContentPlatformOrder.BelongEmpId == query.AssistantId)
                .Select(e => new { OrderId = e.ContentPlatformOrderId, Phone = e.ContentPlatformOrder.Phone, ConsulationType = e.ContentPlatformOrder.ConsulationType }).ToListAsync();
            //当月订单
            var currentPhone = totalSendPhoneList.Where(e => shoppingCartRegistionData.Select(e => e.Phone).Contains(e.Phone)).ToList();
            var currentOrder = totalSendPhoneList.Where(e => shoppingCartRegistionData.Select(e => e.Phone).Contains(e.Phone)).Select(e => e.OrderId).ToList();
            //历史订单
            var historyPhone = totalSendPhoneList.Where(e => !shoppingCartRegistionData.Select(e => e.Phone).Contains(e.Phone)).Select(e => e.Phone).Distinct().ToList();
            var historyOrder = order.Select(e => e.ContentPlatFormOrderId).Where(e => !currentOrder.Contains(e)).Distinct().ToList();
            #region 客资分类

            var firstTypeCount = shoppingCartRegistionData.Where(e => e.EmergencyLevel == (int)EmergencyLevel.Important).Count();
            var secondTypeCount = shoppingCartRegistionData.Where(e => e.EmergencyLevel == (int)EmergencyLevel.Generally).Count();
            var thirdTypeCount = shoppingCartRegistionData.Where(e => e.EmergencyLevel == (int)EmergencyLevel.Ignorable).Count();
            var totalTypeCount = shoppingCartRegistionData.Count();

            var firstTypePhone = shoppingCartRegistionData.Where(e => e.EmergencyLevel == (int)EmergencyLevel.Important).Select(e => e.Phone).ToList();
            var secondTypePhone = shoppingCartRegistionData.Where(e => e.EmergencyLevel == (int)EmergencyLevel.Generally).Select(e => e.Phone).ToList();
            var thirdTypePhone = shoppingCartRegistionData.Where(e => e.EmergencyLevel == (int)EmergencyLevel.Ignorable).Select(e => e.Phone).ToList();

            var firstTypePerformance = order.Where(e => firstTypePhone.Contains(e.Phone)).Sum(e => e.Price);
            var secondTypePerformance = order.Where(e => secondTypePhone.Contains(e.Phone)).Sum(e => e.Price);
            var thirdTypePerformance = order.Where(e => thirdTypePhone.Contains(e.Phone)).Sum(e => e.Price);
            var totalPerformance = firstTypePerformance + secondTypePerformance + thirdTypePerformance;

            //人数
            result.TypeCount = new CustomerTypePerformanceDataDto();
            result.TypeCount.TotalCount = totalTypeCount;
            result.TypeCount.Data = new List<CustomerTypePerformanceDataItemDto>();
            result.TypeCount.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "一级线索", Value = firstTypeCount, Rate = DecimalExtension.CalculateTargetComplete(firstTypeCount, totalTypeCount).Value });
            result.TypeCount.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "二级线索", Value = secondTypeCount, Rate = DecimalExtension.CalculateTargetComplete(secondTypeCount, totalTypeCount).Value });
            result.TypeCount.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "三级线索", Value = thirdTypeCount, Rate = DecimalExtension.CalculateTargetComplete(thirdTypeCount, totalTypeCount).Value });

            //业绩
            result.TypePerformance = new CustomerTypePerformanceDataDto();
            result.TypePerformance.TotalCount = totalPerformance;
            result.TypePerformance.Data = new List<CustomerTypePerformanceDataItemDto>();
            result.TypePerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "一级线索", Value = DecimalExtension.ChangePriceToTenThousand(firstTypePerformance), Rate = DecimalExtension.CalculateTargetComplete(firstTypePerformance, totalPerformance).Value });
            result.TypePerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "二级线索", Value = DecimalExtension.ChangePriceToTenThousand(secondTypePerformance), Rate = DecimalExtension.CalculateTargetComplete(secondTypePerformance, totalPerformance).Value });
            result.TypePerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "三级线索", Value = DecimalExtension.ChangePriceToTenThousand(thirdTypePerformance), Rate = DecimalExtension.CalculateTargetComplete(thirdTypePerformance, totalPerformance).Value });


            #endregion

            #region 【新老客】
            var NewCount = order.Where(o => o.IsOldCustomer == false).Select(e => e.Phone).Distinct().Count();
            var OldCount = order.Where(o => o.IsOldCustomer == true).Select(e => e.Phone).Distinct().Count();
            var NewCustomerPerformance = order.Where(o => o.IsOldCustomer == false).Sum(e => e.Price);
            var OldCustomerPerformance = order.Where(o => o.IsOldCustomer == true).Sum(e => e.Price);

            AssistantOperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerData = new AssistantOperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            totalPerformanceNewOrOldCustomerData.TotalPerformanceNumber = ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceNewCustomerNumber = ChangePriceToTenThousand(NewCustomerPerformance);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(NewCustomerPerformance, curTotalAchievementPrice);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceOldCustomerNumber = ChangePriceToTenThousand(OldCustomerPerformance);
            totalPerformanceNewOrOldCustomerData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(OldCustomerPerformance, curTotalAchievementPrice);
            result.PerformanceNewCustonerOrNoData = totalPerformanceNewOrOldCustomerData;


            //人数
            AssistantOperationBoardGetNewOrOldCustomerCompareDataDetailsDto totalPerformanceNewOrOldCustomerNumData = new AssistantOperationBoardGetNewOrOldCustomerCompareDataDetailsDto();
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNewCustomerNumber = NewCount;
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceOldCustomerNumber = OldCount;
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNumber = NewCount + OldCount;
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNewCustomerRate = DecimalExtension.CalculateTargetComplete(NewCount, totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNumber.Value);
            totalPerformanceNewOrOldCustomerNumData.TotalPerformanceOldCustomerRate = DecimalExtension.CalculateTargetComplete(OldCount, totalPerformanceNewOrOldCustomerNumData.TotalPerformanceNumber.Value);
            result.CustomerDealData = totalPerformanceNewOrOldCustomerNumData;

            #endregion

            #region 【有效/潜在】

            AssistantOperationBoardGetIsEffictivePerformanceDto totalPerformanceIsEffictiveGroupData = new AssistantOperationBoardGetIsEffictivePerformanceDto();
            var curEffictive = order.Where(x => x.AddOrderPrice > 0).Sum(x => x.Price);
            var curNotEffictive = order.Where(x => x.AddOrderPrice == 0).Sum(x => x.Price);
            totalPerformanceIsEffictiveGroupData.TotalPerformanceNumber = ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceIsEffictiveGroupData.EffictivePerformanceNumber = ChangePriceToTenThousand(curEffictive);
            totalPerformanceIsEffictiveGroupData.EffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curEffictive, curTotalAchievementPrice);
            totalPerformanceIsEffictiveGroupData.NotEffictivePerformanceNumber = ChangePriceToTenThousand(curNotEffictive);
            totalPerformanceIsEffictiveGroupData.NotEffictivePerformanceRate = DecimalExtension.CalculateTargetComplete(curNotEffictive, curTotalAchievementPrice);
            result.PerformanceEffictiveOrNoData = totalPerformanceIsEffictiveGroupData;

            //人数
            var EffictiveCount = shoppingCartRegistionData.Where(e => e.AddPrice > 0).Count();
            var NotEffictiveCount = shoppingCartRegistionData.Where(e => e.AddPrice == 0).Count();
            AssistantOperationBoardIsEffictiveDataDto totalPerformanceIsEffictiveNumData = new AssistantOperationBoardIsEffictiveDataDto();
            totalPerformanceIsEffictiveNumData.EffictiveNumber = EffictiveCount;
            totalPerformanceIsEffictiveNumData.NotEffictiveNumber = NotEffictiveCount;
            totalPerformanceIsEffictiveNumData.TotalFlowRateNumber = EffictiveCount + NotEffictiveCount;
            totalPerformanceIsEffictiveNumData.EffictiveRate = DecimalExtension.CalculateTargetComplete(EffictiveCount, totalPerformanceIsEffictiveNumData.TotalFlowRateNumber.Value);
            totalPerformanceIsEffictiveNumData.NotEffictiveRate = DecimalExtension.CalculateTargetComplete(NotEffictiveCount, totalPerformanceIsEffictiveNumData.TotalFlowRateNumber.Value);
            result.DistributeConsulationData = totalPerformanceIsEffictiveNumData;
            #endregion
            #region 【当月/历史】

            AssistantOperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryGroupData = new AssistantOperationBoardGetIsHistoryPerformanceDto();
            var HistoryCount = order.Where(x => historyOrder.Contains(x.ContentPlatFormOrderId)).ToList();
            var curHistory = HistoryCount.Sum(x => x.Price);
            var ThisMonthCount = order.Where(x => currentOrder.Contains(x.ContentPlatFormOrderId)).ToList();
            var curThisMonth = ThisMonthCount.Sum(x => x.Price);
            totalPerformanceIsHistoryGroupData.TotalPerformanceNumber = ChangePriceToTenThousand(curTotalAchievementPrice);
            totalPerformanceIsHistoryGroupData.HistoryPerformanceNumber = ChangePriceToTenThousand(curHistory);
            totalPerformanceIsHistoryGroupData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(curHistory, curTotalAchievementPrice);
            totalPerformanceIsHistoryGroupData.ThisMonthPerformanceNumber = ChangePriceToTenThousand(curThisMonth);
            totalPerformanceIsHistoryGroupData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(curThisMonth, curTotalAchievementPrice);
            result.PerformanceHistoryOrNoData = totalPerformanceIsHistoryGroupData;

            //人数
            AssistantOperationBoardGetIsHistoryPerformanceDto totalPerformanceIsHistoryNumData = new AssistantOperationBoardGetIsHistoryPerformanceDto();
            //totalPerformanceIsHistoryNumData.ThisMonthPerformanceNumber = ThisMonthCount.Select(e => e.Phone).Distinct().Count();
            //totalPerformanceIsHistoryNumData.HistoryPerformanceNumber = HistoryCount.Select(e => e.Phone).Distinct().Count();
            totalPerformanceIsHistoryNumData.ThisMonthPerformanceNumber = currentPhone.Count();
            totalPerformanceIsHistoryNumData.HistoryPerformanceNumber = historyPhone.Count();
            totalPerformanceIsHistoryNumData.TotalPerformanceNumber = totalPerformanceIsHistoryNumData.ThisMonthPerformanceNumber + totalPerformanceIsHistoryNumData.HistoryPerformanceNumber;
            totalPerformanceIsHistoryNumData.ThisMonthPerformanceRate = DecimalExtension.CalculateTargetComplete(totalPerformanceIsHistoryNumData.ThisMonthPerformanceNumber.Value, totalPerformanceIsHistoryNumData.TotalPerformanceNumber.Value);
            totalPerformanceIsHistoryNumData.HistoryPerformanceRate = DecimalExtension.CalculateTargetComplete(totalPerformanceIsHistoryNumData.HistoryPerformanceNumber.Value, totalPerformanceIsHistoryNumData.TotalPerformanceNumber.Value);
            result.SendOrderData = totalPerformanceIsHistoryNumData;

            #endregion

            #region 面诊

            //派单
            var otherCount = totalSendPhoneList.Where(e => e.ConsulationType == (int)ContentPlateFormOrderConsultationType.OTHER).Select(e => e.Phone).Distinct().Count();
            //var unConsulationCount = totalSendPhoneList.Where(e => e.ConsulationType == (int)ContentPlateFormOrderConsultationType.UnConsulation).Select(e => e.Phone).Distinct().Count();
            var independentFollowUpCount = totalSendPhoneList.Where(e => e.ConsulationType == (int)ContentPlateFormOrderConsultationType.IndependentFollowUp).Select(e => e.Phone).Distinct().Count();
            var collaborationCount = totalSendPhoneList.Where(e => e.ConsulationType == (int)ContentPlateFormOrderConsultationType.Collaboration).Select(e => e.Phone).Distinct().Count();
            var voiceCount = totalSendPhoneList.Where(e => e.ConsulationType == (int)ContentPlateFormOrderConsultationType.Voice).Select(e => e.Phone).Distinct().Count();
            result.Consulation = new CustomerTypePerformanceDataDto();
            result.Consulation.TotalCount = totalSendPhoneList.Select(e => e.Phone).Distinct().Count();
            result.Consulation.Data = new List<CustomerTypePerformanceDataItemDto>();
            result.Consulation.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "其它", Value = otherCount, Rate = DecimalExtension.CalculateTargetComplete(otherCount, result.Consulation.TotalCount).Value });
            //result.Consulation.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "未面诊", Value = unConsulationCount, Rate = DecimalExtension.CalculateTargetComplete(unConsulationCount, result.Consulation.TotalCount).Value });
            result.Consulation.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "（助理）照片面诊", Value = independentFollowUpCount, Rate = DecimalExtension.CalculateTargetComplete(independentFollowUpCount, result.Consulation.TotalCount).Value });
            result.Consulation.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "（主播）视频面诊", Value = collaborationCount, Rate = DecimalExtension.CalculateTargetComplete(collaborationCount, result.Consulation.TotalCount).Value });
            result.Consulation.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "（主播）语音面诊", Value = voiceCount, Rate = DecimalExtension.CalculateTargetComplete(voiceCount, result.Consulation.TotalCount).Value });

            //业绩
            var otherPerformance = order.Where(e => e.ConsultationType == (int)ContentPlateFormOrderConsultationType.OTHER).Sum(x => x.Price);
            //var unConsulationPerformance = order.Where(e => e.ConsultationType == (int)ContentPlateFormOrderConsultationType.UnConsulation).Sum(x => x.Price);
            var independentFollowUpPerformance = order.Where(e => e.ConsultationType == (int)ContentPlateFormOrderConsultationType.IndependentFollowUp).Sum(x => x.Price);
            var collaborationPerformance = order.Where(e => e.ConsultationType == (int)ContentPlateFormOrderConsultationType.Collaboration).Sum(x => x.Price);
            var voicePerformance = order.Where(e => e.ConsultationType == (int)ContentPlateFormOrderConsultationType.Voice).Sum(x => x.Price);
            result.ConsulationPerformance = new CustomerTypePerformanceDataDto();
            result.ConsulationPerformance.TotalCount = DecimalExtension.ChangePriceToTenThousand(order.Sum(x => x.Price));
            result.ConsulationPerformance.Data = new List<CustomerTypePerformanceDataItemDto>();
            result.ConsulationPerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "其它", Value = DecimalExtension.ChangePriceToTenThousand(otherPerformance), Rate = DecimalExtension.CalculateTargetComplete(otherPerformance, result.ConsulationPerformance.TotalCount).Value });
            //result.ConsulationPerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "未面诊", Value = DecimalExtension.ChangePriceToTenThousand(unConsulationPerformance), Rate = DecimalExtension.CalculateTargetComplete(unConsulationPerformance, result.ConsulationPerformance.TotalCount).Value });
            result.ConsulationPerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "（助理）照片面诊", Value = DecimalExtension.ChangePriceToTenThousand(independentFollowUpPerformance), Rate = DecimalExtension.CalculateTargetComplete(independentFollowUpPerformance, result.ConsulationPerformance.TotalCount).Value });
            result.ConsulationPerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "（主播）视频面诊", Value = DecimalExtension.ChangePriceToTenThousand(collaborationPerformance), Rate = DecimalExtension.CalculateTargetComplete(collaborationPerformance, result.ConsulationPerformance.TotalCount).Value });
            result.ConsulationPerformance.Data.Add(new CustomerTypePerformanceDataItemDto { Key = "（主播）语音面诊", Value = DecimalExtension.ChangePriceToTenThousand(voicePerformance), Rate = DecimalExtension.CalculateTargetComplete(voicePerformance, result.ConsulationPerformance.TotalCount).Value });
            #endregion

            return result;
        }
        /// <summary>
        /// 获取助理机构业绩分析
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<AssistantHospitalPerformanceDto>> GetAssistantHospitalPerformanceDataAsync(QueryAssistantPerformanceDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }

            //成交数据
            var orderDealInfo = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);

            #region 机构业绩
            var hospitalInfo = await hospitalInfoService.GetHospitalNameListAsync(null, null);
            return orderDealInfo.GroupBy(x => x.LastDealHospitalId).Select(e => new AssistantHospitalPerformanceDto
            {
                Name = hospitalInfo.Where(h => h.Id == e.Key).Select(e => e.Name).FirstOrDefault(),
                NewCustomerPerformance = ChangePriceToTenThousand(orderDealInfo.Where(h => h.LastDealHospitalId == e.Key).Where(e => e.IsOldCustomer == false).Select(e => e.Price).Sum()),
                OldCustomerPerformance = ChangePriceToTenThousand(orderDealInfo.Where(h => h.LastDealHospitalId == e.Key).Where(e => e.IsOldCustomer == true).Select(e => e.Price).Sum()),
            }).ToList();
            #endregion
        }
        /// <summary>
        /// 获取助理机构线索分析
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantHospitalCluesDataDto> GetAssistantHospitalCluesDataAsync(QueryAssistantHospitalCluesDataDto query)
        {
            AssistantHospitalCluesDataDto result = new AssistantHospitalCluesDataDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            var shoppingCartRegistionData = await shoppingCartRegistrationService.GetPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            var currentSendPhoneList = shoppingCartRegistionData.Select(e => e.Phone).ToList();
            var totalSendPhoneList = await _dalContentPlatformOrderSend.GetAll()
                .Where(e => e.IsMainHospital == true && e.SendDate >= selectDate.StartDate && e.SendDate < selectDate.EndDate)
                .Where(e => e.ContentPlatformOrder.IsSupportOrder ? e.ContentPlatformOrder.SupportEmpId == query.AssistantId : e.ContentPlatformOrder.BelongEmpId == query.AssistantId)
                .Select(e => e.ContentPlatformOrder.Phone).ToListAsync();
            var historySendPhoneList = totalSendPhoneList.Where(e => !currentSendPhoneList.Contains(e)).ToList();
            var sendPhoneList = new List<string>();
            if (!query.CurrentMonth && !query.History)
            {
                query.CurrentMonth = true;
                query.History = true;
            }
            if (query.CurrentMonth && query.History)
            {
                sendPhoneList = currentSendPhoneList.Concat(historySendPhoneList).ToList();
            }
            else
            {
                if (query.CurrentMonth)
                {
                    sendPhoneList = currentSendPhoneList;
                }
                if (query.History)
                {
                    sendPhoneList = historySendPhoneList;
                }
            }
            #region 机构业绩
            var hospitalInfo = await hospitalInfoService.GetHospitalNameListAsync(null, null);
            var sendOrderHospitalList = await contentPlateFormOrderService.GetDealCountDataByPhoneListAsync(selectDate.StartDate, selectDate.EndDate, sendPhoneList);
            var hospitalIds = sendOrderHospitalList.Distinct().ToList();
            var toHospitalData = await contentPlatFormOrderDealInfoService.GeVisitAndDealNumByHospitalIdAndPhoneListAsync(hospitalIds, selectDate.StartDate, selectDate.EndDate, sendPhoneList);
            result.Items = hospitalIds.Select(e =>
            {
                AssistantCluesDataItemDto item = new AssistantCluesDataItemDto();
                item.Name = hospitalInfo.Where(h => h.Id == e).Select(e => e.Name).FirstOrDefault();
                item.SendOrderCount = sendOrderHospitalList.Where(x => x == e).Count();
                item.VisitCount = toHospitalData.Where(x => x.IsToHospital == true && x.LastDealHospitalId == e).Count();
                item.DealCount = toHospitalData.Where(x => x.IsDeal == true && x.LastDealHospitalId == e).Count();
                item.ToHospitalRate = DecimalExtension.CalculateTargetComplete(item.VisitCount, item.SendOrderCount).Value;
                item.DealRate = DecimalExtension.CalculateTargetComplete(item.DealCount, item.VisitCount).Value;
                return item;
            }).ToList();
            #endregion
            result.ToHospitalRate = DecimalExtension.CalculateTargetComplete(result.TotalVisitCount, result.TotalSendOrderCount).Value;
            result.DealRate = DecimalExtension.CalculateTargetComplete(result.TotalDealCount, result.TotalVisitCount).Value;
            return result;
        }
        /// <summary>
        /// 获取助理目标完成率和业绩占比
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssiatantTargetCompleteAndPerformanceRateDto> GetAssiatantTargetCompleteAndPerformanceRateDataAsync(QueryAssistantPerformanceDto query)
        {
            AssiatantTargetCompleteAndPerformanceRateDto result = new AssiatantTargetCompleteAndPerformanceRateDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var assistantIdAndNameList = (await amiyaEmployeeService.GetAllAssistantAsync()).ToList(); ;
            var assistantTarget = await dalEmployeePerformanceTarget.GetAll()
                .Where(e => e.Valid == true)
                .Where(e => e.BelongYear == selectDate.EndDate.Year && e.BelongMonth == selectDate.EndDate.Month)
                .Where(e => assistantIdAndNameList.Select(e => e.Id).Contains(e.EmployeeId))
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    Target = e.NewCustomerPerformanceTarget + e.OldCustomerPerformanceTarget,
                }).ToListAsync();
            var currentContentOrderList = await contentPlatFormOrderDealInfoService.GetPerformanceDetailByDateAndAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdAndNameList.Select(e => e.Id).ToList());
            var totalPerformance = currentContentOrderList.Sum(e => e.Price);
            foreach (var assistant in assistantIdAndNameList)
            {
                var sumPerformance = currentContentOrderList.Where(e => e.BelongEmployeeId == assistant.Id).Sum(e => e.Price);
                BaseKeyValueDto<string, decimal> targetItem = new BaseKeyValueDto<string, decimal>();
                var target = assistantTarget.Where(e => e.EmployeeId == assistant.Id).FirstOrDefault()?.Target ?? 0;
                targetItem.Key = assistant.Name;
                targetItem.Value = DecimalExtension.CalculateTargetComplete(sumPerformance, target).Value;
                result.TargetCompleteData.Add(targetItem);
                BaseKeyValueDto<string, decimal> rateItem = new BaseKeyValueDto<string, decimal>();
                rateItem.Key = assistant.Name;
                rateItem.Value = DecimalExtension.CalculateTargetComplete(sumPerformance, totalPerformance).Value;
                result.PerformanceRateData.Add(rateItem);
            }
            return result;
        }


        /// <summary>
        /// 获取助理分诊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantDistributeConsulationDto> GetAssistantDistributeConsulationDataAsync(QueryAssistantPerformanceDto query)
        {
            AssistantDistributeConsulationDto data = new AssistantDistributeConsulationDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            var todayData = await shoppingCartRegistrationService.GetDistributeConsulationTypeDataAsync(DateTime.Now.Date, DateTime.Now.AddDays(1).Date, assistantIdList);
            var currentData = await shoppingCartRegistrationService.GetDistributeConsulationTypeDataAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            var historyData = await shoppingCartRegistrationService.GetDistributeConsulationTypeDataAsync(selectDate.LastMonthStartDate, selectDate.LastMonthEndDate, assistantIdList);
            var lastYearData = await shoppingCartRegistrationService.GetDistributeConsulationTypeDataAsync(selectDate.LastYearThisMonthStartDate, selectDate.LastYearThisMonthEndDate, assistantIdList);

            DistributeConsulationItemDto firtData = new DistributeConsulationItemDto();
            firtData.CurrentDay = todayData.FirstType;
            firtData.Total = currentData.FirstType;
            firtData.YearOnYear = DecimalExtension.CalculateChain(firtData.Total, lastYearData.FirstType).Value;
            firtData.ChainRate = DecimalExtension.CalculateChain(firtData.Total, historyData.FirstType).Value;
            data.FirstType = firtData;

            DistributeConsulationItemDto secondData = new DistributeConsulationItemDto();
            secondData.CurrentDay = todayData.SecondType;
            secondData.Total = currentData.SecondType;
            secondData.YearOnYear = DecimalExtension.CalculateChain(secondData.Total, lastYearData.SecondType).Value;
            secondData.ChainRate = DecimalExtension.CalculateChain(secondData.Total, historyData.SecondType).Value;
            data.SecondType = secondData;

            DistributeConsulationItemDto thirdData = new DistributeConsulationItemDto();
            thirdData.CurrentDay = todayData.ThirdType;
            thirdData.Total = currentData.ThirdType;
            thirdData.YearOnYear = DecimalExtension.CalculateChain(thirdData.Total, lastYearData.ThirdType).Value;
            thirdData.ChainRate = DecimalExtension.CalculateChain(thirdData.Total, historyData.ThirdType).Value;
            data.ThirdType = thirdData;

            DistributeConsulationItemDto totalData = new DistributeConsulationItemDto();
            totalData.CurrentDay = todayData.TotalCount;
            totalData.Total = currentData.TotalCount;
            totalData.YearOnYear = DecimalExtension.CalculateChain(totalData.Total, lastYearData.TotalCount).Value;
            totalData.ChainRate = DecimalExtension.CalculateChain(totalData.Total, historyData.TotalCount).Value;
            data.TotalType = totalData;
            return data;
        }
        /// <summary>
        /// 获取助理有效潜在分诊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantEffOrPotDistributeConsulationDto> GetAssistantEffOrPotDistributeConsulationDataAsync(QueryAssistantPerformanceDto query)
        {
            AssistantEffOrPotDistributeConsulationDto data = new AssistantEffOrPotDistributeConsulationDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            var todayData = await shoppingCartRegistrationService.GetEffOrPotDistributeConsulationTypeDataAsync(DateTime.Now.Date, DateTime.Now.AddDays(1).Date, assistantIdList);
            var currentData = await shoppingCartRegistrationService.GetEffOrPotDistributeConsulationTypeDataAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            var historyData = await shoppingCartRegistrationService.GetEffOrPotDistributeConsulationTypeDataAsync(selectDate.LastMonthStartDate, selectDate.LastMonthEndDate, assistantIdList);
            var lastYearData = await shoppingCartRegistrationService.GetEffOrPotDistributeConsulationTypeDataAsync(selectDate.LastYearThisMonthStartDate, selectDate.LastYearThisMonthEndDate, assistantIdList);

            EffOrPotDistributeConsulationItemDto effective = new EffOrPotDistributeConsulationItemDto();
            effective.CurrentDay = todayData.Effective;
            effective.Total = currentData.Effective;
            effective.YearOnYear = DecimalExtension.CalculateChain(effective.Total, lastYearData.Effective).Value;
            effective.ChainRate = DecimalExtension.CalculateChain(effective.Total, historyData.Effective).Value;
            data.EffctiveData = effective;

            EffOrPotDistributeConsulationItemDto potential = new EffOrPotDistributeConsulationItemDto();
            potential.CurrentDay = todayData.Potential;
            potential.Total = currentData.Potential;
            potential.YearOnYear = DecimalExtension.CalculateChain(potential.Total, lastYearData.Potential).Value;
            potential.ChainRate = DecimalExtension.CalculateChain(potential.Total, historyData.Potential).Value;
            data.PotentialData = potential;

            EffOrPotDistributeConsulationItemDto totalData = new EffOrPotDistributeConsulationItemDto();
            totalData.CurrentDay = todayData.TotalCount;
            totalData.Total = currentData.TotalCount;
            totalData.YearOnYear = DecimalExtension.CalculateChain(totalData.Total, lastYearData.TotalCount).Value;
            totalData.ChainRate = DecimalExtension.CalculateChain(totalData.Total, historyData.TotalCount).Value;
            data.TotalData = totalData;
            return data;
        }

        /// <summary>
        /// 获取助理分诊折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssistantDistributeConsulationBrokenLineDto> GetAssistantDistributeConsulationBrokenLineDataAsync(QueryAssistantPerformanceDto query)
        {
            AssistantDistributeConsulationBrokenLineDto data = new AssistantDistributeConsulationBrokenLineDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            var baseData = await shoppingCartRegistrationService.GetDistributeConsulationTypeBrokenLineDataAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            var firstTypeData = baseData.Where(e => e.Value == (int)EmergencyLevel.Important)
                .GroupBy(e => e.Key)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key,
                    Performance = e.Count()
                }).ToList();
            var secondTypeData = baseData.Where(e => e.Value == (int)EmergencyLevel.Generally)
                .GroupBy(e => e.Key)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key,
                    Performance = e.Count()
                }).ToList();
            var thirdTypeData = baseData.Where(e => e.Value == (int)EmergencyLevel.Ignorable)
               .GroupBy(e => e.Key)
               .Select(e => new PerformanceBrokenLineListInfoDto
               {
                   date = e.Key,
                   Performance = e.Count()
               }).ToList();
            var totalTypeData = baseData
               .GroupBy(e => e.Key)
               .Select(e => new PerformanceBrokenLineListInfoDto
               {
                   date = e.Key,
                   Performance = e.Count()
               }).ToList();
            data.FirstType = this.FillDate(query.EndDate.Year, query.EndDate.Month, firstTypeData);
            data.SencondType = this.FillDate(query.EndDate.Year, query.EndDate.Month, secondTypeData);
            data.ThirdType = this.FillDate(query.EndDate.Year, query.EndDate.Month, thirdTypeData);
            data.TotalType = this.FillDate(query.EndDate.Year, query.EndDate.Month, totalTypeData);
            return data;
        }

        /// <summary>
        /// 助理有效/潜在分诊数据折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerServiceEffOrPotBrokenLineDataDto> GetAssistantEffOrPotBrokenLineDataAsync(QueryAssistantPerformanceDto query)
        {
            AdminCustomerServiceEffOrPotBrokenLineDataDto data = new AdminCustomerServiceEffOrPotBrokenLineDataDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantIdList = new List<int>();
            if (query.AssistantId.HasValue)
            {
                assistantIdList.Add(query.AssistantId.Value);
            }
            else
            {
                assistantIdList = (await amiyaEmployeeService.GetAllAssistantAsync()).Select(e => e.Id).ToList();
            }
            //var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var baseData = await _dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate && assistantIdList.Contains(e.AssignEmpId.Value) && e.AssignEmpId != null).Select(e => new BaseKeyValueDto<string, decimal>
            {
                Key = e.RecordDate.Date.Date.Day.ToString(),
                Value = e.Price,
            }).ToListAsync();
            var effticveData = baseData.Where(e => e.Value > 0)
                .GroupBy(e => e.Key)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key,
                    Performance = e.Count()
                }).ToList();
            var potentialData = baseData.Where(e => e.Value == 0)
                .GroupBy(e => e.Key)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key,
                    Performance = e.Count()
                }).ToList();

            var totalData = baseData
               .GroupBy(e => e.Key)
               .Select(e => new PerformanceBrokenLineListInfoDto
               {
                   date = e.Key,
                   Performance = e.Count()
               }).ToList();
            data.Effective = this.FillDate(query.EndDate.Year, query.EndDate.Month, effticveData);
            data.Potential = this.FillDate(query.EndDate.Year, query.EndDate.Month, potentialData);
            data.Total = this.FillDate(query.EndDate.Year, query.EndDate.Month, totalData);
            return data;
        }

        /// <summary>
        /// 获取助理转化周期柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<AssistantTransformCycleDataDto> GetAssistantTransformCycleDataAsync(QueryAssistantPerformanceDto query)
        {
            AssistantTransformCycleDataDto data = new AssistantTransformCycleDataDto();
            var seqDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            //var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantList = await amiyaEmployeeService.GetAssistantAsync();
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            #region 分诊派单
            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Where(e => e.IsMainHospital == true && e.SendDate >= seqDate.StartDate && e.SendDate < seqDate.EndDate)
                .Where(e => assistantIdList.Contains(e.Sender))
                .Select(e => new { Phone = e.ContentPlatformOrder.Phone, EmpId = e.Sender, SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Distinct().Select(e => e.Phone).ToList();
            var cartInfoList = _dalShoppingCartRegistration.GetAll().Where(e => e.IsReturnBackPrice == false && sendPhoneList.Contains(e.Phone))
                .Select(e => new
                {
                    Phone = e.Phone,
                    AddPrice = e.Price,
                    RecordDate = e.RecordDate
                }).ToList();
            var dataList = (from send in sendInfoList
                            join cart in cartInfoList
                            on send.Phone equals cart.Phone
                            select new
                            {
                                EmpId = send.EmpId,
                                AddPrice = cart.AddPrice,
                                IntervalDays = (send.SendDate - cart.RecordDate).Days
                            }).ToList();
            dataList.RemoveAll(e => e.IntervalDays < 0);
            //转化周期数据
            var res1 = dataList.GroupBy(e => e.EmpId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count());
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                assistantList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其它",
                DecimalExtension.CalAvg(resData.Sum(e => e.IntervalDays), resData.Count())
             );
            }).OrderBy(e => e.Value).ToList();
            res1.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            //当前助理转化周期
            var currentAssistanListCount = dataList.Where(e => e.EmpId == query.AssistantId.Value).Count();
            var endIndex = DecimalExtension.CalTakeCount(currentAssistanListCount);
            var currentAssistanList = dataList.Where(e => e.EmpId == query.AssistantId.Value).OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
            var currentEffectiveDays = currentAssistanList.Where(e => e.AddPrice > 0).Sum(e => e.IntervalDays);
            var currentEffectiveCount = currentAssistanList.Where(e => e.AddPrice > 0).Count();
            var currentPotionelDays = currentAssistanList.Where(e => e.AddPrice == 0).Sum(e => e.IntervalDays);
            var currentPotionelCount = currentAssistanList.Where(e => e.AddPrice == 0).Count();
            data.TotalSendCycle = DecimalExtension.CalAvg(currentAssistanList.Sum(e => e.IntervalDays), currentAssistanList.Count());
            data.EffectiveSendCycle = DecimalExtension.CalAvg(currentEffectiveDays, currentEffectiveCount);
            data.PotionelSendCycle = DecimalExtension.CalAvg(currentPotionelDays, currentPotionelCount);
            data.SendCycleData = res1;

            #endregion

            #region 分诊上门

            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                    .Where(e => (e.ContentPlatFormOrder.IsSupportOrder ? assistantIdList.Contains(e.ContentPlatFormOrder.SupportEmpId) : assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value)))
                    .Select(e => new
                    {
                        EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.ToHospitalDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).ToList();
            var cartInfoList2 = _dalShoppingCartRegistration.GetAll().Where(e => e.IsReturnBackPrice == false && dealPhoneList.Contains(e.Phone))
           .Select(e => new
           {
               Phone = e.Phone,
               AddPrice = e.Price,
               RecordDate = e.RecordDate
           }).ToList();
            var dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList2
                             on deal.Phone equals cart.Phone
                             select new
                             {
                                 EmpId = deal.EmpId,
                                 AddPrice = cart.AddPrice,
                                 IntervalDays = (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             }).ToList();
            dataList2.RemoveAll(e => e.IntervalDays < 0);
            //转化周期数据
            var res2 = dataList2.GroupBy(e => e.EmpId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count(), 0.6m);
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                assistantList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其它",
                DecimalExtension.CalAvg(resData.Sum(e => e.IntervalDays), resData.Count()));
            }).OrderBy(e => e.Value).ToList();
            res2.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            //当前助理转化周期
            var currentAssistanListCount2 = dataList2.Where(e => e.EmpId == query.AssistantId.Value).Count();
            var endIndex2 = DecimalExtension.CalTakeCount(currentAssistanListCount2, 0.6m);
            var currentAssistanList2 = dataList2.Where(e => e.EmpId == query.AssistantId.Value).OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex2);
            var currentEffectiveDays2 = currentAssistanList2.Where(e => e.AddPrice > 0).Sum(e => e.IntervalDays);
            var currentEffectiveCount2 = currentAssistanList2.Where(e => e.AddPrice > 0).Count();
            var currentPotionelDays2 = currentAssistanList2.Where(e => e.AddPrice == 0).Sum(e => e.IntervalDays);
            var currentPotionelCount2 = currentAssistanList2.Where(e => e.AddPrice == 0).Count();
            data.TotalToHospitalCycle = DecimalExtension.CalAvg(currentAssistanList2.Sum(e => e.IntervalDays), currentAssistanList2.Count());
            data.EffectiveToHospitalCycle = DecimalExtension.CalAvg(currentEffectiveDays2, currentEffectiveCount2);
            data.PotionelToHospitalCycle = DecimalExtension.CalAvg(currentPotionelDays2, currentPotionelCount2);
            data.ToHospitalCycleData = res2;


            #endregion

            #region 复购率

            var totalDealList = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.IsDeal == true && e.Price > 0 && e.ContentPlatFormOrder.DealAmount > 0)
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                }).Where(e => assistantIdList.Contains(e.EmpId.Value)).ToListAsync();
            var assistantTotalDealList = totalDealList.GroupBy(e => e.EmpId).Select(e => new
            {
                EmpId = e.Key,
                TotalDealCount = e.Select(e => e.Phone).Distinct().Count()
            }).ToList();
            var currentMonthDeal = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.IsToHospital == true && e.IsOldCustomer == true && e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate)
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                }).Where(e => assistantIdList.Contains(e.EmpId.Value)).ToListAsync();
            var assistantCurrentMonthDeal = currentMonthDeal.GroupBy(e => e.EmpId).Select(e => new
            {
                EmpId = e.Key,
                TotalDealCount = e.Select(e => e.Phone).Distinct().Count()
            }).ToList();
            var list = assistantCurrentMonthDeal.Select(e => e.EmpId).ToList();
            //当月复购率数据
            var res3 = (from total in assistantTotalDealList
                        join current in assistantCurrentMonthDeal
                        on total.EmpId equals current.EmpId
                        into tc
                        from r in tc.DefaultIfEmpty()
                        select new KeyValuePair<string, decimal>(
                            assistantList.Where(a => a.Id == total.EmpId).FirstOrDefault()?.Name ?? "其它",
                            r != null ? (total.TotalDealCount == 0 ? 0 : DecimalExtension.CalculateTargetComplete(r.TotalDealCount, total.TotalDealCount).Value) : 0)
                      ).OrderByDescending(e => e.Value).ToList();
            res3.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            data.OldCustomerRePurcheData = res3;

            #endregion

            return data;
        }


        /// <summary>
        /// 获取主播转化周期柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<AssistantTransformCycleDataDto> GetLiveAnchorTransformCycleDataAsync(QueryLiveAnchorPerformanceDto query)
        {
            AssistantTransformCycleDataDto data = new AssistantTransformCycleDataDto();
            var seqDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var liveanchorIds = new List<string>();
            var nameList = await liveAnchorBaseInfoService.GetAllLiveAnchorAsync(true);
            if (string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                liveanchorIds = nameList.Where(e => e.LiveAnchorName.Contains("刀刀") || e.LiveAnchorName.Contains("吉娜") || e.LiveAnchorName.Contains("璐璐")).Select(e => e.Id).ToList();
            }
            else
            {
                liveanchorIds = new List<string> { query.LiveAnchorBaseId };
            }
            #region 分诊派单
            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Include(x => x.ContentPlatformOrder).ThenInclude(x => x.LiveAnchor)
                .Where(e => e.IsMainHospital == true && e.SendDate >= seqDate.StartDate && e.SendDate < seqDate.EndDate)
                .Where(x => liveanchorIds.Count() == 0 || liveanchorIds.Contains(x.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new { Id = e.ContentPlatformOrder.Id, Phone = e.ContentPlatformOrder.Phone, LiveAnchorBaseId = (e.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId), SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Select(e => e.Phone).Distinct().ToList();
            var cartInfoList = _dalShoppingCartRegistration.GetAll().Where(e => e.IsReturnBackPrice == false && sendPhoneList.Contains(e.Phone))
                .Select(e => new
                {
                    Phone = e.Phone,
                    AddPrice = e.Price,
                    RecordDate = e.RecordDate
                }).ToList();
            var dataList = (from send in sendInfoList
                            join cart in cartInfoList
                            on send.Phone equals cart.Phone
                            select new
                            {
                                Id = send.Id,
                                LiveAnchorBaseId = send.LiveAnchorBaseId,
                                AddPrice = cart.AddPrice,
                                IntervalDays = (send.SendDate - cart.RecordDate).Days
                            }).ToList();
            dataList.RemoveAll(e => e.IntervalDays < 0);
            //转化周期数据
            var res1 = dataList.GroupBy(e => e.LiveAnchorBaseId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count());
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                nameList.Where(a => a.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其它",
                resData.Count() == 0 ? 0 : resData.Sum(e => e.IntervalDays) / (resData.Count())
             );
            }).OrderBy(e => e.Value).ToList();
            //当前主播转化周期
            var currentLiveAnchorListCount = dataList.Where(e => e.LiveAnchorBaseId == query.LiveAnchorBaseId).Count();
            var currentLiveAnchorList = dataList.Where(e => e.LiveAnchorBaseId == query.LiveAnchorBaseId).OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCount * 0.8));

            var currentLiveAnchorListCountAllData = dataList.Count();
            var currentLiveAnchorListAllData = dataList.OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCountAllData * 0.8));

            int currentEffectiveDays = 0;
            int currentEffectiveCount = 0;
            int currentPotionelDays = 0;
            int currentPotionelCount = 0;

            if (!string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                currentEffectiveDays = currentLiveAnchorList.Where(e => e.AddPrice > 0).Sum(e => e.IntervalDays);
                currentEffectiveCount = currentLiveAnchorList.Where(e => e.AddPrice > 0).Count();
                currentPotionelDays = currentLiveAnchorList.Where(e => e.AddPrice == 0).Sum(e => e.IntervalDays);
                currentPotionelCount = currentLiveAnchorList.Where(e => e.AddPrice == 0).Count();
            }
            else
            {
                currentEffectiveDays = currentLiveAnchorListAllData.Where(e => e.AddPrice > 0).Sum(e => e.IntervalDays);
                currentEffectiveCount = currentLiveAnchorListAllData.Where(e => e.AddPrice > 0).Count();
                currentPotionelDays = currentLiveAnchorListAllData.Where(e => e.AddPrice == 0).Sum(e => e.IntervalDays);
                currentPotionelCount = currentLiveAnchorListAllData.Where(e => e.AddPrice == 0).Count();
            }
            data.TotalSendCycle = DecimalExtension.CalAvg(currentEffectiveDays + currentPotionelDays, currentEffectiveCount + currentPotionelCount);
            data.EffectiveSendCycle = DecimalExtension.CalAvg(currentEffectiveDays, currentEffectiveCount);
            data.PotionelSendCycle = DecimalExtension.CalAvg(currentPotionelDays, currentPotionelCount);
            data.SendCycleData = res1;

            #endregion

            #region 分诊上门

            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).Where(e => e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                    .Where(x => liveanchorIds.Count() == 0 || liveanchorIds.Contains(x.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                    .Select(e => new
                    {
                        LiveanchorBaseId = e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.ToHospitalDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).Distinct().ToList();
            var cartInfoList2 = _dalShoppingCartRegistration.GetAll().Where(e => e.IsReturnBackPrice == false && dealPhoneList.Contains(e.Phone))
           .Select(e => new
           {
               Phone = e.Phone,
               AddPrice = e.Price,
               RecordDate = e.RecordDate
           }).ToList();
            var dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList2
                             on deal.Phone equals cart.Phone
                             select new
                             {
                                 LiveAnchorBaseId = deal.LiveanchorBaseId,
                                 AddPrice = cart.AddPrice,
                                 IntervalDays = (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             }).ToList();
            dataList2.RemoveAll(e => e.IntervalDays < 0);
            //转化周期数据
            var res2 = dataList2.GroupBy(e => e.LiveAnchorBaseId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count(), 0.6m);
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                nameList.Where(a => a.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其它",
                resData.Count() == 0 ? 0 : resData.Sum(e => e.IntervalDays) / resData.Count());
            }).OrderBy(e => e.Value).ToList();

            //当前主播转化周期
            var currentLiveAnchorListCount2 = dataList2.Where(e => e.LiveAnchorBaseId == query.LiveAnchorBaseId).Count();
            var currentLiveAnchorList2 = dataList2.Where(e => e.LiveAnchorBaseId == query.LiveAnchorBaseId).OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCount2 * 0.6));

            var currentLiveAnchorListCount2AllData = dataList2.Count();
            var currentLiveAnchorList2AllData = dataList2.OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCount2AllData * 0.6));

            int currentEffectiveDays2 = 0;
            int currentEffectiveCount2 = 0;
            int currentPotionelDays2 = 0;
            int currentPotionelCount2 = 0;

            if (!string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                currentEffectiveDays2 = currentLiveAnchorList2.Where(e => e.AddPrice > 0).Sum(e => e.IntervalDays);
                currentEffectiveCount2 = currentLiveAnchorList2.Where(e => e.AddPrice > 0).Count();
                currentPotionelDays2 = currentLiveAnchorList2.Where(e => e.AddPrice == 0).Sum(e => e.IntervalDays);
                currentPotionelCount2 = currentLiveAnchorList2.Where(e => e.AddPrice == 0).Count();
            }
            else
            {
                currentEffectiveDays2 = currentLiveAnchorList2AllData.Where(e => e.AddPrice > 0).Sum(e => e.IntervalDays);
                currentEffectiveCount2 = currentLiveAnchorList2AllData.Where(e => e.AddPrice > 0).Count();
                currentPotionelDays2 = currentLiveAnchorList2AllData.Where(e => e.AddPrice == 0).Sum(e => e.IntervalDays);
                currentPotionelCount2 = currentLiveAnchorList2AllData.Where(e => e.AddPrice == 0).Count();
            }
            data.TotalToHospitalCycle = DecimalExtension.CalAvg(currentEffectiveDays2 + currentPotionelDays2, currentEffectiveCount2 + currentPotionelCount2);
            data.EffectiveToHospitalCycle = DecimalExtension.CalAvg(currentEffectiveDays2, currentEffectiveCount2);
            data.PotionelToHospitalCycle = DecimalExtension.CalAvg(currentPotionelDays2, currentPotionelCount2);

            data.ToHospitalCycleData = res2;


            #endregion

            #region 复购率

            var totalDealList = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).Where(e => e.IsDeal == true && e.Price > 0 && e.ContentPlatFormOrder.DealAmount > 0 && liveanchorIds.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    LiveAnchorBaseId = e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                }).ToListAsync();

            var liveAnchorTotalDealList = totalDealList.GroupBy(e => e.LiveAnchorBaseId).Select(e => new
            {
                LiveAnchorBaseId = e.Key,
                TotalDealCount = e.Select(e => e.Phone).Distinct().Count()
            }).ToList();
            var currentMonthDeal = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).Where(e => e.IsToHospital == true && e.IsOldCustomer == true && e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate && liveanchorIds.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    LiveAnchorBaseId = e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                }).ToListAsync();
            var liveAnchorCurrentMonthDeal = currentMonthDeal.GroupBy(e => e.LiveAnchorBaseId).Select(e => new
            {
                LiveAnchorBaseId = e.Key,
                TotalDealCount = e.Select(e => e.Phone).Distinct().Count()
            }).ToList();
            var list = liveAnchorCurrentMonthDeal.Select(e => e.LiveAnchorBaseId).ToList();
            //当月复购率数据
            var res3 = (from total in liveAnchorTotalDealList
                        join current in liveAnchorCurrentMonthDeal
                        on total.LiveAnchorBaseId equals current.LiveAnchorBaseId
                        into tc
                        from r in tc.DefaultIfEmpty()
                        select new KeyValuePair<string, decimal>(
                            nameList.Where(a => a.Id == total.LiveAnchorBaseId).FirstOrDefault()?.LiveAnchorName ?? "其它",
                            r != null ? (total.TotalDealCount == 0 ? 0 : DecimalExtension.CalculateTargetComplete(r.TotalDealCount, total.TotalDealCount).Value) : 0)
                      ).OrderByDescending(e => e.Value).ToList();
            List<KeyValuePair<string, decimal>> resultData3 = new List<KeyValuePair<string, decimal>>();
            data.OldCustomerRePurcheData = res3;

            #endregion

            return data;
        }

        #endregion


        #region 行政客服运营看板

        /// <summary>
        /// 组客资数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerServiceCustomerTypeDto> GetAdminCustomerServiceCustomerTypeDataAsync(QueryAssistantPerformanceDto query)
        {

            AdminCustomerServiceCustomerTypeDto data = new AdminCustomerServiceCustomerTypeDto();
            var seqDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var currentData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeDataAsync(seqDate.StartDate, seqDate.EndDate, assistantList.Select(e => e.Id).ToList());
            var lastMonthData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeDataAsync(seqDate.LastMonthStartDate, seqDate.LastMonthEndDate, assistantList.Select(e => e.Id).ToList());
            var latYearData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeDataAsync(seqDate.LastYearThisMonthStartDate, seqDate.LastYearThisMonthEndDate, assistantList.Select(e => e.Id).ToList());
            data.FirstTypeTotal = currentData.FirstType;
            data.FirstTypeChainRate = DecimalExtension.CalculateChain(data.FirstTypeTotal, lastMonthData.FirstType).Value;
            data.FirstTypeYearOnYear = DecimalExtension.CalculateChain(data.FirstTypeTotal, latYearData.FirstType).Value;

            data.SecondTypeTotal = currentData.SecondType;
            data.SecondTypeChainRate = DecimalExtension.CalculateChain(data.SecondTypeTotal, lastMonthData.SecondType).Value;
            data.SecondTypeYearOnYear = DecimalExtension.CalculateChain(data.SecondTypeTotal, latYearData.SecondType).Value;

            data.ThirdTypeTotal = currentData.ThirdType;
            data.ThirdTypeChainRate = DecimalExtension.CalculateChain(data.ThirdTypeTotal, lastMonthData.ThirdType).Value;
            data.ThirdTypeYearOnYear = DecimalExtension.CalculateChain(data.ThirdTypeTotal, latYearData.ThirdType).Value;

            data.TotalTypeTotal = data.FirstTypeTotal + data.SecondTypeTotal + data.ThirdTypeTotal;
            data.TotalTypeChainRate = DecimalExtension.CalculateChain(data.TotalTypeTotal, lastMonthData.FirstType + lastMonthData.SecondType + lastMonthData.ThirdType).Value;
            data.TotalTypeYearOnYear = DecimalExtension.CalculateChain(data.TotalTypeTotal, latYearData.FirstType + latYearData.SecondType + latYearData.ThirdType).Value;
            return data;
        }

        /// <summary>
        /// 个人加v后数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerServiceCustomerTypeDto> GetAdminCustomerServiceCustomerTypeAddWechatDataAsync(QueryAssistantPerformanceDto query)
        {
            AdminCustomerServiceCustomerTypeDto data = new AdminCustomerServiceCustomerTypeDto();
            var seqDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var currentData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeDataAsync(seqDate.StartDate, seqDate.EndDate, new List<int> { query.AssistantId.Value }, null);
            var lastMonthData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeDataAsync(seqDate.LastMonthStartDate, seqDate.LastMonthEndDate, new List<int> { query.AssistantId.Value }, null);
            var latYearData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeDataAsync(seqDate.LastYearThisMonthStartDate, seqDate.LastYearThisMonthEndDate, new List<int> { query.AssistantId.Value }, null);
            data.FirstTypeTotal = currentData.FirstType;
            data.FirstTypeChainRate = DecimalExtension.CalculateChain(data.FirstTypeTotal, lastMonthData.FirstType).Value;
            data.FirstTypeYearOnYear = DecimalExtension.CalculateChain(data.FirstTypeTotal, latYearData.FirstType).Value;

            data.SecondTypeTotal = currentData.SecondType;
            data.SecondTypeChainRate = DecimalExtension.CalculateChain(data.SecondTypeTotal, lastMonthData.SecondType).Value;
            data.SecondTypeYearOnYear = DecimalExtension.CalculateChain(data.SecondTypeTotal, latYearData.SecondType).Value;

            data.ThirdTypeTotal = currentData.ThirdType;
            data.ThirdTypeChainRate = DecimalExtension.CalculateChain(data.ThirdTypeTotal, lastMonthData.ThirdType).Value;
            data.ThirdTypeYearOnYear = DecimalExtension.CalculateChain(data.ThirdTypeTotal, latYearData.ThirdType).Value;

            data.TotalTypeTotal = data.FirstTypeTotal + data.SecondTypeTotal + data.ThirdTypeTotal;
            data.TotalTypeChainRate = DecimalExtension.CalculateChain(data.TotalTypeTotal, lastMonthData.FirstType + lastMonthData.SecondType + lastMonthData.ThirdType).Value;
            data.TotalTypeYearOnYear = DecimalExtension.CalculateChain(data.TotalTypeTotal, latYearData.FirstType + latYearData.SecondType + latYearData.ThirdType).Value;

            return data;
        }

        /// <summary>
        /// 客资折线图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerServiceCustomerTypeBrokenLineDataDto> GetAdminCustomerServiceCustomerTypeBrokenLineDataAsync(QueryAssistantPerformanceDto query)
        {
            AdminCustomerServiceCustomerTypeBrokenLineDataDto data = new AdminCustomerServiceCustomerTypeBrokenLineDataDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var baseData = await shoppingCartRegistrationService.GetAdminCustomerDistributeConsulationTypeBrokenLineDataAsync(selectDate.StartDate, selectDate.EndDate, assistantList.Select(e => e.Id).ToList());
            var firstTypeData = baseData.Where(e => e.Value == (int)EmergencyLevel.Important)
                .GroupBy(e => e.Key)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key,
                    Performance = e.Count()
                }).ToList();
            var secondTypeData = baseData.Where(e => e.Value == (int)EmergencyLevel.Generally)
                .GroupBy(e => e.Key)
                .Select(e => new PerformanceBrokenLineListInfoDto
                {
                    date = e.Key,
                    Performance = e.Count()
                }).ToList();
            var thirdTypeData = baseData.Where(e => e.Value == (int)EmergencyLevel.Ignorable)
               .GroupBy(e => e.Key)
               .Select(e => new PerformanceBrokenLineListInfoDto
               {
                   date = e.Key,
                   Performance = e.Count()
               }).ToList();
            var totalTypeData = baseData
               .GroupBy(e => e.Key)
               .Select(e => new PerformanceBrokenLineListInfoDto
               {
                   date = e.Key,
                   Performance = e.Count()
               }).ToList();
            data.FirstType = this.FillDate(query.EndDate.Year, query.EndDate.Month, firstTypeData);
            data.SencondType = this.FillDate(query.EndDate.Year, query.EndDate.Month, secondTypeData);
            data.ThirdType = this.FillDate(query.EndDate.Year, query.EndDate.Month, thirdTypeData);
            data.TotalType = this.FillDate(query.EndDate.Year, query.EndDate.Month, totalTypeData);
            return data;
        }



        /// <summary>
        /// 获取行政客服漏斗图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerFilterDataDto> GetAdminCustomerFilterDataAsync(QueryAssistantPerformanceDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            AdminCustomerFilterDataDto filterData = new AdminCustomerFilterDataDto();
            AdminCustomerFilterDataItemDto groupDataDto = new AdminCustomerFilterDataItemDto();
            groupDataDto.DataList = new List<AdminCustomerFilterDetailDataDto>();
            AdminCustomerFilterDataItemDto addWechatDataDto = new AdminCustomerFilterDataItemDto();
            addWechatDataDto.DataList = new List<AdminCustomerFilterDetailDataDto>();
            var healthValueList = await _healthValueService.GetValidListAsync();
            #region【小黄车数据】
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            //组小黄车数据
            var baseBusinessPerformance = await shoppingCartRegistrationService.GetAdminCustomerShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList);
            //个人小黄车数据
            var assisatntBusinessPerformance = await shoppingCartRegistrationService.GetAdminCustomerShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, new List<int> { query.AssistantId.Value });
            #endregion


            #region 组数据
            #region 【分诊】
            var allOrderPerformance = await contentPlateFormOrderService.GetAdminCustomerOrderSendAndDealDataByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, baseBusinessPerformance.Select(e => e.Phone).ToList());
            //分诊
            AdminCustomerFilterDetailDataDto consulationdetails = new AdminCustomerFilterDetailDataDto();
            consulationdetails.Key = "Consulation";
            consulationdetails.Name = "分诊量";
            consulationdetails.Value = baseBusinessPerformance.Where(x => x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            groupDataDto.DataList.Add(consulationdetails);
            #endregion

            #region 【加v】
            AdminCustomerFilterDetailDataDto addWechatdetails = new AdminCustomerFilterDetailDataDto();
            //加v
            addWechatdetails.Key = "AddWeChat";
            addWechatdetails.Name = "加v量";
            addWechatdetails.Value = baseBusinessPerformance.Where(x => x.IsAddWeChat == true && x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            groupDataDto.DataList.Add(addWechatdetails);

            //加v率
            groupDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails.Value, consulationdetails.Value);
            groupDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【派单】
            AdminCustomerFilterDetailDataDto sendOrderdetails = new AdminCustomerFilterDetailDataDto();
            //派单
            sendOrderdetails.Key = "SendOrder";
            sendOrderdetails.Name = "派单量";
            sendOrderdetails.Value = allOrderPerformance.SendOrderNum;
            groupDataDto.DataList.Add(sendOrderdetails);

            //派单率
            groupDataDto.SendOrderRate = DecimalExtension.CalculateTargetComplete(sendOrderdetails.Value, addWechatdetails.Value);
            groupDataDto.SendOrderRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【上门】
            AdminCustomerFilterDetailDataDto visitdetails = new AdminCustomerFilterDetailDataDto();
            //上门
            visitdetails.Key = "ToHospital";
            visitdetails.Name = "上门量";
            visitdetails.Value = allOrderPerformance.VisitNum;
            groupDataDto.DataList.Add(visitdetails);

            //上门率
            groupDataDto.ToHospitalRate = DecimalExtension.CalculateTargetComplete(visitdetails.Value, sendOrderdetails.Value);
            groupDataDto.ToHospitalRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion
            filterData.GroupData = groupDataDto;
            #endregion

            #region 个人

            #region 【分诊】
            var phoneList = assisatntBusinessPerformance.Where(e => e.AssignEmpId.HasValue && e.AssignEmpId != 0).Select(e => e.Phone).ToList();
            var addWechatOrderPerformance = await contentPlateFormOrderService.GetAdminCustomerOrderSendAndDealDataByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, phoneList);
            //分诊
            AdminCustomerFilterDetailDataDto consulationdetails2 = new AdminCustomerFilterDetailDataDto();
            consulationdetails2.Key = "Consulation";
            consulationdetails2.Name = "分诊量";
            consulationdetails2.Value = assisatntBusinessPerformance.Where(x => x.AssignEmpId != null && x.IsReturnBackPrice == false).Count();
            addWechatDataDto.DataList.Add(consulationdetails2);
            #endregion

            #region 【加v】
            AdminCustomerFilterDetailDataDto addWechatdetails2 = new AdminCustomerFilterDetailDataDto();
            //加v
            addWechatdetails2.Key = "AddWeChat";
            addWechatdetails2.Name = "加v量";
            addWechatdetails2.Value = assisatntBusinessPerformance.Where(x => x.IsAddWeChat == true && x.AssignEmpId != null && x.IsReturnBackPrice == false).Count();
            addWechatDataDto.DataList.Add(addWechatdetails2);

            //加v率
            addWechatDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails2.Value, consulationdetails2.Value);
            addWechatDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【派单】
            AdminCustomerFilterDetailDataDto sendOrderdetails2 = new AdminCustomerFilterDetailDataDto();
            //派单
            sendOrderdetails2.Key = "SendOrder";
            sendOrderdetails2.Name = "派单量";
            sendOrderdetails2.Value = addWechatOrderPerformance.SendOrderNum;
            addWechatDataDto.DataList.Add(sendOrderdetails2);

            //派单率
            addWechatDataDto.SendOrderRate = DecimalExtension.CalculateTargetComplete(sendOrderdetails2.Value, addWechatdetails2.Value);
            addWechatDataDto.SendOrderRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【上门】
            AdminCustomerFilterDetailDataDto visitdetails2 = new AdminCustomerFilterDetailDataDto();
            //上门
            visitdetails2.Key = "ToHospital";
            visitdetails2.Name = "上门量";
            visitdetails2.Value = addWechatOrderPerformance.VisitNum;
            addWechatDataDto.DataList.Add(visitdetails2);

            //上门率
            addWechatDataDto.ToHospitalRate = DecimalExtension.CalculateTargetComplete(visitdetails2.Value, sendOrderdetails2.Value);
            addWechatDataDto.ToHospitalRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion
            filterData.AddwechatData = addWechatDataDto;

            #endregion

            #region 分诊派单转化周期
            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Where(e => e.IsMainHospital == true && e.SendDate >= selectDate.StartDate && e.SendDate < selectDate.EndDate)
                .Where(e => assistantIdList.Contains(e.Sender))
                .Select(e => new { Phone = e.ContentPlatformOrder.Phone, EmpId = e.Sender, SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Distinct().Select(e => e.Phone).ToList();
            var cartInfoList = _dalShoppingCartRegistration.GetAll().Where(e => e.IsReturnBackPrice == false && sendPhoneList.Contains(e.Phone))
                .Select(e => new
                {
                    CreateBy = e.CreateBy,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate
                }).ToList();
            var dataList = (from send in sendInfoList
                            join cart in cartInfoList
                            on send.Phone equals cart.Phone
                            select new
                            {
                                CreateBy = cart.CreateBy,
                                EmpId = send.EmpId,
                                IntervalDays = (send.SendDate - cart.RecordDate).Days
                            }).ToList();
            dataList.RemoveAll(e => e.IntervalDays < 0);

            //当前组

            var assistanListCount = dataList.Count();
            var endIndex1 = DecimalExtension.CalTakeCount(assistanListCount);
            var assistanList = dataList.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex1);
            groupDataDto.SendCycle = DecimalExtension.CalAvg(assistanList.Sum(e => e.IntervalDays), assistanList.Count());

            //当前助理转化周期
            var currentAssistanListCount = dataList.Where(e => e.CreateBy == query.AssistantId.Value).Count();
            var endIndex2 = DecimalExtension.CalTakeCount(currentAssistanListCount);
            var currentAssistanList = dataList.Where(e => e.CreateBy == query.AssistantId.Value).OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex2);
            addWechatDataDto.SendCycle = DecimalExtension.CalAvg(currentAssistanList.Sum(e => e.IntervalDays), currentAssistanList.Count());

            #endregion

            #region 分诊上门

            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                    .Where(e => (e.ContentPlatFormOrder.IsSupportOrder ? assistantIdList.Contains(e.ContentPlatFormOrder.SupportEmpId) : assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value)))
                    .Select(e => new
                    {
                        EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.ToHospitalDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).ToList();
            var cartInfoList2 = _dalShoppingCartRegistration.GetAll().Where(e => e.IsReturnBackPrice == false && dealPhoneList.Contains(e.Phone))
           .Select(e => new
           {
               CreateBy = e.CreateBy,
               Phone = e.Phone,
               RecordDate = e.RecordDate
           }).ToList();
            var dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList2
                             on deal.Phone equals cart.Phone
                             select new
                             {
                                 CreateBy = cart.CreateBy,
                                 EmpId = deal.EmpId,
                                 IntervalDays = (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             }).ToList();
            dataList2.RemoveAll(e => e.IntervalDays < 0);
            //当前组
            var assistanListCount2 = dataList2.Count();
            var endIndex3 = DecimalExtension.CalTakeCount(assistanListCount2);
            var assistanList2 = dataList2.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex3).ToList();
            groupDataDto.HospitalCycle = DecimalExtension.CalAvg(assistanList2.Sum(e => e.IntervalDays), assistanList2.Count());

            //当前助理转化周期
            var currentAssistanListCount2 = dataList2.Where(e => e.CreateBy == query.AssistantId.Value).Count();
            var endIndex4 = DecimalExtension.CalTakeCount(currentAssistanListCount2, 0.6m);
            var currentAssistanList2 = dataList2.Where(e => e.CreateBy == query.AssistantId.Value).OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex4);
            addWechatDataDto.HospitalCycle = DecimalExtension.CalAvg(currentAssistanList2.Sum(e => e.IntervalDays), currentAssistanList2.Count());


            #endregion



            return filterData;
        }

        /// <summary>
        /// 获取行政客服饼状图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerAnalysisDataDto> GetAdminCustomerAnalysisDataAsync(QueryAssistantPerformanceDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var baseData = await shoppingCartRegistrationService.GetAdminCustomerShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantList.Select(e => e.Id).ToList());

            AdminCustomerAnalysisDataDto data = new AdminCustomerAnalysisDataDto();
            data.DistributeConsulationDataList = new List<Item>();
            data.DistributeConsulationAddWechatDataList = new List<Item>();
            data.EffAndPotDataList = new List<Item>();
            data.EffAndPotAddWechatDataList = new List<Item>();
            var beforeLiveCount = baseData.Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore).Count();
            var livingCount = baseData.Where(e => e.BelongChannel == (int)BelongChannel.Living).Count();
            var afterLiveCount = baseData.Where(e => e.BelongChannel == (int)BelongChannel.LiveAfter).Count();
            var totalLiveCount = beforeLiveCount + livingCount + afterLiveCount;
            var beforeLiveAddWechatCount = baseData.Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore && e.IsAddWeChat == true).Count();
            var livingAddWechatCount = baseData.Where(e => e.BelongChannel == (int)BelongChannel.Living && e.IsAddWeChat == true).Count();
            var afterLiveAddWechatCount = baseData.Where(e => e.BelongChannel == (int)BelongChannel.LiveAfter && e.IsAddWeChat == true).Count();
            var potenialCount = baseData.Where(e => e.Price == 0).Count();
            var effictiveCount = baseData.Where(e => e.Price > 0).Count();
            var totalCount = potenialCount + effictiveCount;
            var potenialAddWechatCount = baseData.Where(e => e.Price == 0 && e.IsAddWeChat == true).Count();
            var effictiveAddWechatCount = baseData.Where(e => e.Price > 0 && e.IsAddWeChat == true).Count();

            data.DistributeConsulationDataList.Add(new Item { Name = "直播前", Value = beforeLiveCount, Rate = DecimalExtension.CalculateTargetComplete(beforeLiveCount, totalLiveCount).Value });
            data.DistributeConsulationDataList.Add(new Item { Name = "直播中", Value = livingCount, Rate = DecimalExtension.CalculateTargetComplete(livingCount, totalLiveCount).Value });
            data.DistributeConsulationDataList.Add(new Item { Name = "直播后", Value = afterLiveCount, Rate = DecimalExtension.CalculateTargetComplete(afterLiveCount, totalLiveCount).Value });

            data.DistributeConsulationAddWechatDataList.Add(new Item { Name = "直播前", Value = beforeLiveAddWechatCount, Rate = DecimalExtension.CalculateTargetComplete(beforeLiveAddWechatCount, beforeLiveCount).Value });
            data.DistributeConsulationAddWechatDataList.Add(new Item { Name = "直播中", Value = livingAddWechatCount, Rate = DecimalExtension.CalculateTargetComplete(livingAddWechatCount, livingCount).Value });
            data.DistributeConsulationAddWechatDataList.Add(new Item { Name = "直播后", Value = afterLiveAddWechatCount, Rate = DecimalExtension.CalculateTargetComplete(afterLiveAddWechatCount, afterLiveCount).Value });

            data.EffAndPotDataList.Add(new Item { Name = "有效客资", Value = effictiveCount, Rate = DecimalExtension.CalculateTargetComplete(effictiveCount, totalCount).Value });
            data.EffAndPotDataList.Add(new Item { Name = "潜在客资", Value = potenialCount, Rate = DecimalExtension.CalculateTargetComplete(potenialCount, totalCount).Value });

            data.EffAndPotAddWechatDataList.Add(new Item { Name = "有效客资", Value = effictiveAddWechatCount, Rate = DecimalExtension.CalculateTargetComplete(effictiveAddWechatCount, effictiveCount).Value });
            data.EffAndPotAddWechatDataList.Add(new Item { Name = "潜在客资", Value = potenialAddWechatCount, Rate = DecimalExtension.CalculateTargetComplete(potenialAddWechatCount, potenialCount).Value });

            return data;

        }

        /// <summary>
        /// 获取当前行政客服分诊加v柱状图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AdminCustomerAssistantDisAndAddVDataDto> GetAdminCustomerAssistantDisAndAddVDataAsync(QueryAssistantPerformanceDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var baseData = await shoppingCartRegistrationService.GetAdminCustomerShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantList.Select(e => e.Id).ToList());
            var employeeList = await amiyaEmployeeService.GetAllAssistantAsync();

            AdminCustomerAssistantDisAndAddVDataDto data = new AdminCustomerAssistantDisAndAddVDataDto();
            var dataList = baseData.GroupBy(e => e.AssignEmpId).Select(e => new
            {
                Name = employeeList.Where(c => c.Id == e.Key).FirstOrDefault()?.Name ?? "其它",
                BeforeLiveCount = e.Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore).Count(),
                LivingCount = e.Where(e => e.BelongChannel == (int)BelongChannel.Living).Count(),
                AfterLiveCount = e.Where(e => e.BelongChannel == (int)BelongChannel.LiveAfter).Count(),
                BeforeLiveAddWechatCount = e.Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore && e.IsAddWeChat == true).Count(),
                LivingAddWechatCount = e.Where(e => e.BelongChannel == (int)BelongChannel.Living && e.IsAddWeChat == true).Count(),
                AfterLiveAddWechatCount = e.Where(e => e.BelongChannel == (int)BelongChannel.LiveAfter && e.IsAddWeChat == true).Count(),
            }).ToList();
            data.AssistantDistributeData = dataList.Select(e => new DataItemDto { Name = e.Name, Value = e.BeforeLiveCount + e.LivingCount + e.AfterLiveCount }).OrderByDescending(e => e.Value).ToList();
            data.AssistantDistributeDataDetail = dataList.Select(e => new DataDetailItemDto { Name = e.Name, BeforeLiveValue = e.BeforeLiveCount, LivingValue = e.LivingCount, AfterLiveValue = e.AfterLiveCount }).OrderByDescending(e => e.BeforeLiveValue + e.LivingValue + e.AfterLiveValue).ToList();
            data.AssistantAddWechatData = dataList.Select(e => new DataItemDto { Name = e.Name, Value = DecimalExtension.CalculateTargetComplete(e.BeforeLiveAddWechatCount + e.LivingAddWechatCount + e.AfterLiveAddWechatCount, e.BeforeLiveCount + e.LivingCount + e.AfterLiveCount).Value }).OrderByDescending(e => e.Value).ToList();
            data.AssistantAddWechatDataDetail = dataList.Select(e => new DataDetailItemDto
            {
                Name = e.Name,
                BeforeLiveValue = DecimalExtension.CalculateTargetComplete(e.BeforeLiveAddWechatCount, e.BeforeLiveCount).Value,
                LivingValue = DecimalExtension.CalculateTargetComplete(e.LivingAddWechatCount, e.LivingCount).Value,
                AfterLiveValue = DecimalExtension.CalculateTargetComplete(e.AfterLiveAddWechatCount, e.AfterLiveCount).Value,
            }).OrderByDescending(e => e.BeforeLiveValue + e.LivingValue + e.AfterLiveValue).ToList();
            return data;
        }

        #endregion


        #region 直播前数据运营看板
        /// <summary>
        /// 获取直播前客资和业绩数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveClueAndPerformanceDataDto> GetBeforeLiveClueAndPerformanceDataAsync(QueryBeforeLiveDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var basePhoneList = _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false)
                .Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate)
                .Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.BaseLiveAnchorId == info.LiveAnchorBaseId)
                .Select(e => new { e.CreateBy, e.Phone,e.RecordDate })
                .ToList();
            
            var phoneList = basePhoneList.Select(e => e.Phone).ToList();
            var myPhoneList = basePhoneList.Where(e => e.CreateBy == query.AssistantId.Value).Select(e => e.Phone).ToList();
            var performanceData = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => e.IsDeal == true)
                .Where(e => phoneList.Contains(e.ContentPlatFormOrder.Phone))
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    Price = e.Price,
                    CreateDate=e.CreateDate
                })
                .ToListAsync();
            BeforeLiveClueAndPerformanceDataDto beforeLiveClueAndPerformanceData = new BeforeLiveClueAndPerformanceDataDto();
            beforeLiveClueAndPerformanceData.EmployeeData = new BeforeLiveClueAndPerformanceDataItemDto();
            beforeLiveClueAndPerformanceData.EmployeeData.CustomerCount = basePhoneList.Where(e => e.CreateBy == query.AssistantId.Value).Count();
            beforeLiveClueAndPerformanceData.EmployeeData.Performance = performanceData.Where(e => myPhoneList.Contains(e.Phone)).Sum(e => e.Price);
            beforeLiveClueAndPerformanceData.EmployeeData.CurrentDayCustomerCount= basePhoneList.Where(e => e.CreateBy == query.AssistantId.Value&&e.RecordDate.Date==DateTime.Now.Date).Count();
            beforeLiveClueAndPerformanceData.EmployeeData.CurrentDayPerformance= performanceData.Where(e => myPhoneList.Contains(e.Phone)&&e.CreateDate.Date==DateTime.Now.Date).Sum(e => e.Price);
            beforeLiveClueAndPerformanceData.DepartmentData = new BeforeLiveClueAndPerformanceDataItemDto();
            beforeLiveClueAndPerformanceData.DepartmentData.CustomerCount = basePhoneList.Count();
            beforeLiveClueAndPerformanceData.DepartmentData.Performance = performanceData.Sum(e => e.Price);
            beforeLiveClueAndPerformanceData.DepartmentData.CurrentDayCustomerCount= basePhoneList.Where(e=>e.RecordDate.Date==DateTime.Now.Date).Count();
            beforeLiveClueAndPerformanceData.DepartmentData.CurrentDayPerformance = performanceData.Where(e=>e.CreateDate.Date==DateTime.Now.Date).Sum(e => e.Price);
            return beforeLiveClueAndPerformanceData;
        }
        /// <summary>
        /// 获取直播前线索和业绩折线图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveClueAndPerformanceBrokenDataDto> GetBeforeLiveClueAndPerformanceBrokenDataAsync(QueryBeforeLiveBrokenDataDto query)
        {
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var basePhoneList = _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false)
                .Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate)
                .Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.BaseLiveAnchorId == info.LiveAnchorBaseId)
                .Select(e => new { e.CreateBy, e.Phone, e.RecordDate });

            if (query.Employee)
            {
                basePhoneList = basePhoneList.Where(e => e.CreateBy == query.AssistantId.Value);
            }
            else
            {
                basePhoneList = basePhoneList.Where(e => assistantIdList.Contains(e.CreateBy));
            }
            var dataList = basePhoneList.ToList();
            var phoneList = dataList.Select(e => e.Phone).ToList();
            var performanceData = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => e.IsDeal == true&&e.IsOldCustomer==false)
                .Where(e => phoneList.Contains(e.ContentPlatFormOrder.Phone))
                .Select(e => new
                {
                    CreateDate = e.CreateDate,
                    Price = e.Price
                })
                .ToListAsync();
            BeforeLiveClueAndPerformanceBrokenDataDto beforeLiveClueAndPerformanceBrokenData = new BeforeLiveClueAndPerformanceBrokenDataDto();
            var clueData = dataList.GroupBy(e => e.RecordDate.Day).Select(e => new PerformanceBrokenLineListInfoDto
            {
                date = e.Key.ToString(),
                Performance = e.Count()
            }).ToList();
            beforeLiveClueAndPerformanceBrokenData.ClueData = FillDate(query.EndDate.Year, query.EndDate.Month, clueData);
            var performance = performanceData.GroupBy(e => e.CreateDate.Day).Select(e => new PerformanceBrokenLineListInfoDto
            {
                date = e.Key.ToString(),
                Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
            }).ToList();
            beforeLiveClueAndPerformanceBrokenData.PerformanceData = FillDate(query.EndDate.Year, query.EndDate.Month, performance);
            return beforeLiveClueAndPerformanceBrokenData;
        }
        /// <summary>
        /// 获取直播前漏斗图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveFilterDataDto> GetBeforeLiveFilterDataAsync(QueryBeforeLiveFilterDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            BeforeLiveFilterDataDto filterData = new BeforeLiveFilterDataDto();
            BeforeLiveFilterDataItemDto departmentDataDto = new BeforeLiveFilterDataItemDto();
            departmentDataDto.DataList = new List<BeforeLiveFilterDetailDataDto>();
            BeforeLiveFilterDataItemDto employeeDataDto = new BeforeLiveFilterDataItemDto();
            employeeDataDto.DataList = new List<BeforeLiveFilterDetailDataDto>();
            var healthValueList = await _healthValueService.GetValidListAsync();
            #region【小黄车数据】
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            //组小黄车数据
            var baseBusinessPerformance = await shoppingCartRegistrationService.GetBeforeLiveShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, info.LiveAnchorBaseId,assistantIdList, BelongChannel.LiveBefore);
            //个人小黄车数据
            var assisatntBusinessPerformance = await shoppingCartRegistrationService.GetBeforeLiveShopCartRegisterPerformanceByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, null,new List<int> { query.AssistantId.Value }, BelongChannel.LiveBefore);
            #endregion


            #region 组数据
            #region 【分诊】

            //分诊
            BeforeLiveFilterDetailDataDto consulationdetails = new BeforeLiveFilterDetailDataDto();
            consulationdetails.Key = "Consulation";
            consulationdetails.Name = "分诊量";
            consulationdetails.Value = baseBusinessPerformance.Where(x => x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            departmentDataDto.DataList.Add(consulationdetails);
            #endregion

            #region 【加v】
            BeforeLiveFilterDetailDataDto addWechatdetails = new BeforeLiveFilterDetailDataDto();
            //加v
            addWechatdetails.Key = "AddWeChat";
            addWechatdetails.Name = "加v量";
            addWechatdetails.Value = baseBusinessPerformance.Where(x => x.IsAddWeChat == true && x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            departmentDataDto.DataList.Add(addWechatdetails);

            //加v率
            departmentDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails.Value, consulationdetails.Value);
            departmentDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 获取部门基础数据
            bool isCurrent = true;
            if (query.History)
                isCurrent = false;
            var depeartPhoneList = baseBusinessPerformance.Select(e => e.Phone).ToList();
            var allOrderPerformance = await contentPlateFormOrderService.GetBeforeLiveDepartOrderSendAndDealDataByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList, depeartPhoneList, isCurrent);

            #endregion
            #region 【派单】
            BeforeLiveFilterDetailDataDto sendOrderdetails = new BeforeLiveFilterDetailDataDto();
            //派单
            sendOrderdetails.Key = "SendOrder";
            sendOrderdetails.Name = "派单量";
            sendOrderdetails.Value = allOrderPerformance.SendOrderNum;
            departmentDataDto.DataList.Add(sendOrderdetails);

            //派单率
            departmentDataDto.SendOrderRate = DecimalExtension.CalculateTargetComplete(sendOrderdetails.Value, addWechatdetails.Value);
            departmentDataDto.SendOrderRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【上门】
            BeforeLiveFilterDetailDataDto visitdetails = new BeforeLiveFilterDetailDataDto();
            //上门
            visitdetails.Key = "ToHospital";
            visitdetails.Name = "上门量";
            visitdetails.Value = allOrderPerformance.VisitNum;
            departmentDataDto.DataList.Add(visitdetails);

            //上门率
            departmentDataDto.ToHospitalRate = DecimalExtension.CalculateTargetComplete(visitdetails.Value, sendOrderdetails.Value);
            departmentDataDto.ToHospitalRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 成交

            BeforeLiveFilterDetailDataDto dealData = new BeforeLiveFilterDetailDataDto();
            //成交
            dealData.Key = "Deal";
            dealData.Name = "成交量";
            dealData.Value = allOrderPerformance.DealNum;
            departmentDataDto.DataList.Add(dealData);

            //成交率
            departmentDataDto.DealRate = DecimalExtension.CalculateTargetComplete(dealData.Value, visitdetails.Value);
            departmentDataDto.DealRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "DealRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();

            #endregion
            filterData.DepartData = departmentDataDto;

            #endregion

            #region 分诊上门转化周期

            #region 分诊派单
            List<KeyValuePair<int, int>> dataList = new();
            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Where(e => e.IsMainHospital == true && e.SendDate >= selectDate.StartDate && e.SendDate < selectDate.EndDate)
               .Where(e => e.ContentPlatformOrder.BelongChannel == (int)BelongChannel.LiveBefore)
               .Where(e => assistantIdList.Contains(e.Sender))
               .Select(e => new { Phone = e.ContentPlatformOrder.Phone, EmpId = e.Sender, SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Distinct().Select(e => e.Phone).ToList();
            if (isCurrent)
            {
                var cartInfoList1 = baseBusinessPerformance.Where(e => sendPhoneList.Contains(e.Phone)).ToList();
                dataList = (from send in sendInfoList
                            join cart in cartInfoList1
                            on send.Phone equals cart.Phone
                            select new KeyValuePair<int, int>(cart.CreateBy,
                                (send.SendDate - cart.RecordDate).Days)).ToList();
            }
            else
            {
                var historyPhone = sendPhoneList.Where(e => !baseBusinessPerformance.Select(e => e.Phone).Contains(e));
                var cartInfoList1 = _dalShoppingCartRegistration.GetAll().Where(e => historyPhone.Contains(e.Phone)).Select(e => new { e.Phone, e.CreateBy, e.RecordDate }).ToList();
                dataList = (from send in sendInfoList
                            join cart in cartInfoList1
                            on send.Phone equals cart.Phone
                            select new KeyValuePair<int, int>(cart.CreateBy,
                                (send.SendDate - cart.RecordDate).Days)).ToList();
            }
            dataList.RemoveAll(e => e.Value < 0);
            var endIndex = DecimalExtension.CalTakeCount(dataList.Count(), 1);
            var takeList = dataList.OrderBy(e => e.Value).Skip(0).Take(endIndex);
            //转化周期数据
            var sendCycle = DecimalExtension.CalAvg(takeList.Sum(e => e.Value), takeList.Count());
            departmentDataDto.SendCycle = sendCycle;
            #endregion

            #region 分诊上门
            List<KeyValuePair<int, int>> dataList2 = new();
            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                    .Where(e => (e.ContentPlatFormOrder.IsSupportOrder ? assistantIdList.Contains(e.ContentPlatFormOrder.SupportEmpId) : assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value)))
                    .Select(e => new
                    {
                        EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.ToHospitalDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).ToList();

            if (isCurrent)
            {
                var cartInfoList1 = baseBusinessPerformance.Where(e => sendPhoneList.Contains(e.Phone)).ToList();
                dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList1
                             on deal.Phone equals cart.Phone
                             select new KeyValuePair<int, int>
                             (
                                 cart.CreateBy,
                                (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             )).ToList();
            }
            else {
                var historyPhone = dealPhoneList.Where(e => !baseBusinessPerformance.Select(e => e.Phone).Contains(e));
                var cartInfoList1 = _dalShoppingCartRegistration.GetAll().Where(e => historyPhone.Contains(e.Phone)).Select(e => new { e.Phone, e.CreateBy, e.RecordDate }).ToList();
                dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList1
                             on deal.Phone equals cart.Phone
                             select new KeyValuePair<int, int>
                             (
                                 cart.CreateBy,
                                (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             )).ToList();
            }
            dataList2.RemoveAll(e => e.Value < 0);
            var endIndex2 = DecimalExtension.CalTakeCount(dataList2.Count(), 1);
            var takeList2 = dataList2.OrderBy(e => e.Value).Skip(0).Take(endIndex2);
            //转化周期数据
            var hospitalCycle= DecimalExtension.CalAvg(takeList2.Sum(e => e.Value), takeList2.Count());
            departmentDataDto.HospitalCycle = hospitalCycle;
            #endregion

            #endregion

            #region 个人

            #region 【分诊】
            //当月数据
            var employeePhoneList = assisatntBusinessPerformance.Select(e => e.Phone).ToList();
            var addWechatOrderPerformance = await contentPlateFormOrderService.GetBeforeLiveEmployeeOrderSendAndDealDataByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, assistantIdList, query.AssistantId.Value, employeePhoneList, isCurrent);
            //分诊
            BeforeLiveFilterDetailDataDto consulationdetails2 = new BeforeLiveFilterDetailDataDto();
            consulationdetails2.Key = "Consulation";
            consulationdetails2.Name = "分诊量";
            consulationdetails2.Value = assisatntBusinessPerformance.Where(x => x.AssignEmpId != null && x.IsReturnBackPrice == false).Count();
            employeeDataDto.DataList.Add(consulationdetails2);
            #endregion

            #region 【加v】
            BeforeLiveFilterDetailDataDto addWechatdetails2 = new BeforeLiveFilterDetailDataDto();
            //加v
            addWechatdetails2.Key = "AddWeChat";
            addWechatdetails2.Name = "加v量";
            addWechatdetails2.Value = assisatntBusinessPerformance.Where(x => x.IsAddWeChat == true && x.AssignEmpId != null && x.IsReturnBackPrice == false).Count();
            employeeDataDto.DataList.Add(addWechatdetails2);

            //加v率
            employeeDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails2.Value, consulationdetails2.Value);
            employeeDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【派单】
            BeforeLiveFilterDetailDataDto sendOrderdetails2 = new BeforeLiveFilterDetailDataDto();
            //派单
            sendOrderdetails2.Key = "SendOrder";
            sendOrderdetails2.Name = "派单量";
            sendOrderdetails2.Value = addWechatOrderPerformance.SendOrderNum;
            employeeDataDto.DataList.Add(sendOrderdetails2);

            //派单率
            employeeDataDto.SendOrderRate = DecimalExtension.CalculateTargetComplete(sendOrderdetails2.Value, addWechatdetails2.Value);
            employeeDataDto.SendOrderRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【上门】
            BeforeLiveFilterDetailDataDto visitdetails2 = new BeforeLiveFilterDetailDataDto();
            //上门
            visitdetails2.Key = "ToHospital";
            visitdetails2.Name = "上门量";
            visitdetails2.Value = addWechatOrderPerformance.VisitNum;
            employeeDataDto.DataList.Add(visitdetails2);

            //上门率
            employeeDataDto.ToHospitalRate = DecimalExtension.CalculateTargetComplete(visitdetails2.Value, sendOrderdetails2.Value);
            employeeDataDto.ToHospitalRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region

            BeforeLiveFilterDetailDataDto deal2 = new BeforeLiveFilterDetailDataDto();
            //成交
            deal2.Key = "Deal";
            deal2.Name = "成交量";
            deal2.Value = addWechatOrderPerformance.DealNum;
            employeeDataDto.DataList.Add(deal2);

            //成交率
            employeeDataDto.DealRate = DecimalExtension.CalculateTargetComplete(deal2.Value, visitdetails2.Value);
            employeeDataDto.DealRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "DealRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();

            #endregion

            filterData.EmployeeData = employeeDataDto;

            #endregion
            #region 派单转化周期
            var employeeEndIndex = DecimalExtension.CalTakeCount(dataList.Where(e => e.Key == query.AssistantId).Count(),1);
            var employeeSendData= dataList.Where(e => e.Key == query.AssistantId).OrderBy(e=>e.Value).ToList().Skip(0).Take(employeeEndIndex);
            employeeDataDto.SendCycle = DecimalExtension.CalAvg(employeeSendData.Sum(e=>e.Value), employeeSendData.Count());
            #endregion
            #region 上门转化周期
            var employeeEndIndex2 = DecimalExtension.CalTakeCount(dataList2.Where(e => e.Key == query.AssistantId).Count(),1);
            var employeeToHospitalData = dataList2.Where(e => e.Key == query.AssistantId).OrderBy(e => e.Value).ToList().Skip(0).Take(employeeEndIndex2);
            employeeDataDto.HospitalCycle = DecimalExtension.CalAvg(employeeToHospitalData.Sum(e => e.Value), employeeToHospitalData.Count());
            #endregion
            return filterData;
        }
        /// <summary>
        /// 获取直播前转化周期
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveTransformCycleData> GetBeforeLiveTransformCycleDataAsync(QueryBeforeLiveDataDto query)
        {
            BeforeLiveTransformCycleData data = new BeforeLiveTransformCycleData();
            var seqDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            //var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId }); GetLiveBeforeEmployeeNameListAsync
            var liveBeforeAssistantList = await amiyaEmployeeService.GetLiveBeforeEmployeeNameListAsync();
            //var assistantList = await amiyaEmployeeService.GetAssistantAsync();
            var liveBeforeAssistantIdList = liveBeforeAssistantList.Select(e => e.Id).ToList();
            var assistantList =await amiyaEmployeeService.GetAllAssistantAsync();
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var cartInfoList = _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false && e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate)
                .Select(e => new
                {
                    CreateBy = e.CreateBy,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate
                }).ToList();
            #region 分诊派单

            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Where(e => e.IsMainHospital == true && e.SendDate >= seqDate.StartDate && e.SendDate < seqDate.EndDate)
                .Where(e => assistantIdList.Contains(e.Sender))
                .Select(e => new { Phone = e.ContentPlatformOrder.Phone, EmpId = e.Sender, SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Distinct().Select(e => e.Phone).ToList();
            var cartInfoList1 = cartInfoList.Where(e => sendPhoneList.Contains(e.Phone)).ToList();
            var dataList = (from send in sendInfoList
                            join cart in cartInfoList1
                            on send.Phone equals cart.Phone
                            select new
                            {
                                EmpId = cart.CreateBy,
                                IntervalDays = (send.SendDate - cart.RecordDate).Days
                            }).ToList();
            dataList.RemoveAll(e => e.IntervalDays < 0);
            liveBeforeAssistantList.AddRange(assistantList);
            //转化周期数据
            var res1 = dataList.GroupBy(e => e.EmpId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count(),1);
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                liveBeforeAssistantList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其它",
                DecimalExtension.CalAvg(resData.Sum(e => e.IntervalDays), resData.Count())
             );
            }).OrderBy(e => e.Value).ToList();
            res1.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            data.SendCycleData = res1;
            #endregion

            #region 分诊上门
            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                    .Where(e => (e.ContentPlatFormOrder.IsSupportOrder ? assistantIdList.Contains(e.ContentPlatFormOrder.SupportEmpId) : assistantIdList.Contains(e.ContentPlatFormOrder.BelongEmpId.Value)))
                    .Select(e => new
                    {
                        EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.ToHospitalDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).ToList();
            var cartInfoList2 = cartInfoList.Where(e => dealPhoneList.Contains(e.Phone)).ToList();
            var dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList2
                             on deal.Phone equals cart.Phone
                             select new
                             {
                                 EmpId = cart.CreateBy,
                                 IntervalDays = (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             }).ToList();
            dataList2.RemoveAll(e => e.IntervalDays < 0);
            //转化周期数据
            var res2 = dataList2.GroupBy(e => e.EmpId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count(), 1);
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                liveBeforeAssistantList.Where(a => a.Id == e.Key).FirstOrDefault()?.Name ?? "其它",
                DecimalExtension.CalAvg(resData.Sum(e => e.IntervalDays), resData.Count()));
            }).OrderBy(e => e.Value).ToList();
            res2.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            data.ToHospitalCycleData = res2;
            #endregion
            return data;
        }
        /// <summary>
        /// 获取直播前业绩占比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveTargetCompleteAndPerformanceRateDto> GetBeforeLivePerformanceRateAsync(QueryBeforeLiveDataDto query)
        {
            BeforeLiveTargetCompleteAndPerformanceRateDto data = new BeforeLiveTargetCompleteAndPerformanceRateDto();
            var employeeList = amiyaEmployeeService.GetEmployeeNameList();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var performance = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.IsDeal == true && e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId.Value))
                .Select(e => new
                {
                    e.ContentPlatFormOrder.Phone,
                    e.Price
                }).ToList();
            var beforeLiveData = await _dalShoppingCartRegistration.GetAll()
                .Where(e => e.BaseLiveAnchorId==info.LiveAnchorBaseId && e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => performance.Select(e => e.Phone).Contains(e.Phone))
                .Select(e => new { e.CreateBy, e.Phone }).ToListAsync();
            var totalPerformance = performance.Sum(e => e.Price);
            var list = beforeLiveData.GroupBy(e => e.CreateBy).Select(e =>
            {
                var amount = performance.Where(x => e.Select(e => e.Phone).Contains(x.Phone)).Sum(e => e.Price);
                var name = employeeList.Where(x => x.Id == e.Key).FirstOrDefault()?.Name ?? "其他";
                return new KeyValuePair<string, decimal>(name, DecimalExtension.CalculateTargetComplete(amount, totalPerformance).Value);
            }).ToList();
            list.RemoveAll(e => e.Key == "其他");
            data.PerformanceRate = list;
            return data;
        }
        /// <summary>
        /// 内容运营线索目标达成率
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveTargetCompleteRateDto> GetBeforeLiveTargetCompleteRateAsync(QueryBeforeLiveDataDto query)
        {
            var employeeList = amiyaEmployeeService.GetEmployeeNameList();
            BeforeLiveTargetCompleteRateDto data = new BeforeLiveTargetCompleteRateDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var baseData = await _dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate && e.BaseLiveAnchorId == info.LiveAnchorBaseId && e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Select(e => e.CreateBy).ToListAsync();
            var clueData = baseData.GroupBy(e => e).Select(e => new { Key = e.Key, Count = e.Count() });
            var targetListData = dalLiveAnchorMonthlyTargetBeforeLiving
                .GetAll()
                .Where(e => e.Month == query.EndDate.Month && e.Year == query.EndDate.Year).Select(e => new
                {
                    e.OwnerId,
                    e.TikTokCluesTarget,
                    e.VideoCluesTarget,
                    e.XiaoHongShuCluesTarget
                }).ToList();
            var realTargetData = targetListData.GroupBy(e => e.OwnerId).Select(e =>
            {
                var target = e.Select(e =>
                {
                    var tikTokCluesTarget = e.TikTokCluesTarget == 1 ? 0 : e.TikTokCluesTarget;
                    var xiaoHongShuCluesTarget = e.XiaoHongShuCluesTarget == 1 ? 0 : e.XiaoHongShuCluesTarget;
                    var videoCluesTarget = e.VideoCluesTarget == 1 ? 0 : e.VideoCluesTarget;
                    return tikTokCluesTarget + xiaoHongShuCluesTarget + videoCluesTarget;
                });
                return new { Id = e.Key, Target = target.Sum() };
            }).ToList();
            data.TargetComplete = clueData.Select(e =>
            {
                var name = employeeList.Where(x => x.Id == e.Key).FirstOrDefault()?.Name ?? "其他";
                var target = realTargetData.Where(x => x.Id == e.Key).Sum(x => x.Target);
                var targetComplete = DecimalExtension.CalculateTargetComplete(e.Count, target).Value;
                return new KeyValuePair<string, decimal>(name, targetComplete);
            }).ToList();
            return data;
        }
        /// <summary>
        /// 获取直播前部门平台线索占比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveDepartmentContentPlatformClueRateDto> GetBeforeLiveDepartmentContentPlatformClueRateAsync(QueryBeforeLiveDataDto query)
        {
            BeforeLiveDepartmentContentPlatformClueRateDto beforeLiveDepartment = new BeforeLiveDepartmentContentPlatformClueRateDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var baseData = await _dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate && e.BaseLiveAnchorId == info.LiveAnchorBaseId && e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Select(e => new
                {
                    ContentPlatformName = e.Contentplatform.ContentPlatformName,
                    ContentPlatformId = e.ContentPlatFormId,
                    LiveAnchorName = e.LiveAnchor.Name,
                }).ToListAsync();
            var totalCount = baseData.Count;
            beforeLiveDepartment.DepartmentClue = totalCount;
            beforeLiveDepartment.DepartmentContentPlatformClueRate = baseData.GroupBy(e => e.ContentPlatformName).Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemDto
            {
                Name = e.Key,
                Value = DecimalExtension.CalculateTargetComplete(e.Count(), totalCount).Value,
                Performance = e.Count()
            }).ToList();
            beforeLiveDepartment.TikTokClue = baseData.Where(e => e.ContentPlatformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count();
            beforeLiveDepartment.TikTokClueRate = baseData.Where(e => e.ContentPlatformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").GroupBy(e => e.LiveAnchorName)
                .Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(e.Count(), baseData.Where(e => e.ContentPlatformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count()).Value,
                    Performance = e.Count()
                }).ToList();
            beforeLiveDepartment.WechatVideoClue = baseData.Where(e => e.ContentPlatformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count();
            beforeLiveDepartment.WechatVideoClueRate = baseData.Where(e => e.ContentPlatformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").GroupBy(e => e.LiveAnchorName)
               .Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemDto
               {
                   Name = e.Key,
                   Value = DecimalExtension.CalculateTargetComplete(e.Count(), baseData.Where(e => e.ContentPlatformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count()).Value,
                   Performance = e.Count()
               }).ToList();
            beforeLiveDepartment.XiaoHongShuClue = baseData.Where(e => e.ContentPlatformId == "317c03b8-aff9-4961-8392-fc44d04b1725").Count();
            beforeLiveDepartment.XiaohongshuClueRate = baseData.Where(e => e.ContentPlatformId == "317c03b8-aff9-4961-8392-fc44d04b1725").GroupBy(e => e.LiveAnchorName)
               .Select(e => new BeforeLiveDepartmentContentPlatformClueRateDataItemDto
               {
                   Name = e.Key,
                   Value = DecimalExtension.CalculateTargetComplete(e.Count(), baseData.Where(e => e.ContentPlatformId == "317c03b8-aff9-4961-8392-fc44d04b1725").Count()).Value,
                   Performance = e.Count()
               }).ToList();
            return beforeLiveDepartment;


        }


        /// <summary>
        /// 获取直播前部门平台业绩占比数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BeforeLiveDepartmentContentPlatformPerformanceRateDto> GetBeforeLiveDepartmentContentPlatformPerformanceRateAsync(QueryBeforeLiveFilterDataDto query)
        {
            BeforeLiveDepartmentContentPlatformClueRateDto beforeLiveDepartment = new BeforeLiveDepartmentContentPlatformClueRateDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var baseData = await _dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate && e.BaseLiveAnchorId == info.LiveAnchorBaseId && e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Select(e => e.Phone).ToListAsync();
            BeforeLiveDepartmentContentPlatformPerformanceRateDto data = new BeforeLiveDepartmentContentPlatformPerformanceRateDto();
            List<string> phoneList = new();
            if (query.Current)
            {

                phoneList = baseData.Select(e => e).ToList();
            }
            else
            {
                //该部门本月直播前全部成交手机号(历史和当月)
                var totalPhoneList = dalContentPlatFormOrderDealInfo.GetAll()
                    .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.LiveBefore)
                    .Where(e => assistantIdList.Contains(e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId.Value))
                    .Where(e => e.IsDeal == true && e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate).Select(e => e.ContentPlatFormOrder.Phone).ToList();
                phoneList = totalPhoneList.Where(e => !baseData.Select(e => e).Contains(e)).ToList();
            }
            var performanceList = dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.IsDeal == true && phoneList.Contains(e.ContentPlatFormOrder.Phone) && e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                   .Select(e => new
                   {
                       LiveAnchorName = e.ContentPlatFormOrder.LiveAnchor.Name,
                       ContentPlateformId = e.ContentPlatFormOrder.ContentPlateformId,
                       ContentPlatformName = e.ContentPlatFormOrder.Contentplatform.ContentPlatformName,
                       Price = e.Price
                   }).ToList();
            var totalPerformance = performanceList.Sum(e => e.Price);
            data.DepartmentPerformance = ChangePriceToTenThousand(totalPerformance);
            data.DepartmentContentPlatformPerformanceRate = performanceList.GroupBy(e => e.ContentPlatformName)
                .Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(performanceList.Sum(e => e.Price), totalPerformance).Value,
                    Performance = ChangePriceToTenThousand(performanceList.Sum(e => e.Price))
                }).ToList();
            data.TikTokPerformance = ChangePriceToTenThousand(performanceList.Where(e => e.ContentPlateformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Sum(e => e.Price));
            data.TikTokPerformanceRate = performanceList.Where(e => e.ContentPlateformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").GroupBy(e => e.LiveAnchorName)
                .Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), performanceList.Where(e => e.ContentPlateformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Sum(e => e.Price)).Value,
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
                }).ToList();
            data.WechatVideoPerformance = ChangePriceToTenThousand(performanceList.Where(e => e.ContentPlateformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Sum(e => e.Price));
            data.WechatVideoPerformanceRate = performanceList.Where(e => e.ContentPlateformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").GroupBy(e => e.LiveAnchorName)
               .Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemDto
               {
                   Name = e.Key,
                   Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), performanceList.Where(e => e.ContentPlateformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Sum(e => e.Price)).Value,
                   Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
               }).ToList();
            data.XiaohongshuPerformance = ChangePriceToTenThousand(performanceList.Where(e => e.ContentPlateformId == "317c03b8-aff9-4961-8392-fc44d04b1725").Sum(e => e.Price));
            data.XiaohongshuPerformanceRate = performanceList.Where(e => e.ContentPlateformId == "317c03b8-aff9-4961-8392-fc44d04b1725").GroupBy(e => e.LiveAnchorName)
               .Select(e => new BeforeLiveDepartmentContentPlatformPerformanceRateDataItemDto
               {
                   Name = e.Key,
                   Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), performanceList.Where(e => e.ContentPlateformId == "317c03b8-aff9-4961-8392-fc44d04b1725").Sum(e => e.Price)).Value,
                   Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
               }).ToList();
            return data;
        }


        public async Task<List<BeforeLiveLiveanchorIPDataDto>> GetBeforeLiveLiveanchorIPDataAsync(QueryBeforeLiveDataDto query) {
            List<BeforeLiveLiveanchorIPDataDto> dataList = new List<BeforeLiveLiveanchorIPDataDto>();
            var targetListData = dalLiveAnchorMonthlyTargetBeforeLiving
                .GetAll()
                .Where(e => e.Month == query.EndDate.Month && e.Year == query.EndDate.Year).Select(e => new
                {
                    e.LiveAnchorId,
                    e.TikTokCluesTarget,
                    e.VideoCluesTarget,
                    e.XiaoHongShuCluesTarget
                }).ToList();
            var realTargetData = targetListData.GroupBy(e => e.LiveAnchorId).Select(e =>
            {
                var target = e.Select(e =>
                {
                    var tikTokCluesTarget = e.TikTokCluesTarget == 1 ? 0 : e.TikTokCluesTarget;
                    var xiaoHongShuCluesTarget = e.XiaoHongShuCluesTarget == 1 ? 0 : e.XiaoHongShuCluesTarget;
                    var videoCluesTarget = e.VideoCluesTarget == 1 ? 0 : e.VideoCluesTarget;
                    return tikTokCluesTarget + xiaoHongShuCluesTarget + videoCluesTarget;
                });
                return new { Id = e.Key, Target = target.Sum() };
            }).ToList();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var info = await amiyaEmployeeService.GetByIdAsync(query.AssistantId.Value);
            var assistantList = await amiyaEmployeeService.GetByLiveAnchorBaseIdNameListAsync(new List<string> { info.LiveAnchorBaseId });
            var assistantIdList = assistantList.Select(e => e.Id).ToList();
            var currentList =await GetBeforeLiveLiveanchorIPDataItemsAsync(selectDate.StartDate,selectDate.EndDate,info.LiveAnchorBaseId);
            var lastList=await GetBeforeLiveLiveanchorIPDataItemsAsync(selectDate.LastMonthStartDate, selectDate.LastMonthEndDate, info.LiveAnchorBaseId);
            var historyList=await GetBeforeLiveLiveanchorIPDataItemsAsync(selectDate.LastYearThisMonthStartDate, selectDate.LastYearThisMonthEndDate, info.LiveAnchorBaseId);
            foreach (var item in currentList) {
                BeforeLiveLiveanchorIPDataDto data = new BeforeLiveLiveanchorIPDataDto();
                data.LiveanchorIP = item.Name;
                data.ClueCount = item.ClueCount;
                var lastCount = lastList.Where(e => e.LiveanchorId == item.LiveanchorId).FirstOrDefault()?.ClueCount ?? 0;
                var historyCount = historyList.Where(e => e.LiveanchorId == item.LiveanchorId).FirstOrDefault()?.ClueCount ?? 0;
                var target = realTargetData.Where(e => e.Id == item.LiveanchorId).FirstOrDefault()?.Target ?? 0;
                data.YearOnYear = DecimalExtension.CalculateTargetComplete(data.ClueCount, lastCount).Value;
                data.Chain = DecimalExtension.CalculateTargetComplete(data.ClueCount,historyCount).Value;
                data.TargetComplete= DecimalExtension.CalculateTargetComplete(data.ClueCount, target).Value;
                dataList.Add(data);
            }
            return dataList;
        }
        public async Task<List<BeforeLiveLiveanchorIPDataItemDto>> GetBeforeLiveLiveanchorIPDataItemsAsync(DateTime startDate, DateTime endDate, string baseId)
        {
            var basePhoneList =await _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false)
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate)
                .Where(e => e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.BaseLiveAnchorId==baseId)
                .Select(e => new  {Phone= e.Phone,LiveanchorId= e.LiveAnchorId,Name=e.LiveAnchor.Name })
                .ToListAsync();
           
            return basePhoneList.GroupBy(e=>e.LiveanchorId).Select(e=>new BeforeLiveLiveanchorIPDataItemDto { 
                LiveanchorId=e.Key,
                ClueCount=e.Count(),
                Name=e.FirstOrDefault()?.Name??"其他"
            }).ToList();  
        }

        #endregion

        #region 公共类

        private decimal ChangePriceToTenThousand(decimal performance, int unit = 1)
        {
            if (performance == 0m)
                return 0;
            var result = Math.Round((performance / 10000), unit, MidpointRounding.AwayFromZero);
            return result;
        }
        /// <summary>
        /// 计算对比进度,业绩偏差和后期需完成业绩
        /// </summary>
        /// <param name="performanceTarget">总业绩目标</param>
        /// <param name="currentPerformance">当前完成业绩</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        private decimal CalculateSchedule(decimal performanceTarget, decimal currentPerformance, int year, int month)
        {
            PerformanceScheduleDto performanceScheduleDto = new PerformanceScheduleDto();
            if (performanceTarget == 0m || currentPerformance == 0m)
            {
                return 0;
            }

            decimal timeSchedule = 0;
            var now = DateTime.Now;
            var totalDay = DateTime.DaysInMonth(now.Year, now.Month);
            var nowDay = now.Day;
            if (year != now.Year || month != now.Month)
            {
                timeSchedule = 100m;
            }
            else
            {
                timeSchedule = Math.Round(Convert.ToDecimal(nowDay) / Convert.ToDecimal(totalDay) * 100, 2, MidpointRounding.AwayFromZero);
            }
            decimal performanceSchedule = Math.Round(currentPerformance / performanceTarget * 100, 2, MidpointRounding.AwayFromZero);
            return performanceSchedule - timeSchedule;
        }

        /// <summary>
        /// 填充日期数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private List<PerformanceBrokenLineListInfoDto> FillDate(int year, int month, List<PerformanceBrokenLineListInfoDto> dataList)
        {
            List<PerformanceBrokenLineListInfoDto> list = new List<PerformanceBrokenLineListInfoDto>();

            var totalDays = DateTime.DaysInMonth(year, month);
            for (int i = 1; i < totalDays + 1; i++)
            {
                PerformanceBrokenLineListInfoDto item = new PerformanceBrokenLineListInfoDto();
                item.date = i.ToString();
                item.Performance = dataList.Where(e => e.date == item.date).Select(e => e.Performance).SingleOrDefault() ?? 0m;
                list.Add(item);
            }
            return list;
        }

       


        #endregion
        #region 【历史版本】

        ///// <summary>
        ///// 获取客户运营情况数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<GetCustomerAnalizeDataDto> GetCustomerAnalizeDataAsync(QueryOperationDataDto query)
        //{
        //    GetCustomerAnalizeDataDto result = new GetCustomerAnalizeDataDto();
        //    List<int> LiveAnchorInfoDaoDao = new List<int>();
        //    var liveAnchorDaoDao = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("f0a77257-c905-4719-95c4-ad2c4f33855c");
        //    LiveAnchorInfoDaoDao = liveAnchorDaoDao.Select(x => x.Id).ToList();
        //    var contentPlatFormOrderDataDaoDao = await contentPlateFormOrderService.GetOrderSendAndDealDataByMonthAsync(query.startDate.Value, query.endDate.Value, null, "", LiveAnchorInfoDaoDao);

        //    List<int> LiveAnchorInfoJiNa = new List<int>();
        //    var liveAnchorJina = await liveAnchorService.GetValidListByLiveAnchorBaseIdAsync("af69dcf5-f749-41ea-8b50-fe685facdd8b");
        //    LiveAnchorInfoJiNa = liveAnchorJina.Select(x => x.Id).ToList();
        //    var contentPlatFormOrderDataJiNa = await contentPlateFormOrderService.GetOrderSendAndDealDataByMonthAsync(query.startDate.Value, query.endDate.Value, null, "", LiveAnchorInfoJiNa);
        //    CustomerAnalizeByGroupDto SendGroupDto = new CustomerAnalizeByGroupDto();
        //    SendGroupDto.GroupDaoDao = contentPlatFormOrderDataDaoDao.SendOrderNum;
        //    SendGroupDto.GroupJiNa = contentPlatFormOrderDataJiNa.SendOrderNum;
        //    SendGroupDto.TotalNum = SendGroupDto.GroupDaoDao + SendGroupDto.GroupJiNa;
        //    result.SendNum = SendGroupDto;

        //    CustomerAnalizeByGroupDto VisitGroupDto = new CustomerAnalizeByGroupDto();
        //    VisitGroupDto.GroupDaoDao = contentPlatFormOrderDataDaoDao.VisitNum;
        //    VisitGroupDto.GroupJiNa = contentPlatFormOrderDataJiNa.VisitNum;
        //    VisitGroupDto.TotalNum = VisitGroupDto.GroupJiNa + VisitGroupDto.GroupDaoDao;
        //    result.VisitNum = VisitGroupDto;

        //    CustomerAnalizeByGroupDto DealGroupDto = new CustomerAnalizeByGroupDto();
        //    DealGroupDto.GroupDaoDao = contentPlatFormOrderDataDaoDao.DealNum;
        //    DealGroupDto.GroupJiNa = contentPlatFormOrderDataJiNa.DealNum;
        //    DealGroupDto.TotalNum = DealGroupDto.GroupJiNa + DealGroupDto.GroupDaoDao;
        //    result.DealNum = DealGroupDto;
        //    return result;
        //}

        ///// <summary>
        ///// 获取客户指标转化数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<GetCustomerIndexTransformationResultDto> GetCustomerIndexTransformationDataAsync(QueryOperationDataDto query)
        //{
        //    GetCustomerIndexTransformationResultDto result = new GetCustomerIndexTransformationResultDto();
        //    //小黄车数据
        //    var baseShoppingCartRegistionData = await shoppingCartRegistrationService.GetNewBaseBusinessPerformanceByLiveAnchorNameAsync(query.startDate.Value, query.endDate.Value, null, "");

        //    //订单数据
        //    var baseOrderPerformance = await contentPlateFormOrderService.GetOrderSendAndDealDataByMonthAsync(query.startDate.Value, query.endDate.Value, null, "", new List<int>());

        //    result.AddCardNum = baseShoppingCartRegistionData.Count();
        //    result.RefundCardNum = baseShoppingCartRegistionData.Where(x => x.IsReturnBackPrice == true).Count();
        //    result.DistributeConsulationNum = baseShoppingCartRegistionData.Where(x => x.AssignEmpId.HasValue).Count();
        //    result.AddWechatNum = baseShoppingCartRegistionData.Where(x => x.IsAddWeChat == true).Count();
        //    result.SendOrderNum = baseOrderPerformance.SendOrderNum;
        //    result.VisitNum = baseOrderPerformance.VisitNum;
        //    result.DealNum = baseOrderPerformance.DealNum;

        //    return result;
        //}

        ///// <summary>
        ///// 获取助理业绩分析数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<GetEmployeePerformanceAnalizeDataDto> GetEmployeePerformanceAnalizeDataAsync(QueryOperationDataDto query)
        //{
        //    GetEmployeePerformanceAnalizeDataDto result = new GetEmployeePerformanceAnalizeDataDto();

        //    List<int> amiyaEmployeeIds = new List<int>();
        //    //获取所有助理
        //    var employeeInfos = await amiyaEmployeeService.GetemployeeByPositionIdAsync(4);
        //    amiyaEmployeeIds = employeeInfos.Select(x => x.Id).ToList();

        //    #region 【助理业绩-5条】
        //    var dealInfo = await contentPlateFormOrderService.GetFourCustomerServicePerformanceByCustomerServiceIdAsync(query.startDate.Value, query.endDate.Value, amiyaEmployeeIds);
        //    List<GetEmployeePerformanceDataDto> employeeDataList = new List<GetEmployeePerformanceDataDto>();
        //    foreach (var x in dealInfo)
        //    {
        //        GetEmployeePerformanceDataDto getEmployeePerformanceDataDto = new GetEmployeePerformanceDataDto();
        //        getEmployeePerformanceDataDto.EmployeeName = x.CustomerServiceName;
        //        getEmployeePerformanceDataDto.Performance = x.TotalServicePrice;
        //        var employeeDataTarget = await employeePerformanceTargetService.GetByEmpIdAndYearMonthAsync(x.CustomerServiceId, query.endDate.Value.Year, query.endDate.Value.Month);
        //        if (employeeDataTarget != 0)
        //        {
        //            getEmployeePerformanceDataDto.CompleteRate = Math.Round(x.TotalServicePrice / employeeDataTarget * 100, 2, MidpointRounding.AwayFromZero);
        //        }
        //        else
        //        {
        //            getEmployeePerformanceDataDto.CompleteRate = 0;
        //        }
        //        employeeDataList.Add(getEmployeePerformanceDataDto);
        //    }
        //    result.EmployeeDatas = employeeDataList;
        //    #endregion

        //    #region 【助理获客情况-10条】
        //    var shoppingCartRegistionData = await shoppingCartRegistrationService.GetNewBaseBusinessPerformanceByLiveAnchorNameAsync(query.startDate.Value, query.endDate.Value, null, "");

        //    result.EmployeeDistributeConsulationNumAndAddWechats = shoppingCartRegistionData.Where(x => x.AssignEmpId.HasValue).GroupBy(x => x.AssignEmpId).Select(x => new GetEmployeeDistributeConsulationNumAndAddWechatDto
        //    {
        //        EmployeeId = Convert.ToInt32(x.Key.ToString()),
        //        DistributeConsulationNum = x.Count(),
        //        AddWechatNum = x.Where(e => e.IsAddWeChat == true).Count(),
        //    }).Where(x => x.DistributeConsulationNum > 0).Take(10).ToList();
        //    foreach (var x in result.EmployeeDistributeConsulationNumAndAddWechats)
        //    {
        //        var empInfo = await amiyaEmployeeService.GetByIdAsync(x.EmployeeId);
        //        x.EmployeeName = empInfo.Name;
        //    }
        //    #endregion

        //    #region 【助理客户运营情况-10条】
        //    List<GetEmployeeCustomerAnalizeDto> getEmployeeCustomerAnalizeDtos = new List<GetEmployeeCustomerAnalizeDto>();
        //    foreach (var x in amiyaEmployeeIds)
        //    {
        //        GetEmployeeCustomerAnalizeDto getEmployeeCustomerAnalizeDto = new GetEmployeeCustomerAnalizeDto();
        //        var empInfo = await amiyaEmployeeService.GetByIdAsync(x);
        //        getEmployeeCustomerAnalizeDto.EmployeeName = empInfo.Name;
        //        getEmployeeCustomerAnalizeDto.SendOrderNum = await contentPlatformOrderSendService.GetTotalSendCountByEmployeeAsync(x, query.startDate.Value, query.endDate.Value);
        //        var contentPlatFormVisitAndDealNumData = await contentPlateFormOrderService.GetCustomerVisitAndIsDealByEmployeeIdAsync(query.startDate.Value, query.endDate.Value, x);
        //        getEmployeeCustomerAnalizeDto.VisitNum = contentPlatFormVisitAndDealNumData.VisitNum;
        //        getEmployeeCustomerAnalizeDto.DealNum = contentPlatFormVisitAndDealNumData.DealNum;
        //        getEmployeeCustomerAnalizeDtos.Add(getEmployeeCustomerAnalizeDto);
        //    }
        //    result.GetEmployeeCustomerAnalizes = getEmployeeCustomerAnalizeDtos.Where(x => x.SendOrderNum > 0).OrderByDescending(x => x.SendOrderNum).Take(10).ToList();
        //    #endregion

        //    #region 【业绩贡献占比-根据助理业绩获取条数输出】
        //    var orderDealInfo = await contentPlatFormOrderDealInfoService.GetPerformanceByDateAndLiveAnchorIdsAsync(query.startDate.Value, query.endDate.Value, new List<int>());
        //    //总业绩数据值
        //    var totalAchievement = orderDealInfo.Sum(x => x.Price);
        //    List<GetEmployeePerformanceRankingDto> getEmployeePerformanceRankingDtos = new List<GetEmployeePerformanceRankingDto>();
        //    foreach (var x in employeeDataList)
        //    {
        //        GetEmployeePerformanceRankingDto getEmployeePerformanceRankingDto = new GetEmployeePerformanceRankingDto();
        //        getEmployeePerformanceRankingDto.EmployeeName = x.EmployeeName;
        //        getEmployeePerformanceRankingDto.Performance = DecimalExtension.CalculateTargetComplete(x.Performance, totalAchievement).Value;
        //        getEmployeePerformanceRankingDtos.Add(getEmployeePerformanceRankingDto);
        //    }
        //    result.GetEmployeePerformanceRankings = getEmployeePerformanceRankingDtos.OrderByDescending(x => x.Performance).ToList();

        //    #endregion
        //    return result;
        //}

        #endregion
    }
}
