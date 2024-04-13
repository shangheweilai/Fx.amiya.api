﻿using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto.ShoppingCartRegistration;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fx.Infrastructure.DataAccess;
using jos_sdk_net.Util;
using Fx.Amiya.Dto.Performance;
using Fx.Amiya.Dto.MessageNotice.Input;
using Fx.Amiya.Dto.WxAppConfig;
using Newtonsoft.Json;
using Fx.Amiya.Dto;
using System.Threading;
using Fx.Amiya.Dto.AssistantHomePage.Input;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;

namespace Fx.Amiya.Service
{
    public class ShoppingCartRegistrationService : IShoppingCartRegistrationService
    {
        private IDalShoppingCartRegistration dalShoppingCartRegistration;
        private IContentPlatformService _contentPlatformService;
        private ILiveAnchorWeChatInfoService _liveAnchorWeChatInfoService;
        private IMessageNoticeService messageNoticeService;
        private ILiveAnchorService _liveAnchorService;
        private IUnitOfWork unitOfWork;
        private IDalConfig dalConfig;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private IDalAmiyaEmployee dalAmiyaEmployee;
        private IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo;
        private IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo;
        private IDalContentPlatformOrder dalContentPlatformOrder;
        public ShoppingCartRegistrationService(IDalShoppingCartRegistration dalShoppingCartRegistration,
            IContentPlatformService contentPlatformService,
             IMessageNoticeService messageNoticeService,
            IAmiyaEmployeeService amiyaEmployeeService,
            IUnitOfWork unitOfWork,
            ILiveAnchorService liveAnchorService,
             ILiveAnchorWeChatInfoService liveAnchorWeChatInfoService,
            IDalConfig dalConfig,
            IDalAmiyaEmployee dalAmiyaEmployee, IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo, IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo, IDalContentPlatformOrder dalContentPlatformOrder)
        {
            this.dalShoppingCartRegistration = dalShoppingCartRegistration;
            _contentPlatformService = contentPlatformService;
            _liveAnchorService = liveAnchorService;
            this.messageNoticeService = messageNoticeService;
            this.dalConfig = dalConfig;
            this.unitOfWork = unitOfWork;
            _liveAnchorWeChatInfoService = liveAnchorWeChatInfoService;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.dalAmiyaEmployee = dalAmiyaEmployee;
            this.dalLiveAnchorBaseInfo = dalLiveAnchorBaseInfo;
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            this.dalContentPlatformOrder = dalContentPlatformOrder;
        }



