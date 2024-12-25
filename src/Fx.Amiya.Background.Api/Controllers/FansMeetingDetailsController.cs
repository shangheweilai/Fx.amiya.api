using System;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Vo.FansMeetingDetails.Input;
using Fx.Amiya.Background.Api.Vo.FansMeetingDetails.Result;
using Fx.Amiya.Dto.FansMeetingDetails.Input;
using Fx.Amiya.Dto.OperationLog;
using Fx.Amiya.IService;
using Fx.Amiya.Service;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using Jd.Api.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 粉丝见面会详情
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class FansMeetingDetailsController : ControllerBase
    {
        private IFansMeetingDetailsService fansMeetingDetailsService;
        private readonly IContentPlateFormOrderService contentPlateFormOrderService;
        private readonly ICustomerService customerService;
        private readonly IWxAppConfigService _wxAppConfigService;
        private IHttpContextAccessor _httpContextAccessor;
        private IPermissionService permissionService;
        private IOperationLogService operationLogService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fansMeetingDetailsService"></param>
        /// <param name="contentPlateFormOrderService"></param>
        /// <param name="customerService"></param>
        /// <param name="wxAppConfigService"></param>
        public FansMeetingDetailsController(IFansMeetingDetailsService fansMeetingDetailsService, IContentPlateFormOrderService contentPlateFormOrderService, ICustomerService customerService, IHttpContextAccessor httpContextAccessor, IPermissionService permissionService, IWxAppConfigService wxAppConfigService, IOperationLogService operationLogService)
        {
            this.fansMeetingDetailsService = fansMeetingDetailsService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.customerService = customerService;
            this._wxAppConfigService = wxAppConfigService;
            this._httpContextAccessor = httpContextAccessor;
            this.permissionService = permissionService;
            this.operationLogService = operationLogService;
        }



        /// <summary>
        /// 医院端根据条件获取粉丝见面会详情信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("listWithPageByHospital")]
        [FxTenantAuthorize]
        public async Task<ResultData<FxPageInfo<FansMeetingDetailsVo>>> GetListWithPageByHospitalAsync([FromQuery] QueryFansMeetingDetailsVo query)
        {
            try
            {
                QueryFansMeetingDetailsDto queryDto = new QueryFansMeetingDetailsDto();
                queryDto.StartDate = query.StartDate;
                queryDto.EndDate = query.EndDate;
                queryDto.PageNum = query.PageNum;
                queryDto.KeyWord = query.KeyWord;
                queryDto.PageSize = query.PageSize;

                queryDto.IsHidePhone = false;
                queryDto.FansMeetingId = query.FansMeetingId;
                var q = await fansMeetingDetailsService.GetListAsync(queryDto);
                var fansMeetingDetails = from d in q.List
                                         select new FansMeetingDetailsVo
                                         {
                                             Id = d.Id,
                                             CreateDate = d.CreateDate,
                                             AmiyaConsulationId = d.AmiyaConsulationId,
                                             AmiyaConsulationName = d.AmiyaConsulationName,
                                             UpdateDate = d.UpdateDate,
                                             Valid = d.Valid,
                                             DeleteDate = d.DeleteDate,
                                             FansMeetingId = d.FansMeetingId,
                                             FansMeetingName = d.FansMeetingName,
                                             OrderId = d.OrderId,
                                             AppointmentDate = d.AppointmentDate,
                                             AppointmentDetailsDate = d.AppointmentDetailsDate,
                                             CustomerName = d.CustomerName,
                                             Phone = d.Phone,
                                             CustomerQuantity = d.CustomerQuantity,
                                             CustomerQuantityText = d.CustomerQuantityText,
                                             IsOldCustomer = d.IsOldCustomer,
                                             HospitalConsulationName = d.HospitalConsulationName,
                                             City = d.City,
                                             TravelInformation = d.TravelInformation,
                                             IsNeedDriver = d.IsNeedDriver,
                                             HotelPlan = d.HotelPlan,
                                             PlanConsumption = d.PlanConsumption,
                                             Remark = d.Remark,
                                             CustomerPictureUrl = d.CustomerPictureUrl,
                                             IsDeal = d.IsDeal,
                                             IsToHospital = d.IsToHospital,
                                             CumulativeDealPrice = d.CumulativeDealPrice,
                                             UnDealReason = d.UnDealReason,
                                             FansMeetingProject = d.FansMeetingProject,
                                             FollowUpContent = d.FollowUpContent,
                                             NextAppointmentDate = d.NextAppointmentDate,
                                             IsNeedHospitalHelp = d.IsNeedHospitalHelp,
                                             IsNeedCustomerServiceHelp = d.IsNeedCustomerServiceHelp,
                                             HospitalMemberCardId = d.HospitalMemberCardId,
                                         };

                FxPageInfo<FansMeetingDetailsVo> pageInfo = new FxPageInfo<FansMeetingDetailsVo>();
                pageInfo.TotalCount = q.TotalCount;
                pageInfo.List = fansMeetingDetails;

                return ResultData<FxPageInfo<FansMeetingDetailsVo>>.Success().AddData("fansMeetingDetails", pageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<FansMeetingDetailsVo>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 医院端根据条件导出粉丝见面会详情信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ExportByHospital")]
        [FxTenantAuthorize]
        public async Task<FileStreamResult> ExportByHospitalAsync([FromQuery] QueryFansMeetingDetailsVo query)
        {
            try
            {
                QueryFansMeetingDetailsDto queryDto = new QueryFansMeetingDetailsDto();
                queryDto.StartDate = query.StartDate;
                queryDto.EndDate = query.EndDate;
                queryDto.PageNum = query.PageNum;
                queryDto.KeyWord = query.KeyWord;
                queryDto.PageSize = query.PageSize;

                queryDto.IsHidePhone = true;
                queryDto.FansMeetingId = query.FansMeetingId;
                var q = await fansMeetingDetailsService.GetExportListAsync(queryDto);
                var fansMeetingDetails = from d in q
                                         select new ExportFansMeetingDetailsVo
                                         {
                                             AmiyaConsulationName = d.AmiyaConsulationName,
                                             IsToHospital = d.IsToHospital == true ? "是" : "否",
                                             IsDeal = d.IsDeal == true ? "是" : "否",
                                             AppointmentDate = d.AppointmentDate,
                                             AppointmentDetailsDate = d.AppointmentDetailsDate,
                                             CustomerName = d.CustomerName,
                                             Phone = d.Phone,
                                             CustomerQuantity = d.CustomerQuantityText,
                                             IsOldCustomer = d.IsOldCustomer == true ? "是" : "否",
                                             HospitalConsulationName = d.HospitalConsulationName,
                                             City = d.City,
                                             TravelInformation = d.TravelInformation,
                                             IsNeedDriver = d.IsNeedDriver == true ? "是" : "否",
                                             HotelPlan = d.HotelPlan,
                                             PlanConsumption = d.PlanConsumption,
                                             Remark = d.Remark,
                                             CustomerPictureUrl = d.CustomerPictureUrl,
                                             FansMeetingProject = d.FansMeetingProject,
                                             FollowUpContent = d.FollowUpContent,
                                             NextAppointmentDate = d.NextAppointmentDate,
                                             IsNeedHospitalHelp = d.IsNeedHospitalHelp == true ? "是" : "否",
                                             IsNeedCustomerServiceHelp = d.IsNeedCustomerServiceHelp == true ? "是" : "否",
                                             HospitalMemberCardId = d.HospitalMemberCardId,

                                         };

                FxPageInfo<FansMeetingDetailsVo> pageInfo = new FxPageInfo<FansMeetingDetailsVo>();
                var exportSendOrder = fansMeetingDetails.ToList();
                var stream = ExportExcelHelper.ExportExcel(exportSendOrder);
                var result = File(stream, "application/vnd.ms-excel", $"粉丝见面会详情信息.xls");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }



        /// <summary>
        /// 管理端根据条件获取粉丝见面会详情信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<FxPageInfo<FansMeetingDetailsVo>>> GetListWithPageAsync([FromQuery] QueryFansMeetingDetailsVo query)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int empIdRole = Convert.ToInt32(employee.PositionId);
                var result = await permissionService.getPermissionsById(empIdRole);
                bool isHidePhone = true;
                var findPermissionId = result.Find(x => x == 19);
                if (findPermissionId > 0)
                {
                    isHidePhone = false;
                }
                QueryFansMeetingDetailsDto queryDto = new QueryFansMeetingDetailsDto();
                queryDto.StartDate = query.StartDate;
                queryDto.EndDate = query.EndDate;
                queryDto.PageNum = query.PageNum;
                queryDto.KeyWord = query.KeyWord;
                queryDto.PageSize = query.PageSize;


                queryDto.FansMeetingId = query.FansMeetingId;
                queryDto.IsDeal = query.IsDeal;
                queryDto.IsToHospital = query.IsToHospital;
                queryDto.AmiyaEmployeeId = query.AmiyaEmployeeId;
                queryDto.CustomerQuantity = query.CustomerQuantity;
                queryDto.IsOdCustomer = query.IsOdCustomer;
                queryDto.StartDealPrice = query.StartDealPrice;
                queryDto.EndDealPrice = query.EndDealPrice;
                queryDto.IsHidePhone = isHidePhone;
                var q = await fansMeetingDetailsService.GetListAsync(queryDto);
                var fansMeetingDetails = from d in q.List
                                         select new FansMeetingDetailsVo
                                         {
                                             Id = d.Id,
                                             CreateDate = d.CreateDate,
                                             AmiyaConsulationId = d.AmiyaConsulationId,
                                             AmiyaConsulationName = d.AmiyaConsulationName,
                                             UpdateDate = d.UpdateDate,
                                             Valid = d.Valid,
                                             DeleteDate = d.DeleteDate,
                                             FansMeetingId = d.FansMeetingId,
                                             FansMeetingName = d.FansMeetingName,
                                             OrderId = d.OrderId,
                                             AppointmentDate = d.AppointmentDate,
                                             AppointmentDetailsDate = d.AppointmentDetailsDate,
                                             CustomerName = d.CustomerName,
                                             Phone = d.Phone,
                                             CustomerQuantity = d.CustomerQuantity,
                                             CustomerQuantityText = d.CustomerQuantityText,
                                             IsOldCustomer = d.IsOldCustomer,
                                             HospitalConsulationName = d.HospitalConsulationName,
                                             City = d.City,
                                             TravelInformation = d.TravelInformation,
                                             IsNeedDriver = d.IsNeedDriver,
                                             HotelPlan = d.HotelPlan,
                                             PlanConsumption = d.PlanConsumption,
                                             Remark = d.Remark,
                                             CustomerPictureUrl = d.CustomerPictureUrl,
                                             IsDeal = d.IsDeal,
                                             IsToHospital = d.IsToHospital,
                                             CumulativeDealPrice = d.CumulativeDealPrice,
                                             UnDealReason = d.UnDealReason,
                                             FansMeetingProject = d.FansMeetingProject,
                                             FollowUpContent = d.FollowUpContent,
                                             NextAppointmentDate = d.NextAppointmentDate,
                                             IsNeedHospitalHelp = d.IsNeedHospitalHelp,
                                             IsNeedCustomerServiceHelp = d.IsNeedCustomerServiceHelp,
                                             HospitalMemberCardId = d.HospitalMemberCardId,
                                         };

                FxPageInfo<FansMeetingDetailsVo> pageInfo = new FxPageInfo<FansMeetingDetailsVo>();
                pageInfo.TotalCount = q.TotalCount;
                pageInfo.List = fansMeetingDetails;

                return ResultData<FxPageInfo<FansMeetingDetailsVo>>.Success().AddData("fansMeetingDetails", pageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<FansMeetingDetailsVo>>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 添加粉丝见面会详情
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData> AddAsync(AddFansMeetingDetailsVo addVo)
        {

            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Source = (int)RequestSource.AmiyaBackground;
            operationLog.Code = 0;
            int? empId = employee == null ? null : Convert.ToInt32(employee.Id);
            operationLog.OperationBy = empId;
            try
            {
                AddFansMeetingDetailsDto addDto = new AddFansMeetingDetailsDto();
                addDto.FansMeetingId = addVo.FansMeetingId;
                if (addVo.OrderIdList.Count > 0)
                {
                    foreach (var z in addVo.OrderIdList)
                    {
                        addDto.OrderId = z;
                        var orderInfo = await contentPlateFormOrderService.GetByOrderIdAsync(z);
                        addDto.CustomerName = orderInfo.CustomerName;
                        addDto.Phone = orderInfo.Phone;
                        var quantity = 0;
                        if (orderInfo.OrderStatus == (int)ContentPlateFormOrderStatus.OrderComplete)
                        {
                            quantity = 1;
                        }
                        addDto.CustomerQuantity = quantity;
                        addDto.IsOldCustomer = orderInfo.IsOldCustomer;
                        if (orderInfo.IsSupportOrder == true)
                        {
                            addDto.AmiyaConsulationId = orderInfo.SupportEmpId;
                        }
                        else
                        {
                            addDto.AmiyaConsulationId = orderInfo.BelongEmpId.Value;
                        }

                        var config = await _wxAppConfigService.GetWxAppCallCenterConfigAsync();
                        string encryptPhone = ServiceClass.Encrypt(orderInfo.Phone, config.PhoneEncryptKey);
                        var customerBaseInfo = await customerService.GetCustomerBaseInfoByEncryptPhoneAsync(encryptPhone);
                        addDto.City = customerBaseInfo.City;
                        addDto.Remark = orderInfo.ConsultingContent;
                        if (orderInfo.CustomerPictures.Count > 0)
                        {

                            addDto.CustomerPictureUrl = orderInfo.CustomerPictures.First();
                        }

                        addDto.AppointmentDetailsDate = addVo.AppointmentDetailsDate;
                        addDto.AppointmentDate = addVo.AppointmentDate;
                        if (addDto.AppointmentDate.HasValue == false)
                        {
                            addDto.AppointmentDetailsDate = "待定";
                        }
                        addDto.HospitalConsulationName = addVo.HospitalConsulationName;
                        addDto.TravelInformation = addVo.TravelInformation;
                        addDto.IsNeedDriver = addVo.IsNeedDriver;
                        addDto.HotelPlan = addVo.HotelPlan;
                        addDto.PlanConsumption = addVo.PlanConsumption;
                        await fansMeetingDetailsService.AddAsync(addDto);
                    }
                }
                else
                {
                    addDto.CustomerName = addVo.CustomerName;
                    addDto.Phone = addVo.Phone;
                    addDto.CustomerQuantity = addVo.CustomerQuantity;
                    addDto.IsOldCustomer = addVo.IsOldCustomer;
                    addDto.AmiyaConsulationId = addVo.AmiyaConsulationId;
                    addDto.City = addVo.City;
                    addDto.Remark = addVo.Remark;
                    addDto.CustomerPictureUrl = addVo.CustomerPictureUrl;


                    addDto.AppointmentDetailsDate = addVo.AppointmentDetailsDate;
                    addDto.AppointmentDate = addVo.AppointmentDate;
                    if (addDto.AppointmentDate.HasValue == false)
                    {
                        addDto.AppointmentDetailsDate = "待定";
                    }
                    addDto.HospitalConsulationName = addVo.HospitalConsulationName;
                    addDto.TravelInformation = addVo.TravelInformation;
                    addDto.IsNeedDriver = addVo.IsNeedDriver;
                    addDto.HotelPlan = addVo.HotelPlan;
                    addDto.PlanConsumption = addVo.PlanConsumption;
                    await fansMeetingDetailsService.AddAsync(addDto);
                }




                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                return ResultData.Fail(ex.Message);
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
        /// 根据粉丝见面会详情编号获取粉丝见面会详情信息
        /// </summary>
        /// <param name="id">粉丝见面会详情编号</param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<FansMeetingDetailsVo>> GetByIdAsync(string id)
        {
            try
            {
                var fansMeetingDetails = await fansMeetingDetailsService.GetByIdAsync(id);
                FansMeetingDetailsVo fansMeetingDetailsVo = new FansMeetingDetailsVo();
                fansMeetingDetailsVo.Id = fansMeetingDetails.Id;
                fansMeetingDetailsVo.CreateDate = fansMeetingDetails.CreateDate;
                fansMeetingDetailsVo.AmiyaConsulationId = fansMeetingDetails.AmiyaConsulationId;
                fansMeetingDetailsVo.AmiyaConsulationName = fansMeetingDetails.AmiyaConsulationName;
                fansMeetingDetailsVo.Valid = fansMeetingDetails.Valid;
                fansMeetingDetailsVo.FansMeetingId = fansMeetingDetails.FansMeetingId;
                fansMeetingDetailsVo.OrderId = fansMeetingDetails.OrderId;
                fansMeetingDetailsVo.AppointmentDate = fansMeetingDetails.AppointmentDate;
                fansMeetingDetailsVo.AppointmentDetailsDate = fansMeetingDetails.AppointmentDetailsDate;
                fansMeetingDetailsVo.CustomerName = fansMeetingDetails.CustomerName;
                fansMeetingDetailsVo.Phone = fansMeetingDetails.Phone;
                fansMeetingDetailsVo.CustomerQuantity = fansMeetingDetails.CustomerQuantity;
                fansMeetingDetailsVo.IsOldCustomer = fansMeetingDetails.IsOldCustomer;
                fansMeetingDetailsVo.HospitalConsulationName = fansMeetingDetails.HospitalConsulationName;
                fansMeetingDetailsVo.City = fansMeetingDetails.City;
                fansMeetingDetailsVo.TravelInformation = fansMeetingDetails.TravelInformation;
                fansMeetingDetailsVo.IsNeedDriver = fansMeetingDetails.IsNeedDriver;
                fansMeetingDetailsVo.HotelPlan = fansMeetingDetails.HotelPlan;
                fansMeetingDetailsVo.PlanConsumption = fansMeetingDetails.PlanConsumption;
                fansMeetingDetailsVo.Remark = fansMeetingDetails.Remark;
                fansMeetingDetailsVo.CustomerPictureUrl = fansMeetingDetails.CustomerPictureUrl;
                fansMeetingDetailsVo.IsDeal = fansMeetingDetails.IsDeal;
                fansMeetingDetailsVo.IsToHospital = fansMeetingDetails.IsToHospital;
                fansMeetingDetailsVo.CumulativeDealPrice = fansMeetingDetails.CumulativeDealPrice;

                fansMeetingDetailsVo.UnDealReason = fansMeetingDetails.UnDealReason;
                fansMeetingDetailsVo.FansMeetingProject = fansMeetingDetails.FansMeetingProject;
                fansMeetingDetailsVo.FollowUpContent = fansMeetingDetails.FollowUpContent;
                fansMeetingDetailsVo.NextAppointmentDate = fansMeetingDetails.NextAppointmentDate;
                fansMeetingDetailsVo.IsNeedHospitalHelp = fansMeetingDetails.IsNeedHospitalHelp;
                fansMeetingDetailsVo.IsNeedCustomerServiceHelp = fansMeetingDetails.IsNeedCustomerServiceHelp;
                fansMeetingDetailsVo.HospitalMemberCardId = fansMeetingDetails.HospitalMemberCardId;
                return ResultData<FansMeetingDetailsVo>.Success().AddData("fansMeetingDetails", fansMeetingDetailsVo);
            }
            catch (Exception ex)
            {
                return ResultData<FansMeetingDetailsVo>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 修改粉丝见面会详情信息
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData> UpdateAsync(UpdateFansMeetingDetailsVo updateVo)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Source = (int)RequestSource.AmiyaBackground;
            operationLog.Code = 0;
            int? empId = employee == null ? null : Convert.ToInt32(employee.Id);
            operationLog.OperationBy = empId;
            try
            {
                UpdateFansMeetingDetailsDto updateDto = new UpdateFansMeetingDetailsDto();
                if (!empId.HasValue)
                {
                    updateDto.IsHospitalUpdate = true;
                }
                updateDto.Id = updateVo.Id;
                updateDto.FansMeetingId = updateVo.FansMeetingId;
                updateDto.OrderId = updateVo.OrderId;
                updateDto.AppointmentDetailsDate = updateVo.AppointmentDetailsDate;
                updateDto.AppointmentDate = updateVo.AppointmentDate;
                if (updateDto.AppointmentDate.HasValue == false)
                {
                    updateDto.AppointmentDetailsDate = "待定";
                }
                updateDto.CustomerName = updateVo.CustomerName;
                updateDto.Phone = updateVo.Phone;
                updateDto.CustomerQuantity = updateVo.CustomerQuantity;
                updateDto.IsOldCustomer = updateVo.IsOldCustomer;
                updateDto.AmiyaConsulationId = updateVo.AmiyaConsulationId;
                updateDto.HospitalConsulationName = updateVo.HospitalConsulationName;
                updateDto.City = updateVo.City;
                updateDto.TravelInformation = updateVo.TravelInformation;
                updateDto.IsNeedDriver = updateVo.IsNeedDriver;
                updateDto.HotelPlan = updateVo.HotelPlan;
                updateDto.PlanConsumption = updateVo.PlanConsumption;
                updateDto.Remark = updateVo.Remark;
                updateDto.CustomerPictureUrl = updateVo.CustomerPictureUrl;
                updateDto.IsDeal = updateVo.IsDeal;
                updateDto.IsToHospital = updateVo.IsToHospital;
                updateDto.CumulativeDealPrice = updateVo.CumulativeDealPrice;

                updateDto.UnDealReason = updateVo.UnDealReason;
                updateDto.FansMeetingProject = updateVo.FansMeetingProject;
                updateDto.FollowUpContent = updateVo.FollowUpContent;
                updateDto.NextAppointmentDate = updateVo.NextAppointmentDate;
                updateDto.IsNeedHospitalHelp = updateVo.IsNeedHospitalHelp;
                updateDto.IsNeedCustomerServiceHelp = updateVo.IsNeedCustomerServiceHelp;
                updateDto.HospitalMemberCardId = updateVo.HospitalMemberCardId;
                await fansMeetingDetailsService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                return ResultData.Fail(ex.Message);
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
        /// 作废粉丝见面会详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData> DeleteAsync(string id)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int empId = Convert.ToInt32(employee.Id);
                await fansMeetingDetailsService.DeleteAsync(id, empId);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 是否参加粉丝见面会
        /// </summary>
        /// <returns></returns>
        [HttpGet("isAttend")]
        public async Task<ResultData<bool>> isAttendMeeting([FromQuery] AttendMeetingQueryVo query)
        {
            AttendMeetingQueryDto queryDto = new AttendMeetingQueryDto();
            queryDto.Id = query.Id;
            queryDto.Phone = query.Phone;
            queryDto.HospitalId = query.HospitalId;
            var res = await fansMeetingDetailsService.IsAttendMeeting(queryDto);
            return ResultData<bool>.Success().AddData("isAttend", res);
        }

    }
}