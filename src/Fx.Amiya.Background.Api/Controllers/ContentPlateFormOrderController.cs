using Fx.Amiya.Background.Api.Vo.ContentPlateFormOrder;
using Fx.Amiya.Background.Api.Vo.ContentPlateFormOrder.Input;
using Fx.Amiya.Background.Api.Vo.ContentPlatFormOrderSend;
using Fx.Amiya.Background.Api.Vo.Order;
using Fx.Amiya.Background.Api.Vo.OrderCheck;
using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto.BindCustomerService;
using Fx.Amiya.Dto.ContentPlateFormOrder;
using Fx.Amiya.Dto.ContentPlatFormOrderDealDetails.Input;
using Fx.Amiya.Dto.ContentPlatFormOrderSend;
using Fx.Amiya.Dto.CustomerHospitalDealDetails.Input;
using Fx.Amiya.Dto.CustomerHospitalDealInfo.Input;
using Fx.Amiya.Dto.CustomerInfo;
using Fx.Amiya.Dto.OperationLog;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.Dto.TmallOrder;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Amiya.Service;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using Jd.Api.Util;
using jos_sdk_net.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Controllers
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
        private IAmiyaEmployeeService amiyaEmployeeService;
        private IWxAppConfigService _wxAppConfigService;
        private IBindCustomerServiceService bindCustomerServiceService;
        private IContentPlatformOrderSendService _contentPlatformOrderSend;
        private IAmiyaHospitalDepartmentService _departmentService;
        private IOrderService _tmallOrderService;
        private ICustomerHospitalDealInfoService customerHospitalDealInfoService;
        private IAmiyaPositionInfoService amiyaPositionInfoService;
        private IContentPlatFormCustomerPictureService _contentPlatFormCustomerPictureService;
        private readonly ILiveAnchorBaseInfoService liveAnchorBaseInfoService;
        private IHospitalInfoService _hospitalInfoService;
        private IHttpContextAccessor _httpContextAccessor;
        private IOperationLogService operationLogService;
        private ILiveAnchorService liveAnchorService;
        private IAmiyaOperationsBoardService amiyaOperationsBoardService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderService"></param>
        /// <param name="tmallOrderService"></param>
        /// <param name="amiyaEmployeeService"></param>
        /// <param name="bindCustomerServiceService"></param>
        /// <param name="customerHospitalDealInfoService"></param>
        /// <param name="amiyaPositionInfoService"></param>
        /// <param name="hospitalInfoService"></param>
        /// <param name="departmentService"></param>
        /// <param name="contentPlatformOrderSend"></param>
        /// <param name="customerService"></param>
        /// <param name="contentPlatFormCustomerPictureService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="wxAppConfigService"></param>
        /// <param name="operationLogService"></param>
        /// <param name="liveAnchorService"></param>
        public ContentPlateFormOrderController(IContentPlateFormOrderService orderService,
            IOrderService tmallOrderService,
            IAmiyaEmployeeService amiyaEmployeeService,
            IBindCustomerServiceService bindCustomerServiceService,
            IAmiyaOperationsBoardService amiyaOperationsBoardService,
            ICustomerHospitalDealInfoService customerHospitalDealInfoService,
            IAmiyaPositionInfoService amiyaPositionInfoService,
            IHospitalInfoService hospitalInfoService,
            IAmiyaHospitalDepartmentService departmentService,
            IContentPlatformOrderSendService contentPlatformOrderSend,
            ILiveAnchorBaseInfoService liveAnchorBaseInfoService,
           ICustomerService customerService,
            IContentPlatFormCustomerPictureService contentPlatFormCustomerPictureService,
            IHttpContextAccessor httpContextAccessor,
             IWxAppConfigService wxAppConfigService, IOperationLogService operationLogService, ILiveAnchorService liveAnchorService)
        {
            _orderService = orderService;
            _tmallOrderService = tmallOrderService;
            this.customerService = customerService;
            this.customerHospitalDealInfoService = customerHospitalDealInfoService;
            this.bindCustomerServiceService = bindCustomerServiceService;
            this.amiyaPositionInfoService = amiyaPositionInfoService;
            this.amiyaOperationsBoardService = amiyaOperationsBoardService;
            this.amiyaEmployeeService = amiyaEmployeeService;
            _departmentService = departmentService;
            _contentPlatformOrderSend = contentPlatformOrderSend;
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
            _wxAppConfigService = wxAppConfigService;
            _hospitalInfoService = hospitalInfoService;
            _httpContextAccessor = httpContextAccessor;
            _contentPlatFormCustomerPictureService = contentPlatFormCustomerPictureService;
            this.operationLogService = operationLogService;
            this.liveAnchorService = liveAnchorService;
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
                addDto.CustomerType = addVo.CustomerType;
                addDto.CustomerSource = addVo.CustomerSource;
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
                operationLog.Parameters = JsonConvert.SerializeObject(addVo);
                operationLog.RequestType = (int)RequestType.Add;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }


        }


        /// <summary>
        /// 根据条件获取内容平台订单
        /// </summary>
        /// <param name="baseLiveAnchorId">基础主播id</param>
        /// <param name="getCustomerType">获客方式</param>
        /// <param name="liveAnchorId">主播id</param>
        /// <param name="liveAnchorWechatId">主播微信号id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="appointmentStartDate">预约开始时间</param>
        /// <param name="appointmentEndDate">预约结束时间</param>
        /// <param name="keyword">关键词</param>
        /// <param name="belongMonth">归属月份</param>
        /// <param name="minAddOrderPrice">最小下单金额</param>
        /// <param name="maxAddOrderPrice">最大下单金额</param>
        /// <param name="hospitalDepartmentId">科室id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="appointmentHospital">预约医院</param>
        /// <param name="consultationType">面诊类型</param>
        /// <param name="contentPlateFormId">内容平台</param>
        /// <param name="orderSource">订单来源(-1查询全部)</param>
        /// <param name="belongEmpId">归属客服id</param>
        /// <param name="belongCompany">归属公司</param>
        /// <param name="pageNum">第/页</param>
        /// <param name="pageSize">每页显示/行</param>
        /// <returns></returns>
        [HttpGet("contentPlateFormOrderLlistWithPage")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<ContentPlatFormOrderInfoVo>>> GetOrderListWithPageAsync(string baseLiveAnchorId, int? getCustomerType, int? liveAnchorId, string liveAnchorWechatId, DateTime? startDate, DateTime? endDate, DateTime? appointmentStartDate, DateTime? appointmentEndDate, int? belongMonth, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, int? appointmentHospital, int? consultationType, string hospitalDepartmentId, string keyword, int? orderStatus, string contentPlateFormId, int? belongEmpId, int? belongCompany, int orderSource, int pageNum, int pageSize)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                List<int> liveAnchorIds = new List<int>();
                if (!string.IsNullOrEmpty(baseLiveAnchorId))
                {
                    if (!liveAnchorId.HasValue)
                    {
                        liveAnchorIds = (await liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id).ToList();
                        if (liveAnchorIds.Count <= 0)
                        {
                            FxPageInfo<ContentPlatFormOrderInfoVo> emptyPageInfo = new FxPageInfo<ContentPlatFormOrderInfoVo>();
                            emptyPageInfo.TotalCount = 0;
                            emptyPageInfo.List = new List<ContentPlatFormOrderInfoVo>();
                            return ResultData<FxPageInfo<ContentPlatFormOrderInfoVo>>.Success().AddData("contentPlatFormOrder", emptyPageInfo);
                        }
                    }
                    else
                    {
                        liveAnchorIds.Add(liveAnchorId.Value);
                    }
                }
                else
                {
                    if (liveAnchorId.HasValue)
                    {
                        liveAnchorIds.Add(liveAnchorId.Value);
                    }
                }
                var q = await _orderService.GetOrderListWithPageAsync(liveAnchorIds, getCustomerType, liveAnchorWechatId, startDate, endDate, appointmentStartDate, appointmentEndDate, belongMonth, minAddOrderPrice, maxAddOrderPrice, appointmentHospital, consultationType, hospitalDepartmentId, keyword, orderStatus, contentPlateFormId, belongEmpId, employeeId, belongCompany
                    , orderSource, pageNum, pageSize);
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
                    resultVo.IsRiBuLuoLiving = x.IsRiBuLuoLiving;
                    resultVo.BelongMonth = x.BelongMonth;
                    resultVo.AddOrderPrice = x.AddOrderPrice;
                    resultVo.CreateDate = x.CreateDate;
                    resultVo.CustomerName = x.CustomerName;
                    resultVo.GetCustomerTypeText = x.GetCustomerTypeText;
                    resultVo.IsSupportOrder = x.IsSupportOrder;
                    resultVo.SupportEmpName = x.SupportEmpName;
                    resultVo.Phone = x.Phone;
                    var customerBaseInfo = await customerService.GetCustomerBaseInfoByEncryptPhoneAsync(x.EncryptPhone);
                    resultVo.City = customerBaseInfo.City;
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
                    resultVo.BelongChannel = x.BelongChannel;
                    resultVo.BelongChannelText = x.BelongChannelText;
                    resultVo.CheckRemark = x.CheckRemark;
                    resultVo.SettlePrice = x.SettlePrice;
                    resultVo.IsReturnBackPrice = x.IsReturnBackPrice;
                    resultVo.ReturnBackDate = x.ReturnBackDate;
                    resultVo.ReturnBackPrice = x.ReturnBackPrice;
                    resultVo.OtherContentPlatFormOrderId = x.OtherContentPlatFormOrderId;
                    resultVo.IsRepeatProfundityOrder = x.IsRepeatProfundityOrder;
                    resultVo.ConsultingContent2 = x.ConsultingContent2;
                    resultVo.OrderBelongCompany = x.OrderBelongCompany;
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
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="liveAnchorId">归属主播id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="contentPlateFormId">内容平台</param>
        /// <param name="consultationEmpId">面诊员id</param>
        /// <param name="employeeId">归属客服（-1查询所有）</param>
        /// <param name="orderSource">订单来源（-1查询所有）</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("unContentPlatFormSendOrderList")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<UnContentPlateFormSendOrderInfoVo>>> GetUnSendOrderListWithPageAsync(string baseLiveAnchorId, int? liveAnchorId, DateTime? startDate, DateTime? endDate, int? consultationEmpId, string keyword, string contentPlateFormId, int? employeeId, int orderStatus, int orderSource, int pageNum, int pageSize)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int loginEmployeeId = Convert.ToInt32(employee.Id);
            List<int?> liveAnchorIds = new List<int?>();
            if (!string.IsNullOrEmpty(baseLiveAnchorId))
            {
                if (!liveAnchorId.HasValue)
                {
                    var list = (await liveAnchorService.GetLiveAnchorListByBaseInfoId(baseLiveAnchorId)).Select(e => e.Id).ToList();
                    if (list.Count <= 0)
                    {
                        FxPageInfo<UnContentPlateFormSendOrderInfoVo> emptyPageInfo = new FxPageInfo<UnContentPlateFormSendOrderInfoVo>();
                        emptyPageInfo.TotalCount = 0;
                        emptyPageInfo.List = new List<UnContentPlateFormSendOrderInfoVo>();
                        return ResultData<FxPageInfo<UnContentPlateFormSendOrderInfoVo>>.Success().AddData("unSendOrder", emptyPageInfo);
                    }
                    foreach (var item in list)
                    {
                        liveAnchorIds.Add(item);
                    }
                }
                else
                {
                    liveAnchorIds.Add(liveAnchorId);
                }
            }
            else
            {
                if (liveAnchorId.HasValue)
                {
                    liveAnchorIds.Add(liveAnchorId.Value);
                }
            }
            var q = await _orderService.GetUnSendOrderListWithPageAsync(liveAnchorIds, keyword, startDate, endDate, consultationEmpId, (int)loginEmployeeId, employeeId, orderStatus, contentPlateFormId, orderSource, pageNum, pageSize);
            var unSendOrder = from d in q.List
                              select new UnContentPlateFormSendOrderInfoVo
                              {
                                  OrderId = d.OrderId,
                                  ContentPlatFormName = d.ContentPlatFormName,
                                  LiveAnchorName = d.LiveAnchorName,
                                  GoodsName = d.GoodsName,
                                  BelongEmpName = d.BelongEmpName,
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
                                  AppointmentHospitalId = d.AppointmentHospitalId,
                                  AppointmentHospital = d.AppointmentHospital,
                                  AppointmentDate = d.AppointmentDate,
                                  Remark = d.Remark,
                                  LateProjectStage = d.LateProjectStage,
                                  UnSendReason = d.UnSendReason,
                                  OrderSourceText = d.OrderSourceText,
                                  ConsultingContent2 = d.ConsultingContent2
                              };
            FxPageInfo<UnContentPlateFormSendOrderInfoVo> pageInfo = new FxPageInfo<UnContentPlateFormSendOrderInfoVo>();
            pageInfo.TotalCount = q.TotalCount;
            pageInfo.List = unSendOrder;
            return ResultData<FxPageInfo<UnContentPlateFormSendOrderInfoVo>>.Success().AddData("unSendOrder", pageInfo);
        }

        /// <summary>
        /// 修改内容平台订单归属客服
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("BelongEmployeeContentPlatFormOrder")]
        public async Task<ResultData> BelongEmployeeOrderAsync(BelongEmpInfoOrderVo input)
        {
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Source = (int)RequestSource.AmiyaBackground;
            operationLog.Code = 0;
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                UpdateBelongEmpInfoOrderDto dto = new UpdateBelongEmpInfoOrderDto();
                dto.OrderId = input.OrderId;
                dto.BelongEmpId = input.BelongEmpInfo;
                input.OriginalCustomerServiceIds = await _orderService.UpdateOrderBelongEmpIdAsync(dto);
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
                operationLog.Parameters = JsonConvert.SerializeObject(input);
                operationLog.RequestType = (int)RequestType.Update;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

        /// <summary>
        /// 根据条件导出内容平台订单
        /// </summary>
        /// <param name="query">开始时间</param>
        /// <returns></returns>
        [HttpGet("exportContentPlateFormOrderLlistWithPage")]
        [FxInternalAuthorize]
        public async Task<FileStreamResult> ExportOrderListWithPageAsync([FromQuery] QueryExportContentPlateFormOrderLlistWithPage query)
        {
            List<ExportContentPlatFormOrderInfoVo> exportSendOrder = new List<ExportContentPlatFormOrderInfoVo>();
            OperationAddDto operationAddDto = new OperationAddDto();
            operationAddDto.Code = 0;
            try
            {
                bool isHidePhone = true;
                if (query.StartDate.HasValue == false || query.EndDate.HasValue == false)
                {
                    throw new Exception("请选择录单时间进行导出！");
                }
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationAddDto.OperationBy = employeeId;
                if (employee.DepartmentId == "1" || employee.DepartmentId == "7")
                {
                    isHidePhone = false;
                }
                List<int?> liveAnchorIds = new List<int?>();
                if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
                {
                    if (!query.LiveAnchorId.HasValue)
                    {
                        var list = (await liveAnchorService.GetLiveAnchorListByBaseInfoId(query.BaseLiveAnchorId)).Select(e => e.Id).ToList();
                        if (list.Count <= 0)
                        {
                            var resultStream = ExportExcelHelper.ExportExcel(exportSendOrder);
                            var finalResult = File(resultStream, "application/vnd.ms-excel", $"" + query.StartDate.Value.ToString("yyyy年MM月dd日") + "-" + query.EndDate.Value.ToString("yyyy年MM月dd日") + "内容平台订单报表.xls");
                            return finalResult;
                        }
                        foreach (var item in list)
                        {
                            liveAnchorIds.Add(item);
                        }
                    }
                    else
                    {
                        liveAnchorIds.Add(query.LiveAnchorId.Value);
                    }
                }
                else
                {
                    if (query.LiveAnchorId.HasValue)
                    {
                        liveAnchorIds.Add(query.LiveAnchorId.Value);
                    }
                }
                var q = await _orderService.ExportOrderListWithPageAsync(query.StartDate, query.EndDate, query.ConsultationEmpId, query.AppointmentHospital, query.BelongEmpId, liveAnchorIds, query.Keyword, query.HospitalDepartmentId, query.OrderStatus, query.OrderSource, query.ContentPlateFormId, employeeId, isHidePhone);

                #region 【注释代码】
                //var resutList = q.ToList();
                //List<ContentPlatFormOrderInfoVo> contentPlatFormOrderInfoVoList = new List<ContentPlatFormOrderInfoVo>();
                //foreach (var x in resutList)
                //{
                //    ContentPlatFormOrderInfoVo resultVo = new ContentPlatFormOrderInfoVo();
                //    resultVo.Id = x.Id;
                //    resultVo.OrderTypeText = x.OrderTypeText;
                //    resultVo.ContentPlatformName = x.ContentPlatformName;
                //    resultVo.LiveAnchorName = x.LiveAnchorName;
                //    resultVo.LiveAnchorWeChatNo = x.LiveAnchorWeChatNo;
                //    resultVo.ConsultationType = x.ConsultationTypeText;
                //    resultVo.BelongMonth = x.BelongMonth;
                //    resultVo.AddOrderPrice = x.AddOrderPrice;
                //    resultVo.CreateDate = x.CreateDate;
                //    resultVo.CustomerName = x.CustomerName;
                //    resultVo.Phone = x.Phone;
                //    //var customerBaseInfo = await customerService.GetCustomerBaseInfoByEncryptPhoneAsync(x.EncryptPhone);
                //    //resultVo.City = customerBaseInfo.City;
                //    resultVo.AppointmentDate = x.AppointmentDate == null ? "未预约时间" : x.AppointmentDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                //    resultVo.AppointmentHospitalName = x.AppointmentHospitalName;
                //    resultVo.GoodsName = x.GoodsName;
                //    resultVo.ThumbPictureUrl = x.ThumbPictureUrl;
                //    resultVo.ConsultingContent = x.ConsultingContent;
                //    resultVo.OrderStatusText = x.OrderStatusText;
                //    resultVo.DepositAmount = x.DepositAmount;
                //    resultVo.IsToHospital = x.IsToHospital;
                //    resultVo.ToHospitalDate = x.ToHospitalDate;
                //    resultVo.LastDealHospital = x.LastDealHospital;
                //    resultVo.DealAmount = x.DealAmount;
                //    resultVo.DealDate = x.DealDate;
                //    resultVo.UnDealReason = x.UnDealReason;
                //    resultVo.LateProjectStage = x.LateProjectStage;
                //    resultVo.Remark = x.Remark;
                //    resultVo.DepartmentName = x.DepartmentName;
                //    resultVo.BelongEmpName = x.BelongEmpName;
                //    resultVo.UnSendReason = x.UnSendReason;
                //    resultVo.OrderSourceText = x.OrderSourceText;
                //    resultVo.AcceptConsulting = x.AcceptConsulting;
                //    resultVo.CheckStateText = x.CheckStateText;
                //    resultVo.CheckDate = x.CheckDate;
                //    resultVo.CheckByName = x.CheckByName;
                //    resultVo.CheckPrice = x.CheckPrice;
                //    resultVo.Sender = x.Sender;
                //    resultVo.SendDate = x.SendDate;

                //    resultVo.CheckRemark = x.CheckRemark;
                //    resultVo.SettlePrice = x.SettlePrice;
                //    resultVo.IsReturnBackPrice = x.IsReturnBackPrice;
                //    resultVo.ReturnBackDate = x.ReturnBackDate;
                //    resultVo.ReturnBackPrice = x.ReturnBackPrice;
                //    resultVo.OtherContentPlatFormOrderId = x.OtherContentPlatFormOrderId;

                //    //if (x.BelongEmpId != 0)
                //    //{
                //    //    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.BelongEmpId.Value);
                //    //    x.BelongEmpName = empInfo.Name.ToString();
                //    //}
                //    //if (x.CheckBy != 0)
                //    //{
                //    //    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CheckBy.Value);
                //    //    x.CheckByName = empInfo.Name.ToString();
                //    //}
                //    //if (!string.IsNullOrEmpty(x.GoodsDepartmentId))
                //    //{
                //    //    var departmentInfo = await _departmentService.GetByIdAsync(x.GoodsDepartmentId);
                //    //    x.DepartmentName = departmentInfo.DepartmentName;
                //    //}
                //    //if (x.LastDealHospitalId.HasValue)
                //    //{
                //    //    var hospitalInfo = await _hospitalInfoService.GetBaseByIdAsync(x.LastDealHospitalId.Value);
                //    //    x.LastDealHospital = hospitalInfo.Name;
                //    //}
                //    //if (x.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder)
                //    //{
                //    //    var sendOrderInfoList = await _contentPlatformOrderSend.GetSendOrderInfoByOrderId(x.Id);
                //    //    var sendOrderInfo = sendOrderInfoList.OrderByDescending(z => z.SendDate).FirstOrDefault();
                //    //    if (sendOrderInfo != null)
                //    //    {
                //    //        x.SendDate = sendOrderInfo.SendDate;
                //    //        var empInfo = await amiyaEmployeeService.GetByIdAsync(sendOrderInfo.Sender);
                //    //        x.Sender = empInfo.Name;
                //    //    }
                //    //}
                //    contentPlatFormOrderInfoVoList.Add(resultVo);
                //}
                #endregion

                var order = from d in q
                            select new ExportContentPlatFormOrderInfoVo
                            {
                                Id = d.Id,
                                OrderTypeText = d.OrderTypeText,
                                ContentPlatformName = d.ContentPlatformName,
                                LiveAnchorName = d.LiveAnchorName,
                                CreateDate = d.CreateDate,
                                BelongMonth = d.BelongMonth == 0 ? "当月" : "次月",
                                City = d.City,
                                AddOrderPrice = d.AddOrderPrice,
                                CustomerName = d.CustomerName,
                                Phone = d.Phone,
                                EncryptPhone = d.EncryptPhone,
                                AppointmentDate = d.AppointmentDate == null ? "未预约时间" : d.AppointmentDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                AppointmentHospitalName = d.AppointmentHospitalName,
                                GoodsName = d.GoodsName,
                                DepartmentName = d.DepartmentName,
                                //ThumbPictureUrl = d.ThumbPictureUrl,
                                IsOldCustomer = d.IsOldCustomer == true ? "老客业绩" : "新客业绩",
                                ConsultingContent = d.ConsultingContent,
                                IsToHospital = d.IsToHospital == true ? "是" : "否",
                                ToHospitalDate = d.ToHospitalDate,
                                LastDealHospital = d.LastDealHospital,
                                OrderStatusText = d.OrderStatusText,
                                Sender = d.Sender,
                                SendDate = d.SendDate,
                                LiveAnchorWeChatNo = d.LiveAnchorWeChatNo,
                                DepositAmount = d.DepositAmount,
                                DealAmount = d.DealAmount,
                                UnDealReason = d.UnDealReason,
                                LateProjectStage = d.LateProjectStage,
                                Remark = d.Remark,
                                DealDate = d.DealDate,
                                OrderSourceText = d.OrderSourceText,
                                UnSendReason = d.UnSendReason,
                                AcceptConsulting = d.AcceptConsulting,
                                //CheckStateText = d.CheckStateText,
                                //CheckDate = d.CheckDate,
                                //CheckByName = d.CheckByName,
                                //CheckPrice = d.CheckPrice,
                                ConsultationType = d.ConsultationTypeText,
                                //CheckRemark = d.CheckRemark,
                                //SettlePrice = d.SettlePrice,
                                BelongEmpName = d.BelongEmpName,
                                //IsReturnBackPrice = d.IsReturnBackPrice,
                                //ReturnBackDate = d.ReturnBackDate,
                                //ReturnBackPrice = d.ReturnBackPrice,
                                OtherContentPlatFormOrderId = d.OtherContentPlatFormOrderId,
                                SendHospital = d.SendHospital,
                                IsRepeatProfundityOrder = d.IsRepeatProfundityOrder == true ? "是" : "否",
                                ConsultingContent2 = d.ConsultingContent2
                            };
                exportSendOrder = order.ToList();
                foreach (var x in exportSendOrder)
                {

                    var customerBaseInfo = await customerService.GetCustomerBaseInfoByEncryptPhoneAsync(x.EncryptPhone);
                    x.City = customerBaseInfo.City;


                    //if (x.BelongEmpId != 0)
                    //{
                    //    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.BelongEmpId.Value);
                    //    x.BelongEmpName = empInfo.Name.ToString();
                    //}
                    //if (x.CheckBy != 0)
                    //{
                    //    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CheckBy.Value);
                    //    x.CheckByName = empInfo.Name.ToString();
                    //}
                    //if (!string.IsNullOrEmpty(x.GoodsDepartmentId))
                    //{
                    //    var departmentInfo = await _departmentService.GetByIdAsync(x.GoodsDepartmentId);
                    //    x.DepartmentName = departmentInfo.DepartmentName;
                    //}
                    //if (x.LastDealHospitalId.HasValue)
                    //{
                    //    var hospitalInfo = await _hospitalInfoService.GetBaseByIdAsync(x.LastDealHospitalId.Value);
                    //    x.LastDealHospital = hospitalInfo.Name;
                    //}
                }
                var stream = ExportExcelHelper.ExportExcel(exportSendOrder);
                //var stream = ExportExcelHelper.ExportExcel(contentPlatFormOrderInfoVoList);
                var result = File(stream, "application/vnd.ms-excel", $"" + query.StartDate.Value.ToString("yyyy年MM月dd日") + "-" + query.EndDate.Value.ToString("yyyy年MM月dd日") + "内容平台订单报表.xls");
                return result;
            }
            catch (Exception err)
            {
                operationAddDto.Code = -1;
                operationAddDto.Message = err.Message.ToString();
                throw new Exception(err.Message.ToString());
            }
            finally
            {


                operationAddDto.Parameters = JsonConvert.SerializeObject(query);
                operationAddDto.RequestType = (int)RequestType.Export;
                operationAddDto.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationAddDto);
            }
        }


        /// <summary>
        /// 根据条件获取已成交内容平台订单
        /// </summary>
        /// <param name="liveAnchorId">主播ID</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="belongMonth">归属月份</param>
        /// <param name="minAddOrderPrice">最小下单金额</param>
        /// <param name="maxAddOrderPrice">最大下单金额</param>
        /// <param name="consultationEmpId">面诊人员</param>
        /// <param name="hospitalId">医院id</param>
        /// <param name="contentPlateFormId">内容平台</param>
        /// <param name="keyword">关键词</param>
        /// <param name="checkState">审核状态,为空查询所有</param>
        /// <param name="ReturnBackPriceState">回款状态,为空查询所有</param>
        /// <param name="toHospitalType">到院类型,为空查询所有</param>
        /// <param name="pageNum">第/页</param>
        /// <param name="pageSize">每页显示/行</param>
        /// <returns></returns>
        [HttpGet("contentPlateFormOrderDealLlistWithPage")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<ContentPlatFormCompleteOrderInfoVo>>> GetOrderDealListWithPageAsync(int? liveAnchorId, DateTime? startDate, DateTime? endDate, int? belongMonth, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, int? consultationEmpId, int? checkState, bool? ReturnBackPriceState, string keyword, int? hospitalId, int? toHospitalType, string contentPlateFormId, int pageNum, int pageSize)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                var q = await _orderService.GetOrderDealListWithPageAsync(liveAnchorId, startDate, endDate, belongMonth, minAddOrderPrice, maxAddOrderPrice, consultationEmpId, checkState, ReturnBackPriceState, keyword, contentPlateFormId, hospitalId, toHospitalType, employeeId, pageNum, pageSize);
                var order = from d in q.List
                            select new ContentPlatFormCompleteOrderInfoVo
                            {
                                Id = d.Id,
                                OrderTypeText = d.OrderTypeText,
                                ContentPlatformName = d.ContentPlatformName,
                                LiveAnchorName = d.LiveAnchorName,
                                LiveAnchorWeChatNo = d.LiveAnchorWeChatNo,
                                CreateDate = d.CreateDate,
                                BelongMonth = d.BelongMonth,
                                AddOrderPrice = d.AddOrderPrice,
                                ConsultationTypeText = d.ConsultationTypeText,
                                CustomerName = d.CustomerName,
                                IsAcompanying = d.IsAcompanying,
                                Phone = d.Phone,
                                IsToHospital = d.IsToHospital == true ? "是" : "否",
                                ToHospitalType = d.ToHospitalTypeText,
                                AppointmentDate = d.AppointmentDate == null ? "未预约时间" : d.AppointmentDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                AppointmentHospitalName = d.AppointmentHospitalName,
                                GoodsName = d.GoodsName,
                                ThumbPictureUrl = d.ThumbPictureUrl,
                                ConsultingContent = d.ConsultingContent,
                                OrderStatusText = d.OrderStatusText,
                                DepositAmount = d.DepositAmount,
                                DealAmount = d.DealAmount,
                                DealDate = d.DealDate,
                                UnDealReason = d.UnDealReason,
                                LateProjectStage = d.LateProjectStage,
                                Remark = d.Remark,
                                CheckStateText = d.CheckStateText,
                                CheckPrice = d.CheckPrice,
                                CheckRemark = d.CheckRemark,
                                SettlePrice = d.SettlePrice,
                                CheckDate = d.CheckDate,
                                BelongEmpName = d.BelongEmpName,
                                IsReturnBackPrice = d.IsReturnBackPrice,
                                ReturnBackDate = d.ReturnBackDate,
                                ReturnBackPrice = d.ReturnBackPrice,
                                OtherContentPlatFormOrderId = d.OtherContentPlatFormOrderId,
                                CommissionRatio = d.CommissionRatio,
                                IsOldCustomer = d.IsOldCustomer == false ? "新客业绩" : "老客业绩",
                                CustomerServiceSettlePrice = d.CustomerServiceSettlePrice,
                                ConsultingContent2 = d.ConsultingContent2
                            };
                FxPageInfo<ContentPlatFormCompleteOrderInfoVo> orderPageInfo = new FxPageInfo<ContentPlatFormCompleteOrderInfoVo>();
                orderPageInfo.TotalCount = q.TotalCount;
                orderPageInfo.List = order;
                return ResultData<FxPageInfo<ContentPlatFormCompleteOrderInfoVo>>.Success().AddData("contentPlatFormOrder", orderPageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<ContentPlatFormCompleteOrderInfoVo>>.Fail(ex.Message);
            }
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
            orderUpdateInfo.LiveAnchorId = order.LiveAnchorId;
            orderUpdateInfo.GoodsId = order.GoodsId;
            orderUpdateInfo.CustomerName = order.CustomerName;
            orderUpdateInfo.Phone = order.Phone;
            orderUpdateInfo.GetCustomerType = order.GetCustomerType;
            orderUpdateInfo.GetCustomerTypeText = order.GetCustomerTypeText;
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
            orderUpdateInfo.CustomerServiceSettlePrice = order.CustomerServiceSettlePrice;
            orderUpdateInfo.CustomerType = order.CustomerType;
            orderUpdateInfo.CustomerTypeText = order.CustomerTypeText;
            orderUpdateInfo.CustomerSource = order.CustomerSource;
            orderUpdateInfo.CustomerSourceText = order.CustomerSourceText;
            orderUpdateInfo.BelongChannel = order.BelongChannel;
            orderUpdateInfo.BelongChannelText = order.BelongChannelText;
            orderUpdateInfo.ConsultingContent2 = order.ConsultingContent2;
            orderUpdateInfo.IsRiBuLuoLiving = order.IsRiBuLuoLiving;
            orderUpdateInfo.BelongCompanyEnumId = order.BelongCompanyEnumId;
            orderUpdateInfo.BelongCompany = order.BelongCompanyName;
            return ResultData<ContentPlateFormOrderVo>.Success().AddData("orderInfo", orderUpdateInfo);
        }


        /// <summary>
        /// 生成订单喜报
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("orderProsperity/{id}")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<OrderProsperityVo>> OrderProsperityAsync(string id)
        {
            var order = await _orderService.GetByOrderIdAsync(id);
            if (order.OrderStatus != (int)ContentPlateFormOrderStatus.OrderComplete)
            { throw new Exception("该订单暂未成交，无法生成喜报！"); }
            OrderProsperityVo orderUpdateInfo = new OrderProsperityVo();
            orderUpdateInfo.Price = order.DepositAmount.Value + order.DealAmount.Value;
            orderUpdateInfo.LiveAnchorName = order.LiveAnchorName;
            orderUpdateInfo.hospitalPicture = order.SendHospitaPicture;
            orderUpdateInfo.DealDate = order.DealDate;
            return ResultData<OrderProsperityVo>.Success().AddData("orderInfo", orderUpdateInfo);
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
        /// 根据加密手机号获取简易的订单列表
        /// </summary>
        /// <param name="encryptPhone">加密手机号</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("listByEncryptPhone")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<ContentPlatFormOrderInfoSimpleVo>>> GetListByEncryptPhoneAsync(string encryptPhone, int pageNum, int pageSize)
        {
            try
            {
                var q = await _orderService.GetListByEncryptPhoneAsync(encryptPhone, pageNum, pageSize);

                var order = from d in q.List
                            select new ContentPlatFormOrderInfoSimpleVo
                            {
                                Id = d.Id,
                                ContentPlatformName = d.ContentPlatformName,
                                LiveAnchorName = d.LiveAnchorName,
                                GoodsName = d.GoodsName,
                                ConsultingContent = d.ConsultingContent,
                                ConsultingContent2 = d.ConsultingContent2,
                                DealAmount = d.DealAmount,
                                DepositAmount = d.DepositAmount.HasValue ? d.DepositAmount : 0,
                                OrderTypeText = d.OrderTypeText,
                                OrderStatusText = d.OrderStatusText,
                                AppointmentHospitalName = d.AppointmentHospitalName,
                                AppointmentDate = d.AppointmentDate,
                                UnDealReason = d.UnDealReason,
                                LateProjectStage = d.LateProjectStage,
                                Remark = d.Remark,

                            };

                FxPageInfo<ContentPlatFormOrderInfoSimpleVo> orderPageInfo = new FxPageInfo<ContentPlatFormOrderInfoSimpleVo>();
                orderPageInfo.TotalCount = q.TotalCount;
                orderPageInfo.List = order;
                orderPageInfo.PageSize = pageSize;
                orderPageInfo.CurrentPageIndex = pageNum;
                return ResultData<FxPageInfo<ContentPlatFormOrderInfoSimpleVo>>.Success().AddData("order", orderPageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<ContentPlatFormOrderInfoSimpleVo>>.Fail(ex.Message);
            }
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
            updateDto.GetCustomerType = updateVo.GetCustomerType;
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
            updateDto.CustomerType = updateVo.CustomerType;
            updateDto.CustomerSource = updateVo.CustomerSource;
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
        /// 审核订单
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("checkContentPlateFormOrder")]
        [FxInternalAuthorize]
        public async Task<ResultData> CheckOrderAsync(ContentPlateFormOrderCheckVo updateVo)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            if (employee.PositionId != "1")
            {
                if (employee.PositionId != "13")
                {
                    throw new Exception("只有管理员与财务才可进行订单审核！");
                }
            }
            //修改订单
            ContentPlateFormOrderCheckDto updateDto = new ContentPlateFormOrderCheckDto();
            updateDto.Id = updateVo.Id;
            updateDto.OrderDealInfoId = updateVo.OrderDealInfoId;
            updateDto.CheckPrice = updateVo.CheckPrice;
            updateDto.CheckState = updateVo.CheckState;
            updateDto.SettlePrice = updateVo.SettlePrice;
            updateDto.CustomerServiceSettlePrice = updateVo.CustomerServiceSettlePrice;
            updateDto.employeeId = employeeId;
            updateDto.CheckRemark = updateVo.CheckRemark;
            updateDto.SystemUpdatePrice = updateVo.SystemUpdatePrice;
            updateDto.InformationPrice = updateVo.InformationPrice;
            updateDto.CheckPicture = updateVo.CheckPicture;
            updateDto.ReconciliationDocumentsId = updateVo.ReconciliationDocumentsId;
            updateDto.HospitalId = updateVo.HospitalId;
            await _orderService.CheckContentPlateFormOrderAsync(updateDto);
            return ResultData.Success();
        }

        /// <summary>
        /// 订单审核后回款
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("returnBackOrder")]
        [FxInternalAuthorize]
        public async Task<ResultData> ReturnBackOrderAsync(ReturnBackOrderVo updateVo)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            if (employee.PositionId != "1")
            {
                if (employee.PositionId != "13")
                {
                    throw new Exception("只有管理员与财务才可进行订单回款！");
                }
            }
            //修改订单
            ReturnBackOrderDto updateDto = new ReturnBackOrderDto();
            updateDto.OrderId = updateVo.OrderId;
            updateDto.OrderDealId = updateVo.OrderDealId;
            updateDto.ReturnBackPrice = updateVo.ReturnBackPrice;
            updateDto.ReturnBackDate = updateVo.ReturnBackDate;
            await _orderService.ReturnBackOrderAsync(updateDto);
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
            operationLog.Source = (int)RequestSource.AmiyaBackground;
            operationLog.Code = 0;
            try
            {

                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                AddContentPlatFormSendOrderInfoDto addDto = new AddContentPlatFormSendOrderInfoDto();
                addDto.HospitalId = addVo.HospitalId;
                addDto.OtherHospitalId = addVo.OtherHospitalId;
                addDto.OrderId = addVo.OrderId;
                addDto.IsUncertainDate = addVo.IsUncertainDate;
                addDto.AppointmentDate = addVo.AppointmentDate;
                addDto.Remark = addVo.Remark;
                addDto.SendBy = addVo.SendBy;
                addDto.EmployeeId = employeeId;
                addDto.IsSpecifyHospitalEmployee = addVo.IsSpecifyHospitalEmployee;
                addDto.HospitalEmployeeId = addVo.HospitalEmployeeId;
                //await _orderService.SendOrderAsync(addDto);
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
        /// 医院接单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="netWorkConsulationName">医院网咨人员</param>
        /// <param name="sceneConsulationName">医院现场咨询人员</param>
        /// <param name="sendOrderId">派单编号</param>
        /// <returns></returns>
        [HttpGet("contentPlateFormOrderConfirm")]
        [FxTenantAuthorize]
        public async Task<ResultData> RepeateOrderAsync(int sendOrderId, string orderId, string netWorkConsulationName, string sceneConsulationName)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaHospitalEmployeeIdentity;
            int hospitalempId = Convert.ToInt32(employee.Id);
            int hospitalId = Convert.ToInt32(employee.HospitalId);
            await _orderService.HospitalConfirmOrderAsync(sendOrderId, orderId, hospitalempId, hospitalId, netWorkConsulationName, sceneConsulationName);
            return ResultData.Success();
        }
        /// <summary>
        /// 医院重单打回
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("contentPlateFormOrderRepeate")]
        [FxTenantAuthorize]
        public async Task<ResultData> RepeateOrderAsync(ContentPlateFormOrderRepeateVo updateVo)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaHospitalEmployeeIdentity;
            int hospitalempId = Convert.ToInt32(employee.Id);
            int hospitalId = Convert.ToInt32(employee.HospitalId);
            ContentPlateFormOrderRepeateDto updateDto = new ContentPlateFormOrderRepeateDto();
            updateDto.Id = updateVo.Id;
            updateDto.OrderId = updateVo.OrderId;
            updateDto.RepeatePictureUrl = updateVo.RepeatePictureUrl;
            updateDto.ToHospitalDate = updateVo.ToHospitalDate;
            updateDto.IsProfundity = updateVo.IsProfundity;
            updateDto.SceneConsulationName = updateVo.SceneConsulationName;
            updateDto.NetWorkConsulationName = updateVo.NetWorkConsulationName;
            await _orderService.RepeateContentPlateFormOrderAsync(updateDto, hospitalempId, hospitalId);
            return ResultData.Success();
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
            operationLog.Source = (int)RequestSource.AmiyaBackground;
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
                updateDto.OtherHospitalId = updateVo.OtherHospitalId;
                updateDto.SendBy = updateVo.SendBy;
                updateDto.IsSpecifyHospitalEmployee = updateVo.IsSpecifyHospitalEmployee;
                updateDto.HospitalEmployeeId = updateVo.HospitalEmployeeId;
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
                                            OrderType = d.OrderType,
                                            OrderTypeText = d.OrderTypeText,
                                            ContentPlatformId = d.ContentPlatformId,
                                            ContentPlatformName = d.ContentPlatformName,
                                            LiveAnchorId = d.LiveAnchorId,
                                            LiveAnchorName = d.LiveAnchorName,
                                            LiveAnchorWeChatNo = d.LiveAnchorWeChatNo,
                                            CreateDate = d.CreateDate,
                                            UpdateDate = d.UpdateDate,
                                            GoodsId = d.GoodsId,
                                            GoodsName = d.GoodsName,
                                            ThumbPictureUrl = d.ThumbPictureUrl,
                                            CustomerName = d.CustomerName,
                                            AppointmentDate = d.AppointmentDate,
                                            AppointmentHospitalId = d.AppointmentHospitalId,
                                            AppointmentHospitalName = d.AppointmentHospitalName,
                                            OrderStatus = d.OrderStatus,
                                            OrderStatusText = d.OrderStatusText,
                                            Remark = d.Remark,
                                            EncryptPhone = d.EncryptPhone,
                                            Phone = d.Phone,
                                            CustomerServiceId = d.CustomerServiceId,
                                            CustomerServiceName = d.CustomerServiceName,
                                        };
            FxPageInfo<BindCustomerServiceContentPlatformOrderVo> pageInfo = new FxPageInfo<BindCustomerServiceContentPlatformOrderVo>();
            pageInfo.TotalCount = orders.TotalCount;
            pageInfo.List = contentPlatformOrders;
            return ResultData<FxPageInfo<BindCustomerServiceContentPlatformOrderVo>>.Success().AddData("orders", pageInfo);
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
                updateDto.DealPerformanceType = updateVo.DealPerformanceType.HasValue ? updateVo.DealPerformanceType.Value : 0;
                updateDto.UnDealPictureUrl = updateVo.UnDealPictureUrl;
                updateDto.DealDate = updateVo.DealDate;
                updateDto.OtherContentPlatFormOrderId = updateVo.OtherContentPlatFormOrderId;
                updateDto.EmpId = Convert.ToInt32(employee.Id);
                updateDto.ConsumptionType = updateVo.ConsumptionType;
                updateDto.InvitationDocuments = updateVo.InvitationDocuments;
                updateDto.FansMeetingId = updateVo.FansMeetingId;
                updateDto.LastDealInfoId = updateVo.LastDealInfoId;

                updateDto.FansMeetingProject = updateVo.FansMeetingProject;
                updateDto.FollowUpContent = updateVo.FollowUpContent;
                updateDto.NextAppointmentDate = updateVo.NextAppointmentDate;
                updateDto.IsNeedHospitalHelp = updateVo.IsNeedHospitalHelp;
                updateDto.SendOrderId = updateVo.SendOrderId;
                List<AddContentPlatFormOrderDealDetailsDto> addContentPlatFormOrderDealDetailsDtos = new List<AddContentPlatFormOrderDealDetailsDto>();
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
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
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
                updateDto.UpdateBy = Convert.ToInt32(employee.Id);
                updateDto.IsAcompanying = updateVo.IsAcompanying;
                updateDto.OtherContentPlatFormOrderId = updateVo.OtherContentPlatFormOrderId;
                updateDto.InvitationDocuments = updateVo.InvitationDocuments;
                updateDto.ConsumptionType = updateVo.ConsumptionType;
                updateDto.FansMeetingId = updateVo.FansMeetingId;
                updateDto.FansMeetingProject = updateVo.FansMeetingProject;
                updateDto.FollowUpContent = updateVo.FollowUpContent;
                updateDto.NextAppointmentDate = updateVo.NextAppointmentDate;
                updateDto.IsNeedHospitalHelp = updateVo.IsNeedHospitalHelp;
                updateDto.EmpId = employeeId;

                List<AddContentPlatFormOrderDealDetailsDto> addContentPlatFormOrderDealDetailsDtos = new List<AddContentPlatFormOrderDealDetailsDto>();
                if (updateDto.IsFinish == true)
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
        /// 医院完成订单
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("finishContentPlateFormOrder")]
        [FxTenantAuthorize]
        public async Task<ResultData> FinishOrderAsync(ContentPlateFormOrderFinishVo updateVo)
        {
            if (updateVo.ToHospitalType == (int)ContentPlateFormOrderToHospitalType.REFUND || updateVo.ConsumptionType == (int)ConsumptionType.Refund)
            {
                updateVo.DealAmount = -updateVo.DealAmount;
            }
            //修改订单
            ContentPlateFormOrderFinishDto updateDto = new ContentPlateFormOrderFinishDto();
            updateDto.Id = updateVo.Id;
            updateDto.IsFinish = updateVo.IsFinish;
            updateDto.DealAmount = updateVo.DealAmount;
            updateDto.LastDealHospitalId = updateVo.LastDealHospitalId;
            updateDto.ToHospitalDate = updateVo.ToHospitalDate;
            updateDto.LastProjectStage = updateVo.LastProjectStage;
            updateDto.DealPictureUrl = updateVo.DealPictureUrl;
            updateDto.UnDealReason = updateVo.UnDealReason;
            updateDto.IsToHospital = updateVo.IsToHospital;
            updateDto.UnDealPictureUrl = updateVo.UnDealPictureUrl;
            updateDto.DealDate = updateVo.DealDate;
            updateDto.ToHospitalType = updateVo.ToHospitalType;
            updateDto.IsAcompanying = updateVo.IsAcompanying;
            updateDto.DealPerformanceType = (int)ContentPlateFormOrderDealPerformanceType.HospitalDeclaration;
            updateDto.InvitationDocuments = updateVo.InvitationDocuments;
            updateDto.ConsumptionType = updateVo.ConsumptionType;
            updateDto.EmpId = 266;
            updateDto.SendOrderId = updateVo.SendOrderId;
            List<AddContentPlatFormOrderDealDetailsDto> addContentPlatFormOrderDealDetailsDtos = new List<AddContentPlatFormOrderDealDetailsDto>();
            if (updateDto.IsFinish == true)
            {
                foreach (var x in updateVo.AddContentPlatFormOrderDealDetailsVoList)
                {
                    if (updateVo.ToHospitalType == (int)ContentPlateFormOrderToHospitalType.REFUND || updateVo.ConsumptionType == (int)ConsumptionType.Refund)
                    {
                        x.Price = -x.Price;

                    }
                    AddContentPlatFormOrderDealDetailsDto addContentPlatFormOrderDealDetailsDto = new AddContentPlatFormOrderDealDetailsDto();
                    addContentPlatFormOrderDealDetailsDto.GoodsName = x.GoodsName;
                    addContentPlatFormOrderDealDetailsDto.GoodsSpec = x.GoodsSpec;
                    addContentPlatFormOrderDealDetailsDto.Quantity = x.Quantity;
                    addContentPlatFormOrderDealDetailsDto.Price = x.Price;
                    addContentPlatFormOrderDealDetailsDto.CreateBy = 0;
                    addContentPlatFormOrderDealDetailsDto.ContentPlatFormOrderId = updateDto.Id;
                    addContentPlatFormOrderDealDetailsDtos.Add(addContentPlatFormOrderDealDetailsDto);
                }
            }
            updateDto.AddContentPlatFormOrderDealDetailsDtoList = addContentPlatFormOrderDealDetailsDtos;
            await _orderService.FinishContentPlateFormOrderAsync(updateDto);
            return ResultData.Success();
        }
        /// <summary>
        /// 根据加密手机号获取内容平台订单简易信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getContentPlateFormOrderSimpleInfo")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<ContentPlateformOrderSimpleInfoVo>>> GetContentPlateformOrderSimpleInfoAsync(string phone, int pageNum, int pageSize)
        {
            FxPageInfo<ContentPlateformOrderSimpleInfoVo> fxPageInfo = new FxPageInfo<ContentPlateformOrderSimpleInfoVo>();
            var result = await _orderService.GetContentOrderInfoByEncryPhone(phone, pageNum, pageSize);
            fxPageInfo.TotalCount = result.TotalCount;
            fxPageInfo.List = result.List.Select(e => new ContentPlateformOrderSimpleInfoVo
            {
                Id = e.Id,
                AppointmentHospital = e.AppointmentHospital,
                OrderStatus = e.OrderStatus,
                SendHospital = e.SendHospital,
                //ConsultContent = e.ConsultContent,
                IsToHospital = e.IsToHosiotal
            }).ToList();
            return ResultData<FxPageInfo<ContentPlateformOrderSimpleInfoVo>>.Success().AddData("data", fxPageInfo);
        }

        /// <summary>
        /// 根据派单时间获取供应链达人派单和上门数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getCooperationLiveAnchorSendAndVisitNum")]
        [FxInternalAuthorize]
        public async Task<ResultData<GetCooperationLiveAnchorSendAndVisitNumVo>> GetCooperationLiveAnchorSendAndVisitNumAsync([FromQuery] QueryCooperationLiveAnchorSendAndVisitNumVo query)
        {
            var baseLiveAnchorIds = await liveAnchorBaseInfoService.GetCooperateLiveAnchorAsync(null);
            var liveAnchorIds = await liveAnchorService.GetLiveAnchorListByBaseInfoIdListAsync(baseLiveAnchorIds.Select(e => e.Id).ToList());
            var orders = await _orderService.GetOrderSendAndDealDataByMonthAsync(query.SendStartDate, query.SendEndDate.AddDays(1).AddMilliseconds(-1), null, "", liveAnchorIds.Select(e => e.Id).ToList());
            GetCooperationLiveAnchorSendAndVisitNumVo result = new GetCooperationLiveAnchorSendAndVisitNumVo();
            result.SendOrderNum = orders.SendOrderNum;
            result.VisitNum = orders.VisitNum;
            QueryNewCustomerToHospiatlAndTargetCompleteDto queryNewCustomerToHospiatlAndTargetCompleteDto = new QueryNewCustomerToHospiatlAndTargetCompleteDto();
            queryNewCustomerToHospiatlAndTargetCompleteDto.StartDate = query.SendStartDate;
            queryNewCustomerToHospiatlAndTargetCompleteDto.EndDate = query.SendEndDate;
            queryNewCustomerToHospiatlAndTargetCompleteDto.EmpId = query.EmployeeId;
            var operationBoardInfo = await amiyaOperationsBoardService.GetNewCustomerToHospiatlAndTargetCompleteAsync(queryNewCustomerToHospiatlAndTargetCompleteDto);
            result.OldTakeNewDealNum = operationBoardInfo.OldTakeNewCustomerNum;
            return ResultData<GetCooperationLiveAnchorSendAndVisitNumVo>.Success().AddData("CooperationLiveAnchorSendAndVisitNum", result);
        }
        /// <summary>
        /// 获取仅有主派派单数据的订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getOnlyMainHospitalOrder")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<SendContentPlatformOrderVo>>> GetOnlyMainHospitalOrderAsync([FromQuery] QueryOnlyMainHospitalOrderByPageVo query)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int loginEmployeeId = Convert.ToInt32(employee.Id);
            QueryOnlyMainHospitalOrderByPageDto queryDto = new QueryOnlyMainHospitalOrderByPageDto();
            queryDto.PageNum = query.PageNum;
            queryDto.PageSize = query.PageSize;
            queryDto.StartDate = query.StartDate;
            queryDto.EndDate = query.EndDate;
            queryDto.employeeId = loginEmployeeId;
            queryDto.KeyWord = query.KeyWord;
            queryDto.OrderStatus = query.OrderStatus;
            queryDto.HospitalId = query.HospitalId;
            var res = await _orderService.GetOnlyMainHospitalOrderAsync(queryDto);
            FxPageInfo<SendContentPlatformOrderVo> pageInfo = new FxPageInfo<SendContentPlatformOrderVo>();
            pageInfo.TotalCount = res.TotalCount;
            pageInfo.List = res.List.Select(d => new SendContentPlatformOrderVo
            {
                Id = d.Id,
                OrderId = d.OrderId,
                ContentPlatFormName = d.ContentPlatFormName,
                LiveAnchorName = d.LiveAnchorName,
                CustomerName = ServiceClass.GetIncompleteCustomerName(d.CustomerName),
                Phone = d.Phone,
                EncryptPhone = d.EncryptPhone,
                SendHospitalId = d.SendHospitalId,
                GoodsName = d.GoodsName,
                ThumbPictureUrl = d.ThumbPictureUrl,
                ConsultingContent = d.ConsultingContent,
                OrderTypeText = d.OrderTypeText,
                OrderStatusText = d.OrderStatusText,
                IsToHospital = d.IsToHospital,
                ToHospitalTypeText = d.ToHospitalTypeText,
                SenderName = d.SenderName,
                SendDate = d.SendDate,
                SendOrderRemark = d.SendOrderRemark,
                OrderRemark = d.OrderRemark,
                HospitalRemark = d.HospitalRemark,
                OrderSourceText = d.OrderSourceText,
                IsRepeatProfundityOrder = d.IsRepeatProfundityOrder,
                IsMainHospital = d.IsMainHospital,
                ConsultingContent2 = d.ConsultingContent2,
                BelongEmpName = d.BelongEmpName,
                SendHospital = d.SendHospital,
                IsHospitalCheckPhone = d.IsHospitalCheckPhone,
                IsSpecifyHospitalEmployee = d.IsSpecifyHospitalEmployee,
                HospitalEmployeeId = d.HospitalEmployeeId,
                HospitalEmployeeName = d.HospitalEmployeeName,
                BelongChannelText = d.BelongChannelText
            }).ToList();
            return ResultData<FxPageInfo<SendContentPlatformOrderVo>>.Success().AddData("data", pageInfo);
        }
        /// <summary>
        /// 未辅派订单批量改派
        /// </summary>
        /// <param name="batchEditSend"></param>
        /// <returns></returns>
        [HttpPost("batchEditSendInfo")]
        public async Task<ResultData> BatchEditSendInfoAsync(BatchEditSendInfoVo batchEditSend)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            BatchEditSendInfoDto editDto = new BatchEditSendInfoDto();
            editDto.EmployeeId = employeeId;
            editDto.HospitalId = batchEditSend.HospitalId;
            editDto.SendInfoIdList = batchEditSend.SendInfoIdList;
            editDto.IsSpecifyHospitalEmployee = batchEditSend.IsSpecifyHospitalEmployee;
            editDto.HospitalEmployeeId = batchEditSend.HospitalEmployeeId;
            await _orderService.BatchEditSendInfoAsync(editDto);
            return ResultData.Success();
        }


        #region {医院对接同步}
        ///// <summary>
        ///// 医院对接同步完成订单(影响订单业务)
        ///// </summary>
        ///// <param name="updateVo"></param>
        ///// <returns></returns>
        //[HttpPut("finishContentPlateFormOrderByApi")]
        ////[FxPartnerAuthorize]
        //public async Task<ResultData> HospitalFinishOrderByApiAsync(FinishContentPlateFormOrderByApi updateVo)
        //{
        //    OperationAddDto operationAddDto = new OperationAddDto();
        //    operationAddDto.Code = 0;
        //    try
        //    {

        //        int hospitalId = 0;
        //        if (updateVo.HospitalId == "HuaShan")
        //        {
        //            hospitalId = 16;
        //        }
        //        else if (updateVo.HospitalId == "WeiDuoLiYa")
        //        {
        //            hospitalId = 37;
        //        }
        //        else if (updateVo.HospitalId == "test")
        //        {
        //            hospitalId = 4;
        //        }
        //        else
        //        {
        //            throw new Exception("医院编号错误！");
        //        }
        //        //验证是否已绑定客服
        //        var bindCustomerServiceName = await bindCustomerServiceService.GetBindCustomerServiceNameByPhone(updateVo.CustomerPhone);
        //        if (bindCustomerServiceName == "未绑定")
        //        {
        //            AddBindCustomerServiceFirstlyDto addBindCustomerServiceFirstlyDto = new AddBindCustomerServiceFirstlyDto();
        //            addBindCustomerServiceFirstlyDto.CustomerServiceId = 188;
        //            addBindCustomerServiceFirstlyDto.BuyerPhone = updateVo.CustomerPhone;
        //            addBindCustomerServiceFirstlyDto.CreateBy = 188;
        //            addBindCustomerServiceFirstlyDto.CreateDate = DateTime.Now;
        //            string projectDemand = "";
        //            if (updateVo.Details != null && updateVo.Details.Count > 0)
        //            {
        //                foreach (var x in updateVo.Details)
        //                {

        //                    projectDemand += x.ItemName + "，";
        //                }
        //                projectDemand = projectDemand.Substring(0, projectDemand.Length - 1);
        //            }
        //            addBindCustomerServiceFirstlyDto.FirstProjectDemand = projectDemand;
        //            addBindCustomerServiceFirstlyDto.FirstConsumptionDate = DateTime.Now;
        //            addBindCustomerServiceFirstlyDto.NewConsumptionDate = DateTime.Now;
        //            addBindCustomerServiceFirstlyDto.NewConsumptionContentPlatform = (int)OrderFrom.ContentPlatFormOrder;
        //            addBindCustomerServiceFirstlyDto.NewContentPlatForm = "医院";
        //            addBindCustomerServiceFirstlyDto.AllPrice = updateVo.TotalCashAmount;
        //            addBindCustomerServiceFirstlyDto.AllOrderCount = 1;
        //            addBindCustomerServiceFirstlyDto.NewLiveAnchor = "";
        //            addBindCustomerServiceFirstlyDto.NewWechatNo = "";
        //            await bindCustomerServiceService.OnlyAddFistlyAsync(addBindCustomerServiceFirstlyDto);
        //        }

        //        //验证订单是否存在
        //        var contentPlatFormOrder = await _orderService.GetOrderListByPhoneAndHospitalIdAsync(updateVo.CustomerPhone, hospitalId);
        //        //修改订单
        //        ContentPlateFormOrderFinishDto updateDto = new ContentPlateFormOrderFinishDto();
        //        if (contentPlatFormOrder != null)
        //        {
        //            updateDto.Id = contentPlatFormOrder.Id;
        //        }
        //        else
        //        {
        //            updateDto.LastProjectStage = updateVo.CustomerPhone;
        //        }
        //        updateDto.IsFinish = true;

        //        int consumptionType = (int)ConsumptionType.OTHER;
        //        if (Convert.ToInt32(updateVo.Type) == 0)
        //        {
        //            updateDto.ToHospitalType = (int)ContentPlateFormOrderToHospitalType.OTHER;
        //            if (updateVo.ConsumptionType == 1 || updateVo.ConsumptionType == 2 || updateVo.ConsumptionType == 3)
        //            {
        //                consumptionType = (int)ConsumptionType.Deal;
        //            }
        //            else if (updateVo.ConsumptionType == 0)
        //            {
        //                consumptionType = (int)ConsumptionType.Deposit;
        //            }
        //        }
        //        else
        //        {
        //            updateDto.ToHospitalType = (int)ContentPlateFormOrderToHospitalType.REFUND;
        //            updateVo.TotalCashAmount = -updateVo.TotalCashAmount;
        //            consumptionType = (int)ConsumptionType.Refund;
        //        }
        //        updateDto.LastDealHospitalId = hospitalId;
        //        updateDto.ToHospitalDate = updateVo.Date;
        //        updateDto.OtherContentPlatFormOrderId = updateVo.MsgId;
        //        updateDto.DealPictureUrl = "";
        //        updateDto.DealAmount = updateVo.TotalCashAmount;
        //        updateDto.UnDealReason = "";
        //        updateDto.IsToHospital = true;
        //        updateDto.UnDealPictureUrl = "";
        //        updateDto.DealDate = updateVo.Date;
        //        updateDto.IsAcompanying = false;
        //        updateDto.DealPerformanceType = (int)ContentPlateFormOrderDealPerformanceType.HospitalDeclarationInApi;
        //        updateDto.InvitationDocuments = new List<string>();
        //        updateDto.ConsumptionType = consumptionType;
        //        updateDto.EmpId = 0;
        //        List<AddContentPlatFormOrderDealDetailsDto> addContentPlatFormOrderDealDetailsDtos = new List<AddContentPlatFormOrderDealDetailsDto>();
        //        if (updateDto.IsFinish == true && updateVo.Details != null && updateVo.Details.Count > 0)
        //        {
        //            foreach (var x in updateVo.Details)
        //            {
        //                if (updateDto.ToHospitalType == (int)ContentPlateFormOrderToHospitalType.REFUND)
        //                {
        //                    x.CashAmount = -x.CashAmount;
        //                }
        //                AddContentPlatFormOrderDealDetailsDto addContentPlatFormOrderDealDetailsDto = new AddContentPlatFormOrderDealDetailsDto();
        //                addContentPlatFormOrderDealDetailsDto.GoodsName = x.ItemName;
        //                addContentPlatFormOrderDealDetailsDto.GoodsSpec = x.ItemStandard;
        //                addContentPlatFormOrderDealDetailsDto.Quantity = x.Quantity;
        //                addContentPlatFormOrderDealDetailsDto.Price = x.CashAmount;
        //                addContentPlatFormOrderDealDetailsDto.CreateBy = 0;
        //                addContentPlatFormOrderDealDetailsDto.ContentPlatFormOrderId = updateDto.Id;
        //                addContentPlatFormOrderDealDetailsDtos.Add(addContentPlatFormOrderDealDetailsDto);
        //            }
        //        }
        //        updateDto.CustomerName = updateVo.CustomerName;
        //        updateDto.AddContentPlatFormOrderDealDetailsDtoList = addContentPlatFormOrderDealDetailsDtos;
        //        await _orderService.FinishContentPlateFormOrderAsync(updateDto);
        //        return ResultData.Success();
        //    }
        //    catch (Exception err)
        //    {
        //        operationAddDto.Code = -1;
        //        operationAddDto.Message = err.Message.ToString();
        //        throw new Exception(err.Message.ToString());
        //    }
        //    finally
        //    {

        //        operationAddDto.Parameters = JsonConvert.SerializeObject(updateVo);
        //        operationAddDto.RequestType = (int)RequestType.Update;
        //        operationAddDto.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
        //        await operationLogService.AddOperationLogAsync(operationAddDto);
        //    }
        //}


        /// <summary>
        /// 医院对接同步（不影响订单业务）
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("finishContentPlateFormOrderByApi")]
        //[FxPartnerAuthorize]
        public async Task<ResultData> HospitalFinishOrderByApiAsync(FinishContentPlateFormOrderByApi updateVo)
        {
            OperationAddDto operationAddDto = new OperationAddDto();
            operationAddDto.Code = 0;
            try
            {

                int hospitalId = 0;
                if (updateVo.HospitalId == "HuaShan")
                {
                    hospitalId = 16;
                }
                else if (updateVo.HospitalId == "WeiDuoLiYa")
                {
                    hospitalId = 37;
                }
                else if (updateVo.HospitalId == "test")
                {
                    hospitalId = 124;
                }
                else
                {
                    throw new Exception("医院编号错误！");
                }
                //验证是否已绑定客服
                var bindCustomerServiceName = await bindCustomerServiceService.GetBindCustomerServiceNameByPhone(updateVo.CustomerPhone);
                if (bindCustomerServiceName == "未绑定")
                {
                    AddBindCustomerServiceFirstlyDto addBindCustomerServiceFirstlyDto = new AddBindCustomerServiceFirstlyDto();
                    addBindCustomerServiceFirstlyDto.CustomerServiceId = 188;
                    addBindCustomerServiceFirstlyDto.BuyerPhone = updateVo.CustomerPhone;
                    addBindCustomerServiceFirstlyDto.CreateBy = 188;
                    addBindCustomerServiceFirstlyDto.CreateDate = DateTime.Now;
                    string projectDemand = "";
                    if (updateVo.Details != null && updateVo.Details.Count > 0)
                    {
                        foreach (var x in updateVo.Details)
                        {

                            projectDemand += x.ItemName + "，";
                        }
                        projectDemand = projectDemand.Substring(0, projectDemand.Length - 1);
                    }
                    addBindCustomerServiceFirstlyDto.FirstProjectDemand = projectDemand;
                    addBindCustomerServiceFirstlyDto.FirstConsumptionDate = DateTime.Now;
                    addBindCustomerServiceFirstlyDto.NewConsumptionDate = DateTime.Now;
                    addBindCustomerServiceFirstlyDto.NewConsumptionContentPlatform = (int)OrderFrom.ContentPlatFormOrder;
                    addBindCustomerServiceFirstlyDto.NewContentPlatForm = "医院";
                    addBindCustomerServiceFirstlyDto.AllPrice = updateVo.TotalCashAmount;
                    addBindCustomerServiceFirstlyDto.AllOrderCount = 1;
                    addBindCustomerServiceFirstlyDto.NewLiveAnchor = "";
                    addBindCustomerServiceFirstlyDto.NewWechatNo = "";
                    await bindCustomerServiceService.OnlyAddFistlyAsync(addBindCustomerServiceFirstlyDto);
                }
                AddCustomerHospitalDealInfoDto addCustomerHospitalDealInfoDto = new AddCustomerHospitalDealInfoDto();
                addCustomerHospitalDealInfoDto.HospitalId = hospitalId;
                addCustomerHospitalDealInfoDto.Type = updateVo.Type;
                addCustomerHospitalDealInfoDto.CustomerName = updateVo.CustomerName;
                addCustomerHospitalDealInfoDto.CustomerPhone = updateVo.CustomerPhone;
                addCustomerHospitalDealInfoDto.Date = updateVo.Date;
                addCustomerHospitalDealInfoDto.TotalCashAmount = updateVo.TotalCashAmount;
                addCustomerHospitalDealInfoDto.ConsumptionType = updateVo.ConsumptionType;
                addCustomerHospitalDealInfoDto.RefundType = updateVo.RefundType;
                addCustomerHospitalDealInfoDto.MsgId = updateVo.MsgId;
                List<AddCustomerHospitalDealDetailsDto> addCustomerHospitalDealDetailsDtos = new List<AddCustomerHospitalDealDetailsDto>();

                if (updateVo.ConsumptionType == 2 || updateVo.ConsumptionType == 3 || updateVo.RefundType == 2 || updateVo.RefundType == 3)
                {
                    foreach (var x in updateVo.Details)
                    {
                        AddCustomerHospitalDealDetailsDto addContentPlatFormOrderDealDetailsDto = new AddCustomerHospitalDealDetailsDto();
                        addContentPlatFormOrderDealDetailsDto.ItemName = x.ItemName;
                        addContentPlatFormOrderDealDetailsDto.ItemStandard = x.ItemStandard;
                        addContentPlatFormOrderDealDetailsDto.Quantity = x.Quantity;
                        addContentPlatFormOrderDealDetailsDto.CashAmount = x.CashAmount;
                        addCustomerHospitalDealDetailsDtos.Add(addContentPlatFormOrderDealDetailsDto);
                    }
                }
                addCustomerHospitalDealInfoDto.addCustomerHospitalDealDetailsDtos = addCustomerHospitalDealDetailsDtos;
                await customerHospitalDealInfoService.AddAsync(addCustomerHospitalDealInfoDto);
                return ResultData.Success();
            }
            catch (Exception err)
            {
                operationAddDto.Code = -1;
                operationAddDto.Message = err.Message.ToString();
                throw new Exception(err.Message.ToString());
            }
            finally
            {

                operationAddDto.Parameters = JsonConvert.SerializeObject(updateVo);
                operationAddDto.RequestType = (int)RequestType.Update;
                operationAddDto.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationAddDto);
            }
        }
        #endregion


        #region 枚举下拉框

        /// <summary>
        /// 获取内容平台面诊类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("getOrderConsultationTypeList")]
        [FxInternalAuthorize]
        public ResultData<List<ContentPlateFormOrderTypeVo>> GetOrderConsultationTypeList()
        {
            var orderTypes = from d in _orderService.GetOrderConsultationTypeList()
                             select new ContentPlateFormOrderTypeVo
                             {
                                 OrderType = d.OrderType,
                                 OrderTypeText = d.OrderTypeText
                             };
            return ResultData<List<ContentPlateFormOrderTypeVo>>.Success().AddData("orderConsultationTypes", orderTypes.ToList());
        }

        /// <summary>
        /// 获取内容平台订单类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("contentPlateFormOrderTypeList")]
        [FxInternalAuthorize]
        public ResultData<List<ContentPlateFormOrderTypeVo>> GetContentPlateFormOrderTypeList()
        {
            var orderTypes = from d in _orderService.GetOrderTypeList()
                             select new ContentPlateFormOrderTypeVo
                             {
                                 OrderType = d.OrderType,
                                 OrderTypeText = d.OrderTypeText
                             };
            return ResultData<List<ContentPlateFormOrderTypeVo>>.Success().AddData("orderTypes", orderTypes.ToList());
        }

        /// <summary>
        /// 获取内容平台订单状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("contentPlateFormOrderStatusList")]
        [FxInternalAuthorize]
        public ResultData<List<ContentPlateFormOrderStatusVo>> GetContentPlateFormOrderStatusList()
        {
            var orderStatus = from d in _orderService.GetOrderStatusList()
                              select new ContentPlateFormOrderStatusVo
                              {
                                  OrderStatus = d.OrderStatus,
                                  OrderStatusText = d.OrderStatusText
                              };
            return ResultData<List<ContentPlateFormOrderStatusVo>>.Success().AddData("orderStatus", orderStatus.ToList());
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
        /// 获取内容平台订单来源
        /// </summary>
        /// <returns></returns>
        [FxInternalAuthorize]
        [HttpGet("contentPlateFormOrderSourceList")]
        public ResultData<List<ContentPlateFormOrderSourceVo>> GetContentPlateFormOrderSourceList()
        {
            var orderSources = from d in _orderService.GetOrderSourceList()
                               select new ContentPlateFormOrderSourceVo
                               {
                                   OrderSource = d.OrderSource,
                                   OrderSourceText = d.OrderSourceText
                               };
            return ResultData<List<ContentPlateFormOrderSourceVo>>.Success().AddData("orderSources", orderSources.ToList());
        }

        /// <summary>
        /// 获取内容平台订单到院状态
        /// </summary>
        /// <returns></returns>
        [FxInternalOrTenantAuthroize]
        [HttpGet("contentPlateFormOrderToHospitalTypeList")]
        public ResultData<List<ContentPlateFormOrderTypeVo>> GetContentPlateFormOrderToHospitalTypeList()
        {
            var orderTypes = from d in _orderService.GetOrderToHospitalTypeList()
                             select new ContentPlateFormOrderTypeVo
                             {
                                 OrderType = d.OrderType,
                                 OrderTypeText = d.OrderTypeText
                             };
            return ResultData<List<ContentPlateFormOrderTypeVo>>.Success().AddData("orderTypes", orderTypes.ToList());
        }
        #endregion
    }
}
