using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.CustomerServiceCompensation.Input;
using Fx.Amiya.Dto.CustomerServiceCompensation.Result;
using Fx.Amiya.Dto.FinancialBoard;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Fx.Common.Extensions;
using Fx.Infrastructure.DataAccess;
using jos_sdk_net.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class CustomerServiceCompensationService : ICustomerServiceCompensationService
    {
        private readonly IDalCustomerServiceCompensation dalCustomerServiceCompensation;
        private readonly IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo;
        private readonly IDalAmiyaEmployee dalAmiyaEmployee;
        private IRecommandDocumentSettleService recommandDocumentSettleService;
        private ICustomerServiceCheckPerformanceService customerServiceCheckPerformanceService;

        private readonly IUnitOfWork unitOfWork;
        public CustomerServiceCompensationService(IDalCustomerServiceCompensation dalCustomerServiceCompensation,
            IRecommandDocumentSettleService recommandDocumentSettleService,
            IUnitOfWork unitOfWork, IDalContentPlatFormOrderDealInfo dalContentPlatFormOrderDealInfo, IDalAmiyaEmployee dalAmiyaEmployee,
            ICustomerServiceCheckPerformanceService customerServiceCheckPerformanceService)
        {
            this.dalCustomerServiceCompensation = dalCustomerServiceCompensation;
            this.recommandDocumentSettleService = recommandDocumentSettleService;
            this.unitOfWork = unitOfWork;
            this.dalContentPlatFormOrderDealInfo = dalContentPlatFormOrderDealInfo;
            this.dalAmiyaEmployee = dalAmiyaEmployee;
            this.customerServiceCheckPerformanceService = customerServiceCheckPerformanceService;
        }



        /// <summary>
        /// 根据条件获取助理薪资单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<CustomerServiceCompensationDto>> GetListAsync(QueryCustomerServiceCompensationDto query)
        {
            var customerServiceCompensations = from d in dalCustomerServiceCompensation.GetAll().Include(x => x.CreateByEmployee).Include(x => x.BelongEmployee)
                                               where (string.IsNullOrWhiteSpace(query.KeyWord) || d.Name.Contains(query.KeyWord) || d.Remark.Contains(query.KeyWord))
                                               && (!query.BelongEmpId.HasValue || d.BelongEmpId == query.BelongEmpId.Value)
                                               && (!query.Valid.HasValue || d.Valid == query.Valid.Value)
                                               && (!query.StartDate.HasValue || d.CreateDate >= query.StartDate.Value)
                                               && (!query.EndDate.HasValue || d.CreateDate < query.EndDate.Value.AddDays(1).AddMilliseconds(-1))
                                               select new CustomerServiceCompensationDto
                                               {
                                                   Id = d.Id,
                                                   CreateDate = d.CreateDate,
                                                   CreateBy = d.CreateBy,
                                                   CreateByEmpName = d.CreateByEmployee.Name,
                                                   UpdateDate = d.UpdateDate,
                                                   Valid = d.Valid,
                                                   DeleteDate = d.DeleteDate,
                                                   Name = d.Name,
                                                   BelongEmpId = d.BelongEmpId,
                                                   BelongEmpName = d.BelongEmployee.Name,
                                                   TotalPrice = d.TotalPrice,
                                                   OtherPrice = d.OtherPrice,
                                                   Remark = d.Remark,
                                                   Salary = d.Salary,
                                                   CustomerServicePerformance = d.CustomerServicePerformance,
                                                   ToHospitalRate = d.ToHospitalRate,
                                                   OldTakeNewCustomerPrice = d.OldTakeNewCustomerPrice,
                                                   ToHospitalRateReword = d.ToHospitalRateReword,
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
            FxPageInfo<CustomerServiceCompensationDto> customerServiceCompensationPageInfo = new FxPageInfo<CustomerServiceCompensationDto>();
            customerServiceCompensationPageInfo.TotalCount = await customerServiceCompensations.CountAsync();
            customerServiceCompensationPageInfo.List = await customerServiceCompensations.OrderByDescending(x => x.CreateDate).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToListAsync();
            return customerServiceCompensationPageInfo;
        }


        /// <summary>
        /// 添加助理薪资单
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddCustomerServiceCompensationDto addDto)
        {
            unitOfWork.BeginTransaction();
            try
            {
                CustomerServiceCompensation customerServiceCompensation = new CustomerServiceCompensation();
                customerServiceCompensation.Id = Guid.NewGuid().ToString();
                customerServiceCompensation.CreateDate = DateTime.Now;
                customerServiceCompensation.CreateBy = addDto.CreateBy;
                customerServiceCompensation.Name = addDto.Name;
                customerServiceCompensation.Valid = true;
                customerServiceCompensation.BelongEmpId = addDto.BelongEmpId;
                customerServiceCompensation.TotalPrice = addDto.TotalPrice;
                customerServiceCompensation.OtherPrice = addDto.OtherPrice;
                customerServiceCompensation.Remark = addDto.Remark;
                customerServiceCompensation.Salary = addDto.Salary;
                //customerServiceCompensation.PerformancePercent = addDto.PerformancePercent;
                customerServiceCompensation.CustomerServicePerformance = addDto.CustomerServicePerformance;
                customerServiceCompensation.ToHospitalRate = addDto.ToHospitalRate;
                customerServiceCompensation.ToHospitalRateReword = addDto.ToHospitalRateReword;
                customerServiceCompensation.RepeatPurchasesRate = addDto.RepeatPurchasesRate;
                customerServiceCompensation.RepeatPurchasesRateReword = addDto.RepeatPurchasesRateReword;
                customerServiceCompensation.NewCustomerToHospitalReword = addDto.NewCustomerToHospitalReword;
                customerServiceCompensation.OldCustomerToHospitalReword = addDto.OldCustomerToHospitalReword;
                customerServiceCompensation.TargetFinishReword = addDto.TargetFinishReword;
                customerServiceCompensation.OtherChargebacks = addDto.OtherChargebacks;
                customerServiceCompensation.OldTakeNewCustomerPrice = addDto.OldTakeNewCustomerPrice;

                customerServiceCompensation.AddClueCompletePrice = addDto.AddClueCompletePrice;
                customerServiceCompensation.AddWechatCompletePrice = addDto.AddWechatCompletePrice;
                customerServiceCompensation.BeautyAddWechatPrice = addDto.BeautyAddWechatPrice;
                customerServiceCompensation.TakeGoodsAddWechatPrice = addDto.TakeGoodsAddWechatPrice;
                customerServiceCompensation.ConsulationCardPrice = addDto.ConsulationCardPrice;
                customerServiceCompensation.ConsulationCardAddWechatPrice = addDto.ConsulationCardAddWechatPrice;
                customerServiceCompensation.CooperationLiveAnchorToHospitalPrice = addDto.CooperationLiveAnchorToHospitalPrice;
                customerServiceCompensation.CooperationLiveAnchorSendOrderPrice = addDto.CooperationLiveAnchorSendOrderPrice;

                customerServiceCompensation.SpecialHospitalVisitPrice = addDto.SpecialHospitalVisitPrice;
                customerServiceCompensation.Verison = addDto.Verison;

                await dalCustomerServiceCompensation.AddAsync(customerServiceCompensation, true);
                if (addDto.Verison == "1.0")
                {
                    //对账单审核记录加入id
                    await recommandDocumentSettleService.AddCustomerServiceCompensationIdAsync(addDto.RecommandDocumentSettleIdList, customerServiceCompensation.Id, addDto.BelongEmpId);
                }
                if (addDto.Verison == "2.0")
                {
                    //助理新版本提取业绩加入id
                    await customerServiceCheckPerformanceService.AddCustomerServiceCompensationIdAsync(addDto.RecommandDocumentSettleIdList, customerServiceCompensation.Id, addDto.BelongEmpId);
                }
                unitOfWork.Commit();
            }
            catch (Exception err)
            {
                unitOfWork.RollBack();
                throw new Exception(err.ToString());
            }
        }



        public async Task<CustomerServiceCompensationDto> GetByIdAsync(string id)
        {
            var result = await dalCustomerServiceCompensation.GetAll().Include(x => x.CreateByEmployee).Include(x => x.BelongEmployee).Where(x => x.Id == id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomerServiceCompensationDto();
            }

            CustomerServiceCompensationDto returnResult = new CustomerServiceCompensationDto();

            returnResult.Id = result.Id;
            returnResult.CreateDate = result.CreateDate;
            returnResult.CreateBy = result.CreateBy;
            returnResult.CreateByEmpName = result.CreateByEmployee.Name;
            returnResult.UpdateDate = result.UpdateDate;
            returnResult.Valid = result.Valid;
            returnResult.DeleteDate = result.DeleteDate;
            returnResult.Name = result.Name;
            returnResult.BelongEmpId = result.BelongEmpId;
            returnResult.BelongEmpName = result.BelongEmployee.Name;
            returnResult.TotalPrice = result.TotalPrice;
            returnResult.OtherPrice = result.OtherPrice;
            returnResult.Remark = result.Remark;
            returnResult.Salary = result.Salary;
            returnResult.OldTakeNewCustomerPrice = result.OldTakeNewCustomerPrice;
            //returnResult.PerformancePercent = result.PerformancePercent;
            returnResult.CustomerServicePerformance = result.CustomerServicePerformance;
            returnResult.ToHospitalRate = result.ToHospitalRate;
            returnResult.ToHospitalRateReword = result.ToHospitalRateReword;
            returnResult.RepeatPurchasesRate = result.RepeatPurchasesRate;
            returnResult.RepeatPurchasesRateReword = result.RepeatPurchasesRateReword;
            returnResult.NewCustomerToHospitalReword = result.NewCustomerToHospitalReword;
            returnResult.OldCustomerToHospitalReword = result.OldCustomerToHospitalReword;
            returnResult.TargetFinishReword = result.TargetFinishReword;
            returnResult.OtherChargebacks = result.OtherChargebacks;


            returnResult.AddClueCompletePrice = result.AddClueCompletePrice;
            returnResult.AddWechatCompletePrice = result.AddWechatCompletePrice;
            returnResult.BeautyAddWechatPrice = result.BeautyAddWechatPrice;
            returnResult.TakeGoodsAddWechatPrice = result.TakeGoodsAddWechatPrice;
            returnResult.ConsulationCardPrice = result.ConsulationCardPrice;
            returnResult.ConsulationCardAddWechatPrice = result.ConsulationCardAddWechatPrice;
            returnResult.CooperationLiveAnchorToHospitalPrice = result.CooperationLiveAnchorToHospitalPrice;
            returnResult.CooperationLiveAnchorSendOrderPrice = result.CooperationLiveAnchorSendOrderPrice;
            returnResult.SpecialHospitalVisitPrice = result.SpecialHospitalVisitPrice;
            returnResult.Verison = result.Verison;
            return returnResult;
        }



        /// <summary>
        /// 修改助理薪资单
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateCustomerServiceCompensationDto updateDto)
        {
            var result = await dalCustomerServiceCompensation.GetAll().Where(x => x.Id == updateDto.Id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("未找到助理薪资单信息");
            result.Name = updateDto.Name;
            result.BelongEmpId = updateDto.BelongEmpId;
            result.TotalPrice = updateDto.TotalPrice;
            result.OtherPrice = updateDto.OtherPrice;
            result.Remark = updateDto.Remark;
            result.UpdateDate = DateTime.Now;
            result.Salary = updateDto.Salary;
            //result.PerformancePercent = updateDto.PerformancePercent;
            result.CustomerServicePerformance = updateDto.CustomerServicePerformance;
            result.ToHospitalRate = updateDto.ToHospitalRate;
            result.ToHospitalRateReword = updateDto.ToHospitalRateReword;
            result.RepeatPurchasesRate = updateDto.RepeatPurchasesRate;
            result.RepeatPurchasesRateReword = updateDto.RepeatPurchasesRateReword;
            result.NewCustomerToHospitalReword = updateDto.NewCustomerToHospitalReword;
            result.OldCustomerToHospitalReword = updateDto.OldCustomerToHospitalReword;
            result.TargetFinishReword = updateDto.TargetFinishReword;
            result.OtherChargebacks = updateDto.OtherChargebacks;
            result.OldTakeNewCustomerPrice = updateDto.OldTakeNewCustomerPrice;


            result.AddClueCompletePrice = updateDto.AddClueCompletePrice;
            result.AddWechatCompletePrice = updateDto.AddWechatCompletePrice;
            result.BeautyAddWechatPrice = updateDto.BeautyAddWechatPrice;
            result.TakeGoodsAddWechatPrice = updateDto.TakeGoodsAddWechatPrice;
            result.ConsulationCardPrice = updateDto.ConsulationCardPrice;
            result.ConsulationCardAddWechatPrice = updateDto.ConsulationCardAddWechatPrice;
            result.CooperationLiveAnchorToHospitalPrice = updateDto.CooperationLiveAnchorToHospitalPrice;
            result.CooperationLiveAnchorSendOrderPrice = updateDto.CooperationLiveAnchorSendOrderPrice;
            result.SpecialHospitalVisitPrice = updateDto.SpecialHospitalVisitPrice;
            result.Verison = updateDto.Verison;
            await dalCustomerServiceCompensation.UpdateAsync(result, true);
        }

        /// <summary>
        /// 作废助理薪资单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                var result = await dalCustomerServiceCompensation.GetAll().SingleOrDefaultAsync(e => e.Id == id && e.Valid == true);
                if (result == null)
                    throw new Exception("未找到助理薪资单信息");
                result.Valid = false;
                result.DeleteDate = DateTime.Now;
                await dalCustomerServiceCompensation.UpdateAsync(result, true);
                if (result.Verison == "1.0")
                {
                    await recommandDocumentSettleService.RemoveCustomerServiceCompensationIdAsync(id);
                }
                if (result.Verison == "2.0")
                {
                    await customerServiceCheckPerformanceService.RemoveCustomerServiceCompensationIdAsync(id);
                }
                unitOfWork.Commit();

            }
            catch (Exception er)
            {
                unitOfWork.RollBack();
                throw new Exception(er.Message.ToString());
            }
        }
        /// <summary>
        /// 复制薪资单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createEmpId">创建人</param>
        /// <returns></returns>
        public async Task CopyAsync(string id, int createEmpId)
        {
            var data = dalCustomerServiceCompensation.GetAll().Where(e => e.Id == id).SingleOrDefault();
            if (data == null)
                throw new Exception("薪资单编号错误");
            data.Id = Guid.NewGuid().ToString();
            data.Name = $"{data.Name}-复制";
            data.CreateBy = createEmpId;
            data.CreateDate = DateTime.Now;
            await dalCustomerServiceCompensation.AddAsync(data, true);
        }
        /// <summary>
        /// 助理业绩查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<DealInfoListDto>> GetDealInfoListAsync(QueryDealInfoDto queryDto)
        {
            FxPageInfo<DealInfoListDto> pageData = new FxPageInfo<DealInfoListDto>();
            var selectDate = DateTimeExtension.GetStartDateEndDate(queryDto.StartDate.Value, queryDto.EndDate.Value);
            var query = dalContentPlatFormOrderDealInfo.GetAll()
                .Include(e => e.ContentPlatFormOrder)
                .Where(e => e.CreateDate >= selectDate.StartDate && e.CreateDate < selectDate.EndDate);
            if (queryDto.CreateBy.HasValue)
            {
                query = query.Where(e => e.CreateBy == queryDto.CreateBy);
            }
            if (queryDto.BelongEmpId.HasValue)
            {
                query = query.Where(e => e.ContentPlatFormOrder.IsSupportOrder ? e.ContentPlatFormOrder.SupportEmpId == queryDto.BelongEmpId : e.ContentPlatFormOrder.BelongEmpId == queryDto.BelongEmpId);
            }
            if (queryDto.PerformanceType == (int)PerformanceType.Deal)
            {
                query = query.Where(e => e.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.AssistantCheck && e.DealPerformanceType != (int)ContentPlateFormOrderDealPerformanceType.FinanceCheck);
            }
            //if (queryDto.PerformanceType == (int)PerformanceType.Refund)
            //{
            //    query = query.Where(e => e.Price < 0);
            //}
            if (queryDto.PerformanceType == (int)PerformanceType.Check)
            {
                query = query.Where(e => e.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.AssistantCheck || e.DealPerformanceType == (int)ContentPlateFormOrderDealPerformanceType.FinanceCheck);
            }
            if (!string.IsNullOrEmpty(queryDto.Keyword))
            {
                query = query.Where(e => e.Id == queryDto.Keyword || e.ContentPlatFormOrderId == queryDto.Keyword);
            }
            pageData.TotalCount = await query.CountAsync();
            pageData.List = await query.Select(e => new DealInfoListDto
            {
                DealId = e.Id,
                ContentPaltformOrderId = e.ContentPlatFormOrderId,
                DealPrice = e.Price,
                PerformanceType = e.DealPerformanceType,
                PerformanceTypeText = ServiceClass.GetContentPlateFormOrderDealPerformanceType(e.DealPerformanceType),
                CreateById = e.CreateBy,
                CreateDate = e.CreateDate,
                IsDeal = e.IsDeal,
                IsSupportOrder = e.ContentPlatFormOrder.IsSupportOrder,
                BelongEmpId = e.ContentPlatFormOrder.BelongEmpId.Value,
                SupportEmpId = e.ContentPlatFormOrder.SupportEmpId
            }).OrderByDescending(x => x.CreateDate).Skip((queryDto.PageNum.Value - 1) * queryDto.PageSize.Value).Take(queryDto.PageSize.Value).ToListAsync();
            var employeeIdNameList = await dalAmiyaEmployee.GetAll().Select(e => new { e.Id, e.Name }).ToListAsync();
            foreach (var item in pageData.List)
            {
                item.CreateByName = employeeIdNameList.FirstOrDefault(e => e.Id == item.CreateById)?.Name ?? "其他";
                item.SupportEmpName = employeeIdNameList.FirstOrDefault(e => e.Id == item.SupportEmpId)?.Name ?? "其他";
                item.BelongEmpName = employeeIdNameList.FirstOrDefault(e => e.Id == item.BelongEmpId)?.Name ?? "其他";
                var checkPerformance = await customerServiceCheckPerformanceService.GetByDealIdAsync(item.DealId);
                if (!string.IsNullOrEmpty(checkPerformance.DealInfoId))
                {
                    item.IsCheckPerformance = "已提取";
                }
                else
                {
                    item.IsCheckPerformance = "未提取";
                }
            }
            return pageData;
        }

        #region【枚举下拉框】
        public List<BaseKeyValueDto<int>> GetPerformanceTypeText()
        {
            var performanceTypes = Enum.GetValues(typeof(PerformanceType));
            List<BaseKeyValueDto<int>> performTypeList = new List<BaseKeyValueDto<int>>();
            foreach (var item in performanceTypes)
            {
                BaseKeyValueDto<int> performTypeDto = new BaseKeyValueDto<int>();
                performTypeDto.Key = Convert.ToInt32(item);
                performTypeDto.Value = ServiceClass.GetPerformanceTypeText(performTypeDto.Key);
                performTypeList.Add(performTypeDto);
            }
            return performTypeList;
        }
        #endregion

    }
}
