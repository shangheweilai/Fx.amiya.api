using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.FansMeetingDetails.Input;
using Fx.Amiya.Dto.FansMeetingDetails.Result;
using Fx.Amiya.Dto.OperationLog;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class FansMeetingDetailsService : IFansMeetingDetailsService
    {
        private readonly IDalFansMeetingDetails dalFansMeetingDetails;
        private IHttpContextAccessor httpContextAccessor;
        private IOperationLogService operationLogService;
        public FansMeetingDetailsService(IDalFansMeetingDetails dalFansMeetingDetails, IHttpContextAccessor httpContextAccessor, IOperationLogService operationLogService)
        {
            this.dalFansMeetingDetails = dalFansMeetingDetails;
            this.httpContextAccessor = httpContextAccessor;
            this.operationLogService = operationLogService;
        }



        /// <summary>
        /// 根据条件获取粉丝见面会详情信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<FansMeetingDetailsDto>> GetListAsync(QueryFansMeetingDetailsDto query)
        {
            var fansMeetingDetailss = from d in dalFansMeetingDetails.GetAll().Include(x => x.FansMeetingInfo).Include(x => x.AmiyaEmployeeInfo)
                                      where (d.Valid == true && d.FansMeetingId == query.FansMeetingId)
                                      && (string.IsNullOrEmpty(query.KeyWord) || d.Phone.Contains(query.KeyWord))
                                      && (!query.StartDate.HasValue || d.AppointmentDate.Value >= query.StartDate.Value)
                                      && (!query.EndDate.HasValue || d.AppointmentDate.Value < query.EndDate.Value.AddDays(1).AddMilliseconds(-1))
                                      && (!query.IsToHospital.HasValue || d.IsToHospital == query.IsToHospital.Value)
                                      && (!query.IsDeal.HasValue || d.IsDeal == query.IsDeal.Value)
                                      && (!query.AmiyaEmployeeId.HasValue || d.AmiyaConsulationId == query.AmiyaEmployeeId.Value)
                                      && (!query.CustomerQuantity.HasValue || d.CustomerQuantity == query.CustomerQuantity.Value)
                                      && (!query.IsOdCustomer.HasValue || d.IsOldCustomer == query.IsOdCustomer.Value)
                                      && (!query.StartDealPrice.HasValue || d.CumulativeDealPrice >= query.StartDealPrice.Value)
                                      && (!query.EndDealPrice.HasValue || d.CumulativeDealPrice <= query.EndDealPrice.Value)
                                      select new FansMeetingDetailsDto
                                      {
                                          Id = d.Id,
                                          CreateDate = d.CreateDate,
                                          AmiyaConsulationId = d.AmiyaConsulationId,
                                          AmiyaConsulationName = d.AmiyaEmployeeInfo.Name,
                                          UpdateDate = d.UpdateDate,
                                          Valid = d.Valid,
                                          DeleteDate = d.DeleteDate,
                                          FansMeetingId = d.FansMeetingId,
                                          FansMeetingName = d.FansMeetingInfo.Name,
                                          OrderId = d.OrderId,
                                          AppointmentDate = d.AppointmentDate,
                                          AppointmentDetailsDate = d.AppointmentDetailsDate,
                                          CustomerName = d.CustomerName,
                                          Phone = query.IsHidePhone == true ? ServiceClass.GetIncompletePhone(d.Phone) : d.Phone,
                                          CustomerQuantity = d.CustomerQuantity,
                                          CustomerQuantityText = ServiceClass.CustomerQuantityText(d.CustomerQuantity),
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
            FxPageInfo<FansMeetingDetailsDto> fansMeetingDetailsPageInfo = new FxPageInfo<FansMeetingDetailsDto>();
            fansMeetingDetailsPageInfo.TotalCount = await fansMeetingDetailss.CountAsync();
            fansMeetingDetailsPageInfo.List = await fansMeetingDetailss.OrderBy(x => x.AppointmentDate.Value).ThenBy(x => x.AppointmentDetailsDate).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToListAsync();
            return fansMeetingDetailsPageInfo;
        }


        /// <summary>
        /// 根据条件获取粉丝见面会详情信息（非分页）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<FansMeetingDetailsDto>> GetExportListAsync(QueryFansMeetingDetailsDto query)
        {
            var fansMeetingDetailss = from d in dalFansMeetingDetails.GetAll().Include(x => x.FansMeetingInfo).Include(x => x.AmiyaEmployeeInfo)
                                      where (d.Valid == true && d.FansMeetingId == query.FansMeetingId)
                                      && (string.IsNullOrEmpty(query.KeyWord) || d.Phone.Contains(query.KeyWord))
                                      && (!query.StartDate.HasValue || d.AppointmentDate.Value >= query.StartDate.Value)
                                      && (!query.EndDate.HasValue || d.AppointmentDate.Value < query.EndDate.Value.AddDays(1).AddMilliseconds(-1))

                                      && (!query.IsToHospital.HasValue || d.IsToHospital == query.IsToHospital.Value)
                                      && (!query.IsDeal.HasValue || d.IsDeal == query.IsDeal.Value)
                                      && (!query.AmiyaEmployeeId.HasValue || d.AmiyaConsulationId == query.AmiyaEmployeeId.Value)
                                      && (!query.CustomerQuantity.HasValue || d.CustomerQuantity == query.CustomerQuantity.Value)
                                      && (!query.IsOdCustomer.HasValue || d.IsOldCustomer == query.IsOdCustomer.Value)
                                      && (!query.StartDealPrice.HasValue || d.CumulativeDealPrice >= query.StartDealPrice.Value)
                                      && (!query.EndDealPrice.HasValue || d.CumulativeDealPrice <= query.EndDealPrice.Value)
                                      select new FansMeetingDetailsDto
                                      {
                                          Id = d.Id,
                                          CreateDate = d.CreateDate,
                                          AmiyaConsulationId = d.AmiyaConsulationId,
                                          AmiyaConsulationName = d.AmiyaEmployeeInfo.Name,
                                          UpdateDate = d.UpdateDate,
                                          Valid = d.Valid,
                                          DeleteDate = d.DeleteDate,
                                          FansMeetingId = d.FansMeetingId,
                                          FansMeetingName = d.FansMeetingInfo.Name,
                                          OrderId = d.OrderId,
                                          AppointmentDate = d.AppointmentDate,
                                          AppointmentDetailsDate = d.AppointmentDetailsDate,
                                          CustomerName = d.CustomerName,
                                          Phone = query.IsHidePhone == true ? ServiceClass.GetIncompletePhone(d.Phone) : d.Phone,
                                          CustomerQuantity = d.CustomerQuantity,
                                          CustomerQuantityText = ServiceClass.CustomerQuantityText(d.CustomerQuantity),
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
            List<FansMeetingDetailsDto> fansMeetingDetailsPageInfo = new List<FansMeetingDetailsDto>();
            fansMeetingDetailsPageInfo = await fansMeetingDetailss.OrderBy(x => x.AppointmentDate.Value).ThenBy(x => x.AppointmentDetailsDate).ToListAsync();
            return fansMeetingDetailsPageInfo;
        }


        /// <summary>
        /// 添加粉丝见面会详情
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddFansMeetingDetailsDto addDto)
        {

            try
            {
                //查询是否存在该条手机号
                var isExist = await dalFansMeetingDetails.GetAll()
                               .Where(d => d.Valid == true && d.FansMeetingId == addDto.FansMeetingId && d.Phone == addDto.Phone).ToListAsync();
                if (isExist.Count() > 0)
                {
                    throw new Exception("粉丝见面会中已存在该手机号，请重新确认后再添加，手机号：" + addDto.Phone + "！");
                }

                FansMeetingDetails fansMeetingDetails = new FansMeetingDetails();
                fansMeetingDetails.Id = Guid.NewGuid().ToString();
                fansMeetingDetails.CreateDate = DateTime.Now;
                fansMeetingDetails.AmiyaConsulationId = addDto.AmiyaConsulationId;
                fansMeetingDetails.Valid = true;
                fansMeetingDetails.FansMeetingId = addDto.FansMeetingId;
                fansMeetingDetails.OrderId = addDto.OrderId;
                fansMeetingDetails.AppointmentDate = addDto.AppointmentDate;
                fansMeetingDetails.AppointmentDetailsDate = addDto.AppointmentDetailsDate;
                fansMeetingDetails.CustomerName = addDto.CustomerName;
                fansMeetingDetails.Phone = addDto.Phone;
                fansMeetingDetails.CustomerQuantity = addDto.CustomerQuantity;
                fansMeetingDetails.IsOldCustomer = addDto.IsOldCustomer;
                //如果啊美雅助理字段为空则归属给客户池客服
                fansMeetingDetails.AmiyaConsulationId = addDto.AmiyaConsulationId == 0 ? 188 : addDto.AmiyaConsulationId;
                fansMeetingDetails.HospitalConsulationName = addDto.HospitalConsulationName;
                fansMeetingDetails.City = addDto.City;
                fansMeetingDetails.TravelInformation = addDto.TravelInformation;
                fansMeetingDetails.IsNeedDriver = addDto.IsNeedDriver;
                fansMeetingDetails.HotelPlan = addDto.HotelPlan;
                fansMeetingDetails.PlanConsumption = addDto.PlanConsumption;
                fansMeetingDetails.Remark = addDto.Remark;
                fansMeetingDetails.CustomerPictureUrl = addDto.CustomerPictureUrl;
                await dalFansMeetingDetails.AddAsync(fansMeetingDetails, true);

            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }


        public async Task<FansMeetingDetailsDto> GetByIdAsync(string id)
        {
            var result = await dalFansMeetingDetails.GetAll().Include(x => x.FansMeetingInfo).Include(x => x.AmiyaEmployeeInfo).Where(x => x.Id == id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new FansMeetingDetailsDto();
            }

            FansMeetingDetailsDto returnResult = new FansMeetingDetailsDto();
            returnResult.Id = result.Id;
            returnResult.CreateDate = result.CreateDate;
            returnResult.AmiyaConsulationId = result.AmiyaConsulationId;
            returnResult.AmiyaConsulationName = result.AmiyaEmployeeInfo.Name;
            returnResult.Valid = result.Valid;
            returnResult.FansMeetingId = result.FansMeetingId;
            returnResult.OrderId = result.OrderId;
            returnResult.AppointmentDate = result.AppointmentDate;
            returnResult.AppointmentDetailsDate = result.AppointmentDetailsDate;
            returnResult.CustomerName = result.CustomerName;
            returnResult.Phone = result.Phone;
            returnResult.CustomerQuantity = result.CustomerQuantity;
            returnResult.IsOldCustomer = result.IsOldCustomer;
            returnResult.AmiyaConsulationId = result.AmiyaConsulationId;
            returnResult.HospitalConsulationName = result.HospitalConsulationName;
            returnResult.City = result.City;
            returnResult.TravelInformation = result.TravelInformation;
            returnResult.IsNeedDriver = result.IsNeedDriver;
            returnResult.HotelPlan = result.HotelPlan;
            returnResult.PlanConsumption = result.PlanConsumption;
            returnResult.Remark = result.Remark;
            returnResult.CustomerPictureUrl = result.CustomerPictureUrl;
            returnResult.IsDeal = result.IsDeal;
            returnResult.IsToHospital = result.IsToHospital;
            returnResult.CumulativeDealPrice = result.CumulativeDealPrice;
            returnResult.UnDealReason = result.UnDealReason;
            returnResult.FansMeetingProject = result.FansMeetingProject;
            returnResult.FollowUpContent = result.FollowUpContent;
            returnResult.NextAppointmentDate = result.NextAppointmentDate;
            returnResult.IsNeedHospitalHelp = result.IsNeedHospitalHelp;
            returnResult.IsNeedCustomerServiceHelp = result.IsNeedCustomerServiceHelp;
            returnResult.HospitalMemberCardId = result.HospitalMemberCardId;
            return returnResult;
        }



        /// <summary>
        /// 修改粉丝见面会详情
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateFansMeetingDetailsDto updateDto)
        {
            var result = await dalFansMeetingDetails.GetAll().Where(x => x.Id == updateDto.Id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("未找到粉丝见面会详情信息");
            var isExist = await dalFansMeetingDetails.GetAll()
                              .Where(d => d.Valid == true && d.FansMeetingId == updateDto.FansMeetingId && d.Phone == updateDto.Phone && d.Id != updateDto.Id).ToListAsync();
            if (isExist.Count() > 0)
            {
                throw new Exception("粉丝见面会中已存在该手机号，请重新确认后再添加！手机号：" + updateDto.Phone);
            }
            if (updateDto.IsHospitalUpdate != true)
            {
                if (result.AmiyaConsulationId != updateDto.AmiyaConsulationId)
                {
                    throw new Exception("无法修改其他助理数据信息！");
                }
            }
            result.FansMeetingId = updateDto.FansMeetingId;
            result.OrderId = updateDto.OrderId;
            result.AppointmentDate = updateDto.AppointmentDate;
            result.AppointmentDetailsDate = updateDto.AppointmentDetailsDate;
            result.CustomerName = updateDto.CustomerName;
            result.Phone = updateDto.Phone;
            result.CustomerQuantity = updateDto.CustomerQuantity;
            result.IsOldCustomer = updateDto.IsOldCustomer;
            result.AmiyaConsulationId = updateDto.AmiyaConsulationId;
            result.HospitalConsulationName = updateDto.HospitalConsulationName;
            result.City = updateDto.City;
            result.TravelInformation = updateDto.TravelInformation;
            result.IsNeedDriver = updateDto.IsNeedDriver;
            result.HotelPlan = updateDto.HotelPlan;
            result.PlanConsumption = updateDto.PlanConsumption;
            result.Remark = updateDto.Remark;
            result.CustomerPictureUrl = updateDto.CustomerPictureUrl;
            result.UpdateDate = DateTime.Now;
            result.IsDeal = updateDto.IsDeal;
            result.IsToHospital = updateDto.IsToHospital;
            result.CumulativeDealPrice = updateDto.CumulativeDealPrice;

            result.UnDealReason = updateDto.UnDealReason;
            result.FansMeetingProject = updateDto.FansMeetingProject;
            result.FollowUpContent = updateDto.FollowUpContent;
            result.NextAppointmentDate = updateDto.NextAppointmentDate;
            result.IsNeedHospitalHelp = updateDto.IsNeedHospitalHelp;
            result.IsNeedCustomerServiceHelp = updateDto.IsNeedCustomerServiceHelp;
            result.HospitalMemberCardId = updateDto.HospitalMemberCardId;
            await dalFansMeetingDetails.UpdateAsync(result, true);
        }
        /// <summary>
        /// 作废粉丝见面会详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id, int employeeId)
        {
            try
            {
                var result = await dalFansMeetingDetails.GetAll().SingleOrDefaultAsync(e => e.Id == id && e.Valid == true);
                if (result == null)
                    throw new Exception("未找到粉丝见面会详情信息");
                if (result.AmiyaConsulationId != employeeId)
                {
                    throw new Exception("无法删除其他助理数据信息！");
                }
                result.Valid = false;
                result.DeleteDate = DateTime.Now;
                await dalFansMeetingDetails.UpdateAsync(result, true);

            }
            catch (Exception er)
            {
                throw new Exception(er.Message.ToString());
            }
        }
        /// <summary>
        /// 判断用户是否参加粉丝见面会
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsAttendMeeting(AttendMeetingQueryDto query)
        {
            var result = dalFansMeetingDetails.GetAll()
                .Where(e => e.FansMeetingId == query.Id)
                .Where(e => e.Phone == query.Phone)
                .Where(e => e.FansMeetingInfo.HospitalId == query.HospitalId)
                .Count();
            return result > 0;
        }
        /// <summary>
        /// 生成成交信息
        /// </summary>
        /// <param name="generate"></param>
        /// <returns></returns>
        public async Task GenerateDealInfoAsync(GenerateDealInfoDto generate)
        {
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Source = (int)RequestSource.AmiyaBackground;
            operationLog.Code = 0;
            operationLog.OperationBy = generate.EmpId;
            try
            {
                var record = await dalFansMeetingDetails.GetAll().Where(e => e.Id == generate.Id).FirstOrDefaultAsync();
                record.IsDeal = generate.IsDeal;
                record.IsToHospital = generate.IsToHospital;
                record.CumulativeDealPrice = generate.DealPrice;
                record.UnDealReason = generate.UnDealReason;
                record.FansMeetingProject = generate.FansMeetingProject;
                record.FollowUpContent = generate.FollowUpContent;
                record.NextAppointmentDate = generate.NextAppointmentDate;
                record.IsNeedHospitalHelp = generate.IsNeedHospitalHelp;
                record.UpdateDate = DateTime.Now;
                await dalFansMeetingDetails.UpdateAsync(record, true);
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                throw ex;
            }
            finally
            {
                operationLog.Parameters = JsonConvert.SerializeObject(generate);
                operationLog.RequestType = (int)RequestType.Update;
                operationLog.RouteAddress = "粉丝见面会数据同步";
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }
    }
}
