using Fx.Amiya.Dto.AmiyaLivingOperationBoard;
using Fx.Amiya.Dto.AmiyaLivingOperationBoard.Input;
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
    public class AmiyaLivingOperationBoardService : IAmiyaLivingOperationBoardService
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
        private readonly IDalLiveAnchorMonthlyTargetLiving dalLiveAnchorMonthlyTargetLiving;

        public AmiyaLivingOperationBoardService(ILiveAnchorMonthlyTargetBeforeLivingService liveAnchorMonthlyTargetBeforeLivingService, ILiveAnchorMonthlyTargetLivingService liveAnchorMonthlyTargetLivingService, ILiveAnchorBaseInfoService liveAnchorBaseInfoService, IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService, ILiveAnchorService liveAnchorService, IHospitalInfoService hospitalInfoService, IShoppingCartRegistrationService shoppingCartRegistrationService, IContentPlateFormOrderService contentPlateFormOrderService, IAmiyaEmployeeService amiyaEmployeeService, IEmployeePerformanceTargetService employeePerformanceTargetService, IContentPlatformOrderSendService contentPlatformOrderSendService, ILiveAnchorMonthlyTargetAfterLivingService liveAnchorMonthlyTargetAfterLivingService, IDalEmployeePerformanceTarget dalEmployeePerformanceTarget, IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo, IHealthValueService healthValueService, IDalContentPlatformOrderSend dalContentPlatformOrderSend, IDalShoppingCartRegistration dalShoppingCartRegistration, IDalLiveAnchorMonthlyTargetLiving dalLiveAnchorMonthlyTargetLiving)
        {
            this.liveAnchorMonthlyTargetBeforeLivingService = liveAnchorMonthlyTargetBeforeLivingService;
            this.liveAnchorMonthlyTargetLivingService = liveAnchorMonthlyTargetLivingService;
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
            this.contentPlatFormOrderDealInfoService = contentPlatFormOrderDealInfoService;
            this.liveAnchorService = liveAnchorService;
            this.hospitalInfoService = hospitalInfoService;
            this.shoppingCartRegistrationService = shoppingCartRegistrationService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.amiyaEmployeeService = amiyaEmployeeService;
            this.employeePerformanceTargetService = employeePerformanceTargetService;
            this.contentPlatformOrderSendService = contentPlatformOrderSendService;
            this.liveAnchorMonthlyTargetAfterLivingService = liveAnchorMonthlyTargetAfterLivingService;
            this.dalEmployeePerformanceTarget = dalEmployeePerformanceTarget;
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            _healthValueService = healthValueService;
            _dalContentPlatformOrderSend = dalContentPlatformOrderSend;
            _dalShoppingCartRegistration = dalShoppingCartRegistration;
            this.dalLiveAnchorMonthlyTargetLiving = dalLiveAnchorMonthlyTargetLiving;
        }

        /// <summary>
        /// 直播中客资和新客业绩
        /// </summary>
        /// <returns></returns>
        public async Task<LivingCustomerAndPerformanceDataDto> GetLivingCustomerAndPerformanceDataAsync(QueryLivingDataDto query)
        {
            var selectDate = DateTimeExtension.GetSequentialDateByStartAndEndDate(query.EndDate.Year, query.EndDate.Month);
            LivingCustomerAndPerformanceDataDto data = new LivingCustomerAndPerformanceDataDto();
            var baseData = _dalShoppingCartRegistration.GetAll()
               .Where(e => e.IsReturnBackPrice == false)
               .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.BaseLiveAnchorId == query.BaseLiveAnchorId)
               .Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate)
               .Where(e => e.BelongChannel == (int)BelongChannel.Living)
               .Select(e => new { e.Phone,e.RecordDate }).ToList();
            var currentPhoneList= baseData.Select(e=>e.Phone).ToList();
            var lastData = _dalShoppingCartRegistration.GetAll()
              .Where(e => e.IsReturnBackPrice == false)
              .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.BaseLiveAnchorId == query.BaseLiveAnchorId)
              .Where(e => e.RecordDate >= selectDate.LastMonthStartDate && e.RecordDate < selectDate.LastMonthEndDate)
              .Where(e => e.BelongChannel == (int)BelongChannel.Living)
              .Count();
            var historyData = _dalShoppingCartRegistration.GetAll()
              .Where(e => e.IsReturnBackPrice == false)
              .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.BaseLiveAnchorId == query.BaseLiveAnchorId)
              .Where(e => e.RecordDate >= selectDate.LastYearThisMonthStartDate && e.RecordDate < selectDate.LastYearThisMonthEndDate)
              .Where(e => e.BelongChannel == (int)BelongChannel.Living)
              .Count();
            var target =  dalLiveAnchorMonthlyTargetLiving.GetAll()
                .Where(e => e.LiveAnchor.LiveAnchorBaseId == query.BaseLiveAnchorId && e.Month == query.EndDate.Month && e.Year == query.EndDate.Year)
                .Where(e => e.ConsultationTarget > 1)
                .Sum(e => e.ConsultationTarget);
            var performance = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == query.BaseLiveAnchorId)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.IsDeal == true)
                .Where(e => e.IsOldCustomer == false)
                .Select(e=>new {e.Price,e.CreateDate,e.ContentPlatFormOrder.Phone });
            var lastPerformance = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= selectDate.LastMonthStartDate && e.CreateDate < selectDate.LastMonthEndDate)
                .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == query.BaseLiveAnchorId)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.IsDeal == true)
                .Where(e => e.IsOldCustomer == false)
                .Sum(e => e.Price);
            var historyPerformance = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= selectDate.LastYearThisMonthStartDate && e.CreateDate < selectDate.LastYearThisMonthEndDate)
                .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == query.BaseLiveAnchorId)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.IsDeal == true)
                .Where(e => e.IsOldCustomer == false)
                .Sum(e => e.Price);
            data.ClueCount = baseData.Count();
            data.CurrentClueCount = baseData.Where(e => e.RecordDate.Date == DateTime.Now.Date).Count();
            data.ClueTargetCompleteRate = DecimalExtension.CalculateTargetComplete(data.ClueCount,target).Value;
            data.ClueChain = DecimalExtension.CalculateTargetComplete(data.ClueCount, lastData).Value;
            data.ClueYearOnYear = DecimalExtension.CalculateTargetComplete(data.ClueCount, historyData).Value;
            
            data.Performance = performance.Sum(e=>e.Price);
            data.CurrentPerformance = performance.Where(e => e.CreateDate.Date == DateTime.Now.Date).Sum(e=>e.Price);
            data.PerformanceChain = DecimalExtension.CalculateTargetComplete(data.Performance,lastPerformance).Value;
            data.PerformanceYearOnYear = DecimalExtension.CalculateTargetComplete(data.Performance, historyPerformance).Value;
            var currentPerformance=performance.Where(e=>currentPhoneList.Contains(e.Phone)).Sum(e => e.Price);
            data.CurrentMontPerformance = currentPerformance;
            return data;
        }
        /// <summary>
        /// 直播中客资和业绩折线图
        /// </summary>
        /// <returns></returns>
        public async Task<LivingCustomerAndPerformanceBrokenLineDataDto> GetLivingCustomerAndPerformanceBrokenLineDataAsync(QueryLivingDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate,query.EndDate);
            LivingCustomerAndPerformanceBrokenLineDataDto data = new LivingCustomerAndPerformanceBrokenLineDataDto();
            var baseData = _dalShoppingCartRegistration.GetAll()
               .Where(e => e.IsReturnBackPrice == false)
               .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.BaseLiveAnchorId == query.BaseLiveAnchorId)
               .Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate)
               .Where(e => e.BelongChannel == (int)BelongChannel.Living)
               .Select(e => new { e.Phone, e.RecordDate })
               .ToList();
            var performanceData = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                 .Where(e => string.IsNullOrEmpty(query.BaseLiveAnchorId) || e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == query.BaseLiveAnchorId)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.IsDeal == true)
                .Where(e => e.IsOldCustomer == false)
                .Select(e => new { e.Price, e.CreateDate }).ToListAsync();
            
            var clueData = baseData.GroupBy(e => e.RecordDate.Date.Day).Select(e => new PerformanceBrokenLineListInfoDto
            {
                date = e.Key.ToString(),
                Performance = e.Count()
            }).ToList();
            data.ClueData = FillDate(query.EndDate.Year,query.EndDate.Month, clueData);
            var performanceDataList = performanceData.GroupBy(e => e.CreateDate.Date.Day).Select(e => new PerformanceBrokenLineListInfoDto
            {
                date = e.Key.ToString(),
                Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
            }).ToList();
            data.PerformanceData = FillDate(query.EndDate.Year, query.EndDate.Month, performanceDataList);
            return data;
        }
        /// <summary>
        /// 直播中漏斗图数据
        /// </summary>
        /// <returns></returns>
        public async Task<LivingFilterDataDto> GetLivingFilterDataAsync(QueryLivingDataDto query)
        {
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            LivingFilterDataDto filterData = new LivingFilterDataDto();
            LivingFilterDataItemDto departmentDataDto = new LivingFilterDataItemDto();
            departmentDataDto.DataList = new List<LivingFilterDetailDataDto>();
            LivingFilterDataItemDto employeeDataDto = new LivingFilterDataItemDto();
            employeeDataDto.DataList = new List<LivingFilterDetailDataDto>();
            var healthValueList = await _healthValueService.GetValidListAsync();
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetAllLiveAnchorAsync(true);
            var baseLiveanchorIdList = baseLiveanchorList.Select(e => e.Id).ToList();
            #region【小黄车数据】
            //组小黄车数据
            var baseData = await _dalShoppingCartRegistration.GetAll()
                .Where(e => e.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate)
                .Where(e => e.IsReturnBackPrice == false)
                .Where(e=>baseLiveanchorIdList.Contains(e.BaseLiveAnchorId))
                .Select(e => new
                {
                    BaseLiveAnchorId = e.BaseLiveAnchorId,
                    AssignEmpId = e.AssignEmpId,
                    IsAddWechat = e.IsAddWeChat,
                    Phone = e.Phone,
                    BaseLiveanchorId = e.BaseLiveAnchorId,
                    RecordDate = e.RecordDate,
                }).ToListAsync();
            #endregion


            #region 部门数据
            #region 【分诊】

            //分诊
            LivingFilterDetailDataDto consulationdetails = new LivingFilterDetailDataDto();
            consulationdetails.Key = "Consulation";
            consulationdetails.Name = "分诊量";
            consulationdetails.Value = baseData.Where(x => x.AssignEmpId != 0 && x.AssignEmpId.HasValue).Count();
            departmentDataDto.DataList.Add(consulationdetails);
            #endregion

            #region 【加v】
            LivingFilterDetailDataDto addWechatdetails = new LivingFilterDetailDataDto();
            //加v
            addWechatdetails.Key = "AddWeChat";
            addWechatdetails.Name = "加v量";
            addWechatdetails.Value = baseData.Where(x => x.IsAddWechat == true).Count();
            departmentDataDto.DataList.Add(addWechatdetails);

            //加v率
            departmentDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails.Value, consulationdetails.Value);
            departmentDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 获取部门基础数据
            var depeartPhoneList = baseData.Select(e => e.Phone).ToList();
            var allOrderPerformance = await contentPlateFormOrderService.GetLivingOrderSendAndDealDataAsync(selectDate.StartDate, selectDate.EndDate, baseLiveanchorIdList, depeartPhoneList, query.IsCurrent);

            #endregion
            #region 【派单】
            LivingFilterDetailDataDto sendOrderdetails = new LivingFilterDetailDataDto();
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
            LivingFilterDetailDataDto visitdetails = new LivingFilterDetailDataDto();
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

            LivingFilterDetailDataDto dealData = new LivingFilterDetailDataDto();
            //成交
            dealData.Key = "Deal";
            dealData.Name = "成交量";
            dealData.Value = allOrderPerformance.DealNum;
            departmentDataDto.DataList.Add(dealData);

            //成交率
            departmentDataDto.DealRate = DecimalExtension.CalculateTargetComplete(dealData.Value, visitdetails.Value);
            departmentDataDto.DealRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "DealRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();

            #endregion
            filterData.Company = departmentDataDto;

            #endregion

            #region 分诊上门转化周期

            #region 分诊派单
            List<KeyValuePair<string, int>> dataList = new();
            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Where(e => e.IsMainHospital == true && e.SendDate >= selectDate.StartDate && e.SendDate < selectDate.EndDate)
               .Where(e => e.ContentPlatformOrder.BelongChannel == (int)BelongChannel.Living)
               .Where(e => baseLiveanchorIdList.Contains(e.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId))
               .Select(e => new { Phone = e.ContentPlatformOrder.Phone, EmpId = e.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId, SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Distinct().Select(e => e.Phone).ToList();
            if (query.IsCurrent)
            {
                var cartInfoList1 = baseData.Where(e => sendPhoneList.Contains(e.Phone)).ToList();
                dataList = (from send in sendInfoList
                            join cart in cartInfoList1
                            on send.Phone equals cart.Phone
                            select new KeyValuePair<string, int>(cart.BaseLiveAnchorId,
                                (send.SendDate - cart.RecordDate).Days)).ToList();
            }
            else
            {
                var historyPhone = sendPhoneList.Where(e => !baseData.Select(e => e.Phone).Contains(e));
                var cartInfoList1 = _dalShoppingCartRegistration.GetAll().Where(e => historyPhone.Contains(e.Phone)).Select(e => new { e.Phone, e.BaseLiveAnchorId, e.RecordDate }).ToList();
                dataList = (from send in sendInfoList
                            join cart in cartInfoList1
                            on send.Phone equals cart.Phone
                            select new KeyValuePair<string, int>(cart.BaseLiveAnchorId,
                                (send.SendDate - cart.RecordDate).Days)).ToList();
            }
            dataList.RemoveAll(e => e.Value < 0);
            var endIndex = DecimalExtension.CalTakeCount(dataList.Count(), 0.8m);
            var takeList = dataList.OrderBy(e => e.Value).Skip(0).Take(endIndex);
            //转化周期数据
            var sendCycle = DecimalExtension.CalAvg(takeList.Sum(e => e.Value), takeList.Count());
            departmentDataDto.SendCycle = sendCycle;
            #endregion

            #region 分诊上门
            List<KeyValuePair<string, int>> dataList2 = new();
            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                     .Where(e => baseLiveanchorIdList.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                    .Select(e => new
                    {
                        EmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.ToHospitalDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).ToList();

            if (query.IsCurrent)
            {
                var cartInfoList1 = baseData.Where(e => sendPhoneList.Contains(e.Phone)).ToList();
                dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList1
                             on deal.Phone equals cart.Phone
                             select new KeyValuePair<string, int>
                             (
                                 cart.BaseLiveAnchorId,
                                (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             )).ToList();
            }
            else
            {
                var historyPhone = dealPhoneList.Where(e => !baseData.Select(e => e.Phone).Contains(e));
                var cartInfoList1 = _dalShoppingCartRegistration.GetAll().Where(e => historyPhone.Contains(e.Phone)).Select(e => new { e.Phone, e.BaseLiveAnchorId, e.RecordDate }).ToList();
                dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList1
                             on deal.Phone equals cart.Phone
                             select new KeyValuePair<string, int>
                             (
                                 cart.BaseLiveAnchorId,
                                (deal.ToHospitalDate.Value - cart.RecordDate).Days
                             )).ToList();
            }
            dataList2.RemoveAll(e => e.Value < 0);
            var endIndex2 = DecimalExtension.CalTakeCount(dataList2.Count(), 0.6m);
            var takeList2 = dataList2.OrderBy(e => e.Value).Skip(0).Take(endIndex2);
            //转化周期数据
            var hospitalCycle = DecimalExtension.CalAvg(takeList2.Sum(e => e.Value), takeList2.Count());
            departmentDataDto.HospitalCycle = hospitalCycle;
            #endregion

            #endregion

            #region 组数据

            #region 【分诊】
            //当月数据
            var employeePhoneList = baseData.Where(e => e.BaseLiveAnchorId == query.BaseLiveAnchorId).Select(e => e.Phone).ToList();
            var addWechatOrderPerformance = await contentPlateFormOrderService.GetLivingOrderSendAndDealDataAsync(selectDate.StartDate, selectDate.EndDate, new List<string> { query.BaseLiveAnchorId }, employeePhoneList, query.IsCurrent);
            //分诊
            LivingFilterDetailDataDto consulationdetails2 = new LivingFilterDetailDataDto();
            consulationdetails2.Key = "Consulation";
            consulationdetails2.Name = "分诊量";
            consulationdetails2.Value = baseData.Where(e => e.BaseLiveAnchorId == query.BaseLiveAnchorId && e.AssignEmpId != 0 && e.AssignEmpId.HasValue).Count();
            employeeDataDto.DataList.Add(consulationdetails2);
            #endregion

            #region 【加v】
            LivingFilterDetailDataDto addWechatdetails2 = new LivingFilterDetailDataDto();
            //加v
            addWechatdetails2.Key = "AddWeChat";
            addWechatdetails2.Name = "加v量";
            addWechatdetails2.Value = baseData.Where(e => e.BaseLiveAnchorId == query.BaseLiveAnchorId).Count();
            employeeDataDto.DataList.Add(addWechatdetails2);

            //加v率
            employeeDataDto.AddWeChatRate = DecimalExtension.CalculateTargetComplete(addWechatdetails2.Value, consulationdetails2.Value);
            employeeDataDto.AddWeChatRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "AddWeChatHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();
            #endregion

            #region 【派单】
            LivingFilterDetailDataDto sendOrderdetails2 = new LivingFilterDetailDataDto();
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
            LivingFilterDetailDataDto visitdetails2 = new LivingFilterDetailDataDto();
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

            LivingFilterDetailDataDto deal2 = new LivingFilterDetailDataDto();
            //成交
            deal2.Key = "Deal";
            deal2.Name = "成交量";
            deal2.Value = addWechatOrderPerformance.DealNum;
            employeeDataDto.DataList.Add(deal2);

            //成交率
            employeeDataDto.DealRate = DecimalExtension.CalculateTargetComplete(deal2.Value, visitdetails2.Value);
            employeeDataDto.DealRateHealthValueThisMonth = healthValueList.Where(e => e.Key == "DealRateHealthValueThisMonth").Select(e => e.Rate).FirstOrDefault();

            #endregion

            filterData.CurrentGroup = employeeDataDto;

            #endregion
            #region 派单转化周期
            var employeeEndIndex = DecimalExtension.CalTakeCount(dataList.Where(e => e.Key == query.BaseLiveAnchorId).Count(), 0.8m);
            var employeeSendData = dataList.Where(e => e.Key == query.BaseLiveAnchorId).OrderBy(e => e.Value).ToList().Skip(0).Take(employeeEndIndex);
            employeeDataDto.SendCycle = DecimalExtension.CalAvg(employeeSendData.Sum(e => e.Value), employeeSendData.Count());
            #endregion
            #region 上门转化周期
            var employeeEndIndex2 = DecimalExtension.CalTakeCount(dataList2.Where(e => e.Key == query.BaseLiveAnchorId).Count(), 0.6m);
            var employeeToHospitalData = dataList2.Where(e => e.Key == query.BaseLiveAnchorId).OrderBy(e => e.Value).ToList().Skip(0).Take(employeeEndIndex2);
            employeeDataDto.HospitalCycle = DecimalExtension.CalAvg(employeeToHospitalData.Sum(e => e.Value), employeeToHospitalData.Count());
            #endregion
            return filterData;
        }

        /// <summary>
        /// 直播中转化周期数据
        /// </summary>
        /// <returns></returns>
        public async Task<LivingCycleDataDto> GetLivingCycleDataAsync(QueryLivingDataDto query)
        {
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetAllLiveAnchorAsync(true);
            var baseLiveanchorIdList = baseLiveanchorList.Select(e => e.Id).ToList();
            LivingCycleDataDto data = new LivingCycleDataDto();
            var seqDate = DateTimeExtension.GetStartDateEndDate(query.StartDate,query.EndDate);
            var cartInfoList = _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false && e.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.RecordDate >= seqDate.StartDate && e.RecordDate < seqDate.EndDate)
                .Where(e => baseLiveanchorIdList.Contains(e.BaseLiveAnchorId))
                .Select(e => new
                {
                    BaseLiveAnchorId = e.BaseLiveAnchorId,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate
                }).ToList();
            #region 分诊派单

            var sendInfoList = await _dalContentPlatformOrderSend.GetAll().Where(e => e.IsMainHospital == true && e.SendDate >= seqDate.StartDate && e.SendDate < seqDate.EndDate)
                .Where(e => e.ContentPlatformOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => baseLiveanchorIdList.Contains(e.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new { Phone = e.ContentPlatformOrder.Phone, LiveAnchorBaseId = e.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId, SendDate = e.SendDate }).ToListAsync();
            var sendPhoneList = sendInfoList.Distinct().Select(e => e.Phone).ToList();

            List<KeyValuePair<string, int>> dataList = new List<KeyValuePair<string, int>>();
            if (query.IsCurrent)
            {
                dataList = (from send in sendInfoList
                            join cart in cartInfoList
                            on send.Phone equals cart.Phone
                            select new KeyValuePair<string, int>
                            (
                               cart.BaseLiveAnchorId,
                               (send.SendDate - cart.RecordDate).Days
                            )).ToList();
            }
            else
            {
                var historySendPhone = sendInfoList.Where(e => !cartInfoList.Select(e => e.Phone).Contains(e.Phone)).Select(e => e.Phone);
                var historyCardList = _dalShoppingCartRegistration.GetAll().Where(e => baseLiveanchorIdList.Contains(e.BaseLiveAnchorId) && historySendPhone.Contains(e.Phone) && e.BelongChannel == (int)BelongChannel.Living)
                    .Select(e => new { e.Phone, e.BaseLiveAnchorId, e.RecordDate });

                dataList = (from send in sendInfoList
                            join cart in historyCardList
                            on send.Phone equals cart.Phone
                            select new KeyValuePair<string, int>
                            (
                                cart.BaseLiveAnchorId,
                                (send.SendDate - cart.RecordDate).Days
                            )).ToList();
            }

            dataList.RemoveAll(e => e.Value < 0);
            //转化周期数据
            var res1 = dataList.GroupBy(e => e.Key).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count(), 0.8m);
                var resData = e.OrderBy(e => e.Value).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                baseLiveanchorList.Where(a => a.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其它",
                DecimalExtension.CalAvg(resData.Sum(e => e.Value), resData.Count())
             );
            }).OrderBy(e => e.Value).ToList();
            res1.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            data.SendCycleData = res1;
            #endregion

            #region 分诊上门
            var dealInfoList = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate && e.IsOldCustomer == false && e.IsToHospital == true && e.ToHospitalDate.HasValue)
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => baseLiveanchorIdList.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                    .Select(e => new
                    {
                        LiveAnchorBaseId = e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                        Phone = e.ContentPlatFormOrder.Phone,
                        ToHospitalDate = e.CreateDate
                    }).ToListAsync();
            var dealPhoneList = dealInfoList.Select(e => e.Phone).ToList();
            List<KeyValuePair<string, int>> dataList2 = new List<KeyValuePair<string, int>>();
            if (query.IsCurrent)
            {
                dataList2 = (from deal in dealInfoList
                             join cart in cartInfoList
                             on deal.Phone equals cart.Phone
                             select new KeyValuePair<string, int>
                             (
                                 cart.BaseLiveAnchorId,
                                 (deal.ToHospitalDate - cart.RecordDate).Days
                             )).ToList();
            }
            else
            {
                var historyDealPhone = dealInfoList.Where(e => !cartInfoList.Select(e => e.Phone).Contains(e.Phone)).Select(e => e.Phone);
                var historyCardList = _dalShoppingCartRegistration.GetAll().Where(e => baseLiveanchorIdList.Contains(e.BaseLiveAnchorId) && historyDealPhone.Contains(e.Phone) && e.BelongChannel == (int)BelongChannel.Living);
                    dataList2 = (from deal in dealInfoList
                                 join cart in historyCardList
                                 on deal.Phone equals cart.Phone
                                 select new KeyValuePair<string, int>
                                 (
                                     cart.BaseLiveAnchorId,
                                     (deal.ToHospitalDate- cart.RecordDate).Days
                                 )).ToList();
            }

            dataList2.RemoveAll(e => e.Value < 0);
            //转化周期数据
            var res2 = dataList2.GroupBy(e => e.Key).Select(e =>
            {
                var endIndex = DecimalExtension.CalTakeCount(e.Count(), 0.6m);
                var resData = e.OrderBy(e => e.Value).Skip(0).Take(endIndex);
                return new KeyValuePair<string, int>(
                baseLiveanchorList.Where(a => a.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其它",
                DecimalExtension.CalAvg(resData.Sum(e => e.Value), resData.Count()));
            }).OrderBy(e => e.Value).ToList();
            res2.RemoveAll(e => e.Key == "其它" || e.Value == 0);
            data.ToHospitalCycleData = res2;
            #endregion
            return data;
        }

        /// <summary>
        /// 直播中线索目标完成率
        /// </summary>
        /// <returns></returns>
        public async Task<LivingClueTargetDataDto> GetLivingClueTargetDataAsync(QueryLivingDataDto query)
        {
            LivingClueTargetDataDto data = new LivingClueTargetDataDto();
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetAllLiveAnchorAsync(true);
            var baseLiveanchorIdList = baseLiveanchorList.Select(e => e.Id).ToList();
            var seqDate = DateTimeExtension.GetStartDateEndDate(query.StartDate,query.EndDate);
            var target =await dalLiveAnchorMonthlyTargetLiving.GetAll()
                .Where(e => baseLiveanchorIdList.Contains(e.LiveAnchor.LiveAnchorBaseId) && e.Month == query.EndDate.Month && e.Year == query.EndDate.Year)
                .Where(e=>e.ConsultationTarget>1)
                .Select(e => new { e.LiveAnchor.LiveAnchorBaseId, e.ConsultationTarget })
                .GroupBy(e => e.LiveAnchorBaseId)
                .Select(e=>new { 
                    BaseLiveanchorId=e.Key,
                    Target=e.Sum(e=>e.ConsultationTarget)
                }).ToListAsync();
            var clueData= _dalShoppingCartRegistration.GetAll()
                .Where(e => e.IsReturnBackPrice == false && e.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.RecordDate >= seqDate.StartDate && e.RecordDate < seqDate.EndDate)
                .Where(e => baseLiveanchorIdList.Contains(e.BaseLiveAnchorId))
                .Select(e => new
                {
                    BaseLiveAnchorId = e.BaseLiveAnchorId,
                }).ToList();
            var targetData= clueData.GroupBy(e => e.BaseLiveAnchorId).Select(e => { 
                var name=baseLiveanchorList.Where(x=>x.Id==e.Key).FirstOrDefault()?.LiveAnchorName??"其他";
                var t = target.Where(x => x.BaseLiveanchorId == e.Key).FirstOrDefault()?.Target??0;
                var targetComplete = DecimalExtension.CalculateTargetComplete(e.Count(),t).Value;
                return new KeyValuePair<string, decimal>(name, targetComplete);
            }).ToList();
            data.ClueTargetComplete = targetData;
            return data;
        }
        /// <summary>
        /// 直播中业绩贡献占比
        /// </summary>
        /// <returns></returns>
        public async Task<LivingPerformanceRateDto> GetLivingPerformanceRateAsync(QueryLivingDataDto query)
        {
            LivingPerformanceRateDto data = new LivingPerformanceRateDto();
            var baseLiveanchorList = await liveAnchorBaseInfoService.GetAllLiveAnchorAsync(true);
            var baseLiveanchorIdList = baseLiveanchorList.Select(e => e.Id).ToList();
            var seqDate = DateTimeExtension.GetStartDateEndDate(query.StartDate,query.EndDate);
            
            var performanceData = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.IsDeal == true && e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.CreateDate >= seqDate.StartDate && e.CreateDate < seqDate.EndDate)
                .Where(e => baseLiveanchorIdList.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new
                {
                    BaseLiveAnchorId = e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                    Price=e.Price
                }).ToList();
            var totalPerformance = performanceData.Sum(e => e.Price);
            var performanceRateData = performanceData.GroupBy(e => e.BaseLiveAnchorId).Select(e => {
                var name = baseLiveanchorList.Where(x => x.Id == e.Key).FirstOrDefault()?.LiveAnchorName ?? "其他";
                var perfromance = e.Sum(e => e.Price);
                var performanceRate = DecimalExtension.CalculateTargetComplete(perfromance, totalPerformance).Value;
                return new KeyValuePair<string, decimal>(name, performanceRate);
            }).ToList();
            data.PerformanceRate = performanceRateData;
            return data;
        }
        /// <summary>
        /// 直播中平台和账号获客占比
        /// </summary>
        /// <returns></returns>
        public async Task<LivingContentplatformClueDataDto> GetLivingContentplatformClueDataAsync(QueryLivingDataDto query)
        {
            LivingContentplatformClueDataDto livingData = new LivingContentplatformClueDataDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var baseData = await _dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= selectDate.StartDate && e.RecordDate < selectDate.EndDate && e.BaseLiveAnchorId == query.BaseLiveAnchorId && e.BelongChannel == (int)BelongChannel.Living && e.IsReturnBackPrice == false)
                .Select(e => new
                {
                    ContentPlatformName = e.Contentplatform.ContentPlatformName,
                    ContentPlatformId = e.ContentPlatFormId,
                    LiveAnchorName = e.LiveAnchor.Name,
                    IsRiBuLuoLiving = e.IsRiBuLuoLiving
                }).ToListAsync();
            var totalCount = baseData.Count;
            livingData.ContentPlatformTotalClue = totalCount;
            livingData.ContentPlatformClueRate = baseData.GroupBy(e => e.ContentPlatformName).Select(e => new LivingContentplatformClueDataItemDto
            {
                Name = e.Key,
                Value = DecimalExtension.CalculateTargetComplete(e.Count(), totalCount).Value,
                Performance = e.Count()
            }).ToList();
            livingData.TikTokTotalClue = baseData.Where(e => e.ContentPlatformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Count();
            livingData.TikTokClueRate= baseData.Where(e=>e.ContentPlatformId== "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").GroupBy(e => e.LiveAnchorName).Select(e => new LivingContentplatformClueDataItemDto
            {
                Name = e.Key,
                Value = DecimalExtension.CalculateTargetComplete(e.Count(), livingData.TikTokTotalClue).Value,
                Performance = e.Count()
            }).ToList();
            livingData.WechatVideoTotalClue = baseData.Where(e => e.ContentPlatformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Count();
            livingData.WechatVideoClueRate = baseData.Where(e => e.ContentPlatformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").GroupBy(e => e.LiveAnchorName).Select(e => new LivingContentplatformClueDataItemDto
            {
                Name = e.Key,
                Value = DecimalExtension.CalculateTargetComplete(e.Count(), livingData.WechatVideoTotalClue).Value,
                Performance = e.Count()
            }).ToList();
            return livingData;
        }
        /// <summary>
        /// 直播中平台和账号业绩占比
        /// </summary>
        /// <returns></returns>
        public async Task<LivingContentplatformPerformanceDataDto> GetLivingContentplatformPerformanceDataAsync(QueryLivingDataDto query)
        {
            LivingContentplatformPerformanceDataDto livingData = new LivingContentplatformPerformanceDataDto();
            var selectDate = DateTimeExtension.GetStartDateEndDate(query.StartDate, query.EndDate);
            var performanceList = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => e.ContentPlatFormOrder.BelongChannel == (int)BelongChannel.Living)
                .Where(e => e.IsDeal == true&& e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate)
                .Where(e => e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId==query.BaseLiveAnchorId)
                   .Select(e => new
                   {
                       LiveAnchorName = e.ContentPlatFormOrder.LiveAnchor.Name,
                       ContentPlateformId = e.ContentPlatFormOrder.ContentPlateformId,
                       ContentPlatformName = e.ContentPlatFormOrder.Contentplatform.ContentPlatformName,
                       Price = e.Price
                   }).ToList();
            var totalPerformance = performanceList.Sum(e=>e.Price);
            livingData.ContentPlatformTotalPerformance = ChangePriceToTenThousand(totalPerformance);
            livingData.ContentPlatformPerformanceRate = performanceList.GroupBy(e => e.ContentPlatformName)
                .Select(e => new LivingContentplatformPerformanceDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), totalPerformance).Value,
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
                }).ToList();
            livingData.TikTokAccountTotalPerformance = ChangePriceToTenThousand(performanceList.Where(e => e.ContentPlateformId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").Sum(e => e.Price));
            livingData.TikTokAccountPerformanceRate = performanceList.Where(e=>e.ContentPlateformId== "4e4e9564-f6c3-47b6-a7da-e4518bab66a1").GroupBy(e => e.LiveAnchorName)
                .Select(e => new LivingContentplatformPerformanceDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), totalPerformance).Value,
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
                }).ToList();
            livingData.WechatVideoAccountTotalPerformance = ChangePriceToTenThousand(performanceList.Where(e => e.ContentPlateformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").Sum(e=>e.Price));
            livingData.WechatVideoAccountPerformanceRate = performanceList.Where(e => e.ContentPlateformId == "9196b247-1ab9-4d0c-a11e-a1ef09019878").GroupBy(e => e.LiveAnchorName)
                .Select(e => new LivingContentplatformPerformanceDataItemDto
                {
                    Name = e.Key,
                    Value = DecimalExtension.CalculateTargetComplete(e.Sum(e => e.Price), totalPerformance).Value,
                    Performance = ChangePriceToTenThousand(e.Sum(e => e.Price))
                }).ToList();

            return livingData;
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
