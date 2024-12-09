using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.AmiyaEmployee;
using Fx.Amiya.Dto.CustomerServiceCheckPerformance.Input;
using Fx.Amiya.Dto.CustomerServiceCheckPerformance.Result;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Fx.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class CustomerServiceCheckPerformanceService : ICustomerServiceCheckPerformanceService
    {
        private readonly IDalCustomerServiceCheckPerformance dalCustomerServiceCheckPerformance;
        private readonly IAmiyaEmployeeService amiyaEmployeeService;
        private readonly IUnitOfWork unitOfWork;
        public CustomerServiceCheckPerformanceService(IDalCustomerServiceCheckPerformance dalCustomerServiceCheckPerformance, IAmiyaEmployeeService amiyaEmployeeService, IUnitOfWork unitOfWork)
        {
            this.dalCustomerServiceCheckPerformance = dalCustomerServiceCheckPerformance;
            this.amiyaEmployeeService = amiyaEmployeeService;
            this.unitOfWork = unitOfWork;
        }



        /// <summary>
        /// 根据条件获取助理提取业绩信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<CustomerServiceCheckPerformanceDto>> GetListAsync(QueryCustomerServiceCheckPerformanceDto query)
        {
            AmiyaEmployeeDto employeeInfo = new AmiyaEmployeeDto();
            var customerServiceCheckPerformances = from d in dalCustomerServiceCheckPerformance.GetAll().Include(x => x.AmiyaEmployee)
                                                   where (string.IsNullOrEmpty(query.KeyWord) || d.Remark.Contains(query.KeyWord) || d.DealInfoId.Contains(query.KeyWord) || d.OrderId.Contains(query.KeyWord))
                                                   && (!query.Valid.HasValue || d.Valid == query.Valid.Value)
                                                   && (!query.BelongEmpId.HasValue || d.BelongEmpId == query.BelongEmpId.Value)
                                                   && (!query.CheckEmpId.HasValue || d.CheckEmpId == query.CheckEmpId.Value)
                                                   && (!query.StartDate.HasValue || d.CreateDate >= query.StartDate.Value)
                                                   && (!query.EndDate.HasValue || d.CreateDate < query.EndDate.Value)
                                                   && (query.PerformanceTypeList == null || query.PerformanceTypeList.Contains(d.PerformanceType))
                                                   select new CustomerServiceCheckPerformanceDto
                                                   {
                                                       Id = d.Id,
                                                       CreateDate = d.CreateDate,
                                                       UpdateDate = d.UpdateDate,
                                                       Valid = d.Valid,
                                                       DeleteDate = d.DeleteDate,
                                                       DealInfoId = d.DealInfoId,
                                                       OrderId = d.OrderId,
                                                       OrderFrom = d.OrderFrom,
                                                       OrderFromText = ServiceClass.GetOrderFromText(d.OrderFrom),
                                                       DealPrice = d.DealPrice,
                                                       DealCreateDate = d.DealCreateDate,
                                                       PerformanceType = d.PerformanceType,
                                                       PerformanceTypeText = ServiceClass.GetPerformanceTypeText(d.PerformanceType),
                                                       BelongEmpId = d.BelongEmpId,
                                                       BelongEmpName = d.AmiyaEmployee.Name,
                                                       CheckEmpId = d.CheckEmpId,
                                                       Remark = d.Remark,
                                                       Point = d.Point,
                                                       PerformanceCommision = d.PerformanceCommision,
                                                       PerformanceCommisionCheck = d.PerformanceCommisionCheck,
                                                       BillId = d.BillId,
                                                       CheckBillId = d.CheckBillId,
                                                   };
            FxPageInfo<CustomerServiceCheckPerformanceDto> customerServiceCheckPerformancePageInfo = new FxPageInfo<CustomerServiceCheckPerformanceDto>();
            customerServiceCheckPerformancePageInfo.List = await customerServiceCheckPerformances.ToListAsync();
            if (!string.IsNullOrEmpty(query.customerServiceCompensationId))
            {

                if (query.PerformanceTypeList.Contains((int)PerformanceType.Check) == false)
                {
                    customerServiceCheckPerformancePageInfo.List = customerServiceCheckPerformancePageInfo.List.Where(e => e.BillId == query.customerServiceCompensationId).ToList();
                }
                else
                {
                    if (query.BelongEmpId.HasValue)
                    {

                        var empInfo = await amiyaEmployeeService.GetByIdAsync(query.BelongEmpId.Value);
                        if (empInfo.PositionId != 30)
                        {
                            customerServiceCheckPerformancePageInfo.List = customerServiceCheckPerformancePageInfo.List.Where(e => e.BillId == query.customerServiceCompensationId).ToList();
                        }
                        else
                        {
                            customerServiceCheckPerformancePageInfo.List = customerServiceCheckPerformancePageInfo.List.Where(e => e.CheckBillId == query.customerServiceCompensationId).ToList();
                        }
                    }
                    if (query.CheckEmpId.HasValue)
                    {
                        customerServiceCheckPerformancePageInfo.List = customerServiceCheckPerformancePageInfo.List.Where(e => e.CheckBillId == query.customerServiceCompensationId).ToList();

                    }
                }
            }
            customerServiceCheckPerformancePageInfo.TotalCount = customerServiceCheckPerformancePageInfo.List.Count();
            customerServiceCheckPerformancePageInfo.List = customerServiceCheckPerformancePageInfo.List.OrderByDescending(x => x.CreateDate).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToList();
            foreach (var x in customerServiceCheckPerformancePageInfo.List)
            {
                if (x.CheckEmpId.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CheckEmpId.Value);
                    x.CheckEmpName = empInfo.Name;
                }
            }
            return customerServiceCheckPerformancePageInfo;
        }


        /// <summary>
        /// 添加助理提取业绩
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddCustomerServiceCheckPerformanceDto addDto)
        {
            try
            {
                CustomerServiceCheckPerformance customerServiceCheckPerformance = new CustomerServiceCheckPerformance();
                customerServiceCheckPerformance.Id = Guid.NewGuid().ToString();
                customerServiceCheckPerformance.CreateDate = DateTime.Now;
                customerServiceCheckPerformance.Valid = true;
                customerServiceCheckPerformance.DealInfoId = addDto.DealInfoId;
                customerServiceCheckPerformance.OrderId = addDto.OrderId;
                customerServiceCheckPerformance.OrderFrom = addDto.OrderFrom;
                customerServiceCheckPerformance.DealPrice = addDto.DealPrice;
                customerServiceCheckPerformance.DealCreateDate = addDto.DealCreateDate;
                customerServiceCheckPerformance.PerformanceType = addDto.PerformanceType;
                customerServiceCheckPerformance.BelongEmpId = addDto.BelongEmpId;
                customerServiceCheckPerformance.Remark = addDto.Remark;
                customerServiceCheckPerformance.Point = addDto.Point;
                customerServiceCheckPerformance.CheckEmpId = addDto.CheckEmpId;
                customerServiceCheckPerformance.BillId = addDto.BillId;
                customerServiceCheckPerformance.CheckBillId = addDto.CheckBillId;
                customerServiceCheckPerformance.PerformanceCommisionCheck = addDto.PerformanceCommisionCheck;
                customerServiceCheckPerformance.PerformanceCommision = addDto.PerformanceCommision;
                await dalCustomerServiceCheckPerformance.AddAsync(customerServiceCheckPerformance, true);

            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }


        public async Task AddListAsync(List<AddCustomerServiceCheckPerformanceDto> addDto)
        {
            unitOfWork.BeginTransaction();
            try
            {
                foreach (var x in addDto)
                {

                    CustomerServiceCheckPerformance customerServiceCheckPerformance = new CustomerServiceCheckPerformance();
                    customerServiceCheckPerformance.Id = Guid.NewGuid().ToString();
                    customerServiceCheckPerformance.CreateDate = DateTime.Now;
                    customerServiceCheckPerformance.Valid = true;
                    customerServiceCheckPerformance.DealInfoId = x.DealInfoId;
                    customerServiceCheckPerformance.OrderId = x.OrderId;
                    customerServiceCheckPerformance.OrderFrom = x.OrderFrom;
                    customerServiceCheckPerformance.DealPrice = x.DealPrice;
                    customerServiceCheckPerformance.DealCreateDate = x.DealCreateDate;
                    customerServiceCheckPerformance.PerformanceType = x.PerformanceType;
                    customerServiceCheckPerformance.BelongEmpId = x.BelongEmpId;
                    customerServiceCheckPerformance.Remark = x.Remark;
                    customerServiceCheckPerformance.Point = x.Point;
                    customerServiceCheckPerformance.CheckEmpId = x.CheckEmpId;
                    customerServiceCheckPerformance.BillId = x.BillId;
                    customerServiceCheckPerformance.CheckBillId = x.CheckBillId;
                    customerServiceCheckPerformance.PerformanceCommisionCheck = x.PerformanceCommisionCheck;
                    customerServiceCheckPerformance.PerformanceCommision = x.PerformanceCommision;
                    await dalCustomerServiceCheckPerformance.AddAsync(customerServiceCheckPerformance, true);
                }
                unitOfWork.Commit();
            }
            catch (Exception err)
            {
                unitOfWork.RollBack();
                throw new Exception(err.ToString());
            }
        }
        public async Task<CustomerServiceCheckPerformanceDto> GetByIdAsync(string id)
        {
            var result = await dalCustomerServiceCheckPerformance.GetAll().Where(x => x.Id == id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomerServiceCheckPerformanceDto();
            }

            CustomerServiceCheckPerformanceDto returnResult = new CustomerServiceCheckPerformanceDto();
            returnResult.Id = result.Id;
            returnResult.CreateDate = result.CreateDate;
            returnResult.Valid = result.Valid;
            returnResult.DealInfoId = result.DealInfoId;
            returnResult.OrderId = result.OrderId;
            returnResult.OrderFrom = result.OrderFrom;
            returnResult.DealPrice = result.DealPrice;
            returnResult.DealCreateDate = result.DealCreateDate;
            returnResult.PerformanceType = result.PerformanceType;
            returnResult.BelongEmpId = result.BelongEmpId;
            returnResult.Remark = result.Remark;
            returnResult.Point = result.Point;
            returnResult.PerformanceCommision = result.PerformanceCommision;
            returnResult.PerformanceCommisionCheck = result.PerformanceCommisionCheck;
            returnResult.CheckEmpId = result.CheckEmpId;
            returnResult.BillId = result.BillId;
            returnResult.CheckBillId = result.CheckBillId;
            return returnResult;
        }

        public async Task<CustomerServiceCheckPerformanceDto> GetByDealIdAsync(string dealId)
        {
            var result = await dalCustomerServiceCheckPerformance.GetAll().Where(x => x.DealInfoId == dealId && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomerServiceCheckPerformanceDto();
            }

            CustomerServiceCheckPerformanceDto returnResult = new CustomerServiceCheckPerformanceDto();
            returnResult.Id = result.Id;
            returnResult.CreateDate = result.CreateDate;
            returnResult.Valid = result.Valid;
            returnResult.DealInfoId = result.DealInfoId;
            returnResult.OrderId = result.OrderId;
            returnResult.OrderFrom = result.OrderFrom;
            returnResult.DealPrice = result.DealPrice;
            returnResult.DealCreateDate = result.DealCreateDate;
            returnResult.PerformanceType = result.PerformanceType;
            returnResult.BelongEmpId = result.BelongEmpId;
            returnResult.Remark = result.Remark;
            returnResult.Point = result.Point;
            returnResult.PerformanceCommision = result.PerformanceCommision;
            returnResult.PerformanceCommisionCheck = result.PerformanceCommisionCheck;
            returnResult.CheckEmpId = result.CheckEmpId;
            returnResult.BillId = result.BillId;
            returnResult.CheckBillId = result.CheckBillId;
            return returnResult;
        }

        /// <summary>
        /// 修改助理提取业绩
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateCustomerServiceCheckPerformanceDto updateDto)
        {
            var result = await dalCustomerServiceCheckPerformance.GetAll().Where(x => x.Id == updateDto.Id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("未找到助理提取业绩信息");

            result.DealInfoId = updateDto.DealInfoId;
            result.OrderId = updateDto.OrderId;
            result.OrderFrom = updateDto.OrderFrom;
            result.DealPrice = updateDto.DealPrice;
            result.DealCreateDate = updateDto.DealCreateDate;
            result.PerformanceType = updateDto.PerformanceType;
            result.BelongEmpId = updateDto.BelongEmpId;
            result.Remark = updateDto.Remark;
            result.Point = updateDto.Point;
            result.PerformanceCommision = updateDto.PerformanceCommision;
            result.PerformanceCommisionCheck = updateDto.PerformanceCommisionCheck;
            result.CheckEmpId = updateDto.CheckEmpId;
            result.BillId = updateDto.BillId;
            result.CheckBillId = updateDto.CheckBillId;
            result.UpdateDate = DateTime.Now;
            await dalCustomerServiceCheckPerformance.UpdateAsync(result, true);
        }

        /// <summary>
        /// 作废助理提取业绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            try
            {
                var result = await dalCustomerServiceCheckPerformance.GetAll().SingleOrDefaultAsync(e => e.Id == id && e.Valid == true);
                if (result == null)
                    throw new Exception("未找到助理提取业绩信息");
                result.Valid = false;
                result.DeleteDate = DateTime.Now;
                await dalCustomerServiceCheckPerformance.UpdateAsync(result, true);

            }
            catch (Exception er)
            {
                throw new Exception(er.Message.ToString());
            }
        }
        /// <summary>
        /// 批量作废助理提取业绩
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteListAsync(List<string> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var result = await dalCustomerServiceCheckPerformance.GetAll().SingleOrDefaultAsync(e => e.Id == id && e.Valid == true);
                    if (result == null)
                        throw new Exception("未找到助理提取业绩信息");
                    result.Valid = false;
                    result.DeleteDate = DateTime.Now;
                    await dalCustomerServiceCheckPerformance.UpdateAsync(result, true);
                }

            }
            catch (Exception er)
            {
                throw new Exception(er.Message.ToString());
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
                var result = await dalCustomerServiceCheckPerformance.GetAll().Where(x => x.Id == z).FirstOrDefaultAsync();
                if (result != null)
                {
                    if (result.PerformanceType != (int)PerformanceType.Check)
                    {
                        result.BillId = customerServiceCompensationId;
                        await dalCustomerServiceCheckPerformance.UpdateAsync(result, true);
                    }
                    else if (result.PerformanceType == (int)PerformanceType.Check)
                    {
                        //当该薪资单最终归属客服与当前生成薪资人员相等时则录入薪资单据id
                        if (result.BelongEmpId == CustomerServiceCompensationEmpId)
                        {
                            result.BillId = customerServiceCompensationId;
                            await dalCustomerServiceCheckPerformance.UpdateAsync(result, true);
                        }
                        //当该薪资单最终归属客服与当前生成薪资人员不等时则录入稽查薪资单据id
                        else
                        {
                            result.CheckBillId = customerServiceCompensationId;
                            await dalCustomerServiceCheckPerformance.UpdateAsync(result, true);
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
            var list = await dalCustomerServiceCheckPerformance.GetAll().Where(e => e.BillId == customerServiceCompensationId).ToListAsync();
            var result = list.Count();
            if (result > 0)
            {
                foreach (var item in list)
                {
                    item.BillId = null;
                    await dalCustomerServiceCheckPerformance.UpdateAsync(item, true);
                }
            }
            //回退稽查单
            var list2 = await dalCustomerServiceCheckPerformance.GetAll().Where(e => e.CheckBillId == customerServiceCompensationId).ToListAsync();
            var result2 = list2.Count();
            if (result2 > 0)
            {
                foreach (var item in list2)
                {
                    item.CheckBillId = null;
                    await dalCustomerServiceCheckPerformance.UpdateAsync(item, true);
                }
            }
        }


    }
}
