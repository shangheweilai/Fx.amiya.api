using Fx.Amiya.Dto.HospitalInfo;
using Fx.Amiya.IDal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fx.Amiya.IService;
using Fx.Infrastructure;
using Fx.Amiya.DbModels.Model;
using Fx.Infrastructure.DataAccess;
using Fx.Common;
using Fx.Amiya.Dto.HospitalEnvironmentPicture;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.HospitalContract.Input;
using Fx.Amiya.Dto.HospitalContract.Result;
using Fx.Amiya.Dto.HospitalProject.Input;
using Fx.Amiya.Dto.HospitalProject.Result;

namespace Fx.Amiya.Service
{
    public class HospitalInfoService : IHospitalInfoService
    {
        private IDalHospitalInfo dalHospitalInfo;
        private IDalHospitalTagDetail dalHospitalTagDetail;
        private IUnitOfWork unitOfWork;
        private IDalHospitalPartakeItem dalHospitalQuotedPriceItemInfo;
        private IHospitalEnvironmentPictureService _hospitalEnvironmentPictureService;
        private IDalItemInfo dalItemInfo;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private IDalCompanyBaseInfo dalCompanyBaseInfo;
        private IDalHospitalContract dalHospitalContract;
        private IDalAmiyaEmployee dalAmiyaEmployee;
        private IDalHospitalProject dalHospitalProject;
        private IDalContentPlatformOrderSend dalContentPlatformOrderSend;
        public HospitalInfoService(IDalHospitalInfo dalHospitalInfo,
            IDalHospitalTagDetail dalHospitalTagDetail,
            IUnitOfWork unitOfWork,
            IHospitalEnvironmentPictureService hospitalEnvironmentPictureService,
            IDalHospitalPartakeItem dalHospitalQuotedPriceItemInfo,
            IAmiyaEmployeeService amiyaEmployeeService,
            IDalHospitalProject dalHospitalProject,
            IDalItemInfo dalItemInfo, IDalCompanyBaseInfo dalCompanyBaseInfo, IDalHospitalContract dalHospitalContract, IDalAmiyaEmployee dalAmiyaEmployee, IDalContentPlatformOrderSend dalContentPlatformOrderSend)
        {
            this.dalHospitalInfo = dalHospitalInfo;
            this.dalHospitalTagDetail = dalHospitalTagDetail;
            this.unitOfWork = unitOfWork;
            this.dalHospitalProject = dalHospitalProject;
            this.dalHospitalQuotedPriceItemInfo = dalHospitalQuotedPriceItemInfo;
            this.dalItemInfo = dalItemInfo;
            _hospitalEnvironmentPictureService = hospitalEnvironmentPictureService;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.dalCompanyBaseInfo = dalCompanyBaseInfo;
            this.dalHospitalContract = dalHospitalContract;
            this.dalAmiyaEmployee = dalAmiyaEmployee;
            this.dalContentPlatformOrderSend = dalContentPlatformOrderSend;
        }