        public async Task<FxPageInfo<ShoppingCartRegistrationDto>> GetListWithPageAsync(DateTime? startDate, DateTime? endDate, int? LiveAnchorId, bool? isCreateOrder, int? createBy, bool? isSendOrder, int? employeeId, bool? isAddWechat, bool? isWriteOff, bool? isConsultation, bool? isReturnBackPrice, string keyword, string contentPlatFormId, int pageNum, int pageSize, decimal? minPrice, decimal? maxPrice, int? assignEmpId, DateTime? startRefundTime, DateTime? endRefundTime, DateTime? startBadReviewTime, DateTime? endBadReviewTime, int? ShoppingCartRegistrationCustomerType, int? emergencyLevel, bool? isBadReview, string baseLiveAnchorId, int? source)
        {
            try
            {
                var config = await GetCallCenterConfig();
                var shoppingCartRegistration = from d in dalShoppingCartRegistration.GetAll()
                                               where (keyword == null || d.Phone.Contains(keyword) || d.SubPhone.Contains(keyword) || d.CustomerNickName.Contains(keyword) || d.LiveAnchorWechatNo.Contains(keyword) || d.Remark.Contains(keyword))
                                               && ((!startDate.HasValue && !endDate.HasValue) || d.RecordDate >= startDate.Value.Date && d.RecordDate < endDate.Value.AddDays(1).Date)
                                               && (string.IsNullOrEmpty(contentPlatFormId) || d.ContentPlatFormId == contentPlatFormId)
                                               && (!createBy.HasValue || d.CreateBy == createBy)
                                               && (!isAddWechat.HasValue || d.IsAddWeChat == isAddWechat)
                                               && (!isWriteOff.HasValue || d.IsWriteOff == isWriteOff)
                                               && (!isSendOrder.HasValue || d.IsSendOrder == isSendOrder)
                                               && (!isCreateOrder.HasValue || d.IsCreateOrder == isCreateOrder)
                                               && (!isConsultation.HasValue || d.IsConsultation == isConsultation)
                                               && (!isReturnBackPrice.HasValue || d.IsReturnBackPrice == isReturnBackPrice)
                                               && (assignEmpId.HasValue || d.AssignEmpId == null)
                                               && (assignEmpId == 0 || d.AssignEmpId == assignEmpId)
                                               && (!minPrice.HasValue || d.Price >= minPrice)
                                               && (!maxPrice.HasValue || d.Price <= maxPrice)
                                               && (!LiveAnchorId.HasValue || d.LiveAnchorId == LiveAnchorId)
                                               && (!startRefundTime.HasValue || d.RefundDate >= startRefundTime.Value.Date)
                                               && (!endRefundTime.HasValue || d.RefundDate <= endRefundTime.Value.AddDays(1).Date)
                                               && (!startBadReviewTime.HasValue || d.BadReviewDate >= startBadReviewTime.Value.Date)
                                               && (!endBadReviewTime.HasValue || d.BadReviewDate <= endBadReviewTime.Value.AddDays(1).Date)
                                               && (!emergencyLevel.HasValue || d.EmergencyLevel == emergencyLevel)
                                               && (!ShoppingCartRegistrationCustomerType.HasValue || d.ShoppingCartRegistrationCustomerType == ShoppingCartRegistrationCustomerType)
                                               && (!isBadReview.HasValue || d.IsBadReview == isBadReview)
                                               && (string.IsNullOrEmpty(baseLiveAnchorId) || d.BaseLiveAnchorId == baseLiveAnchorId)
                                               && (!source.HasValue || d.Source == source.Value)
                                               select new ShoppingCartRegistrationDto
                                               {
                                                   Id = d.Id,
                                                   RecordDate = d.RecordDate,
                                                   ContentPlatFormId = d.ContentPlatFormId,
                                                   LiveAnchorId = d.LiveAnchorId,
                                                   LiveAnchorWechatNo = d.LiveAnchorWechatNo,
                                                   CustomerNickName = d.CustomerNickName,
                                                   Phone = d.Phone,
                                                   HiddenPhone = ServiceClass.GetIncompletePhone(d.Phone),
                                                   EncryptPhone = ServiceClass.Encrypt(d.Phone, config.PhoneEncryptKey),
                                                   SubPhone = d.SubPhone,
                                                   HiddenSubPhone = string.IsNullOrEmpty(d.SubPhone) ? "" : ServiceClass.GetIncompletePhone(d.SubPhone),
                                                   EncryptSubPhone = string.IsNullOrEmpty(d.SubPhone) ? "" : ServiceClass.Encrypt(d.SubPhone, config.PhoneEncryptKey),
                                                   Price = d.Price,
                                                   ConsultationType = d.ConsultationType,
                                                   ConsultationTypeText = ServiceClass.GetConsulationTypeText(d.ConsultationType),
                                                   ShoppingCartRegistrationCustomerType = d.ShoppingCartRegistrationCustomerType,
                                                   ShoppingCartRegistrationCustomerTypeText = ServiceClass.GetShoppingCartCustomerTypeText(d.ShoppingCartRegistrationCustomerType),
                                                   IsWriteOff = d.IsWriteOff,
                                                   IsCreateOrder = d.IsCreateOrder,
                                                   IsSendOrder = d.IsSendOrder,
                                                   IsConsultation = d.IsConsultation,
                                                   ConsultationDate = d.ConsultationDate,
                                                   IsAddWeChat = d.IsAddWeChat,
                                                   IsReturnBackPrice = d.IsReturnBackPrice,
                                                   Remark = d.Remark,
                                                   CreateBy = d.CreateBy,
                                                   AssignEmpId = d.AssignEmpId,
                                                   CreateDate = d.CreateDate,
                                                   IsReContent = d.IsReContent,
                                                   ReContent = d.ReContent,
                                                   RefundDate = d.RefundDate,
                                                   RefundReason = d.RefundReason,
                                                   BadReviewDate = d.BadReviewDate,
                                                   BadReviewContent = d.BadReviewContent,
                                                   BadReviewReason = d.BadReviewReason,
                                                   IsBadReview = d.IsBadReview,
                                                   EmergencyLevel = d.EmergencyLevel,
                                                   Source = d.Source,
                                                   SourceText = ServiceClass.GetTiktokCustomerSourceText(d.Source),
                                                   ProductType = d.ProductType,
                                                   ProductTypeText = ServiceClass.GetShoppingCartTakeGoodsProductTypeText(d.ProductType),
                                                   BaseLiveAnchorId = d.BaseLiveAnchorId,
                                                   GetCustomerType = d.GetCustomerType,
                                                   GetCustomerTypeText = ServiceClass.GetShoppingCartGetCustomerTypeText(d.GetCustomerType),
                                               };
                var employee = await dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == employeeId);
                if (!employee.AmiyaPositionInfo.IsDirector)
                {
                    shoppingCartRegistration = from d in shoppingCartRegistration
                                               where d.CreateBy == employeeId || d.AssignEmpId == employeeId
                                               select d;
                }
                FxPageInfo<ShoppingCartRegistrationDto> shoppingCartRegistrationPageInfo = new FxPageInfo<ShoppingCartRegistrationDto>();
                shoppingCartRegistrationPageInfo.TotalCount = await shoppingCartRegistration.CountAsync();
                shoppingCartRegistrationPageInfo.List = await shoppingCartRegistration.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                foreach (var x in shoppingCartRegistrationPageInfo.List)
                {
                    var contentPlatFormInfo = await _contentPlatformService.GetByIdAsync(x.ContentPlatFormId);
                    x.ContentPlatFormName = contentPlatFormInfo.ContentPlatformName;
                    var liveAnchorInfo = await _liveAnchorService.GetByIdAsync(x.LiveAnchorId);
                    x.LiveAnchorName = liveAnchorInfo.Name;
                    var empInfo = await _amiyaEmployeeService.GetByIdAsync(x.CreateBy);
                    x.CreateByName = empInfo.Name;
                    if (x.AssignEmpId.HasValue)
                    {
                        var assignEmpInfo = await _amiyaEmployeeService.GetByIdAsync(x.AssignEmpId.Value);
                        x.AssignEmpName = assignEmpInfo.Name;
                    }
                    x.BaseLiveAnchorName = dalLiveAnchorBaseInfo.GetAll().Where(e => e.Id == x.BaseLiveAnchorId).SingleOrDefault()?.LiveAnchorName ?? "";

                }
                return shoppingCartRegistrationPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task AddAsync(AddShoppingCartRegistrationDto addDto)
        {
            //unitOfWork.BeginTransaction();


            try
            {
                //if (!string.IsNullOrEmpty(addDto.Phone))
                //{
                //    var bind = await _dalBindCustomerService.GetAll()
                //  .Include(e => e.CustomerServiceAmiyaEmployee)
                //  .SingleOrDefaultAsync(e => e.BuyerPhone == addDto.Phone);
                //    if (bind != null)
                //    {
                //        if (bind.CustomerServiceId != addDto.CreateBy)
                //        {
                //            var employee = await dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == addDto.CreateBy);
                //            if (employee.IsCustomerService && !employee.AmiyaPositionInfo.IsDirector)
                //            {
                //                throw new Exception("该客户已绑定给" + bind.CustomerServiceAmiyaEmployee.Name + ",请联系对应人员进行操作！");
                //            }
                //        }

                //    }
                //    else
                //    {
                //        //添加绑定客服
                //        BindCustomerService bindCustomerService = new BindCustomerService();
                //        bindCustomerService.CustomerServiceId = addDto.CreateBy;
                //        bindCustomerService.BuyerPhone = addDto.Phone;
                //        bindCustomerService.UserId = null;
                //        bindCustomerService.CreateBy = addDto.CreateBy;
                //        bindCustomerService.CreateDate = DateTime.Now;
                //        await _dalBindCustomerService.AddAsync(bindCustomerService, true);
                //    }
                //}

                ShoppingCartRegistration shoppingCartRegistration = new ShoppingCartRegistration();
                shoppingCartRegistration.Id = CreateOrderIdHelper.GetNextNumber();
                shoppingCartRegistration.RecordDate = addDto.RecordDate;
                shoppingCartRegistration.ContentPlatFormId = addDto.ContentPlatFormId;
                shoppingCartRegistration.LiveAnchorId = addDto.LiveAnchorId;
                shoppingCartRegistration.LiveAnchorWechatNo = addDto.LiveAnchorWechatNo;
                shoppingCartRegistration.CustomerNickName = addDto.CustomerNickName;
                shoppingCartRegistration.ShoppingCartRegistrationCustomerType = addDto.ShoppingCartRegistrationCustomerType;
                shoppingCartRegistration.Phone = addDto.Phone;
                shoppingCartRegistration.SubPhone = addDto.SubPhone;
                shoppingCartRegistration.IsAddWeChat = addDto.IsAddWeChat;
                shoppingCartRegistration.Price = addDto.Price;
                shoppingCartRegistration.ConsultationType = addDto.ConsultationType;
                shoppingCartRegistration.IsWriteOff = addDto.IsWriteOff;
                shoppingCartRegistration.IsConsultation = addDto.IsConsultation;
                shoppingCartRegistration.ConsultationDate = addDto.ConsultationDate;
                shoppingCartRegistration.IsReturnBackPrice = addDto.IsReturnBackPrice;
                shoppingCartRegistration.Remark = addDto.Remark;
                shoppingCartRegistration.CreateBy = addDto.CreateBy;
                shoppingCartRegistration.AssignEmpId = addDto.AssignEmpId;
                shoppingCartRegistration.CreateDate = DateTime.Now;
                shoppingCartRegistration.BadReviewContent = addDto.BadReviewContent;
                shoppingCartRegistration.BadReviewDate = addDto.BadReviewDate;
                shoppingCartRegistration.BadReviewReason = addDto.BadReviewReason;
                shoppingCartRegistration.GetCustomerType = addDto.GetCustomerType;
                shoppingCartRegistration.IsReContent = addDto.IsReContent;
                shoppingCartRegistration.ReContent = addDto.ReContent;
                shoppingCartRegistration.RefundDate = addDto.RefundDate;
                shoppingCartRegistration.RefundReason = addDto.RefundReason;
                shoppingCartRegistration.IsBadReview = addDto.IsBadReview;
                shoppingCartRegistration.IsCreateOrder = addDto.IsCreateOrder;
                shoppingCartRegistration.IsSendOrder = addDto.IsSendOrder;
                shoppingCartRegistration.EmergencyLevel = addDto.EmergencyLevel;
                shoppingCartRegistration.Source = addDto.Source;
                shoppingCartRegistration.ProductType = addDto.ProductType;
                var baseLiveAnchorId = await _liveAnchorService.GetByIdAsync(addDto.LiveAnchorId);
                if (!string.IsNullOrEmpty(baseLiveAnchorId.LiveAnchorBaseId))
                {
                    shoppingCartRegistration.BaseLiveAnchorId = baseLiveAnchorId.LiveAnchorBaseId;
                }
                await dalShoppingCartRegistration.AddAsync(shoppingCartRegistration, true);

                //unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                //unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task AddListAsync(List<AddShoppingCartRegistrationDto> addDtoList)
        {
            unitOfWork.BeginTransaction();
            try
            {
                foreach (var addDto in addDtoList)
                {
                    var searchDataByPhone = await dalShoppingCartRegistration.GetAll().FirstOrDefaultAsync(e => e.Phone == addDto.Phone);
                    if (searchDataByPhone != null)
                    {
                        continue;
                    }

                    ShoppingCartRegistration shoppingCartRegistration = new ShoppingCartRegistration();
                    shoppingCartRegistration.Id = CreateOrderIdHelper.GetNextNumber();
                    shoppingCartRegistration.RecordDate = addDto.RecordDate;
                    shoppingCartRegistration.ContentPlatFormId = addDto.ContentPlatFormId;
                    shoppingCartRegistration.LiveAnchorId = addDto.LiveAnchorId;
                    shoppingCartRegistration.ShoppingCartRegistrationCustomerType = addDto.ShoppingCartRegistrationCustomerType;
                    shoppingCartRegistration.LiveAnchorWechatNo = addDto.LiveAnchorWechatNo;
                    shoppingCartRegistration.CustomerNickName = addDto.CustomerNickName;
                    shoppingCartRegistration.Phone = addDto.Phone;
                    shoppingCartRegistration.SubPhone = addDto.SubPhone;
                    shoppingCartRegistration.IsAddWeChat = addDto.IsAddWeChat;
                    shoppingCartRegistration.Price = addDto.Price;
                    shoppingCartRegistration.ConsultationType = addDto.ConsultationType;
                    shoppingCartRegistration.IsWriteOff = addDto.IsWriteOff;
                    shoppingCartRegistration.IsConsultation = addDto.IsConsultation;
                    shoppingCartRegistration.ConsultationDate = addDto.ConsultationDate;
                    shoppingCartRegistration.IsReturnBackPrice = addDto.IsReturnBackPrice;
                    shoppingCartRegistration.Remark = addDto.Remark;
                    shoppingCartRegistration.CreateBy = addDto.CreateBy;
                    shoppingCartRegistration.AssignEmpId = addDto.AssignEmpId;
                    shoppingCartRegistration.CreateDate = DateTime.Now;
                    shoppingCartRegistration.BadReviewContent = addDto.BadReviewContent;
                    shoppingCartRegistration.BadReviewDate = addDto.BadReviewDate;
                    shoppingCartRegistration.BadReviewReason = addDto.BadReviewReason;
                    shoppingCartRegistration.IsReContent = addDto.IsReContent;
                    shoppingCartRegistration.ReContent = addDto.ReContent;
                    shoppingCartRegistration.RefundDate = addDto.RefundDate;
                    shoppingCartRegistration.RefundReason = addDto.RefundReason;
                    shoppingCartRegistration.IsBadReview = addDto.IsBadReview;
                    shoppingCartRegistration.IsCreateOrder = addDto.IsCreateOrder;
                    shoppingCartRegistration.IsSendOrder = addDto.IsSendOrder;
                    shoppingCartRegistration.EmergencyLevel = addDto.EmergencyLevel;
                    shoppingCartRegistration.GetCustomerType = (int)ShoppingCartGetCustomerType.Ohter;
                    shoppingCartRegistration.Source = addDto.Source;
                    var baseLiveAnchorId = await _liveAnchorService.GetByIdAsync(addDto.LiveAnchorId);
                    if (!string.IsNullOrEmpty(baseLiveAnchorId.LiveAnchorBaseId))
                    {
                        shoppingCartRegistration.BaseLiveAnchorId = baseLiveAnchorId.LiveAnchorBaseId;
                    }
                    await dalShoppingCartRegistration.AddAsync(shoppingCartRegistration, true);
                    Thread.Sleep(1000);
                }

                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<ShoppingCartRegistrationDto> GetByIdAsync(string id)
        {
            try
            {
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (shoppingCartRegistration == null)
                {
                    return new ShoppingCartRegistrationDto();
                }

                ShoppingCartRegistrationDto shoppingCartRegistrationDto = new ShoppingCartRegistrationDto();
                shoppingCartRegistrationDto.Id = shoppingCartRegistration.Id;
                shoppingCartRegistrationDto.RecordDate = shoppingCartRegistration.RecordDate;
                shoppingCartRegistrationDto.ContentPlatFormId = shoppingCartRegistration.ContentPlatFormId;
                shoppingCartRegistrationDto.LiveAnchorId = shoppingCartRegistration.LiveAnchorId;
                shoppingCartRegistrationDto.LiveAnchorWechatNo = shoppingCartRegistration.LiveAnchorWechatNo;
                //var wechatInfo = await _liveAnchorWeChatInfoService.GetValidAsync();
                //var wechatResult = wechatInfo.FirstOrDefault(x => x.LiveAnchorId == shoppingCartRegistration.LiveAnchorId && x.WeChatNo == shoppingCartRegistration.LiveAnchorWechatNo);
                //shoppingCartRegistrationDto.LiveAnchorWeChatId = wechatResult.Id;
                shoppingCartRegistrationDto.CustomerNickName = shoppingCartRegistration.CustomerNickName;
                shoppingCartRegistrationDto.Phone = shoppingCartRegistration.Phone;
                shoppingCartRegistrationDto.SubPhone = shoppingCartRegistration.SubPhone;
                shoppingCartRegistrationDto.ShoppingCartRegistrationCustomerType = shoppingCartRegistration.ShoppingCartRegistrationCustomerType;
                shoppingCartRegistrationDto.Price = shoppingCartRegistration.Price;
                shoppingCartRegistrationDto.IsAddWeChat = shoppingCartRegistration.IsAddWeChat;
                shoppingCartRegistrationDto.ConsultationType = shoppingCartRegistration.ConsultationType;
                shoppingCartRegistrationDto.IsWriteOff = shoppingCartRegistration.IsWriteOff;
                shoppingCartRegistrationDto.IsConsultation = shoppingCartRegistration.IsConsultation;
                shoppingCartRegistrationDto.ConsultationDate = shoppingCartRegistration.ConsultationDate;
                shoppingCartRegistrationDto.IsReturnBackPrice = shoppingCartRegistration.IsReturnBackPrice;
                shoppingCartRegistrationDto.Remark = shoppingCartRegistration.Remark;
                shoppingCartRegistrationDto.CreateBy = shoppingCartRegistration.CreateBy;
                shoppingCartRegistrationDto.AssignEmpId = shoppingCartRegistration.AssignEmpId;
                shoppingCartRegistrationDto.IsCreateOrder = shoppingCartRegistration.IsCreateOrder;
                shoppingCartRegistrationDto.IsSendOrder = shoppingCartRegistration.IsSendOrder;
                shoppingCartRegistrationDto.CreateDate = shoppingCartRegistration.CreateDate;
                shoppingCartRegistrationDto.IsReContent = shoppingCartRegistration.IsReContent;
                shoppingCartRegistrationDto.ReContent = shoppingCartRegistration.ReContent;
                shoppingCartRegistrationDto.RefundDate = shoppingCartRegistration.RefundDate;
                shoppingCartRegistrationDto.RefundReason = shoppingCartRegistration.RefundReason;
                shoppingCartRegistrationDto.BadReviewDate = shoppingCartRegistration.BadReviewDate;
                shoppingCartRegistrationDto.BadReviewContent = shoppingCartRegistration.BadReviewContent;
                shoppingCartRegistrationDto.BadReviewReason = shoppingCartRegistration.BadReviewReason;
                shoppingCartRegistrationDto.IsBadReview = shoppingCartRegistration.IsBadReview;
                shoppingCartRegistrationDto.EmergencyLevel = shoppingCartRegistration.EmergencyLevel;
                shoppingCartRegistrationDto.Source = shoppingCartRegistration.Source;
                shoppingCartRegistrationDto.GetCustomerType = shoppingCartRegistration.GetCustomerType;
                shoppingCartRegistrationDto.ProductType = shoppingCartRegistration.ProductType;
                shoppingCartRegistrationDto.SourceText = ServiceClass.GetTiktokCustomerSourceText(shoppingCartRegistration.Source);
                shoppingCartRegistrationDto.BaseLiveAnchorId = shoppingCartRegistration.BaseLiveAnchorId;

                return shoppingCartRegistrationDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task<ShoppingCartRegistrationDto> GetByPhoneAsync(string phone, int createBy)
        {
            try
            {
                var shoppingCartRegistration = dalShoppingCartRegistration.GetAll().Where(k => k.CreateBy == createBy || k.AssignEmpId == createBy).Where(e => e.Phone == phone || e.SubPhone == phone).OrderByDescending(k => k.CreateDate).FirstOrDefault();
                if (shoppingCartRegistration == null)
                {
                    return new ShoppingCartRegistrationDto();
                }

                ShoppingCartRegistrationDto shoppingCartRegistrationDto = new ShoppingCartRegistrationDto();
                shoppingCartRegistrationDto.Id = shoppingCartRegistration.Id;
                shoppingCartRegistrationDto.RecordDate = shoppingCartRegistration.RecordDate;
                shoppingCartRegistrationDto.ContentPlatFormId = shoppingCartRegistration.ContentPlatFormId;
                shoppingCartRegistrationDto.ShoppingCartRegistrationCustomerType = shoppingCartRegistration.ShoppingCartRegistrationCustomerType;
                var contentPlatForm = await _contentPlatformService.GetByIdAsync(shoppingCartRegistrationDto.ContentPlatFormId);
                shoppingCartRegistrationDto.ContentPlatFormName = contentPlatForm.ContentPlatformName;
                shoppingCartRegistrationDto.LiveAnchorId = shoppingCartRegistration.LiveAnchorId;
                var liveAnchorInfo = await _liveAnchorService.GetByIdAsync(shoppingCartRegistrationDto.LiveAnchorId);
                shoppingCartRegistrationDto.LiveAnchorName = liveAnchorInfo.Name;
                shoppingCartRegistrationDto.LiveAnchorWechatNo = shoppingCartRegistration.LiveAnchorWechatNo;
                //var wechatInfo = await _liveAnchorWeChatInfoService.GetValidAsync();
                //var wechatResult = wechatInfo.FirstOrDefault(x => x.LiveAnchorId == shoppingCartRegistration.LiveAnchorId && x.WeChatNo == shoppingCartRegistration.LiveAnchorWechatNo);
                //shoppingCartRegistrationDto.LiveAnchorWeChatId = wechatResult.Id;
                shoppingCartRegistrationDto.CustomerNickName = shoppingCartRegistration.CustomerNickName;
                shoppingCartRegistrationDto.Phone = shoppingCartRegistration.Phone;
                shoppingCartRegistrationDto.GetCustomerType = shoppingCartRegistration.GetCustomerType;
                shoppingCartRegistrationDto.GetCustomerTypeText = ServiceClass.GetShoppingCartGetCustomerTypeText(shoppingCartRegistration.GetCustomerType);
                shoppingCartRegistrationDto.SubPhone = shoppingCartRegistration.SubPhone;
                shoppingCartRegistrationDto.Price = shoppingCartRegistration.Price;
                shoppingCartRegistrationDto.IsAddWeChat = shoppingCartRegistration.IsAddWeChat;
                shoppingCartRegistrationDto.ConsultationType = shoppingCartRegistration.ConsultationType;
                shoppingCartRegistrationDto.ConsultationTypeText = ServiceClass.GetConsulationTypeText(shoppingCartRegistration.ConsultationType);
                shoppingCartRegistrationDto.IsWriteOff = shoppingCartRegistration.IsWriteOff;
                shoppingCartRegistrationDto.IsConsultation = shoppingCartRegistration.IsConsultation;
                shoppingCartRegistrationDto.ConsultationDate = shoppingCartRegistration.ConsultationDate;
                shoppingCartRegistrationDto.IsReturnBackPrice = shoppingCartRegistration.IsReturnBackPrice;
                shoppingCartRegistrationDto.Remark = shoppingCartRegistration.Remark;
                shoppingCartRegistrationDto.CreateBy = shoppingCartRegistration.CreateBy;
                shoppingCartRegistrationDto.AssignEmpId = shoppingCartRegistration.AssignEmpId;
                shoppingCartRegistrationDto.IsCreateOrder = shoppingCartRegistration.IsCreateOrder;
                shoppingCartRegistrationDto.IsSendOrder = shoppingCartRegistration.IsSendOrder;
                shoppingCartRegistrationDto.CreateDate = shoppingCartRegistration.CreateDate;
                shoppingCartRegistrationDto.IsReContent = shoppingCartRegistration.IsReContent;
                shoppingCartRegistrationDto.ReContent = shoppingCartRegistration.ReContent;
                shoppingCartRegistrationDto.RefundDate = shoppingCartRegistration.RefundDate;
                shoppingCartRegistrationDto.RefundReason = shoppingCartRegistration.RefundReason;
                shoppingCartRegistrationDto.BadReviewDate = shoppingCartRegistration.BadReviewDate;
                shoppingCartRegistrationDto.BadReviewContent = shoppingCartRegistration.BadReviewContent;
                shoppingCartRegistrationDto.BadReviewReason = shoppingCartRegistration.BadReviewReason;
                shoppingCartRegistrationDto.IsBadReview = shoppingCartRegistration.IsBadReview;
                shoppingCartRegistrationDto.EmergencyLevel = shoppingCartRegistration.EmergencyLevel;
                shoppingCartRegistrationDto.Source = shoppingCartRegistration.Source;
                shoppingCartRegistrationDto.ProductType = shoppingCartRegistration.ProductType;
                shoppingCartRegistrationDto.ShoppingCartRegistrationCustomerType = shoppingCartRegistration.ShoppingCartRegistrationCustomerType;
                shoppingCartRegistrationDto.ShoppingCartRegistrationCustomerTypeText = ServiceClass.GetShoppingCartCustomerTypeText(shoppingCartRegistration.ShoppingCartRegistrationCustomerType);
                shoppingCartRegistrationDto.SourceText = ServiceClass.GetTiktokCustomerSourceText(shoppingCartRegistration.Source);
                return shoppingCartRegistrationDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<ShoppingCartRegistrationDto> GetByPhoneAsync(string phone)
        {
            try
            {
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().FirstOrDefaultAsync(e => e.Phone == phone || e.SubPhone == phone);
                if (shoppingCartRegistration == null)
                {
                    return new ShoppingCartRegistrationDto();
                }

                ShoppingCartRegistrationDto shoppingCartRegistrationDto = new ShoppingCartRegistrationDto();
                shoppingCartRegistrationDto.Id = shoppingCartRegistration.Id;
                shoppingCartRegistrationDto.RecordDate = shoppingCartRegistration.RecordDate;
                shoppingCartRegistrationDto.ContentPlatFormId = shoppingCartRegistration.ContentPlatFormId;
                shoppingCartRegistrationDto.LiveAnchorId = shoppingCartRegistration.LiveAnchorId;
                shoppingCartRegistrationDto.LiveAnchorWechatNo = shoppingCartRegistration.LiveAnchorWechatNo;
                shoppingCartRegistrationDto.CustomerNickName = shoppingCartRegistration.CustomerNickName;
                shoppingCartRegistrationDto.ShoppingCartRegistrationCustomerType = shoppingCartRegistrationDto.ShoppingCartRegistrationCustomerType;
                shoppingCartRegistrationDto.GetCustomerType = shoppingCartRegistration.GetCustomerType;
                shoppingCartRegistrationDto.Phone = shoppingCartRegistration.Phone;
                shoppingCartRegistrationDto.SubPhone = shoppingCartRegistration.SubPhone;
                shoppingCartRegistrationDto.Price = shoppingCartRegistration.Price;
                shoppingCartRegistrationDto.IsCreateOrder = shoppingCartRegistration.IsCreateOrder;
                shoppingCartRegistrationDto.IsSendOrder = shoppingCartRegistration.IsSendOrder;
                shoppingCartRegistrationDto.IsAddWeChat = shoppingCartRegistration.IsAddWeChat;
                shoppingCartRegistrationDto.ConsultationType = shoppingCartRegistration.ConsultationType;
                shoppingCartRegistrationDto.IsWriteOff = shoppingCartRegistration.IsWriteOff;
                shoppingCartRegistrationDto.IsConsultation = shoppingCartRegistration.IsConsultation;
                shoppingCartRegistrationDto.ConsultationDate = shoppingCartRegistration.ConsultationDate;
                shoppingCartRegistrationDto.IsReturnBackPrice = shoppingCartRegistration.IsReturnBackPrice;
                shoppingCartRegistrationDto.Remark = shoppingCartRegistration.Remark;
                shoppingCartRegistrationDto.CreateBy = shoppingCartRegistration.CreateBy;
                shoppingCartRegistrationDto.AssignEmpId = shoppingCartRegistration.AssignEmpId;
                shoppingCartRegistrationDto.CreateDate = shoppingCartRegistration.CreateDate;
                shoppingCartRegistrationDto.IsReContent = shoppingCartRegistration.IsReContent;
                shoppingCartRegistrationDto.ReContent = shoppingCartRegistration.ReContent;
                shoppingCartRegistrationDto.RefundDate = shoppingCartRegistration.RefundDate;
                shoppingCartRegistrationDto.RefundReason = shoppingCartRegistration.RefundReason;
                shoppingCartRegistrationDto.BadReviewDate = shoppingCartRegistration.BadReviewDate;
                shoppingCartRegistrationDto.BadReviewContent = shoppingCartRegistration.BadReviewContent;
                shoppingCartRegistrationDto.BadReviewReason = shoppingCartRegistration.BadReviewReason;
                shoppingCartRegistrationDto.IsBadReview = shoppingCartRegistration.IsBadReview;
                shoppingCartRegistrationDto.Source = shoppingCartRegistration.Source;
                shoppingCartRegistrationDto.ProductType = shoppingCartRegistrationDto.ProductType;
                return shoppingCartRegistrationDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateAsync(UpdateShoppingCartRegistrationDto updateDto)
        {
            var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
            if (shoppingCartRegistration == null)
                throw new Exception("小黄车登记编号错误！");
            if (updateDto.OperationBy != shoppingCartRegistration.CreateBy)
            {
                shoppingCartRegistration.IsAddWeChat = updateDto.IsAddWeChat;
                shoppingCartRegistration.Remark = updateDto.Remark;
                await dalShoppingCartRegistration.UpdateAsync(shoppingCartRegistration, true);
                //throw new Exception("数据已编辑成功，因当前登录账号和创建人不一致,该部分数据只有加V与备注修改生效！");
            }
            #region 删除部分
            //if (!string.IsNullOrEmpty(updateDto.Phone))
            //{
            //    var bind = await _dalBindCustomerService.GetAll()
            //  .Include(e => e.CustomerServiceAmiyaEmployee)
            //  .SingleOrDefaultAsync(e => e.BuyerPhone == updateDto.Phone);
            //    if (bind != null)
            //    {
            //        if (bind.CustomerServiceId != shoppingCartRegistration.CreateBy)
            //        {
            //            var employee = await dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == shoppingCartRegistration.CreateBy);
            //            if (employee.IsCustomerService && !employee.AmiyaPositionInfo.IsDirector)
            //            {
            //                throw new Exception("该客户已绑定给" + bind.CustomerServiceAmiyaEmployee.Name + ",请联系对应人员进行操作！");
            //            }
            //        }

            //    }
            //    else
            //    {
            //        //添加绑定客服
            //        BindCustomerService bindCustomerService = new BindCustomerService();
            //        bindCustomerService.CustomerServiceId = shoppingCartRegistration.CreateBy;
            //        bindCustomerService.BuyerPhone = updateDto.Phone;
            //        bindCustomerService.UserId = null;
            //        bindCustomerService.CreateBy = shoppingCartRegistration.CreateBy;
            //        bindCustomerService.CreateDate = DateTime.Now;
            //        await _dalBindCustomerService.AddAsync(bindCustomerService, true);
            //    }
            //}
            #endregion
            else
            {
                shoppingCartRegistration.RecordDate = updateDto.RecordDate;
                shoppingCartRegistration.ContentPlatFormId = updateDto.ContentPlatFormId;
                shoppingCartRegistration.LiveAnchorId = updateDto.LiveAnchorId;
                shoppingCartRegistration.LiveAnchorWechatNo = updateDto.LiveAnchorWechatNo;
                shoppingCartRegistration.CustomerNickName = updateDto.CustomerNickName;
                shoppingCartRegistration.Phone = updateDto.Phone;
                shoppingCartRegistration.SubPhone = updateDto.SubPhone;
                shoppingCartRegistration.Price = updateDto.Price;
                shoppingCartRegistration.GetCustomerType = updateDto.GetCustomerType;
                shoppingCartRegistration.IsAddWeChat = updateDto.IsAddWeChat;
                shoppingCartRegistration.ShoppingCartRegistrationCustomerType = updateDto.ShoppingCartRegistrationCustomerType;
                shoppingCartRegistration.ConsultationType = updateDto.ConsultationType;
                shoppingCartRegistration.IsWriteOff = updateDto.IsWriteOff;
                shoppingCartRegistration.IsConsultation = updateDto.IsConsultation;
                shoppingCartRegistration.ConsultationDate = updateDto.ConsultationDate;
                shoppingCartRegistration.IsReturnBackPrice = updateDto.IsReturnBackPrice;
                shoppingCartRegistration.Remark = updateDto.Remark;
                shoppingCartRegistration.IsReContent = updateDto.IsReContent;
                shoppingCartRegistration.ReContent = updateDto.ReContent;
                shoppingCartRegistration.RefundDate = updateDto.RefundDate;
                shoppingCartRegistration.RefundReason = updateDto.RefundReason;
                shoppingCartRegistration.BadReviewDate = updateDto.BadReviewDate;
                shoppingCartRegistration.BadReviewContent = updateDto.BadReviewContent;
                shoppingCartRegistration.BadReviewReason = updateDto.BadReviewReason;
                shoppingCartRegistration.IsBadReview = updateDto.IsBadReview;
                shoppingCartRegistration.AssignEmpId = updateDto.AssignEmpId;
                shoppingCartRegistration.IsCreateOrder = updateDto.IsCreateOrder;
                shoppingCartRegistration.IsSendOrder = updateDto.IsSendOrder;
                shoppingCartRegistration.EmergencyLevel = updateDto.EmergencyLevel;
                shoppingCartRegistration.Source = updateDto.Source;
                shoppingCartRegistration.CreateBy = updateDto.CreateBy;
                shoppingCartRegistration.ProductType = updateDto.ProductType;
                var baseLiveAnchorId = await _liveAnchorService.GetByIdAsync(updateDto.LiveAnchorId);
                if (!string.IsNullOrEmpty(baseLiveAnchorId.LiveAnchorBaseId))
                {
                    shoppingCartRegistration.BaseLiveAnchorId = baseLiveAnchorId.LiveAnchorBaseId;
                }
                await dalShoppingCartRegistration.UpdateAsync(shoppingCartRegistration, true);
            }
        }

        public async Task AssignAsync(string id, int assignBy)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (shoppingCartRegistration == null)
                    throw new Exception("小黄车登记编号错误！");
                shoppingCartRegistration.AssignEmpId = assignBy;
                await dalShoppingCartRegistration.UpdateAsync(shoppingCartRegistration, true);

                //动消息添加提示内容
                AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                addMessageNoticeDto.AcceptBy = assignBy;
                addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.DistributeInterviewNotice;
                addMessageNoticeDto.NoticeContent = "您收到了新的分诊订单，请及时跟进~";
                await messageNoticeService.AddAsync(addMessageNoticeDto);

                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 录单触达
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task UpdateCreateOrderAsync(string phone)
        {
            //   unitOfWork.BeginTransaction();
            try
            {
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().Where(e => e.Phone == phone || e.SubPhone == phone).ToListAsync();
                if (shoppingCartRegistration.Count > 0)
                {
                    foreach (var x in shoppingCartRegistration)
                    {
                        x.EmergencyLevel = (int)EmergencyLevel.Important;
                        x.IsCreateOrder = true;
                        x.IsAddWeChat = true;
                        //x.IsWriteOff = true;
                        x.IsConsultation = true;
                        x.ConsultationDate = DateTime.Now;
                        await dalShoppingCartRegistration.UpdateAsync(x, true);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 派单触达
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task UpdateSendOrderAsync(string phone)
        {
            //   unitOfWork.BeginTransaction();
            try
            {
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().Where(e => e.Phone == phone || e.SubPhone == phone).ToListAsync();
                if (shoppingCartRegistration.Count > 0)
                {
                    foreach (var x in shoppingCartRegistration)
                    {
                        x.EmergencyLevel = (int)EmergencyLevel.Important;
                        x.IsSendOrder = true;
                        await dalShoppingCartRegistration.UpdateAsync(x, true);
                    }
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
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().SingleOrDefaultAsync(e => e.Id == id);

                if (shoppingCartRegistration == null)
                    throw new Exception("小黄车登记编号错误");

                await dalShoppingCartRegistration.DeleteAsync(shoppingCartRegistration, true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public List<EmergencyLevelDto> GetEmergencyLevelList()
        {
            var emergencyLevels = Enum.GetValues(typeof(EmergencyLevel));
            List<EmergencyLevelDto> emergencyLevelList = new List<EmergencyLevelDto>();
            foreach (var item in emergencyLevels)
            {
                EmergencyLevelDto emergencyLevelDto = new EmergencyLevelDto();
                emergencyLevelDto.EmergencyLevel = Convert.ToInt32(item);
                emergencyLevelDto.EmergencyText = ServiceClass.GetShopCartRegisterEmergencyLevelText(emergencyLevelDto.EmergencyLevel);
                emergencyLevelList.Add(emergencyLevelDto);
            }
            return emergencyLevelList;
        }
        /// <summary>
        /// 获取客户来源列表
        /// </summary>
        /// <returns></returns>
        public List<BaseKeyValueDto<int>> GetCustomerSourceList()
        {
            var emergencyLevels = Enum.GetValues(typeof(TiktokCustomerSource));
            List<BaseKeyValueDto<int>> emergencyLevelList = new List<BaseKeyValueDto<int>>();
            foreach (var item in emergencyLevels)
            {
                BaseKeyValueDto<int> emergencyLevelDto = new BaseKeyValueDto<int>();
                emergencyLevelDto.Key = Convert.ToInt32(item);
                emergencyLevelDto.Value = ServiceClass.GetTiktokCustomerSourceText(emergencyLevelDto.Key);
                emergencyLevelList.Add(emergencyLevelDto);
            }
            return emergencyLevelList;
        }


        /// <summary>
        /// 获取客户类型列表
        /// </summary>
        /// <returns></returns>
        public List<BaseKeyValueDto<int>> GetCustomerTypeList()
        {
            var enumResult = Enum.GetValues(typeof(ShoppingCartRegistionCustomerSource));
            List<BaseKeyValueDto<int>> result = new List<BaseKeyValueDto<int>>();
            foreach (var item in enumResult)
            {
                BaseKeyValueDto<int> keyAndValue = new BaseKeyValueDto<int>();
                keyAndValue.Key = Convert.ToInt32(item);
                keyAndValue.Value = ServiceClass.GetShoppingCartCustomerTypeText(keyAndValue.Key);
                result.Add(keyAndValue);
            }
            return result;
        }

        /// <summary>
        /// 获取带货产品类型列表
        /// </summary>
        /// <returns></returns>
        public List<BaseKeyValueDto<int>> GetShoppingCartTakeGoodsProductTypeList()
        {
            var emergencyLevels = Enum.GetValues(typeof(ShoppingCartProductType));
            List<BaseKeyValueDto<int>> resultList = new List<BaseKeyValueDto<int>>();
            foreach (var item in emergencyLevels)
            {
                BaseKeyValueDto<int> resultDto = new BaseKeyValueDto<int>();
                resultDto.Key = Convert.ToInt32(item);
                resultDto.Value = ServiceClass.GetShoppingCartTakeGoodsProductTypeText(resultDto.Key);
                resultList.Add(resultDto);
            }
            return resultList;
        }

        /// <summary>
        /// 获取面诊方式列表
        /// </summary>
        /// <returns></returns>
        public List<BaseKeyValueDto<int>> GetShoppingCartConsultationTypeText()
        {
            var emergencyLevels = Enum.GetValues(typeof(ShoppingCartConsultationType));
            List<BaseKeyValueDto<int>> emergencyLevelList = new List<BaseKeyValueDto<int>>();
            foreach (var item in emergencyLevels)
            {
                BaseKeyValueDto<int> emergencyLevelDto = new BaseKeyValueDto<int>();
                emergencyLevelDto.Key = Convert.ToInt32(item);
                emergencyLevelDto.Value = ServiceClass.GetConsulationTypeText(emergencyLevelDto.Key);
                emergencyLevelList.Add(emergencyLevelDto);
            }
            return emergencyLevelList;
        }

        /// <summary>
        /// 获取获客方式
        /// </summary>
        /// <returns></returns>
        public List<BaseKeyValueDto<int>> GetShoppingCartGetCustomerTypeText()
        {
            var emergencyLevels = Enum.GetValues(typeof(ShoppingCartGetCustomerType));
            List<BaseKeyValueDto<int>> emergencyLevelList = new List<BaseKeyValueDto<int>>();
            foreach (var item in emergencyLevels)
            {
                BaseKeyValueDto<int> emergencyLevelDto = new BaseKeyValueDto<int>();
                emergencyLevelDto.Key = Convert.ToInt32(item);
                emergencyLevelDto.Value = ServiceClass.GetShoppingCartGetCustomerTypeText(emergencyLevelDto.Key);
                emergencyLevelList.Add(emergencyLevelDto);
            }
            return emergencyLevelList;
        }

        #region 【日数据业绩生成】

        /// <summary>
        /// 根据条件获取今日面诊业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetDialyConsulationCardInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = recordDate.AddDays(1);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.IsConsultation == true && o.ConsultationDate.HasValue == true && o.IsReturnBackPrice == false && o.IsBadReview == false)
                .Where(o => o.ConsultationDate >= currentDate && o.ConsultationDate < endDate && o.LiveAnchorId == liveAnchorId)
                         select new ShoppingCartRegistrationDto
                         {
                             ConsultationDate = d.ConsultationDate,
                             RecordDate = d.RecordDate,
                             ConsultationType = d.ConsultationType,
                         };
            return await result.OrderByDescending(x => x.RecordDate).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取今日加v派单业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetDialyAddWeChatInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = recordDate.Date.AddDays(1);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.RecordDate >= currentDate && o.RecordDate < endDate && o.LiveAnchorId == liveAnchorId && o.IsAddWeChat == true)
                         select new ShoppingCartRegistrationDto
                         {
                             IsSendOrder = d.IsSendOrder,
                         };
            return await result.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取今日小黄车退款量
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetDialyYellowCardRefundInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = recordDate.Date.AddDays(1);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.RefundDate.Value >= currentDate && o.RefundDate.Value < endDate && o.LiveAnchorId == liveAnchorId && o.IsReturnBackPrice == true)
                         select new ShoppingCartRegistrationDto
                         {
                             IsSendOrder = d.IsSendOrder,
                         };
            return await result.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取今日小黄车差评量
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetDialyYellowCardBadReviewInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = DateTime.Now.Date.AddDays(1);
            //选定的月份
            DateTime currentDate = DateTime.Now.Date;
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.BadReviewDate.Value >= currentDate && o.BadReviewDate.Value < endDate && o.LiveAnchorId == liveAnchorId && o.IsBadReview == true)
                         select new ShoppingCartRegistrationDto
                         {
                             IsSendOrder = d.IsSendOrder,
                         };
            return await result.ToListAsync();
        }
        #endregion

        #region 【报表相关】
        public async Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistrationReportAsync(DateTime? startDate, DateTime? endDate, int? emergencyLevel, int? LiveAnchorId, bool? isCreateOrder, bool? isSendOrder, int? employeeId, bool? isAddWechat, bool? isWriteOff, bool? isConsultation, bool? isReturnBackPrice, string keyword, string contentPlatFormId, bool isHidePhone, string baseLiveAnchorId, int? source)
        {
            try
            {
                var shoppingCartRegistration = from d in dalShoppingCartRegistration.GetAll().Include(x => x.Contentplatform).Include(x => x.LiveAnchor).Include(x => x.AmiyaEmployee)
                                               where (keyword == null || d.Phone.Contains(keyword) || d.SubPhone.Contains(keyword) || d.CustomerNickName.Contains(keyword))
                                               && ((!startDate.HasValue && !endDate.HasValue) || d.RecordDate >= startDate.Value.Date && d.RecordDate < endDate.Value.AddDays(1).Date)
                                               && (string.IsNullOrEmpty(contentPlatFormId) || d.ContentPlatFormId == contentPlatFormId)
                                               && (!LiveAnchorId.HasValue || d.LiveAnchorId == LiveAnchorId) && (!isAddWechat.HasValue || d.IsAddWeChat == isAddWechat)
                                               && (!isWriteOff.HasValue || d.IsWriteOff == isWriteOff)
                                               && (!emergencyLevel.HasValue || d.EmergencyLevel == emergencyLevel)
                                               && (!isConsultation.HasValue || d.IsConsultation == isConsultation)
                                               && (!isSendOrder.HasValue || d.IsSendOrder == isSendOrder)
                                               && (!isCreateOrder.HasValue || d.IsCreateOrder == isCreateOrder)
                                               && (!isReturnBackPrice.HasValue || d.IsReturnBackPrice == isReturnBackPrice)
                                               && (string.IsNullOrEmpty(baseLiveAnchorId) || d.BaseLiveAnchorId == baseLiveAnchorId)
                                               && (!source.HasValue || d.Source == source.Value)
                                               select new ShoppingCartRegistrationDto
                                               {
                                                   Id = d.Id,
                                                   RecordDate = d.RecordDate,
                                                   ContentPlatFormId = d.ContentPlatFormId,
                                                   ContentPlatFormName = d.Contentplatform.ContentPlatformName,
                                                   LiveAnchorId = d.LiveAnchorId,
                                                   EmergencyLevel = d.EmergencyLevel,
                                                   LiveAnchorName = d.LiveAnchor.Name,
                                                   IsCreateOrder = d.IsCreateOrder,
                                                   IsSendOrder = d.IsSendOrder,
                                                   LiveAnchorWechatNo = d.LiveAnchorWechatNo,
                                                   CustomerNickName = d.CustomerNickName,
                                                   Phone = isHidePhone == true ? ServiceClass.GetIncompletePhone(d.Phone) : d.Phone,
                                                   SubPhone = isHidePhone == true ? (string.IsNullOrEmpty(d.SubPhone) ? "" : ServiceClass.GetIncompletePhone(d.SubPhone)) : d.SubPhone,
                                                   Price = d.Price,
                                                   ConsultationType = d.ConsultationType,
                                                   IsWriteOff = d.IsWriteOff,
                                                   IsAddWeChat = d.IsAddWeChat,
                                                   IsConsultation = d.IsConsultation,
                                                   IsReturnBackPrice = d.IsReturnBackPrice,
                                                   Remark = d.Remark,
                                                   CreateBy = d.CreateBy,
                                                   CreateByName = d.AmiyaEmployee.Name,
                                                   CreateDate = d.CreateDate,
                                                   ConsultationTypeText = ServiceClass.GetConsulationTypeText(d.ConsultationType),
                                                   Source = d.Source,
                                                   SourceText = ServiceClass.GetTiktokCustomerSourceText(d.Source),
                                                   BaseLiveAnchorId = d.BaseLiveAnchorId,
                                                   BaseLiveAnchorName = dalLiveAnchorBaseInfo.GetAll().Where(e => e.Id == d.BaseLiveAnchorId).SingleOrDefault() == null ? "" : dalLiveAnchorBaseInfo.GetAll().Where(e => e.Id == d.BaseLiveAnchorId).SingleOrDefault().LiveAnchorName,
                                                   ProductTypeText = ServiceClass.GetShoppingCartTakeGoodsProductTypeText(d.ProductType)
                                               };
                var employee = await dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == employeeId);
                if (!employee.AmiyaPositionInfo.IsDirector)
                {
                    shoppingCartRegistration = from d in shoppingCartRegistration
                                               where (d.CreateBy == employeeId || d.AssignEmpId == employeeId)
                                               select d;
                }
                List<ShoppingCartRegistrationDto> shoppingCartRegistrationPageInfo = new List<ShoppingCartRegistrationDto>();
                shoppingCartRegistrationPageInfo = await shoppingCartRegistration.OrderByDescending(x => x.RecordDate).ToListAsync();
                return shoppingCartRegistrationPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }



        #endregion

        #region 【业绩数据】
        /// <summary>
        /// 根据条件获取小黄车登记业绩（照片面诊业绩与视频面诊业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistrationListByLiveAnchorNameAndIsVideoAsync(int year, int month, bool isVideo, List<int> liveAnchorIds)
        {
            int consultationType = 0;
            if (isVideo == true)
            {
                consultationType = (int)ShoppingCartConsultationType.Video;
            }
            else
            {
                consultationType = (int)ShoppingCartConsultationType.Picture;
            }
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            //选定的月份
            DateTime currentDate = new DateTime(year, month, 1);
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.IsConsultation == true && o.IsReturnBackPrice == false && o.IsBadReview == false)
                .Where(o => o.ConsultationDate >= currentDate && o.ConsultationDate < endDate && o.ConsultationType == consultationType)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                         select new ShoppingCartRegistrationDto
                         {
                             RecordDate = d.RecordDate,
                             EmergencyLevelText = ServiceClass.GetShopCartRegisterEmergencyLevelText(d.EmergencyLevel),
                             CustomerNickName = d.CustomerNickName,
                             Phone = ServiceClass.GetIncompletePhone(d.Phone),
                             Price = d.Price,
                             IsCreateOrder = d.IsCreateOrder,
                             IsSendOrder = d.IsSendOrder,
                             IsConsultation = d.IsConsultation,
                             IsAddWeChat = d.IsAddWeChat,
                             IsWriteOff = d.IsWriteOff,
                             IsReturnBackPrice = d.IsReturnBackPrice,
                             IsBadReview = d.IsBadReview,
                         };
            return await result.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取小黄车登记业绩（经营看板业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorIds"></param>
        /// <param name="isHistoryConsulationCardConsumed">是否为历史面诊卡消耗数据</param>
        /// <param name="isConsulationCardRefund">是否为历史面诊卡退单数据</param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetBaseBusinessPerformanceByLiveAnchorNameAsync(int year, int month, bool? isHistoryConsulationCardConsumed, bool? isConsulationCardRefund, List<int> liveAnchorIds)
        {
            var contentPlatForm = await _contentPlatformService.GetValidListAsync();
            var comtentPlatFormId = contentPlatForm.Where(x => x.ContentPlatformName == "抖音").Select(x => x.Id).FirstOrDefault();
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            //选定的月份
            DateTime currentDate = new DateTime(year, month, 1);
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.ContentPlatFormId == comtentPlatFormId)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                         select d;

            if (isHistoryConsulationCardConsumed == null && isConsulationCardRefund == null)
            {
                result = from d in result
              .Where(o => o.RecordDate >= currentDate && o.RecordDate < endDate)
                         select d;
            }
            if (isHistoryConsulationCardConsumed == false)
            {
                result = from d in result
                .Where(o => o.IsReturnBackPrice == false && o.IsBadReview == false && o.RecordDate >= currentDate && o.RecordDate < endDate)
                  .Where(o => o.ConsultationDate.HasValue == true && o.RecordDate.Month == o.ConsultationDate.Value.Month)
                         select d;
            }
            else if (isHistoryConsulationCardConsumed == true)
            {
                result = from d in result
                  .Where(o => o.IsReturnBackPrice == false && o.IsBadReview == false && o.ConsultationDate.HasValue == true && o.ConsultationDate.Value >= currentDate && o.ConsultationDate.Value < endDate && o.RecordDate.Month != o.ConsultationDate.Value.Month)
                         select d;
            }

            if (isConsulationCardRefund == false)
            {
                result = from d in result
                .Where(o => o.IsReturnBackPrice == false && o.IsBadReview == false && o.RecordDate >= currentDate && o.RecordDate < endDate)
                  .Where(o => o.RefundDate.HasValue == true && o.RecordDate.Month == o.RefundDate.Value.Month)
                         select d;
            }
            else if (isConsulationCardRefund == true)
            {
                result = from d in result
                  .Where(o => o.IsReturnBackPrice == true && o.RefundDate.HasValue == true && o.RefundDate.Value >= currentDate && o.RefundDate.Value < endDate && o.RecordDate.Month != o.RefundDate.Value.Month)
                         select d;
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        RecordDate = d.RecordDate,
                        EmergencyLevelText = ServiceClass.GetShopCartRegisterEmergencyLevelText(d.EmergencyLevel),
                        CustomerNickName = d.CustomerNickName,
                        Phone = d.Phone,
                        Price = d.Price,
                        RefundDate = d.RefundDate,
                        IsCreateOrder = d.IsCreateOrder,
                        IsSendOrder = d.IsSendOrder,
                        IsConsultation = d.IsConsultation,
                        ConsultationDate = d.ConsultationDate,
                        IsAddWeChat = d.IsAddWeChat,
                        IsWriteOff = d.IsWriteOff,
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        IsBadReview = d.IsBadReview,
                    };
            return await x.OrderByDescending(x => x.RecordDate).ToListAsync();
        }


        /// <summary>
        /// 根据平台/有效潜在获取小黄车登记业绩（新经营看板业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetNewBaseBusinessPerformanceByLiveAnchorNameAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, string contentPlatFormId)
        {
            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => string.IsNullOrEmpty(contentPlatFormId) || o.ContentPlatFormId == contentPlatFormId)
            .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate)

                         select d;
            if (isEffectiveCustomerData.HasValue)
            {
                result = result.Where(o => isEffectiveCustomerData == true ? o.Price > 0 : o.Price == 0);
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                    };
            return await x.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取助理月业绩完成情况数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetAsistantMonthPerformanceDataAsync(QueryAssistantHomePageDataDto query)
        {

            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => o.RecordDate >= query.StartDate.Value && o.RecordDate < query.EndDate.Value)
                         select d;
            if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                var ids = (await _liveAnchorService.GetLiveAnchorListByBaseInfoId(query.BaseLiveAnchorId)).Select(e => e.Id);
                result = result.Where(e => ids.Contains(e.LiveAnchorId));
            }
            if (!string.IsNullOrEmpty(query.ContentPlatformId))
            {
                var ids = await _liveAnchorService.GetLiveAnchorIdsByContentPlatformIdAndBaseId(query.ContentPlatformId, "");
                result = result.Where(e => ids.Contains(e.LiveAnchorId));
            }
            if (query.LiveAnchorId.HasValue)
            {
                result = result.Where(e => e.LiveAnchorId == query.LiveAnchorId);
            }
            if (!string.IsNullOrEmpty(query.WechatNoId))
            {
                result = result.Where(e => e.LiveAnchorWechatNo == query.WechatNoId);
            }
            //if (query.Source.HasValue)
            //{
            //    contentPlatformOrderDeal = contentPlatformOrderDeal.Where(e => e.ContentPlatFormOrder.OrderSource == query.Source);
            //}
            if (query.AssistantId.HasValue)
            {
                result = result.Where(e => e.AssignEmpId == query.AssistantId.Value);
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                    };
            return await x.ToListAsync();
        }

        /// <summary>
        /// 获取面诊卡库存
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistrationInventoryAsync(List<int> liveAnchorIds)
        {
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.IsConsultation == false && o.IsReturnBackPrice == false && o.IsBadReview == false)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                         select new ShoppingCartRegistrationDto
                         {
                             RecordDate = d.RecordDate,
                             EmergencyLevelText = ServiceClass.GetShopCartRegisterEmergencyLevelText(d.EmergencyLevel),
                             CustomerNickName = d.CustomerNickName,
                             Phone = d.Phone,
                             Price = d.Price,
                             IsCreateOrder = d.IsCreateOrder,
                             IsSendOrder = d.IsSendOrder,
                             IsConsultation = d.IsConsultation,
                             IsAddWeChat = d.IsAddWeChat,
                             IsWriteOff = d.IsWriteOff,
                             IsReturnBackPrice = d.IsReturnBackPrice,
                             IsBadReview = d.IsBadReview,
                         };
            return await result.ToListAsync();
        }


        /// <summary>
        /// 根据条件获取小黄车照片/视频面诊业绩折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<PerformanceInfoByDateDto>> GetPictureOrVideoConsultationBrokenLineAsync(int year, int month, bool isVideo, List<int> liveAnchorIds)
        {
            //开始月份
            DateTime startTime = new DateTime(year, 1, 1);
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            int consultationType = 0;
            if (isVideo == true)
            {
                consultationType = (int)ShoppingCartConsultationType.Video;
            }
            else
            {
                consultationType = (int)ShoppingCartConsultationType.Picture;
            }
            var list = await dalShoppingCartRegistration.GetAll()
                .Where(o => o.IsConsultation == true && o.IsReturnBackPrice == false && o.IsBadReview == false)
                .Where(o => o.RecordDate >= startTime && o.RecordDate < endDate && o.ConsultationType == consultationType)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId)).ToListAsync();

            var resultList = list.Select(x => new PerformanceInfoDateDto { Date = x.CreateDate, PerfomancePrice = x.Price }).ToList();
            var returnResult = resultList.GroupBy(x => x.Date.Month).Select(x => new PerformanceInfoByDateDto { Date = x.Key.ToString(), PerfomancePrice = x.Count() }).ToList();
            return returnResult;
        }

        /// <summary>
        /// 根据条件获取小黄车登记业绩（经营看板业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isHistoryConsulationCardConsumed">是否为历史面诊卡消耗数据</param>
        /// <param name="isConsulationCardRefund">是否为历史面诊卡退单数据</param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<PerformanceBrokenLine>> GetBaseBusinessPerformanceByLiveAnchorNameBrokenLineAsync(int year, int month, bool? isHistoryConsulationCardConsumed, bool? isConsulationCardRefund, bool? isAddWechat, bool? isConsulation, List<int> liveAnchorIds)
        {
            var contentPlatForm = await _contentPlatformService.GetValidListAsync();
            var comtentPlatFormId = contentPlatForm.Where(x => x.ContentPlatformName == "抖音").Select(x => x.Id).FirstOrDefault();
            //开始月份
            DateTime currentDate = new DateTime(year, 1, 1);
            //筛选结束的月份
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1);
            var result = from d in dalShoppingCartRegistration.GetAll()
                .Where(o => o.ContentPlatFormId == comtentPlatFormId)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Where(x => !isAddWechat.HasValue || x.IsAddWeChat == isAddWechat.Value)
                .Where(x => !isConsulation.HasValue || x.IsConsultation == isConsulation.Value)
                         select d;

            if (isHistoryConsulationCardConsumed == null && isConsulationCardRefund == null)
            {
                result = from d in result
              .Where(o => o.RecordDate >= currentDate && o.RecordDate < endDate)
                         select d;
            }
            if (isHistoryConsulationCardConsumed == false)
            {
                result = from d in result
                .Where(o => o.IsReturnBackPrice == false && o.IsBadReview == false && o.RecordDate >= currentDate && o.RecordDate < endDate)
                  .Where(o => o.ConsultationDate.HasValue == true && o.RecordDate.Month == o.ConsultationDate.Value.Month)
                         select d;
            }
            else if (isHistoryConsulationCardConsumed == true)
            {
                result = from d in result
                  .Where(o => o.IsReturnBackPrice == false && o.IsBadReview == false && o.ConsultationDate.HasValue == true && o.ConsultationDate.Value >= currentDate && o.ConsultationDate.Value < endDate && o.RecordDate.Month != o.ConsultationDate.Value.Month)
                         select d;
            }

            if (isConsulationCardRefund == false)
            {
                result = from d in result
                .Where(o => o.IsReturnBackPrice == false && o.IsBadReview == false && o.RecordDate >= currentDate && o.RecordDate < endDate)
                  .Where(o => o.RefundDate.HasValue == true && o.RecordDate.Month == o.RefundDate.Value.Month)
                         select d;
            }
            else if (isConsulationCardRefund == true)
            {
                result = from d in result
                  .Where(o => o.IsReturnBackPrice == true && o.RefundDate.HasValue == true && o.RefundDate.Value >= currentDate && o.RefundDate.Value < endDate && o.RecordDate.Month != o.RefundDate.Value.Month)
                         select d;
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        RecordDate = d.RecordDate,
                        ConsultationDate = d.ConsultationDate,
                        Price = d.Price,
                        RefundDate = d.RefundDate
                    };
            var selectResult = await x.OrderByDescending(x => x.RecordDate).ToListAsync();
            List<PerformanceInfoDateDto> performanceInfoDateDto = new List<PerformanceInfoDateDto>();

            if (isHistoryConsulationCardConsumed.HasValue)
            {
                performanceInfoDateDto = selectResult.Select(x => new PerformanceInfoDateDto { Date = x.ConsultationDate.Value, PerfomancePrice = x.Price }).ToList();
            }
            if (isConsulationCardRefund.HasValue)
            {
                performanceInfoDateDto = selectResult.Select(x => new PerformanceInfoDateDto { Date = x.RefundDate.Value, PerfomancePrice = x.Price }).ToList();
            }
            if (isHistoryConsulationCardConsumed == null && isConsulationCardRefund == null)
            {

                performanceInfoDateDto = selectResult.Select(x => new PerformanceInfoDateDto { Date = x.RecordDate, PerfomancePrice = x.Price }).ToList();
            }

            var returnResult = performanceInfoDateDto.GroupBy(x => x.Date.Month).Select(x => new PerformanceBrokenLine { Date = x.Key.ToString(), PerfomancePrice = x.Count() }).ToList();
            return returnResult;
        }

        /// <summary>
        /// 根据时间获取下卡，退卡，分诊，加v数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<GetCustomerDataDto> GetCustomerDataAsync(DateTime? startDate, DateTime? endDate)
        {
            var list = await dalShoppingCartRegistration.GetAll()
                .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate.Value.AddDays(1).AddMilliseconds(-1)).ToListAsync();
            GetCustomerDataDto result = new GetCustomerDataDto();
            result.AddCardNum = list.Count();
            result.RefundCardNum = list.Where(x => x.IsReturnBackPrice == true).Count();
            result.DistributeConsulationNum = list.Where(x => x.AssignEmpId.HasValue).Count();
            result.AddWechatNum = list.Where(x => x.IsAddWeChat).Count();
            return result;
        }
        #endregion

        #region 啊美雅运营看板

        public async Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetIndicatorConversionDataAsync(DateTime startDate,DateTime endDate,string baseLiveAnchorId,bool? isEffective) {
            ShoppingCartRegistrationIndicatorBaseDataDto data = new ShoppingCartRegistrationIndicatorBaseDataDto();
            var liveanchorIds =(await _liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e=>e.Id);
            var baseData= dalShoppingCartRegistration.GetAll().Where(e => e.CreateDate >= startDate && e.CreateDate < endDate&&liveanchorIds.Contains(e.LiveAnchorId)&&(!isEffective.HasValue||(isEffective.Value?e.Price>0:e.Price<=0))).Select(e=>new {
                IsSendOrder=e.IsSendOrder,
                Phone=e.Phone,
                CreateDate=e.CreateDate,
                IsAddWeChat = e.IsAddWeChat,             
            }).ToList();
            data.TotalCount = baseData.Count();
            data.SendOrderCount = baseData.Where(e => e.IsSendOrder == true).Count();
            var phoneList = baseData.Select(e => e.Phone).Distinct().ToList();
            var contentOrderList = dalContentPlatformOrder.GetAll().Where(e => phoneList.Contains(e.Phone) && e.CreateDate >= startDate).Select(e => new {
                SendDate=e.SendDate,
                Phone=e.Phone,
                ToHospitalDate=e.ToHospitalDate,
                IsOldCustomer=e.IsOldCustomer,
                Price=e.AddOrderPrice,
                IsToHospital=e.IsToHospital,
                DealPrice=e.ContentPlatformOrderDealInfoList.Where(e=>e.IsDeal==true).Sum(e=>e.Price),
                IsDeal=e.ContentPlatformOrderDealInfoList.Where(e=>e.IsDeal==true).Count()>0,
                IsRePurchase= e.ContentPlatformOrderDealInfoList.Where(e => e.IsDeal == true).Count() > 1,
            });
            data.SevenDaySendOrderCount = (from c in contentOrderList
                                           from s in baseData
                                           where s.Phone == c.Phone && c.SendDate <= s.CreateDate.AddDays(7) select s).Count();                          
            data.FifteenToHospitalCount= (from c in contentOrderList
                                          from s in baseData
                                          where s.Phone == c.Phone && c.ToHospitalDate <= s.CreateDate.AddDays(15)
                                          select s).Count();
            data.OldCustomerCount = contentOrderList.Where(e => e.IsOldCustomer == true).Count();
            data.OldCustomerToHospitalCount = contentOrderList.Where(e => e.IsOldCustomer == true && e.IsToHospital == true).Count();
            data.OldCustomerRepurchase = contentOrderList.Where(e => e.IsOldCustomer == true && e.IsRePurchase == true).Count();
            data.AddWechatCount = baseData.Where(e => e.IsAddWeChat).Count();
            data.ToHospitalCount = contentOrderList.Where(e => e.IsToHospital == true).Count();
            data.NewCustomerCount = contentOrderList.Where(e => e.IsOldCustomer == false).Count();
            data.NewCustomerDealCount = contentOrderList.Where(e => e.IsOldCustomer == false && e.IsDeal == true).Count();
            data.NewCustomerTotalPerformance = contentOrderList.Where(e => e.IsOldCustomer == true).Sum(e => e.DealPrice);
            data.OldCustomerTotalPerformance = contentOrderList.Where(e => e.IsOldCustomer == false).Sum(e => e.DealPrice);
            return data;
        }

        #endregion

        private async Task<CallCenterConfigDto> GetCallCenterConfig()
        {
            var config = await dalConfig.GetAll().SingleOrDefaultAsync();
            return JsonConvert.DeserializeObject<WxAppConfigDto>(config.ConfigJson).CallCenterConfig;
        }

    }
}
