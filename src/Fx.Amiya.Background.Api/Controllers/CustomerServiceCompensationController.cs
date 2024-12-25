using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Vo;
using Fx.Amiya.Background.Api.Vo.CustomerServiceCompensation.Input;
using Fx.Amiya.Background.Api.Vo.CustomerServiceCompensation.Result;
using Fx.Amiya.Dto.CustomerServiceCompensation;
using Fx.Amiya.Dto.CustomerServiceCompensation.Input;
using Fx.Amiya.Dto.OperationLog;
using Fx.Amiya.IService;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Infrastructure;
using Fx.Open.Infrastructure.Web;
using Jd.Api.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 助理薪资单
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CustomerServiceCompensationController : ControllerBase
    {
        private ICustomerServiceCompensationService customerServiceCompensationService;
        private IHttpContextAccessor _httpContextAccessor;
        private IOperationLogService operationLogService;

        public CustomerServiceCompensationController(IHttpContextAccessor httpContextAccessor, ICustomerServiceCompensationService customerServiceCompensationService, IOperationLogService operationLogService)
        {
            this.customerServiceCompensationService = customerServiceCompensationService;
            _httpContextAccessor = httpContextAccessor;
            this.operationLogService = operationLogService;
        }



        /// <summary>
        /// 根据条件获取助理薪资单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        [FxInternalAuthorize]
        public async Task<ResultData<FxPageInfo<CustomerServiceCompensationVo>>> GetListWithPageAsync([FromQuery] QueryCustomerServiceCompensationVo query)
        {
            try
            {
                QueryCustomerServiceCompensationDto queryDto = new QueryCustomerServiceCompensationDto();
                queryDto.StartDate = query.StartDate;
                queryDto.EndDate = query.EndDate;
                queryDto.BelongEmpId = query.BelongEmpId;
                queryDto.Valid = query.Valid;
                queryDto.KeyWord = query.KeyWord;
                queryDto.PageNum = query.PageNum;
                queryDto.PageSize = query.PageSize;
                var q = await customerServiceCompensationService.GetListAsync(queryDto);
                var customerServiceCompensation = from d in q.List
                                                  select new CustomerServiceCompensationVo
                                                  {
                                                      Id = d.Id,
                                                      CreateDate = d.CreateDate,
                                                      CreateBy = d.CreateBy,
                                                      CreateByEmpName = d.CreateByEmpName,
                                                      UpdateDate = d.UpdateDate,
                                                      Valid = d.Valid,
                                                      DeleteDate = d.DeleteDate,
                                                      Name = d.Name,
                                                      BelongEmpId = d.BelongEmpId,
                                                      BelongEmpName = d.BelongEmpName,
                                                      TotalPrice = d.TotalPrice,
                                                      OtherPrice = d.OtherPrice,
                                                      Remark = d.Remark,
                                                      Salary = d.Salary,
                                                      CustomerServicePerformance = d.CustomerServicePerformance,
                                                      ToHospitalRate = d.ToHospitalRate,
                                                      ToHospitalRateReword = d.ToHospitalRateReword,
                                                      OldTakeNewCustomerPrice = d.OldTakeNewCustomerPrice,
                                                      RepeatPurchasesRate = d.RepeatPurchasesRate,
                                                      RepeatPurchasesRateReword = d.RepeatPurchasesRateReword,
                                                      NewCustomerToHospitalReword = d.NewCustomerToHospitalReword,
                                                      OldCustomerToHospitalReword = d.OldCustomerToHospitalReword,
                                                      TargetFinishReword = d.TargetFinishReword,
                                                      OtherChargebacks = d.OtherChargebacks,
                                                      AddClueCompletePrice = d.AddClueCompletePrice,
                                                      AddWechatCompletePrice = d.AddWechatCompletePrice,
                                                      BeautyAddWechatPrice = d.BeautyAddWechatPrice,
                                                      TakeGoodsAddWechatPrice = d.TakeGoodsAddWechatPrice,
                                                      ConsulationCardPrice = d.ConsulationCardPrice,
                                                      ConsulationCardAddWechatPrice = d.ConsulationCardAddWechatPrice,
                                                      CooperationLiveAnchorToHospitalPrice = d.CooperationLiveAnchorToHospitalPrice,
                                                      CooperationLiveAnchorSendOrderPrice = d.CooperationLiveAnchorSendOrderPrice,
                                                      SpecialHospitalVisitPrice = d.SpecialHospitalVisitPrice,
                                                      Verison = d.Verison
                                                  };

                FxPageInfo<CustomerServiceCompensationVo> pageInfo = new FxPageInfo<CustomerServiceCompensationVo>();
                pageInfo.TotalCount = q.TotalCount;
                pageInfo.List = customerServiceCompensation;

                return ResultData<FxPageInfo<CustomerServiceCompensationVo>>.Success().AddData("customerServiceCompensation", pageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<CustomerServiceCompensationVo>>.Fail(ex.Message);
            }
        }




        /// <summary>
        /// 添加助理薪资单
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [FxInternalAuthorize]
        public async Task<ResultData> AddAsync(AddCustomerServiceCompensationVo addVo)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                AddCustomerServiceCompensationDto addDto = new AddCustomerServiceCompensationDto();
                addDto.Name = addVo.Name;
                addDto.BelongEmpId = addVo.BelongEmpId;
                addDto.TotalPrice = addVo.TotalPrice;
                addDto.OtherPrice = addVo.OtherPrice;
                addDto.Remark = addVo.Remark;
                addDto.RecommandDocumentSettleIdList = addVo.RecommandDocumentSettleIdList;
                addDto.CreateBy = employeeId;
                addDto.Salary = addVo.Salary;
                //addDto.PerformancePercent = addVo.PerformancePercent;
                addDto.CustomerServicePerformance = addVo.CustomerServicePerformance;
                addDto.ToHospitalRate = addVo.ToHospitalRate;
                addDto.ToHospitalRateReword = addVo.ToHospitalRateReword;
                addDto.RepeatPurchasesRate = addVo.RepeatPurchasesRate;
                addDto.RepeatPurchasesRateReword = addVo.RepeatPurchasesRateReword;
                addDto.NewCustomerToHospitalReword = addVo.NewCustomerToHospitalReword;
                addDto.OldCustomerToHospitalReword = addVo.OldCustomerToHospitalReword;
                addDto.TargetFinishReword = addVo.TargetFinishReword;
                addDto.OtherChargebacks = addVo.OtherChargebacks;
                addDto.OldTakeNewCustomerPrice = addVo.OldTakeNewCustomerPrice;

                addDto.AddClueCompletePrice = addVo.AddClueCompletePrice;
                addDto.AddWechatCompletePrice = addVo.AddWechatCompletePrice;
                addDto.BeautyAddWechatPrice = addVo.BeautyAddWechatPrice;
                addDto.TakeGoodsAddWechatPrice = addVo.TakeGoodsAddWechatPrice;
                addDto.ConsulationCardPrice = addVo.ConsulationCardPrice;
                addDto.ConsulationCardAddWechatPrice = addVo.ConsulationCardAddWechatPrice;
                addDto.CooperationLiveAnchorToHospitalPrice = addVo.CooperationLiveAnchorToHospitalPrice;
                addDto.CooperationLiveAnchorSendOrderPrice = addVo.CooperationLiveAnchorSendOrderPrice;
                addDto.SpecialHospitalVisitPrice = addVo.SpecialHospitalVisitPrice;
                addDto.Verison = addVo.Verison;
                await customerServiceCompensationService.AddAsync(addDto);

                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 根据助理薪资单编号获取助理薪资单信息
        /// </summary>
        /// <param name="id">助理薪资单编号</param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData<CustomerServiceCompensationVo>> GetByIdAsync(string id)
        {
            try
            {
                var customerServiceCompensation = await customerServiceCompensationService.GetByIdAsync(id);
                CustomerServiceCompensationVo customerServiceCompensationVo = new CustomerServiceCompensationVo();
                customerServiceCompensationVo.Id = customerServiceCompensation.Id;
                customerServiceCompensationVo.CreateDate = customerServiceCompensation.CreateDate;
                customerServiceCompensationVo.CreateBy = customerServiceCompensation.CreateBy;
                customerServiceCompensationVo.CreateByEmpName = customerServiceCompensation.CreateByEmpName;
                customerServiceCompensationVo.UpdateDate = customerServiceCompensation.UpdateDate;
                customerServiceCompensationVo.Valid = customerServiceCompensation.Valid;
                customerServiceCompensationVo.DeleteDate = customerServiceCompensation.DeleteDate;
                customerServiceCompensationVo.Name = customerServiceCompensation.Name;
                customerServiceCompensationVo.BelongEmpId = customerServiceCompensation.BelongEmpId;
                customerServiceCompensationVo.BelongEmpName = customerServiceCompensation.BelongEmpName;
                customerServiceCompensationVo.TotalPrice = customerServiceCompensation.TotalPrice;
                customerServiceCompensationVo.OtherPrice = customerServiceCompensation.OtherPrice;
                customerServiceCompensationVo.Remark = customerServiceCompensation.Remark;
                customerServiceCompensationVo.Salary = customerServiceCompensation.Salary;
                customerServiceCompensationVo.OldTakeNewCustomerPrice = customerServiceCompensation.OldTakeNewCustomerPrice;
                //customerServiceCompensationVo.PerformancePercent = customerServiceCompensation.PerformancePercent;
                customerServiceCompensationVo.CustomerServicePerformance = customerServiceCompensation.CustomerServicePerformance;
                customerServiceCompensationVo.ToHospitalRate = customerServiceCompensation.ToHospitalRate;
                customerServiceCompensationVo.ToHospitalRateReword = customerServiceCompensation.ToHospitalRateReword;
                customerServiceCompensationVo.RepeatPurchasesRate = customerServiceCompensation.RepeatPurchasesRate;
                customerServiceCompensationVo.RepeatPurchasesRateReword = customerServiceCompensation.RepeatPurchasesRateReword;
                customerServiceCompensationVo.NewCustomerToHospitalReword = customerServiceCompensation.NewCustomerToHospitalReword;
                customerServiceCompensationVo.OldCustomerToHospitalReword = customerServiceCompensation.OldCustomerToHospitalReword;
                customerServiceCompensationVo.TargetFinishReword = customerServiceCompensation.TargetFinishReword;
                customerServiceCompensationVo.OtherChargebacks = customerServiceCompensation.OtherChargebacks;


                customerServiceCompensationVo.AddClueCompletePrice = customerServiceCompensation.AddClueCompletePrice;
                customerServiceCompensationVo.AddWechatCompletePrice = customerServiceCompensation.AddWechatCompletePrice;
                customerServiceCompensationVo.BeautyAddWechatPrice = customerServiceCompensation.BeautyAddWechatPrice;
                customerServiceCompensationVo.TakeGoodsAddWechatPrice = customerServiceCompensation.TakeGoodsAddWechatPrice;
                customerServiceCompensationVo.ConsulationCardPrice = customerServiceCompensation.ConsulationCardPrice;
                customerServiceCompensationVo.ConsulationCardAddWechatPrice = customerServiceCompensation.ConsulationCardAddWechatPrice;
                customerServiceCompensationVo.CooperationLiveAnchorToHospitalPrice = customerServiceCompensation.CooperationLiveAnchorToHospitalPrice;
                customerServiceCompensationVo.CooperationLiveAnchorSendOrderPrice = customerServiceCompensation.CooperationLiveAnchorSendOrderPrice;
                customerServiceCompensationVo.SpecialHospitalVisitPrice = customerServiceCompensation.SpecialHospitalVisitPrice;
                customerServiceCompensationVo.Verison = customerServiceCompensation.Verison;
                return ResultData<CustomerServiceCompensationVo>.Success().AddData("customerServiceCompensation", customerServiceCompensationVo);
            }
            catch (Exception ex)
            {
                return ResultData<CustomerServiceCompensationVo>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 修改助理薪资单信息
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        [FxInternalAuthorize]
        public async Task<ResultData> UpdateAsync(UpdateCustomerServiceCompensationVo updateVo)
        {
            try
            {
                UpdateCustomerServiceCompensationDto updateDto = new UpdateCustomerServiceCompensationDto();
                updateDto.Id = updateVo.Id;
                updateDto.Name = updateVo.Name;
                updateDto.BelongEmpId = updateVo.BelongEmpId;
                updateDto.TotalPrice = updateVo.TotalPrice;
                updateDto.OtherPrice = updateVo.OtherPrice;
                updateDto.Remark = updateVo.Remark;
                updateDto.Salary = updateVo.Salary;
                //updateDto.PerformancePercent = updateVo.PerformancePercent;
                updateDto.CustomerServicePerformance = updateVo.CustomerServicePerformance;
                updateDto.ToHospitalRate = updateVo.ToHospitalRate;
                updateDto.ToHospitalRateReword = updateVo.ToHospitalRateReword;
                updateDto.RepeatPurchasesRate = updateVo.RepeatPurchasesRate;
                updateDto.RepeatPurchasesRateReword = updateVo.RepeatPurchasesRateReword;
                updateDto.NewCustomerToHospitalReword = updateVo.NewCustomerToHospitalReword;
                updateDto.OldCustomerToHospitalReword = updateVo.OldCustomerToHospitalReword;
                updateDto.TargetFinishReword = updateVo.TargetFinishReword;
                updateDto.OtherChargebacks = updateVo.OtherChargebacks;
                updateDto.OldTakeNewCustomerPrice = updateVo.OldTakeNewCustomerPrice;

                updateDto.AddClueCompletePrice = updateVo.AddClueCompletePrice;
                updateDto.AddWechatCompletePrice = updateVo.AddWechatCompletePrice;
                updateDto.BeautyAddWechatPrice = updateVo.BeautyAddWechatPrice;
                updateDto.TakeGoodsAddWechatPrice = updateVo.TakeGoodsAddWechatPrice;
                updateDto.ConsulationCardPrice = updateVo.ConsulationCardPrice;
                updateDto.ConsulationCardAddWechatPrice = updateVo.ConsulationCardAddWechatPrice;
                updateDto.CooperationLiveAnchorToHospitalPrice = updateVo.CooperationLiveAnchorToHospitalPrice;
                updateDto.CooperationLiveAnchorSendOrderPrice = updateVo.CooperationLiveAnchorSendOrderPrice;
                updateDto.SpecialHospitalVisitPrice = updateVo.SpecialHospitalVisitPrice;
                updateDto.Verison = updateVo.Verison;
                await customerServiceCompensationService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 复制助理薪资单
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        [FxInternalAuthorize]
        [HttpPost("copyCompensation")]
        public async Task<ResultData> CopyCompensation(CopyCompensationVo copy)
        {
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            await customerServiceCompensationService.CopyAsync(copy.Id, employeeId);
            return ResultData.Success();
        }
        /// <summary>
        /// 作废助理薪资单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData> DeleteAsync(string id)
        {
            try
            {

                await customerServiceCompensationService.DeleteAsync(id);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 助理业绩查询
        /// </summary>
        /// <param name="queryVo"></param>
        /// <returns></returns>
        [HttpGet("getAssistantDealInfoList")]
        public async Task<ResultData<FxPageInfo<DealInfoListVo>>> GetAssistantDealInfoListAsync([FromQuery] QueryDealInfoVo queryVo)
        {
            FxPageInfo<DealInfoListVo> page = new FxPageInfo<DealInfoListVo>();
            QueryDealInfoDto queryDto = new QueryDealInfoDto();
            queryDto.StartDate = queryVo.StartDate;
            queryDto.EndDate = queryVo.EndDate;
            queryDto.CreateBy = queryVo.CreateBy;
            queryDto.BelongEmpId = queryVo.BelongEmpId;
            queryDto.PerformanceType = queryVo.PerformanceType;
            queryDto.Keyword = queryVo.KeyWord;
            queryDto.PageSize = queryVo.PageSize;
            queryDto.PageNum = queryVo.PageNum;
            var res = await customerServiceCompensationService.GetDealInfoListAsync(queryDto);
            page.TotalCount = res.TotalCount;
            page.List = res.List.Select(e => new DealInfoListVo
            {
                DealId = e.DealId,
                ContentPaltformOrderId = e.ContentPaltformOrderId,
                DealPrice = e.DealPrice,
                PerformanceType = e.PerformanceType,
                PerformanceTypeText = e.PerformanceTypeText,
                CreateById = e.CreateById,
                CreateByName = e.CreateByName,
                CreateDate = e.CreateDate,
                IsDeal = e.IsDeal,
                IsSupportOrder = e.IsSupportOrder,
                BelongEmpId = e.BelongEmpId,
                BelongEmpName = e.BelongEmpName,
                SupportEmpId = e.SupportEmpId,
                SupportEmpName = e.SupportEmpName,
                IsCheckPerformance = e.IsCheckPerformance,
                BeforeReplenishmentPrice = e.BeforeReplenishmentPrice,
                BeforeReplenishmentIsCreateBill = e.BeforeReplenishmentIsCreateBill,
                ConfirmDealPrice = e.ConfirmDealPrice,
            }).ToList();
            return ResultData<FxPageInfo<DealInfoListVo>>.Success().AddData("data", page);
        }

        #region【枚举下拉框】

        /// <summary>
        /// 薪资业绩类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("PerformanceType")]
        public async Task<ResultData<List<BaseIdAndNameVo<int>>>> GetCustomerSourceListAsync()
        {
            var nameList = customerServiceCompensationService.GetPerformanceTypeText();
            var result = nameList.Select(e => new BaseIdAndNameVo<int>
            {
                Id = e.Key,
                Name = e.Value
            }).ToList();
            return ResultData<List<BaseIdAndNameVo<int>>>.Success().AddData("PerformanceTypeList", result);

        }
        #endregion
    }
}