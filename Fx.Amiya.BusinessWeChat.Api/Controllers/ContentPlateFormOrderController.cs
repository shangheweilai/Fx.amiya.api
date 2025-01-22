using Fx.Amiya.BusinessWeChat.Api.Vo.Base;
using Fx.Amiya.BusinessWeChat.Api.Vo.ContentPlateFormOrder;
using Fx.Amiya.BusinessWeChat.Api.Vo.ContentPlatFormOrderSend;
using Fx.Amiya.Dto.ContentPlateFormOrder;
using Fx.Amiya.Dto.ContentPlatFormOrderDealDetails.Input;
using Fx.Amiya.Dto.ContentPlatFormOrderSend;
using Fx.Amiya.Dto.CustomerInfo;
using Fx.Amiya.Dto.OperationLog;
using Fx.Amiya.IService;
using Fx.Amiya.Service;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using jos_sdk_net.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.BusinessWechat.Api.Controllers
{
    /// <summary>
    /// 内容平台订单模块
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ContentPlateFormOrderController : ControllerBase
    {
        private IContentPlateFormOrderService _orderService;
        private ICustomerService customerService;
        private IBindCustomerServiceService bindCustomerServiceService;
        private IShoppingCartRegistrationService shoppingCartRegistrationService;
        private IWxAppConfigService _wxAppConfigService;
        private IContentPlatFormCustomerPictureService _contentPlatFormCustomerPictureService;
        private IHttpContextAccessor _httpContextAccessor;
        private IAmiyaPositionInfoService amiyaPositionInfoService;
        private IOperationLogService operationLogService;
        /// <summary>
        /// 内容平台订单模块
        /// </summary>
        /// <param name="orderService"></param>
        /// <param name="customerService"></param>
        /// <param name="contentPlatFormCustomerPictureService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="amiyaPositionInfoService"></param>
        /// <param name="wxAppConfigService"></param>
        public ContentPlateFormOrderController(IContentPlateFormOrderService orderService,
            IContentPlatFormCustomerPictureService contentPlatFormCustomerPictureService,
            IShoppingCartRegistrationService shoppingCartRegistrationService,
            IHttpContextAccessor httpContextAccessor,
            IBindCustomerServiceService bindCustomerServiceService,
            IAmiyaPositionInfoService amiyaPositionInfoService,
            ICustomerService customerService,
             IWxAppConfigService wxAppConfigService, IOperationLogService operationLogService)
        {
            _orderService = orderService;
            this.customerService = customerService;
            this.shoppingCartRegistrationService = shoppingCartRegistrationService;
            this.bindCustomerServiceService = bindCustomerServiceService;
            this.amiyaPositionInfoService = amiyaPositionInfoService;
            _wxAppConfigService = wxAppConfigService;
            _httpContextAccessor = httpContextAccessor;
            _contentPlatFormCustomerPictureService = contentPlatFormCustomerPictureService;
            this.operationLogService = operationLogService;
        }


        /// <summary>
        /// 录单
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("contentPlateFormAddOrder")]
        [FxInternalAuthorize]
        public async Task<ResultData> ContentPlateFormAddOrderAsync(ContentPlateFormOrderAddVo addVo)
        {
            OperationAddDto operationLog = new OperationAddDto();
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                operationLog.OperationBy = Convert.ToInt32(employee.Id);
                //if (employee.PositionName == "客服" || employee.PositionName == "客服管理员")
                //{
                //    var IsExistOrder = _tmallOrderService.IsExistPhoneAsync(addVo.Phone);
                //    if (await IsExistOrder == true)
                //    {
                //        throw new Exception("该客户手机号已在下单平台产生过订单，请确认后联系客服主管！");
                //    }
                //}

                //添加订单
                ContentPlateFormOrderAddDto addDto = new ContentPlateFormOrderAddDto();
                addDto.EmployeeId = addVo.BelongEmpId;
                addDto.Id = CreateOrderIdHelper.GetNextNumber();
                addDto.BelongMonth = addVo.BelongMonth;
                addDto.AddOrderPrice = addVo.AddOrderPrice;
                addDto.OrderType = addVo.OrderType;
                addDto.ContentPlateFormId = addVo.ContentPlateFormId;
                addDto.ConsultationEmpId = addVo.ConsultationEmpId;
                addDto.ConsultationType = addVo.ConsultationType;
                addDto.LiveAnchorId = addVo.LiveAnchorId;
                addDto.LiveAnchorWeChatNo = addVo.LiveAnchorWeChatNo;
                addDto.CustomerName = addVo.CustomerName;
                addDto.Phone = addVo.Phone;
                addDto.City = addVo.City;
                addDto.Sex = addVo.Sex;
                addDto.Birthday = addVo.Birthday;
                addDto.Occupation = addVo.Occupation;
                addDto.WechatNumber = addVo.WechatNumber;
                addDto.AppointmentDate = addVo.AppointmentDate;
                addDto.AppointmentHospitalId = addVo.AppointmentHospitalId;
                addDto.DepositAmount = addVo.DepositAmount;
                addDto.OrderSource = addVo.OrderSource;
                addDto.UnSendReason = addVo.UnSendReason;
                addDto.AcceptConsulting = addVo.AcceptConsulting;
                addDto.GoodsId = addVo.GoodsId;
                addDto.HospitalDepartmentId = addVo.HospitalDepartmentId;
                addDto.ConsultingContent = addVo.ConsultingContent;
                addDto.Remark = addVo.Remark;
                addDto.LateProjectStage = addVo.LateProjectStage;
                addDto.CustomerPictures = new List<string>();
                addDto.CustomerPictures = addVo.CustomerPictures;
                addDto.IsSupportOrder = addVo.IsSupportOrder;
                addDto.SupportEmpId = addVo.SupportEmpId;
                addDto.GetCustomerType = addVo.GetCustomerType;
                addDto.CustomerSource = addVo.CustomerSource;
                addDto.CustomerType = addVo.CustomerType;
                addDto.BelongChannel = addVo.BelongChannel;
                addDto.ConsultingContent2 = addVo.ConsultingContent2;
                addDto.IsRiBuLuoLiving = addVo.IsRiBuLuoLiving;
                addDto.BelongCompanyEnumId = addVo.BelongCompanyEnumId;
                await _orderService.AddContentPlateFormOrderAsync(addDto);


                //编辑客户基础信息
                EditCustomerDto editDto = new EditCustomerDto();
                var config = await _wxAppConfigService.GetWxAppCallCenterConfigAsync();
                string encryptPhon = ServiceClass.Encrypt(addVo.Phone, config.PhoneEncryptKey);
                editDto.EncryptPhone = encryptPhon;
                editDto.Name = addVo.CustomerName;
                editDto.Sex = addVo.Sex;
                editDto.Birthday = addVo.Birthday;
                editDto.Occupation = addVo.Occupation;
                editDto.WechatNumber = addVo.WechatNumber;
                editDto.City = addVo.City;
                await customerService.EditAsync(editDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                throw ex;
            }
            finally
            {
                operationLog.Source = (int)RequestSource.AmiyaBusinessWechat;
                operationLog.Parameters = JsonConvert.SerializeObject(addVo);
                operationLog.RequestType = (int)RequestType.Add;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }

        }



        /// <summary>
        /// 根据条件获取内容平台订单
        /// </summary>
        /// <param name="liveAnchorId">主播id</param>
        /// <param name="getCustomerType">获客方式</param>
        /// <param name="liveAnchorWechatId">主播微信号id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="keyword">关键词</param>
        /// <param name="belongMonth">归属月份</param>
        /// <param name="minAddOrderPrice">最小下单金额</param>
        /// <param name="maxAddOrderPrice">最大下单金额</param>
        /// <param name="hospitalDepartmentId">科室id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="appointmentHospital">预约医院</param>
        /// <param name="consultationType">面诊状态</param>
        /// <param name="contentPlateFormId">内容平台</param>
        /// <param name="orderSource">订单来源(-1查询全部)</param>
        /// <param name="belongEmpId">归属客服id</param>
        /// <param name="pageNum">第/页</param>
        /// <param name="pageSize">每页显示/行</param>
        /// <returns></returns>
        [HttpGet("contentPlateFormOrderLlistWithPage")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<ContentPlatFormOrderInfoVo>>> GetOrderListWithPageAsync(int? liveAnchorId, int? getCustomerType, string liveAnchorWechatId, DateTime? startDate, DateTime? endDate, DateTime? appointmentStartDate, DateTime? appointmentEndDate, int? belongMonth, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, int? appointmentHospital, int? consultationType, string hospitalDepartmentId, string keyword, int? orderStatus, string contentPlateFormId, int? belongEmpId,int?belongCompany, int orderSource, int pageNum, int pageSize)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                List<int> liveAnchorIds = new List<int>();
                if (liveAnchorId.HasValue)
                {
                    liveAnchorIds.Add(liveAnchorId.Value);
                }
                var q = await _orderService.GetOrderListWithPageAsync(liveAnchorIds, getCustomerType, liveAnchorWechatId, startDate, endDate, appointmentStartDate, appointmentEndDate, belongMonth, minAddOrderPrice, maxAddOrderPrice, appointmentHospital, consultationType, hospitalDepartmentId, keyword, orderStatus, contentPlateFormId, belongEmpId, employeeId,belongCompany, orderSource, pageNum, pageSize);
                List<ContentPlatFormOrderInfoVo> contentPlatFormOrderInfoVoList = new List<ContentPlatFormOrderInfoVo>();
                var resutList = q.List.ToList();
                foreach (var x in resutList)
                {
                    ContentPlatFormOrderInfoVo resultVo = new ContentPlatFormOrderInfoVo();
                    resultVo.Id = x.Id;
                    resultVo.OrderTypeText = x.OrderTypeText;
                    resultVo.ContentPlatformName = x.ContentPlatformName;
                    resultVo.LiveAnchorName = x.LiveAnchorName;
                    resultVo.LiveAnchorWeChatNo = x.LiveAnchorWeChatNo;
                    resultVo.ConsultationType = x.ConsultationTypeText;
                    resultVo.BelongMonth = x.BelongMonth;
                    resultVo.AddOrderPrice = x.AddOrderPrice;
                    resultVo.IsRiBuLuoLiving = x.IsRiBuLuoLiving;
                    resultVo.CreateDate = x.CreateDate;
                    resultVo.CustomerName = x.CustomerName;
                    resultVo.Phone = x.Phone;
                    var customerBaseInfo = await customerService.GetCustomerBaseInfoByEncryptPhoneAsync(x.EncryptPhone);
                    resultVo.City = customerBaseInfo.City;
                    resultVo.DepartmentName =
                    resultVo.AppointmentDate = x.AppointmentDate == null ? "未预约时间" : x.AppointmentDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    resultVo.AppointmentHospitalName = x.AppointmentHospitalName;
                    resultVo.GoodsName = x.GoodsName;
                    resultVo.ThumbPictureUrl = x.ThumbPictureUrl;
                    resultVo.ConsultingContent = x.ConsultingContent;
                    resultVo.OrderStatusText = x.OrderStatusText;
                    resultVo.DepositAmount = x.DepositAmount;
                    resultVo.IsToHospital = x.IsToHospital;
                    resultVo.ToHospitalDate = x.ToHospitalDate;
                    resultVo.LastDealHospital = x.LastDealHospital;
                    resultVo.DealAmount = x.DealAmount;
                    resultVo.DealDate = x.DealDate;
                    resultVo.UnDealReason = x.UnDealReason;
                    resultVo.LateProjectStage = x.LateProjectStage;
                    resultVo.Remark = x.Remark;
                    resultVo.DepartmentName = x.DepartmentName;
                    resultVo.BelongEmpName = x.BelongEmpName;
                    resultVo.UnSendReason = x.UnSendReason;
                    resultVo.OrderSourceText = x.OrderSourceText;
                    resultVo.AcceptConsulting = x.AcceptConsulting;
                    resultVo.CheckStateText = x.CheckStateText;
                    resultVo.CheckDate = x.CheckDate;
                    resultVo.CheckByName = x.CheckByName;
                    resultVo.CheckPrice = x.CheckPrice;
                    resultVo.Sender = x.Sender;
                    resultVo.SendDate = x.SendDate;

                    resultVo.CheckRemark = x.CheckRemark;
                    resultVo.SettlePrice = x.SettlePrice;
                    resultVo.IsReturnBackPrice = x.IsReturnBackPrice;
                    resultVo.ReturnBackDate = x.ReturnBackDate;
                    resultVo.ReturnBackPrice = x.ReturnBackPrice;
                    resultVo.OtherContentPlatFormOrderId = x.OtherContentPlatFormOrderId;
                    resultVo.IsRepeatProfundityOrder = x.IsRepeatProfundityOrder;
                    resultVo.BelongChannel = x.BelongChannel;
                    resultVo.BelongChannelText = x.BelongChannelText;
                    //    if (x.BelongEmpId != 0)
                    //    {
                    //        var empInfo = await amiyaEmployeeService.GetByIdAsync(x.BelongEmpId.Value);
                    //        x.BelongEmpName = empInfo.Name.ToString();
                    //    }
                    //    if (x.CheckBy != 0)
                    //    {
                    //        var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CheckBy.Value);
                    //        x.CheckByName = empInfo.Name.ToString();
                    //    }
                    //    if (!string.IsNullOrEmpty(x.GoodsDepartmentId))
                    //    {
                    //        var departmentInfo = await _departmentService.GetByIdAsync(x.GoodsDepartmentId);
                    //        x.DepartmentName = departmentInfo.DepartmentName;
                    //    }
                    //    if (x.LastDealHospitalId.HasValue)
                    //    {
                    //        var hospitalInfo = await _hospitalInfoService.GetBaseByIdAsync(x.LastDealHospitalId.Value);
                    //        x.LastDealHospital = hospitalInfo.Name;
                    //    }
                    //    if (x.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                    //    {
                    //        var sendOrderInfoList = await _contentPlatformOrderSend.GetSendOrderInfoByOrderId(x.Id);
                    //        var sendOrderInfo = sendOrderInfoList.OrderByDescending(z => z.SendDate).FirstOrDefault();
                    //        if (sendOrderInfo != null)
                    //        {
                    //            x.SendDate = sendOrderInfo.SendDate;
                    //            var empInfo = await amiyaEmployeeService.GetByIdAsync(sendOrderInfo.Sender);
                    //            x.Sender = empInfo.Name;
                    //        }
                    //    }
                    contentPlatFormOrderInfoVoList.Add(resultVo);
                }
                FxPageInfo<ContentPlatFormOrderInfoVo> pageInfo = new FxPageInfo<ContentPlatFormOrderInfoVo>();
                pageInfo.TotalCount = q.TotalCount;
                pageInfo.List = contentPlatFormOrderInfoVoList;
                return ResultData<FxPageInfo<ContentPlatFormOrderInfoVo>>.Success().AddData("contentPlatFormOrder", pageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<ContentPlatFormOrderInfoVo>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 获取未派单订单列表（分页）
        /// </summary>
        /// <param name="liveAnchorId">归属主播id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="contentPlateFormId">内容平台</param>
        /// <param name="consultationEmpId">面诊员id</param>
        /// <param name="employeeId">员工id（-1查询所有）</param>
        /// <param name="orderSource">订单来源（-1查询所有）</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("unContentPlatFormSendOrderList")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<UnContentPlateFormSendOrderInfoVo>>> GetUnSendOrderListWithPageAsync(int? liveAnchorId, DateTime? startDate, DateTime? endDate, int? consultationEmpId, string keyword, string contentPlateFormId, int? employeeId, int orderStatus, int orderSource, int pageNum, int pageSize)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            var LoginEmployeeId = Convert.ToInt32(employee.Id);
            List<int?> liveAnchorIds = new List<int?>();
            if (liveAnchorId.HasValue)
            {
                liveAnchorIds.Add(liveAnchorId.Value);
            }
            var q = await _orderService.GetUnSendOrderListWithPageAsync(liveAnchorIds, keyword, startDate, endDate, consultationEmpId, LoginEmployeeId, (int)employeeId, orderStatus, contentPlateFormId, orderSource, pageNum, pageSize);
            var unSendOrder = from d in q.List
                              select new UnContentPlateFormSendOrderInfoVo
                              {
                                  OrderId = d.OrderId,
                                  ContentPlatFormName = d.ContentPlatFormName,
                                  LiveAnchorName = d.LiveAnchorName,
                                  GoodsName = d.GoodsName,
                                  ThumbPictureUrl = d.ThumbPictureUrl,
                                  ConsultingContent = d.ConsultingContent,
                                  CustomerName = d.CustomerName,
                                  Phone = d.Phone,
                                  ConsultationTypeText = d.ConsultationTypeText,
                                  EncryptPhone = d.EncryptPhone,
                                  DealAmount = d.DealAmount,
                                  DepositAmount = d.DepositAmount.HasValue ? d.DepositAmount : 0,
                                  OrderTypeText = d.OrderTypeText,
                                  OrderStatusText = d.OrderStatusText,
                                  AppointmentHospital = d.AppointmentHospital,
                                  AppointmentDate = d.AppointmentDate,
                                  Remark = d.Remark,
                                  LateProjectStage = d.LateProjectStage,
                                  UnSendReason = d.UnSendReason,
                                  OrderSourceText = d.OrderSourceText,
                              };
            FxPageInfo<UnContentPlateFormSendOrderInfoVo> pageInfo = new FxPageInfo<UnContentPlateFormSendOrderInfoVo>();
            pageInfo.TotalCount = q.TotalCount;
            pageInfo.List = unSendOrder;
            return ResultData<FxPageInfo<UnContentPlateFormSendOrderInfoVo>>.Success().AddData("unSendOrder", pageInfo);
        }


        /// <summary>
        /// 根据订单编号获取订单要修改的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData<ContentPlateFormOrderVo>> GetByIdAsync(string id)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            var order = await _orderService.GetByOrderIdAsync(id);
            var positionInfo = await amiyaPositionInfoService.GetByIdAsync(Convert.ToInt32(employee.PositionId));
            if (employeeId != order.BelongEmpId && employee.IsCustomerService == true && !positionInfo.IsDirector)
            //if (employeeId != order.BelongEmpId && employee.IsCustomerService == true)
            {
                //加上辅助客服是否与当前登陆角色相等;
                if (employeeId != order.SupportEmpId && employee.IsCustomerService == true)
                {
                    var bindCustomerInfo = await bindCustomerServiceService.GetEmployeeIdByPhone(order.Phone);
                    if (bindCustomerInfo != 0 && bindCustomerInfo != employeeId)
                    {
                        throw new Exception("该订单已归属到其他客服名下，您暂时无法操作！");
                    }
                }
            }
            ContentPlateFormOrderVo orderUpdateInfo = new ContentPlateFormOrderVo();
            orderUpdateInfo.Id = order.Id;
            orderUpdateInfo.UserId = order.UserId;
            orderUpdateInfo.OrderType = order.OrderType;
            orderUpdateInfo.ContentPlateFormId = order.ContentPlateFormId;
            orderUpdateInfo.ConsultationType = order.ConsultationType;
            orderUpdateInfo.ConsultationTypeText = order.ConsultationTypeText;
            orderUpdateInfo.BelongMonth = order.BelongMonth;
            orderUpdateInfo.AddOrderPrice = order.AddOrderPrice;
            orderUpdateInfo.GetCustomerType = order.GetCustomerType;
            orderUpdateInfo.GetCustomerTypeText = order.GetCustomerTypeText;
            orderUpdateInfo.LiveAnchorId = order.LiveAnchorId;
            orderUpdateInfo.GoodsId = order.GoodsId;
            orderUpdateInfo.CustomerName = order.CustomerName;
            orderUpdateInfo.Phone = order.Phone;
            orderUpdateInfo.EncryptPhone = order.EncryptPhone;
            var config = await _wxAppConfigService.GetWxAppCallCenterConfigAsync();
            string encryptPhone = ServiceClass.Encrypt(order.Phone, config.PhoneEncryptKey);
            var customerBaseInfo = await customerService.GetCustomerBaseInfoByEncryptPhoneAsync(encryptPhone);
            orderUpdateInfo.City = customerBaseInfo.City;
            orderUpdateInfo.Sex = customerBaseInfo.Sex;
            orderUpdateInfo.Birthday = customerBaseInfo.Birthday;
            orderUpdateInfo.Age = customerBaseInfo.Age;
            orderUpdateInfo.Occupation = customerBaseInfo.Occupation;
            orderUpdateInfo.WechatNumber = customerBaseInfo.WechatNumber;
            orderUpdateInfo.LiveAnchorBaseWechatId = order.LiveAnchorBaseWechatId;
            orderUpdateInfo.AppointmentDate = order.AppointmentDate;
            orderUpdateInfo.ConsultationEmpId = order.ConsultationEmpId;
            orderUpdateInfo.AppointmentHospitalId = order.AppointmentHospitalId;
            orderUpdateInfo.DepositAmount = order.DepositAmount;
            orderUpdateInfo.ConsultingContent = order.ConsultingContent;
            orderUpdateInfo.OrderStatus = order.OrderStatus;
            orderUpdateInfo.OrderStatusText = order.OrderStatusText;
            orderUpdateInfo.AppointmentHospitalName = order.AppointmentHospitalName;
            orderUpdateInfo.AppointmentHospitalId = order.AppointmentHospitalId;
            orderUpdateInfo.Remark = order.Remark;
            orderUpdateInfo.LateProjectStage = order.LateProjectStage;
            orderUpdateInfo.CheckState = order.CheckState;
            orderUpdateInfo.LiveAnchorWeChatNo = order.LiveAnchorWeChatNo;
            orderUpdateInfo.IsOldCustomer = order.IsOldCustomer;
            orderUpdateInfo.IsAcompanying = order.IsAcompanying;
            orderUpdateInfo.CommissionRatio = order.CommissionRatio;
            orderUpdateInfo.CheckPrice = order.CheckPrice;
            orderUpdateInfo.HospitalDepartmentId = order.HospitalDepartmentId;
            orderUpdateInfo.SettlePrice = order.SettlePrice;
            orderUpdateInfo.OrderSource = order.OrderSource;
            orderUpdateInfo.AcceptConsulting = order.AcceptConsulting;
            orderUpdateInfo.UnSendReason = order.UnSendReason;
            orderUpdateInfo.CreateDate = order.CreateDate;
            orderUpdateInfo.NetWorkConsulationName = order.NetWorkConsulationName;
            orderUpdateInfo.SceneConsulationName = order.SceneConsulationName;
            orderUpdateInfo.SendDate = order.SendDate;
            orderUpdateInfo.UnDealPictureUrl = order.UnDealPictureUrl;
            orderUpdateInfo.UnDealReason = order.UnDealReason;
            orderUpdateInfo.DealPictureUrl = order.DealPictureUrl;
            orderUpdateInfo.DealPerformanceTypeText = order.DealPerformanceTypeText;
            orderUpdateInfo.UpdateDate = order.UpdateDate;
            orderUpdateInfo.DealDate = order.DealDate;
            orderUpdateInfo.DealAmount = order.DealAmount;
            orderUpdateInfo.AppointmentHospitalName = order.AppointmentHospitalName;
            orderUpdateInfo.GoodsName = order.GoodsName;
            orderUpdateInfo.ContentPlateFormName = order.ContentPlateFormName;
            orderUpdateInfo.OrderTypeText = order.OrderTypeText;
            orderUpdateInfo.OrderSourceText = order.OrderSourceText;
            orderUpdateInfo.HospitalDepartmentName = order.HospitalDepartmentName;
            orderUpdateInfo.ConsultationEmpName = order.ConsultationEmpName;
            orderUpdateInfo.LiveAnchorName = order.LiveAnchorName;
            orderUpdateInfo.CheckStateText = order.CheckStateText;
            orderUpdateInfo.BelongEmpId = order.BelongEmpId;
            orderUpdateInfo.BelongEmpName = order.BelongEmpName;
            orderUpdateInfo.LastDealHospitalId = order.LastDealHospitalId;
            orderUpdateInfo.LastDealHospitalName = order.LastDealHospitalName;
            orderUpdateInfo.OtherContentPlatFormOrderId = order.OtherContentPlatFormOrderId;
            orderUpdateInfo.ThumbPicture = order.ThumbPicture;
            orderUpdateInfo.CheckBy = order.CheckBy;
            orderUpdateInfo.CheckDate = order.CheckDate;
            orderUpdateInfo.CheckByName = order.CheckByName;
            orderUpdateInfo.IsReturnBackPrice = order.IsReturnBackPrice;
            orderUpdateInfo.ReturnBackDate = order.ReturnBackDate;
            orderUpdateInfo.ReturnBackPrice = order.ReturnBackPrice;
            orderUpdateInfo.IsToHospital = order.IsToHospital;
            orderUpdateInfo.ToHospitalType = order.ToHospitalType;
            orderUpdateInfo.ToHospitalTypeText = order.ToHospitalTypeText;
            orderUpdateInfo.ToHospitalDate = order.ToHospitalDate;
            orderUpdateInfo.SendBy = order.SendBy;
            orderUpdateInfo.SendByName = order.SendByName;
            orderUpdateInfo.SendHospitalName = order.SendHospitalName;
            var pictures = await _contentPlatFormCustomerPictureService.GetListAsync(orderUpdateInfo.Id, "顾客照片");
            orderUpdateInfo.CustomerPictures = pictures.Select(z => z.CustomerPicture).ToList();
            orderUpdateInfo.IsRepeatProfundityOrder = order.IsRepeatProfundityOrder;
            orderUpdateInfo.IsCreateBill = order.IsCreateBill;
            orderUpdateInfo.CreateBillCompany = order.CreateBillCompany;
            orderUpdateInfo.IsSupportOrder = order.IsSupportOrder;
            orderUpdateInfo.SupportEmpName = order.SupportEmpName;
            orderUpdateInfo.SupportEmpId = order.SupportEmpId;
            orderUpdateInfo.CustomerType = order.CustomerType;
            orderUpdateInfo.CustomerTypeText = order.CustomerTypeText;
            orderUpdateInfo.CustomerSource = order.CustomerSource;
            orderUpdateInfo.CustomerSourceText = order.CustomerSourceText;
            orderUpdateInfo.BelongChannel = order.BelongChannel;
            orderUpdateInfo.BelongChannelText = order.BelongChannelText;
            orderUpdateInfo.ConsultingContent2 = order.ConsultingContent2;
            orderUpdateInfo.IsRiBuLuoLiving = order.IsRiBuLuoLiving;
            orderUpdateInfo.BelongCompanyEnumId = order.BelongCompanyEnumId;
            orderUpdateInfo.BelongCompanyName = order.BelongCompanyName;
            return ResultData<ContentPlateFormOrderVo>.Success().AddData("orderInfo", orderUpdateInfo);
        }


        /// <summary>
        /// 根据订单编号查看重单截图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("selectRepeateOrderPic")]
        [FxInternalAuthorize]
        public async Task<ResultData<string>> SelectRepeateOrderPicAsync(string id)
        {
            var repeateOrderPictureUrl = await _orderService.SelectRepeateOrderPicAsync(id);
            return ResultData<string>.Success().AddData("repeateOrderPictureUrl", repeateOrderPictureUrl);
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("updateContentPlateFormOrder")]
        [FxInternalAuthorize]
        public async Task<ResultData> UpdateOrderAsync(ContentPlateFormOrderUpdateVo updateVo)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);

            //修改订单
            ContentPlateFormOrderUpdateDto updateDto = new ContentPlateFormOrderUpdateDto();
            updateDto.Id = updateVo.Id;
            updateDto.AddOrderPrice = updateVo.AddOrderPirce;
            updateDto.BelongMonth = updateVo.BelongMonth;
            updateDto.OrderType = updateVo.OrderType;
            updateDto.ContentPlateFormId = updateVo.ContentPlateFormId;
            updateDto.GetCustomerType = updateVo.GetCustomerType;
            updateDto.LiveAnchorWeChatNo = updateVo.LiveAnchorWeChatNo;
            updateDto.LiveAnchorId = updateVo.LiveAnchorId;
            updateDto.GoodsId = updateVo.GoodsId;
            updateDto.ConsultationType = updateVo.ConsultationType;
            updateDto.HospitalDepartmentId = updateVo.HospitalDepartmentId;
            updateDto.CustomerName = updateVo.CustomerName;
            updateDto.ConsultationEmpId = updateVo.ConsultationEmpId;
            updateDto.Phone = updateVo.Phone;
            updateDto.AppointmentDate = updateVo.AppointmentDate;
            updateDto.AppointmentHospitalId = updateVo.AppointmentHospitalId;
            updateDto.DepositAmount = updateVo.DepositAmount;
            updateDto.ConsultingContent = updateVo.ConsultingContent;
            updateDto.Remark = updateVo.Remark;
            updateDto.LateProjectStage = updateVo.LateProjectStage;
            updateDto.EmployeeId = employeeId;
            updateDto.OrderSource = updateVo.OrderSource;
            updateDto.AcceptConsulting = updateVo.AcceptConsulting;
            updateDto.UnSendReason = updateVo.UnSendReason;
            updateDto.CustomerPictures = updateVo.CustomerPictures;

            updateDto.City = updateVo.City;
            updateDto.Sex = updateVo.Sex;
            updateDto.Birthday = updateVo.Birthday;
            updateDto.Occupation = updateVo.Occupation;
            updateDto.WechatNumber = updateVo.WechatNumber;
            updateDto.CustomerSource = updateVo.CustomerSource;
            updateDto.CustomerType = updateVo.CustomerType;
            updateDto.BelongChannel = updateVo.BelongChannel;
            updateDto.ConsultingContent2 = updateVo.ConsultingContent2;
            updateDto.IsRiBuLuoLiving = updateVo.IsRiBuLuoLiving;
            updateDto.BelongCompanyEnumId = updateVo.BelongCompanyEnumId;
            await _orderService.UpdateContentPlateFormOrderAsync(updateDto);


            //编辑客户基础信息
            EditCustomerDto editDto = new EditCustomerDto();
            var config = await _wxAppConfigService.GetWxAppCallCenterConfigAsync();
            string encryptPhon = ServiceClass.Encrypt(updateVo.Phone, config.PhoneEncryptKey);
            editDto.EncryptPhone = encryptPhon;
            editDto.Name = updateVo.CustomerName;
            editDto.Sex = updateVo.Sex;
            editDto.Birthday = updateVo.Birthday;
            editDto.Occupation = updateVo.Occupation;
            editDto.WechatNumber = updateVo.WechatNumber;
            editDto.City = updateVo.City;
            await customerService.EditAsync(editDto);
            return ResultData.Success();
        }


        /// <summary>
        /// 关闭重单可深度状态
        /// </summary>
        /// <param name="close"></param>
        /// <returns></returns>
        [HttpPost("colseRepeatProfundityOrder")]
        [FxInternalAuthorize]
        public async Task<ResultData> CloseRepeatProfundityOrder(CloseRepeatProfundityOrderVo close)
        {
            await _orderService.UpdateContentPalteformRepeaterOrderStatusAsync(close.ContentPlateFormId);
            return ResultData.Success();
        }

        /// <summary>
        /// 添加派单
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost]
        [FxInternalAuthorize]
        public async Task<ResultData> AddAsync(AddContentPlatFormSendOrderInfoVo addVo)
        {
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Source = (int)RequestSource.AmiyaBusinessWechat;
            operationLog.Code = 0;
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                AddContentPlatFormSendOrderInfoDto addDto = new AddContentPlatFormSendOrderInfoDto();
                addDto.HospitalId = addVo.HospitalId;
                addDto.OrderId = addVo.OrderId;
                addDto.IsUncertainDate = addVo.IsUncertainDate;
                addDto.AppointmentDate = addVo.AppointmentDate;
                addDto.Remark = addVo.Remark;
                addDto.SendBy = addVo.SendBy;
                addDto.EmployeeId = employeeId;
                addDto.OtherHospitalId = addVo.OtherHospitalId;
                addDto.IsSpecifyHospitalEmployee = addVo.IsSpecifyHospitalEmployee;
                addDto.HospitalEmployeeId = addVo.HospitalEmployeeId;
                await _orderService.AddAsync(addDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                throw ex;
            }
            finally
            {
                operationLog.Parameters = JsonConvert.SerializeObject(addVo);
                operationLog.RequestType = (int)RequestType.Update;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

        /// <summary>
        /// 修改派单
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        [FxInternalAuthorize]
        public async Task<ResultData> UpdateAsync(UpdateContentPlatFormSendOrderInfoVo updateVo)
        {
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Source = (int)RequestSource.AmiyaBusinessWechat;
            operationLog.Code = 0;
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                UpdateContentPlatFormSendOrderInfoDto updateDto = new UpdateContentPlatFormSendOrderInfoDto();
                updateDto.Id = updateVo.Id;
                updateDto.OrderId = updateVo.OrderId;
                updateDto.HospitalId = updateVo.HospitalId;
                updateDto.IsUncertainDate = updateVo.IsUncertainDate;
                updateDto.AppointmentDate = updateVo.AppointmentDate;
                updateDto.Remark = updateVo.Remark;

                await _orderService.UpdateAsync(updateDto, employeeId);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                throw ex;
            }
            finally
            {
                operationLog.Parameters = JsonConvert.SerializeObject(updateVo);
                operationLog.RequestType = (int)RequestType.Update;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

        /// <summary>
        /// 客服完成订单
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("finishContentPlateFormOrderByEmployee")]
        [FxInternalAuthorize]
        public async Task<ResultData> FinishOrderByEmployeeAsync(ContentPlateFormOrderFinishVo updateVo)
        {
            OperationAddDto operationLog = new OperationAddDto();
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                if (updateVo.ToHospitalType == (int)ContentPlateFormOrderToHospitalType.REFUND || updateVo.ConsumptionType == (int)ConsumptionType.Refund)
                {
                    if (employee.DepartmentId == "1" || employee.DepartmentId == "7")
                    {
                        updateVo.DealAmount = -updateVo.DealAmount;
                    }
                    else
                    {
                        throw new Exception("只有管理员与财务方可录入退款订单，请联系对应人员操作！");
                    }
                }

                ContentPlateFormOrderFinishDto updateDto = new ContentPlateFormOrderFinishDto();
                updateDto.Id = updateVo.Id;
                updateDto.IsFinish = updateVo.IsFinish;
                updateDto.LastDealHospitalId = updateVo.LastDealHospitalId;
                updateDto.ToHospitalDate = updateVo.ToHospitalDate;
                updateDto.DealAmount = updateVo.DealAmount;
                updateDto.LastProjectStage = updateVo.LastProjectStage;
                updateDto.DealPictureUrl = updateVo.DealPictureUrl;
                updateDto.CommissionRatio = updateVo.CommissionRatio;
                updateDto.IsAcompanying = updateVo.IsAcompanying;
                updateDto.UnDealReason = updateVo.UnDealReason;
                updateDto.IsToHospital = updateVo.IsToHospital;
                updateDto.ToHospitalType = updateVo.ToHospitalType;
                updateDto.DealPerformanceType = updateVo.DealPerformanceType;
                updateDto.UnDealPictureUrl = updateVo.UnDealPictureUrl;
                updateDto.DealDate = updateVo.DealDate;
                updateDto.OtherContentPlatFormOrderId = updateVo.OtherContentPlatFormOrderId;
                updateDto.EmpId = Convert.ToInt32(employee.Id);
                updateDto.FansMeetingId = updateVo.FansMeetingId;

                updateDto.FansMeetingProject = updateVo.FansMeetingProject;
                updateDto.FollowUpContent = updateVo.FollowUpContent;
                updateDto.NextAppointmentDate = updateVo.NextAppointmentDate;
                updateDto.IsNeedHospitalHelp = updateVo.IsNeedHospitalHelp;
                //企业微信新版本发布后退回
                updateDto.ConsumptionType = updateVo.ConsumptionType;
                //updateDto.ConsumptionType = (int)ConsumptionType.Deal;
                updateDto.InvitationDocuments = updateVo.InvitationDocuments;
                List<AddContentPlatFormOrderDealDetailsDto> addContentPlatFormOrderDealDetailsDtos = new List<AddContentPlatFormOrderDealDetailsDto>();
                updateDto.SendOrderId = updateVo.SendOrderId;
                if (updateDto.IsFinish == true && updateVo.AddContentPlatFormOrderDealDetailsVoList != null)
                {
                    foreach (var x in updateVo.AddContentPlatFormOrderDealDetailsVoList)
                    {
                        if (updateVo.ToHospitalType == (int)ContentPlateFormOrderToHospitalType.REFUND || updateVo.ConsumptionType == (int)ConsumptionType.Refund)
                        {
                            if (employee.DepartmentId == "1" || employee.DepartmentId == "7")
                            {
                                x.Price = -x.Price;
                            }
                            else
                            {
                                throw new Exception("只有管理员与财务方可录入退款订单，请联系对应人员操作！");
                            }
                        }
                        AddContentPlatFormOrderDealDetailsDto addContentPlatFormOrderDealDetailsDto = new AddContentPlatFormOrderDealDetailsDto();
                        addContentPlatFormOrderDealDetailsDto.GoodsName = x.GoodsName;
                        addContentPlatFormOrderDealDetailsDto.GoodsSpec = x.GoodsSpec;
                        addContentPlatFormOrderDealDetailsDto.Quantity = x.Quantity;
                        addContentPlatFormOrderDealDetailsDto.Price = x.Price;
                        addContentPlatFormOrderDealDetailsDto.CreateBy = updateDto.EmpId;
                        addContentPlatFormOrderDealDetailsDto.ContentPlatFormOrderId = updateDto.Id;
                        addContentPlatFormOrderDealDetailsDtos.Add(addContentPlatFormOrderDealDetailsDto);
                    }
                }
                updateDto.AddContentPlatFormOrderDealDetailsDtoList = addContentPlatFormOrderDealDetailsDtos;
                await _orderService.FinishContentPlateFormOrderAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Code = -1;
                operationLog.Message = ex.Message;
                throw ex;
            }
            finally
            {
                operationLog.Source = (int)RequestSource.AmiyaBusinessWechat;
                operationLog.Parameters = JsonConvert.SerializeObject(updateVo);
                operationLog.RequestType = (int)RequestType.Add;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

        /// <summary>
        /// 客服修改已完成订单
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("updateFinishContentPlateFormOrderByEmployee")]
        [FxInternalAuthorize]
        public async Task<ResultData> UpdateFinishOrderByEmployeeAsync(UpdateContentPlateFormOrderFinishVo updateVo)
        {
            OperationAddDto operationLog = new OperationAddDto();
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                UpdateContentPlateFormOrderFinishDto updateDto = new UpdateContentPlateFormOrderFinishDto();
                updateDto.Id = updateVo.Id;
                updateDto.DealId = updateVo.DealId;
                updateDto.IsFinish = updateVo.IsFinish;
                updateDto.LastDealHospitalId = updateVo.LastDealHospitalId;
                updateDto.ToHospitalDate = updateVo.ToHospitalDate;
                updateDto.DealAmount = updateVo.DealAmount;
                updateDto.LastProjectStage = updateVo.LastProjectStage;
                updateDto.DealPictureUrl = updateVo.DealPictureUrl;
                updateDto.UnDealReason = updateVo.UnDealReason;
                updateDto.IsToHospital = updateVo.IsToHospital;
                updateDto.ToHospitalType = updateVo.ToHospitalType;
                updateDto.UnDealPictureUrl = updateVo.UnDealPictureUrl;
                updateDto.DealDate = updateVo.DealDate;
                updateDto.DealPerformanceType = updateVo.DealPerformanceType;
                updateDto.CommissionRatio = updateVo.CommissionRatio;
                updateDto.IsAcompanying = updateVo.IsAcompanying;
                updateDto.OtherContentPlatFormOrderId = updateVo.OtherContentPlatFormOrderId;
                updateDto.InvitationDocuments = updateVo.InvitationDocuments;
                updateDto.ConsumptionType = updateVo.ConsumptionType;
                updateDto.FansMeetingId = updateVo.FansMeetingId;
                updateDto.EmpId = employeeId;
                updateDto.SendOrderId = updateVo.SendOrderId;
                List<AddContentPlatFormOrderDealDetailsDto> addContentPlatFormOrderDealDetailsDtos = new List<AddContentPlatFormOrderDealDetailsDto>();
                if (updateDto.IsFinish == true)
                {
                    foreach (var x in updateVo.AddContentPlatFormOrderDealDetailsVoList)
                    {
                        AddContentPlatFormOrderDealDetailsDto addContentPlatFormOrderDealDetailsDto = new AddContentPlatFormOrderDealDetailsDto();
                        addContentPlatFormOrderDealDetailsDto.GoodsName = x.GoodsName;
                        addContentPlatFormOrderDealDetailsDto.GoodsSpec = x.GoodsSpec;
                        addContentPlatFormOrderDealDetailsDto.Quantity = x.Quantity;
                        addContentPlatFormOrderDealDetailsDto.Price = x.Price;
                        addContentPlatFormOrderDealDetailsDto.CreateBy = Convert.ToInt32(employee.Id);
                        addContentPlatFormOrderDealDetailsDto.ContentPlatFormOrderId = updateDto.Id;
                        addContentPlatFormOrderDealDetailsDtos.Add(addContentPlatFormOrderDealDetailsDto);
                    }
                }
                updateDto.AddContentPlatFormOrderDealDetailsDtoList = addContentPlatFormOrderDealDetailsDtos;
                await _orderService.UpdateFinishContentPlateFormOrderAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Code = -1;
                operationLog.Message = ex.Message;
                throw ex;
            }
            finally
            {
                operationLog.Parameters = JsonConvert.SerializeObject(updateVo);
                operationLog.RequestType = (int)RequestType.Add;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData> DeleteAsync(string id)
        {
            await _orderService.DeleteOrderAsync(id);
            return ResultData.Success();
        }
        /// <summary>
        /// 获取已绑定客服的内容平台订单列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="customerServiceId"></param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="liveAnchorId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="liveAnchorWechatNoId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("bindCustomerServieOrders")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<BindCustomerServiceContentPlatformOrderVo>>> GetBindCustomerServieContentPlatformOrdersAsync(int? customerServiceId, int? orderStatus, int? liveAnchorId, DateTime? startDate, DateTime? endDate, string keyword, string liveAnchorWechatNoId, int pageNum, int pageSize)
        {
            var orders = await _orderService.GetBindCustomerServieContentPlatformOrdersAsync(customerServiceId, orderStatus, liveAnchorId, startDate, endDate, keyword, liveAnchorWechatNoId, pageNum, pageSize);
            var contentPlatformOrders = from d in orders.List
                                        select new BindCustomerServiceContentPlatformOrderVo
                                        {
                                            Id = d.Id,
                                            CreateDate = d.CreateDate,
                                            GoodsName = d.GoodsName,
                                            ThumbPictureUrl = d.ThumbPictureUrl,
                                            CustomerName = d.CustomerName,
                                            AppointmentHospitalName = d.AppointmentHospitalName,
                                            EncryptPhone = d.EncryptPhone,
                                            Phone = d.Phone,
                                            CustomerServiceName = d.CustomerServiceName,
                                        };
            FxPageInfo<BindCustomerServiceContentPlatformOrderVo> pageInfo = new FxPageInfo<BindCustomerServiceContentPlatformOrderVo>();
            pageInfo.TotalCount = orders.TotalCount;
            pageInfo.List = contentPlatformOrders;
            return ResultData<FxPageInfo<BindCustomerServiceContentPlatformOrderVo>>.Success().AddData("orders", pageInfo);
        }

        #region 枚举下拉框

        /// <summary>
        /// 获取内容平台面诊状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("getOrderConsultationTypeList")]
        [FxInternalAuthorize]
        public ResultData<List<BaseKeyAndValueVo>> GetOrderConsultationTypeList()
        {
            var orderTypes = from d in _orderService.GetOrderConsultationTypeList()
                             select new BaseKeyAndValueVo
                             {
                                 Id = d.OrderType.ToString(),
                                 Name = d.OrderTypeText
                             };
            return ResultData<List<BaseKeyAndValueVo>>.Success().AddData("orderConsultationTypes", orderTypes.ToList());
        }

        /// <summary>
        /// 获取内容平台订单类型
        /// </summary>
        /// <returns></returns>
        [FxInternalAuthorize]
        [HttpGet("contentPlateFormOrderTypeList")]
        public ResultData<List<BaseKeyAndValueVo>> GetContentPlateFormOrderTypeList()
        {
            var orderTypes = from d in _orderService.GetOrderTypeList()
                             select new BaseKeyAndValueVo
                             {
                                 Id = d.OrderType.ToString(),
                                 Name = d.OrderTypeText
                             };
            return ResultData<List<BaseKeyAndValueVo>>.Success().AddData("orderTypes", orderTypes.ToList());
        }

        /// <summary>
        /// 获取内容平台订单状态
        /// </summary>
        /// <returns></returns>
        [FxInternalAuthorize]
        [HttpGet("contentPlateFormOrderStatusList")]
        public ResultData<List<BaseKeyAndValueVo>> GetContentPlateFormOrderStatusList()
        {
            var orderStatus = from d in _orderService.GetOrderStatusList()
                              select new BaseKeyAndValueVo
                              {
                                  Id = d.OrderStatus.ToString(),
                                  Name = d.OrderStatusText
                              };
            return ResultData<List<BaseKeyAndValueVo>>.Success().AddData("orderStatus", orderStatus.ToList());
        }


        /// <summary>
        /// 获取内容平台订单来源
        /// </summary>
        /// <returns></returns>
        [FxInternalAuthorize]
        [HttpGet("contentPlateFormOrderSourceList")]
        public ResultData<List<BaseKeyAndValueVo>> GetContentPlateFormOrderSourceList()
        {
            var orderSources = from d in _orderService.GetOrderSourceList()
                               select new BaseKeyAndValueVo
                               {
                                   Id = d.OrderSource.ToString(),
                                   Name = d.OrderSourceText = d.OrderSourceText
                               };
            return ResultData<List<BaseKeyAndValueVo>>.Success().AddData("orderSources", orderSources.ToList());
        }

        /// <summary>
        /// 获取内容平台订单到院状态
        /// </summary>
        /// <returns></returns>
        [FxInternalAuthorize]
        [HttpGet("contentPlateFormOrderToHospitalTypeList")]
        public ResultData<List<BaseKeyAndValueVo>> GetContentPlateFormOrderToHospitalTypeList()
        {
            var orderTypes = from d in _orderService.GetOrderToHospitalTypeList()
                             select new BaseKeyAndValueVo
                             {
                                 Id = d.OrderType.ToString(),
                                 Name = d.OrderTypeText
                             };
            return ResultData<List<BaseKeyAndValueVo>>.Success().AddData("orderTypes", orderTypes.ToList());
        }

        /// <summary>
        /// 获客方式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("shoppingCartGetCustomerTypeList")]
        public async Task<ResultData<List<BaseKeyAndValueVo>>> GetShoppingCartGetCustomerTypeListAsync()
        {
            var nameList = shoppingCartRegistrationService.GetShoppingCartGetCustomerTypeText();
            var result = nameList.Select(e => new BaseKeyAndValueVo
            {
                Id = e.Key.ToString(),
                Name = e.Value
            }).ToList();
            return ResultData<List<BaseKeyAndValueVo>>.Success().AddData("typeList", result);
        }

        #endregion
    }
}
