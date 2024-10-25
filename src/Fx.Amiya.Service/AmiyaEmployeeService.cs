using Fx.Amiya.DbModels.Model;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fx.Amiya.Dto.AmiyaEmployee;
using System.Text.RegularExpressions;
using Fx.Common;
using Fx.Common.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;
using Fx.Amiya.Dto;

namespace Fx.Amiya.Service
{
    public class AmiyaEmployeeService : IAmiyaEmployeeService
    {
        private IDalAmiyaEmployee dalAmiyaEmployee;
        private IDalBindCustomerService dalBindCustomerService;
        private IOrderAppInfoService orderAppInfoService;
        private IEmployeeBindLiveAnchorService employeeBindLiveAnchorService;
        private IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo;

        public List<BaseIdAndNameDto<int>> GetEmployeeNameList() { 
            return  dalAmiyaEmployee.GetAll().Where(e=>e.Valid==true).Select(e=>new BaseIdAndNameDto<int> {
                Id=e.Id,
                Name=e.Name
            }).ToList();
        }

        public AmiyaEmployeeService(IDalAmiyaEmployee dalAmiyaEmployee,
            IDalBindCustomerService dalBindCustomerService,
            IOrderAppInfoService orderAppInfoService,
            IEmployeeBindLiveAnchorService employeeBindLiveAnchorService, IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo)
        {
            this.dalAmiyaEmployee = dalAmiyaEmployee;
            this.dalBindCustomerService = dalBindCustomerService;
            this.orderAppInfoService = orderAppInfoService;
            this.employeeBindLiveAnchorService = employeeBindLiveAnchorService;
            this.dalLiveAnchorBaseInfo = dalLiveAnchorBaseInfo;
        }


        public async Task<AmiyaEmployeeDto> LoginAsync(string userName, string password)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .SingleOrDefaultAsync(e => e.UserName == userName);

                if (employee == null)
                    throw new Exception("用户名不存在");

                if (employee.Valid == false)
                    throw new Exception("账户已失效");

                if (employee.Password != password)
                    throw new Exception("密码错误");

                AmiyaEmployeeDto employeeDto = new AmiyaEmployeeDto();
                employeeDto.Id = employee.Id;
                employeeDto.Name = employee.Name;
                employeeDto.UserName = employee.UserName;
                employeeDto.Password = employee.Password;
                employeeDto.Valid = employee.Valid;
                employeeDto.PositionId = employee.AmiyaPositionId;
                employeeDto.PositionName = employee.AmiyaPositionInfo.Name;
                employeeDto.IsCustomerService = employee.IsCustomerService;
                employeeDto.IsDirector = employee.AmiyaPositionInfo.IsDirector;
                employeeDto.DepartmentId = employee.AmiyaPositionInfo.DepartmentId;
                employeeDto.DepartmentName = employee.AmiyaPositionInfo.AmiyaDepartment.Name;
                employeeDto.UserId = employee.UserId;
                employeeDto.Avatar = employee.Avatar;
                employeeDto.ReadDataCenter = employee.AmiyaPositionInfo.ReadDataCenter;
                employeeDto.ReadLiveAnchorData = employee.AmiyaPositionInfo.ReadLiveAnchorData;
                employeeDto.ReadSelfLiveAnchorData = employee.AmiyaPositionInfo.ReadSelfLiveAnchorData;
                employeeDto.ReadCooperateLiveAnchorData = employee.AmiyaPositionInfo.ReadCooperateLiveAnchorData;
                employeeDto.ReadTakeGoodsData = employee.AmiyaPositionInfo.ReadTakeGoodsData;
                return employeeDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<AmiyaEmployeeDto> LoginByUserIdAndCodeAsync(string userId, string code)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .SingleOrDefaultAsync(e => e.UserId == userId && e.Code == code);

                if (employee == null)
                    throw new Exception("用户名不存在");

                if (employee.Valid == false)
                    throw new Exception("账户已失效");

                if (employee.CodeExpireDate < DateTime.Now)
                    throw new Exception("登陆已超时，请重新登陆！");

