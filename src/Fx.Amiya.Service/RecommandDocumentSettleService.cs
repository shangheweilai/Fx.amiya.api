
using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class RecommandDocumentSettleService : IRecommandDocumentSettleService
    {
        private IDalRecommandDocumentSettle _dalRecommandDocumentSettle;
        private IAmiyaEmployeeService amiyaEmployeeService;
        private ILiveAnchorService liveAnchorService;
        private IDalAmiyaEmployee dalAmiyaEmployee;
        public RecommandDocumentSettleService(IDalRecommandDocumentSettle dalRecommandDocumentSettle,
            ILiveAnchorService liveAnchorService,
            IAmiyaEmployeeService amiyaEmployeeService, IDalAmiyaEmployee dalAmiyaEmployee)
        {
            _dalRecommandDocumentSettle = dalRecommandDocumentSettle;
            this.amiyaEmployeeService = amiyaEmployeeService;
            this.liveAnchorService = liveAnchorService;
            this.dalAmiyaEmployee = dalAmiyaEmployee;
        }

        public async Task<List<RecommandDocumentSettleDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, bool? isSettle, bool? accountType, int chooseHospitalId, string keyword)
        {
            var record = _dalRecommandDocumentSettle.GetAll().Include(x => x.AmiyaEmployee)
              .Where(e => (string.IsNullOrEmpty(keyword) || e.RecommandDocumentId.Contains(keyword) || e.OrderId.Contains(keyword) || e.DealInfoId.Contains(keyword)))
              .Where(e => !isSettle.HasValue || e.IsSettle == isSettle.Value)
              .Where(e => !startDate.HasValue || e.CreateDate >= startDate)
              .Where(e => chooseHospitalId == 0 || e.HospitalId == chooseHospitalId)
              .Where(e => !startDate.HasValue || e.CreateDate >= startDate)
              .Where(e => !endDate.HasValue || e.CreateDate <= endDate)
              .Where(e => !accountType.HasValue || e.AccountType == accountType).OrderByDescending(x => x.CreateDate)
              .Select(e => new RecommandDocumentSettleDto
              {
                  Id = e.Id,
                  RecommandDocumentId = e.RecommandDocumentId,
                  OrderId = e.OrderId,
                  DealInfoId = e.DealInfoId,
                  OrderFrom = e.OrderFrom,
                  HospitalId = e.HospitalId,
                  OrderFromText = ServiceClass.GetOrderFromText(e.OrderFrom),
                  IsOldCustomer = e.IsOldCustomer,
                  IsOldCustomerText = e.IsOldCustomer == true ? "老客业绩" : "新客业绩",
                  OrderPrice = e.OrderPrice,
                  ReturnBackPrice = e.ReturnBackPrice,
                  CreateDate = e.CreateDate,
                  RecolicationPrice = e.RecolicationPrice,
                  CreateEmpId = e.CreateEmpId,
                  IsSettle = e.IsSettle,
                  SettleDate = e.SettleDate,
                  BelongLiveAnchorAccount = e.BelongLiveAnchorAccount,
                  BelongEmpId = e.BelongEmpId,
                  CreateByEmpName = e.AmiyaEmployee.Name,
                  AccountTypeText = e.AccountType == true ? "出账" : "入账",
                  AccountPrice = e.AccountPrice,
                  CustomerServiceSettlePrice = e.CustomerServiceSettlePrice
              });

            var result = await record.ToListAsync();
            if (chooseHospitalId == 0)
            {
                result = result.Where(x => x.HospitalId != 16 && x.HospitalId != 37).ToList();
            }
            return result;
        }

        public async Task<FxPageInfo<RecommandDocumentSettleDto>> GetListWithPageAsync(QueryReconciliationDocumentsSettleDto query)
        {
            var record = _dalRecommandDocumentSettle.GetAll().Include(x => x.AmiyaEmployee)
              .Where(e => Convert.ToInt32(e.DealInfoId) < 2410010000000000M)
              .Where(e => (string.IsNullOrEmpty(query.KeyWord) || e.RecommandDocumentId.Contains(query.KeyWord) || e.OrderId.Contains(query.KeyWord) || e.DealInfoId.Contains(query.KeyWord) || e.CustomerServiceCompensationId == query.KeyWord || e.InspectCustomerServiceCompensationId == query.KeyWord || e.CheckRemark.Contains(query.KeyWord)))
              .Where(e => !query.StartDate.HasValue || e.CreateDate >= query.StartDate)
              .Where(e => !query.EndDate.HasValue || e.CreateDate <= query.EndDate.Value.AddDays(1).AddMilliseconds(-1))
              .Where(e => !query.ChooseHospitalId.HasValue || e.HospitalId == query.ChooseHospitalId)
              .Where(e => !query.IsOldCustoemr.HasValue || e.IsOldCustomer == query.IsOldCustoemr)
              .Where(e => !query.CheckState.HasValue || e.CompensationCheckState == query.CheckState)
              .Where(e => query.OrderFrom == 0 || e.OrderFrom == query.OrderFrom)
              .Where(e => query.AddOrderPrice == -1 || (query.AddOrderPrice == 0 ? e.ContentPlatFormOrderAddOrderPrice == query.AddOrderPrice : e.ContentPlatFormOrderAddOrderPrice > 0))
              .Where(e => !query.CreateEmpId.HasValue || e.CreateEmpId == query.CreateEmpId)
              .Where(e => !query.IsInspectOrder.HasValue || e.IsInspectPerformance == query.IsInspectOrder.Value)
              .Where(e => !query.InspectEmpId.HasValue || e.InspectEmpId == query.InspectEmpId.Value).OrderByDescending(x => x.CreateDate)
              .Where(e => query.LiveAnchorIds.Count == 0 || query.LiveAnchorIds.Contains(e.BelongLiveAnchorAccount.Value))
              .Select(e => new RecommandDocumentSettleDto
              {
                  Id = e.Id,
                  RecommandDocumentId = e.RecommandDocumentId,
                  HospitalId = e.HospitalId,
                  OrderId = e.OrderId,
                  DealInfoId = e.DealInfoId,
                  OrderFrom = e.OrderFrom,
                  OrderFromText = ServiceClass.GetOrderFromText(e.OrderFrom),
                  IsOldCustomer = e.IsOldCustomer,
                  ContentPlatFormOrderAddOrderPrice = e.ContentPlatFormOrderAddOrderPrice,
                  IsOldCustomerText = e.IsOldCustomer == true ? "老客业绩" : "新客业绩",
                  OrderPrice = e.OrderPrice,
                  CreateDate = e.CreateDate,
                  RecolicationPrice = e.RecolicationPrice,
                  CreateEmpId = e.CreateEmpId,
                  BelongLiveAnchorAccount = e.BelongLiveAnchorAccount,
                  ReturnBackPrice = e.ReturnBackPrice,
                  BelongEmpId = e.BelongEmpId,
                  AccountTypeText = e.AccountType == true ? "出账" : "入账",
                  CustomerServiceSettlePrice = e.CustomerServiceSettlePrice,
                  CheckBelongEmpId = e.CheckBelongEmpId,
                  CheckRemark = e.CheckRemark,
                  CheckTypeText = ServiceClass.GetReconciliationDocumentSettleCheckType(e.CheckType),
                  IsInspectPerformance = e.IsInspectPerformance,
                  InspectPrice = e.InspectPrice,
                  InspectPercent = e.InspectPercent,
                  InspectBy = e.InspectEmpId,
                  CustomerServiceOrderPerformance = e.CustomerServiceOrderPerformance,
                  CheckDate = e.CheckDate,
                  CompensationCheckState = e.CompensationCheckState,
                  CompensationCheckStateText = ServiceClass.GetCheckTypeText(e.CompensationCheckState),
                  CustomerServiceCompensationId = e.CustomerServiceCompensationId,
                  InspectCustomerServiceCompensationId = e.InspectCustomerServiceCompensationId,
                  CustomerServicePerformance = e.CustomerServicePerformance,
                  PerformancePercent = e.PerformancePercent,
              });
            if (query.CheckState != (int)CheckType.CheckedSuccess)
            {
                record = record.Where(e => query.BelongEmpId.Count == 0 || query.BelongEmpId.Contains(e.BelongEmpId));
            }
            else
            {
                record = record.Where(e => query.BelongEmpId.Count == 0 || query.BelongEmpId.Contains(e.CheckBelongEmpId));
            }
            if (query.IsGenerateSalry.HasValue)
            {
                record = record.Where(e => query.IsGenerateSalry == 1 ? string.IsNullOrEmpty(e.CustomerServiceCompensationId) : !string.IsNullOrEmpty(e.CustomerServiceCompensationId));
            }
            if (query.IsGenerateInspectSalry.HasValue)
            {
                record = record.Where(e => query.IsGenerateInspectSalry == 1 ? string.IsNullOrEmpty(e.InspectCustomerServiceCompensationId) : !string.IsNullOrEmpty(e.InspectCustomerServiceCompensationId));
            }

            //合作达人部分
            var liveAnchorBaseInfo = await liveAnchorService.GetNotSelfLiveAnchor();
            var record2 = _dalRecommandDocumentSettle.GetAll().Include(x => x.AmiyaEmployee)
             .Where(e => liveAnchorBaseInfo.Select(x => x.Id).Contains(e.BelongLiveAnchorAccount.Value))
             .Where(e => (string.IsNullOrEmpty(query.KeyWord) || e.RecommandDocumentId.Contains(query.KeyWord) || e.OrderId.Contains(query.KeyWord) || e.DealInfoId.Contains(query.KeyWord) || e.CustomerServiceCompensationId == query.KeyWord || e.InspectCustomerServiceCompensationId == query.KeyWord || e.CheckRemark.Contains(query.KeyWord)))
             .Where(e => !query.StartDate.HasValue || e.CreateDate >= query.StartDate)
             .Where(e => !query.EndDate.HasValue || e.CreateDate <= query.EndDate.Value.AddDays(1).AddMilliseconds(-1))
             .Where(e => !query.ChooseHospitalId.HasValue || e.HospitalId == query.ChooseHospitalId)
             .Where(e => !query.IsOldCustoemr.HasValue || e.IsOldCustomer == query.IsOldCustoemr)
             .Where(e => !query.CheckState.HasValue || e.CompensationCheckState == query.CheckState)
             .Where(e => query.OrderFrom == 0 || e.OrderFrom == query.OrderFrom)
             .Where(e => query.AddOrderPrice == -1 || (query.AddOrderPrice == 0 ? e.ContentPlatFormOrderAddOrderPrice == query.AddOrderPrice : e.ContentPlatFormOrderAddOrderPrice > 0))
             .Where(e => !query.CreateEmpId.HasValue || e.CreateEmpId == query.CreateEmpId)
             .Where(e => !query.IsInspectOrder.HasValue || e.IsInspectPerformance == query.IsInspectOrder.Value)
             .Where(e => !query.InspectEmpId.HasValue || e.InspectEmpId == query.InspectEmpId.Value).OrderByDescending(x => x.CreateDate)
              .Where(e => query.LiveAnchorIds.Count == 0 || query.LiveAnchorIds.Contains(e.BelongLiveAnchorAccount.Value))
             .Select(e => new RecommandDocumentSettleDto
             {
                 Id = e.Id,
                 RecommandDocumentId = e.RecommandDocumentId,
                 HospitalId = e.HospitalId,
                 OrderId = e.OrderId,
                 DealInfoId = e.DealInfoId,
                 OrderFrom = e.OrderFrom,
                 OrderFromText = ServiceClass.GetOrderFromText(e.OrderFrom),
                 IsOldCustomer = e.IsOldCustomer,
                 ContentPlatFormOrderAddOrderPrice = e.ContentPlatFormOrderAddOrderPrice,
                 IsOldCustomerText = e.IsOldCustomer == true ? "老客业绩" : "新客业绩",
                 OrderPrice = e.OrderPrice,
                 CreateDate = e.CreateDate,
                 RecolicationPrice = e.RecolicationPrice,
                 CreateEmpId = e.CreateEmpId,
                 BelongLiveAnchorAccount = e.BelongLiveAnchorAccount,
                 ReturnBackPrice = e.ReturnBackPrice,
                 BelongEmpId = e.BelongEmpId,
                 AccountTypeText = e.AccountType == true ? "出账" : "入账",
                 CustomerServiceSettlePrice = e.CustomerServiceSettlePrice,
                 CheckBelongEmpId = e.CheckBelongEmpId,
                 CheckRemark = e.CheckRemark,
                 CheckTypeText = ServiceClass.GetReconciliationDocumentSettleCheckType(e.CheckType),
                 IsInspectPerformance = e.IsInspectPerformance,
                 InspectPrice = e.InspectPrice,
                 InspectPercent = e.InspectPercent,
                 InspectBy = e.InspectEmpId,
                 CustomerServiceOrderPerformance = e.CustomerServiceOrderPerformance,
                 CheckDate = e.CheckDate,
                 CompensationCheckState = e.CompensationCheckState,
                 CompensationCheckStateText = ServiceClass.GetCheckTypeText(e.CompensationCheckState),
                 CustomerServiceCompensationId = e.CustomerServiceCompensationId,
                 InspectCustomerServiceCompensationId = e.InspectCustomerServiceCompensationId,
                 CustomerServicePerformance = e.CustomerServicePerformance,
                 PerformancePercent = e.PerformancePercent,
             });
            if (query.CheckState != (int)CheckType.CheckedSuccess)
            {
                record2 = record2.Where(e => query.BelongEmpId.Count == 0 || query.BelongEmpId.Contains(e.BelongEmpId));
            }
            else
            {
                record2 = record2.Where(e => query.BelongEmpId.Count == 0 || query.BelongEmpId.Contains(e.CheckBelongEmpId));
            }
            if (query.IsGenerateSalry.HasValue)
            {
                record2 = record2.Where(e => query.IsGenerateSalry == 1 ? string.IsNullOrEmpty(e.CustomerServiceCompensationId) : !string.IsNullOrEmpty(e.CustomerServiceCompensationId));
            }
            if (query.IsGenerateInspectSalry.HasValue)
            {
                record2 = record2.Where(e => query.IsGenerateInspectSalry == 1 ? string.IsNullOrEmpty(e.InspectCustomerServiceCompensationId) : !string.IsNullOrEmpty(e.InspectCustomerServiceCompensationId));
            }
            FxPageInfo<RecommandDocumentSettleDto> resultPageInfo = new FxPageInfo<RecommandDocumentSettleDto>();
            var res1 = await record.ToListAsync();
            var res2 = await record2.ToListAsync();
            res1.AddRange(res2);
            resultPageInfo.TotalCount = res1.Count();
            resultPageInfo.List = res1.OrderByDescending(x => x.CreateDate).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToList();
            return resultPageInfo;
        }


        public async Task<List<RecommandDocumentSettleDto>> GetRecommandDocumentSettleAsync(List<string> recommandDocumentIds, bool? isSettle)
        {
            var recommandDocumentSettle = await _dalRecommandDocumentSettle.GetAll().Where(z => !isSettle.HasValue || z.IsSettle == isSettle).Where(z => recommandDocumentIds.Contains(z.RecommandDocumentId)).ToListAsync();
            List<RecommandDocumentSettleDto> RecommandDocumentSettleDtoList = new List<RecommandDocumentSettleDto>();
            foreach (var z in recommandDocumentSettle)
            {

                RecommandDocumentSettleDto recommandDocumentSettleDto = new RecommandDocumentSettleDto();
                recommandDocumentSettleDto.RecommandDocumentId = z.RecommandDocumentId;
                recommandDocumentSettleDto.Id = z.Id;
                recommandDocumentSettleDto.OrderId = z.OrderId;
                recommandDocumentSettleDto.OrderPrice = z.OrderPrice;
                recommandDocumentSettleDto.IsOldCustomer = z.IsOldCustomer;
                recommandDocumentSettleDto.IsOldCustomerText = z.IsOldCustomer == true ? "老客业绩" : "新客业绩";
                recommandDocumentSettleDto.DealInfoId = z.DealInfoId;
                recommandDocumentSettleDto.OrderFrom = z.OrderFrom;
                recommandDocumentSettleDto.RecolicationPrice = z.RecolicationPrice;
                recommandDocumentSettleDto.CreateEmpId = z.CreateEmpId;
                recommandDocumentSettleDto.ReturnBackPrice = z.ReturnBackPrice;
                recommandDocumentSettleDto.CreateDate = z.CreateDate;
                recommandDocumentSettleDto.IsSettle = z.IsSettle;
                RecommandDocumentSettleDtoList.Add(recommandDocumentSettleDto);
            };
            return RecommandDocumentSettleDtoList;

        }

        public async Task AddAsync(AddRecommandDocumentSettleDto addRecommandDocumentSettleDto)
        {
            RecommandDocumentSettle recommandDocumentSettle = new RecommandDocumentSettle();
            recommandDocumentSettle.Id = Guid.NewGuid().ToString();
            recommandDocumentSettle.RecommandDocumentId = addRecommandDocumentSettleDto.RecommandDocumentId;
            recommandDocumentSettle.OrderId = addRecommandDocumentSettleDto.OrderId;
            recommandDocumentSettle.OrderFrom = addRecommandDocumentSettleDto.OrderFrom;
            recommandDocumentSettle.OrderPrice = addRecommandDocumentSettleDto.OrderPrice;
            recommandDocumentSettle.RecolicationPrice = addRecommandDocumentSettleDto.RecolicationPrice;
            recommandDocumentSettle.IsOldCustomer = addRecommandDocumentSettleDto.IsOldCustomer;
            recommandDocumentSettle.DealInfoId = addRecommandDocumentSettleDto.DealInfoId;
            recommandDocumentSettle.ReturnBackPrice = addRecommandDocumentSettleDto.ReturnBackPrice;
            recommandDocumentSettle.CheckType = (int)ReconciliationDocumentSettleCheckType.Others;
            recommandDocumentSettle.CustomerServiceSettlePrice = addRecommandDocumentSettleDto.CustomerServiceSettlePrice;
            recommandDocumentSettle.CreateDate = DateTime.Now;
            recommandDocumentSettle.CreateEmpId = addRecommandDocumentSettleDto.CreateEmpId;
            recommandDocumentSettle.IsSettle = false;
            recommandDocumentSettle.BelongEmpId = addRecommandDocumentSettleDto.BelongEmpId;
            recommandDocumentSettle.IsInspectPerformance = false;
            recommandDocumentSettle.InspectPercent = 0.00M;
            recommandDocumentSettle.InspectPrice = 0.00M;
            recommandDocumentSettle.CustomerServiceOrderPerformance = 0.00M;
            recommandDocumentSettle.BelongLiveAnchorAccount = addRecommandDocumentSettleDto.BelongLiveAnchorAccount;
            recommandDocumentSettle.CreateBy = addRecommandDocumentSettleDto.CreateBy;
            recommandDocumentSettle.AccountType = addRecommandDocumentSettleDto.AccountType;
            recommandDocumentSettle.AccountPrice = addRecommandDocumentSettleDto.AccountPrice;
            recommandDocumentSettle.HospitalId = addRecommandDocumentSettleDto.HospitalId;
            recommandDocumentSettle.ContentPlatFormOrderAddOrderPrice = addRecommandDocumentSettleDto.ContentPlatFormOrderAddOrderPrice;
            await _dalRecommandDocumentSettle.AddAsync(recommandDocumentSettle, true);
        }


        public async Task UpdateIsRerturnBackAsync(string Id, DateTime returnBackDate)
        {
            var result = await _dalRecommandDocumentSettle.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.IsSettle = true;
                result.SettleDate = returnBackDate;
                await _dalRecommandDocumentSettle.UpdateAsync(result, true);
            }
        }
        public async Task CheckAsync(CheckReconciliationDocumentSettleDto checkDto)
        {
            var result = await _dalRecommandDocumentSettle.GetAll().Where(x => x.Id == checkDto.Id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.CompensationCheckState = checkDto.CheckState;
                result.CheckBy = checkDto.CheckBy;
                result.CheckRemark = checkDto.CheckRemark;
                result.CheckDate = DateTime.Now;
                result.IsInspectPerformance = checkDto.IsInspectPerformance;
                result.CheckType = checkDto.CheckType;

                #region 助理业绩
                result.CustomerServiceOrderPerformance = checkDto.CustomerServiceOrderPerformance;
                result.CheckBelongEmpId = checkDto.CheckBelongEmpId;
                result.PerformancePercent = checkDto.PerformancePercent;
                result.CustomerServicePerformance = checkDto.CustomerServicePerformance;
                #endregion

                #region 稽查人员业绩
                result.InspectEmpId = checkDto.InspectEmpId;
                result.InspectPercent = checkDto.InspectPercent;
                result.InspectPrice = checkDto.InspectPrice;
                #endregion
                await _dalRecommandDocumentSettle.UpdateAsync(result, true);
            }
        }
        /// <summary>
        /// 生成薪资单编号
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="customerServiceCompensationId"></param>
        /// <returns></returns>
        public async Task AddCustomerServiceCompensationIdAsync(List<string> ids, string customerServiceCompensationId, int CustomerServiceCompensationEmpId)
        {
            foreach (var z in ids)
            {
                var result = await _dalRecommandDocumentSettle.GetAll().Where(x => x.Id == z).FirstOrDefaultAsync();
                if (result != null)
                {
                    if (result.IsInspectPerformance == false)
                    {
                        result.CustomerServiceCompensationId = customerServiceCompensationId;
                        await _dalRecommandDocumentSettle.UpdateAsync(result, true);
                    }
                    else if (result.IsInspectPerformance == true)
                    {
                        //当该薪资单最终归属客服与当前生成薪资人员相等时则录入薪资单据id
                        if (result.CheckBelongEmpId == CustomerServiceCompensationEmpId)
                        {
                            result.CustomerServiceCompensationId = customerServiceCompensationId;
                            await _dalRecommandDocumentSettle.UpdateAsync(result, true);
                        }
                        //当该薪资单最终归属客服与当前生成薪资人员不等时则录入稽查薪资单据id
                        else
                        {
                            result.InspectCustomerServiceCompensationId = customerServiceCompensationId;
                            await _dalRecommandDocumentSettle.UpdateAsync(result, true);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 薪资单作废时移除薪资单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="customerServiceCompensationId"></param>
        /// <returns></returns>
        public async Task RemoveCustomerServiceCompensationIdAsync(string customerServiceCompensationId)
        {
            //回退薪资单
            var list = await _dalRecommandDocumentSettle.GetAll().Where(e => e.CustomerServiceCompensationId == customerServiceCompensationId).ToListAsync();
            var result = list.Count();
            if (result > 0)
            {
                foreach (var item in list)
                {
                    item.CustomerServiceCompensationId = null;
                    await _dalRecommandDocumentSettle.UpdateAsync(item, true);
                }
            }
            //回退稽查单
            var list2 = await _dalRecommandDocumentSettle.GetAll().Where(e => e.InspectCustomerServiceCompensationId == customerServiceCompensationId).ToListAsync();
            var result2 = list2.Count();
            if (result2 > 0)
            {
                foreach (var item in list2)
                {
                    item.InspectCustomerServiceCompensationId = null;
                    await _dalRecommandDocumentSettle.UpdateAsync(item, true);
                }
            }
        }

        /// <summary>
        /// 根据薪资单id获取对账单记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<RecommandDocumentSettleDto>> GetListWithPageByCustomerServiceCompensationIdAsync(QueryReconciliationDocumentsSettleDto query)
        {
            var record = _dalRecommandDocumentSettle.GetAll().Include(x => x.AmiyaEmployee)
              .Where(e => e.CustomerServiceCompensationId == query.CustomerServiceCompensationId).OrderByDescending(x => x.CreateDate)
              .Select(e => new RecommandDocumentSettleDto
              {
                  Id = e.Id,
                  RecommandDocumentId = e.RecommandDocumentId,
                  HospitalId = e.HospitalId,
                  OrderId = e.OrderId,
                  DealInfoId = e.DealInfoId,
                  OrderFrom = e.OrderFrom,
                  OrderFromText = ServiceClass.GetOrderFromText(e.OrderFrom),
                  IsOldCustomer = e.IsOldCustomer,
                  IsOldCustomerText = e.IsOldCustomer == true ? "老客业绩" : "新客业绩",
                  OrderPrice = e.OrderPrice,
                  CreateDate = e.CreateDate,
                  RecolicationPrice = e.RecolicationPrice,
                  CreateEmpId = e.CreateEmpId,
                  BelongLiveAnchorAccount = e.BelongLiveAnchorAccount,
                  ReturnBackPrice = e.ReturnBackPrice,
                  BelongEmpId = e.BelongEmpId,
                  AccountTypeText = e.AccountType == true ? "出账" : "入账",
                  CustomerServiceSettlePrice = e.CustomerServiceSettlePrice,
                  CheckBelongEmpId = e.CheckBelongEmpId,
                  CheckRemark = e.CheckRemark,
                  CheckDate = e.CheckDate,
                  CompensationCheckState = e.CompensationCheckState,
                  CompensationCheckStateText = ServiceClass.GetCheckTypeText(e.CompensationCheckState),
                  CustomerServiceCompensationId = e.CustomerServiceCompensationId
              });

            FxPageInfo<RecommandDocumentSettleDto> resultPageInfo = new FxPageInfo<RecommandDocumentSettleDto>();
            resultPageInfo.TotalCount = await record.CountAsync();
            resultPageInfo.List = await record.OrderByDescending(x => x.CreateDate).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToListAsync();
            return resultPageInfo;
        }
        /// <summary>
        /// 获取业绩上传人名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseIdAndNameDto<int>>> GetCreateEmpNameListAsync()
        {
            var idList = _dalRecommandDocumentSettle.GetAll().Select(e => e.CreateEmpId).ToList();
            var nameList = await dalAmiyaEmployee.GetAll().Where(e => idList.Contains(e.Id)).Select(e => new BaseIdAndNameDto<int>
            {
                Id = e.Id,
                Name = e.Name,
            }).ToListAsync();
            return nameList;
        }


        /// <summary>
        /// 获取薪资审核类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseKeyValueDto>> GeReconciliationtCheckTypeAsync()
        {
            var consumptionVoucherTypes = Enum.GetValues(typeof(ReconciliationDocumentSettleCheckType));

            List<BaseKeyValueDto> consumptionVoucherTypeList = new List<BaseKeyValueDto>();
            foreach (var item in consumptionVoucherTypes)
            {
                BaseKeyValueDto baseKeyValueDto = new BaseKeyValueDto();
                baseKeyValueDto.Key = Convert.ToInt32(item).ToString();
                baseKeyValueDto.Value = ServiceClass.GetReconciliationDocumentSettleCheckType(Convert.ToInt32(item));
                consumptionVoucherTypeList.Add(baseKeyValueDto);
            }
            return consumptionVoucherTypeList;
        }
    }
}
