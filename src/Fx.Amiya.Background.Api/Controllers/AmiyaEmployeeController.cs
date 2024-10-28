using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Vo.AmiyaEmployee;
using Fx.Amiya.Background.Api.Vo.Login;
using Fx.Amiya.Common;
using Fx.Amiya.Dto.AmiyaEmployee;
using Fx.Amiya.IService;
using Fx.Infrastructure;
using Fx.Infrastructure.Utils;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Fx.Amiya.Background.Api.Vo.Auth;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Common.Extensions;
using Fx.Amiya.Background.Api.Vo;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 啊美雅员工API
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class AmiyaEmployeeController : ControllerBase
    {
        private IAmiyaEmployeeService employeeService;
        private IHttpContextAccessor httpContextAccessor;
        private FxAppGlobal _fxAppGlobal;
        private IHospitalEmployeeService hospitalEmployeeService;
        public AmiyaEmployeeController(IAmiyaEmployeeService employeeService,
            IHttpContextAccessor httpContextAccessor,
            FxAppGlobal fxAppGlobal,
            IHospitalEmployeeService hospitalEmployeeService)
        {
            this.employeeService = employeeService;
            this.httpContextAccessor = httpContextAccessor;
            _fxAppGlobal = fxAppGlobal;
            this.hospitalEmployeeService = hospitalEmployeeService;
        }


        /// <summary>
        /// 检查密码是否合法
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkPassword/{password}")]
        public async Task<ResultData<bool>> CheckPasswordAsync(string password)
        {
            bool legitimate = await employeeService.CheckPasswordAsync(password);
            return ResultData<bool>.Success().AddData("islegitimate", legitimate);
        }


        /// <summary>
        /// 添加啊美雅员工信息
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ResultData> AddEmployeeAsync(AddAmiyaEmployeeVo addVo)
        {
            try
            {
                AddAmiyaEmployeeDto addDto = new AddAmiyaEmployeeDto();
                addDto.Name = addVo.Name;
                addDto.UserName = addVo.UserName;
                addDto.Password = addVo.Password.GetMD5String();
                addDto.PositionId = addVo.PositionId;
                addDto.IsCustomerService = addVo.IsCustomerService;
                addDto.Email = addVo.Email;
                addDto.LiveAnchorBaseId = addVo.LiveAnchorBaseId;
                addDto.OldCustomerCommission= addVo.OldCustomerCommission;
                addDto.NewCustomerCommission=addVo.NewCustomerCommission;
                addDto.InspectionCommission= addVo.InspectionCommission;
                addDto.AdministrativeInspectionCommission = addVo.AdministrativeInspectionCommission;
                addDto.CooperateLiveanchorNewCustomerCommission = addVo.CooperateLiveanchorNewCustomerCommission;
                addDto.CooperateLiveanchorOldCustomerCommission = addVo.CooperateLiveanchorOldCustomerCommission;
                addDto.TmallOrderCommission = addVo.TmallOrderCommission;
                addDto.PotentialNewCustomerCommission = addVo.PotentialNewCustomerCommission;
                addDto.AdministrativeInspection=addVo.AdministrativeInspection;
                await employeeService.AddAsync(addDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 重置帐户密码
        /// 默认密码为：12345
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPut("resetpassword/{employeeId}")]
        public async Task<ResultData> ResetPassword(int employeeId)
        {
            try
            {

                await employeeService.ResetPasswordAsync(employeeId, "12345".GetMD5String());
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 更新帐户可用性
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPut("updateAccountValid/{employeeId}/{valid}")]
        public async Task<ResultData> UpdateAccountValid(int employeeId, bool valid)
        {
            try
            {
                await employeeService.UpdateAccountValidAsync(employeeId, valid);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 取啊美雅员工列表（分页）
        /// </summary>
        /// <param name="keyword">搜索员工名字关键字</param>
        /// <param name="valid">是否有效</param>
        /// <param name="positionId">职位id</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        public async Task<ResultData<FxPageInfo<AmiyaEmployeeVo>>> GetListWithPageAsync(string keyword, bool valid, int positionId, int pageNum, int pageSize)
        {
            try
            {
                var q = await employeeService.GetListWithPageAsync(keyword, valid, positionId, pageNum, pageSize);

                var employeeInfos = from d in q.List
                                    select new AmiyaEmployeeVo
                                    {
                                        Id = d.Id,
                                        Name = d.Name,
                                        UserName = d.UserName,
                                        Valid = d.Valid,
                                        Email = d.Email,
                                        PositionId = d.PositionId,
                                        PositionName = d.PositionName,
                                        IsCustomerService = d.IsCustomerService,
                                        LiveAnchorBaseName=d.LiveAnchorBaseName,
                                        OldCustomerCommission=d.OldCustomerCommission,
                                        NewCustomerCommission=d.NewCustomerCommission,
                                        InspectionCommission=d.InspectionCommission
                                    };

                FxPageInfo<AmiyaEmployeeVo> employeePage = new FxPageInfo<AmiyaEmployeeVo>();
                employeePage.TotalCount = q.TotalCount;
                employeePage.List = employeeInfos;

                return ResultData<FxPageInfo<AmiyaEmployeeVo>>.Success().AddData("employeeInfo", employeePage);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<AmiyaEmployeeVo>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 根据员工编号获取员工信息
        /// </summary>
        /// <param name="id">员工编号</param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        public async Task<ResultData<AmiyaEmployeeVo>> GetByIdAsync(int id)
        {
            try
            {
                AmiyaEmployeeVo employeeVo = new AmiyaEmployeeVo();
                var q = await employeeService.GetByIdAsync(id);
                if (q.Id != 0)
                {
                    employeeVo.Id = q.Id;
                    employeeVo.Name = q.Name;
                    employeeVo.UserName = q.UserName;
                    employeeVo.Valid = q.Valid;
                    employeeVo.PositionId = q.PositionId;
                    employeeVo.Email = q.Email;
                    employeeVo.PositionName = q.PositionName;
                    employeeVo.IsCustomerService = q.IsCustomerService;
                    employeeVo.LiveAnchorIds = q.LiveAnchorIds;
                    employeeVo.LiveAnchorBaseId = q.LiveAnchorBaseId;
                    employeeVo.OldCustomerCommission=q.OldCustomerCommission;
                    employeeVo.NewCustomerCommission= q.NewCustomerCommission;
                    employeeVo.InspectionCommission=q.InspectionCommission;
                    employeeVo.AdministrativeInspectionCommission = q.AdministrativeInspectionCommission;
                    employeeVo.CooperateLiveanchorNewCustomerCommission = q.CooperateLiveanchorNewCustomerCommission;
                    employeeVo.CooperateLiveanchorOldCustomerCommission = q.CooperateLiveanchorOldCustomerCommission;
                    employeeVo.TmallOrderCommission = q.TmallOrderCommission;
                    employeeVo.PotentialNewCustomerCommission = q.PotentialNewCustomerCommission;
                    employeeVo.AdministrativeInspection=q.AdministrativeInspection;
                }
                return ResultData<AmiyaEmployeeVo>.Success().AddData("employeeInfo", employeeVo);
            }
            catch (Exception ex)
            {
                return ResultData<AmiyaEmployeeVo>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ResultData> UpdateAsync(UpdateAmiyaEmployeeVo updateVo)
        {
            try
            {
                UpdateAmiyaEmployeeDto updateDto = new UpdateAmiyaEmployeeDto();
                updateDto.Id = updateVo.Id;
                updateDto.Name = updateVo.Name;
                updateDto.UserName = updateVo.UserName;
                updateDto.Valid = updateVo.Valid;
                updateDto.Email = updateVo.Email;
                updateDto.PositionId = updateVo.PositionId;
                updateDto.IsCustomerService = updateVo.IsCustomerService;
                updateDto.LiveAnchorIds = updateVo.LiveAnchorIds;
                updateDto.LiveAnchorBaseId = updateVo.LiveAnchorBaseId;
                updateDto.InspectionCommission=updateVo.InspectionCommission;
                updateDto.OldCustomerCommission=updateVo.OldCustomerCommission;
                updateDto.NewCustomerCommission=updateVo.NewCustomerCommission;
                updateDto.AdministrativeInspectionCommission = updateVo.AdministrativeInspectionCommission;
                updateDto.CooperateLiveanchorNewCustomerCommission = updateVo.CooperateLiveanchorNewCustomerCommission;
                updateDto.CooperateLiveanchorOldCustomerCommission = updateVo.CooperateLiveanchorOldCustomerCommission;
                updateDto.TmallOrderCommission = updateVo.TmallOrderCommission;
                updateDto.PotentialNewCustomerCommission = updateVo.PotentialNewCustomerCommission;
                updateDto.AdministrativeInspection=updateVo.AdministrativeInspection;
                await employeeService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("updatePassword")]
        public async Task<ResultData> UpdatePasswordAsync(UpdatePasswordAmiiyaVo updateVo)
        {
            try
            {
                var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);

                UpdatePasswordAmiyaDto updateDto = new UpdatePasswordAmiyaDto();
                updateDto.OldPassword = updateVo.OldPassword.GetMD5String();
                updateDto.NewPassword = updateVo.NewPassword.GetMD5String();
                await employeeService.UpdatePasswordAsync(updateDto, employeeId);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 管理员修改员工密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("updatePasswordById")]
        public async Task<ResultData> UpdateEmployeePasswordByIdAsync(UpdateEmployeePasswordVo updateVo)
        {
            UpdateEmployeePasswordDto updateDto = new UpdateEmployeePasswordDto();
            updateDto.Id = updateVo.Id;
            updateDto.Password = updateVo.Password.GetMD5String();
            await employeeService.UpdateEmployeePasswordByIdAsync(updateDto);
            return ResultData.Success();
        }



        /// <summary>
        /// 根据员工编号删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultData> DeleteAsync(int id)
        {
            try
            {
                var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                await employeeService.DeleteAsync(id, employeeId);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }




        /// <summary>
        /// 根据员工id集合获取员工姓名列表
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("getEmployeeNameList")]
        [AllowAnonymous]
        public async Task<ResultData<List<EmployeeBaseInfoVo>>> GetEmployeeBaseInfoListAsync(EmployeeIdVo employee)
        {
            try
            {
                List<EmployeeBaseInfoVo> employeeList = new List<EmployeeBaseInfoVo>();



                if (employee.Type == (byte)EmployeeType.AmiyaEmployee)
                {
                    var amiyeEmployee = from d in await employeeService.GetInfoListIdsAsync(employee.Ids)
                                        select new EmployeeBaseInfoVo
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        };
                    employeeList.AddRange(amiyeEmployee.ToList());
                }
                else
                {
                    var hospitalEmplyee = from d in await hospitalEmployeeService.GetBaseInfoListAsync(employee.Ids)
                                          select new EmployeeBaseInfoVo
                                          {
                                              Id = d.Id,
                                              Name = d.Name
                                          };
                    employeeList.AddRange(hospitalEmplyee.ToList());
                }



                return ResultData<List<EmployeeBaseInfoVo>>.Success().AddData("employee", employeeList);
            }
            catch (Exception ex)
            {
                return ResultData<List<EmployeeBaseInfoVo>>.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 获取啊美雅客服列表（分页）
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns> 
        [HttpGet("customerServiceList")]
        public async Task<ResultData<FxPageInfo<CustomerServiceEmployeeVo>>> GetCustomerSeviceListWithPageAsync(int pageNum, int pageSize)
        {
            var q = await employeeService.GetCustomerSeviceListWithPageAsync(pageNum, pageSize);
            var customerService = from d in q.List
                                  select new CustomerServiceEmployeeVo
                                  {
                                      Id = d.Id,
                                      Name = d.Name,
                                      UserName = d.UserName,
                                      BindCustomerQuantity = d.BindCustomerQuantity,
                                      BindOrderQuantity = d.BindOrderQuantity
                                  };
            FxPageInfo<CustomerServiceEmployeeVo> customerServicePageInfo = new FxPageInfo<CustomerServiceEmployeeVo>();
            customerServicePageInfo.TotalCount = q.TotalCount;
            customerServicePageInfo.List = customerService;
            return ResultData<FxPageInfo<CustomerServiceEmployeeVo>>.Success().AddData("customerService", customerServicePageInfo);
        }



        /// <summary>
        /// 根据主播基础id获取客服姓名列表
        /// </summary>
        /// <param name="baseLiveAnchorId">主播基础信息id</param>
        /// <returns></returns>
        [HttpGet("customerServiceNameList")]
        public async Task<ResultData<List<AmiyaEmployeeNameVo>>> GetCustomerServiceNameListAsync(string baseLiveAnchorId)
        {
            var employee = from d in await employeeService.GetCustomerServiceNameListAsync(baseLiveAnchorId)
                           select new AmiyaEmployeeNameVo
                           {
                               Id = d.Id,
                               Name = d.Name
                           };
            return ResultData<List<AmiyaEmployeeNameVo>>.Success().AddData("employee", employee.ToList());
        }
        /// <summary>
        /// 获取运直播前人员姓名列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("operatingConsultingNameList")]
        public async Task<ResultData<List<AmiyaEmployeeNameVo>>> GetOperatingConsultingNameListAsync()
        {
            var employee = from d in await employeeService.GetOperatingConsultingNameListAsync()
                           select new AmiyaEmployeeNameVo
                           {
                               Id = d.Id,
                               Name = d.Name
                           };
            return ResultData<List<AmiyaEmployeeNameVo>>.Success().AddData("employee", employee.ToList());
        }

        /// <summary>
        /// 获取面诊人员姓名列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("consultationNameList")]
        public async Task<ResultData<List<AmiyaEmployeeNameVo>>> GetConsultingNameListAsync()
        {
            var employee = from d in await employeeService.GetConsultingNameListAsync()
                           select new AmiyaEmployeeNameVo
                           {
                               Id = d.Id,
                               Name = d.Name
                           };
            return ResultData<List<AmiyaEmployeeNameVo>>.Success().AddData("employee", employee.ToList());
        }

        /// <summary>
        /// 根据职位id获取人员(非客服)
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEmployeeByPositionId")]
        public async Task<ResultData<List<AmiyaEmployeeNameVo>>> GetemployeeByPositionIdAsync(int? positionId)
        {
            var employee = from d in await employeeService.GetemployeeByPositionIdAsync(positionId)
                           select new AmiyaEmployeeNameVo
                           {
                               Id = d.Id,
                               Name = d.Name
                           };
            return ResultData<List<AmiyaEmployeeNameVo>>.Success().AddData("employee", employee.ToList());
        }
        

    }
}