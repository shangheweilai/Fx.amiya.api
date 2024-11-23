using Fx.Amiya.DbModels.Model;
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
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.HospitalBoard;
using Fx.Common.Extensions;

namespace Fx.Amiya.Service
{
    public class ShoppingCartRegistrationService : IShoppingCartRegistrationService
    {
        private readonly IEmployeePerformanceTargetService employeePerformanceTargetService;
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
        private IDalContentPlatformOrderSend dalContentPlatformOrderSend;
        private ILiveAnchorBaseInfoService liveAnchorBaseInfoService;


        public ShoppingCartRegistrationService(IDalShoppingCartRegistration dalShoppingCartRegistration,
            IContentPlatformService contentPlatformService,
            IEmployeePerformanceTargetService employeePerformanceTargetService,
             IMessageNoticeService messageNoticeService,
            IAmiyaEmployeeService amiyaEmployeeService,
            IUnitOfWork unitOfWork,
            ILiveAnchorService liveAnchorService,
             ILiveAnchorWeChatInfoService liveAnchorWeChatInfoService,
            IDalConfig dalConfig,
            IDalAmiyaEmployee dalAmiyaEmployee, IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo, IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo, IDalContentPlatformOrder dalContentPlatformOrder, IDalContentPlatformOrderSend dalContentPlatformOrderSend, ILiveAnchorBaseInfoService liveAnchorBaseInfoService)
        {
            this.dalShoppingCartRegistration = dalShoppingCartRegistration;
            _contentPlatformService = contentPlatformService;
            _liveAnchorService = liveAnchorService;
            this.messageNoticeService = messageNoticeService;
            this.dalConfig = dalConfig;
            this.unitOfWork = unitOfWork;
            this.employeePerformanceTargetService = employeePerformanceTargetService;
            _liveAnchorWeChatInfoService = liveAnchorWeChatInfoService;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.dalAmiyaEmployee = dalAmiyaEmployee;
            this.dalLiveAnchorBaseInfo = dalLiveAnchorBaseInfo;
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            this.dalContentPlatformOrder = dalContentPlatformOrder;
            this.dalContentPlatformOrderSend = dalContentPlatformOrderSend;
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
        }



