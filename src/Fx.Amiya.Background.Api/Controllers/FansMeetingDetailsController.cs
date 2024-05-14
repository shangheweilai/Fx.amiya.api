﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Vo.FansMeetingDetails.Input;
using Fx.Amiya.Background.Api.Vo.FansMeetingDetails.Result;
using Fx.Amiya.Dto.FansMeetingDetails.Input;
using Fx.Amiya.IService;
using Fx.Amiya.Service;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fansMeetingDetailsService"></param>
        /// <param name="contentPlateFormOrderService"></param>
        /// <param name="customerService"></param>
        /// <param name="wxAppConfigService"></param>
        public FansMeetingDetailsController(IFansMeetingDetailsService fansMeetingDetailsService, IContentPlateFormOrderService contentPlateFormOrderService, ICustomerService customerService, IHttpContextAccessor httpContextAccessor, IWxAppConfigService wxAppConfigService)
        {
            this.fansMeetingDetailsService = fansMeetingDetailsService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.customerService = customerService;
            this._wxAppConfigService = wxAppConfigService;
            this._httpContextAccessor = httpContextAccessor;
        }



        /// <summary>
        /// 根据条件获取粉丝见面会详情信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<FxPageInfo<FansMeetingDetailsVo>>> GetListWithPageAsync([FromQuery] QueryFansMeetingDetailsVo query)
        {
            try
            {
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
                                             IsDeal=d.IsDeal,
                                             IsToHospital=d.IsToHospital,
                                             CumulativeDealPrice=d.CumulativeDealPrice
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
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 根据粉丝见面会详情编号获取粉丝见面会详情信息
        /// </summary>
        /// <param name="id">粉丝见面会详情编号</param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalAuthorize]
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
                fansMeetingDetailsVo.AmiyaConsulationId = fansMeetingDetails.AmiyaConsulationId;
                fansMeetingDetailsVo.HospitalConsulationName = fansMeetingDetails.HospitalConsulationName;
                fansMeetingDetailsVo.City = fansMeetingDetails.City;
                fansMeetingDetailsVo.TravelInformation = fansMeetingDetails.TravelInformation;
                fansMeetingDetailsVo.IsNeedDriver = fansMeetingDetails.IsNeedDriver;
                fansMeetingDetailsVo.HotelPlan = fansMeetingDetails.HotelPlan;
                fansMeetingDetailsVo.PlanConsumption = fansMeetingDetails.PlanConsumption;
                fansMeetingDetailsVo.Remark = fansMeetingDetails.Remark;
                fansMeetingDetailsVo.CustomerPictureUrl = fansMeetingDetails.CustomerPictureUrl;
                fansMeetingDetailsVo.IsDeal=fansMeetingDetails.IsDeal;
                fansMeetingDetailsVo.IsToHospital = fansMeetingDetails.IsToHospital;
                fansMeetingDetailsVo.CumulativeDealPrice = fansMeetingDetails.CumulativeDealPrice;
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
        [FxInternalAuthorize]
        public async Task<ResultData> UpdateAsync(UpdateFansMeetingDetailsVo updateVo)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int empId = Convert.ToInt32(employee.Id);
                UpdateFansMeetingDetailsDto updateDto = new UpdateFansMeetingDetailsDto();
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
                updateDto.AmiyaConsulationId = empId;
                updateDto.HospitalConsulationName = updateVo.HospitalConsulationName;
                updateDto.City = updateVo.City;
                updateDto.TravelInformation = updateVo.TravelInformation;
                updateDto.IsNeedDriver = updateVo.IsNeedDriver;
                updateDto.HotelPlan = updateVo.HotelPlan;
                updateDto.PlanConsumption = updateVo.PlanConsumption;
                updateDto.Remark = updateVo.Remark;
                updateDto.CustomerPictureUrl = updateVo.CustomerPictureUrl;
                updateDto.IsDeal=updateVo.IsDeal;
                updateDto.IsToHospital = updateVo.IsToHospital;
                updateDto.CumulativeDealPrice=updateVo.CumulativeDealPrice;
                await fansMeetingDetailsService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
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
        public async Task<ResultData<bool>> isAttendMeeting([FromQuery]AttendMeetingQueryVo query)
        {
            AttendMeetingQueryDto queryDto = new AttendMeetingQueryDto();
            queryDto.Id = query.Id;
            queryDto.Phone = query.Phone;
            queryDto.HospitalId = query.HospitalId;
            var res =await fansMeetingDetailsService.IsAttendMeeting(queryDto);
            return ResultData<bool>.Success().AddData("isAttend",res);
        }

    }
}