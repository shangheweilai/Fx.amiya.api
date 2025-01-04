using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Input;
using Fx.Amiya.Dto.AssistantHomePage.Input;
using Fx.Amiya.Dto.AssistantHomePage.Result;
using Fx.Amiya.Dto.ContentPlateFormOrder;
using Fx.Amiya.Dto.ContentPlatFormOrderDealDetails.Input;
using Fx.Amiya.Dto.ContentPlatFormOrderSend;
using Fx.Amiya.Dto.FinancialBoard;
using Fx.Amiya.Dto.Performance;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.Dto.TmallOrder;
using Fx.Amiya.Dto.WxAppConfig;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Fx.Infrastructure.DataAccess;
using jos_sdk_net.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class ContentPlatFormOrderDealInfoService : IContentPlatFormOrderDealInfoService
    {
        private IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo;
        private IContentPlatFormCustomerPictureService _contentPlatFormCustomerPictureService;
        private IHospitalInfoService _hospitalInfoService;
        private ICompanyBaseInfoService companyBaseInfoService;
        private ICustomerBaseInfoService customerBaseInfoService;
        private IDalBindCustomerService _dalBindCustomerService;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private IWxAppConfigService wxAppConfigService;
        private IDalAmiyaEmployee _dalAmiyaEmployee;
        private IDalHospitalInfo _dalHospitalInfo;
        private IContentPlatFormOrderDealDetailsService contentPlatFormOrderDealDetailsService;
        private IDalRecommandDocumentSettle dalRecommandDocumentSettle;
        private IDalCompanyBaseInfo dalCompanyBaseInfo;
        private IDalLiveAnchor dalLiveAnchor;
        private ILiveAnchorService liveAnchorService;
        private IDalConfig dalConfig;
        public ContentPlatFormOrderDealInfoService(IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo,
            IAmiyaEmployeeService amiyaEmployeeService,
            ICompanyBaseInfoService companyBaseInfoService,
            ICustomerBaseInfoService customerBaseInfoService,
            IWxAppConfigService wxAppConfigService,
            IContentPlatFormCustomerPictureService contentPlatFormCustomerPictureService,
            IDalBindCustomerService dalBindCustomerService,
            IDalAmiyaEmployee dalAmiyaEmployee,
            IContentPlatFormOrderDealDetailsService contentPlatFormOrderDealDetailsService,
            IHospitalInfoService hospitalInfoService, IDalHospitalInfo dalHospitalInfo, IDalRecommandDocumentSettle dalRecommandDocumentSettle, IDalCompanyBaseInfo dalCompanyBaseInfo, IDalLiveAnchor dalLiveAnchor, ILiveAnchorService liveAnchorService, IDalConfig dalConfig)
        {
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            _hospitalInfoService = hospitalInfoService;
            this.contentPlatFormOrderDealDetailsService = contentPlatFormOrderDealDetailsService;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.wxAppConfigService = wxAppConfigService;
            _contentPlatFormCustomerPictureService = contentPlatFormCustomerPictureService;
            _dalBindCustomerService = dalBindCustomerService;
            _dalAmiyaEmployee = dalAmiyaEmployee;
            this.companyBaseInfoService = companyBaseInfoService;
            this.customerBaseInfoService = customerBaseInfoService;
            _dalHospitalInfo = dalHospitalInfo;
            this.dalRecommandDocumentSettle = dalRecommandDocumentSettle;
            this.dalCompanyBaseInfo = dalCompanyBaseInfo;
            this.dalLiveAnchor = dalLiveAnchor;
            this.liveAnchorService = liveAnchorService;
            this.dalConfig = dalConfig;
        }

        /// <summary>
        /// 获取简易的到院订单
        /// </summary>
        /// <param name="startDate">登记开始日期</param>
        /// <param name="endDate">登记结束日期</param>
        /// <param name="isDeal">是否成交（空查询所有）</param>
        ///// <param name="dealStartDate">成交开始时间</param>
        ///// <param name="dealEndDate">成交结束时间</param>
        /// <param name="lastDealHospitalId">最终成交医院id（空查询所有）</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetSimpleOrderListWithPageAsync(DateTime? startDate, DateTime? endDate, bool? isDeal, int? lastDealHospitalId, string keyWord, int employeeId, int pageNum, int pageSize)
        {
            var config = await wxAppConfigService.GetCallCenterConfig();
            try
            {
                var dealInfo = from d in dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(z => z.ContentPlatformOrderSendList) select d;

                var employee = await _dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == employeeId);
                if (employee.IsCustomerService && !employee.AmiyaPositionInfo.IsDirector)
                {
                    dealInfo = from d in dealInfo
                               where _dalBindCustomerService.GetAll().Count(e => e.CustomerServiceId == employeeId && e.BuyerPhone == d.ContentPlatFormOrder.Phone) > 0 || d.ContentPlatFormOrder.SupportEmpId == employeeId || d.ContentPlatFormOrder.BelongEmpId == employeeId
                               where (d.ContentPlatFormOrder.IsSupportOrder == false || d.ContentPlatFormOrder.SupportEmpId == employeeId)
                               select d;
                }
                ////财务录入数据只有管理员研发财务和CMO能看到
                //if (employee.AmiyaPositionInfo.Id != 1 && employee.AmiyaPositionInfo.Id != 13 && employee.AmiyaPositionInfo.Id != 16 && employee.AmiyaPositionInfo.Id != 29)
                //{

                //    dealInfo = from d in dealInfo
                //               where d.CreateBy != 61 && d.CreateBy != 80
                //               select d;
                //}
                if (startDate != null)
                {
                    DateTime startrq = ((DateTime)startDate).Date;
                    dealInfo = from d in dealInfo
                               where (d.CreateDate >= startrq)
                               select d;
                }
                if (endDate != null)
                {
                    DateTime enddate = ((DateTime)endDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.CreateDate < enddate)
                               select d;
                }


                //if (isDeal == true)
                //{
                //    //if (!dealStartDate.HasValue || !dealEndDate.HasValue)
                //    //{
                //    //    throw new Exception("成交时间为必填项，请完整填写成交的开始时间与结束时间！");
                //    //}
                //    DateTime startrq = ((DateTime)dealStartDate).Date;
                //    DateTime endrq = ((DateTime)dealEndDate).Date.AddDays(1);
                //    dealInfo = from d in dealInfo
                //               where (d.DealDate.HasValue)
                //               && (d.DealDate.Value >= startrq && d.DealDate.Value < endrq)
                //               && (d.IsDeal == true)
                //               select d;
                //}
                var ContentPlatFOrmOrderDealInfo = from d in dealInfo
                                                   where (string.IsNullOrEmpty(keyWord) || d.ContentPlatFormOrderId.Contains(keyWord) || d.ContentPlatFormOrder.Phone.Contains(keyWord) || d.Id.Contains(keyWord))
                                                   && (!isDeal.HasValue || d.IsDeal == isDeal.Value)
                                                   && (d.Valid == true)
                                                   && (!lastDealHospitalId.HasValue || d.LastDealHospitalId.Value == lastDealHospitalId.Value)
                                                   && (d.IsToHospital)
                                                   select new ContentPlatFormOrderDealInfoDto
                                                   {
                                                       Id = d.Id,
                                                       ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                                                       CreateDate = d.CreateDate,
                                                       IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                                                       Phone = ServiceClass.GetIncompletePhone(d.ContentPlatFormOrder.Phone),
                                                       IsDeal = d.IsDeal,
                                                       Price = d.Price,
                                                       IsOldCustomer = d.IsOldCustomer,
                                                       ConsultationTypeText = ServiceClass.GetContentPlateFormOrderConsultationTypeText(d.ContentPlatFormOrder.ConsulationType),
                                                       ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                                                       ToHospitalDate = d.ToHospitalDate,
                                                       DealDate = d.DealDate,
                                                   };

                FxPageInfo<ContentPlatFormOrderDealInfoDto> ContentPlatFOrmOrderDealInfoPageInfo = new FxPageInfo<ContentPlatFormOrderDealInfoDto>();
                ContentPlatFOrmOrderDealInfoPageInfo.TotalCount = await ContentPlatFOrmOrderDealInfo.CountAsync();
                ContentPlatFOrmOrderDealInfoPageInfo.List = await ContentPlatFOrmOrderDealInfo.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

                return ContentPlatFOrmOrderDealInfoPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 获取成交情况列表
        /// </summary>
        /// <param name="startDate">登记开始日期</param>
        /// <param name="endDate">登记结束日期</param>
        /// <param name="sendStartDate">派单开始日期</param>
        /// <param name="sendEndDate">派单结束日期</param>
        /// <param name="consultationType">面诊状态（空查询所有）</param>
        /// <param name="isToHospital">是否到院（空查询所有）</param>
        /// <param name="tohospitalStartDate">到院开始时间</param>
        /// <param name="toHospitalEndDate">到院结束时间</param>
        /// <param name="toHospitalType">到院类型（空查询所有）</param>
        /// <param name="isDeal">是否成交（空查询所有）</param>
        /// <param name="dealStartDate">成交开始时间（是否成交为“是”时必填，其他情况可空）</param>
        /// <param name="dealEndDate">成交结束时间(是否成交为“是”时必填，其他情况可空)</param>
        /// <param name="lastDealHospitalId">最终成交医院id（空查询所有）</param>
        /// <param name="isAccompanying">是否陪诊（空查询所有）</param>
        /// <param name="minAddOrderPrice">最小下单金额（空查询所有）</param>
        /// <param name="maxAddOrderPrice">最大下单金额（空查询所有）</param>
        /// <param name="isOldCustomer">新老客业绩（空查询所有）</param>
        /// <param name="CheckState">审核状态（空查询所有）</param>
        /// <param name="isReturnBakcPrice">是否回款（空查询所有）</param>
        /// <param name="returnBackPriceStartDate">回款开始时间</param>
        /// <param name="returnBackPriceEndDate">回款结束时间</param>
        /// <param name="customerServiceId">跟进人员（空查询所有）</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="employeeId"></param>
        /// <param name="createBillCompanyId">开票公司id</param>
        /// <param name="dataFrom">数据获取方：true：财务；false：其他</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetOrderListWithPageAsync(DateTime? startDate, DateTime? endDate, DateTime? sendStartDate, DateTime? sendEndDate, int? consultationType, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, bool? isToHospital, DateTime? tohospitalStartDate, DateTime? toHospitalEndDate, DateTime? dealStartDate, DateTime? dealEndDate, int? toHospitalType, bool? isDeal, int? lastDealHospitalId, bool? isAccompanying, bool? isOldCustomer, int? CheckState, bool? isReturnBakcPrice, DateTime? returnBackPriceStartDate, DateTime? returnBackPriceEndDate, int? customerServiceId, string keyWord, int employeeId, string createBillCompanyId, bool? isCreateBill, int pageNum, int pageSize, bool? dataFrom, int? consumptionType)
        {
            var config = await wxAppConfigService.GetCallCenterConfig();
            try
            {
                var dealInfo = from d in dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(z => z.ContentPlatformOrderSendList) select d;
                var customerBaseInfo = await customerBaseInfoService.GetByPhoneAsync(keyWord);
                var basePhone = customerBaseInfo.Phone;
                var employee = await _dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == employeeId);
                //普通客服角色过滤其他订单信息只展示自己录单信息
                if (employee.IsCustomerService && !employee.AmiyaPositionInfo.IsDirector)
                {
                    dealInfo = from d in dealInfo
                               where _dalBindCustomerService.GetAll().Count(e => e.CustomerServiceId == employeeId && e.BuyerPhone == d.ContentPlatFormOrder.Phone) > 0 || d.ContentPlatFormOrder.SupportEmpId == employeeId || d.ContentPlatFormOrder.BelongEmpId == employeeId
                               where (d.ContentPlatFormOrder.IsSupportOrder == false || d.ContentPlatFormOrder.SupportEmpId == employeeId)
                               select d;
                }
                ////财务录入数据只有管理员研发财务和CMO能看到
                //if (employee.AmiyaPositionInfo.Id != 1 && employee.AmiyaPositionInfo.Id != 13 && employee.AmiyaPositionInfo.Id != 16 && employee.AmiyaPositionInfo.Id != 29)
                //{

                //    dealInfo = from d in dealInfo
                //               where d.CreateBy != 61 && d.CreateBy != 80
                //               select d;
                //}
                if (startDate != null)
                {
                    DateTime startrq = ((DateTime)startDate).Date;
                    dealInfo = from d in dealInfo
                               where (d.CreateDate >= startrq)
                               select d;
                }
                if (consumptionType != null)
                {
                    dealInfo = from d in dealInfo
                               where (d.ConsumptionType == consumptionType)
                               select d;
                }
                if (endDate != null)
                {
                    DateTime enddate = ((DateTime)endDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.CreateDate < enddate)
                               select d;
                }

                if (sendStartDate != null)
                {
                    DateTime startrq = ((DateTime)sendStartDate).Date;
                    dealInfo = from d in dealInfo
                               where (d.ContentPlatFormOrder.SendDate >= startrq)
                               select d;
                }
                if (sendEndDate != null)
                {
                    DateTime enddate = ((DateTime)sendEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ContentPlatFormOrder.SendDate < enddate)
                               select d;
                }
                if (!string.IsNullOrEmpty(createBillCompanyId))
                {
                    dealInfo = from d in dealInfo
                               where d.BelongCompany == createBillCompanyId
                               select d;
                }
                if (isCreateBill != null)
                {
                    dealInfo = from d in dealInfo
                               where d.IsCreateBill == isCreateBill
                               select d;
                }
                if (isToHospital == true)
                {
                    if (!tohospitalStartDate.HasValue || !toHospitalEndDate.HasValue)
                    {
                        throw new Exception("到院时间为必填项，请完整填写到院的开始时间与结束时间！");
                    }
                    DateTime startrq = ((DateTime)tohospitalStartDate).Date;
                    DateTime endrq = ((DateTime)toHospitalEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ToHospitalDate.HasValue)
                               && (d.ToHospitalDate.Value >= startrq && d.ToHospitalDate.Value < endrq)
                               && (d.IsToHospital == true)
                               select d;
                }

                if (isDeal == true)
                {
                    if (!dealStartDate.HasValue || !dealEndDate.HasValue)
                    {
                        throw new Exception("成交时间为必填项，请完整填写成交的开始时间与结束时间！");
                    }
                    DateTime startrq = ((DateTime)dealStartDate).Date;
                    DateTime endrq = ((DateTime)dealEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.DealDate.HasValue)
                               && (d.DealDate.Value >= startrq && d.DealDate.Value < endrq)
                               && (d.IsDeal == true)
                               select d;
                }
                if (isReturnBakcPrice == true)
                {
                    if (!returnBackPriceStartDate.HasValue || !returnBackPriceEndDate.HasValue)
                    {
                        throw new Exception("回款时间为必填项，请完整填写回款的开始时间与结束时间！");
                    }
                    DateTime startrq = ((DateTime)returnBackPriceStartDate).Date;
                    DateTime endrq = ((DateTime)returnBackPriceEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ReturnBackDate.HasValue)
                               && (d.ReturnBackDate.Value >= startrq && d.ReturnBackDate.Value < endrq)
                               && (d.IsToHospital == true)
                               select d;
                }
                if (dataFrom.HasValue && dataFrom.Value == true)
                {
                    config.HidePhoneNumber = false;
                }
                var ContentPlatFOrmOrderDealInfo = from d in dealInfo
                                                   where (string.IsNullOrEmpty(keyWord) || d.ContentPlatFormOrderId.Contains(keyWord) || d.ContentPlatFormOrder.Phone.Contains(keyWord) || d.Id.Contains(keyWord) || d.ContentPlatFormOrder.Phone.Contains(basePhone))
                                                   && (!isToHospital.HasValue || d.IsToHospital == isToHospital.Value)
                                                   && (!toHospitalType.HasValue || d.ToHospitalType == toHospitalType.Value)
                                                   && (!isDeal.HasValue || d.IsDeal == isDeal.Value)
                                                   && (d.Valid == true)
                                                   && (!lastDealHospitalId.HasValue || d.LastDealHospitalId.Value == lastDealHospitalId.Value)
                                                   && (!isOldCustomer.HasValue || d.IsOldCustomer == isOldCustomer.Value)
                                                   && (!isAccompanying.HasValue || d.IsAcompanying == isAccompanying.Value)
                                                   && (!CheckState.HasValue || d.CheckState == CheckState.Value)
                                                   && (!isReturnBakcPrice.HasValue || d.IsReturnBackPrice == isReturnBakcPrice.Value)
                                                   //&& (!customerServiceId.HasValue || d.CreateBy == customerServiceId) 
                                                   && (!customerServiceId.HasValue || d.ContentPlatFormOrder.BelongEmpId == customerServiceId)
                                                   && (!consultationType.HasValue || d.ContentPlatFormOrder.ConsulationType == consultationType)
                                                   && (!minAddOrderPrice.HasValue || d.ContentPlatFormOrder.AddOrderPrice >= minAddOrderPrice)
                                                   && (!maxAddOrderPrice.HasValue || d.ContentPlatFormOrder.AddOrderPrice <= maxAddOrderPrice)
                                                   select new ContentPlatFormOrderDealInfoDto
                                                   {
                                                       Id = d.Id,
                                                       ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                                                       CreateDate = d.CreateDate,
                                                       Phone = config.HidePhoneNumber == true ? ServiceClass.GetIncompletePhone(d.ContentPlatFormOrder.Phone) : d.ContentPlatFormOrder.Phone,
                                                       EncryptPhone = ServiceClass.Encrypt(d.ContentPlatFormOrder.Phone, config.PhoneEncryptKey),
                                                       IsDeal = d.IsDeal,
                                                       IsOldCustomer = d.IsOldCustomer,
                                                       IsAcompanying = d.IsAcompanying,
                                                       ConsultationTypeText = ServiceClass.GetContentPlateFormOrderConsultationTypeText(d.ContentPlatFormOrder.ConsulationType),
                                                       CommissionRatio = d.CommissionRatio,
                                                       IsToHospital = d.IsToHospital,
                                                       AddOrderPrice = d.ContentPlatFormOrder.AddOrderPrice,
                                                       ToHospitalType = d.ToHospitalType,
                                                       ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                                                       ToHospitalDate = d.ToHospitalDate,
                                                       DealPerformanceType = d.DealPerformanceType,
                                                       DealPerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(d.DealPerformanceType),
                                                       LastDealHospitalId = d.LastDealHospitalId,
                                                       SendDate = d.ContentPlatFormOrder.SendDate,
                                                       DealPicture = d.DealPicture,
                                                       Remark = d.Remark,
                                                       Price = d.Price,
                                                       DealDate = d.DealDate,
                                                       OtherAppOrderId = d.OtherAppOrderId,
                                                       CheckState = d.CheckState,
                                                       CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState.Value),
                                                       CheckPrice = d.CheckPrice,
                                                       CheckDate = d.CheckDate,
                                                       CheckBy = d.CheckBy,
                                                       InformationPrice = d.InformationPrice,
                                                       SystemUpdatePrice = d.SystemUpdatePrice,
                                                       SettlePrice = d.SettlePrice,
                                                       CheckRemark = d.CheckRemark,
                                                       IsReturnBackPrice = d.IsReturnBackPrice,
                                                       ReturnBackDate = d.ReturnBackDate,
                                                       ReturnBackPrice = d.ReturnBackPrice,
                                                       CreateBy = d.CreateBy,
                                                       //CreateBy = d.ContentPlatFormOrder.BelongEmpId.HasValue ? d.ContentPlatFormOrder.BelongEmpId.Value : -1,
                                                       BelongLiveAnchor = d.ContentPlatFormOrder.LiveAnchor.Name,
                                                       ReconciliationDocumentsId = d.ReconciliationDocumentsId,
                                                       IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                                                       IsCreateBill = d.IsCreateBill,
                                                       BelongCompany = d.BelongCompany,
                                                       ConsumptionType = d.ConsumptionType,
                                                       ConsumptionTypeText = ServiceClass.GetConsumptionTypeText(d.ConsumptionType),
                                                       CustomerServiceSettlePrice = d.CustomerServiceSettlePrice ?? 0m
                                                   };

                FxPageInfo<ContentPlatFormOrderDealInfoDto> ContentPlatFOrmOrderDealInfoPageInfo = new FxPageInfo<ContentPlatFormOrderDealInfoDto>();
                ContentPlatFOrmOrderDealInfoPageInfo.TotalCount = await ContentPlatFOrmOrderDealInfo.CountAsync();
                ContentPlatFOrmOrderDealInfoPageInfo.List = await ContentPlatFOrmOrderDealInfo.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                foreach (var z in ContentPlatFOrmOrderDealInfoPageInfo.List)
                {
                    if (string.IsNullOrEmpty(z.BelongCompany))
                    {
                        z.BelongCompany = "";
                    }
                    else
                    {
                        z.BelongCompany = dalCompanyBaseInfo.GetAll().Where(e => e.Id == z.BelongCompany).SingleOrDefault()?.Name;
                    }
                    if (z.LastDealHospitalId.HasValue && z.LastDealHospitalId.Value != 0)
                    {
                        var dealHospital = await _hospitalInfoService.GetBaseByIdAsync(z.LastDealHospitalId.Value);
                        z.LastDealHospital = dealHospital.Name;
                    }
                    if (z.CheckBy.HasValue)
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CheckBy.Value);
                        z.CheckByEmpName = empInfo.Name;
                    }
                    if (z.CreateBy == 0)
                    {
                        z.CreateByEmpName = "医院创建";
                    }
                    else
                    {
                        if (z.CreateBy != -1)
                        {
                            var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CreateBy);
                            z.CreateByEmpName = empInfo.Name;
                        }
                        else
                        {
                            z.CreateByEmpName = "未知";
                        }
                    }
                }
                return ContentPlatFOrmOrderDealInfoPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetContentPlatFormOrderDealInfoByReconciliationDocumentsIdAsync(string reconciliationDocumentsId, int pageNum, int pageSize)
        {
            try
            {
                var dealInfoList = dalRecommandDocumentSettle.GetAll().Where(e => e.RecommandDocumentId == reconciliationDocumentsId && e.OrderFrom == (int)OrderFrom.ContentPlatFormOrder).Select(e => e.DealInfoId).ToList();
                var dealInfo = from d in dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).OrderByDescending(x => x.CreateDate) select d;

                var ContentPlatFOrmOrderDealInfo = from d in dealInfo
                                                   where (string.IsNullOrEmpty(reconciliationDocumentsId) || dealInfoList.Contains(d.Id))
                                                   where d.Valid == true
                                                   select new ContentPlatFormOrderDealInfoDto
                                                   {
                                                       Id = d.Id,
                                                       ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                                                       Phone = ServiceClass.GetIncompletePhone(d.ContentPlatFormOrder.Phone),
                                                       IsDeal = d.IsDeal,
                                                       ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                                                       Price = d.Price,
                                                       DealDate = d.DealDate,
                                                       CreateDate = d.CreateDate,
                                                       CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState.Value),
                                                       CheckPrice = d.CheckPrice,
                                                       CheckDate = d.CheckDate,
                                                       CheckBy = d.CheckBy,
                                                       DealPerformanceType = d.DealPerformanceType,
                                                       DealPerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(d.DealPerformanceType),
                                                       InformationPrice = d.InformationPrice,
                                                       SystemUpdatePrice = d.SystemUpdatePrice,
                                                       SettlePrice = d.SettlePrice,
                                                       CheckRemark = d.CheckRemark,
                                                       IsReturnBackPrice = d.IsReturnBackPrice,
                                                       ReturnBackDate = d.ReturnBackDate,
                                                       CreateBy = d.CreateBy,
                                                       ReturnBackPrice = d.ReturnBackPrice,
                                                       LiveAnchorName = d.ContentPlatFormOrder.LiveAnchor.Name,
                                                       LastDealHospitalId = d.LastDealHospitalId,
                                                       IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                                                       ConsumptionTypeText = ServiceClass.GetConsumptionTypeText(d.ConsumptionType)
                                                   };

                FxPageInfo<ContentPlatFormOrderDealInfoDto> ContentPlatFOrmOrderDealInfoPageInfo = new FxPageInfo<ContentPlatFormOrderDealInfoDto>();
                ContentPlatFOrmOrderDealInfoPageInfo.TotalCount = await ContentPlatFOrmOrderDealInfo.CountAsync();
                ContentPlatFOrmOrderDealInfoPageInfo.List = await ContentPlatFOrmOrderDealInfo.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                foreach (var z in ContentPlatFOrmOrderDealInfoPageInfo.List)
                {
                    if (z.LastDealHospitalId.HasValue)
                    {
                        var dealHospital = await _hospitalInfoService.GetBaseByIdAsync(z.LastDealHospitalId.Value);
                        z.LastDealHospital = dealHospital.Name;
                    }
                    if (z.CheckBy.HasValue)
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CheckBy.Value);
                        z.CheckByEmpName = empInfo.Name;
                    }
                    if (z.CreateBy == 0)
                    {
                        z.CreateByEmpName = "医院创建";
                    }
                    else
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CreateBy);
                        z.CreateByEmpName = empInfo.Name;
                    }
                }
                return ContentPlatFOrmOrderDealInfoPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetOrderDealInfoListReportAsync(DateTime? startDate, DateTime? endDate, DateTime? sendStartDate, DateTime? sendEndDate, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, int? consultationType, bool? isToHospital, DateTime? tohospitalStartDate, DateTime? toHospitalEndDate, int? toHospitalType, bool? isDeal, int? lastDealHospitalId, bool? isAccompanying, bool? isOldCustomer, int? CheckState, DateTime? checkStartDate, DateTime? checkEndDate, bool? isCreateBill, bool? isReturnBakcPrice, DateTime? returnBackPriceStartDate, DateTime? returnBackPriceEndDate, int? customerServiceId, string belongCompanyId, string keyWord, int employeeId, bool hidePhone, int? consumptionType, List<int?> liveAnchorIds)
        {
            try
            {
                var dealInfo = from d in dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder) select d;

                if (customerServiceId.HasValue)
                {
                    var employee = await _dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == customerServiceId.Value);
                    //普通客服角色过滤其他订单信息只展示自己录单信息
                    if (employee.IsCustomerService && !employee.AmiyaPositionInfo.IsDirector)
                    {
                        dealInfo = from d in dealInfo
                                   where _dalBindCustomerService.GetAll().Count(e => e.CustomerServiceId == customerServiceId.Value && e.BuyerPhone == d.ContentPlatFormOrder.Phone) > 0 || d.ContentPlatFormOrder.SupportEmpId == customerServiceId.Value || d.ContentPlatFormOrder.BelongEmpId == customerServiceId.Value
                                   where (d.ContentPlatFormOrder.IsSupportOrder == false || d.ContentPlatFormOrder.SupportEmpId == customerServiceId.Value)
                                   select d;
                    }
                }


                ////财务录入数据只有管理员研发财务和CMO能看到
                //if (employee.AmiyaPositionInfo.Id != 1 && employee.AmiyaPositionInfo.Id != 13 && employee.AmiyaPositionInfo.Id != 16 && employee.AmiyaPositionInfo.Id != 29)
                //{

                //    dealInfo = from d in dealInfo
                //               where d.CreateBy != 61 && d.CreateBy != 80 && d.CreateBy != 215 && d.CreateBy != 202
                //               select d;
                //}
                if (startDate != null && endDate != null)
                {
                    DateTime startrq = ((DateTime)startDate).Date;
                    DateTime endrq = ((DateTime)endDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.CreateDate >= startrq && d.CreateDate < endrq)
                               select d;
                }
                if (sendStartDate != null && sendEndDate != null)
                {
                    DateTime startrq = ((DateTime)sendStartDate).Date;
                    DateTime endrq = ((DateTime)sendEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ContentPlatFormOrder.SendDate >= startrq && d.ContentPlatFormOrder.SendDate < endrq)
                               select d;
                }
                if (isToHospital == true)
                {
                    if (!tohospitalStartDate.HasValue && !toHospitalEndDate.HasValue)
                    {
                        throw new Exception("到院时间为必填项，请完整填写到院的开始时间与结束时间！");
                    }
                    DateTime startrq = ((DateTime)tohospitalStartDate).Date;
                    DateTime endrq = ((DateTime)toHospitalEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ToHospitalDate.HasValue)
                               && (d.ToHospitalDate.Value >= startrq && d.ToHospitalDate.Value < endrq)
                               && (d.IsToHospital == true)
                               select d;
                }
                if (isReturnBakcPrice == true)
                {
                    if (!returnBackPriceStartDate.HasValue && !returnBackPriceEndDate.HasValue)
                    {
                        throw new Exception("回款时间为必填项，请完整填写回款的开始时间与结束时间！");
                    }
                    DateTime startrq = ((DateTime)returnBackPriceStartDate).Date;
                    DateTime endrq = ((DateTime)returnBackPriceEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ReturnBackDate.HasValue)
                               && (d.ReturnBackDate.Value >= startrq && d.ReturnBackDate.Value < endrq)
                               && (d.IsToHospital == true)
                               select d;
                }
                if (isCreateBill.HasValue)
                {
                    dealInfo = from d in dealInfo
                               where (d.IsCreateBill == isCreateBill.Value)
                               && (string.IsNullOrEmpty(belongCompanyId) || d.BelongCompany == belongCompanyId)
                               select d;
                }
                if (consumptionType.HasValue)
                {
                    dealInfo = from d in dealInfo
                               where (d.ConsumptionType == consumptionType)
                               select d;
                }
                if (CheckState == (int)CheckType.CheckedSuccess)
                {
                    if (!checkStartDate.HasValue && !checkEndDate.HasValue)
                    {
                        throw new Exception("审核时间为必填项，请完整填写审核的开始时间与结束时间！");
                    }
                    DateTime startrq = ((DateTime)checkStartDate).Date;
                    DateTime endrq = ((DateTime)checkEndDate).Date.AddDays(1);
                    dealInfo = from d in dealInfo
                               where (d.ToHospitalDate.HasValue)
                               && (d.CheckDate.Value >= startrq && d.CheckDate.Value < endrq)
                               && (d.IsToHospital == true)
                               select d;
                }
                var ContentPlatFOrmOrderDealInfo = from d in dealInfo
                                                   where (string.IsNullOrEmpty(keyWord) || d.ContentPlatFormOrderId.Contains(keyWord) || d.ContentPlatFormOrder.Phone.Contains(keyWord) || d.Id.Contains(keyWord))
                                                   && (!isToHospital.HasValue || d.IsToHospital == isToHospital.Value)
                                                   && (!toHospitalType.HasValue || d.ToHospitalType == toHospitalType.Value)
                                                   && (!isDeal.HasValue || d.IsDeal == isDeal.Value)
                                                   && (d.Valid == true)
                                                   && (!lastDealHospitalId.HasValue || d.LastDealHospitalId.Value == lastDealHospitalId.Value)
                                                   && (!isOldCustomer.HasValue || d.IsOldCustomer == isOldCustomer.Value)
                                                   && (!isAccompanying.HasValue || d.IsAcompanying == isAccompanying.Value)
                                                   && (!CheckState.HasValue || d.CheckState == CheckState.Value)
                                                   && (!isReturnBakcPrice.HasValue || d.IsReturnBackPrice == isReturnBakcPrice.Value)
                                                   && (!consultationType.HasValue || d.ContentPlatFormOrder.ConsulationType == consultationType.Value)
                                                   && (!customerServiceId.HasValue || d.ContentPlatFormOrder.BelongEmpId == customerServiceId || d.ContentPlatFormOrder.SupportEmpId == customerServiceId)
                                                   && (!minAddOrderPrice.HasValue || d.ContentPlatFormOrder.AddOrderPrice >= minAddOrderPrice)
                                                   && (!maxAddOrderPrice.HasValue || d.ContentPlatFormOrder.AddOrderPrice <= maxAddOrderPrice)
                                                   && (liveAnchorIds.Count <= 0 || liveAnchorIds.Contains(d.ContentPlatFormOrder.LiveAnchorId))
                                                   select new ContentPlatFormOrderDealInfoDto
                                                   {
                                                       Id = d.Id,
                                                       ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                                                       OrderCreateDate = d.ContentPlatFormOrder.CreateDate,
                                                       ContentPlatFormName = d.ContentPlatFormOrder.Contentplatform.ContentPlatformName,
                                                       LiveAnchorName = d.ContentPlatFormOrder.LiveAnchor.Name,
                                                       ConsultationType = d.ContentPlatFormOrder.ConsulationType,
                                                       ConsultationTypeText = ServiceClass.GetContentPlateFormOrderConsultationTypeText(d.ContentPlatFormOrder.ConsulationType),
                                                       GoodsName = d.ContentPlatFormOrder.AmiyaGoodsDemand.ProjectNname,
                                                       SendDate = d.ContentPlatFormOrder.SendDate,
                                                       CustomerNickName = d.ContentPlatFormOrder.CustomerName,
                                                       AddOrderPrice = d.ContentPlatFormOrder.AddOrderPrice,
                                                       CreateDate = d.CreateDate,
                                                       IsDeal = d.IsDeal,
                                                       IsOldCustomer = d.IsOldCustomer,
                                                       IsAcompanying = d.IsAcompanying,
                                                       DealPerformanceType = d.DealPerformanceType,
                                                       DealPerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(d.DealPerformanceType),
                                                       Phone = hidePhone == true ? ServiceClass.GetIncompletePhone(d.ContentPlatFormOrder.Phone) : d.ContentPlatFormOrder.Phone,
                                                       CommissionRatio = d.CommissionRatio,
                                                       IsToHospital = d.IsToHospital,
                                                       ToHospitalType = d.ToHospitalType,
                                                       ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                                                       ToHospitalDate = d.ToHospitalDate,
                                                       LastDealHospitalId = d.LastDealHospitalId,
                                                       Remark = d.Remark,
                                                       Price = d.Price,
                                                       DealDate = d.DealDate,
                                                       OtherAppOrderId = d.OtherAppOrderId,
                                                       CheckState = d.CheckState,
                                                       CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState.Value),
                                                       CheckPrice = d.CheckPrice,
                                                       CheckDate = d.CheckDate,
                                                       CheckBy = d.CheckBy,
                                                       InformationPrice = d.InformationPrice,
                                                       SystemUpdatePrice = d.SystemUpdatePrice,
                                                       SettlePrice = d.SettlePrice,
                                                       CheckRemark = d.CheckRemark,
                                                       IsCreateBill = d.IsCreateBill,
                                                       BelongCompany = d.BelongCompany,
                                                       IsReturnBackPrice = d.IsReturnBackPrice,
                                                       ReturnBackDate = d.ReturnBackDate,
                                                       ReturnBackPrice = d.ReturnBackPrice,
                                                       CreateBy = d.ContentPlatFormOrder.BelongEmpId.HasValue ? d.ContentPlatFormOrder.BelongEmpId.Value : -1,
                                                       ReconciliationDocumentsId = d.ReconciliationDocumentsId,
                                                       IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                                                       ConsumptionType = d.ConsumptionType,
                                                       ConsumptionTypeText = ServiceClass.GetConsumptionTypeText(d.ConsumptionType),
                                                       CustomerServiceSettlePrice = d.CustomerServiceSettlePrice ?? 0m,
                                                       LastDealInfoId = d.LastDealInfoId,
                                                   };

                List<ContentPlatFormOrderDealInfoDto> ContentPlatFOrmOrderDealInfoPageInfo = new List<ContentPlatFormOrderDealInfoDto>();
                ContentPlatFOrmOrderDealInfoPageInfo = await ContentPlatFOrmOrderDealInfo.OrderByDescending(x => x.CreateDate).ToListAsync();
                foreach (var z in ContentPlatFOrmOrderDealInfoPageInfo)
                {
                    if (z.LastDealHospitalId.HasValue && z.LastDealHospitalId != 0)
                    {
                        var dealHospital = await _hospitalInfoService.GetBaseByIdAsync(z.LastDealHospitalId.Value);
                        z.LastDealHospital = dealHospital.Name;
                    }
                    if (z.CheckBy.HasValue && z.CheckBy != 0)
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CheckBy.Value);
                        z.CheckByEmpName = empInfo.Name;
                    }
                    if (z.CreateBy == 0)
                    {
                        z.CreateByEmpName = "医院";
                    }
                    else if (z.CreateBy == -1)
                    {
                        z.CreateByEmpName = "其他";
                    }
                    else
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CreateBy);
                        z.CreateByEmpName = empInfo.Name;
                    }
                    if (z.IsCreateBill == true)
                    {
                        var belongCompanyInfo = await companyBaseInfoService.GetByIdAsync(z.BelongCompany);
                        z.BelongCompanyName = belongCompanyInfo.Name;
                    }
                    //判断是否为助理补单的业绩
                    if (z.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                    {
                        //取上一条的成交信息
                        var lastDealInfo = await this.GetByIdAsync(z.LastDealInfoId);
                        //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                        if (lastDealInfo.CreateDate < startDate.Value)
                        {
                            z.Price -= lastDealInfo.Price;
                        }
                    }
                }
                return ContentPlatFOrmOrderDealInfoPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 根据订单号展示成交情况
        /// </summary>
        /// <param name="contentPlafFormOrderId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetListWithPageAsync(string contentPlafFormOrderId, int pageNum, int pageSize)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = from d in dalContentPlatFormOrderDealInfo.GetAll()
                                                   where (string.IsNullOrEmpty(contentPlafFormOrderId) || d.ContentPlatFormOrderId == contentPlafFormOrderId)
                                                   where d.Valid == true
                                                   select new ContentPlatFormOrderDealInfoDto
                                                   {
                                                       Id = d.Id,
                                                       ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                                                       CreateDate = d.CreateDate,
                                                       IsDeal = d.IsDeal,
                                                       IsOldCustomer = d.IsOldCustomer,
                                                       IsAcompanying = d.IsAcompanying,
                                                       CommissionRatio = d.CommissionRatio,
                                                       IsToHospital = d.IsToHospital,
                                                       ToHospitalType = d.ToHospitalType,
                                                       ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                                                       ToHospitalDate = d.ToHospitalDate,
                                                       LastDealHospitalId = d.LastDealHospitalId,
                                                       DealPicture = d.DealPicture,
                                                       DealPerformanceType = d.DealPerformanceType,
                                                       DealPerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(d.DealPerformanceType),
                                                       Remark = d.Remark,
                                                       Price = d.Price,
                                                       DealDate = d.DealDate,
                                                       OtherAppOrderId = d.OtherAppOrderId,
                                                       CheckState = d.CheckState,
                                                       CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState.Value),
                                                       CheckPrice = d.CheckPrice,
                                                       CheckDate = d.CheckDate,
                                                       CheckBy = d.CheckBy,
                                                       InformationPrice = d.InformationPrice,
                                                       SystemUpdatePrice = d.SystemUpdatePrice,
                                                       SettlePrice = d.SettlePrice,
                                                       CheckRemark = d.CheckRemark,
                                                       IsReturnBackPrice = d.IsReturnBackPrice,
                                                       ReturnBackDate = d.ReturnBackDate,
                                                       ReturnBackPrice = d.ReturnBackPrice,
                                                       CreateBy = d.CreateBy,
                                                       ReconciliationDocumentsId = d.ReconciliationDocumentsId,
                                                       IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                                                       ConsumptionType = d.ConsumptionType,
                                                       ConsumptionTypeText = ServiceClass.GetConsumptionTypeText(d.ConsumptionType)
                                                   };

                FxPageInfo<ContentPlatFormOrderDealInfoDto> ContentPlatFOrmOrderDealInfoPageInfo = new FxPageInfo<ContentPlatFormOrderDealInfoDto>();
                ContentPlatFOrmOrderDealInfoPageInfo.TotalCount = await ContentPlatFOrmOrderDealInfo.CountAsync();
                ContentPlatFOrmOrderDealInfoPageInfo.List = await ContentPlatFOrmOrderDealInfo.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                foreach (var z in ContentPlatFOrmOrderDealInfoPageInfo.List)
                {
                    if (z.LastDealHospitalId.HasValue)
                    {
                        var dealHospital = await _hospitalInfoService.GetBaseByIdAsync(z.LastDealHospitalId.Value);
                        z.LastDealHospital = dealHospital.Name;
                    }
                    if (z.CreateBy == 0)
                    {
                        z.CreateByEmpName = "医院添加";
                    }
                    else
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CreateBy);
                        z.CreateByEmpName = empInfo.Name;
                    }
                }
                return ContentPlatFOrmOrderDealInfoPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 根据订单号展示成交情况
        /// </summary>
        /// <param name="contentPlafFormOrderId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> HospitalGetListWithPageAsync(string contentPlafFormOrderId, int pageNum, int pageSize, int hospitalId)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = from d in dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.LastDealHospitalId == hospitalId)
                                                   where (string.IsNullOrEmpty(contentPlafFormOrderId) || d.ContentPlatFormOrderId == contentPlafFormOrderId)
                                                   where d.Valid == true
                                                   select new ContentPlatFormOrderDealInfoDto
                                                   {
                                                       Id = d.Id,
                                                       ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                                                       CreateDate = d.CreateDate,
                                                       IsDeal = d.IsDeal,
                                                       IsOldCustomer = d.IsOldCustomer,
                                                       IsAcompanying = d.IsAcompanying,
                                                       CommissionRatio = d.CommissionRatio,
                                                       IsToHospital = d.IsToHospital,
                                                       ToHospitalType = d.ToHospitalType,
                                                       ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                                                       ToHospitalDate = d.ToHospitalDate,
                                                       LastDealHospitalId = d.LastDealHospitalId,
                                                       DealPicture = d.DealPicture,
                                                       DealPerformanceType = d.DealPerformanceType,
                                                       DealPerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(d.DealPerformanceType),
                                                       Remark = d.Remark,
                                                       Price = d.Price,
                                                       DealDate = d.DealDate,
                                                       OtherAppOrderId = d.OtherAppOrderId,
                                                       CheckState = d.CheckState,
                                                       CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState.Value),
                                                       CheckPrice = d.CheckPrice,
                                                       CheckDate = d.CheckDate,
                                                       CheckBy = d.CheckBy,
                                                       InformationPrice = d.InformationPrice,
                                                       SystemUpdatePrice = d.SystemUpdatePrice,
                                                       SettlePrice = d.SettlePrice,
                                                       CheckRemark = d.CheckRemark,
                                                       IsReturnBackPrice = d.IsReturnBackPrice,
                                                       ReturnBackDate = d.ReturnBackDate,
                                                       ReturnBackPrice = d.ReturnBackPrice,
                                                       CreateBy = d.CreateBy,
                                                       ReconciliationDocumentsId = d.ReconciliationDocumentsId,
                                                       IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                                                       ConsumptionType = d.ConsumptionType,
                                                       ConsumptionTypeText = ServiceClass.GetConsumptionTypeText(d.ConsumptionType)
                                                   };

                FxPageInfo<ContentPlatFormOrderDealInfoDto> ContentPlatFOrmOrderDealInfoPageInfo = new FxPageInfo<ContentPlatFormOrderDealInfoDto>();
                ContentPlatFOrmOrderDealInfoPageInfo.TotalCount = await ContentPlatFOrmOrderDealInfo.CountAsync();
                ContentPlatFOrmOrderDealInfoPageInfo.List = await ContentPlatFOrmOrderDealInfo.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                foreach (var z in ContentPlatFOrmOrderDealInfoPageInfo.List)
                {
                    if (z.LastDealHospitalId.HasValue)
                    {
                        var dealHospital = await _hospitalInfoService.GetBaseByIdAsync(z.LastDealHospitalId.Value);
                        z.LastDealHospital = dealHospital.Name;
                    }
                    if (z.CreateBy == 0)
                    {
                        z.CreateByEmpName = "医院添加";
                    }
                    else
                    {
                        var empInfo = await _amiyaEmployeeService.GetByIdAsync(z.CreateBy);
                        z.CreateByEmpName = empInfo.Name;
                    }
                }
                return ContentPlatFOrmOrderDealInfoPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task<string> AddAsync(AddContentPlatFormOrderDealInfoDto addDto)
        {
            try
            {
                ContentPlatformOrderDealInfo ContentPlatFOrmOrderDealInfo = new ContentPlatformOrderDealInfo();
                ContentPlatFOrmOrderDealInfo.Id = CreateOrderIdHelper.GetNextNumber();
                ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId = addDto.ContentPlatFormOrderId;
                ContentPlatFOrmOrderDealInfo.CreateDate = addDto.CreateDate;
                ContentPlatFOrmOrderDealInfo.IsToHospital = addDto.IsToHospital;
                ContentPlatFOrmOrderDealInfo.ToHospitalType = addDto.ToHospitalType;
                ContentPlatFOrmOrderDealInfo.ToHospitalDate = addDto.ToHospitalDate;
                ContentPlatFOrmOrderDealInfo.LastDealHospitalId = addDto.LastDealHospitalId;
                ContentPlatFOrmOrderDealInfo.IsDeal = addDto.IsDeal;
                ContentPlatFOrmOrderDealInfo.DealPicture = addDto.DealPicture;
                ContentPlatFOrmOrderDealInfo.Remark = addDto.Remark;
                ContentPlatFOrmOrderDealInfo.Price = addDto.Price;
                ContentPlatFOrmOrderDealInfo.DealDate = addDto.DealDate;
                ContentPlatFOrmOrderDealInfo.OtherAppOrderId = addDto.OtherAppOrderId;
                ContentPlatFOrmOrderDealInfo.IsAcompanying = addDto.IsAcompanying;
                ContentPlatFOrmOrderDealInfo.IsOldCustomer = addDto.IsOldCustomer;
                ContentPlatFOrmOrderDealInfo.CreateBy = addDto.CreateBy;
                ContentPlatFOrmOrderDealInfo.CommissionRatio = addDto.CommissionRatio;
                ContentPlatFOrmOrderDealInfo.CheckBy = 0;
                ContentPlatFOrmOrderDealInfo.CheckPrice = 0.00M;
                ContentPlatFOrmOrderDealInfo.CheckState = 0;
                ContentPlatFOrmOrderDealInfo.SettlePrice = 0.00M;
                ContentPlatFOrmOrderDealInfo.DealPerformanceType = addDto.DealPerformanceType;
                ContentPlatFOrmOrderDealInfo.IsRepeatProfundityOrder = addDto.IsRepeatProfundityOrder;
                ContentPlatFOrmOrderDealInfo.ConsumptionType = addDto.ConsumptionType;
                ContentPlatFOrmOrderDealInfo.LastDealInfoId = addDto.LastDealInfoId;
                ContentPlatFOrmOrderDealInfo.LastDealInfoCreateDate = addDto.LastDealInfoCreateDate;
                ContentPlatFOrmOrderDealInfo.Valid = true;
                await dalContentPlatFormOrderDealInfo.AddAsync(ContentPlatFOrmOrderDealInfo, true);

                //添加邀约凭证图片
                if (addDto.InvitationDocuments != null)
                {
                    foreach (var z in addDto.InvitationDocuments)
                    {
                        AddContentPlatFormCustomerPictureDto addPicture = new AddContentPlatFormCustomerPictureDto();
                        addPicture.OrderDealId = ContentPlatFOrmOrderDealInfo.Id;
                        addPicture.ContentPlatFormOrderId = addDto.ContentPlatFormOrderId;
                        addPicture.CustomerPicture = z;
                        addPicture.Description = "邀约凭证";
                        await _contentPlatFormCustomerPictureService.AddAsync(addPicture);
                    }
                }

                //添加成交详情
                if (addDto.AddContentPlatFormOrderDealDetailsDtoList.Count > 0)
                {
                    foreach (var x in addDto.AddContentPlatFormOrderDealDetailsDtoList)
                    {
                        x.ContentPlatFormOrderDealId = ContentPlatFOrmOrderDealInfo.Id;
                        await contentPlatFormOrderDealDetailsService.AddAsync(x);
                    }
                }
                return ContentPlatFOrmOrderDealInfo.Id;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<ContentPlatFormOrderDealInfoDto> GetByIdAsync(string id)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).SingleOrDefaultAsync(e => e.Id == id);
                if (ContentPlatFOrmOrderDealInfo == null)
                {
                    throw new Exception("未找到该成交信息");
                }

                ContentPlatFormOrderDealInfoDto contentPlatFOrmOrderDealInfoDto = new ContentPlatFormOrderDealInfoDto();
                contentPlatFOrmOrderDealInfoDto.Id = ContentPlatFOrmOrderDealInfo.Id;
                contentPlatFOrmOrderDealInfoDto.ContentPlatFormOrderId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId;
                contentPlatFOrmOrderDealInfoDto.CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate;
                contentPlatFOrmOrderDealInfoDto.CustomerPhone = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.Phone;
                contentPlatFOrmOrderDealInfoDto.IsToHospital = ContentPlatFOrmOrderDealInfo.IsToHospital;
                contentPlatFOrmOrderDealInfoDto.ToHospitalType = ContentPlatFOrmOrderDealInfo.ToHospitalType;
                contentPlatFOrmOrderDealInfoDto.ToHospitalDate = ContentPlatFOrmOrderDealInfo.ToHospitalDate;
                contentPlatFOrmOrderDealInfoDto.LastDealHospitalId = ContentPlatFOrmOrderDealInfo.LastDealHospitalId;
                contentPlatFOrmOrderDealInfoDto.IsDeal = ContentPlatFOrmOrderDealInfo.IsDeal;
                contentPlatFOrmOrderDealInfoDto.DealPicture = ContentPlatFOrmOrderDealInfo.DealPicture;
                contentPlatFOrmOrderDealInfoDto.Remark = ContentPlatFOrmOrderDealInfo.Remark;
                contentPlatFOrmOrderDealInfoDto.Price = ContentPlatFOrmOrderDealInfo.Price;
                contentPlatFOrmOrderDealInfoDto.DealDate = ContentPlatFOrmOrderDealInfo.DealDate;
                contentPlatFOrmOrderDealInfoDto.OtherAppOrderId = ContentPlatFOrmOrderDealInfo.OtherAppOrderId;
                contentPlatFOrmOrderDealInfoDto.IsAcompanying = ContentPlatFOrmOrderDealInfo.IsAcompanying;
                contentPlatFOrmOrderDealInfoDto.IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer;
                contentPlatFOrmOrderDealInfoDto.CommissionRatio = ContentPlatFOrmOrderDealInfo.CommissionRatio;
                contentPlatFOrmOrderDealInfoDto.CheckState = ContentPlatFOrmOrderDealInfo.CheckState;
                contentPlatFOrmOrderDealInfoDto.CheckStateText = ServiceClass.GetCheckTypeText(ContentPlatFOrmOrderDealInfo.CheckState.Value);
                contentPlatFOrmOrderDealInfoDto.CheckPrice = ContentPlatFOrmOrderDealInfo.CheckPrice;
                contentPlatFOrmOrderDealInfoDto.CheckDate = ContentPlatFOrmOrderDealInfo.CheckDate;
                contentPlatFOrmOrderDealInfoDto.CheckBy = ContentPlatFOrmOrderDealInfo.CheckBy;
                contentPlatFOrmOrderDealInfoDto.SettlePrice = ContentPlatFOrmOrderDealInfo.SettlePrice;
                contentPlatFOrmOrderDealInfoDto.DealPerformanceType = ContentPlatFOrmOrderDealInfo.DealPerformanceType;
                contentPlatFOrmOrderDealInfoDto.CheckRemark = ContentPlatFOrmOrderDealInfo.CheckRemark;
                contentPlatFOrmOrderDealInfoDto.IsReturnBackPrice = ContentPlatFOrmOrderDealInfo.IsReturnBackPrice;
                contentPlatFOrmOrderDealInfoDto.ReturnBackDate = ContentPlatFOrmOrderDealInfo.ReturnBackDate;
                contentPlatFOrmOrderDealInfoDto.ReturnBackPrice = ContentPlatFOrmOrderDealInfo.ReturnBackPrice;
                contentPlatFOrmOrderDealInfoDto.CreateBy = ContentPlatFOrmOrderDealInfo.CreateBy;
                contentPlatFOrmOrderDealInfoDto.ReconciliationDocumentsId = ContentPlatFOrmOrderDealInfo.ReconciliationDocumentsId;
                contentPlatFOrmOrderDealInfoDto.ConsumptionType = ContentPlatFOrmOrderDealInfo.ConsumptionType;
                contentPlatFOrmOrderDealInfoDto.ConsultationTypeText = ServiceClass.GetConsumptionTypeText(ContentPlatFOrmOrderDealInfo.ConsumptionType);
                var InvitationDocuments = await _contentPlatFormCustomerPictureService.GetListWithPageAsync(null, ContentPlatFOrmOrderDealInfo.Id, "邀约凭证", 1, 5);
                contentPlatFOrmOrderDealInfoDto.InvitationDocuments = new List<string>();
                foreach (var x in InvitationDocuments.List)
                {
                    contentPlatFOrmOrderDealInfoDto.InvitationDocuments.Add(x.CustomerPicture);
                }
                return contentPlatFOrmOrderDealInfoDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 拆单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SplitOrderDealInfo(SplitContentPlatFormOrderDealInfoDto input)
        {
            var dealInfo = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).SingleOrDefaultAsync(e => e.Id == input.DealId && e.Valid == true);
            if (dealInfo.CheckState == (int)CheckState.CheckSuccess)
            {
                throw new Exception("该笔成交单已完成审核，无法拆单！");
            }
            dealInfo.Price = input.LocalDealPirce;
            await dalContentPlatFormOrderDealInfo.UpdateAsync(dealInfo, true);
            foreach (var x in input.DealPrice)
            {
                var insertData = dealInfo;
                insertData.Price = x;
                insertData.Id = CreateOrderIdHelper.GetNextNumber() + "C";
                await dalContentPlatFormOrderDealInfo.AddAsync(insertData, true);
            }

        }
        public async Task<ContentPlatFormOrderDealInfoDto> GetByOtherAppOrderIdAsync(string otherAppOrder)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.OtherAppOrderId == otherAppOrder && e.Valid == true);
                if (ContentPlatFOrmOrderDealInfo == null)
                {
                    return new ContentPlatFormOrderDealInfoDto();
                }

                ContentPlatFormOrderDealInfoDto contentPlatFOrmOrderDealInfoDto = new ContentPlatFormOrderDealInfoDto();
                contentPlatFOrmOrderDealInfoDto.Id = ContentPlatFOrmOrderDealInfo.Id;
                return contentPlatFOrmOrderDealInfoDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 获取消费类型名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseKeyValueDto<int>>> GetConsumptionTypeAsync()
        {
            var showDirectionTypes = Enum.GetValues(typeof(ConsumptionType));
            List<BaseKeyValueDto<int>> requestTypeList = new List<BaseKeyValueDto<int>>();
            foreach (var item in showDirectionTypes)
            {
                BaseKeyValueDto<int> requestType = new BaseKeyValueDto<int>();
                requestType.Key = Convert.ToInt32(item);
                requestType.Value = ServiceClass.GetConsumptionTypeText(Convert.ToInt32(item));
                requestTypeList.Add(requestType);
            }
            return requestTypeList;
        }
        public async Task UpdateAsync(UpdateContentPlatFormOrderDealInfoDto updateDto)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (ContentPlatFOrmOrderDealInfo == null)
                    throw new Exception("未找到该成交信息！");


                ContentPlatFOrmOrderDealInfo.Id = updateDto.Id;
                ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId = updateDto.ContentPlatFormOrderId;
                ContentPlatFOrmOrderDealInfo.IsToHospital = updateDto.IsToHospital;
                ContentPlatFOrmOrderDealInfo.ToHospitalType = updateDto.ToHospitalType;
                ContentPlatFOrmOrderDealInfo.ToHospitalDate = updateDto.ToHospitalDate;
                ContentPlatFOrmOrderDealInfo.LastDealHospitalId = updateDto.LastDealHospitalId;
                ContentPlatFOrmOrderDealInfo.IsDeal = updateDto.IsDeal;
                ContentPlatFOrmOrderDealInfo.DealPicture = updateDto.DealPicture;
                ContentPlatFOrmOrderDealInfo.Remark = updateDto.Remark;
                ContentPlatFOrmOrderDealInfo.Price = updateDto.Price;
                ContentPlatFOrmOrderDealInfo.DealDate = updateDto.DealDate;
                ContentPlatFOrmOrderDealInfo.OtherAppOrderId = updateDto.OtherAppOrderId;
                ContentPlatFOrmOrderDealInfo.IsAcompanying = updateDto.IsAcompanying;
                ContentPlatFOrmOrderDealInfo.IsOldCustomer = updateDto.IsOldCustomer;
                ContentPlatFOrmOrderDealInfo.DealPerformanceType = updateDto.DealPerformanceType;
                ContentPlatFOrmOrderDealInfo.CommissionRatio = updateDto.CommissionRatio;
                ContentPlatFOrmOrderDealInfo.ConsumptionType = updateDto.ConsumptionType;
                await dalContentPlatFormOrderDealInfo.UpdateAsync(ContentPlatFOrmOrderDealInfo, true);
                //清除邀约凭证图片
                await _contentPlatFormCustomerPictureService.DeleteByContentPlatFormOrderDealIdAsync(updateDto.Id);
                //添加邀约凭证图片
                foreach (var z in updateDto.InvitationDocuments)
                {
                    AddContentPlatFormCustomerPictureDto addPicture = new AddContentPlatFormCustomerPictureDto();
                    addPicture.OrderDealId = ContentPlatFOrmOrderDealInfo.Id;
                    addPicture.ContentPlatFormOrderId = updateDto.ContentPlatFormOrderId;
                    addPicture.CustomerPicture = z;
                    addPicture.Description = "邀约凭证";
                    await _contentPlatFormCustomerPictureService.AddAsync(addPicture);
                }
                //清除自己提交的成交详情
                DeleteContentPlatFormOrderDealDetailsByDealIdDto deleteContentPlatFormOrderDealDetailsByDealIdDto = new DeleteContentPlatFormOrderDealDetailsByDealIdDto();
                deleteContentPlatFormOrderDealDetailsByDealIdDto.DealId = updateDto.Id;
                deleteContentPlatFormOrderDealDetailsByDealIdDto.CreateBy = updateDto.UpdateBy;
                await contentPlatFormOrderDealDetailsService.DeleteByDealIdAsync(deleteContentPlatFormOrderDealDetailsByDealIdDto);

                //添加成交详情
                if (updateDto.AddContentPlatFormOrderDealDetailsDtoList.Count > 0)
                {
                    //添加成交详情
                    foreach (var x in updateDto.AddContentPlatFormOrderDealDetailsDtoList)
                    {
                        x.ContentPlatFormOrderDealId = ContentPlatFOrmOrderDealInfo.Id;
                        await contentPlatFormOrderDealDetailsService.AddAsync(x);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateIsOldCustomerAsync(string orderDealId, bool isOldCustomer)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.Id == orderDealId);
                if (ContentPlatFOrmOrderDealInfo == null)
                    throw new Exception("未找到该成交信息！");
                ContentPlatFOrmOrderDealInfo.IsOldCustomer = isOldCustomer;
                await dalContentPlatFormOrderDealInfo.UpdateAsync(ContentPlatFOrmOrderDealInfo, true);
                await _contentPlatFormCustomerPictureService.DeleteByContentPlatFormOrderDealIdAsync(ContentPlatFOrmOrderDealInfo.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateDealPictureAsync(string dealId, string picture, int empId)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.Id == dealId);
                if (ContentPlatFOrmOrderDealInfo == null)
                    throw new Exception("未找到该成交信息！");
                if (ContentPlatFOrmOrderDealInfo.CreateBy != empId)
                {
                    throw new Exception("无法修改他人的成交截图！");
                }
                ContentPlatFOrmOrderDealInfo.DealPicture = picture;
                await dalContentPlatFormOrderDealInfo.UpdateAsync(ContentPlatFOrmOrderDealInfo, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 审核成交情况
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task CheckAsync(UpdateContentPlatFormOrderDealInfoDto updateDto)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (ContentPlatFOrmOrderDealInfo == null)
                    throw new Exception("未找到该成交信息！");


                ContentPlatFOrmOrderDealInfo.Id = updateDto.Id;
                ContentPlatFOrmOrderDealInfo.CheckBy = updateDto.CheckBy;
                ContentPlatFOrmOrderDealInfo.CheckDate = DateTime.Now;
                ContentPlatFOrmOrderDealInfo.CheckPrice = updateDto.CheckPrice;
                ContentPlatFOrmOrderDealInfo.CheckRemark = updateDto.CheckRemark;
                ContentPlatFOrmOrderDealInfo.CheckState = updateDto.CheckState;
                ContentPlatFOrmOrderDealInfo.CustomerServiceSettlePrice = updateDto.CustomerServiceSettlePrice;
                ContentPlatFOrmOrderDealInfo.SettlePrice = updateDto.SettlePrice;
                ContentPlatFOrmOrderDealInfo.InformationPrice = updateDto.InformationPrice;
                ContentPlatFOrmOrderDealInfo.SystemUpdatePrice = updateDto.SystemUpdatePrice;
                ContentPlatFOrmOrderDealInfo.ReconciliationDocumentsId = updateDto.ReconciliationDocumentsId;
                await dalContentPlatFormOrderDealInfo.UpdateAsync(ContentPlatFOrmOrderDealInfo, true);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 成交情况回款
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task SettleAsync(UpdateContentPlatFormOrderDealInfoDto updateDto)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (ContentPlatFOrmOrderDealInfo == null)
                    throw new Exception("未找到该成交信息！");


                ContentPlatFOrmOrderDealInfo.Id = updateDto.Id;
                ContentPlatFOrmOrderDealInfo.IsReturnBackPrice = true;
                ContentPlatFOrmOrderDealInfo.ReturnBackDate = updateDto.ReturnBackDate;
                ContentPlatFOrmOrderDealInfo.ReturnBackPrice = updateDto.ReturnBackPrice;
                await dalContentPlatFormOrderDealInfo.UpdateAsync(ContentPlatFOrmOrderDealInfo, true);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 根据对账单id批量回款
        /// </summary>
        /// <param name="reconciliationDocumentsList"></param>
        /// <returns></returns>
        public async Task SettleListAsync(ReturnBackOrderDto returnBackOrder)
        {
            try
            {

                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.Id == returnBackOrder.OrderDealId).FirstOrDefaultAsync();
                if (ContentPlatFOrmOrderDealInfo != null)
                {
                    OrderReturnBackPriceDto orderReturnBackPriceDto = new OrderReturnBackPriceDto();
                    ContentPlatFOrmOrderDealInfo.IsReturnBackPrice = true;
                    ContentPlatFOrmOrderDealInfo.ReturnBackDate = returnBackOrder.ReturnBackDate;
                    if (ContentPlatFOrmOrderDealInfo.ReturnBackPrice.HasValue)
                    {
                        ContentPlatFOrmOrderDealInfo.ReturnBackPrice += returnBackOrder.ReturnBackPrice;
                    }
                    else
                    {
                        ContentPlatFOrmOrderDealInfo.ReturnBackPrice = returnBackOrder.ReturnBackPrice;
                    }
                    await dalContentPlatFormOrderDealInfo.UpdateAsync(ContentPlatFOrmOrderDealInfo, true);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var ContentPlatFOrmOrderDealInfo = await dalContentPlatFormOrderDealInfo.GetAll().SingleOrDefaultAsync(e => e.Id == id);

                if (ContentPlatFOrmOrderDealInfo == null)
                    throw new Exception("未找到该成交信息");

                await dalContentPlatFormOrderDealInfo.DeleteAsync(ContentPlatFOrmOrderDealInfo, true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetByOrderIdAsync(string orderId)
        {
            try
            {
                List<ContentPlatFormOrderDealInfoDto> returnList = new List<ContentPlatFormOrderDealInfoDto>();
                var selectResult = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.ContentPlatFormOrderId == orderId && e.Valid == true).ToListAsync();
                if (selectResult == null)
                {
                    return returnList;
                }
                foreach (var ContentPlatFOrmOrderDealInfo in selectResult)
                {
                    ContentPlatFormOrderDealInfoDto contentPlatFOrmOrderDealInfoDto = new ContentPlatFormOrderDealInfoDto();
                    contentPlatFOrmOrderDealInfoDto.Id = ContentPlatFOrmOrderDealInfo.Id;
                    contentPlatFOrmOrderDealInfoDto.ContentPlatFormOrderId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId;
                    contentPlatFOrmOrderDealInfoDto.CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate;
                    contentPlatFOrmOrderDealInfoDto.IsToHospital = ContentPlatFOrmOrderDealInfo.IsToHospital;
                    contentPlatFOrmOrderDealInfoDto.ToHospitalType = ContentPlatFOrmOrderDealInfo.ToHospitalType;
                    contentPlatFOrmOrderDealInfoDto.ToHospitalDate = ContentPlatFOrmOrderDealInfo.ToHospitalDate;
                    contentPlatFOrmOrderDealInfoDto.LastDealHospitalId = ContentPlatFOrmOrderDealInfo.LastDealHospitalId;
                    contentPlatFOrmOrderDealInfoDto.IsDeal = ContentPlatFOrmOrderDealInfo.IsDeal;
                    contentPlatFOrmOrderDealInfoDto.DealPicture = ContentPlatFOrmOrderDealInfo.DealPicture;
                    contentPlatFOrmOrderDealInfoDto.Remark = ContentPlatFOrmOrderDealInfo.Remark;
                    contentPlatFOrmOrderDealInfoDto.Price = ContentPlatFOrmOrderDealInfo.Price;
                    contentPlatFOrmOrderDealInfoDto.DealDate = ContentPlatFOrmOrderDealInfo.DealDate;
                    contentPlatFOrmOrderDealInfoDto.OtherAppOrderId = ContentPlatFOrmOrderDealInfo.OtherAppOrderId;
                    contentPlatFOrmOrderDealInfoDto.IsAcompanying = ContentPlatFOrmOrderDealInfo.IsAcompanying;
                    contentPlatFOrmOrderDealInfoDto.IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer;
                    contentPlatFOrmOrderDealInfoDto.CommissionRatio = ContentPlatFOrmOrderDealInfo.CommissionRatio;
                    contentPlatFOrmOrderDealInfoDto.CheckState = ContentPlatFOrmOrderDealInfo.CheckState;
                    contentPlatFOrmOrderDealInfoDto.CheckStateText = ServiceClass.GetCheckTypeText(ContentPlatFOrmOrderDealInfo.CheckState.Value);
                    contentPlatFOrmOrderDealInfoDto.CheckPrice = ContentPlatFOrmOrderDealInfo.CheckPrice;
                    contentPlatFOrmOrderDealInfoDto.CheckDate = ContentPlatFOrmOrderDealInfo.CheckDate;
                    contentPlatFOrmOrderDealInfoDto.CheckBy = ContentPlatFOrmOrderDealInfo.CheckBy;
                    contentPlatFOrmOrderDealInfoDto.SettlePrice = ContentPlatFOrmOrderDealInfo.SettlePrice;
                    contentPlatFOrmOrderDealInfoDto.CheckRemark = ContentPlatFOrmOrderDealInfo.CheckRemark;
                    contentPlatFOrmOrderDealInfoDto.IsReturnBackPrice = ContentPlatFOrmOrderDealInfo.IsReturnBackPrice;
                    contentPlatFOrmOrderDealInfoDto.ReturnBackDate = ContentPlatFOrmOrderDealInfo.ReturnBackDate;
                    contentPlatFOrmOrderDealInfoDto.ReturnBackPrice = ContentPlatFOrmOrderDealInfo.ReturnBackPrice;
                    contentPlatFOrmOrderDealInfoDto.CreateBy = ContentPlatFOrmOrderDealInfo.CreateBy;
                    contentPlatFOrmOrderDealInfoDto.DealPerformanceType = ContentPlatFOrmOrderDealInfo.DealPerformanceType;
                    contentPlatFOrmOrderDealInfoDto.DealPerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(ContentPlatFOrmOrderDealInfo.DealPerformanceType);
                    contentPlatFOrmOrderDealInfoDto.InformationPrice = ContentPlatFOrmOrderDealInfo.InformationPrice;
                    contentPlatFOrmOrderDealInfoDto.SystemUpdatePrice = ContentPlatFOrmOrderDealInfo.SystemUpdatePrice;
                    contentPlatFOrmOrderDealInfoDto.ConsumptionType = ContentPlatFOrmOrderDealInfo.ConsumptionType;
                    contentPlatFOrmOrderDealInfoDto.CustomerServiceSettlePrice = ContentPlatFOrmOrderDealInfo.CustomerServiceSettlePrice.HasValue ? ContentPlatFOrmOrderDealInfo.CustomerServiceSettlePrice.Value : 0.00M;
                    contentPlatFOrmOrderDealInfoDto.ReconciliationDocumentsId = ContentPlatFOrmOrderDealInfo.ReconciliationDocumentsId;
                    returnList.Add(contentPlatFOrmOrderDealInfoDto);
                }
                return returnList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 获取总业绩
        /// </summary>
        /// <param name="IsOldCustomer">新老客</param>
        /// <returns></returns>
        public async Task<decimal> GetPerformance(bool? IsOldCustomer)
        {
            var performance = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.IsToHospital == true && (IsOldCustomer == null || e.IsOldCustomer == IsOldCustomer) && e.Valid == true).SumAsync(e => e.Price);
            return performance;
        }
        /// <summary>
        /// 新客上门人数
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetNewCustomerToHospitalCount()
        {
            var count = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.IsToHospital == true && e.IsOldCustomer == false && e.Valid == true).CountAsync();
            return count;
        }
        /// <summary>
        /// 新客成交人数
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetNewCustomerDealCount()
        {
            var count = await dalContentPlatFormOrderDealInfo.GetAll().Where(e => e.IsDeal == true && e.IsOldCustomer == false && e.Valid == true).CountAsync();
            return count;
        }

        #region【业绩板块】


        /// <summary>
        /// 根据主播获取啊美雅业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldCustomer">新客/老客</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceByYearAndMonth(int year, int month, bool? isOldCustomer, List<int> LiveAnchorIds)
        {
            // int maxDays = DateTime.DaysInMonth(year, month);

            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            //选定的月份
            DateTime currentDate = new DateTime(year, month, 1);
            return await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
                .Where(o => o.CreateDate >= currentDate && o.CreateDate < endDate && o.IsDeal == true)
                .Where(o => isOldCustomer == null || o.IsOldCustomer == isOldCustomer)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                 .Where(o => o.Valid == true)
                .Select(
                  ContentPlatFOrmOrderDealInfo =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           Price = ContentPlatFOrmOrderDealInfo.Price,
                           IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer
                       }
                ).ToListAsync();
        }

        /// <summary>
        /// 获取派单成交业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend">历史/当月派单,true为历史派单当月成交，false为当月派单当月成交</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetSendAndDealPerformanceByYearAndMonth(int year, int month, bool? isOldSend, List<int> liveAnchorIds)
        {
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            //选定的月份
            DateTime currentDate = new DateTime(year, month, 1);
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Where(x => x.ContentPlatFormOrderId != null).Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
                .Where(o => o.CreateDate >= currentDate && o.CreateDate < endDate && o.IsDeal == true)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                  .Where(o => o.Valid == true)
                .ToListAsync();
            if (isOldSend == true)
            {
                result = result.Where(x => x.ContentPlatFormOrder.SendDate.HasValue && x.ContentPlatFormOrder.SendDate.Value.Month != x.CreateDate.Month).ToList();
            }
            if (isOldSend == false)
            {
                result = result.Where(x => x.ContentPlatFormOrder.SendDate.HasValue && x.ContentPlatFormOrder.SendDate.Value.Month == x.CreateDate.Month).ToList();
            }
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           Id = d.Id,
                           ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                           CreateDate = d.CreateDate,
                           SendDate = d.ContentPlatFormOrder.SendDate,
                           CustomerNickName = d.ContentPlatFormOrder.CustomerName,
                           Phone = ServiceClass.GetIncompletePhone(d.ContentPlatFormOrder.Phone),
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                           AddOrderPrice = d.ContentPlatFormOrder.AddOrderPrice,
                           Price = d.Price,
                       }
                ).ToList();

            return returnInfo;
        }


        /// <summary>
        /// 根据到院id获取上门成交业绩
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetTodaySendPerformanceByHospitalIdAsync(List<int> hospitalId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = DateTime.Now.Date.AddDays(1);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.IsToHospital == true && o.ToHospitalDate.HasValue == true && o.ToHospitalDate >= currentDate && o.ToHospitalDate < endDate)
                .Where(o => hospitalId.Count == 0 || hospitalId.Contains(o.LastDealHospitalId.Value))
                  .Where(o => o.Valid == true)
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           ToHospitalDate = d.ToHospitalDate,
                           DealDate = d.DealDate,
                       }
                ).ToList();

            return returnInfo;
        }
        /// <summary>
        /// 根据到院id获取上门成交业绩
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetSendPerformanceByHospitalIdAndDateTimeAsync(QueryHospitalTransformDataDto query)
        {

            var result = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.IsToHospital == true && o.ToHospitalDate.HasValue == true && o.ToHospitalDate >= query.StartDate && o.ToHospitalDate < query.EndDate)
                .Where(o => query.HospitalId.Count == 0 || query.HospitalId.Contains(o.LastDealHospitalId.Value))
                .Where(o => query.LiveAnchorIds.Count == 0 || query.LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                  .Where(o => o.Valid == true)
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           ToHospitalDate = d.ToHospitalDate,
                           DealDate = d.DealDate,
                       }
                ).ToList();

            return returnInfo;
        }

        /// <summary>
        /// 根据到院id获取当月上门成交业绩
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetMonthSendPerformanceByHospitalIdListAsync(List<int> hospitalIds, DateTime recordDate)
        {
            //筛选结束的月份
            int days = DateTime.DaysInMonth(recordDate.Year, recordDate.Month);
            DateTime endDate = Convert.ToDateTime(recordDate.Year + "-" + recordDate.Month + "-" + days);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.IsToHospital == true && o.CreateDate >= currentDate && o.CreateDate < endDate)
                  .Where(o => o.Valid == true)
                .Where(o => hospitalIds.Count == 0 || hospitalIds.Contains(o.LastDealHospitalId.Value))
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           ToHospitalDate = d.ToHospitalDate,
                           DealDate = d.DealDate,
                       }
                ).ToList();

            return returnInfo;
        }

        /// <summary>
        /// 根据到院id和时间获取当月上门成交业绩
        /// </summary>
        /// <param name="recordStartDate"></param>
        /// <param name="recordEndDate"></param>
        /// <param name="hospitalIds"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetMonthSendPerformanceByHospitalIdListAsync(List<int> hospitalIds, DateTime recordStartDate, DateTime recordEndDate)
        {
            var result = await dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.IsToHospital == true && o.CreateDate >= recordStartDate && o.CreateDate < recordEndDate)
                .Where(o => hospitalIds.Count == 0 || hospitalIds.Contains(o.LastDealHospitalId.Value))
                  .Where(o => o.Valid == true)
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           ToHospitalDate = d.ToHospitalDate,
                           DealDate = d.DealDate,
                       }
                ).ToList();
            foreach (var z in returnInfo)
            {
                //判断是否为助理补单的业绩
                if (z.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                {
                    //取上一条的成交信息
                    var lastDealInfo = await this.GetByIdAsync(z.LastDealInfoId);
                    //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                    if (lastDealInfo.CreateDate < recordStartDate)
                    {
                        z.Price -= lastDealInfo.Price;
                    }
                }
            }
            return returnInfo;
        }

        /// <summary>
        /// 根据到院id，时间和手机号获取当月上门成交业绩
        /// </summary>
        /// <param name="recordStartDate"></param>
        /// <param name="recordEndDate"></param>
        /// <param name="hospitalIds"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GeVisitAndDealNumByHospitalIdAndPhoneListAsync(List<int> hospitalIds, DateTime recordStartDate, DateTime recordEndDate, List<string> phoneList)
        {
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                .Where(o => o.IsToHospital == true && o.CreateDate >= recordStartDate && o.CreateDate < recordEndDate)
                  .Where(o => o.Valid == true)
                .Where(o => hospitalIds.Count == 0 || hospitalIds.Contains(o.LastDealHospitalId.Value))
                .Where(x => phoneList.Contains(x.ContentPlatFormOrder.Phone))
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           LastDealHospitalId = d.LastDealHospitalId,
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           ToHospitalDate = d.ToHospitalDate,
                           DealDate = d.DealDate,
                       }
                ).ToList();

            return returnInfo;
        }
        /// <summary>
        /// 根据到院id与月份获取上门成交业绩
        /// </summary>
        /// <param name="month"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetSendPerformanceByHospitalIdAndMonthAsync(int hospitalId, int year, int month)
        {
            //筛选结束的月份
            //DateTime dtNow = DateTime.Now;
            //int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            //DateTime endDate = Convert.ToDateTime(DateTime.Now.Year + "-" + month + "-" + days);
            //选定的月份
            //DateTime currentDate = Convert.ToDateTime(DateTime.Now.Year + "-" + month + "-01");
            DateTime startDate = Convert.ToDateTime(year + "-" + month + "-01");
            DateTime endDate = startDate.AddMonths(1);
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                  .Where(o => o.Valid == true)
                .Where(o => o.IsToHospital == true && o.ToHospitalDate.HasValue == true && o.ToHospitalDate >= startDate && o.ToHospitalDate < endDate)
                .Where(o => hospitalId == 0 || o.LastDealHospitalId.Value == hospitalId)
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           ToHospitalDate = d.ToHospitalDate,
                           DealDate = d.DealDate,
                       }
                ).ToList();

            return returnInfo;
        }


        /// <summary>
        /// 根据主播id获取当日上门成交业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetTodaySendPerformanceAsync(int liveAnchorId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = recordDate.Date.AddDays(1);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).Where(o => o.Valid == true)
                .Where(o => o.IsToHospital == true && o.ToHospitalDate.HasValue == true && o.ToHospitalDate >= currentDate && o.ToHospitalDate < endDate)
                .Where(o => liveAnchorId == 0 || o.ContentPlatFormOrder.LiveAnchorId == liveAnchorId)
                .ToListAsync();
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           IsToHospital = d.IsToHospital,
                           IsDeal = d.IsDeal,
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalType = d.ToHospitalType,
                           Price = d.Price,
                           AddOrderPrice = d.ContentPlatFormOrder.AddOrderPrice
                       }
                ).ToList();

            return returnInfo;
        }

        /// <summary>
        /// 新/老客业绩折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isCustomer"></param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        public async Task<List<PerformanceInfoByDateDto>> GetPerformanceBrokenLineAsync(int year, int month, bool? isCustomer, List<int> LiveAnchorIds)
        {
            //开始月份
            DateTime startTime = new DateTime(year, 1, 1);
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            var orderinfo = await dalContentPlatFormOrderDealInfo.GetAll().Where(o => o.Valid == true)
                .Where(o => o.IsDeal == true && o.CreateDate >= startTime && o.CreateDate < endDate)
                .Where(o => isCustomer == null || o.IsOldCustomer == isCustomer)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id)).ToListAsync();

            var list = orderinfo.Select(x => new PerformanceInfoDateDto { Date = x.CreateDate, PerfomancePrice = x.Price }).ToList();
            var returnResult = list.GroupBy(x => x.Date.Month).Select(x => new PerformanceInfoByDateDto { Date = x.Key.ToString(), PerfomancePrice = x.Sum(z => z.PerfomancePrice) }).ToList();
            return returnResult;
        }

        /// <summary>
        /// 成交情况折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend"></param>
        /// <returns></returns>
        public async Task<List<PerformanceBrokenLine>> GetHistoryAndThisMonthOrderPerformance(int year, int month, bool? isOldSend, List<int> liveAnchorIds)
        {
            //开始月份
            DateTime startTime = new DateTime(year, 1, 1);
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            var orderinfo = await dalContentPlatFormOrderDealInfo.GetAll().Where(x => x.ContentPlatFormOrderId != null).Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
                  .Where(o => o.Valid == true)
                .Where(o => o.IsDeal == true && o.CreateDate >= startTime && o.CreateDate < endDate)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .ToListAsync();

            if (isOldSend == true)
            {
                orderinfo = orderinfo.Where(x => x.ContentPlatFormOrder.SendDate.HasValue && x.ContentPlatFormOrder.SendDate.Value.Month != x.CreateDate.Month).ToList();
            }
            if (isOldSend == false)
            {
                orderinfo = orderinfo.Where(x => x.ContentPlatFormOrder.SendDate.HasValue && x.ContentPlatFormOrder.SendDate.Value.Month == x.CreateDate.Month).ToList();
            }

            return orderinfo.GroupBy(x => x.CreateDate.Month).Select(x => new PerformanceBrokenLine { Date = x.Key.ToString(), PerfomancePrice = x.Sum(z => z.Price) }).ToList();
        }


        /// <summary>
        /// 根据主播获取分组独立/协助业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isAssist">是否协助</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <param name="amiyaEmployeeId">主播独立业绩获取时传入主播客服id</param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetIndependentOrAssistPerformanceByYearAndMonth(int year, int month, bool? isAssist, List<int> LiveAnchorIds, int amiyaEmployeeId)
        {
            int consultationType = 0;
            if (isAssist == true)
            {
                consultationType = (int)ContentPlateFormOrderConsultationType.Collaboration;
            }
            else if (isAssist == false)
            {
                consultationType = (int)ContentPlateFormOrderConsultationType.IndependentFollowUp;
            }

            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            //选定的月份
            DateTime currentDate = new DateTime(year, month, 1);
            return await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
                  .Where(o => o.Valid == true)
                .Where(o => o.CreateDate >= currentDate && o.CreateDate < endDate && o.IsDeal == true)
                .Where(o => isAssist == null || o.ContentPlatFormOrder.ConsulationType == consultationType)
                .Where(o => amiyaEmployeeId == 0 || o.CreateBy == amiyaEmployeeId)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .Select(
                  ContentPlatFOrmOrderDealInfo =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           Price = ContentPlatFOrmOrderDealInfo.Price,
                           ConsultationTypeText = ServiceClass.GetContentPlateFormOrderConsultationTypeText(ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType)
                       }
                ).ToListAsync();
        }

        /// <summary>
        /// 新/老客客单价折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldCustomer"></param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        public async Task<List<PerformanceBrokenLine>> GetGuestUnitPricePerformanceBrokenLineAsync(int year, int month, bool? isOldCustomer, List<int> LiveAnchorIds)
        {
            //开始月份
            DateTime startTime = new DateTime(year, 1, 1);
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            var orderinfo = await dalContentPlatFormOrderDealInfo.GetAll()
                  .Where(o => o.Valid == true)
                .Where(o => o.IsDeal == true && o.CreateDate >= startTime && o.CreateDate < endDate)
                .Where(o => isOldCustomer == null || o.IsOldCustomer == isOldCustomer)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id)).ToListAsync();

            var list = orderinfo.Select(x => new PerformanceInfoDateDto { Date = x.CreateDate, PerfomancePrice = x.Price }).ToList();
            var returnResult = list.GroupBy(x => x.Date.Month).Select(x => new PerformanceBrokenLine { Date = x.Key.ToString(), PerfomancePrice = x.Sum(z => z.PerfomancePrice) / x.Count() }).ToList();
            return returnResult;
        }

        /// <summary>
        /// 获取独立/协助业绩折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<PerformanceBrokenLine>> GetIndependenceOrAssistAsync(int year, int month, bool? isAssist, List<int> LiveAnchorIds, int amiyaEmployeeId)
        {
            int consultationType = 0;
            if (isAssist == true)
            {
                consultationType = (int)ContentPlateFormOrderConsultationType.Collaboration;
            }
            else if (isAssist == false)
            {
                consultationType = (int)ContentPlateFormOrderConsultationType.IndependentFollowUp;
            }
            //开始月份
            DateTime startTime = new DateTime(year, 1, 1);
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            var orderinfo = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).Where(o => o.Valid == true)
               .Where(o => o.CreateDate >= startTime && o.CreateDate < endDate && o.IsDeal == true)
                .Where(o => isAssist == null || o.ContentPlatFormOrder.ConsulationType == consultationType)
                .Where(o => amiyaEmployeeId == 0 || o.CreateBy == amiyaEmployeeId)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .ToListAsync();


            return orderinfo.GroupBy(x => x.CreateDate.Month).Select(x => new PerformanceBrokenLine { Date = x.Key.ToString(), PerfomancePrice = x.Sum(z => z.Price) }).ToList();
        }


        #endregion

        #region 【新业绩板块】

        /// <summary>
        /// 根据精确时间线获取主播获取啊美雅业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldCustomer">新客/老客</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceByDateAsync(DateTime startDate, DateTime endDate, List<int> LiveAnchorIds)
        {
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).Where(o => o.Valid == true)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
                {
                    Id = ContentPlatFOrmOrderDealInfo.Id,
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    ConsultationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType,
                    IsAcompanying = ContentPlatFOrmOrderDealInfo.IsAcompanying,
                    AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
                    BaseLiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                    BaseLiveAnchorName = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchor.Name,
                }
                ).ToListAsync();

            foreach (var z in result)
            {
                //判断是否为助理补单的业绩
                if (z.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                {
                    //取上一条的成交信息
                    var lastDealInfo = await this.GetByIdAsync(z.LastDealInfoId);
                    //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                    if (lastDealInfo.CreateDate < startDate)
                    {
                        z.Price -= lastDealInfo.Price;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据精确时间线获取派单成交业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend">历史/当月派单,true为历史派单当月成交，false为当月派单当月成交</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetSendAndDealPerformanceAsync(DateTime startDate, DateTime endDate, bool? isOldSend, List<int> liveAnchorIds)
        {
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor).Where(o => o.Valid == true)
                .Where(o => o.IsDeal == true && o.CreateDate >= startDate && o.CreateDate < endDate)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .ToListAsync();
            if (isOldSend == true)
            {
                result = result.Where(c => c.ContentPlatFormOrder.SendDate.HasValue && c.ContentPlatFormOrder.SendDate < startDate).ToList();
            }
            if (isOldSend == false)
            {
                result = result.Where(x => x.ContentPlatFormOrder.SendDate.HasValue && x.ContentPlatFormOrder.SendDate >= startDate && x.ContentPlatFormOrder.SendDate < endDate).ToList();
            }
            var returnInfo = result.Select(
                  d =>
                       new ContentPlatFormOrderDealInfoDto
                       {
                           Id = d.Id,
                           ContentPlatFormOrderId = d.ContentPlatFormOrderId,
                           CreateDate = d.CreateDate,
                           SendDate = d.ContentPlatFormOrder.SendDate,
                           CustomerNickName = d.ContentPlatFormOrder.CustomerName,
                           Phone = ServiceClass.GetIncompletePhone(d.ContentPlatFormOrder.Phone),
                           IsOldCustomer = d.IsOldCustomer,
                           ToHospitalTypeText = ServiceClass.GerContentPlatFormOrderToHospitalTypeText(d.ToHospitalType),
                           AddOrderPrice = d.ContentPlatFormOrder.AddOrderPrice,
                           Price = d.Price,
                       }
                ).ToList();

            return returnInfo;
        }


        #endregion

        #region 全国机构top10运营数据

        /// <summary>
        /// 总业绩
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenHospitalTotalPerformance()
        {
            var performanceList = (from d in _dalHospitalInfo.GetAll()
                                   from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.IsDeal == true && c.LastDealHospitalId != null && c.Valid == true)
                                   where d.Id == h.LastDealHospitalId
                                   group h by d.Name into g
                                   orderby g.Sum(item => item.Price) descending
                                   select new ContentPlateformOrderDealInfoHospitalPerformanceDto
                                   {
                                       HospitalName = g.Key,
                                       Performance = g.Sum(item => item.Price)
                                   });
            return performanceList.Skip(0).Take(10).ToList();
        }
        /// <summary>
        /// 新客业绩
        /// </summary>
        /// <returns></returns>

        public async Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenNewCustomerPerformance()
        {
            var performanceList = (from d in _dalHospitalInfo.GetAll()
                                   from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.IsDeal == true && c.LastDealHospitalId != null && c.IsOldCustomer == false && c.Valid == true)
                                   where d.Id == h.LastDealHospitalId
                                   group h by d.Name into g
                                   orderby g.Sum(item => item.Price) descending
                                   select new ContentPlateformOrderDealInfoHospitalPerformanceDto
                                   {
                                       HospitalName = g.Key,
                                       Performance = g.Sum(item => item.Price)
                                   });
            return performanceList.Skip(0).Take(10).ToList();
        }
        /// <summary>
        /// 老客业绩
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenOldCustomerPerformance()
        {
            var performanceList = (from d in _dalHospitalInfo.GetAll()
                                   from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.IsDeal == true && c.LastDealHospitalId != null && c.IsOldCustomer == true && c.Valid == true)
                                   where d.Id == h.LastDealHospitalId
                                   group h by d.Name into g
                                   orderby g.Sum(item => item.Price) descending
                                   select new ContentPlateformOrderDealInfoHospitalPerformanceDto
                                   {
                                       HospitalName = g.Key,
                                       Performance = g.Sum(item => item.Price)
                                   });
            return performanceList.Skip(0).Take(10).ToList();
        }
        /// <summary>
        /// 新客上门人数占比
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenNewCustomerToHospitalPformance()
        {
            var performanceList = (from d in _dalHospitalInfo.GetAll()
                                   from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.LastDealHospitalId != null && c.IsOldCustomer == false && c.IsToHospital == true && c.Valid == true)
                                   where d.Id == h.LastDealHospitalId
                                   group h by d.Name into g
                                   orderby g.Count() descending
                                   select new ContentPlateformOrderDealInfoHospitalPerformanceDto
                                   {
                                       HospitalName = g.Key,
                                       Performance = g.Count()
                                   });
            return performanceList.Skip(0).Take(10).ToList();

        }
        /// <summary>
        /// 新客成交人数占比
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenNewCustomerDealPerformance()
        {
            var performanceList = (from d in _dalHospitalInfo.GetAll()
                                   from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.LastDealHospitalId != null && c.IsOldCustomer == false && c.Valid == true)
                                   where d.Id == h.LastDealHospitalId
                                   group h by d.Name into g
                                   orderby g.Count() descending
                                   select new ContentPlateformOrderDealInfoHospitalPerformanceDto
                                   {
                                       HospitalName = g.Key,
                                       Performance = g.Count()
                                   });
            return performanceList.Skip(0).Take(10).ToList();
        }



        #endregion

        #region 全国城市top10运营数据

        /// <summary>
        /// 获取总业绩业绩前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityTotalPerformance()
        {
            var performanceList = from d in _dalHospitalInfo.GetAll()
                                  from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.IsDeal == true && c.LastDealHospitalId != null && c.Valid == true)
                                  where d.Id == h.LastDealHospitalId
                                  group h by d.CooperativeHospitalCity.Name into g
                                  orderby g.Sum(item => item.Price) descending
                                  select new ContentPlateformOrderDealInfoCityPerformanceDto
                                  {
                                      CityName = g.Key,
                                      Performance = g.Sum(item => item.Price)
                                  };
            return await performanceList.Skip(0).Take(10).ToListAsync();
        }
        /// <summary>
        /// 获取新客业绩前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityNewCustomerPerformance()
        {
            var performanceList = from d in _dalHospitalInfo.GetAll()
                                  from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.IsDeal == true && c.LastDealHospitalId != null && c.IsOldCustomer == false && c.Valid == true)
                                  where d.Id == h.LastDealHospitalId
                                  group h by d.CooperativeHospitalCity.Name into g
                                  orderby g.Sum(item => item.Price) descending
                                  select new ContentPlateformOrderDealInfoCityPerformanceDto
                                  {
                                      CityName = g.Key,
                                      Performance = g.Sum(item => item.Price)
                                  };
            return await performanceList.Skip(0).Take(10).ToListAsync();
        }
        /// <summary>
        /// 获取老客业绩前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityOldCustomerPerformance()
        {
            var performanceList = from d in _dalHospitalInfo.GetAll()
                                  from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.IsDeal == true && c.LastDealHospitalId != null && c.IsOldCustomer == true && c.Valid == true)
                                  where d.Id == h.LastDealHospitalId
                                  group h by d.CooperativeHospitalCity.Name into g
                                  orderby g.Sum(item => item.Price) descending
                                  select new ContentPlateformOrderDealInfoCityPerformanceDto
                                  {
                                      CityName = g.Key,
                                      Performance = g.Sum(item => item.Price)
                                  };
            return await performanceList.Skip(0).Take(10).ToListAsync();
        }
        /// <summary>
        /// 获取新客上门前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityNewCustomerToHospitalPformance()
        {
            var performanceList = from d in _dalHospitalInfo.GetAll()
                                  from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.LastDealHospitalId != null && c.IsOldCustomer == false && c.IsToHospital == true && c.Valid == true)
                                  where d.Id == h.LastDealHospitalId
                                  group h by d.CooperativeHospitalCity.Name into g
                                  orderby g.Count() descending
                                  select new ContentPlateformOrderDealInfoCityPerformanceDto
                                  {
                                      CityName = g.Key,
                                      Performance = g.Count()
                                  };
            return await performanceList.Skip(0).Take(10).ToListAsync();
        }
        /// <summary>
        /// 获取新客成交人数前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityNewCustomerDealPerformance()
        {
            var performanceList = from d in _dalHospitalInfo.GetAll()
                                  from h in dalContentPlatFormOrderDealInfo.GetAll().Where(c => c.LastDealHospitalId != null && c.IsOldCustomer == false && c.IsToHospital == true && c.Valid == true)
                                  where d.Id == h.LastDealHospitalId
                                  group h by d.CooperativeHospitalCity.Name into g
                                  orderby g.Count() descending
                                  select new ContentPlateformOrderDealInfoCityPerformanceDto
                                  {
                                      CityName = g.Key,
                                      Performance = g.Count()
                                  };
            return await performanceList.Skip(0).Take(10).ToListAsync();
        }

        public Task<List<ContentPlatFormOrderDealInfoDto>> GetTodaySendPerformanceByHospitalIdListAsync(List<int?> hospitalIds, DateTime recordDate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据到院id和时间获取指定月份成交量
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<int> GetMonthSendPerformanceByHospitalIdAsync(int hospitalId, int year, int month)
        {
            DateTime startDate = Convert.ToDateTime(year + "-" + month + "-01");
            DateTime endDate = startDate.AddMonths(1);
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                .Where(x => x.Valid == true)
                .Where(o => o.ToHospitalDate.HasValue == true && o.ToHospitalDate >= startDate && o.ToHospitalDate < endDate && o.IsDeal == true)
                .Where(o => o.LastDealHospitalId.Value == hospitalId)
                .ToListAsync();
            return result.Count();
        }
        /// <summary>
        /// 获取指定年份成交量
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<int> GetYearSendPerformanceByHospitalIdAsync(int hospitalId, int year)
        {
            DateTime startDate = Convert.ToDateTime(year + "-01-01");
            DateTime endDate = startDate.AddYears(1);
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                .Where(x => x.Valid == true)
                .Where(o => o.ToHospitalDate.HasValue == true && o.ToHospitalDate >= startDate && o.ToHospitalDate < endDate)
                .Where(o => o.LastDealHospitalId.Value == hospitalId)
                .ToListAsync();
            return result.Count();
        }
        /// <summary>
        /// 获取总成交量
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<int> GetSendPerformanceByHospitalIdAsync(int hospitalId)
        {
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder)
                .Where(x => x.Valid == true)
                .Where(o => o.LastDealHospitalId.Value == hospitalId)
                .ToListAsync();
            return result.Count();
        }



        #endregion

        #region 【对账板块】
        /// <summary>
        /// 获取时间段内未对账机构列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<List<UnCheckHospitalOrderDto>> GetUnCheckHospitalOrderAsync(DateTime? startDate, DateTime? endDate, int? hospitalId)
        {
            DateTime startrq = new DateTime();
            DateTime endrq = new DateTime();
            if (startDate.HasValue)
            {
                startrq = ((DateTime)startDate);
            }
            if (endDate.HasValue)
            {
                endrq = ((DateTime)endDate).Date.AddDays(1);
            }
            var orders = from d in dalContentPlatFormOrderDealInfo.GetAll()
                         where (!startDate.HasValue || d.CreateDate >= startrq)
                         && d.Valid == true
                         && (!endDate.HasValue || d.CreateDate <= endrq)
                         && (d.CheckState == (int)CheckType.NotChecked)
                         && (d.IsDeal == true)
                         && (!hospitalId.HasValue || d.LastDealHospitalId == hospitalId)
                         && (d.LastDealHospitalId.HasValue == true && d.LastDealHospitalId != 0)
                         && (d.Price > 0)
                         && (!hospitalId.HasValue || d.LastDealHospitalId.Value == hospitalId)
                         select d;
            var orderList = await orders.ToListAsync();
            return orderList.GroupBy(x => x.LastDealHospitalId.Value).Select(x => new UnCheckHospitalOrderDto { HospitalId = x.Key, TotalUnCheckPrice = x.Sum(z => z.Price), TotalUnCheckOrderCount = x.Count() }).ToList();
        }


        /// <summary>
        /// 根据成交编号获取成交登记日期
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        public async Task<DateTime> GetDealCraeteDateByDealIdAsync(string dealId)
        {
            var result = await dalContentPlatFormOrderDealInfo.GetAll().Where(x => x.Id == dealId && x.Valid == true).FirstOrDefaultAsync();
            DateTime resultDate = result.CreateDate;
            if (result.Price < 0)
            {
                //寻找上一条成交的成交时间
                var dealInfo = await this.GetByOrderIdAsync(result.ContentPlatFormOrderId);
                var dealResult = dealInfo.Where(x => x.CreateDate < resultDate && x.Price > 0).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                resultDate = dealResult.CreateDate;
            }
            return resultDate;
        }


        /// <summary>
        /// 获取截止时间的助理总业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<decimal> GetTotalPerformanceByEmployeeAndDateAsync(DateTime startDate, DateTime endDate, int employeeId)
        {
            var query = await dalContentPlatFormOrderDealInfo.GetAll()
             .Include(e => e.ContentPlatFormOrder)
             .Where(e => e.CreateDate >= startDate && e.CreateDate < endDate)
             .Where(x => x.Valid == true)
             .Where(e => e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId == employeeId : e.ContentPlatFormOrder.BelongEmpId == employeeId)
             .Where(e => e.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.AssistantCheck && e.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.FinanceCheck && e.Price > 0).ToListAsync();

            var result = query.Sum(x => x.Price);
            return result;
        }


        #endregion


        #region 财务看板板块


        /// <summary>
        /// 根据客服id获取财务看板客服录入成交单业绩信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerServiceId"></param>
        /// <returns></returns>
        public async Task<CustomerServiceBoardDataDto> GetCustomerServiceBoardDataByCustomerServiceIdAsync(DateTime? startDate, DateTime? endDate, int customerServiceId)
        {
            startDate = startDate == null ? DateTime.Now.Date : startDate.Value.Date;
            endDate = endDate == null ? DateTime.Now.AddDays(1).Date : endDate.Value.AddDays(1).Date;
            var dealData = dalContentPlatFormOrderDealInfo.GetAll()
             .Where(x => x.Valid == true)
                .Where(e => e.CheckDate >= startDate && e.CheckDate < endDate && e.CreateBy == customerServiceId && e.CheckState == 2)
                .GroupBy(e => e.CreateBy)
                .Select(e => new CustomerServiceBoardDataDto
                {
                    CustomerServiceName = Convert.ToString(e.Key),
                    DealPrice = e.Sum(item => item.CheckPrice) ?? 0m,
                    TotalServicePrice = e.Sum(item => item.SettlePrice) ?? 0m,
                    NewCustomerPrice = e.Sum(item => item.IsOldCustomer == false ? item.CheckPrice ?? 0m : 0m),
                    NewCustomerServicePrice = e.Sum(item => item.IsOldCustomer == false ? item.SettlePrice ?? 0m : 0m),
                    OldCustomerPrice = e.Sum(item => item.IsOldCustomer == true ? item.CheckPrice ?? 0m : 0m),
                    OldCustomerServicePrice = e.Sum(item => item.IsOldCustomer == true ? item.SettlePrice ?? 0m : 0m)
                }).FirstOrDefault();
            if (dealData != null)
                dealData.CustomerServiceName = await _dalAmiyaEmployee.GetAll().Where(e => e.Id == Convert.ToInt32(customerServiceId)).Select(e => e.Name).FirstOrDefaultAsync();
            return dealData;
        }


        /// <summary>
        /// 根据客服id获取财务看板归属客服业绩信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerServiceId"></param>
        /// <returns></returns>
        public async Task<List<CustomerServiceBoardDataDto>> GetCustomerServiceBelongBoardDataByCustomerServiceIdAsync(DateTime? startDate, DateTime? endDate, int? customerServiceId)
        {
            startDate = startDate == null ? DateTime.Now.Date : startDate.Value.Date;
            endDate = endDate == null ? DateTime.Now.AddDays(1).Date : endDate.Value.AddDays(1).Date;
            var dealData = await dalContentPlatFormOrderDealInfo.GetAll()
             .Where(x => x.Valid == true)
                .Where(e => e.CheckDate >= startDate && e.CheckDate < endDate && e.CheckState == 2)
                .Where(e => !customerServiceId.HasValue || e.ContentPlatFormOrder.BelongEmpId == customerServiceId.Value)
                .GroupBy(e => e.ContentPlatFormOrder.BelongEmpId)
                .Select(e => new CustomerServiceBoardDataDto
                {
                    CustomerServiceName = Convert.ToString(e.Key),
                    DealPrice = e.Sum(item => item.CheckPrice) ?? 0m,
                    TotalServicePrice = e.Sum(item => item.SettlePrice) ?? 0m,
                    NewCustomerPrice = e.Sum(item => item.IsOldCustomer == false ? item.CheckPrice ?? 0m : 0m),
                    NewCustomerServicePrice = e.Sum(item => item.IsOldCustomer == false ? item.SettlePrice ?? 0m : 0m),
                    OldCustomerPrice = e.Sum(item => item.IsOldCustomer == true ? item.CheckPrice ?? 0m : 0m),
                    OldCustomerServicePrice = e.Sum(item => item.IsOldCustomer == true ? item.SettlePrice ?? 0m : 0m)
                }).ToListAsync();


            //if(dealData!=null)
            //    dealData.CustomerServiceName = await _dalAmiyaEmployee.GetAll().Where(e => e.Id == Convert.ToInt32(customerServiceId)).Select(e => e.Name).FirstOrDefaultAsync();
            return dealData;
        }


        public async Task<List<LiveAnchorBoardDataDto>> GetLiveAnchorPriceByLiveAnchorIdAsync(DateTime? startDate, DateTime? endDate, List<int> liveAnchorIds)
        {
            startDate = startDate == null ? DateTime.Now.Date : startDate.Value.Date;
            endDate = endDate == null ? DateTime.Now.AddDays(1).Date : endDate.Value.AddDays(1).Date;
            var dataList = dalContentPlatFormOrderDealInfo.GetAll().Include(e => e.ContentPlatFormOrder)
             .Where(x => x.Valid == true).Where(e => e.CheckDate >= startDate && e.CheckDate < endDate && e.CheckState == 2)
                .Where(e => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(e.ContentPlatFormOrder.LiveAnchorId.HasValue ? e.ContentPlatFormOrder.LiveAnchorId.Value : 0))
                .GroupBy(e => new { e.ContentPlatFormOrder.LiveAnchorId, e.BelongCompany })
                .Select(e => new LiveAnchorBoardDataDto
                {
                    CompanyName = e.Key.BelongCompany,
                    LiveAnchorName = e.Key.LiveAnchorId.ToString(),
                    DealPrice = e.Sum(item => item.CheckPrice) ?? 0m,
                    TotalServicePrice = e.Sum(item => item.SettlePrice) ?? 0m,
                    NewCustomerPrice = e.Sum(item => item.IsOldCustomer == false ? item.CheckPrice : 0) ?? 0m,
                    OldCustomerPrice = e.Sum(item => item.IsOldCustomer == true ? item.CheckPrice : 0) ?? 0m,
                    NewCustomerServicePrice = e.Sum(item => item.IsOldCustomer == false ? item.SettlePrice : 0) ?? 0m,
                    OldCustomerServicePrice = e.Sum(item => item.IsOldCustomer == true ? item.SettlePrice : 0) ?? 0m,
                }).ToList();
            foreach (var item in dataList)
            {
                item.LiveAnchorName = dalLiveAnchor.GetAll().Where(e => e.Id == Convert.ToInt32(item.LiveAnchorName)).Select(e => e.Name).SingleOrDefault() ?? "未知(订单没有主播归属信息)";
                item.CompanyName = dalCompanyBaseInfo.GetAll().Where(e => e.Id == item.CompanyName).Select(e => e.Name).SingleOrDefault() ?? "未知(已对账未开票)";
            }
            return dataList;

        }
        /// <summary>
        /// 获取医院对账业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<List<FinancialHospitalDealPriceBoardDto>> GetHospitalDealPriceDataAsync(DateTime? startDate, DateTime? endDate, int? hospitalId, int pageNum, int pageSize)
        {
            startDate = startDate.HasValue ? startDate.Value.Date : DateTime.Now.Date;
            endDate = endDate.HasValue ? endDate.Value.AddDays(1).Date : DateTime.Now.Date.AddDays(1).Date;
            var dealInfo = dalContentPlatFormOrderDealInfo.GetAll()
             .Where(x => x.Valid == true).Where(e => e.CheckDate >= startDate && e.CheckDate < endDate && e.CheckState == (int)CheckType.CheckedSuccess);
            if (hospitalId.HasValue && hospitalId != -1)
            {
                dealInfo = dealInfo.Where(e => e.LastDealHospitalId == hospitalId);
            }
            var dealInfoResult = dealInfo.GroupBy(e => e.LastDealHospitalId).Select(e => new FinancialHospitalDealPriceBoardDto
            {
                HospitalName = _dalHospitalInfo.GetAll().Where(h => h.Id == e.Key).FirstOrDefault() == null ? "未知(订单未归属医院)" : _dalHospitalInfo.GetAll().Where(h => h.Id == e.Key).FirstOrDefault().Name,
                DealPrice = e.Sum(item => item.CheckPrice ?? 0m),
                TotalServicePrice = e.Sum(item => item.SettlePrice ?? 0m),
                InformationPrice = e.Sum(item => item.InformationPrice ?? 0m),
                SystemUsePrice = e.Sum(item => item.SystemUpdatePrice ?? 0m),
                ReturnBackPrice = e.Sum(item => item.ReturnBackPrice ?? 0m)
            }).OrderByDescending(e => e.DealPrice);
            return dealInfoResult.ToList();

        }

        #endregion

        #region 系统端新业绩看板

        public async Task<List<PerformanceDto>> GetPerformanceByDateAndLiveAnchorIdsAsync(DateTime startDate, DateTime endDate, List<int> LiveAnchorIds)
        {

            var result1 = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null && o.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .Select(ContentPlatFOrmOrderDealInfo => new PerformanceDto
                {
                    Id = ContentPlatFOrmOrderDealInfo.Id,
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    LiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId,
                    ToHospitalType = ContentPlatFOrmOrderDealInfo.ToHospitalType,
                    CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    BelongChannel = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.BelongChannel,
                    SendDate = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SendDate.Value,
                    AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
                    ContentPlatFormId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ContentPlateformId,
                    ConsulationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType,
                }).ToListAsync();
            var result2 = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null && o.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .Select(ContentPlatFOrmOrderDealInfo => new PerformanceDto
                {
                    Id = ContentPlatFOrmOrderDealInfo.Id,
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    LiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId,
                    ToHospitalType = ContentPlatFOrmOrderDealInfo.ToHospitalType,
                    CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    BelongChannel = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.BelongChannel,
                    SendDate = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SendDate.Value,
                    AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
                    ContentPlatFormId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ContentPlateformId,
                    ConsulationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType,
                    LastDealInfoId = ContentPlatFOrmOrderDealInfo.LastDealInfoId,
                }).ToListAsync();
            foreach (var x in result2)
            {
                //取上一条的成交信息
                var lastDealInfo = await this.GetByIdAsync(x.LastDealInfoId);
                //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                if (lastDealInfo.CreateDate < startDate)
                {
                    x.Price -= lastDealInfo.Price;
                }

            }
            result1.AddRange(result2);
            return result1;
        }
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceDetailByDateAsync(DateTime startDate, DateTime endDate, List<int> LiveAnchorIds)
        {
            return dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null)
                .Where(o => LiveAnchorIds.Count == 0 || LiveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
                {
                    Id = ContentPlatFOrmOrderDealInfo.Id,
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    ConsultationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType,
                    IsAcompanying = ContentPlatFOrmOrderDealInfo.IsAcompanying,
                    AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
                    ContentPlatFormId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ContentPlateformId,
                    SendDate = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SendDate,
                    CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate,
                    LiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchor.Id,
                    BelongLiveAnchor = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId,
                    LastDealHospitalId = ContentPlatFOrmOrderDealInfo.LastDealHospitalId,
                    DealDate = ContentPlatFOrmOrderDealInfo.DealDate,
                    BelongEmployeeId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.BelongEmpId.Value,
                    IsSupportOrder = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.IsSupportOrder,
                    SupportEmpId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SupportEmpId,
                }
                ).ToList();
        }
        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceDetailByDateAndAssistantIdListAsync(DateTime startDate, DateTime endDate, List<int> assistantId)
        {
            //归属客服业绩
            var belongData = dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true && x.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null)
                .Where(o => assistantId.Count == 0 || (o.ContentPlatFormOrder.IsSupportOrder == false && assistantId.Contains(o.ContentPlatFormOrder.BelongEmpId.Value)))
                .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
                {
                    ContentPlatFormOrderId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId,
                    Phone = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.Phone,
                    BelongEmployeeId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.BelongEmpId.Value,
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
                    LastDealHospitalId = ContentPlatFOrmOrderDealInfo.LastDealHospitalId,
                    SendDate = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SendDate,
                    ConsultationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType
                }
                ).ToList();
            var supportData = dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true && x.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
            .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null)
            .Where(o => assistantId.Count == 0 || (o.ContentPlatFormOrder.IsSupportOrder == true && assistantId.Contains(o.ContentPlatFormOrder.SupportEmpId)))
            .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
            {
                ContentPlatFormOrderId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId,
                Phone = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.Phone,
                BelongEmployeeId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SupportEmpId,
                Price = ContentPlatFOrmOrderDealInfo.Price,
                IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
                LastDealHospitalId = ContentPlatFOrmOrderDealInfo.LastDealHospitalId,
                SendDate = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SendDate,
                ConsultationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType
            }
            ).ToList();
            belongData.AddRange(supportData);
            var replenishmentData = dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
           .Where(x => x.Valid == true && x.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
          .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null)
          .Where(o => assistantId.Count == 0 || assistantId.Contains(o.CreateBy))
          .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
          {
              ContentPlatFormOrderId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrderId,
              Phone = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.Phone,
              BelongEmployeeId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SupportEmpId,
              Price = ContentPlatFOrmOrderDealInfo.Price,
              IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
              AddOrderPrice = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.AddOrderPrice,
              LastDealHospitalId = ContentPlatFOrmOrderDealInfo.LastDealHospitalId,
              SendDate = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.SendDate,
              ConsultationType = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.ConsulationType,
              LastDealInfoId = ContentPlatFOrmOrderDealInfo.LastDealInfoId,
          }
          ).ToList();
            foreach (var x in replenishmentData)
            {
                //取上一条的成交信息
                var lastDealInfo = await this.GetByIdAsync(x.LastDealInfoId);
                //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                if (lastDealInfo.CreateDate < startDate)
                {
                    x.Price -= lastDealInfo.Price;
                }

            }
            belongData.AddRange(replenishmentData);
            return belongData;
        }
        #endregion

        #region 助理首页
        /// <summary>
        /// 获取今日到院数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<FxPageInfo<TodayToHospitalDataDto>> GetTodayToHospitalDataAsync(QueryAssistantHomePageDataDto query)
        {
            FxPageInfo<TodayToHospitalDataDto> todayToHospitalData = new FxPageInfo<TodayToHospitalDataDto>();
            if (!query.Date.HasValue)
            {
                query.Date = DateTime.Now;
            }
            var startDate = query.Date.Value.Date;
            var endDate = query.Date.Value.Date.AddDays(1).Date;
            var contentPlatformOrderDeal = dalContentPlatFormOrderDealInfo.GetAll().Include(e => e.ContentPlatFormOrder)
             .Where(x => x.Valid == true).Where(e => e.ToHospitalDate >= startDate && e.ToHospitalDate < endDate);
            if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                var ids = (await liveAnchorService.GetLiveAnchorListByBaseInfoId(query.BaseLiveAnchorId)).Select(e => e.Id);
                contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => ids.Contains(e.ContentPlatFormOrder.LiveAnchorId.Value));
            }
            if (!string.IsNullOrEmpty(query.ContentPlatformId))
            {
                contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => e.ContentPlatFormOrder.ContentPlateformId == query.ContentPlatformId);
            }
            if (query.LiveAnchorId.HasValue)
            {
                contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => e.ContentPlatFormOrder.LiveAnchorId == query.LiveAnchorId);
            }
            if (!string.IsNullOrEmpty(query.WechatNoId))
            {
                contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => e.ContentPlatFormOrder.LiveAnchorWeChatNo == query.WechatNoId);
            }
            if (query.Source.HasValue)
            {
                contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => e.ContentPlatFormOrder.OrderSource == query.Source);
            }
            if (query.AssistantId.HasValue)
            {
                contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => e.ContentPlatFormOrder.BelongEmpId == query.AssistantId.Value);
            }
            todayToHospitalData.TotalCount = await contentPlatformOrderDeal.CountAsync();
            var config = await GetCallCenterConfig();
            todayToHospitalData.List = (from c in contentPlatformOrderDeal
                                        join h in _dalHospitalInfo.GetAll() on c.LastDealHospitalId equals h.Id
                                        join e in _dalAmiyaEmployee.GetAll() on c.ContentPlatFormOrder.BelongEmpId equals e.Id
                                        orderby c.ToHospitalDate descending
                                        select new TodayToHospitalDataDto
                                        {
                                            Name = c.ContentPlatFormOrder.CustomerName,
                                            Phone = config.HidePhoneNumber == true ? ServiceClass.GetIncompletePhone(c.ContentPlatFormOrder.Phone) : c.ContentPlatFormOrder.Phone,
                                            EncryptPhone = ServiceClass.Encrypt(c.ContentPlatFormOrder.Phone, config.PhoneEncryptKey),
                                            AssistantName = e.Name,
                                            SendHospital = h.Name,
                                            Status = c.IsDeal ? "已成交" : "未成交",
                                            AddOrderPrice = c.ContentPlatFormOrder.AddOrderPrice,
                                            DealPrice = c.Price,
                                            IsOldCustomer = c.IsOldCustomer ? "老客" : "新客"

                                        }).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToList();
            return todayToHospitalData;
        }

        /// <summary>
        /// 获取助理首页月业绩完成情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<MonthPerformanceCompleteSituationDataDto> GetAssistantMonthPerformanceDataAsync(QueryAssistantHomePageDataDto query)
        {
            MonthPerformanceCompleteSituationDataDto monthData = new MonthPerformanceCompleteSituationDataDto();
            var performance = dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true)
                .Where(o => o.CreateDate >= query.StartDate.Value && o.CreateDate < query.EndDate.Value && o.IsDeal == true && o.ContentPlatFormOrderId != null);
            if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                var ids = (await liveAnchorService.GetLiveAnchorListByBaseInfoId(query.BaseLiveAnchorId)).Select(e => e.Id);
                performance = performance.Where(e => ids.Contains(e.ContentPlatFormOrder.LiveAnchorId.Value));
            }
            if (!string.IsNullOrEmpty(query.ContentPlatformId))
            {
                performance = performance.Where(e => e.ContentPlatFormOrder.ContentPlateformId == query.ContentPlatformId);
            }
            if (query.LiveAnchorId.HasValue)
            {
                performance = performance.Where(e => e.ContentPlatFormOrder.LiveAnchorId == query.LiveAnchorId);
            }
            if (!string.IsNullOrEmpty(query.WechatNoId))
            {
                performance = performance.Where(e => e.ContentPlatFormOrder.LiveAnchorWeChatNo == query.WechatNoId);
            }
            if (query.Source.HasValue)
            {
                performance = performance.Where(e => e.ContentPlatFormOrder.OrderSource == query.Source);
            }
            if (query.AssistantId.HasValue)
            {
                //需优化获取辅助客服
                performance = performance.Where(e => e.ContentPlatFormOrder.BelongEmpId == query.AssistantId.Value || e.ContentPlatFormOrder.SupportEmpId == query.AssistantId.Value)
                    .Where(d => d.ContentPlatFormOrder.IsSupportOrder == false || d.ContentPlatFormOrder.SupportEmpId == query.AssistantId.Value);
            }
            monthData.CompletedPerformance = performance.Sum(e => e.Price);
            monthData.NewCustomerPerformance = performance.Where(e => e.IsOldCustomer == false).Sum(e => e.Price);
            monthData.OldCustomerPerformance = performance.Where(e => e.IsOldCustomer == true).Sum(e => e.Price);
            return monthData;
        }

        private async Task<CallCenterConfigDto> GetCallCenterConfig()
        {
            var config = await dalConfig.GetAll().SingleOrDefaultAsync();
            return JsonConvert.DeserializeObject<WxAppConfigDto>(config.ConfigJson).CallCenterConfig;
        }

        #endregion

        #region【系统端运营看板】

        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetSimplePerformanceDetailByDateAsync(int year, List<int> liveAnchorIds, bool? isOldCustomer)
        {
            DateTime startDate = Convert.ToDateTime(year + "-01-01 00:00:00");
            DateTime endDate = Convert.ToDateTime(year + "-12-31 23:59:59");


            //正常成交单
            var result1 = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null && o.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                 .Where(o => !isOldCustomer.HasValue || o.IsOldCustomer == isOldCustomer.Value)
                .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
                {
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    LastDealInfoCreateDate = ContentPlatFOrmOrderDealInfo.LastDealInfoCreateDate,
                    ReplenishmentCreateDate = ContentPlatFOrmOrderDealInfo.ReplenishmentCreateDate,
                    CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    LiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId.HasValue ? ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId.Value : 0

                }).ToListAsync();
            //补单
            var result2 = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == true)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null && o.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                 .Where(o => !isOldCustomer.HasValue || o.IsOldCustomer == isOldCustomer.Value)
                .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
                {
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    LastDealInfoId = ContentPlatFOrmOrderDealInfo.LastDealInfoId,
                    LastDealInfoCreateDate = ContentPlatFOrmOrderDealInfo.LastDealInfoCreateDate,
                    ReplenishmentCreateDate = ContentPlatFOrmOrderDealInfo.ReplenishmentCreateDate,
                    CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    LiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId.HasValue ? ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId.Value : 0
                }).ToListAsync();
            foreach (var x in result2)
            {
                //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                if (x.LastDealInfoCreateDate < startDate)
                {
                    //取上一条的成交信息
                    var lastDealInfo = await this.GetByIdAsync(x.LastDealInfoId);
                    x.Price -= lastDealInfo.Price;
                }
            }

            result1.AddRange(result2);

            return result1;
        }

        public async Task<List<ContentPlatFormOrderDealInfoDto>> GetLossOrderDealInfoDataByMonthAndLiveAnchorAsync(int year, int month, List<int> liveAnchorIds, bool? isOldCustomer)
        {

            DateTime startDate = Convert.ToDateTime(year + "-" + month + "-01 00:00:00");
            DateTime endDate = DateTime.Now;
            if (month == 12)
            {
                year += 1;
                endDate = Convert.ToDateTime(year + "-01-01 00:00:00");
            }
            else
            {
                month += 1;
                endDate = Convert.ToDateTime(year + "-" + month + "-01 00:00:00");
            }

            //漏单
            var result3 = await dalContentPlatFormOrderDealInfo.GetAll().Include(x => x.ContentPlatFormOrder).ThenInclude(x => x.LiveAnchor)
             .Where(x => x.Valid == false)
                .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate && o.IsDeal == true && o.ContentPlatFormOrderId != null && o.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.CustomerServiceReplenishmentOrder)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.ContentPlatFormOrder.LiveAnchor.Id))
                 .Where(o => !isOldCustomer.HasValue || o.IsOldCustomer == isOldCustomer.Value)
                .Select(ContentPlatFOrmOrderDealInfo => new ContentPlatFormOrderDealInfoDto
                {
                    Price = ContentPlatFOrmOrderDealInfo.Price,
                    LastDealInfoId = ContentPlatFOrmOrderDealInfo.LastDealInfoId,
                    LastDealInfoCreateDate = ContentPlatFOrmOrderDealInfo.LastDealInfoCreateDate,
                    ReplenishmentCreateDate = ContentPlatFOrmOrderDealInfo.ReplenishmentCreateDate,
                    CreateDate = ContentPlatFOrmOrderDealInfo.CreateDate,
                    IsOldCustomer = ContentPlatFOrmOrderDealInfo.IsOldCustomer,
                    LiveAnchorId = ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId.HasValue ? ContentPlatFOrmOrderDealInfo.ContentPlatFormOrder.LiveAnchorId.Value : 0
                }).ToListAsync();
            List<ContentPlatFormOrderDealInfoDto> ContentPlatFormOrderDealInfoDtoResults = new List<ContentPlatFormOrderDealInfoDto>();
            foreach (var x in result3)
            {
                //用上一条的成交信息的“创建时间”与当前查询开始时间进行比对，若小于当前查询开始时间，成交金额=当前成交金额-上一条成交单的成交金额，若大于等于则跳过
                if (x.ReplenishmentCreateDate > endDate)
                {
                    ContentPlatFormOrderDealInfoDtoResults.Add(x);
                }
            }
            return ContentPlatFormOrderDealInfoDtoResults;
        }



        #endregion

        #region 【枚举下拉框】

        public List<BaseIdAndNameDto> GetOrderDealPerformanceTypeList()
        {
            var orderTypes = Enum.GetValues(typeof(ContentPlateFormOrderDealPerformanceType));
            List<BaseIdAndNameDto> orderTypeList = new List<BaseIdAndNameDto>();
            foreach (var item in orderTypes)
            {
                BaseIdAndNameDto orderType = new BaseIdAndNameDto();
                orderType.Id = Convert.ToInt32(item).ToString();
                orderType.Name = ServiceClass.GetContentPlateFormOrderDealPerformanceType(Convert.ToInt32(item));
                orderTypeList.Add(orderType);
            }
            return orderTypeList;
        }









        #endregion


    }
}
