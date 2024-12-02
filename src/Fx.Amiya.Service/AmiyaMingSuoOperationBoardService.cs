using Fx.Amiya.Dto;
using Fx.Amiya.Dto.AmiyaLivingOperationBoard;
using Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Input;
using Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Result;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.Performance;
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
    public class AmiyaMingSuoOperationBoardService : IAmiyaMingSuoOperationBoardService
    {
        private readonly ILiveAnchorBaseInfoService liveAnchorBaseInfoService;
        private readonly IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService;
        private readonly ILiveAnchorService liveAnchorService;
        private readonly IHospitalInfoService hospitalInfoService;
        private readonly IShoppingCartRegistrationService shoppingCartRegistrationService;
        private readonly IContentPlateFormOrderService contentPlateFormOrderService;
        private readonly IAmiyaEmployeeService amiyaEmployeeService;
        private readonly IEmployeePerformanceTargetService employeePerformanceTargetService;
        private readonly IContentPlatformOrderSendService contentPlatformOrderSendService;
        private readonly IDalEmployeePerformanceTarget dalEmployeePerformanceTarget;
        private readonly IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo;
        private readonly IHealthValueService _healthValueService;
        private readonly IDalContentPlatformOrderSend _dalContentPlatformOrderSend;
        private readonly IDalShoppingCartRegistration _dalShoppingCartRegistration;
        private readonly IDalLiveAnchorMonthlyTargetAfterLiving dalLiveAnchorMonthlyTargetAfterLiving;
        private readonly ILiveAnchorMonthlyTargetAfterLivingService liveAnchorMonthlyTargetAfterLivingService;

        public AmiyaMingSuoOperationBoardService(ILiveAnchorBaseInfoService liveAnchorBaseInfoService, IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService, ILiveAnchorService liveAnchorService, IHospitalInfoService hospitalInfoService, IShoppingCartRegistrationService shoppingCartRegistrationService, IContentPlateFormOrderService contentPlateFormOrderService, IAmiyaEmployeeService amiyaEmployeeService, IEmployeePerformanceTargetService employeePerformanceTargetService, IContentPlatformOrderSendService contentPlatformOrderSendService, IDalEmployeePerformanceTarget dalEmployeePerformanceTarget, IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo, IHealthValueService healthValueService, IDalContentPlatformOrderSend dalContentPlatformOrderSend, IDalShoppingCartRegistration dalShoppingCartRegistration, ILiveAnchorMonthlyTargetAfterLivingService liveAnchorMonthlyTargetAfterLivingService, IDalLiveAnchorMonthlyTargetAfterLiving dalLiveAnchorMonthlyTargetAfterLiving)
        {
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
            this.contentPlatFormOrderDealInfoService = contentPlatFormOrderDealInfoService;
            this.liveAnchorService = liveAnchorService;
            this.hospitalInfoService = hospitalInfoService;
            this.shoppingCartRegistrationService = shoppingCartRegistrationService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.amiyaEmployeeService = amiyaEmployeeService;
            this.employeePerformanceTargetService = employeePerformanceTargetService;
            this.contentPlatformOrderSendService = contentPlatformOrderSendService;
            this.dalEmployeePerformanceTarget = dalEmployeePerformanceTarget;
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            _healthValueService = healthValueService;
            _dalContentPlatformOrderSend = dalContentPlatformOrderSend;
            _dalShoppingCartRegistration = dalShoppingCartRegistration;
            this.liveAnchorMonthlyTargetAfterLivingService = liveAnchorMonthlyTargetAfterLivingService;
            this.dalLiveAnchorMonthlyTargetAfterLiving = dalLiveAnchorMonthlyTargetAfterLiving;
        }
        /// <summary>
        /// 获取总业绩
        /// </summary>
        /// <returns></returns>
        public async Task<OperationMingSuoAchievementDataDto> GetTotalAchievementAndDateScheduleAsync(QueryOperationDataDto query)
        {
            OperationMingSuoAchievementDataDto result = new OperationMingSuoAchievementDataDto();
            var dateSchedule = DateTimeExtension.GetDatetimeSchedule(query.endDate.Value).FirstOrDefault();
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                baseLiveanchorList = baseLiveanchorList.Where(x => x.Id == query.keyWord).ToList();
            }
            var baseIds = baseLiveanchorList.Select(e => e.Id).ToList();

            //获取各个平台的主播ID
            var LiveAnchorInfoList = await liveAnchorService.GetLiveAnchorListByBaseInfoIdListAsync(baseIds);
            var LiveAnchorInfo = LiveAnchorInfoList.Select(x => x.Id).ToList();
            var sequentialDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month == 0 ? 1 : query.endDate.Value.Month);

            //获取目标
            var target = await liveAnchorMonthlyTargetAfterLivingService.GetPerformanceTargetAsync(query.endDate.Value.Year, query.endDate.Value.Month, LiveAnchorInfo);
            #region【线索】


            var shoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDateAndBaseIdsAsync(sequentialDate.StartDate, sequentialDate.EndDate, baseIds);

            var todayshoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDateAndBaseIdsAsync(Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day), DateTime.Now, baseIds);

            var shoppingCartRegistionYearOnYear = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDateAndBaseIdsAsync(sequentialDate.LastYearThisMonthStartDate, sequentialDate.LastYearThisMonthEndDate, baseIds);

            var shoppingCartRegistionChain = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDateAndBaseIdsAsync(sequentialDate.LastMonthStartDate, sequentialDate.LastMonthEndDate, baseIds);

            var curClue = shoppingCartRegistionData.Count();
            var ClueYearOnYear = shoppingCartRegistionYearOnYear.Count();
            var ClueChainRatio = shoppingCartRegistionChain.Count();
            result.TodayTotalClues = todayshoppingCartRegistionData.Count();
            result.TotalCluesCompleteRate = DecimalExtension.CalculateTargetComplete(curClue, target.CluesTarget);
            result.TotalCluesYearOnYear = DecimalExtension.CalculateChain(curClue, ClueYearOnYear);
            result.TotalCluesChainRatio = DecimalExtension.CalculateChain(curClue, ClueChainRatio);

            if (query.startDate.Value.Year == query.endDate.Value.Year && query.startDate.Value.Month == query.endDate.Value.Month)
            {
                result.TotalClues = curClue;
            }
            else
            {
                //非本月数据总业绩取累计数据
                var sumShoppingCartRegistionData = await shoppingCartRegistrationService.GetShoppingCartRegistionDataByRecordDateAndBaseIdsAsync(query.startDate.Value, query.endDate.Value, baseIds);
                result.TotalClues = sumShoppingCartRegistionData.Count();
            }
            #endregion

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

            order = order.Where(x => LiveAnchorInfo.Contains(x.LiveAnchorId.Value)).ToList();
            //业绩折线图
            var dateList = order.GroupBy(x => x.CreateDate.Day).Select(x => new OerationTotalAchievementBrokenLineListDto
            {
                Time = x.Key,
                TotalCustomerPerformance = x.Sum(e => e.Price),

            });
            List<OerationTotalAchievementBrokenLineListDto> GroupList = new List<OerationTotalAchievementBrokenLineListDto>();
            for (int i = 1; i < dateSchedule.Key + 1; i++)
            {
                OerationTotalAchievementBrokenLineListDto item = new OerationTotalAchievementBrokenLineListDto();
                item.Time = i;
                item.TotalCustomerPerformance = dateList.Where(e => e.Time == i).Select(e => e.TotalCustomerPerformance).SingleOrDefault();

                GroupList.Add(item);
            }
            result.TotalPerformanceBrokenLineList = GroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = DecimalExtension.ChangePriceToTenThousand(e.TotalCustomerPerformance) }).OrderBy(e => Convert.ToInt32(e.date)).ToList();
            //线索折线图
            var clueList = shoppingCartRegistionData.GroupBy(x => x.RecordDate.Day).Select(x => new OerationTotalAchievementBrokenLineListDto
            {
                Time = x.Key,
                TotalCustomerPerformance = x.Count(),
            });
            List<OerationTotalAchievementBrokenLineListDto> ClueGroupList = new List<OerationTotalAchievementBrokenLineListDto>();
            for (int i = 1; i < dateSchedule.Key + 1; i++)
            {
                OerationTotalAchievementBrokenLineListDto item = new OerationTotalAchievementBrokenLineListDto();
                item.Time = i;
                item.TotalCustomerPerformance = clueList.Where(e => e.Time == i).Select(e => e.TotalCustomerPerformance).SingleOrDefault();

                ClueGroupList.Add(item);
            }
            result.TotalCluesBrokenLineList = ClueGroupList.Select(e => new PerformanceBrokenLineListInfoDto { date = e.Time.ToString(), Performance = e.TotalCustomerPerformance }).OrderBy(e => Convert.ToInt32(e.date)).ToList();


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
        /// 获取名索漏斗图数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ResultMingSuoOperationDataDto> GetMingSuoFilterDataAsync(QueryMingSuoFilterDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            ResultMingSuoOperationDataDto filterData = new ResultMingSuoOperationDataDto();
            AssistantNewCustomerOperationDataDto NewCustomerDataDto = new AssistantNewCustomerOperationDataDto();
            NewCustomerDataDto.newCustomerOperationDataDetails = new List<AssistantNewCustomerOperationDataDetails>();

            AssistantOldCustomerOperationDataDto OldCustomerDataDto = new AssistantOldCustomerOperationDataDto();
            var healthValueList = await _healthValueService.GetValidListAsync();
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                baseLiveanchorList = baseLiveanchorList.Where(x => x.Id == query.LiveAnchorBaseId).ToList();
            }
            var baseIds = baseLiveanchorList.Select(e => e.Id).ToList();

            //获取各个平台的主播ID
            var LiveAnchorInfo = await liveAnchorService.GetLiveAnchorListByBaseInfoIdListAsync(baseIds);
            #region【小黄车数据】
            var employeeInfo = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(baseIds);
            var empIdList = employeeInfo.Select(x => x.Id).ToList();
            //小黄车数据
            var baseBusinessPerformance = await shoppingCartRegistrationService.GetBeforeLiveShopCartRegisterPerformanceByAssistantIdListAndBaseIdListAsync(selectDate.StartDate, selectDate.EndDate, baseIds, new List<int>(), BelongChannel.LiveBefore);
            #endregion
            string z = "";
            foreach (var k in baseBusinessPerformance)
            {
                z += k.Id + ",";
            }

            #region 新客数据
            #region 【线索】
            //线索
            AssistantNewCustomerOperationDataDetails clues = new AssistantNewCustomerOperationDataDetails();
            clues.Key = "Clues";
            clues.Name = "线索量";
            clues.Value = baseBusinessPerformance.Count();
            NewCustomerDataDto.newCustomerOperationDataDetails.Add(clues);
            #endregion
            #region 【分诊】

            //分诊
            AssistantNewCustomerOperationDataDetails consulationdetails = new AssistantNewCustomerOperationDataDetails();
            consulationdetails.Key = "Consulation";
            consulationdetails.Name = "分诊量";
            consulationdetails.Value = baseBusinessPerformance.Where(x => x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            NewCustomerDataDto.newCustomerOperationDataDetails.Add(consulationdetails);

            //线索有效率
            NewCustomerDataDto.ClueEffectiveRate = DecimalExtension.CalculateTargetComplete(consulationdetails.Value, clues.Value);
            NewCustomerDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ClueEffictiveRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【加v】
            AssistantNewCustomerOperationDataDetails addWechatdetails = new AssistantNewCustomerOperationDataDetails();
            //加v
            addWechatdetails.Key = "AddWeChat";
            addWechatdetails.Name = "加v量";
            addWechatdetails.Value = baseBusinessPerformance.Where(x => x.IsAddWeChat == true && x.AssignEmpId != 0 && x.AssignEmpId.HasValue && x.IsReturnBackPrice == false).Count();
            NewCustomerDataDto.newCustomerOperationDataDetails.Add(addWechatdetails);

            //加v率
            NewCustomerDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails.Value, consulationdetails.Value);
            NewCustomerDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 获取部门基础数据
            bool isCurrent = true;
            if (query.History)
                isCurrent = false;
            var depeartPhoneList = baseBusinessPerformance.Select(e => e.Phone).ToList();
            var allOrderPerformance = await contentPlateFormOrderService.GetBeforeLiveDepartOrderSendAndDealDataByAssistantIdListAsync(selectDate.StartDate, selectDate.EndDate, empIdList, depeartPhoneList, isCurrent);

            #endregion

            #region 【派单】
            AssistantNewCustomerOperationDataDetails sendOrderdetails = new AssistantNewCustomerOperationDataDetails();
            //派单
            sendOrderdetails.Key = "SendOrder";
            sendOrderdetails.Name = "派单量";
            sendOrderdetails.Value = allOrderPerformance.SendOrderNum;
            NewCustomerDataDto.newCustomerOperationDataDetails.Add(sendOrderdetails);

            //派单率
            NewCustomerDataDto.SendOrderRate = DecimalExtension.CalculateTargetComplete(sendOrderdetails.Value, addWechatdetails.Value);
            NewCustomerDataDto.SendOrderRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "SendOrderRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【上门】
            AssistantNewCustomerOperationDataDetails visitdetails = new AssistantNewCustomerOperationDataDetails();
            //上门
            visitdetails.Key = "ToHospital";
            visitdetails.Name = "上门量";
            visitdetails.Value = allOrderPerformance.VisitNum;
            NewCustomerDataDto.newCustomerOperationDataDetails.Add(visitdetails);

            //上门率
            NewCustomerDataDto.ToHospitalRate = DecimalExtension.CalculateTargetComplete(visitdetails.Value, sendOrderdetails.Value);
            NewCustomerDataDto.ToHospitalRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "ToHospitalRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【成交】

            AssistantNewCustomerOperationDataDetails dealData = new AssistantNewCustomerOperationDataDetails();
            //成交
            dealData.Key = "Deal";
            dealData.Name = "成交量";
            dealData.Value = allOrderPerformance.DealNum;
            NewCustomerDataDto.newCustomerOperationDataDetails.Add(dealData);

            //成交率
            NewCustomerDataDto.DealRate = DecimalExtension.CalculateTargetComplete(dealData.Value, visitdetails.Value);
            NewCustomerDataDto.DealRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "DealRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();

            #endregion
            filterData.NewCustomerData = NewCustomerDataDto;

            #endregion


            #region 老客数据
            var oldCustomerData = await contentPlateFormOrderService.GetOldCustomerBuyAgainByMonthAsync(selectDate.StartDate, null, "", LiveAnchorInfo.Select(x => x.Id).ToList());
            OldCustomerDataDto.TotalDealPeople = oldCustomerData.TotalDealCustomer;
            OldCustomerDataDto.SecondDealPeople = oldCustomerData.SecondDealCustomer;
            OldCustomerDataDto.ThirdDealPeople = oldCustomerData.ThirdDealCustomer;
            OldCustomerDataDto.FourthDealCustomer = oldCustomerData.FourthDealCustomer;
            OldCustomerDataDto.FifThOrMoreOrMoreDealCustomer = oldCustomerData.FifThOrMoreOrMoreDealCustomer;
            OldCustomerDataDto.SecondDealCycle = oldCustomerData.SecondDealCycle;
            OldCustomerDataDto.ThirdDealCycle = oldCustomerData.ThirdDealCycle;
            OldCustomerDataDto.FourthDealCycle = oldCustomerData.FourthDealCycle;
            OldCustomerDataDto.FifthDealCycle = oldCustomerData.FifthDealCycle;


            //OldCustomerDataDto.SecondTimeBuyRate = CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.SecondDealPeople), Convert.ToDecimal(OldCustomerDataDto.TotalDealPeople)).Value;
            OldCustomerDataDto.SecondTimeBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.SecondDealPeople), Convert.ToDecimal(OldCustomerDataDto.TotalDealPeople)).Value; ;

            //OldCustomerDataDto.ThirdTimeBuyRate = CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.ThirdDealPeople), Convert.ToDecimal(OldCustomerDataDto.SecondDealPeople)).Value;
            OldCustomerDataDto.ThirdTimeBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.ThirdDealPeople), Convert.ToDecimal(OldCustomerDataDto.TotalDealPeople)).Value;

            // OldCustomerDataDto.FourthTimeBuyRate = CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.FourthDealCustomer), Convert.ToDecimal(OldCustomerDataDto.ThirdDealPeople)).Value;
            OldCustomerDataDto.FourthTimeBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.FourthDealCustomer), Convert.ToDecimal(OldCustomerDataDto.TotalDealPeople)).Value;

            //OldCustomerDataDto.FifthTimeOrMoreBuyRate = CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.FifThOrMoreOrMoreDealCustomer), Convert.ToDecimal(OldCustomerDataDto.FourthDealCustomer)).Value;
            OldCustomerDataDto.FifthTimeOrMoreBuyRateProportion = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.FifThOrMoreOrMoreDealCustomer), Convert.ToDecimal(OldCustomerDataDto.TotalDealPeople)).Value;

            OldCustomerDataDto.BuyRate = DecimalExtension.CalculateTargetComplete(Convert.ToDecimal(OldCustomerDataDto.FifThOrMoreOrMoreDealCustomer + OldCustomerDataDto.FourthDealCustomer + OldCustomerDataDto.ThirdDealPeople + OldCustomerDataDto.SecondDealPeople), Convert.ToDecimal(OldCustomerDataDto.TotalDealPeople)).Value;

            filterData.OldCustomerData = OldCustomerDataDto;

            #endregion


            return filterData;
        }


        /// <summary>
        /// 获取医生转化周期柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<MingSuoTransformCycleDataDto> GetLiveAnchorTransformCycleDataAsync(QueryOperationDataDto query)
        {
            MingSuoTransformCycleDataDto data = new MingSuoTransformCycleDataDto();
            var seqDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.endDate.Value.Year, query.endDate.Value.Month);
            var liveanchorIds = new List<string>();
            var nameList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.keyWord))
            {
                nameList = nameList.Where(x => x.Id == query.keyWord).ToList();
            }

            liveanchorIds = nameList.Select(x => x.Id).ToList();
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
            dataList = dataList.OrderBy(x => x.IntervalDays).ToList();
            //转化周期数据
            var res1 = dataList.GroupBy(e => e.LiveAnchorBaseId).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count());
                var total = dataList.Sum(e => e.IntervalDays);
                var resData = e.OrderBy(e => e.IntervalDays).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                nameList.Where(a => a.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其它",
                resData.Count() == 0 ? 0 : resData.Sum(e => e.IntervalDays) / (resData.Count())
             );
            }).OrderBy(e => e.Value).ToList();
            //当前主播转化周期
            var currentLiveAnchorListCount = dataList.Where(e => e.LiveAnchorBaseId == query.keyWord).Count();
            var currentLiveAnchorList = dataList.Where(e => e.LiveAnchorBaseId == query.keyWord).OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCount * 0.8));

            var currentLiveAnchorListCountAllData = dataList.Count();
            var currentLiveAnchorListAllData = dataList.OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCountAllData * 0.8));

            int currentEffectiveDays = 0;
            int currentEffectiveCount = 0;
            int currentPotionelDays = 0;
            int currentPotionelCount = 0;

            if (!string.IsNullOrEmpty(query.keyWord))
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
            data.ThisMonthSendCycle = DecimalExtension.CalAvg(currentEffectiveDays, currentEffectiveCount);
            data.HistorySendCycle = DecimalExtension.CalAvg(currentPotionelDays, currentPotionelCount);
            data.SendCycleData = res1.OrderByDescending(x => x.Value).ToList();

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
            var currentLiveAnchorListCount2 = dataList2.Where(e => e.LiveAnchorBaseId == query.keyWord).Count();
            var currentLiveAnchorList2 = dataList2.Where(e => e.LiveAnchorBaseId == query.keyWord).OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCount2 * 0.6));

            var currentLiveAnchorListCount2AllData = dataList2.Count();
            var currentLiveAnchorList2AllData = dataList2.OrderBy(e => e.IntervalDays).Skip(0).Take((int)(currentLiveAnchorListCount2AllData * 0.6));

            int currentEffectiveDays2 = 0;
            int currentEffectiveCount2 = 0;
            int currentPotionelDays2 = 0;
            int currentPotionelCount2 = 0;

            if (!string.IsNullOrEmpty(query.keyWord))
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
            data.ThisMonthSendCycle = DecimalExtension.CalAvg(currentEffectiveDays2, currentEffectiveCount2);
            data.ThisMonthToHospitalCycle = DecimalExtension.CalAvg(currentPotionelDays2, currentPotionelCount2);

            data.ToHospitalCycleData = res2.OrderByDescending(x => x.Value).ToList();


            #endregion



            return data;
        }

        /// <summary>
        /// 名索医生线索和业绩目标完成率
        /// </summary>
        /// <returns></returns>
        public async Task<MingSuoClueTargetDataDto> GetMingSuoClueAndPerformanceTargetDataAsync(QueryMingSuoCompleteDataDto query)
        {
            MingSuoClueTargetDataDto data = new MingSuoClueTargetDataDto();
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                baseLiveanchorList = baseLiveanchorList.Where(x => x.Id == query.BaseLiveAnchorId).ToList();
            }
            var baseLiveanchorIdList = baseLiveanchorList.Select(e => e.Id).ToList();

            var seqDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            //var customerServiceIds = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(baseLiveanchorIdList);

            var target = await dalLiveAnchorMonthlyTargetAfterLiving.GetAll()
                .Where(e => baseLiveanchorIdList.Contains(e.LiveAnchor.LiveAnchorBaseId) && e.Month == query.EndDate.Month && e.Year == query.EndDate.Year)
                .Where(e => e.CluesTarget > 1)
                .Select(e => new { e.LiveAnchor.LiveAnchorBaseId, e.CluesTarget, e.PerformanceTarget })
                .GroupBy(e => e.LiveAnchorBaseId)
                .Select(e => new
                {
                    BaseLiveanchorId = e.Key,
                    ClueTarget = e.Sum(e => e.CluesTarget),
                    PerformanceTarget = e.Sum(e => e.PerformanceTarget)
                }).ToListAsync();
            //线索目标完成率
            var clueData = _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false && e.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.RecordDate >= seqDate.StartDate && e.RecordDate < seqDate.EndDate)
                .Where(e => baseLiveanchorIdList.Contains(e.BaseLiveAnchorId))
                .Select(e => new
                {
                    BaseLiveAnchorId = e.BaseLiveAnchorId,
                }).ToList();
            var clueTargetData = clueData.GroupBy(e => e.BaseLiveAnchorId).Select(e =>
            {
                var name = baseLiveanchorList.Where(x => x.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其他";
                var t = target.Where(x => x.BaseLiveanchorId == e.Key).FirstOrDefault()?.ClueTarget ?? 0;
                var targetComplete = DecimalExtension.CalculateTargetComplete(e.Count(), t).Value;
                return new KeyValuePair<string, decimal>(name, targetComplete);
            }).ToList();
            data.ClueTargetComplete = clueTargetData.OrderByDescending(x => x.Value).ToList();

            //业绩目标完成率
            data.PerformanceTargetComplete = new List<BaseKeyValueDto<string, decimal>>();
            foreach (var baseLiveAnchor in baseLiveanchorList)
            {
                var liveAnchorTarget = target.Where(e => e.BaseLiveanchorId == baseLiveAnchor.Id).FirstOrDefault()?.PerformanceTarget ?? 0;
                var liveAnchorIds = await liveAnchorService.GetAllLiveAnchorListByBaseInfoId(baseLiveAnchor.Id);
                var order = await contentPlatFormOrderDealInfoService.GetSendAndDealPerformanceAsync(seqDate.StartDate, seqDate.EndDate, null, liveAnchorIds.Select(x => x.Id).ToList());
                BaseKeyValueDto<string, decimal> liveAnchorPerfomances = new BaseKeyValueDto<string, decimal>();
                liveAnchorPerfomances.Key = baseLiveAnchor.LiveAnchorName;
                liveAnchorPerfomances.Value = DecimalExtension.CalculateTargetComplete(order.Sum(x => x.Price), liveAnchorTarget).Value;
                data.PerformanceTargetComplete.Add(liveAnchorPerfomances);

            }
            data.PerformanceTargetComplete = data.PerformanceTargetComplete.OrderByDescending(x => x.Value).ToList();
            return data;
        }

        /// <summary>
        /// 获取助理目标完成率和业绩占比
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<AssiatantTargetCompleteAndPerformanceRateDto> GetAssiatantTargetCompleteAndPerformanceRateDataAsync(QueryMingSuoAssistantPerformanceDto query)
        {
            AssiatantTargetCompleteAndPerformanceRateDto result = new AssiatantTargetCompleteAndPerformanceRateDto();
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                baseLiveanchorList = baseLiveanchorList.Where(x => x.Id == query.LiveAnchorBaseId).ToList();
            }
            var baseLiveanchorIdList = baseLiveanchorList.Select(e => e.Id).ToList();
            var assistantIdAndNameList = await amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(baseLiveanchorIdList);
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
            result.TargetCompleteData = result.TargetCompleteData.OrderByDescending(x => x.Value).ToList();
            result.PerformanceRateData = result.PerformanceRateData.OrderByDescending(x => x.Value).ToList();
            return result;
        }

        /// <summary>
        /// 名索IP获客占比
        /// </summary>
        /// <returns></returns>
        public async Task<MingSuoContentplatformClueDataDto> GetMingSuoContentplatformClueDataAsync(QueryMingSuoAssistantPerformanceDto query)
        {
            MingSuoContentplatformClueDataDto MingSuoData = new MingSuoContentplatformClueDataDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                baseLiveanchorList = baseLiveanchorList.Where(x => x.Id == query.LiveAnchorBaseId).ToList();
            }
            var baseLiveAnchorIds = baseLiveanchorList.Select(x => x.Id).ToList();
            var baseData = await _dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate && e.BelongChannel == (int)BelongChannel.LiveBefore && e.IsReturnBackPrice == false)
                .Where(e => baseLiveAnchorIds.Contains(e.BaseLiveAnchorId))
                .Select(e => new
                {
                    id = e.Id,
                    ContentPlatformName = e.Contentplatform.ContentPlatformName,
                    ContentPlatformId = e.ContentPlatFormId,
                    LiveAnchorName = e.LiveAnchor.Name,
                }).ToListAsync();
            var totalCount = baseData.Count;
            MingSuoData.ContentPlatformTotalClue = totalCount;
            MingSuoData.ContentPlatformClueRate = baseData.GroupBy(e => e.LiveAnchorName).Select(e => new LivingContentplatformClueDataItemDto
            {
                Name = e.Key,
                Value = DecimalExtension.CalculateTargetComplete(e.Count(), totalCount).Value,
                Performance = e.Count()
            }).ToList();
            return MingSuoData;
        }
        /// <summary>
        /// 名索IP业绩占比
        /// </summary>
        /// <returns></returns>
        public async Task<MingSuoContentplatformPerformanceDataDto> GetMingSuoContentplatformPerformanceDataAsync(QueryMingSuoAssistantPerformanceDto query)
        {
            MingSuoContentplatformPerformanceDataDto MingSuoData = new MingSuoContentplatformPerformanceDataDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetMingSuoLiveAnchorAsync();
            if (!string.IsNullOrEmpty(query.LiveAnchorBaseId))
            {
                baseLiveanchorList = baseLiveanchorList.Where(x => x.Id == query.LiveAnchorBaseId).ToList();
            }
            var performanceList = dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.LiveBefore)
                .Where(e => e.IsDeal == true && e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => baseLiveanchorList.Select(x => x.Id).ToList().Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                   .Select(e => new
                   {
                       Id = e.Id,
                       conteid = e.ContentPlatFormOrderId,
                       LiveAnchorName = e.ContentPlatFormOrder.LiveAnchor.Name,
                       ContentPlateformId = e.ContentPlatFormOrder.ContentPlateformId,
                       ContentPlatformName = e.ContentPlatFormOrder.Contentplatform.ContentPlatformName,
                       Price = e.Price
                   }).ToList();
            var totalPerformance = performanceList.Sum(e => e.Price);
            MingSuoData.ContentPlatformTotalPerformance = ChangePriceToTenThousand(totalPerformance);
            MingSuoData.ContentPlatformPerformanceRate = performanceList.GroupBy(e => e.LiveAnchorName)
                .Select(e => new LivingContentplatformPerformanceDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), totalPerformance).Value,
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
                }).ToList();
            return MingSuoData;
        }
        #region 公共类

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

        private decimal ChangePriceToTenThousand(decimal performance, int unit = 1)
        {
            if (performance == 0m)
                return 0;
            var result = Math.Round((performance / 10000), unit, MidpointRounding.AwayFromZero);
            return result;
        }
        #endregion
    }
}