        /// <summary>
        /// 获取医院列表（分页）
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<HospitalInfoDto>> GetListWithPageAsync(string keyword, int? cityId, int pageNum, int pageSize, bool? valid)
        {
            try
            {
                if (pageSize > 100)
                    throw new Exception("每次查询不能超过100条");

                var hospital = from d in dalHospitalInfo.GetAll()
                               where (keyword == null || d.Name.Contains(keyword))
                               && (valid == null || d.Valid == valid)
                               && (cityId == null || d.CityId == cityId)
                               orderby d.Sort descending
                               select new HospitalInfoDto
                               {
                                   Id = d.Id,
                                   Name = d.Name,
                                   Address = d.Address,
                                   Longitude = d.Longitude,
                                   Latitude = d.Latitude,
                                   Phone = d.Phone,
                                   Valid = d.Valid,
                                   ThumbPicUrl = d.ThumbPicUrl,
                                   CreateBy = d.CreateBy,
                                   CreateName = d.CreateByAmiyaEmployee.Name,
                                   CreateDate = d.CreateDate,
                                   UpdateBy = d.UpdateBy,
                                   UpdateName = d.UpdateByAmiyaEmployee.Name,
                                   UpdateDate = d.UpdateDate,
                                   CityId = d.CityId,
                                   City = d.CooperativeHospitalCity.Name,
                                   DueTime = d.DueTime,
                                   ContractUrl = d.ContractUrl,
                                   BelongCompany = d.BelongCompany,
                                   IsShareInMiniProgram = d.IsShareInMiniProgram,
                                   SimpleName = d.SimpleName,
                                   Sort = d.Sort,
                                   SendOrder = d.SendOrder,
                                   SendOrderText = d.SendOrder.HasValue ? ServiceClass.GetSendOrderText(d.SendOrder.Value) : null,
                                   NewCustomerCommissionRatio = d.NewCustomerCommissionRatio,
                                   OldCustomerCommissionRatio = d.OldCustomerCommissionRatio,
                                   RepeatOrderRule = d.RepeatOrderRule,
                                   YearServiceFee = d.YearServiceFee,
                                   YearServiceFeeText = d.YearServiceFee.HasValue ? ServiceClass.GetYearServiceFeeOrSecurityDepositText(d.YearServiceFee.Value) : null,
                                   SecurityDeposit = d.SecurityDeposit,
                                   SecurityDepositText = d.SecurityDeposit.HasValue ? ServiceClass.GetYearServiceFeeOrSecurityDepositText(d.SecurityDeposit.Value) : null,
                                   YearServiceMoney = d.YearServiceMoney,
                                   SecurityDepositMoney = d.SecurityDepositMoney

                               };
                FxPageInfo<HospitalInfoDto> hospitalPageInfo = new FxPageInfo<HospitalInfoDto>();
                hospitalPageInfo.TotalCount = await hospital.CountAsync();
                hospitalPageInfo.List = await hospital.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

                //展示离到期时间还有多少并且如果到期后自动改为无效
                foreach (var x in hospitalPageInfo.List)
                {

                    if (x.DueTime.HasValue)
                    {
                        var dateResultDay = (x.DueTime.Value - DateTime.Now).Days;
                        var dateResultHours = (x.DueTime.Value - DateTime.Now).Hours;
                        var dateResultMin = (x.DueTime.Value - DateTime.Now).Minutes;
                        var dateResultSecond = (x.DueTime.Value - DateTime.Now).Seconds;
                        if (dateResultDay > 0)
                        {
                            if (dateResultDay < 10)
                            {
                                x.HasUsedTime = "<span style=color:red>" + dateResultDay + "天</span>";
                            }
                            else
                            {
                                x.HasUsedTime = "<span style=color:green>" + dateResultDay + "天</span>";
                            }
                        }
                        else if (dateResultHours > 0)
                        {
                            x.HasUsedTime = "<span style=color:red>" + dateResultHours + "小时</span>";
                        }

                        else if (dateResultMin > 0)
                        {
                            x.HasUsedTime = "<span style=color:red>" + dateResultMin + "分</span>";
                        }
                        else if (dateResultSecond > 0)
                        {
                            x.HasUsedTime = "<span style=color:red>" + dateResultSecond + "秒</span>";
                        }

                        else
                        {

                            x.HasUsedTime = "<span style=color:gray>已过期</span>";
                            ////验证是否需要修改为已过期
                            //if (x.Valid != false)
                            //{
                            //    var updateInfo = await dalHospitalInfo.GetAll().SingleOrDefaultAsync(e => e.Id == x.Id);
                            //    updateInfo.Valid = false;
                            //    await dalHospitalInfo.UpdateAsync(updateInfo, true);
                            //    x.Valid = false;
                            //}
                        }
                    }

                }
                return hospitalPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 获取即将到期的医院列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<FxPageInfo<HospitalInfoDto>> GetAboutToExpiredListAsync()
        {
            try
            {
                DateTime nowDate = DateTime.Now.AddMonths(1);
                var hospital = from d in dalHospitalInfo.GetAll()
                               where (d.DueTime.Value < nowDate && d.Valid == true)
                               select new HospitalInfoDto
                               {
                                   Id = d.Id,
                                   Name = d.Name,

                               };
                FxPageInfo<HospitalInfoDto> hospitalPageInfo = new FxPageInfo<HospitalInfoDto>();
                hospitalPageInfo.TotalCount = await hospital.CountAsync();
                hospitalPageInfo.List = await hospital.ToListAsync();
                return hospitalPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 获取医院资料审核情况列表（分页）
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="CheckState">审核状态（-1查询全部）</param>
        /// <param name="submitState">资料提交状态（-1查询全部）</param>
        /// <returns></returns>
        public async Task<FxPageInfo<HospitalCheckInfoDto>> GetCheckListWithPageAsync(string keyword, int pageNum, int pageSize, int CheckState, int submitState)
        {
            try
            {
                if (pageSize > 100)
                    throw new Exception("每次查询不能超过100条");

                var hospital = from d in dalHospitalInfo.GetAll()
                               where (keyword == null || d.Name.Contains(keyword))
                               && (d.Valid == true)
                               && (CheckState == -1 || d.CheckState == CheckState)
                               && (submitState == -1 || d.SubmitState == submitState)
                               select new HospitalCheckInfoDto
                               {
                                   Id = d.Id,
                                   Name = d.Name,
                                   ThumbPicUrl = d.ThumbPicUrl,
                                   CheckState = d.CheckState,
                                   CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState),
                                   CheckBy = d.CheckBy,
                                   CheckDate = d.CheckDate,
                                   CheckRemark = d.CheckRemark,
                                   SubmitStateText = ServiceClass.GetSubmitTypeText(d.SubmitState),
                                   SubmitState = d.SubmitState,
                               };
                FxPageInfo<HospitalCheckInfoDto> hospitalPageInfo = new FxPageInfo<HospitalCheckInfoDto>();
                hospitalPageInfo.TotalCount = await hospital.CountAsync();
                hospitalPageInfo.List = await hospital.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

                return hospitalPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 啊美雅全国供应链派单指南
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<HospitalInfoDto>> GetAmiyaTotalSendHospitalInstructionsAsync(QueryAmiyaTotalSendHospitalInstructionsDto query)
        {
            try
            {
                var hospital = from d in dalHospitalInfo.GetAll().Include(x => x.CooperativeHospitalCity).ThenInclude(x => x.Province)
                               where
                               (query.HospitalName == null || d.Name.Contains(query.HospitalName) || d.SimpleName.Contains(query.HospitalName))
                                && (d.Valid == true)
                               && (query.CityId == 0 || d.CityId == query.CityId)
                               select new HospitalInfoDto
                               {
                                   Id = d.Id,
                                   Name = d.Name,
                                   CityId = d.CityId,
                                   City = d.CooperativeHospitalCity.Name,
                                   CitySort = d.CooperativeHospitalCity.Sort,
                                   ProvinceId = d.CooperativeHospitalCity.ProvinceId,
                                   ProvinceName = d.CooperativeHospitalCity.Province.Name,
                                   DueTime = d.DueTime,
                                   Sort = d.Sort,
                                   SendOrderText = d.SendOrder.HasValue ? ServiceClass.GetSendOrderText(d.SendOrder.Value) : null,
                                   NewCustomerCommissionRatio = d.NewCustomerCommissionRatio,
                                   OldCustomerCommissionRatio = d.OldCustomerCommissionRatio,
                                   RepeatOrderRule = d.RepeatOrderRule,
                                   YearServiceFeeText = d.YearServiceFee.HasValue ? ServiceClass.GetYearServiceFeeOrSecurityDepositText(d.YearServiceFee.Value) : null,
                                   SecurityDepositText = d.SecurityDeposit.HasValue ? ServiceClass.GetYearServiceFeeOrSecurityDepositText(d.SecurityDeposit.Value) : null,
                                   YearServiceMoney = d.YearServiceMoney,
                                   SecurityDepositMoney = d.SecurityDepositMoney

                               };
                List<HospitalInfoDto> hospitalPageInfo = new List<HospitalInfoDto>();
                hospitalPageInfo = await hospital.OrderByDescending(x => x.CitySort).ThenByDescending(x => x.Sort).ToListAsync();

                //展示离到期时间还有多少并且如果到期后自动改为无效
                foreach (var x in hospitalPageInfo)
                {
                    //if (x.CityId.HasValue)
                    //{
                    //    var province = await provinceService.GetByIdAsync(x.ProvinceId);
                    //    x.ProvinceName = province.Name;
                    //}
                    if (x.DueTime.HasValue)
                    {
                        var dateResultDay = (x.DueTime.Value - DateTime.Now).Days;
                        var dateResultHours = (x.DueTime.Value - DateTime.Now).Hours;
                        var dateResultMin = (x.DueTime.Value - DateTime.Now).Minutes;
                        var dateResultSecond = (x.DueTime.Value - DateTime.Now).Seconds;
                        if (dateResultDay > 0)
                        {
                            if (dateResultDay < 10)
                            {
                                x.HasUsedTime = "<span style=color:red>" + dateResultDay + "天</span>";
                            }
                            else
                            {
                                x.HasUsedTime = "<span style=color:green>" + dateResultDay + "天</span>";
                            }
                        }
                        else if (dateResultHours > 0)
                        {
                            x.HasUsedTime = "<span style=color:red>" + dateResultHours + "小时</span>";
                        }

                        else if (dateResultMin > 0)
                        {
                            x.HasUsedTime = "<span style=color:red>" + dateResultMin + "分</span>";
                        }
                        else if (dateResultSecond > 0)
                        {
                            x.HasUsedTime = "<span style=color:red>" + dateResultSecond + "秒</span>";
                        }

                        else
                        {

                            x.HasUsedTime = "<span style=color:gray>已过期</span>";
                            //验证是否需要修改为已过期
                            if (x.Valid != false)
                            {
                                var updateInfo = await dalHospitalInfo.GetAll().SingleOrDefaultAsync(e => e.Id == x.Id);
                                updateInfo.Valid = false;
                                await dalHospitalInfo.UpdateAsync(updateInfo, true);
                                x.Valid = false;
                            }
                        }
                    }

                }
                return hospitalPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 获取医院名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HospitalNameDto>> GetHospitalNameListAsync(bool? valid, string name)
        {
            try
            {
                var hospital = from d in dalHospitalInfo.GetAll()
                               where (valid == null || d.Valid == valid)
                               && (string.IsNullOrWhiteSpace(name) || d.Name.Contains(name))
                               select new HospitalNameDto
                               {
                                   Id = d.Id,
                                   Name = d.Name
                               };

                return await hospital.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 获取医院简称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HospitalNameDto>> GetHospitalSimpleNameListAsync(bool? valid)
        {
            try
            {
                var hospital = from d in dalHospitalInfo.GetAll()
                               where (valid == null || d.Valid == valid)
                               select new HospitalNameDto
                               {
                                   Id = d.Id,
                                   Name = d.SimpleName
                               };

                return await hospital.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task HospitalInfoCheckAsync(HospitalInfoCheckInfoDto updateDto)
        {
            var result = await dalHospitalInfo.GetAll().Where(x => x.Id == updateDto.Id).FirstOrDefaultAsync();
            result.CheckState = updateDto.CheckState;
            result.CheckBy = updateDto.CheckBy;
            result.CheckDate = DateTime.Now;
            result.CheckRemark = updateDto.Remark;
            if (updateDto.CheckState == Convert.ToInt32(CheckType.CheckNotPassed))
            {
                result.SubmitState = Convert.ToInt32(SubmintType.UnSubmit);
            }
            await dalHospitalInfo.UpdateAsync(result, true);
        }

        /// <summary>
        /// 获取资料审核通过的医院医院名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HospitalNameDto>> GetCheckPassedHospitalNameListAsync(bool? valid, string name)
        {
            try
            {
                var hospital = from d in dalHospitalInfo.GetAll()
                               where (valid == null || d.Valid == valid)
                               && (string.IsNullOrWhiteSpace(name) || d.Name.Contains(name))
                               && (d.CheckState == Convert.ToInt32(CheckType.CheckedSuccess))
                               select new HospitalNameDto
                               {
                                   Id = d.Id,
                                   Name = d.Name
                               };

                return await hospital.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 添加医院
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddHospitalInfoDto addDto, int employeeId)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var hospitalCount = await dalHospitalInfo.GetAll().CountAsync(e => e.Name == addDto.Name);
                if (hospitalCount > 0)
                    throw new Exception("添加失败，已存在该医院名称");

                HospitalInfo hospitalInfo = new HospitalInfo();
                hospitalInfo.Name = addDto.Name;
                hospitalInfo.SimpleName = addDto.SimpleName;
                hospitalInfo.Sort = addDto.Sort;
                hospitalInfo.ThumbPicUrl = addDto.ThumbPicUrl;
                hospitalInfo.Address = addDto.Address;
                hospitalInfo.Longitude = addDto.Longitude;
                hospitalInfo.Latitude = addDto.Latitude;
                hospitalInfo.Phone = addDto.Phone;
                hospitalInfo.Valid = true;
                hospitalInfo.CreateBy = employeeId;
                hospitalInfo.CreateDate = DateTime.Now;
                hospitalInfo.CityId = addDto.CityId;
                hospitalInfo.DueTime = addDto.DueTime;
                hospitalInfo.ContractUrl = addDto.ContractUrl;
                hospitalInfo.BusinessHours = addDto.BusinessHours;
                hospitalInfo.BelongCompany = addDto.BelongCompany;
                hospitalInfo.IsShareInMiniProgram = addDto.IsShareInMiniProgram;
                hospitalInfo.SendOrder = addDto.SendOrder;
                hospitalInfo.NewCustomerCommissionRatio = addDto.NewCustomerCommissionRatio;
                hospitalInfo.OldCustomerCommissionRatio = addDto.OldCustomerCommissionRatio;
                hospitalInfo.RepeatOrderRule = addDto.RepeatOrderRule;
                hospitalInfo.YearServiceFee = addDto.YearServiceFee;
                hospitalInfo.SecurityDeposit = addDto.SecurityDeposit;
                hospitalInfo.YearServiceMoney = addDto.YearServiceMoney;
                hospitalInfo.SecurityDepositMoney = addDto.SecurityDepositMoney;
                await dalHospitalInfo.AddAsync(hospitalInfo, true);

                List<HospitalTagDetail> hospitalTagDetailList = new List<HospitalTagDetail>();
                foreach (var item in addDto.TagIds)
                {
                    HospitalTagDetail hospitalTagDetail = new HospitalTagDetail();
                    hospitalTagDetail.HospitalId = hospitalInfo.Id;
                    hospitalTagDetail.TagId = item;
                    hospitalTagDetailList.Add(hospitalTagDetail);
                }
                await dalHospitalTagDetail.AddCollectionAsync(hospitalTagDetailList, true);

                unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }



        /// <summary>
        /// 根据医院编号获取医院信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HospitalInfoDetailDto> GetByIdAsync(int id)
        {
            try
            {
                var hospitalInfo = from d in dalHospitalInfo.GetAll()
                                   where d.Id == id
                                   select new HospitalInfoDetailDto
                                   {
                                       Id = d.Id,
                                       Name = d.Name,
                                       SimpleName = d.SimpleName,
                                       Sort = d.Sort,
                                       ThumbPicUrl = d.ThumbPicUrl,
                                       Address = d.Address,
                                       Longitude = d.Longitude,
                                       Latitude = d.Latitude,
                                       BelongCompany = d.BelongCompany,
                                       Phone = d.Phone,
                                       Valid = d.Valid,
                                       CityId = d.CityId,
                                       DescriptionPicture = d.DescriptionPicture,
                                       DueTime = d.DueTime,
                                       IndustryHonors = d.IndustryHonors,
                                       Description = d.Description,
                                       ProfileRank = d.ProfileRank,
                                       ContractUrl = d.ContractUrl,
                                       City = d.CooperativeHospitalCity.Name,
                                       BusinessHours = d.BusinessHours,
                                       Area = d.Area,
                                       CheckState = d.CheckState,
                                       CheckStateText = ServiceClass.GetCheckTypeText(d.CheckState),
                                       CheckBy = d.CheckBy,
                                       CheckDate = d.CheckDate,
                                       CheckRemark = d.CheckRemark,
                                       SubmitStateText = ServiceClass.GetSubmitTypeText(d.SubmitState),
                                       SubmitState = d.SubmitState,
                                       IsShareInMiniProgram = d.IsShareInMiniProgram,
                                       HospitalCreateDate = d.HospitalCreateTime,
                                       SendOrder = d.SendOrder,
                                       SendOrderText = d.SendOrder.HasValue ? ServiceClass.GetSendOrderText(d.SendOrder.Value) : null,
                                       NewCustomerCommissionRatio = d.NewCustomerCommissionRatio,
                                       OldCustomerCommissionRatio = d.OldCustomerCommissionRatio,
                                       RepeatOrderRule = d.RepeatOrderRule,
                                       YearServiceFee = d.YearServiceFee,
                                       YearServiceFeeText = d.YearServiceFee.HasValue ? ServiceClass.GetYearServiceFeeOrSecurityDepositText(d.YearServiceFee.Value) : null,
                                       SecurityDeposit = d.SecurityDeposit,
                                       SecurityDepositText = d.SecurityDeposit.HasValue ? ServiceClass.GetYearServiceFeeOrSecurityDepositText(d.SecurityDeposit.Value) : null,
                                       SecurityDepositMoney = d.SecurityDepositMoney,
                                       YearServiceMoney = d.YearServiceMoney,
                                       ScaleTagList = (from t in d.HospitalTagDetailList
                                                       where t.TagInfo.Valid && t.TagInfo.Type == 0
                                                       select new HospitalTagNameDto
                                                       {
                                                           Id = t.TagId,
                                                           Name = t.TagInfo.Name
                                                       }).ToList(),


                                       FacilityTagList = (from t in d.HospitalTagDetailList
                                                          where t.TagInfo.Valid && t.TagInfo.Type == 1
                                                          select new HospitalTagNameDto
                                                          {
                                                              Id = t.TagId,
                                                              Name = t.TagInfo.Name
                                                          }).ToList()
                                   };

                if (await hospitalInfo.CountAsync() == 0)
                    throw new Exception("医院编号错误");

                var result = await hospitalInfo.SingleOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task<HospitalInfoDto> GetBaseByIdAsync(int id)
        {
            try
            {
                var hospital = await dalHospitalInfo.GetAll()
                    .Include(e => e.CreateByAmiyaEmployee)
                    .Include(e => e.UpdateByAmiyaEmployee)
                    .Include(e => e.CooperativeHospitalCity)
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (hospital == null)
                {
                    return null;
                }
                HospitalInfoDto hospitalInfoDto = new HospitalInfoDto();
                hospitalInfoDto.Id = hospital.Id;
                hospitalInfoDto.Name = hospital.Name;
                hospitalInfoDto.ThumbPicUrl = hospital.ThumbPicUrl;
                hospitalInfoDto.Address = hospital.Address;
                hospitalInfoDto.Longitude = hospital.Longitude;
                hospitalInfoDto.Latitude = hospital.Latitude;
                hospitalInfoDto.Phone = hospital.Phone;
                hospitalInfoDto.Valid = hospital.Valid;
                hospitalInfoDto.DueTime = hospital.DueTime;
                hospitalInfoDto.ContractUrl = hospital.ContractUrl;
                hospitalInfoDto.CreateBy = hospital.CreateBy;
                hospitalInfoDto.CreateName = hospital.CreateByAmiyaEmployee.Name;
                hospitalInfoDto.CreateDate = hospital.CreateDate;
                hospitalInfoDto.UpdateBy = hospital.UpdateBy;
                hospitalInfoDto.UpdateName = hospital.UpdateByAmiyaEmployee?.Name;
                hospitalInfoDto.UpdateDate = hospital.UpdateDate;
                hospitalInfoDto.CityId = hospital.CityId;
                hospitalInfoDto.City = hospital.CooperativeHospitalCity != null ? hospital.CooperativeHospitalCity.Name : null;
                return hospitalInfoDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<HospitalInfoDto> GetBaseByNameAsync(string Name)
        {
            try
            {
                var hospital = await dalHospitalInfo.GetAll()
                    .Include(e => e.CreateByAmiyaEmployee)
                    .Include(e => e.UpdateByAmiyaEmployee)
                    .Include(e => e.CooperativeHospitalCity)
                    .SingleOrDefaultAsync(e => e.Name == Name);

                if (hospital == null)
                {
                    return null;
                }
                HospitalInfoDto hospitalInfoDto = new HospitalInfoDto();
                hospitalInfoDto.Id = hospital.Id;
                hospitalInfoDto.Name = hospital.Name;
                hospitalInfoDto.ThumbPicUrl = hospital.ThumbPicUrl;
                hospitalInfoDto.Address = hospital.Address;
                hospitalInfoDto.Longitude = hospital.Longitude;
                hospitalInfoDto.Latitude = hospital.Latitude;
                hospitalInfoDto.DueTime = hospital.DueTime;
                hospitalInfoDto.ContractUrl = hospital.ContractUrl;
                hospitalInfoDto.Phone = hospital.Phone;
                hospitalInfoDto.Valid = hospital.Valid;
                hospitalInfoDto.CreateBy = hospital.CreateBy;
                hospitalInfoDto.CreateName = hospital.CreateByAmiyaEmployee.Name;
                hospitalInfoDto.CreateDate = hospital.CreateDate;
                hospitalInfoDto.UpdateBy = hospital.UpdateBy;
                hospitalInfoDto.UpdateName = hospital.UpdateByAmiyaEmployee?.Name;
                hospitalInfoDto.UpdateDate = hospital.UpdateDate;
                hospitalInfoDto.CityId = hospital.CityId;
                hospitalInfoDto.City = hospital.CooperativeHospitalCity != null ? hospital.CooperativeHospitalCity.Name : null;
                return hospitalInfoDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 修改医院信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateHospitalInfoDto updateDto, int employeeId)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var hospital = await dalHospitalInfo.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);

                if (hospital == null)
                    throw new Exception("医院编号错误");

                hospital.Name = updateDto.Name;
                hospital.ThumbPicUrl = updateDto.ThumbPicUrl;
                hospital.Address = updateDto.Address;
                hospital.Longitude = updateDto.Longitude;
                hospital.Latitude = updateDto.Latitude;
                hospital.Phone = updateDto.Phone;
                hospital.Valid = updateDto.Valid;
                hospital.UpdateBy = employeeId;
                hospital.UpdateDate = DateTime.Now;
                hospital.DueTime = updateDto.DueTime;
                hospital.CityId = updateDto.CityId;
                hospital.BusinessHours = updateDto.BusinessHours;
                hospital.IsShareInMiniProgram = updateDto.IsShareInMiniProgram;
                hospital.BelongCompany = updateDto.BelongCompany;
                hospital.SendOrder = updateDto.SendOrder;
                hospital.NewCustomerCommissionRatio = updateDto.NewCustomerCommissionRatio;
                hospital.OldCustomerCommissionRatio = updateDto.OldCustomerCommissionRatio;
                hospital.RepeatOrderRule = updateDto.RepeatOrderRule;
                hospital.YearServiceFee = updateDto.YearServiceFee;
                hospital.SecurityDeposit = updateDto.SecurityDeposit;
                hospital.YearServiceMoney = updateDto.YearServiceMoney;
                hospital.SecurityDepositMoney = updateDto.SecurityDepositMoney;
                hospital.Sort = updateDto.Sort;
                hospital.SimpleName = updateDto.SimpleName;
                await dalHospitalInfo.UpdateAsync(hospital, true);

                var tagDetail = await dalHospitalTagDetail.GetAll().Where(e => e.HospitalId == updateDto.Id).ToListAsync();

                foreach (var item in updateDto.TagIds)
                {
                    if (!tagDetail.Exists(e => e.TagId == item))
                    {
                        HospitalTagDetail hospitalTagDetail = new HospitalTagDetail();
                        hospitalTagDetail.HospitalId = updateDto.Id;
                        hospitalTagDetail.TagId = item;
                        await dalHospitalTagDetail.AddAsync(hospitalTagDetail, true);
                    }
                }


                foreach (var item in tagDetail)
                {
                    if (Array.IndexOf(updateDto.TagIds, item.TagId) == -1)
                    {
                        await dalHospitalTagDetail.DeleteAsync(item, true);
                    }
                }

                unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 修改医院合同信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task UpdateContractUrlAsync(string contractUrl, int hospitalId, int employeeId)
        {
            try
            {
                var hospital = await dalHospitalInfo.GetAll().SingleOrDefaultAsync(e => e.Id == hospitalId);

                if (hospital == null)
                    throw new Exception("医院编号错误");
                hospital.ContractUrl = contractUrl;
                hospital.UpdateBy = employeeId;
                hospital.UpdateDate = DateTime.Now;
                await dalHospitalInfo.UpdateAsync(hospital, true);

            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 医院端修改医院信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task HospitalUpdateAsync(HospitalUpdateHospitalInfoDto updateDto)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var hospital = await dalHospitalInfo.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);

                if (hospital == null)
                    throw new Exception("医院编号错误");

                hospital.Name = updateDto.Name;
                hospital.ThumbPicUrl = updateDto.ThumbPicUrl;
                hospital.Address = updateDto.Address;
                hospital.HospitalCreateTime = updateDto.HospitalCreateDate;
                hospital.IndustryHonors = updateDto.IndustryHonors;
                hospital.ProfileRank = updateDto.ProfileRank;
                hospital.DescriptionPicture = updateDto.DescriptionPicture;
                hospital.Phone = updateDto.Phone;
                hospital.Description = updateDto.Description;
                hospital.Area = updateDto.Area;
                hospital.SubmitState = Convert.ToInt32(SubmintType.Submited);
                hospital.CheckState = Convert.ToInt32(CheckType.NotChecked);
                hospital.UpdateDate = DateTime.Now;
                await dalHospitalInfo.UpdateAsync(hospital, true);

                var tagDetail = await dalHospitalTagDetail.GetAll().Where(e => e.HospitalId == updateDto.Id).ToListAsync();

                foreach (var item in updateDto.TagIds)
                {
                    if (!tagDetail.Exists(e => e.TagId == item))
                    {
                        HospitalTagDetail hospitalTagDetail = new HospitalTagDetail();
                        hospitalTagDetail.HospitalId = updateDto.Id;
                        hospitalTagDetail.TagId = item;
                        await dalHospitalTagDetail.AddAsync(hospitalTagDetail, true);
                    }
                }


                foreach (var item in tagDetail)
                {
                    if (Array.IndexOf(updateDto.TagIds, item.TagId) == -1)
                    {
                        await dalHospitalTagDetail.DeleteAsync(item, true);
                    }
                }

                if (updateDto.HospitalEnvironmentPicture.Count > 0)
                {

                    foreach (var x in updateDto.HospitalEnvironmentPicture)
                    {
                        //删除原有图片，后期删除阿里云数据(todo;)
                        await _hospitalEnvironmentPictureService.DeleteByHospitalIdAndEnvironmentIdAsync(x.HospitalId, x.HospitalEnvironmentId);
                        //添加图片链接
                        foreach (var z in x.PictureUrl)
                        {
                            hospitalEnvironmentPictureAddDto addpicDto = new hospitalEnvironmentPictureAddDto();
                            addpicDto.HospitalId = x.HospitalId;
                            addpicDto.HospitalEnvironmentId = x.HospitalEnvironmentId;
                            addpicDto.PictureUrl = z;
                            await _hospitalEnvironmentPictureService.AddAsync(addpicDto);
                        }
                    }
                }
                unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var hospital = await dalHospitalInfo.GetAll()
                    .Include(e => e.DocterList)
                    .Include(e => e.HospitalAppointmentNumerList)
                    .Include(e => e.AppointmentInfoList)
                    .Include(e => e.RecommendHospitalInfoList)
                    .Include(e => e.HospitalTagDetailList)
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (hospital == null)
                    throw new Exception("医院编号错误");


                if (hospital.AppointmentInfoList.Count > 0)
                    throw new Exception("删除失败，该医院已有预约数据");

                await dalHospitalInfo.DeleteAsync(hospital, true);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }




        /// <summary>
        /// 获取医院列表（小程序）
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<List<HospitalInfoSimpleDto>> GetSimpleListAsync(string keyword, string city)
        {
            try
            {
                var hospital = from d in dalHospitalInfo.GetAll()
                             .Include(e => e.AppointmentInfoList)
                               where d.Valid && (keyword == null || d.Name.Contains(keyword))
                               && (city == null || d.Address.Contains(city))
                               select new HospitalInfoSimpleDto
                               {
                                   Id = d.Id,
                                   Name = d.Name,
                                   ThumbPicUrl = d.ThumbPicUrl,
                                   Longitude = d.Longitude,
                                   Latitude = d.Latitude,
                                   IsRecommend = GetRecommendHospital(d.RecommendHospitalInfoList) == null ? false : true,
                                   RecommendIndex = GetRecommendHospital(d.RecommendHospitalInfoList),
                                   Address = d.Address,
                                   Phone = d.Phone,
                                   BusinessHours = d.BusinessHours
                               };
                var q = await hospital.ToListAsync();

                List<HospitalInfoSimpleDto> hospitalList = new List<HospitalInfoSimpleDto>();
                hospitalList.AddRange(q.Where(e => e.IsRecommend).OrderBy(e => e.RecommendIndex));
                hospitalList.AddRange(q.Where(e => e.IsRecommend == false));

                return hospitalList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }



        /// <summary>
        /// 获取医院推荐排名
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static int? GetRecommendHospital(List<RecommendHospitalInfo> list)
        {
            try
            {
                DateTime date = DateTime.Now;
                var recommendHospital = list.SingleOrDefault(e => e.Valid && e.StartDate.Date <= date.Date && e.EndDate.Date > date.Date);
                return recommendHospital?.RecommendIndex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }



        /// <summary>
        /// 获取医院详细列表（分页，小程序）
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<WxHospitalInfoDto>> GetListWithPageOfWxAsync(int pageNum, int pageSize, string city, int itemInfoId)
        {
            try
            {
                var hospital = from h in dalHospitalInfo.GetAll()
                               .Include(e => e.RecommendHospitalInfoList)
                               join d in dalHospitalQuotedPriceItemInfo.GetAll() on h.Id equals d.HospitalId
                               where h.Valid
                               && (city == null || h.Address.Contains(city))
                               && d.ItemId == itemInfoId
                               select new WxHospitalInfoDto
                               {
                                   Id = h.Id,
                                   Name = h.Name,
                                   Address = h.Address,
                                   Longitude = h.Longitude,
                                   Latitude = h.Latitude,
                                   Phone = h.Phone,
                                   ThumbPicUrl = h.ThumbPicUrl,
                                   IsRecommend = GetRecommendHospital(h.RecommendHospitalInfoList) == null ? false : true,
                                   RecommendIndex = GetRecommendHospital(h.RecommendHospitalInfoList),
                                   DocterList = (from d in h.DocterList
                                                 select new DocterDto
                                                 {
                                                     Id = d.Id,
                                                     Name = d.Name,
                                                     PicUrl = d.PicUrl,
                                                     Position = d.Position,
                                                     WorkYearNumer = DateTime.Now.Year - d.ObtainEmploymentYear,
                                                     Description = d.Description,
                                                 }).ToList(),

                                   ScaleTagList = (from s in h.HospitalTagDetailList
                                                   where s.TagInfo.Valid && s.TagInfo.Type == 0
                                                   select new HospitalTagNameDto
                                                   {
                                                       Id = s.TagId,
                                                       Name = s.TagInfo.Name
                                                   }).ToList(),

                                   FacilityTagList = (from s in h.HospitalTagDetailList
                                                      where s.TagInfo.Valid && s.TagInfo.Type == 1
                                                      select new HospitalTagNameDto
                                                      {
                                                          Id = s.TagId,
                                                          Name = s.TagInfo.Name
                                                      }).ToList()
                               };


                // hospitalPageInfo.List = await hospital.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                var q = await hospital.ToListAsync();

                List<WxHospitalInfoDto> hospitalList = new List<WxHospitalInfoDto>();
                hospitalList.AddRange(q.Where(e => e.IsRecommend).OrderBy(e => e.RecommendIndex));
                hospitalList.AddRange(q.Where(e => e.IsRecommend == false));

                FxPageInfo<WxHospitalInfoDto> hospitalPageInfo = new FxPageInfo<WxHospitalInfoDto>();
                hospitalPageInfo.TotalCount = await hospital.CountAsync();
                hospitalPageInfo.List = hospitalList.Skip((pageNum - 1) * pageSize).Take(pageSize);
                return hospitalPageInfo;
                //HACK 排序
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 根据标签，名称，城市等筛选获取医院列表（分页）
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="city"></param>
        /// <param name="hospitalName"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<WxHospitalInfoDto>> GetListHosPitalAsync(int pageNum, int pageSize, string city, string hospitalName, List<string> tags)
        {
            try
            {
                var hospital = from h in dalHospitalInfo.GetAll()
                               .Include(e => e.RecommendHospitalInfoList)
                               where h.Valid
                               && (city == null || h.Address.Contains(city))
                               && (hospitalName == null || h.Name.Contains(hospitalName))
                               select new WxHospitalInfoDto
                               {
                                   Id = h.Id,
                                   Name = h.Name,
                                   Address = h.Address,
                                   Longitude = h.Longitude,
                                   Latitude = h.Latitude,
                                   Phone = h.Phone,
                                   ThumbPicUrl = h.ThumbPicUrl,
                                   IsRecommend = GetRecommendHospital(h.RecommendHospitalInfoList) == null ? false : true,
                                   RecommendIndex = GetRecommendHospital(h.RecommendHospitalInfoList),
                                   DocterList = (from d in h.DocterList
                                                 select new DocterDto
                                                 {
                                                     Id = d.Id,
                                                     Name = d.Name,
                                                     PicUrl = d.PicUrl,
                                                     Position = d.Position,
                                                     WorkYearNumer = DateTime.Now.Year - d.ObtainEmploymentYear,
                                                     Description = d.Description,
                                                 }).ToList(),

                                   ScaleTagList = (from s in h.HospitalTagDetailList
                                                   where s.TagInfo.Valid && s.TagInfo.Type == 0
                                                   select new HospitalTagNameDto
                                                   {
                                                       Id = s.TagId,
                                                       Name = s.TagInfo.Name
                                                   }).ToList(),

                                   FacilityTagList = (from s in h.HospitalTagDetailList
                                                      where s.TagInfo.Valid && s.TagInfo.Type == 1
                                                      select new HospitalTagNameDto
                                                      {
                                                          Id = s.TagId,
                                                          Name = s.TagInfo.Name
                                                      }).ToList()
                               };
                var q = await hospital.ToListAsync();

                List<WxHospitalInfoDto> hospitalListResult = new List<WxHospitalInfoDto>();
                List<WxHospitalInfoDto> hospitalList = new List<WxHospitalInfoDto>();
                hospitalList.AddRange(q.Where(e => e.IsRecommend).OrderBy(e => e.RecommendIndex));
                hospitalList.AddRange(q.Where(e => e.IsRecommend == false));
                if (tags.Count > 0)
                {
                    foreach (var x in hospitalList)
                    {
                        bool IsAdd = false;
                        for (int a = 0; a < tags.Count; a++)
                        {
                            if (x.ScaleTagList.Exists(k => k.Id == Convert.ToInt16(tags[a])) || x.FacilityTagList.Exists(o => o.Id == Convert.ToInt16(tags[a])))
                            {
                                IsAdd = true;
                            }
                            else
                            {
                                IsAdd = false;
                                break;
                            }

                        }
                        if (IsAdd == true)
                        {
                            WxHospitalInfoDto hospitalInfoResultDto = new WxHospitalInfoDto();
                            hospitalInfoResultDto.Id = x.Id;
                            hospitalInfoResultDto.Name = x.Name;
                            hospitalInfoResultDto.Address = x.Address;
                            hospitalInfoResultDto.Longitude = x.Longitude;
                            hospitalInfoResultDto.Latitude = x.Latitude;
                            hospitalInfoResultDto.Phone = x.Phone;
                            hospitalInfoResultDto.ThumbPicUrl = x.ThumbPicUrl;
                            hospitalInfoResultDto.IsRecommend = x.IsRecommend;
                            hospitalInfoResultDto.RecommendIndex = x.RecommendIndex;
                            hospitalInfoResultDto.DocterList = x.DocterList;
                            hospitalInfoResultDto.ScaleTagList = x.ScaleTagList;
                            hospitalInfoResultDto.FacilityTagList = x.FacilityTagList;
                            hospitalListResult.Add(hospitalInfoResultDto);
                        }
                    }
                }
                else
                {
                    hospitalListResult = hospitalList;
                }

                FxPageInfo<WxHospitalInfoDto> hospitalPageInfo = new FxPageInfo<WxHospitalInfoDto>();
                hospitalPageInfo.TotalCount = hospitalListResult.Count();
                hospitalPageInfo.List = hospitalListResult.Skip((pageNum - 1) * pageSize).Take(pageSize);
                return hospitalPageInfo;
                //HACK 排序
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 根据商品编号获取参与项目的医院名称列表
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<List<HospitalNameDto>> GetPartakeItemHospitalNameListAsync(string goodsId, string name)
        {
            var itemInfo = await dalItemInfo.GetAll().FirstOrDefaultAsync(e => e.OtherAppItemId == goodsId);
            if (itemInfo == null)
                return new List<HospitalNameDto>();
            var hospital = from d in dalHospitalQuotedPriceItemInfo.GetAll()
                           where d.ItemId == itemInfo.Id
                           && (string.IsNullOrWhiteSpace(name) || d.HospitalInfo.Name.Contains(name))
                           select new HospitalNameDto
                           {
                               Id = d.HospitalId,
                               Name = d.HospitalInfo.Name
                           };

            return await hospital.ToListAsync();
        }

        public async Task<FxPageInfo<WxHospitalInfoDto>> GetWxHospitalNameListAsync(int pageNum, int pageSize, string city, string hospitalName, List<string> tags)
        {
            try
            {
                var hospital = from h in dalHospitalInfo.GetAll()
                               .Include(e => e.RecommendHospitalInfoList)
                               where h.Valid
                               && (city == null || h.Address.Contains(city))
                               && (hospitalName == null || h.Name.Contains(hospitalName))
                               && (h.IsShareInMiniProgram == true)
                               select new WxHospitalInfoDto
                               {
                                   Id = h.Id,
                                   Name = h.Name,
                                   Address = h.Address,
                                   Longitude = h.Longitude,
                                   Latitude = h.Latitude,
                                   Phone = h.Phone,
                                   ThumbPicUrl = h.ThumbPicUrl,
                                   IsRecommend = GetRecommendHospital(h.RecommendHospitalInfoList) == null ? false : true,
                                   RecommendIndex = GetRecommendHospital(h.RecommendHospitalInfoList),
                                   DocterList = (from d in h.DocterList
                                                 select new DocterDto
                                                 {
                                                     Id = d.Id,
                                                     Name = d.Name,
                                                     PicUrl = d.PicUrl,
                                                     Position = d.Position,
                                                     WorkYearNumer = DateTime.Now.Year - d.ObtainEmploymentYear,
                                                     Description = d.Description,
                                                 }).ToList(),

                                   ScaleTagList = (from s in h.HospitalTagDetailList
                                                   where s.TagInfo.Valid && s.TagInfo.Type == 0
                                                   select new HospitalTagNameDto
                                                   {
                                                       Id = s.TagId,
                                                       Name = s.TagInfo.Name
                                                   }).ToList(),

                                   FacilityTagList = (from s in h.HospitalTagDetailList
                                                      where s.TagInfo.Valid && s.TagInfo.Type == 1
                                                      select new HospitalTagNameDto
                                                      {
                                                          Id = s.TagId,
                                                          Name = s.TagInfo.Name
                                                      }).ToList()
                               };
                var q = await hospital.ToListAsync();

                List<WxHospitalInfoDto> hospitalListResult = new List<WxHospitalInfoDto>();
                List<WxHospitalInfoDto> hospitalList = new List<WxHospitalInfoDto>();
                hospitalList.AddRange(q.Where(e => e.IsRecommend).OrderBy(e => e.RecommendIndex));
                hospitalList.AddRange(q.Where(e => e.IsRecommend == false));
                if (tags.Count > 0)
                {
                    foreach (var x in hospitalList)
                    {
                        bool IsAdd = false;
                        for (int a = 0; a < tags.Count; a++)
                        {
                            if (x.ScaleTagList.Exists(k => k.Id == Convert.ToInt16(tags[a])) || x.FacilityTagList.Exists(o => o.Id == Convert.ToInt16(tags[a])))
                            {
                                IsAdd = true;
                            }
                            else
                            {
                                IsAdd = false;
                                break;
                            }

                        }
                        if (IsAdd == true)
                        {
                            WxHospitalInfoDto hospitalInfoResultDto = new WxHospitalInfoDto();
                            hospitalInfoResultDto.Id = x.Id;
                            hospitalInfoResultDto.Name = x.Name;
                            hospitalInfoResultDto.Address = x.Address;
                            hospitalInfoResultDto.Longitude = x.Longitude;
                            hospitalInfoResultDto.Latitude = x.Latitude;
                            hospitalInfoResultDto.Phone = x.Phone;
                            hospitalInfoResultDto.ThumbPicUrl = x.ThumbPicUrl;
                            hospitalInfoResultDto.IsRecommend = x.IsRecommend;
                            hospitalInfoResultDto.RecommendIndex = x.RecommendIndex;
                            hospitalInfoResultDto.DocterList = x.DocterList;
                            hospitalInfoResultDto.ScaleTagList = x.ScaleTagList;
                            hospitalInfoResultDto.FacilityTagList = x.FacilityTagList;
                            hospitalListResult.Add(hospitalInfoResultDto);
                        }
                    }
                }
                else
                {
                    hospitalListResult = hospitalList;
                }

                FxPageInfo<WxHospitalInfoDto> hospitalPageInfo = new FxPageInfo<WxHospitalInfoDto>();
                hospitalPageInfo.TotalCount = hospitalListResult.Count();
                hospitalPageInfo.List = hospitalListResult.Skip((pageNum - 1) * pageSize).Take(pageSize);
                return hospitalPageInfo;
                //HACK 排序
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 获取预约叫车预约医院名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HospitalNameDto>> GetWxAppointCarHospitalNameList()
        {
            var list = dalHospitalInfo.GetAll().Where(e => e.Valid == true && e.IsShareInMiniProgram == true).Select(e => new HospitalNameDto
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return list;
        }
        /// <summary>
        /// 根据归属公司获取医院名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HospitalNameDto>> GetHospitalNameListByCompany(string companyId)
        {
            return dalHospitalInfo.GetAll().Where(e => e.BelongCompany == companyId).Select(e => new HospitalNameDto
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
        }

        public async Task<List<BaseKeyValueDto<int>>> GetSendOrderListAsync()
        {
            var showDirectionTypes = Enum.GetValues(typeof(SendOrder));
            List<BaseKeyValueDto<int>> requestTypeList = new List<BaseKeyValueDto<int>>();
            foreach (var item in showDirectionTypes)
            {
                BaseKeyValueDto<int> requestType = new BaseKeyValueDto<int>();
                requestType.Key = Convert.ToInt32(item);
                requestType.Value = ServiceClass.GetSendOrderText(Convert.ToInt32(item));
                requestTypeList.Add(requestType);
            }
            return requestTypeList;
        }

        public async Task<List<BaseKeyValueDto<int>>> GetYearServiceStatusAsync()
        {
            var showDirectionTypes = Enum.GetValues(typeof(YearServiceFeeOrSecurityDeposit));
            List<BaseKeyValueDto<int>> requestTypeList = new List<BaseKeyValueDto<int>>();
            foreach (var item in showDirectionTypes)
            {
                BaseKeyValueDto<int> requestType = new BaseKeyValueDto<int>();
                requestType.Key = Convert.ToInt32(item);
                requestType.Value = ServiceClass.GetYearServiceFeeOrSecurityDepositText(Convert.ToInt32(item));
                requestTypeList.Add(requestType);
            }
            return requestTypeList;
        }
        /// <summary>
        /// 根据时间获取医院派单数
        /// </summary>
        /// <returns></returns>
        public async Task<List<ActiveHospitalInfoDto>> GetActiveHospitalListByTimeAsync(QueryActiveHospitalDto query)
        {
            return await dalContentPlatformOrderSend.GetAll().Where(e => e.SendDate > query.StartDate && e.SendDate < query.EndDate.AddDays(1).Date).GroupBy(e => e.HospitalId).Select(e => new ActiveHospitalInfoDto
            {
                HospitalId = e.Key,
                HospitalName = dalHospitalInfo.GetAll().Where(h => h.Id == e.Key).Single().Name,
                SendOrderCount = e.Count()
            }).ToListAsync();
        }

        #region 【新的医院合同接口】

        public async Task UpdateContractAsync(UpdateHospitalContractDto updateDto)
        {
            var contract = await dalHospitalContract.GetAll().Where(e => e.Id == updateDto.Id).FirstOrDefaultAsync();
            if (contract == null) throw new Exception("合同编号错误!");
            contract.Name = updateDto.Name;
            contract.ContractUrl = updateDto.ContractUrl;
            contract.StartDate = updateDto.StartDate;
            contract.ExpireDate = updateDto.ExpireDate;
            contract.UpdateDate = DateTime.Now;
            await dalHospitalContract.UpdateAsync(contract, true);
        }

        public async Task AddHospitalContractAsync(AddHospitalContractDto addDto)
        {
            HospitalContract contract = new HospitalContract();
            contract.Id = Guid.NewGuid().ToString().Replace("-", "");
            contract.Name = addDto.Name;
            contract.HospitalId = addDto.HospitalId;
            contract.ContractUrl = addDto.ContractUrl;
            contract.StartDate = addDto.StartDate;
            contract.ExpireDate = addDto.ExpireDate;
            contract.CreateDate = DateTime.Now;
            contract.Valid = true;
            await dalHospitalContract.AddAsync(contract, true);
        }

        public async Task DeleteHospitalContractAsync(string id)
        {
            var contract = await dalHospitalContract.GetAll().Where(e => e.Id == id).FirstOrDefaultAsync();
            if (contract == null) throw new Exception("合同编号错误!");
            contract.Valid = false;
            await dalHospitalContract.UpdateAsync(contract, true);
        }

        public async Task<List<HospitalContractInfoDto>> GetHospitalContractListAsync(int hospitalId)
        {
            return await dalHospitalContract.GetAll()
                .Where(e => e.HospitalId == hospitalId && e.Valid == true)
                .Select(e => new HospitalContractInfoDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ContractUrl = e.ContractUrl,
                    StartDate = e.StartDate,
                    ExpireDate = e.ExpireDate,
                }).ToListAsync();
        }


        public async Task<HospitalContractInfoDto> GetHospitalContractByIdAsync(string id)
        {
            HospitalContractInfoDto hospitalContractInfoDto = new HospitalContractInfoDto();
            var contract = dalHospitalContract.GetAll()
               .Where(e => e.Id == id).FirstOrDefault();
            if (contract == null) throw new Exception("合同编号错误!");
            hospitalContractInfoDto.Id = contract.Id;
            hospitalContractInfoDto.Name = contract.Name;
            hospitalContractInfoDto.ContractUrl = contract.ContractUrl;
            hospitalContractInfoDto.StartDate = contract.StartDate;
            hospitalContractInfoDto.ExpireDate = contract.ExpireDate;
            return hospitalContractInfoDto;
        }

        #endregion

        #region 【新的医院项目ppt接口】

        public async Task UpdateProjectAsync(UpdateHospitalProjectDto updateDto)
        {
            var project = await dalHospitalProject.GetAll().Where(e => e.Id == updateDto.Id).FirstOrDefaultAsync();
            if (project == null) throw new Exception("医院项目ppt编号错误!");
            project.Name = updateDto.Name;
            project.ProjectUrl = updateDto.ProjectUrl;
            project.UpdateDate = DateTime.Now;
            await dalHospitalProject.UpdateAsync(project, true);
        }

        public async Task AddHospitalProjectAsync(AddHospitalProjectDto addDto)
        {
            HospitalProject project = new HospitalProject();
            project.Id = Guid.NewGuid().ToString().Replace("-", "");
            project.Name = addDto.Name;
            project.HospitalId = addDto.HospitalId;
            project.ProjectUrl = addDto.ProjectUrl;
            project.CreateDate = DateTime.Now;
            project.Valid = true;
            await dalHospitalProject.AddAsync(project, true);
        }

        public async Task DeleteHospitalProjectAsync(string id)
        {
            var project = await dalHospitalProject.GetAll().Where(e => e.Id == id).FirstOrDefaultAsync();
            if (project == null) throw new Exception("医院项目ppt编号错误!");
            project.Valid = false;
            await dalHospitalProject.UpdateAsync(project, true);
        }

        public async Task<List<HospitalProjectInfoDto>> GetHospitalProjectListAsync(int hospitalId)
        {
            return await dalHospitalProject.GetAll()
                .Where(e => e.HospitalId == hospitalId && e.Valid == true)
                .Select(e => new HospitalProjectInfoDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ProjectUrl = e.ProjectUrl,
                }).ToListAsync();
        }


        public async Task<HospitalProjectInfoDto> GetHospitalProjectByIdAsync(string id)
        {
            HospitalProjectInfoDto hospitalProjectInfoDto = new HospitalProjectInfoDto();
            var project = dalHospitalProject.GetAll()
               .Where(e => e.Id == id).FirstOrDefault();
            if (project == null) throw new Exception("医院项目ppt编号错误!");
            hospitalProjectInfoDto.Id = project.Id;
            hospitalProjectInfoDto.Name = project.Name;
            hospitalProjectInfoDto.ProjectUrl = project.ProjectUrl;
            return hospitalProjectInfoDto;
        }



        #endregion

    }
}