        public async Task<FxPageInfo<ShoppingCartRegistrationDto>> GetListWithPageAsync(DateTime? startDate, DateTime? endDate, int? LiveAnchorId, bool? isCreateOrder, int? createBy, bool? isSendOrder, int? employeeId, bool? isAddWechat, bool? isWriteOff, bool? isConsultation, bool? isReturnBackPrice, string keyword, string contentPlatFormId, int pageNum, int pageSize, decimal? minPrice, decimal? maxPrice, int? assignEmpId, DateTime? startRefundTime, DateTime? endRefundTime, DateTime? startBadReviewTime, DateTime? endBadReviewTime, int? ShoppingCartRegistrationCustomerType, int? emergencyLevel, bool? isBadReview, string baseLiveAnchorId, int? source, int? belongChannel)
        {
            try
            {
                var config = await GetCallCenterConfig();
                var shoppingCartRegistration = from d in dalShoppingCartRegistration.GetAll()
                                               where (keyword == null || d.Phone.Contains(keyword) || d.SubPhone.Contains(keyword) || d.CustomerNickName.Contains(keyword) || d.LiveAnchorWechatNo.Contains(keyword) || d.Remark.Contains(keyword) || d.Id.Contains(keyword) || d.CustomerWechatNo.Contains(keyword) || d.FromTitle.Contains(keyword))
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
                                               && (!belongChannel.HasValue || d.BelongChannel == belongChannel.Value)
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
                                                   BelongChannel = d.BelongChannel,
                                                   BelongChannelName = ServiceClass.BelongChannelText(d.BelongChannel),
                                                   CluePicture = d.CluePicture,
                                                   AddWechatPicture = d.AddWechatPicture,
                                                   AddWechatEmpId = d.AddWechatEmpId,
                                                   IsRiBuLuoLiving = d.IsRiBuLuoLiving,
                                                   IsHistoryCustomerActive = d.IsHistoryCustomerActive,
                                                   ActiveEmployeeId = d.ActiveEmployeeId,
                                                   CustomerWechatNo = d.CustomerWechatNo,
                                                   FromTitle = d.FromTitle
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
                    if (x.AddWechatEmpId != null)
                    {
                        var empInfo2 = await _amiyaEmployeeService.GetByIdAsync(x.AddWechatEmpId.Value);
                        x.AddWechatEmpName = empInfo2.Name;
                    }
                    if (x.ActiveEmployeeId.HasValue)
                    {
                        var activeEmployeeInfo = await _amiyaEmployeeService.GetByIdAsync(x.ActiveEmployeeId.Value);
                        x.ActiveEmployeeName = activeEmployeeInfo.Name;
                    }
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
                shoppingCartRegistration.BelongChannel = addDto.BelongChannel;
                shoppingCartRegistration.CluePicture = addDto.CluePicture;
                shoppingCartRegistration.AddWechatPicture = addDto.AddWechatPicture;
                shoppingCartRegistration.IsRiBuLuoLiving = addDto.IsRiBuLuoLiving;
                shoppingCartRegistration.IsHistoryCustomerActive = false;
                shoppingCartRegistration.CustomerWechatNo = addDto.CustomerWechatNo;
                shoppingCartRegistration.FromTitle = addDto.FromTitle;
                var baseLiveAnchorId = await _liveAnchorService.GetByIdAsync(addDto.LiveAnchorId);
                if (!string.IsNullOrEmpty(baseLiveAnchorId.LiveAnchorBaseId))
                {
                    shoppingCartRegistration.BaseLiveAnchorId = baseLiveAnchorId.LiveAnchorBaseId;
                }
                if (addDto.Phone != "00000000000")
                {

                    var isExistPhone = await this.GetByPhoneAsync(addDto.Phone);
                    if (isExistPhone.Count() > 0 && isExistPhone.Where(z => z.BaseLiveAnchorId == baseLiveAnchorId.LiveAnchorBaseId).Count() > 0)
                    {
                        throw new Exception("已存在该客户手机号" + addDto.Phone + "，无法录入，请重新填写！");
                    }
                }
                //添加加v人员
                if (shoppingCartRegistration.IsAddWeChat == true)
                {
                    shoppingCartRegistration.AddWechatEmpId = addDto.CreateBy;
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
            string repeatePhone = "";
            unitOfWork.BeginTransaction();
            try
            {
                foreach (var addDto in addDtoList)
                {
                    var searchDataByPhone = await dalShoppingCartRegistration.GetAll().FirstOrDefaultAsync(e => e.Phone == addDto.Phone);
                    if (searchDataByPhone != null&& addDto.Phone != "00000000000")
                    {
                        repeatePhone += addDto.Phone + ",";
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
                    shoppingCartRegistration.GetCustomerType = addDto.GetCustomerType;
                    shoppingCartRegistration.Source = addDto.Source;
                    shoppingCartRegistration.BelongChannel = addDto.BelongChannel;
                    shoppingCartRegistration.IsRiBuLuoLiving = addDto.IsRiBuLuoLiving;
                    shoppingCartRegistration.IsHistoryCustomerActive = false;
                    shoppingCartRegistration.FromTitle = addDto.FromTitle;
                    shoppingCartRegistration.CustomerWechatNo = addDto.CustomerWechatNo;
                    shoppingCartRegistration.ProductType = addDto.ProductType;
                    shoppingCartRegistration.CluePicture = addDto.CluePicture;
                    shoppingCartRegistration.AddWechatPicture = addDto.AddWechatPicture;
                    var baseLiveAnchorId = await _liveAnchorService.GetByIdAsync(addDto.LiveAnchorId);
                    if (!string.IsNullOrEmpty(baseLiveAnchorId.LiveAnchorBaseId))
                    {
                        shoppingCartRegistration.BaseLiveAnchorId = baseLiveAnchorId.LiveAnchorBaseId;
                    }
                    if (addDto.Phone != "00000000000")
                    {
                        var isExistPhone = await this.GetByPhoneAsync(addDto.Phone);
                        if (isExistPhone.Count() > 0 && isExistPhone.Where(z => z.BaseLiveAnchorId == baseLiveAnchorId.LiveAnchorBaseId).Count() > 0)
                        {
                            throw new Exception("已存在该客户手机号" + addDto.Phone + "，无法录入，请重新填写！");
                        }
                    }
                    //添加加v人员
                    if (shoppingCartRegistration.IsAddWeChat == true)
                    {
                        shoppingCartRegistration.AddWechatEmpId = addDto.CreateBy;
                    }
                    await dalShoppingCartRegistration.AddAsync(shoppingCartRegistration, true);
                    await Task.Delay(1);
                }
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (!string.IsNullOrEmpty(repeatePhone))
                {
                    throw new Exception("导入成功，存在部分重复手机号如下：" + repeatePhone + "请确认这些手机号是否为复购顾客！");
                }
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
                shoppingCartRegistrationDto.BelongChannel = shoppingCartRegistration.BelongChannel;
                shoppingCartRegistrationDto.BelongChannelName = ServiceClass.BelongChannelText(shoppingCartRegistrationDto.BelongChannel);
                shoppingCartRegistrationDto.AddWechatPicture = shoppingCartRegistration.AddWechatPicture;
                shoppingCartRegistrationDto.CluePicture = shoppingCartRegistration.CluePicture;
                shoppingCartRegistrationDto.IsRiBuLuoLiving = shoppingCartRegistration.IsRiBuLuoLiving;
                shoppingCartRegistrationDto.IsHistoryCustomerActive = shoppingCartRegistration.IsHistoryCustomerActive;
                shoppingCartRegistrationDto.ActiveEmployeeId = shoppingCartRegistration.ActiveEmployeeId;
                shoppingCartRegistrationDto.CustomerWechatNo = shoppingCartRegistration.CustomerWechatNo;
                shoppingCartRegistrationDto.FromTitle = shoppingCartRegistration.FromTitle;

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
                shoppingCartRegistrationDto.BelongChannel = shoppingCartRegistration.BelongChannel;
                shoppingCartRegistrationDto.BelongChannelName = ServiceClass.BelongChannelText(shoppingCartRegistrationDto.BelongChannel);
                shoppingCartRegistrationDto.IsRiBuLuoLiving = shoppingCartRegistration.IsRiBuLuoLiving;
                shoppingCartRegistrationDto.IsHistoryCustomerActive = shoppingCartRegistration.IsHistoryCustomerActive;
                shoppingCartRegistrationDto.ActiveEmployeeId = shoppingCartRegistration.ActiveEmployeeId;
                shoppingCartRegistrationDto.CustomerWechatNo = shoppingCartRegistration.CustomerWechatNo;
                shoppingCartRegistrationDto.FromTitle = shoppingCartRegistration.FromTitle;
                return shoppingCartRegistrationDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<ShoppingCartRegistrationDto> GetAddOrderPriceByPhoneAndLiveAnchorIdAsync(string phone, int liveAnchorId)
        {
            try
            {
                var liveAnchorInfo = await _liveAnchorService.GetByIdAsync(liveAnchorId);
                if (liveAnchorInfo == null)
                {
                    return new ShoppingCartRegistrationDto();
                }
                var shoppingCartRegistration = dalShoppingCartRegistration.GetAll().Where(k => k.BaseLiveAnchorId == liveAnchorInfo.LiveAnchorBaseId).Where(e => e.Phone == phone || e.SubPhone == phone).OrderByDescending(k => k.CreateDate).FirstOrDefault();
                if (shoppingCartRegistration == null)
                {
                    return new ShoppingCartRegistrationDto();
                }

                ShoppingCartRegistrationDto shoppingCartRegistrationDto = new ShoppingCartRegistrationDto();
                shoppingCartRegistrationDto.Id = shoppingCartRegistration.Id;
                shoppingCartRegistrationDto.Price = shoppingCartRegistration.Price;
                return shoppingCartRegistrationDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<ShoppingCartRegistrationDto>> GetByPhoneAsync(string phone)
        {
            try
            {
                List<ShoppingCartRegistrationDto> result = new List<ShoppingCartRegistrationDto>();
                var shoppingCartRegistrationResult = await dalShoppingCartRegistration.GetAll().Where(e => e.Phone == phone || e.SubPhone == phone).OrderByDescending(x => x.CreateDate).ToListAsync();
                if (shoppingCartRegistrationResult == null)
                {
                    return new List<ShoppingCartRegistrationDto>();
                }
                foreach (var shoppingCartRegistration in shoppingCartRegistrationResult)
                {

                    ShoppingCartRegistrationDto shoppingCartRegistrationDto = new ShoppingCartRegistrationDto();
                    shoppingCartRegistrationDto.Id = shoppingCartRegistration.Id;
                    shoppingCartRegistrationDto.RecordDate = shoppingCartRegistration.RecordDate;
                    shoppingCartRegistrationDto.ContentPlatFormId = shoppingCartRegistration.ContentPlatFormId;
                    shoppingCartRegistrationDto.LiveAnchorId = shoppingCartRegistration.LiveAnchorId;
                    shoppingCartRegistrationDto.LiveAnchorWechatNo = shoppingCartRegistration.LiveAnchorWechatNo;
                    shoppingCartRegistrationDto.BaseLiveAnchorId = shoppingCartRegistration.BaseLiveAnchorId;
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
                    shoppingCartRegistrationDto.BelongChannel = shoppingCartRegistrationDto.BelongChannel;
                    shoppingCartRegistrationDto.IsRiBuLuoLiving = shoppingCartRegistration.IsRiBuLuoLiving;
                    shoppingCartRegistrationDto.IsHistoryCustomerActive = shoppingCartRegistration.IsHistoryCustomerActive;
                    shoppingCartRegistrationDto.ActiveEmployeeId = shoppingCartRegistration.ActiveEmployeeId;
                    shoppingCartRegistrationDto.CustomerWechatNo = shoppingCartRegistration.CustomerWechatNo;
                    shoppingCartRegistrationDto.FromTitle = shoppingCartRegistration.FromTitle;
                    result.Add(shoppingCartRegistrationDto);
                }
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateAsync(UpdateShoppingCartRegistrationDto updateDto)
        {
            
            var baseLiveAnchorId = await _liveAnchorService.GetByIdAsync(updateDto.LiveAnchorId);
            if (updateDto.Phone != "00000000000")
            {
                var isExistPhone = await this.GetByPhoneAsync(updateDto.Phone);
                if (isExistPhone.Count() > 0 && isExistPhone.Where(z => z.BaseLiveAnchorId == baseLiveAnchorId.LiveAnchorBaseId).Where(x => x.Id != updateDto.Id).Count() > 0)
                {
                    throw new Exception("已存在该客户手机号" + updateDto.Phone + "，无法录入，请重新填写！");
                }
            }
            var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
            if (shoppingCartRegistration == null)
                throw new Exception("小黄车登记编号错误！");
            if (!string.IsNullOrEmpty(baseLiveAnchorId.LiveAnchorBaseId))
            {
                shoppingCartRegistration.BaseLiveAnchorId = baseLiveAnchorId.LiveAnchorBaseId;
            }
            if (!shoppingCartRegistration.IsAddWeChat)
            {
                if (updateDto.IsAddWeChat)
                {
                    shoppingCartRegistration.AddWechatEmpId = updateDto.OperationBy;
                }
            }
            if (updateDto.OperationBy != shoppingCartRegistration.CreateBy)
            {
                shoppingCartRegistration.LiveAnchorWechatNo = updateDto.LiveAnchorWechatNo;
                shoppingCartRegistration.CustomerNickName = updateDto.CustomerNickName;
                shoppingCartRegistration.Phone = updateDto.Phone;
                shoppingCartRegistration.IsAddWeChat = updateDto.IsAddWeChat;
                shoppingCartRegistration.Remark = updateDto.Remark;
                shoppingCartRegistration.CluePicture = updateDto.CluePicture;
                shoppingCartRegistration.AddWechatPicture = updateDto.AddWechatPicture;

                shoppingCartRegistration.IsHistoryCustomerActive = updateDto.IsHistoryCustomerActive;
                shoppingCartRegistration.ActiveEmployeeId = updateDto.ActiveEmployeeId;
                shoppingCartRegistration.CustomerWechatNo = updateDto.CustomerWechatNo;
                shoppingCartRegistration.FromTitle = updateDto.FromTitle;
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
                //shoppingCartRegistration.CreateBy = updateDto.CreateBy;
                shoppingCartRegistration.ProductType = updateDto.ProductType;
                shoppingCartRegistration.BelongChannel = updateDto.BelongChannel;
                shoppingCartRegistration.CluePicture = updateDto.CluePicture;
                shoppingCartRegistration.AddWechatPicture = updateDto.AddWechatPicture;
                shoppingCartRegistration.IsRiBuLuoLiving = updateDto.IsRiBuLuoLiving;
                shoppingCartRegistration.IsHistoryCustomerActive = updateDto.IsHistoryCustomerActive;
                shoppingCartRegistration.ActiveEmployeeId = updateDto.ActiveEmployeeId;
                shoppingCartRegistration.CustomerWechatNo = updateDto.CustomerWechatNo;
                shoppingCartRegistration.FromTitle = updateDto.FromTitle;
                

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

                //动态消息添加提示内容
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
        public List<BaseKeyValueDto<int>> GetCustomerSourceList(string contentPlatFormId, int? channel)
        {
            var emergencyLevels = Enum.GetValues(typeof(TiktokCustomerSource));
            List<BaseKeyValueDto<int>> emergencyLevelList = new List<BaseKeyValueDto<int>>();
            List<GetBaseIdAndNameDictionaryList> resultList = new List<GetBaseIdAndNameDictionaryList>();
            foreach (var item in emergencyLevels)
            {
                GetBaseIdAndNameDictionaryList addRes = new GetBaseIdAndNameDictionaryList();


                addRes.Key = Convert.ToInt32(item);
                addRes.Name = ServiceClass.GetTiktokCustomerSourceText(Convert.ToInt32(item));

                Dictionary<List<string>, List<int>> res = new Dictionary<List<string>, List<int>>();
                List<string> contentPlatFormIds = new List<string>();
                List<int> channels = new List<int>();
                switch (addRes.Key)
                {
                    case 0:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //小红书
                        contentPlatFormIds.Add("317c03b8-aff9-4961-8392-fc44d04b1725");
                        //快手
                        contentPlatFormIds.Add("7a9d6564-e273-4847-9047-e7bbb7c6805c");
                        channels.Add((int)BelongChannel.LiveBefore);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 1:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //快手
                        contentPlatFormIds.Add("7a9d6564-e273-4847-9047-e7bbb7c6805c");
                        //私域
                        contentPlatFormIds.Add("22a0b287-232d-4373-a9dd-c372aaae57dc");
                        //小红书
                        contentPlatFormIds.Add("317c03b8-aff9-4961-8392-fc44d04b1725");
                        channels.Add((int)BelongChannel.Living);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 2:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //快手
                        contentPlatFormIds.Add("7a9d6564-e273-4847-9047-e7bbb7c6805c");
                        channels.Add((int)BelongChannel.LiveAfter);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 3:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //小红书
                        contentPlatFormIds.Add("317c03b8-aff9-4961-8392-fc44d04b1725");
                        //快手
                        contentPlatFormIds.Add("7a9d6564-e273-4847-9047-e7bbb7c6805c");
                        channels.Add((int)BelongChannel.LiveBefore);
                        channels.Add((int)BelongChannel.LiveAfter);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 5:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //啊美雅官网
                        contentPlatFormIds.Add("b0e992ad-1b7b-4397-a5fe-1bfbc1f0130e");
                        //合作达人
                        contentPlatFormIds.Add("d2e71501-7327-4883-9294-371a77c4cabd");
                        //小程序
                        contentPlatFormIds.Add("6725f235-35da-42a6-acdd-46f721dde53a");
                        channels.Add((int)BelongChannel.LiveAfter);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 6:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //小红书
                        contentPlatFormIds.Add("317c03b8-aff9-4961-8392-fc44d04b1725");
                        //快手
                        contentPlatFormIds.Add("7a9d6564-e273-4847-9047-e7bbb7c6805c");
                        channels.Add((int)BelongChannel.LiveAfter);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 7:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        channels.Add((int)BelongChannel.LiveBefore);
                        channels.Add((int)BelongChannel.Living);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 8:
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        channels.Add((int)BelongChannel.Living);
                        res.Add(contentPlatFormIds, channels);
                        break;
                    case 9:
                        //抖音
                        contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //视频号
                        contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //快手
                        contentPlatFormIds.Add("7a9d6564-e273-4847-9047-e7bbb7c6805c");
                        channels.Add((int)BelongChannel.LiveAfter);
                        res.Add(contentPlatFormIds, channels);
                        break;

                    case 10:
                        //私域
                        contentPlatFormIds.Add("22a0b287-232d-4373-a9dd-c372aaae57dc");
                        channels.Add((int)BelongChannel.LiveBefore);
                        channels.Add((int)BelongChannel.Living);
                        channels.Add((int)BelongChannel.LiveAfter);
                        res.Add(contentPlatFormIds, channels);
                        break;
                        //case 11:
                        //    //抖音
                        //    contentPlatFormIds.Add("4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                        //    //视频号
                        //    contentPlatFormIds.Add("9196b247-1ab9-4d0c-a11e-a1ef09019878");
                        //    channels.Add((int)BelongChannel.Living);
                        //    channels.Add((int)BelongChannel.LiveAfter);
                        //    res.Add(contentPlatFormIds, channels);
                        //    break;
                }
                addRes.BaseIdAndName = res;
                resultList.Add(addRes);
            }
            if (channel.HasValue && !string.IsNullOrEmpty(contentPlatFormId))
            {
                foreach (var k in resultList)
                {
                    var existValue = k.BaseIdAndName.Where(x => x.Value.Contains(channel.Value)).ToList();
                    foreach (var y in existValue)
                    {
                        var existKey = y.Key.Where(x => x.Contains(contentPlatFormId)).ToList();
                        if (existKey.Count() > 0)
                        {
                            BaseKeyValueDto<int> emergencyLevelDto = new BaseKeyValueDto<int>();
                            emergencyLevelDto.Key = k.Key;
                            emergencyLevelDto.Value = ServiceClass.GetTiktokCustomerSourceText(emergencyLevelDto.Key);
                            emergencyLevelList.Add(emergencyLevelDto);

                        }
                    }
                }
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
        /// <summary>
        /// 获取归属渠道列表
        /// </summary>
        /// <returns></returns>
        public List<BaseIdAndNameDto<int>> GetBelongDepartmentList()
        {
            var belongDepartments = Enum.GetValues(typeof(BelongChannel));
            List<BaseIdAndNameDto<int>> emergencyLevelList = new List<BaseIdAndNameDto<int>>();
            foreach (var belong in belongDepartments)
            {
                BaseIdAndNameDto<int> item = new BaseIdAndNameDto<int>();
                item.Id = Convert.ToInt32(belong);
                item.Name = ServiceClass.BelongChannelText(item.Id);
                emergencyLevelList.Add(item);
            }
            return emergencyLevelList;
        }
        /// <sum
        /// <summary>
        /// 根据创建人与时间线获取医美/带货客资加v量
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="createBy"></param>
        /// <returns></returns>
        public async Task<GetShoppingCartRegistionAddWechatNumDto> GetShoppingCartRegistionAddWechatNumAsync(QueryAddWeChatDto query)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).SingleOrDefaultAsync(e => e.Id == query.EmployeeId);
                var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll().Where(k => k.BaseLiveAnchorId == employee.LiveAnchorBaseId && k.RecordDate >= query.StartDate && k.RecordDate <= query.EndDate.AddDays(1).AddMilliseconds(-1)).ToListAsync();
                var shoppingCartRegistrationAddWechat = shoppingCartRegistration.Where(k => k.IsAddWeChat == true).ToList();
                GetShoppingCartRegistionAddWechatNumDto result = new GetShoppingCartRegistionAddWechatNumDto();
                result.BeautyCustomerAddWechatNum = shoppingCartRegistrationAddWechat.Where(x => x.ShoppingCartRegistrationCustomerType == (int)ShoppingCartRegistionCustomerSource.AestheticMedicine).Count();
                result.TakeGoodsCustomerAddWechatNum = shoppingCartRegistrationAddWechat.Where(x => x.ShoppingCartRegistrationCustomerType == (int)ShoppingCartRegistionCustomerSource.TakeGoods).Count();
                result.AddWeChatRate = DecimalExtension.CalculateTargetComplete(shoppingCartRegistrationAddWechat.Count(), shoppingCartRegistration.Count());
                result.shoppingCartRegistionAddNumAndCompleteRateDto = new ShoppingCartRegistionAddNumAndCompleteRateDto();
                result.shoppingCartRegistionAddNumAndCompleteRateDto.CreateNum = shoppingCartRegistration.Where(x => x.CreateBy == query.EmployeeId).Count();
                result.shoppingCartRegistionAddNumAndCompleteRateDto.CreateNumTarget = await employeePerformanceTargetService.GetClueTargetByEmpIdAndYearMonthAsync(query.EmployeeId, query.StartDate.Year, query.EndDate.Month);
                result.shoppingCartRegistionAddNumAndCompleteRateDto.CreateNumCompleteRate = DecimalExtension.CalculateTargetComplete(result.shoppingCartRegistionAddNumAndCompleteRateDto.CreateNum, result.shoppingCartRegistionAddNumAndCompleteRateDto.CreateNumTarget);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
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
        /// 获取直播后小黄车相关日运营数据
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <param name="recordDate"></param>
        /// <returns></returns>
        public async Task<AfterLiveDataDto> GetAfterLiveDataByLiveAnchorIdAsync(int liveAnchorId, DateTime recordDate)
        {
            //筛选结束的月份
            DateTime endDate = recordDate.Date.AddDays(1);
            //选定的月份
            DateTime currentDate = recordDate.Date;
            var result = dalShoppingCartRegistration.GetAll()
                .Where(o => o.RecordDate >= currentDate && o.RecordDate < endDate && o.LiveAnchorId == liveAnchorId).Select(e => new
                {
                    IsAddWechat = e.IsAddWeChat,
                    AssignEmpId = e.AssignEmpId
                }).ToList();
            AfterLiveDataDto afterLiveData = new AfterLiveDataDto();
            afterLiveData.ClueCount = result.Count();
            afterLiveData.AddWechatCount = result.Where(e => e.IsAddWechat == true).Count();
            afterLiveData.DistributeConsulation = result.Where(e => e.AssignEmpId != null).Count();
            return afterLiveData;
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
        public async Task<List<ShoppingCartRegistrationDto>> GetNewBaseBusinessPerformanceByLiveAnchorNameAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, string contentPlatFormId, string liveAnchorBaseId)
        {
            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => string.IsNullOrEmpty(contentPlatFormId) || o.ContentPlatFormId == contentPlatFormId)
            .Where(o => string.IsNullOrEmpty(liveAnchorBaseId) || o.BaseLiveAnchorId == liveAnchorBaseId)
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
        /// 根据基础主播获取获取潜在/有效 加v,分诊
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetPerformanceByBaseLiveAnchorIdAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, string baseLiveAnchorId)
        {
            var liveanchorIds = (await _liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id);
            var result = from d in dalShoppingCartRegistration.GetAll()
                         .Where(e => e.AssignEmpId != null && e.RecordDate >= startDate && e.RecordDate < endDate && liveanchorIds.Contains(e.LiveAnchorId))
                         select d;
            if (isEffectiveCustomerData.HasValue)
            {
                result = result.Where(e => !isEffectiveCustomerData.HasValue || (isEffectiveCustomerData.Value ? (e.Price == 29.9m || e.Price == 199m) : (e.Price != 29.9m && e.Price != 199m)));
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        Phone = d.Phone,
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                    };
            return await x.ToListAsync();
        }
        /// <summary>
        /// 根据助理id集合获取获取潜在/有效 加v,分诊
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList)
        {

            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate)
            .Where(o => assistantIdList.Contains(o.AssignEmpId.Value))
                         select d;

            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        Phone = d.Phone,
                        AddPrice = d.Price,
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                        IsSendOrder = d.IsSendOrder,
                        EmergencyLevel = d.EmergencyLevel
                    };
            return await x.ToListAsync();
        }
        /// <summary>
        /// 根据条件获取助理小黄车业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetShopCartRegisterPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, List<int> assistantIdList)
        {

            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate)
            .Where(o => assistantIdList.Contains(o.AssignEmpId.Value))
                         select d;

            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                        Price = d.Price
                    };
            if (isEffectiveCustomerData.HasValue)
            {
                x = x.Where(o => isEffectiveCustomerData == true ? o.Price > 0 : o.Price == 0);
            }
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
                .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate.Value).ToListAsync();
            GetCustomerDataDto result = new GetCustomerDataDto();
            result.TotalFlowRateNum = list.Count();
            result.RefundCardNum = list.Where(x => x.IsReturnBackPrice == true).Count();
            result.DistributeConsulationNum = list.Where(x => x.AssignEmpId.HasValue).Count();
            result.AddWechatNum = list.Where(x => x.IsAddWeChat == true).Count();
            return result;
        }
        #endregion

        #region 【啊美雅运营看板】

        public async Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistionDataByRecordDate(DateTime startDate, DateTime endDate, string liveAnchorBaseId)
        {
            var shoppingCartRegistration = await dalShoppingCartRegistration.GetAll()
                                          .Where(d => (d.RecordDate >= startDate.Date && d.RecordDate < endDate))
                                          .Where(x => string.IsNullOrEmpty(liveAnchorBaseId) || x.BaseLiveAnchorId == liveAnchorBaseId)
                                           .Select(d => new ShoppingCartRegistrationDto
                                           {
                                               AddPrice = d.Price,
                                               Phone = d.Phone,
                                               IsReturnBackPrice = d.IsReturnBackPrice,
                                               AssignEmpId = d.AssignEmpId,
                                               IsAddWeChat = d.IsAddWeChat,
                                               ContentPlatFormId = d.ContentPlatFormId,
                                               BaseLiveAnchorId = d.BaseLiveAnchorId,
                                               BelongChannel = d.BelongChannel,
                                               Source = d.Source,
                                               RecordDate = d.RecordDate,
                                               IsSendOrder = d.IsSendOrder,
                                           }).ToListAsync();
            return shoppingCartRegistration;
        }

        /// <summary>
        /// 获取指标转化基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="isEffective"></param>
        /// <returns></returns>
        public async Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetIndicatorConversionDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, bool? isEffective, bool? isCurrentMonth = null)
        {
            ShoppingCartRegistrationIndicatorBaseDataDto data = new ShoppingCartRegistrationIndicatorBaseDataDto();
            var liveanchorIds = (await _liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id);
            var baseData = dalShoppingCartRegistration.GetAll()
                .Where(e => e.AssignEmpId != null)
                .Where(e => e.Phone != "00000000000")
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && liveanchorIds.Contains(e.LiveAnchorId) && (!isEffective.HasValue || (isEffective.Value ? (e.Price == 199m || e.Price == 29.9m) : (e.Price != 199m && e.Price != 29.9m)))).Select(e => new
                {
                    IsSendOrder = e.IsSendOrder,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate.Date,
                    IsAddWeChat = e.IsAddWeChat,
                }).ToList();
            data.TotalCount = baseData.Select(e => e.Phone).Count();
            var phoneList = baseData.Select(e => e.Phone).Distinct().ToList();

            data.SevenDaySendOrderCount = (from c in dalContentPlatformOrder.GetAll()
                                           .Where(e => e.SendDate > startDate && e.SendDate < endDate && e.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                                           .Select(e => new { Phone = e.Phone, SendDate = e.SendDate }).ToList()
                                           from s in baseData
                                           where s.Phone == c.Phone && c.SendDate.Value.Date <= s.RecordDate.AddDays(7)
                                           select s).Select(e => e.Phone).Distinct().Count();
            data.FifteenToHospitalCount = (from c in dalContentPlatFormOrderDealInfo.GetAll()
                                           .Where(e => e.ContentPlatFormOrder.SendDate > startDate && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                                           .Select(e => new
                                           {
                                               Phone = e.ContentPlatFormOrder.Phone,
                                               ToHospitalDate = e.ToHospitalDate,
                                               SendDate = e.ContentPlatFormOrder.SendDate
                                           }).ToList()
                                           from s in baseData
                                           where s.Phone == c.Phone && c.ToHospitalDate <= s.RecordDate.AddDays(15) && c.SendDate >= s.RecordDate
                                           select s).Select(e => e.Phone).Distinct().Count();

            data.SendOrderCount = await dalContentPlatformOrderSend.GetAll()
             .Where(o => o.SendDate >= startDate && o.SendDate < endDate)
             .Where(o => o.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
             .Where(o => o.ContentPlatformOrder.IsOldCustomer == false)
             .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatformOrder.AddOrderPrice == 199m || o.ContentPlatformOrder.AddOrderPrice == 29.9m) : (o.ContentPlatformOrder.AddOrderPrice != 199m && o.ContentPlatformOrder.AddOrderPrice != 29.9m)))
             .Where(e => e.ContentPlatformOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatformOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                .Select(e => e.ContentPlatformOrder.Phone)
                .Distinct()
                .CountAsync();


            data.OldCustomerCountEndLastMonth = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(o => o.CreateDate < startDate)
            .Where(e => e.IsOldCustomer == true)
            .Where(e => e.IsDeal == true)
            .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
            .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
           .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m))).Select(e => e.ContentPlatFormOrder.Phone).Count();
            data.OldCustomerCount = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(e => e.IsOldCustomer == true && e.IsDeal == true)
            .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
             .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
           .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m))).Select(e => e.ContentPlatFormOrder.Phone).Distinct().Count();
            data.NewCustomerCount = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(e => e.IsOldCustomer == false && e.IsDeal == true)
            .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
             .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
           .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m))).Select(e => e.ContentPlatFormOrder.Phone).Distinct().Count();
            data.OldCustomerDealCount = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                 .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                .Where(x => x.CreateDate >= startDate && x.CreateDate < endDate && x.IsOldCustomer == true && x.IsDeal == true)
               .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
                .Select(e => e.ContentPlatFormOrder.Phone)
                .Distinct()
                .Count();
            data.OldCustomerTotalPerformance = dalContentPlatFormOrderDealInfo.GetAll()
                 .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                .Where(x => x.CreateDate >= startDate && x.CreateDate < endDate && x.IsOldCustomer == true && x.IsDeal == true)
              .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
                .Sum(x => x.Price);
            var newVisitCount = dalContentPlatFormOrderDealInfo.GetAll()
             .Where(o => o.CreateDate >= startDate && o.CreateDate < endDate)
              .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
             .Where(e => e.IsOldCustomer == false)
            .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
             .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId);
            data.NewCustomerToHospitalCount = newVisitCount
                .Where(e => e.IsToHospital == true)
                .Select(e => e.ContentPlatFormOrder.Phone)
                .Distinct()
                .Count();
            data.NewCustomerDealCount = dalContentPlatFormOrderDealInfo.GetAll()
                 .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                .Where(x => x.CreateDate >= startDate && x.CreateDate < endDate && x.IsOldCustomer == false && x.IsDeal == true)
                .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
                .Select(e => e.ContentPlatFormOrder.Phone)
                .Distinct()
                .Count();
            data.NewCustomerTotalPerformance = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                 .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                .Where(x => x.CreateDate >= startDate && x.CreateDate < endDate && x.IsOldCustomer == false && x.IsDeal == true)
                .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
                .Sum(x => x.Price);
            data.AddWechatCount = baseData.Where(e => e.IsAddWeChat == true).Select(e => e.Phone).Count();
            data.ToHospitalCount = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                .Where(e => e.ContentPlatFormOrder.IsOldCustomer == false)
                 .Where(e => e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                .Where(o => !isEffective.HasValue || (isEffective.Value ? (o.ContentPlatFormOrder.AddOrderPrice == 199m || o.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (o.ContentPlatFormOrder.AddOrderPrice != 199m && o.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
                .Where(x => x.CreateDate >= startDate && x.CreateDate < endDate && x.IsToHospital == true)
                .Select(e => e.ContentPlatFormOrder.Phone)
                .Distinct()
                .Count();
            return data;
        }
        /// <summary>
        /// 获取分诊新客转化情况基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="isEffective"></param>
        /// <param name="isOldCustomer"></param>
        /// <returns></returns>
        public async Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetCurrentMonthNewCustomerConversionDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, bool? isEffective, bool isOldCustomer)
        {
            ShoppingCartRegistrationIndicatorBaseDataDto data = new ShoppingCartRegistrationIndicatorBaseDataDto();
            var liveanchorIds = (await _liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id);
            var assistantNameList = (await _amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(new List<string> { baseLiveAnchorId })).Select(e => e.Id).ToList();
            var baseData = dalShoppingCartRegistration.GetAll()
                 .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.Price == 29.9m || e.Price == 199m) : (e.Price != 29.9m && e.Price != 199m)))
                .Where(e => e.AssignEmpId != null && e.RecordDate >= startDate && e.RecordDate < endDate && liveanchorIds.Contains(e.LiveAnchorId))
                .Select(e => new
                {
                    IsSendOrder = e.IsSendOrder,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate,
                    IsAddWeChat = e.IsAddWeChat,
                }).ToList();
            var phoneList = baseData.Select(e => e.Phone).Distinct().ToList();
            var sendC = dalContentPlatformOrder.GetAll()
              .Where(o => o.SendDate >= startDate && o.SendDate < endDate)
              .Where(e => e.IsOldCustomer == isOldCustomer)
              .Where(o => o.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
              .Where(o => phoneList.Contains(o.Phone))
               .Select(o => o.Phone)
              .Where(o => phoneList.Contains(o))
              .Distinct().Count();
            data.TotalCount = baseData.Select(e => e.Phone).Count();
            data.SendOrderCount = sendC;

            var contentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => phoneList.Contains(e.ContentPlatFormOrder.Phone) && e.CreateDate >= startDate && e.CreateDate < endDate && e.IsOldCustomer == isOldCustomer)
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    IsToHospital = e.IsToHospital,
                    DealPrice = e.Price,
                    IsDeal = e.IsDeal,
                }).ToList();
            data.AddWechatCount = baseData.Where(e => e.IsAddWeChat).Select(e => e.Phone).Count();
            data.ToHospitalCount = contentOrderList.Where(e => e.IsToHospital == true).Select(e => e.Phone).Distinct().Count();
            data.NewCustomerCount = contentOrderList.Select(e => e.Phone).Distinct().Count();
            data.NewCustomerDealCount = contentOrderList.Where(e => e.IsDeal == true).Select(e => e.Phone).Distinct().Count();
            data.NewCustomerTotalPerformance = contentOrderList.Sum(e => e.DealPrice);
            return data;
        }
        /// <summary>
        /// 根据助理id获取指标转化情况
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="isEffective"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationIndicatorBaseDataDto>> GetIndicatorConversionDataByAssistantIdsAsync(DateTime startDate, DateTime endDate, List<int> assistantIds, bool? isEffective)
        {

            ShoppingCartRegistrationIndicatorBaseDataDto data = new ShoppingCartRegistrationIndicatorBaseDataDto();
            var baseData = dalShoppingCartRegistration.GetAll()
                .Where(e => e.AssignEmpId != null)
                .Where(e => assistantIds.Contains(e.AssignEmpId.Value))
                .Where(e => e.Phone != "00000000000")
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && (!isEffective.HasValue || (isEffective.Value ? (e.Price == 199m || e.Price == 29.9m) : (e.Price != 199m && e.Price != 29.9m)))).Select(e => new
                {
                    AssignEmpId = e.AssignEmpId,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate.Date,
                    IsAddWeChat = e.IsAddWeChat,
                }).ToList();
            var sevenDaySendOrderData = (from c in dalContentPlatformOrder.GetAll()
                                             .Where(e => e.SendDate > startDate && e.SendDate < endDate && e.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                                             .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.AddOrderPrice == 199m || e.AddOrderPrice == 29.9m) : (e.AddOrderPrice != 199m && e.AddOrderPrice != 29.9m)))
                                             .Select(e => new { Phone = e.Phone, SendDate = e.SendDate, AssignEmpId = e.IsSupportOrder ? e.SupportEmpId : e.BelongEmpId }).ToList()
                                         from s in baseData
                                         where s.Phone == c.Phone && c.SendDate.Value.Date <= s.RecordDate.AddDays(7)
                                         select new { Phone = s.Phone, AssignEmpId = c.AssignEmpId }).ToList();
            var fifteenToHospitalData = (from c in dalContentPlatFormOrderDealInfo.GetAll()
                                             .Where(e => e.ContentPlatFormOrder.SendDate > startDate && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder && e.ContentPlatFormOrder.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                                             .Select(e => new
                                             {
                                                 Phone = e.ContentPlatFormOrder.Phone,
                                                 ToHospitalDate = e.ToHospitalDate,
                                                 SendDate = e.ContentPlatFormOrder.SendDate,
                                                 AssignEmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId
                                             }).ToList()
                                         from s in baseData
                                         where s.Phone == c.Phone && c.ToHospitalDate <= s.RecordDate.AddDays(15) && c.SendDate >= s.RecordDate
                                         select new { Phone = s.Phone, AssignEmpId = c.AssignEmpId });
            var sendOrderData = dalContentPlatformOrderSend.GetAll()
             .Where(o => o.SendDate >= startDate && o.SendDate < endDate)
             .Where(o => o.ContentPlatformOrder.IsOldCustomer == false)
             .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatformOrder.AddOrderPrice == 199m || e.ContentPlatformOrder.AddOrderPrice == 29.9m) : (e.ContentPlatformOrder.AddOrderPrice != 199m && e.ContentPlatformOrder.AddOrderPrice != 29.9m)))
             .Where(o => (o.ContentPlatformOrder.IsSupportOrder == false && assistantIds.Contains(o.ContentPlatformOrder.BelongEmpId.Value)) || o.ContentPlatformOrder.IsSupportOrder == true && assistantIds.Contains(o.ContentPlatformOrder.SupportEmpId))
             .Select(e => new { Phone = e.ContentPlatformOrder.Phone, AssignEmpId = e.ContentPlatformOrder.IsSupportOrder ? e.ContentPlatformOrder.SupportEmpId : e.ContentPlatformOrder.BelongEmpId }).ToList();
            var visitData = dalContentPlatFormOrderDealInfo.GetAll()
             .Where(o => (o.CreateDate >= startDate && o.CreateDate < endDate))
             .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == false && assistantIds.Contains(o.ContentPlatFormOrder.BelongEmpId.Value)) || o.ContentPlatFormOrder.IsSupportOrder == true && assistantIds.Contains(o.ContentPlatFormOrder.SupportEmpId))
             .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
             .Select(e => new
             {
                 Phone = e.ContentPlatFormOrder.Phone,
                 IsDeal = e.IsDeal,
                 IsToHospital = e.IsToHospital,
                 ToHospitalDate = e.ToHospitalDate,
                 DealPrice = e.Price,
                 IsOldCustomer = e.IsOldCustomer,
                 AssignEmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                 CreateDate = e.CreateDate
             }).ToList();
            var oldCustomerCountEndLastMonthData = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(o => o.CreateDate < startDate)
            .Where(e => e.IsOldCustomer == true)
            .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == false && assistantIds.Contains(o.ContentPlatFormOrder.BelongEmpId.Value)))
            .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
            .Select(o => new { BelongEmpId = o.ContentPlatFormOrder.BelongEmpId.Value, Phone = o.ContentPlatFormOrder.Phone })
            .Distinct()
            .GroupBy(e => e.BelongEmpId)
            .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto { EmpId = e.Key, OldCustomerCountEndLastMonth = e.Count() }).ToList();
            var supportOldCustomerCountEndLastMonthData = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(o => o.CreateDate < startDate)
            .Where(e => e.IsOldCustomer == true)
            .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == true && assistantIds.Contains(o.ContentPlatFormOrder.SupportEmpId)))
            .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
            .Select(o => new { BelongEmpId = o.ContentPlatFormOrder.BelongEmpId.Value, Phone = o.ContentPlatFormOrder.Phone })
            .Distinct()
            .GroupBy(e => e.BelongEmpId)
            .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto { EmpId = e.Key, OldCustomerCountEndLastMonth = e.Count() }).ToList();
            oldCustomerCountEndLastMonthData.AddRange(supportOldCustomerCountEndLastMonthData);
            var oldCustomerCountData = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(e => e.IsOldCustomer == true && e.IsDeal == true)
            .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == false && assistantIds.Contains(o.ContentPlatFormOrder.BelongEmpId.Value)))
             .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
            .Select(o => new { BelongEmpId = o.ContentPlatFormOrder.BelongEmpId.Value, Phone = o.ContentPlatFormOrder.Phone })
            .Distinct()
            .GroupBy(e => e.BelongEmpId)
            .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto { EmpId = e.Key, OldCustomerCount = e.Count() }).ToList();
            var supportOldCustomerCountData = dalContentPlatFormOrderDealInfo.GetAll()
           .Where(e => e.IsOldCustomer == true)
           .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == true && assistantIds.Contains(o.ContentPlatFormOrder.SupportEmpId)))
            .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
           .Select(o => new { BelongEmpId = o.ContentPlatFormOrder.SupportEmpId, Phone = o.ContentPlatFormOrder.Phone })
           .Distinct()
           .GroupBy(e => e.BelongEmpId)
           .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto { EmpId = e.Key, OldCustomerCount = e.Count() }).ToList();
            oldCustomerCountData.AddRange(supportOldCustomerCountData);
            var newCustomerCountData = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(e => e.IsOldCustomer == false && e.IsDeal == true)
            .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == false && assistantIds.Contains(o.ContentPlatFormOrder.BelongEmpId.Value)))
             .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
            .Select(o => new { BelongEmpId = o.ContentPlatFormOrder.BelongEmpId.Value, Phone = o.ContentPlatFormOrder.Phone })
            .Distinct()
            .GroupBy(e => e.BelongEmpId)
            .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto { EmpId = e.Key, NewCustomerCount = e.Count() }).ToList();
            var supportnewCustomerCountData = dalContentPlatFormOrderDealInfo.GetAll()
            .Where(e => e.IsOldCustomer == false && e.IsDeal == true)
            .Where(o => (o.ContentPlatFormOrder.IsSupportOrder == true && assistantIds.Contains(o.ContentPlatFormOrder.SupportEmpId)))
             .Where(e => !isEffective.HasValue || (isEffective.Value ? (e.ContentPlatFormOrder.AddOrderPrice == 199m || e.ContentPlatFormOrder.AddOrderPrice == 29.9m) : (e.ContentPlatFormOrder.AddOrderPrice != 199m && e.ContentPlatFormOrder.AddOrderPrice != 29.9m)))
            .Select(o => new { BelongEmpId = o.ContentPlatFormOrder.SupportEmpId, Phone = o.ContentPlatFormOrder.Phone })
            .Distinct()
            .GroupBy(e => e.BelongEmpId)
            .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto { EmpId = e.Key, NewCustomerCount = e.Count() }).ToList();
            newCustomerCountData.AddRange(supportnewCustomerCountData);
            var list1 = baseData.GroupBy(e => e.AssignEmpId)
                .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
                {
                    EmpId = e.Key ?? 0,
                    TotalCount = e.Select(e => e.Phone).Count(),
                    AddWechatCount = e.Where(e => e.IsAddWeChat).Select(e => e.Phone).Count(),
                }).ToList();
            var list2 = sevenDaySendOrderData.GroupBy(e => e.AssignEmpId)
                .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
                {
                    EmpId = e.Key ?? 0,
                    SevenDaySendOrderCount = e.Select(e => e.Phone).Distinct().Count()
                }).ToList();
            var list3 = fifteenToHospitalData.GroupBy(e => e.AssignEmpId)
                .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
                {
                    EmpId = e.Key ?? 0,
                    FifteenToHospitalCount = e.Select(e => e.Phone).Distinct().Count()
                });
            var list4 = sendOrderData.GroupBy(e => e.AssignEmpId)
                .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
                {
                    EmpId = e.Key ?? 0,
                    SendOrderCount = e.Select(e => e.Phone).Distinct().Count()
                });
            var list5 = visitData.GroupBy(e => e.AssignEmpId)
                .Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
                {
                    EmpId = e.Key ?? 0,
                    OldCustomerDealCount = e.Where(e => e.IsOldCustomer == true && e.IsDeal == true).Select(e => e.Phone).Distinct().Count(),
                    OldCustomerTotalPerformance = e.Where(e => e.IsOldCustomer == true && e.IsDeal == true).Sum(e => e.DealPrice),
                    NewCustomerDealCount = e.Where(e => e.IsOldCustomer == false && e.IsDeal == true).Select(e => e.Phone).Distinct().Count(),
                    NewCustomerToHospitalCount = e.Where(e => e.IsToHospital == true && e.IsOldCustomer == false).Select(e => e.Phone).Distinct().Count(),
                    NewCustomerTotalPerformance = e.Where(e => e.IsOldCustomer == false && e.IsDeal == true).Sum(e => e.DealPrice),
                    ToHospitalCount = e.Where(e => e.IsToHospital == true && e.IsOldCustomer == false).Select(e => e.Phone).Distinct().Count(),
                });
            List<ShoppingCartRegistrationIndicatorBaseDataDto> list = new List<ShoppingCartRegistrationIndicatorBaseDataDto>();
            list.AddRange(list1);
            list.AddRange(list2);
            list.AddRange(list3);
            list.AddRange(list4);
            list.AddRange(list5);
            list.AddRange(oldCustomerCountEndLastMonthData);
            list.AddRange(oldCustomerCountData);
            list.AddRange(newCustomerCountData);
            var aggregateData = list.GroupBy(e => e.EmpId).Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
            {
                EmpId = e.Key,
                TotalCount = e.Sum(e => e.TotalCount),
                AddWechatCount = e.Sum(e => e.AddWechatCount),
                SevenDaySendOrderCount = e.Sum(e => e.SevenDaySendOrderCount),
                FifteenToHospitalCount = e.Sum(e => e.FifteenToHospitalCount),
                SendOrderCount = e.Sum(e => e.SendOrderCount),
                OldCustomerCount = e.Sum(e => e.OldCustomerCount),
                OldCustomerCountEndLastMonth = e.Sum(e => e.OldCustomerCountEndLastMonth),
                OldCustomerDealCount = e.Sum(e => e.OldCustomerDealCount),
                NewCustomerCount = e.Sum(e => e.NewCustomerCount),
                OldCustomerTotalPerformance = e.Sum(e => e.OldCustomerTotalPerformance),
                NewCustomerDealCount = e.Sum(e => e.NewCustomerDealCount),
                NewCustomerTotalPerformance = e.Sum(e => e.NewCustomerTotalPerformance),
                ToHospitalCount = e.Sum(e => e.ToHospitalCount),
                NewCustomerToHospitalCount = e.Sum(e => e.NewCustomerToHospitalCount),
            });

            return aggregateData.ToList();

        }
        /// <summary>
        /// 获取历史新客转化情况
        /// </summary>
        /// <param name="startData"></param>
        /// <param name="endDate"></param>
        /// <param name="isOldCustomer"></param>
        /// <returns></returns>
        public async Task<CompanyNewCustomerConversionBaseDataDto> GetHistoryNewCustomerConversionDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, bool? isOldCustomer, bool? isEffective)
        {
            ShoppingCartRegistrationIndicatorBaseDataDto data = new ShoppingCartRegistrationIndicatorBaseDataDto();
            var liveanchorIds = (await _liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id);
            var basePhoneData = dalShoppingCartRegistration.GetAll()
                .Where(e => e.AssignEmpId != null && e.RecordDate >= startDate && e.RecordDate < endDate && liveanchorIds.Contains(e.LiveAnchorId)).Select(e => new
                {
                    Phone = e.Phone,
                }).ToList();
            var phoneList = basePhoneData.Select(e => e.Phone).Distinct().ToList();
            var sendC = dalContentPlatformOrder.GetAll()
              .Where(o => o.SendDate >= startDate && o.SendDate < endDate)
              .Where(e => e.IsOldCustomer == isOldCustomer)
              .Where(e => !isEffective.HasValue || (isEffective.Value ? e.AddOrderPrice > 0 : e.AddOrderPrice <= 0))
              .Where(o => liveanchorIds.Contains(o.LiveAnchorId.Value))
              .Where(e => !phoneList.Contains(e.Phone))
              .Select(e => e.Phone).Distinct().Count();
            data.SendOrderCount = sendC;
            var contentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                .Where(e => !phoneList.Contains(e.ContentPlatFormOrder.Phone) && e.CreateDate >= startDate && e.CreateDate < endDate && e.IsOldCustomer == isOldCustomer)
                .Where(e => !isEffective.HasValue || (isEffective.Value ? e.ContentPlatFormOrder.AddOrderPrice > 0 : e.ContentPlatFormOrder.AddOrderPrice <= 0))
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    IsToHospital = e.IsToHospital,
                    DealPrice = e.Price,
                    IsDeal = e.IsDeal,
                    ToHospitalDate = e.ToHospitalDate
                }).ToList();
            data.ToHospitalCount = contentOrderList.Where(e => e.IsToHospital == true && e.ToHospitalDate > startDate).Select(e => e.Phone).Distinct().Count();
            data.NewCustomerDealCount = contentOrderList.Where(e => e.IsDeal == true).Select(e => e.Phone).Distinct().Count();
            data.NewCustomerTotalPerformance = contentOrderList.Sum(e => e.DealPrice);
            CompanyNewCustomerConversionBaseDataDto baseData = new CompanyNewCustomerConversionBaseDataDto();
            baseData.SendOrderCount = data.SendOrderCount;
            baseData.ToHospitalCount = data.ToHospitalCount;
            baseData.DealCount = data.NewCustomerDealCount;
            baseData.DealPrice = data.NewCustomerTotalPerformance;
            return baseData;
        }

        #endregion

        #region 转化
        /// <summary>
        /// 获取流量和客户转化基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        public async Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetFlowAndCustomerTransformDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, List<string> contentPlatformIds)
        {
            ShoppingCartRegistrationIndicatorBaseDataDto data = new ShoppingCartRegistrationIndicatorBaseDataDto();
            var liveanchorIds = (await _liveAnchorService.GetAllLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id);
            var assistantNameList = (await _amiyaEmployeeService.GetByLiveAnchorBaseIdListAsync(new List<string> { baseLiveAnchorId })).Select(e => e.Id).ToList();
            var baseData = dalShoppingCartRegistration.GetAll()
                .Where(e => contentPlatformIds == null || contentPlatformIds.Contains(e.ContentPlatFormId))
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && liveanchorIds.Contains(e.LiveAnchorId))
                .Select(e => new
                {
                    AssignEmpId = e.AssignEmpId,
                    IsSendOrder = e.IsSendOrder,
                    Phone = e.Phone,
                    RecordDate = e.RecordDate,
                    IsAddWeChat = e.IsAddWeChat,
                }).ToList();
            var phoneList = baseData.Select(e => e.Phone).ToList();
            //var sendC = dalContentPlatformOrder.GetAll()
            //  .Where(o => contentPlatformIds == null || contentPlatformIds.Contains(o.ContentPlateformId))
            //  .Where(o => o.SendDate >= startDate && o.SendDate < endDate)
            //  .Where(o => o.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
            //  .Where(e => e.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder && e.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder)
            //  .Select(o => o.Phone)
            //  .Distinct()
            //  .Count();
            var sendC = dalContentPlatformOrderSend.GetAll()
              .Where(o => contentPlatformIds == null || contentPlatformIds.Contains(o.ContentPlatformOrder.ContentPlateformId))
              .Where(o => o.SendDate >= startDate && o.SendDate < endDate)
              .Where(o => o.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
              .Where(e => e.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder && e.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder)
              .Where(e => e.IsMainHospital == true)
              .Select(o => o.ContentPlatformOrder.Phone)
              .Distinct()
              .Count();
            data.ClueCount = baseData.Count();
            data.TotalCount = baseData.Where(e => e.AssignEmpId != null).Count();
            data.SendOrderCount = sendC;
            var contentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(e => contentPlatformIds == null || contentPlatformIds.Contains(e.ContentPlatFormOrder.ContentPlateformId))
                .Where(e => e.CreateDate >= startDate && e.CreateDate < endDate)
                .Where(o => o.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId == baseLiveAnchorId)
                .Select(e => new
                {
                    Phone = e.ContentPlatFormOrder.Phone,
                    IsToHospital = (e.IsOldCustomer == false && e.IsToHospital == true) ? true : false,
                    RealToHospital = e.IsToHospital,
                    DealPrice = e.Price,
                    IsDeal = e.IsDeal,
                    IsOldCustomer = e.IsOldCustomer
                }).ToList();
            data.AddWechatCount = baseData.Where(e => e.IsAddWeChat).Count();
            data.ToHospitalCount = contentOrderList.Where(e => e.IsToHospital == true).Select(e => e.Phone).Distinct().Count();
            data.OldCustomerDealCount = contentOrderList.Where(e => e.IsDeal == true && e.IsOldCustomer == true && e.RealToHospital == true).Select(e => e.Phone).Distinct().Count();
            data.NewCustomerDealCount = contentOrderList.Where(e => e.IsDeal == true && e.IsOldCustomer == false && e.RealToHospital == true).Select(e => e.Phone).Distinct().Count();
            data.NewCustomerTotalPerformance = contentOrderList.Where(e => e.IsOldCustomer == false).Sum(e => e.DealPrice);
            data.OldCustomerTotalPerformance = contentOrderList.Where(e => e.IsOldCustomer == true).Sum(e => e.DealPrice);
            return data;
        }

        /// <summary>
        /// 获取助理流量和客户转化基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationIndicatorBaseDataDto>> GetAssitantFlowAndCustomerTransformDataAsync(DateTime startDate, DateTime endDate, bool? isCurrentMonth, string baseLiveAnchorId, List<string> contentPlatformIds)
        {
            var nameList = await liveAnchorBaseInfoService.GetValidAsync(true);
            var liveanchorIds = new List<string>();
            if (string.IsNullOrEmpty(baseLiveAnchorId))
            {
                liveanchorIds = nameList.Where(e => e.LiveAnchorName.Contains("刀刀") || e.LiveAnchorName.Contains("吉娜") || e.LiveAnchorName.Contains("璐璐")).Select(e => e.Id).ToList();
            }
            else
            {
                liveanchorIds = new List<string>() { baseLiveAnchorId };
            }
            var baseData = dalShoppingCartRegistration.GetAll()
               .Where(e => contentPlatformIds == null || contentPlatformIds.Contains(e.ContentPlatFormId))
               .Where(e => e.AssignEmpId != null && e.RecordDate >= startDate && e.RecordDate < endDate && liveanchorIds.Contains(e.BaseLiveAnchorId))
               .Select(e => new
               {
                   AssignEmpId = e.AssignEmpId,
                   IsSendOrder = e.IsSendOrder,
                   Phone = e.Phone,
                   RecordDate = e.RecordDate,
                   IsAddWeChat = e.IsAddWeChat,
               }).ToList();
            var phoneList = baseData.Select(e => e.Phone).ToList();
            var sendData = dalContentPlatformOrderSend.GetAll()
              .Where(o => contentPlatformIds == null || contentPlatformIds.Contains(o.ContentPlatformOrder.ContentPlateformId))
              .Where(o => o.SendDate >= startDate && o.SendDate < endDate && liveanchorIds.Contains(o.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId))
              .Where(e => e.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder && e.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder)
              .Where(e => e.IsMainHospital == true)
              .Select(e => new
              {
                  AssignEmpId = e.ContentPlatformOrder.IsSupportOrder ? e.ContentPlatformOrder.SupportEmpId : e.ContentPlatformOrder.BelongEmpId,
                  Phone = e.ContentPlatformOrder.Phone
              }).ToList();
            var contentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
                .Where(o => contentPlatformIds == null || contentPlatformIds.Contains(o.ContentPlatFormOrder.ContentPlateformId))
                .Where(e => e.CreateDate >= startDate && e.CreateDate < endDate && liveanchorIds.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new
                {
                    AssignEmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
                    Phone = e.ContentPlatFormOrder.Phone,
                    IsToHospital = (e.IsOldCustomer == false && e.IsToHospital == true) ? true : false,
                    RealToHospital = e.IsToHospital,
                    DealPrice = e.Price,
                    IsDeal = e.IsDeal,
                    IsOldCustomer = e.IsOldCustomer
                }).ToList();
            //var sendData = dalContentPlatformOrderSend.GetAll()
            //  .Where(o => !isCurrentMonth.HasValue || (isCurrentMonth.Value ? phoneList.Contains(o.ContentPlatformOrder.Phone): !phoneList.Contains(o.ContentPlatformOrder.Phone)))
            //  .Where(o => contentPlatformIds == null || contentPlatformIds.Contains(o.ContentPlatformOrder.ContentPlateformId))
            //  .Where(o => o.SendDate >= startDate && o.SendDate < endDate && liveanchorIds.Contains(o.ContentPlatformOrder.LiveAnchor.LiveAnchorBaseId))
            //  .Where(e => e.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder && e.OrderStatus != (int)ContentPlateFormOrderStatus.RepeatOrder)
            //  .Where(e => e.IsMainHospital == true)
            //  .Select(e => new
            //  {
            //      AssignEmpId = e.ContentPlatformOrder.IsSupportOrder ? e.ContentPlatformOrder.SupportEmpId : e.ContentPlatformOrder.BelongEmpId,
            //      Phone = e.ContentPlatformOrder.Phone
            //  }).ToList();
            //var contentOrderList = dalContentPlatFormOrderDealInfo.GetAll()
            //    .Where(o => !isCurrentMonth.HasValue || (isCurrentMonth.Value ? phoneList.Contains(o.ContentPlatFormOrder.Phone) : !phoneList.Contains(o.ContentPlatFormOrder.Phone)))
            //    .Where(o => contentPlatformIds == null || contentPlatformIds.Contains(o.ContentPlatFormOrder.ContentPlateformId))
            //    .Where(e => e.CreateDate >= startDate && e.CreateDate < endDate && liveanchorIds.Contains(e.ContentPlatFormOrder.LiveAnchor.LiveAnchorBaseId))
            //    .Select(e => new
            //    {
            //        AssignEmpId = e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId : e.ContentPlatFormOrder.BelongEmpId,
            //        Phone = e.ContentPlatFormOrder.Phone,
            //        IsToHospital = (e.IsOldCustomer == false && e.IsToHospital == true) ? true : false,
            //        RealToHospital = e.IsToHospital,
            //        DealPrice = e.Price,
            //        IsDeal = e.IsDeal,
            //        IsOldCustomer = e.IsOldCustomer
            //    }).ToList();
            var list1 = baseData.GroupBy(e => e.AssignEmpId).Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
            {
                EmpId = e.Key.Value,
                TotalCount = e.Count(),
                AddWechatCount = e.Where(e => e.IsAddWeChat == true).Count()
            });
            var list2 = sendData.GroupBy(e => e.AssignEmpId).Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
            {
                EmpId = e.Key.Value,
                SendOrderCount = e.Select(e => e.Phone).Distinct().Count()
            });
            var list3 = contentOrderList.GroupBy(e => e.AssignEmpId).Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
            {
                EmpId = e.Key.Value,
                ToHospitalCount = e.Where(e => e.IsToHospital == true).Select(e => e.Phone).Distinct().Count(),
                OldCustomerDealCount = e.Where(e => e.IsDeal == true && e.IsOldCustomer == true && e.RealToHospital == true).Select(e => e.Phone).Distinct().Count(),
                NewCustomerDealCount = e.Where(e => e.IsDeal == true && e.IsOldCustomer == false && e.RealToHospital == true).Select(e => e.Phone).Distinct().Count(),
                NewCustomerTotalPerformance = e.Where(e => e.IsOldCustomer == false).Sum(e => e.DealPrice),
                OldCustomerTotalPerformance = e.Where(e => e.IsOldCustomer == true).Sum(e => e.DealPrice)
            });
            var unionList = list1.Concat(list2).Concat(list3);
            return unionList.GroupBy(e => e.EmpId).Select(e => new ShoppingCartRegistrationIndicatorBaseDataDto
            {
                EmpId = e.Key,
                TotalCount = e.Sum(e => e.TotalCount),
                AddWechatCount = e.Sum(e => e.AddWechatCount),
                SendOrderCount = e.Sum(e => e.SendOrderCount),
                ToHospitalCount = e.Sum(e => e.ToHospitalCount),
                OldCustomerDealCount = e.Sum(e => e.OldCustomerDealCount),
                NewCustomerDealCount = e.Sum(e => e.NewCustomerDealCount),
                NewCustomerTotalPerformance = e.Sum(e => e.NewCustomerTotalPerformance),
                OldCustomerTotalPerformance = e.Sum(e => e.OldCustomerTotalPerformance)
            }).ToList();

        }

        #endregion

        private async Task<CallCenterConfigDto> GetCallCenterConfig()
        {
            var config = await dalConfig.GetAll().SingleOrDefaultAsync();
            return JsonConvert.DeserializeObject<WxAppConfigDto>(config.ConfigJson).CallCenterConfig;
        }
        /// <summary>
        /// 根据助理获取客资人数
        /// </summary>
        /// <returns></returns>
        public async Task<AssistantDistributeConsulationTypeDataDto> GetDistributeConsulationTypeDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList, bool? isAddWechat = null)
        {
            AssistantDistributeConsulationTypeDataDto data = new AssistantDistributeConsulationTypeDataDto();
            var data2 = await dalShoppingCartRegistration.GetAll()
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && assistantIdList.Contains(e.AssignEmpId.Value))
                .Where(e => isAddWechat == null || e.IsAddWeChat == isAddWechat)
                .GroupBy(e => e.EmergencyLevel).Select(e => new
                {
                    CustomerType = e.Key,
                    Count = e.Count()
                }).ToListAsync();
            data.FirstType = data2.Where(e => e.CustomerType == (int)EmergencyLevel.Important).FirstOrDefault()?.Count ?? 0;
            data.SecondType = data2.Where(e => e.CustomerType == (int)EmergencyLevel.Generally).FirstOrDefault()?.Count ?? 0;
            data.ThirdType = data2.Where(e => e.CustomerType == (int)EmergencyLevel.Ignorable).FirstOrDefault()?.Count ?? 0;
            data.TotalCount = data.FirstType + data.SecondType + data.ThirdType;
            return data;
        }
        /// <summary>
        /// 根据助理获取有效/潜在客资人数
        /// </summary>
        /// <returns></returns>
        public async Task<EffOrPotAssistantDistributeConsulationDataDto> GetEffOrPotDistributeConsulationTypeDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList)
        {
            EffOrPotAssistantDistributeConsulationDataDto data = new EffOrPotAssistantDistributeConsulationDataDto();
            var data2 = await dalShoppingCartRegistration.GetAll()
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && assistantIdList.Contains(e.AssignEmpId.Value))
                .Select(e => e.Price)
                .ToListAsync();
            data.Effective = data2.Where(e => e > 0).Count();
            data.Potential = data2.Where(e => e == 0).Count();
            data.TotalCount = data.Effective + data.Potential;
            return data;
        }

        /// <summary>
        /// 根据助理获取助理分诊折线图基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        public async Task<List<BaseKeyValueDto<string, int>>> GetDistributeConsulationTypeBrokenLineDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList)
        {

            return await dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && assistantIdList.Contains(e.AssignEmpId.Value)).Select(e => new BaseKeyValueDto<string, int>
            {
                Key = e.RecordDate.Date.Date.Day.ToString(),
                Value = e.EmergencyLevel,
            }).ToListAsync();


        }

        #region 行政客服看板

        /// <summary>
        /// 获取行政客服客资数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantId"></param>
        /// <param name="isAddWechat"></param>
        /// <returns></returns>
        public async Task<AssistantDistributeConsulationTypeDataDto> GetAdminCustomerDistributeConsulationTypeDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIds, bool? isAddWechat = null)
        {
            AssistantDistributeConsulationTypeDataDto data = new AssistantDistributeConsulationTypeDataDto();
            var data2 = await dalShoppingCartRegistration.GetAll()
                .Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && assistantIds.Contains(e.CreateBy))
                .Where(e => e.AssignEmpId != null)
                .GroupBy(e => e.EmergencyLevel).Select(e => new
                {
                    CustomerType = e.Key,
                    Count = e.Count()
                }).ToListAsync();
            data.FirstType = data2.Where(e => e.CustomerType == (int)EmergencyLevel.Important).FirstOrDefault()?.Count ?? 0;
            data.SecondType = data2.Where(e => e.CustomerType == (int)EmergencyLevel.Generally).FirstOrDefault()?.Count ?? 0;
            data.ThirdType = data2.Where(e => e.CustomerType == (int)EmergencyLevel.Ignorable).FirstOrDefault()?.Count ?? 0;
            data.TotalCount = data.FirstType + data.SecondType + data.ThirdType;
            return data;
        }

        /// <summary>
        /// 获取行政客服分诊折线图基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        public async Task<List<BaseKeyValueDto<string, int>>> GetAdminCustomerDistributeConsulationTypeBrokenLineDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIds)
        {
            return await dalShoppingCartRegistration.GetAll().Where(e => e.RecordDate >= startDate && e.RecordDate < endDate && assistantIds.Contains(e.CreateBy) && e.AssignEmpId != null).Select(e => new BaseKeyValueDto<string, int>
            {
                Key = e.RecordDate.Date.Date.Day.ToString(),
                Value = e.EmergencyLevel,
            }).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取行政客服小黄车业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetAdminCustomerShopCartRegisterPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, List<int> assistantIds,BelongChannel? belongChannel=null)
        {
            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate)
            .Where(o => o.AssignEmpId != null && o.IsReturnBackPrice == false)
            .Where(o => assistantIds.Contains(o.CreateBy))
                         select d;
            if (belongChannel.HasValue) {
                result = result.Where(e => e.BelongChannel == (int)belongChannel);
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                        Price = d.Price,
                        Phone = d.Phone,
                        CreateBy=d.CreateBy,
                        RecordDate=d.RecordDate,
                        BelongChannel = d.BelongChannel
                    };

            return await x.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取直播前小黄车业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetBeforeLiveShopCartRegisterPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, string baseId,List<int> assistantIds, BelongChannel? belongChannel = null)
        {
            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate)
            .Where(o => o.AssignEmpId != null && o.IsReturnBackPrice == false) select d;
            if (!string.IsNullOrEmpty(baseId))
            {
                result = from d in result.Where(o => o.BaseLiveAnchorId == baseId)
                         select d;
            }
            else {
                result = from d in result.Where(o => assistantIds.Contains(o.CreateBy))
                         select d;
            }
            
           
            if (belongChannel.HasValue)
            {
                result = result.Where(e => e.BelongChannel == (int)belongChannel);
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                        Price = d.Price,
                        Phone = d.Phone,
                        CreateBy = d.CreateBy,
                        RecordDate = d.RecordDate,
                        BelongChannel = d.BelongChannel
                    };

            return await x.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取直播前小黄车业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartRegistrationDto>> GetBeforeLiveShopCartRegisterPerformanceByAssistantIdListAndBaseIdListAsync(DateTime startDate, DateTime endDate, List<string> baseIds, List<int> assistantIds, BelongChannel? belongChannel = null)
        {
            var result = from d in dalShoppingCartRegistration.GetAll()
            .Where(o => o.RecordDate >= startDate && o.RecordDate < endDate)
            .Where(o => o.AssignEmpId != null && o.IsReturnBackPrice == false)
                         select d;
            if (baseIds.Count>0)
            {
                result = from d in result.Where(o => baseIds.Contains(o.BaseLiveAnchorId))
                         select d;
            }
            if (assistantIds.Count > 0)
            {
                result = from d in result.Where(o => assistantIds.Contains(o.CreateBy))
                         select d;
            }


            if (belongChannel.HasValue)
            {
                result = result.Where(e => e.BelongChannel == (int)belongChannel);
            }
            var x = from d in result
                    select new ShoppingCartRegistrationDto
                    {
                        IsReturnBackPrice = d.IsReturnBackPrice,
                        AssignEmpId = d.AssignEmpId,
                        IsAddWeChat = d.IsAddWeChat,
                        Price = d.Price,
                        Phone = d.Phone,
                        CreateBy = d.CreateBy,
                        RecordDate = d.RecordDate,
                        BelongChannel = d.BelongChannel
                    };

            return await x.ToListAsync();
        }



        #endregion
    }
}