                AmiyaEmployeeDto employeeDto = new AmiyaEmployeeDto();
                employeeDto.Id = employee.Id;
                employeeDto.Name = employee.Name;
                employeeDto.UserName = employee.UserName;
                employeeDto.Password = employee.Password;
                employeeDto.Valid = employee.Valid;
                employeeDto.IsDirector = employee.AmiyaPositionInfo.IsDirector;
                employeeDto.PositionId = employee.AmiyaPositionId;
                employeeDto.PositionName = employee.AmiyaPositionInfo.Name;
                employeeDto.IsCustomerService = employee.IsCustomerService;
                employeeDto.DepartmentId = employee.AmiyaPositionInfo.DepartmentId;
                employeeDto.DepartmentName = employee.AmiyaPositionInfo.AmiyaDepartment.Name;
                employeeDto.ReadDataCenter = employee.AmiyaPositionInfo.ReadDataCenter;
                employeeDto.ReadLiveAnchorData = employee.AmiyaPositionInfo.ReadLiveAnchorData;
                employeeDto.ReadCooperateLiveAnchorData = employee.AmiyaPositionInfo.ReadCooperateLiveAnchorData;
                employeeDto.ReadSelfLiveAnchorData = employee.AmiyaPositionInfo.ReadSelfLiveAnchorData;
                employeeDto.ReadTakeGoodsData = employee.AmiyaPositionInfo.ReadTakeGoodsData;
                return employeeDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> GetCodeAsync()
        {
            try
            {
                var businessInfo = await orderAppInfoService.GetBusinessWeChatAppInfo((byte)AppType.AmiyaBusinessWechat);
                string redirecturi = "https://app.ameiyes.com/amiyabusinesswechat/#/";
                redirecturi = HttpUtility.UrlEncode(redirecturi);
                string url2 = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={businessInfo.ShopId}&redirect_uri={redirecturi}&response_type=code&scope=snsapi_privateinfo&state=test&agentid={businessInfo.AppSecret}#wechat_redirect";
                var res = await HttpUtil.HTTPJsonGetAsync(url2);
                JObject requestObject = JsonConvert.DeserializeObject(res) as JObject;
                var errCode = requestObject["errcode"].ToString();
                if (errCode != "0")
                {
                    throw new Exception(requestObject["errmsg"].ToString());
                }


                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<AmiyaEmployeeDto> GetByCodeAsync(string code)
        {
            try
            {
                string userId = "";
                var employee = await dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .SingleOrDefaultAsync(e => e.Code == code);

                if (employee == null || employee.CodeExpireDate < DateTime.Now)
                {
                    var businessInfo = await orderAppInfoService.GetBusinessWeChatAppInfo((byte)AppType.AmiyaBusinessWechat);
                    string url = $"https://qyapi.weixin.qq.com/cgi-bin/auth/getuserinfo?access_token={businessInfo.AccessToken}&code={code}";
                    var res = await HttpUtil.HTTPJsonGetAsync(url);
                    JObject requestObject = JsonConvert.DeserializeObject(res) as JObject;
                    var errCode = requestObject["errcode"].ToString();
                    if (errCode != "0")
                    {
                        throw new Exception(requestObject["errmsg"].ToString());
                    }
                    userId = requestObject["userid"].ToString();
                    employee = await dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment).SingleOrDefaultAsync(e => e.UserId == userId);
                    if (employee == null)
                    {
                        var requestUrl = $"https://qyapi.weixin.qq.com/cgi-bin/auth/getuserdetail?access_token={businessInfo.AccessToken}";
                        var data = new { user_ticket = requestObject["user_ticket"].ToString() };
                        var ticketResult = await HttpUtil.HttpJsonPostAsync(requestUrl, JsonConvert.SerializeObject(data));
                        JObject ticketObject = JsonConvert.DeserializeObject(ticketResult) as JObject;
                        var ticketErrCode = ticketObject["errcode"].ToString();
                        if (ticketErrCode != "0")
                        {
                            var errmsg = ticketObject["errmsg"].ToString();
                            if (errmsg.Contains("invalid code"))
                            {
                                errmsg = "授权超时，请重新试一下~";
                            }
                            throw new Exception(errmsg);
                        }
                        string phone = ticketObject["mobile"].ToString();
                        employee = await dalAmiyaEmployee.GetAll()
                        .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment).SingleOrDefaultAsync(e => e.UserName == phone);
                        if (employee == null)
                        {
                            throw new Exception("您使用的企业微信绑定的手机号与啊美雅预约系统的手机号不同，请确保绑定手机号相同后再进行企业微信授权登陆！");
                        }
                    }
                    //刷新用户code
                    await this.UpdateBusinessWechatUserIdAndCode(employee.Id, userId, code);
                }
                if (employee.Valid == false)
                    throw new Exception("账户已失效");
                AmiyaEmployeeDto employeeDto = new AmiyaEmployeeDto();
                employeeDto.Id = employee.Id;
                employeeDto.Name = employee.Name;
                employeeDto.UserName = employee.UserName;
                employeeDto.Password = employee.Password;
                employeeDto.Valid = employee.Valid;
                employeeDto.PositionId = employee.AmiyaPositionId;
                employeeDto.PositionName = employee.AmiyaPositionInfo.Name;
                employeeDto.IsCustomerService = employee.IsCustomerService;
                employeeDto.DepartmentId = employee.AmiyaPositionInfo.DepartmentId;
                employeeDto.DepartmentName = employee.AmiyaPositionInfo.AmiyaDepartment.Name;
                employeeDto.UserId = employee.UserId;
                employeeDto.Code = code;

                return employeeDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateBusinessWechatUserIdAndCode(int id, string userId, string code)
        {
            var employeeInfo = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(x => x.Id == id);
            employeeInfo.UserId = userId;
            employeeInfo.Code = code;
            employeeInfo.CodeExpireDate = DateTime.Now.AddSeconds(300);
            await dalAmiyaEmployee.UpdateAsync(employeeInfo, true);
        }

        public async Task<bool> CheckPasswordAsync(string password)
        {
            string tr = "(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{8,}";
            Regex regex = new Regex(tr);
            if (!regex.IsMatch(password))
                return false;
            return true;

        }

        public async Task AddAsync(AddAmiyaEmployeeDto addDto)
        {
            try
            {
                var count = await dalAmiyaEmployee.GetAll().CountAsync(e => e.UserName == addDto.UserName);
                if (count > 0)
                    throw new Exception("用户名已被占用，请重新输入！");
                if (addDto.IsCustomerService && string.IsNullOrEmpty(addDto.LiveAnchorBaseId))
                    throw new Exception("请为该客服添加绑定主播！");
                AmiyaEmployee employee = new AmiyaEmployee()
                {
                    Name = addDto.Name,
                    UserName = addDto.UserName,
                    Password = addDto.Password,
                    AmiyaPositionId = addDto.PositionId,
                    Valid = true,
                    Email = addDto.Email,
                    IsCustomerService = addDto.IsCustomerService,
                    LiveAnchorBaseId = addDto.LiveAnchorBaseId,
                    OldCustomerCommission = addDto.OldCustomerCommission,
                    NewCustomerCommission = addDto.NewCustomerCommission,
                    InspectionCommission = addDto.InspectionCommission,
                    AdministrativeInspectionCommission = addDto.AdministrativeInspectionCommission,
                    CooperateLiveanchorNewCustomerCommission = addDto.CooperateLiveanchorNewCustomerCommission,
                    CooperateLiveanchorOldCustomerCommission = addDto.CooperateLiveanchorOldCustomerCommission,
                    TmallOrderCommission = addDto.TmallOrderCommission,
                    PotentialNewCustomerCommission = addDto.PotentialNewCustomerCommission,
                    AdministrativeInspection = addDto.AdministrativeInspection
                };

                await dalAmiyaEmployee.AddAsync(employee, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }



        public async Task<AmiyaEmployeeDto> GetByIdAsync(int id)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                    return new AmiyaEmployeeDto();

                AmiyaEmployeeDto employeeDto = new AmiyaEmployeeDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Avatar = employee.Avatar,
                    UserName = employee.UserName,
                    Password = employee.Password,
                    Valid = employee.Valid,
                    Email = (employee.Email == "0") ? "" : employee.Email,
                    PositionId = employee.AmiyaPositionId,
                    PositionName = employee.AmiyaPositionInfo.Name,
                    IsDirector = employee.AmiyaPositionInfo.IsDirector,
                    IsCustomerService = employee.IsCustomerService,
                    DepartmentId = employee.AmiyaPositionInfo.DepartmentId,
                    DepartmentName = employee.AmiyaPositionInfo.AmiyaDepartment.Name,
                    LiveAnchorBaseId = employee.LiveAnchorBaseId,
                    OldCustomerCommission = employee.OldCustomerCommission,
                    NewCustomerCommission = employee.NewCustomerCommission,
                    InspectionCommission = employee.InspectionCommission,
                    AdministrativeInspectionCommission = employee.AdministrativeInspectionCommission,
                    CooperateLiveanchorNewCustomerCommission = employee.CooperateLiveanchorNewCustomerCommission,
                    CooperateLiveanchorOldCustomerCommission = employee.CooperateLiveanchorOldCustomerCommission,
                    TmallOrderCommission = employee.TmallOrderCommission,
                    LiveAnchorBaseName = dalLiveAnchorBaseInfo.GetAll().Where(e => e.Id == employee.LiveAnchorBaseId).FirstOrDefault()?.LiveAnchorName,
                    PotentialNewCustomerCommission = employee.PotentialNewCustomerCommission,
                    AdministrativeInspection = employee.AdministrativeInspection,
                };
                if (employeeDto.IsCustomerService == true || employeeDto.PositionId == 19)
                {
                    employeeDto.LiveAnchorIds = new List<int>();
                    var liveAnchorIdsResult = await employeeBindLiveAnchorService.GetByEmpIdAsync(employeeDto.Id);
                    if (liveAnchorIdsResult.Count > 0)
                    {
                        foreach (var x in liveAnchorIdsResult)
                        {
                            employeeDto.LiveAnchorIds.Add(x.LiveAnchorId);
                        }
                    }
                }
                return employeeDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<AmiyaEmployeeDto>> GetByLiveAnchorBaseIdAsync(string liveAnchorBaseId)
        {
            try
            {
                List<AmiyaEmployeeDto> amiyaEmployeeDtos = new List<AmiyaEmployeeDto>();
                var employeeInfo = dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .Where(e => e.LiveAnchorBaseId == liveAnchorBaseId);

                if (employeeInfo == null)
                    return amiyaEmployeeDtos;
                var employee = await employeeInfo.ToListAsync();
                foreach (var x in employee)
                {
                    AmiyaEmployeeDto employeeDto = new AmiyaEmployeeDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Avatar = x.Avatar,
                        UserName = x.UserName,
                        Password = x.Password,
                        Valid = x.Valid,
                        Email = (x.Email == "0") ? "" : x.Email,
                        PositionId = x.AmiyaPositionId,
                        PositionName = x.AmiyaPositionInfo.Name,
                        IsCustomerService = x.IsCustomerService,
                        DepartmentId = x.AmiyaPositionInfo.DepartmentId,
                        DepartmentName = x.AmiyaPositionInfo.AmiyaDepartment.Name,
                        LiveAnchorBaseId = x.LiveAnchorBaseId
                    };
                    if (employeeDto.IsCustomerService == true || employeeDto.PositionId == 19)
                    {
                        employeeDto.LiveAnchorIds = new List<int>();
                        var liveAnchorIdsResult = await employeeBindLiveAnchorService.GetByEmpIdAsync(employeeDto.Id);
                        if (liveAnchorIdsResult.Count > 0)
                        {
                            foreach (var z in liveAnchorIdsResult)
                            {
                                employeeDto.LiveAnchorIds.Add(z.LiveAnchorId);
                            }
                        }
                    }
                    amiyaEmployeeDtos.Add(employeeDto);
                }
                return amiyaEmployeeDtos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<AmiyaEmployeeDto> GetByNameAsync(string name)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll()
                    .SingleOrDefaultAsync(e => e.Name == name);

                if (employee == null)
                    return new AmiyaEmployeeDto();

                AmiyaEmployeeDto employeeDto = new AmiyaEmployeeDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    UserName = employee.UserName,
                    Valid = employee.Valid,
                    Email = (employee.Email == "0") ? "" : employee.Email,
                    IsCustomerService = employee.IsCustomerService,
                };
                return employeeDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<FxPageInfo<AmiyaEmployeeDto>> GetListWithPageAsync(string keyword, bool valid, int positionId, int pageNum, int pageSize)
        {
            try
            {
                var employees = from d in dalAmiyaEmployee.GetAll()
                                where (keyword == null || d.Name.Contains(keyword))
                                && (d.Valid == valid)
                                && (positionId == 0 || d.AmiyaPositionId == positionId)
                                select new AmiyaEmployeeDto
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    UserName = d.UserName,
                                    Password = d.Password,
                                    Email = (d.Email == "0") ? "" : d.Email,
                                    Valid = d.Valid,
                                    PositionId = d.AmiyaPositionId,
                                    PositionName = d.AmiyaPositionInfo.Name,
                                    IsCustomerService = d.IsCustomerService,
                                    LiveAnchorBaseId = d.LiveAnchorBaseId,
                                    OldCustomerCommission = d.OldCustomerCommission,
                                    NewCustomerCommission = d.NewCustomerCommission,
                                    InspectionCommission = d.InspectionCommission
                                };
                FxPageInfo<AmiyaEmployeeDto> employeePageInfo = new FxPageInfo<AmiyaEmployeeDto>();
                employeePageInfo.TotalCount = await employees.CountAsync();
                employeePageInfo.List = await employees.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                foreach (var item in employeePageInfo.List)
                {
                    item.LiveAnchorBaseName = dalLiveAnchorBaseInfo.GetAll().Where(e => e.Id == item.LiveAnchorBaseId).FirstOrDefault()?.LiveAnchorName;
                }
                return employeePageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }




        public async Task ResetPasswordAsync(int employeeId, string password)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == employeeId);
                if (employee == null)
                    throw new Exception("员工编号错误");

                employee.Password = password;
                await dalAmiyaEmployee.UpdateAsync(employee, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }




        public async Task UpdateAccountValidAsync(int employeeId, bool valid)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == employeeId);
                if (employee == null)
                    throw new Exception("员工编号错误");

                employee.Valid = valid;
                await dalAmiyaEmployee.UpdateAsync(employee, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }




        public async Task UpdateAsync(UpdateAmiyaEmployeeDto updateDto)
        {
            try
            {

                var employee = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (employee == null)
                    throw new Exception("员工编号错误！");

                var count = await dalAmiyaEmployee.GetAll().CountAsync(e => e.UserName == updateDto.UserName && e.Id != updateDto.Id);
                if (count > 0)
                    throw new Exception("用户名已被占用，请重新输入！");

                if (updateDto.IsCustomerService && string.IsNullOrEmpty(updateDto.LiveAnchorBaseId))
                    throw new Exception("请为该客服绑定基础基础主播！");
                employee.Name = updateDto.Name;
                employee.UserName = updateDto.UserName;
                employee.Valid = updateDto.Valid;
                employee.Email = updateDto.Email;
                employee.AmiyaPositionId = updateDto.PositionId;
                employee.IsCustomerService = updateDto.IsCustomerService;
                employee.LiveAnchorBaseId = updateDto.LiveAnchorBaseId;
                employee.OldCustomerCommission = updateDto.OldCustomerCommission;
                employee.NewCustomerCommission = updateDto.NewCustomerCommission;
                employee.InspectionCommission = updateDto.InspectionCommission;
                employee.AdministrativeInspectionCommission = updateDto.AdministrativeInspectionCommission;
                employee.CooperateLiveanchorNewCustomerCommission = updateDto.CooperateLiveanchorNewCustomerCommission;
                employee.CooperateLiveanchorOldCustomerCommission = updateDto.CooperateLiveanchorOldCustomerCommission;
                employee.TmallOrderCommission = updateDto.TmallOrderCommission;
                employee.PotentialNewCustomerCommission = updateDto.PotentialNewCustomerCommission;
                employee.AdministrativeInspection = updateDto.AdministrativeInspection;
                await dalAmiyaEmployee.UpdateAsync(employee, true);


                if (updateDto.IsCustomerService == true || updateDto.PositionId == 19)
                {
                    if (updateDto.LiveAnchorIds.Count > 0)
                    {
                        //当为客服状态需要添加归属主播
                        UpdateEmployeeBindLiveAnchorDto dto = new UpdateEmployeeBindLiveAnchorDto();
                        dto.EmployeeId = updateDto.Id;
                        dto.LiveAnchorId = updateDto.LiveAnchorIds;
                        await employeeBindLiveAnchorService.UpdateAsync(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }




        public async Task DeleteAsync(int employeeId, int deleteBy)
        {
            try
            {
                if (employeeId == deleteBy)
                    throw new Exception("删除失败");

                var employee = await dalAmiyaEmployee.GetAll()
                   .SingleOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                    throw new Exception("啊美雅员工编号错误");

                await dalAmiyaEmployee.DeleteAsync(employee, true);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }








        public async Task UpdatePasswordAsync(UpdatePasswordAmiyaDto updateDto, int employeeId)
        {
            try
            {
                var employee = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == employeeId);

                if (updateDto.OldPassword != employee.Password)
                    throw new Exception("原始密码错误");

                employee.Password = updateDto.NewPassword;
                await dalAmiyaEmployee.UpdateAsync(employee, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 管理员修改员工密码
        /// </summary>
        /// <returns></returns>
        public async Task UpdateEmployeePasswordByIdAsync(UpdateEmployeePasswordDto updateDto)
        {
            var employee = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
            if (employee == null)
                throw new Exception("员工编号错误");
            employee.Password = updateDto.Password;
            await dalAmiyaEmployee.UpdateAsync(employee, true);
        }





        /// <summary>
        /// 根据员工id集合获取员工姓名列表
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeBaseInfoDto>> GetInfoListIdsAsync(int[] employeeIds)
        {
            try
            {
                List<AmiyaEmployeeBaseInfoDto> amiyaEmployeeList = new List<AmiyaEmployeeBaseInfoDto>();
                foreach (var item in employeeIds)
                {
                    var employee = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == item);
                    if (employee != null)
                    {
                        AmiyaEmployeeBaseInfoDto amiyaEmployee = new AmiyaEmployeeBaseInfoDto();
                        amiyaEmployee.Id = employee.Id;
                        amiyaEmployee.Name = employee.Name;
                        amiyaEmployeeList.Add(amiyaEmployee);
                    }
                }
                return amiyaEmployeeList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<FxPageInfo<CustomerServiceEmployeeDto>> GetCustomerSeviceListWithPageAsync(int pageNum, int pageSize)
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.IsCustomerService && d.Valid
                           select new CustomerServiceEmployeeDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                               UserName = d.UserName,
                           };

            var customerServiceEmployeeList = await employee.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

            foreach (var item in customerServiceEmployeeList)
            {
                var bindCustomerService = await dalBindCustomerService.GetAll().Where(e => e.CustomerServiceId == item.Id).ToListAsync();
                item.BindCustomerQuantity = bindCustomerService.Count();

            }


            FxPageInfo<CustomerServiceEmployeeDto> customerServicePageInfo = new FxPageInfo<CustomerServiceEmployeeDto>();
            customerServicePageInfo.TotalCount = await employee.CountAsync();
            customerServicePageInfo.List = customerServiceEmployeeList;

            return customerServicePageInfo;
        }




        /// <summary>
        /// 获取客服姓名列表
        /// </summary>
        /// <param name="baseLiveAnchorId">主播基础信息id</param>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetCustomerServiceNameListAsync(string baseLiveAnchorId = null)
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.IsCustomerService == true && (string.IsNullOrEmpty(baseLiveAnchorId) || d.LiveAnchorBaseId == baseLiveAnchorId)
                           && d.Valid == true
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();

        }
        /// <summary>
        /// 获取运营咨询人员姓名列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetOperatingConsultingNameListAsync()
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.Valid
                           && d.AmiyaPositionInfo.Id == 19
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();

        }

        /// <summary>
        /// 获取财务人员姓名列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetFinancialNameListAsync()
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.Valid
                           && d.AmiyaPositionInfo.Id == 13
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();

        }
        /// <summary>
        /// 获取面诊员姓名列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetConsultingNameListAsync()
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.IsCustomerService && d.Valid
                           && d.AmiyaPositionInfo.Name.Contains("面诊员")
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();

        }

        /// <summary>
        /// 根据职位id获取人员
        /// </summary>
        /// <returns></returns>

        public async Task<List<AmiyaEmployeeNameDto>> GetemployeeByPositionIdAsync(int? positionId)
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.Valid
                           && (!positionId.HasValue || d.AmiyaPositionInfo.Id == positionId)
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();

        }
        /// <summary>
        /// 根据主播基础信息id获取客服列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetCustomerServiceByBaseLiveAnchorid(string baseLiveAnchorId)
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.IsCustomerService == true && d.LiveAnchorBaseId == baseLiveAnchorId && (d.AmiyaPositionId == 4 || d.AmiyaPositionId == 30)
                           && d.Valid == true
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();

        }
        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task UpdateAvatarAsync(int id, string url)
        {
            var account = await dalAmiyaEmployee.GetAll().SingleOrDefaultAsync(e => e.Id == id);
            if (account == null) throw new Exception("用户编号错误！");
            account.Avatar = url;
            await dalAmiyaEmployee.UpdateAsync(account, true);
        }
        /// <summary>
        /// 判断员工是否是管理员或查看数据中心权限
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<bool> IsAdminOrHasPremissionLookDataCenterAsync(int employeeId)
        {
            var employee = await dalAmiyaEmployee.GetAll().Include(e => e.AmiyaPositionInfo).Where(e => e.Id == employeeId && (e.AmiyaPositionInfo.ReadDataCenter == true || e.AmiyaPositionId == 1)).SingleOrDefaultAsync();
            if (employee != null) return true;
            return false;
        }

        public async Task<List<AmiyaEmployeeDto>> GetByLiveAnchorBaseIdListAsync(List<string> liveAnchorBaseId)
        {
            try
            {
                List<AmiyaEmployeeDto> amiyaEmployeeDtos = new List<AmiyaEmployeeDto>();
                var employeeInfo = dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .Where(e => ((liveAnchorBaseId == null || liveAnchorBaseId.Count() == 0) || liveAnchorBaseId.Contains(e.LiveAnchorBaseId)) && e.IsCustomerService == true && e.Valid == true);
                var employee = await employeeInfo.Select(e => new AmiyaEmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                }).ToListAsync();
                return employee;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<List<AmiyaEmployeeDto>> GetByLiveAnchorBaseIdNameListAsync(List<string> liveAnchorBaseId)
        {
            try
            {
                List<AmiyaEmployeeDto> amiyaEmployeeDtos = new List<AmiyaEmployeeDto>();
                var employeeInfo = dalAmiyaEmployee.GetAll()
                    .Include(e => e.AmiyaPositionInfo).ThenInclude(e => e.AmiyaDepartment)
                    .Where(e => ((liveAnchorBaseId == null || liveAnchorBaseId.Count() == 0) || liveAnchorBaseId.Contains(e.LiveAnchorBaseId)) && e.Valid == true);
                var employee = await employeeInfo.Select(e => new AmiyaEmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                }).ToListAsync();
                return employee;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 获取直播前员工名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetLiveBeforeEmployeeNameListAsync() { 
            var nameList=await dalAmiyaEmployee.GetAll()
                .Where(e=>(e.AmiyaPositionId == 19 || e.AmiyaPositionId == 21) &&e.Valid==true)
                .Select(e=>new AmiyaEmployeeNameDto { Id=e.Id,Name=e.Name}).ToListAsync();
            return nameList;
        }
        /// <summary>
        /// 获取助理(包含行政客服)
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetAllAssistantAsync()
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.IsCustomerService == true && (d.AmiyaPositionId == 4 || d.AmiyaPositionId == 30)
                           && d.Valid == true
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();
        }
        

        /// <summary>
        /// 获取助理(不包含行政客服)
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmiyaEmployeeNameDto>> GetAssistantAsync()
        {
            var employee = from d in dalAmiyaEmployee.GetAll()
                           where d.IsCustomerService == true && d.AmiyaPositionId == 4
                           && d.Valid == true
                           select new AmiyaEmployeeNameDto
                           {
                               Id = d.Id,
                               Name = d.Name,
                           };
            return await employee.ToListAsync();
        }
    }
}
