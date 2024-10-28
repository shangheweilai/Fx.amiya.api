using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Vo;
using Fx.Amiya.Background.Api.Vo.CustomerServiceCheckPerformance.Input;
using Fx.Amiya.Background.Api.Vo.CustomerServiceCheckPerformance.Result;
using Fx.Amiya.Dto.CustomerServiceCheckPerformance.Input;
using Fx.Amiya.IService;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 助理提取业绩
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CustomerServiceCheckPerformanceController : ControllerBase
    {
        private ICustomerServiceCheckPerformanceService customerServiceCheckPerformanceService;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerServiceCheckPerformanceService"></param>
        public CustomerServiceCheckPerformanceController(IHttpContextAccessor httpContextAccessor, ICustomerServiceCheckPerformanceService customerServiceCheckPerformanceService)
        {
            this.customerServiceCheckPerformanceService = customerServiceCheckPerformanceService;
            this._httpContextAccessor = httpContextAccessor;
        }



        /// <summary>
        /// 根据条件获取助理提取业绩信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<FxPageInfo<CustomerServiceCheckPerformanceVo>>> GetListWithPageAsync([FromQuery] QueryCustomerServiceCheckPerformanceVo query)
        {
            try
            {
                QueryCustomerServiceCheckPerformanceDto queryDto = new QueryCustomerServiceCheckPerformanceDto();
                queryDto.StartDate = query.StartDate;
                queryDto.EndDate = query.EndDate;
                queryDto.PageNum = query.PageNum;
                queryDto.PageSize = query.PageSize;
                queryDto.Valid = query.Valid;
                queryDto.KeyWord = query.KeyWord;
                queryDto.BelongEmpId = query.BelongEmpId;
                queryDto.CheckEmpId = query.CheckEmpId;
                queryDto.customerServiceCompensationId = query.customerServiceCompensationId;
                List<int> performanceTypeData = new List<int>();
                if (!string.IsNullOrEmpty(query.PerformanceTypeList))
                {
                    var data = query.PerformanceTypeList.Split(',');
                    foreach (var x in data)
                    {
                        performanceTypeData.Add(Convert.ToInt32(x));
                    }
                }
                queryDto.PerformanceTypeList = performanceTypeData;
                var q = await customerServiceCheckPerformanceService.GetListAsync(queryDto);
                var customerServiceCheckPerformance = from d in q.List
                                                      select new CustomerServiceCheckPerformanceVo
                                                      {
                                                          Id = d.Id,
                                                          CreateDate = d.CreateDate,
                                                          UpdateDate = d.UpdateDate,
                                                          Valid = d.Valid,
                                                          DeleteDate = d.DeleteDate,
                                                          DealInfoId = d.DealInfoId,
                                                          OrderId = d.OrderId,
                                                          OrderFrom = d.OrderFrom,
                                                          OrderFromText = d.OrderFromText,
                                                          DealPrice = d.DealPrice,
                                                          DealCreateDate = d.DealCreateDate,
                                                          PerformanceType = d.PerformanceType,
                                                          PerformanceTypeText = d.PerformanceTypeText,
                                                          BelongEmpId = d.BelongEmpId,
                                                          BelongEmpName = d.BelongEmpName,
                                                          CheckEmpId = d.CheckEmpId,
                                                          CheckEmpName = d.CheckEmpName,
                                                          Remark = d.Remark,
                                                          Point = d.Point,
                                                          PerformanceCommision = d.PerformanceCommision,
                                                          PerformanceCommisionCheck = d.PerformanceCommisionCheck,
                                                          BillId = d.BillId,
                                                          CheckBillId = d.CheckBillId,
                                                      };

                FxPageInfo<CustomerServiceCheckPerformanceVo> pageInfo = new FxPageInfo<CustomerServiceCheckPerformanceVo>();
                pageInfo.TotalCount = q.TotalCount;
                pageInfo.List = customerServiceCheckPerformance;

                return ResultData<FxPageInfo<CustomerServiceCheckPerformanceVo>>.Success().AddData("customerServiceCheckPerformance", pageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<CustomerServiceCheckPerformanceVo>>.Fail(ex.Message);
            }
        }




        /// <summary>
        /// 添加助理提取业绩
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [FxInternalAuthorize]
        public async Task<ResultData> AddAsync(AddCustomerServiceCheckPerformanceVo addVo)
        {
            try
            {
                AddCustomerServiceCheckPerformanceDto addDto = new AddCustomerServiceCheckPerformanceDto();
                addDto.DealInfoId = addVo.DealInfoId;
                addDto.OrderId = addVo.OrderId;
                addDto.OrderFrom = addVo.OrderFrom;
                addDto.DealPrice = addVo.DealPrice;
                addDto.DealCreateDate = addVo.DealCreateDate;
                addDto.PerformanceType = addVo.PerformanceType;
                addDto.BelongEmpId = addVo.BelongEmpId;
                addDto.Remark = addVo.Remark;
                addDto.Point = addVo.Point;
                addDto.PerformanceCommision = addVo.PerformanceCommision;
                addDto.PerformanceCommisionCheck = addVo.PerformanceCommisionCheck;
                addDto.CheckEmpId = addVo.CheckEmpId;
                addDto.BillId = addVo.BillId;
                addDto.CheckBillId = addVo.CheckBillId;
                await customerServiceCheckPerformanceService.AddAsync(addDto);

                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 批量添加助理提取业绩（只用于成交，在提交前不允许存在业绩类型为“4（助理稽查）,5（财务稽查）”状态
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("addList")]
        [FxInternalAuthorize]
        public async Task<ResultData> AddListAsync(List<AddCustomerServiceCheckPerformanceVo> addVo)
        {
            try
            {
                List<AddCustomerServiceCheckPerformanceDto> addListDto = new List<AddCustomerServiceCheckPerformanceDto>();
                var isExistCheck = addVo.Where(x => x.PerformanceType == (int)PerformanceType.Check).Count();
                if (isExistCheck > 0)
                {
                    throw new Exception("选中数据存在稽查业绩，请重新确认后提交！");
                }
                foreach (var x in addVo)
                {

                    AddCustomerServiceCheckPerformanceDto addDto = new AddCustomerServiceCheckPerformanceDto();
                    addDto.DealInfoId = x.DealInfoId;
                    addDto.OrderId = x.OrderId;
                    addDto.OrderFrom = x.OrderFrom;
                    addDto.DealPrice = x.DealPrice;
                    addDto.DealCreateDate = x.DealCreateDate;
                    addDto.PerformanceType = x.PerformanceType;
                    addDto.BelongEmpId = x.BelongEmpId;
                    addDto.Remark = x.Remark;
                    addDto.Point = x.Point;
                    addDto.PerformanceCommision = Math.Round((x.DealPrice * x.Point) / 100, 2, MidpointRounding.AwayFromZero);
                    addDto.PerformanceCommisionCheck = Math.Round((x.DealPrice * x.Point) / 100, 2, MidpointRounding.AwayFromZero);
                    addDto.CheckEmpId = x.CheckEmpId;
                    addDto.BillId = x.BillId;
                    addDto.CheckBillId = x.CheckBillId;
                    addListDto.Add(addDto);
                }
                await customerServiceCheckPerformanceService.AddListAsync(addListDto);

                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 根据助理提取业绩编号获取助理提取业绩信息
        /// </summary>
        /// <param name="id">助理提取业绩编号</param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData<CustomerServiceCheckPerformanceVo>> GetByIdAsync(string id)
        {
            try
            {
                var customerServiceCheckPerformance = await customerServiceCheckPerformanceService.GetByIdAsync(id);
                CustomerServiceCheckPerformanceVo customerServiceCheckPerformanceVo = new CustomerServiceCheckPerformanceVo();
                customerServiceCheckPerformanceVo.Id = customerServiceCheckPerformance.Id;
                customerServiceCheckPerformanceVo.CreateDate = customerServiceCheckPerformance.CreateDate;
                customerServiceCheckPerformanceVo.Valid = customerServiceCheckPerformance.Valid;
                customerServiceCheckPerformanceVo.DealInfoId = customerServiceCheckPerformance.DealInfoId;
                customerServiceCheckPerformanceVo.OrderId = customerServiceCheckPerformance.OrderId;
                customerServiceCheckPerformanceVo.OrderFrom = customerServiceCheckPerformance.OrderFrom;
                customerServiceCheckPerformanceVo.DealPrice = customerServiceCheckPerformance.DealPrice;
                customerServiceCheckPerformanceVo.DealCreateDate = customerServiceCheckPerformance.DealCreateDate;
                customerServiceCheckPerformanceVo.PerformanceType = customerServiceCheckPerformance.PerformanceType;
                customerServiceCheckPerformanceVo.BelongEmpId = customerServiceCheckPerformance.BelongEmpId;
                customerServiceCheckPerformanceVo.Remark = customerServiceCheckPerformance.Remark;
                customerServiceCheckPerformanceVo.Point = customerServiceCheckPerformance.Point;
                customerServiceCheckPerformanceVo.PerformanceCommision = customerServiceCheckPerformance.PerformanceCommision;
                customerServiceCheckPerformanceVo.PerformanceCommisionCheck = customerServiceCheckPerformance.PerformanceCommisionCheck;
                customerServiceCheckPerformanceVo.CheckEmpId = customerServiceCheckPerformance.CheckEmpId;
                customerServiceCheckPerformanceVo.BillId = customerServiceCheckPerformance.BillId;
                customerServiceCheckPerformanceVo.CheckBillId = customerServiceCheckPerformance.CheckBillId;
                return ResultData<CustomerServiceCheckPerformanceVo>.Success().AddData("customerServiceCheckPerformance", customerServiceCheckPerformanceVo);
            }
            catch (Exception ex)
            {
                return ResultData<CustomerServiceCheckPerformanceVo>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 修改助理提取业绩信息
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        [FxInternalAuthorize]
        public async Task<ResultData> UpdateAsync(UpdateCustomerServiceCheckPerformanceVo updateVo)
        {
            try
            {
                UpdateCustomerServiceCheckPerformanceDto updateDto = new UpdateCustomerServiceCheckPerformanceDto();
                updateDto.Id = updateVo.Id;

                updateDto.DealInfoId = updateVo.DealInfoId;
                updateDto.OrderId = updateVo.OrderId;
                updateDto.OrderFrom = updateVo.OrderFrom;
                updateDto.DealPrice = updateVo.DealPrice;
                updateDto.DealCreateDate = updateVo.DealCreateDate;
                updateDto.PerformanceType = updateVo.PerformanceType;
                updateDto.BelongEmpId = updateVo.BelongEmpId;
                updateDto.Remark = updateVo.Remark;
                updateDto.Point = updateVo.Point;
                updateDto.PerformanceCommision = updateVo.PerformanceCommision;
                updateDto.PerformanceCommisionCheck = updateVo.PerformanceCommisionCheck;
                updateDto.CheckEmpId = updateVo.CheckEmpId;
                updateDto.BillId = updateVo.BillId;
                updateDto.CheckBillId = updateVo.CheckBillId;
                await customerServiceCheckPerformanceService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 作废助理提取业绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData> DeleteAsync(string id)
        {
            try
            {
                await customerServiceCheckPerformanceService.DeleteAsync(id);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }

    }
}