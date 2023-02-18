﻿using Fx.Amiya.Background.Api.Vo.FinancialBorad;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 财务看板板块数据接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class FinancialboardController : ControllerBase
    {
        private readonly IBillService billService;
        private readonly IFinancialboardService financialboardSerice;

        public FinancialboardController(IBillService billService, IFinancialboardService financialboardSerice)
        {
            this.billService = billService;
            this.financialboardSerice = financialboardSerice;
        }
        #region 财务看板展示

        /// <summary>
        /// 医院维度
        /// </summary>
        /// <returns></returns>
        [HttpGet("hospitalBoard")]
        public async Task<ResultData<FxPageInfo<FinancialHospitalBoardVo>>> GetHospitalBoard(int? hospitalId,DateTime? startDate,DateTime? endDate,int pageNum,int pageSize) {
            var data=await billService.FinancialHospitalBoardDataAsync(hospitalId,startDate,endDate,pageNum,pageSize);
            FxPageInfo<FinancialHospitalBoardVo> fxPageInfo = new FxPageInfo<FinancialHospitalBoardVo>();
            fxPageInfo.TotalCount = data.TotalCount;
            fxPageInfo.List = data.List.Select(e=>new FinancialHospitalBoardVo { 
                HospitalName=e.HospitalName,
                DealPrice=e.DealPrice,
                TotalServicePrice=e.TotalServicePrice,
                NoIncludeTaxPrice=e.NoIncludeTaxPrice,
                InformationPrice=e.InformationPrice,
                SystemUsePrice=e.SystemUsePrice,
                ReturnBackPrice=e.ReturnBackPrice,
                UnReturnBackPrice=e.UnReturnBackPrice
            });
            return ResultData<FxPageInfo<FinancialHospitalBoardVo>>.Success().AddData("data",fxPageInfo);
        }

        /// <summary>
        /// 子公司维度
        /// </summary>
        /// <returns></returns>
        [HttpGet("subsidiaryBoard")]
        public async Task<ResultData<FxPageInfo<FinancialHospitalBoardVo>>> GetSubsidiaryBoard(string companyId, int? hospitalId, DateTime? startDate, DateTime? endDate, int pageNum, int pageSize) {
            var data = await billService.FinancialCompanyBoardDataAsync(companyId,hospitalId, startDate, endDate, pageNum, pageSize);
            FxPageInfo<FinancialHospitalBoardVo> fxPageInfo = new FxPageInfo<FinancialHospitalBoardVo>();
            fxPageInfo.TotalCount = data.TotalCount;
            fxPageInfo.List = data.List.Select(e => new FinancialHospitalBoardVo
            {
                CompanyName=e.CompanyName,
                HospitalName = e.HospitalName,
                DealPrice = e.DealPrice,
                TotalServicePrice = e.TotalServicePrice,
                NoIncludeTaxPrice = e.NoIncludeTaxPrice,
                InformationPrice = e.InformationPrice,
                SystemUsePrice = e.SystemUsePrice,
                ReturnBackPrice = e.ReturnBackPrice,
                UnReturnBackPrice = e.UnReturnBackPrice
            });
            return ResultData<FxPageInfo<FinancialHospitalBoardVo>>.Success().AddData("data", fxPageInfo);
        }

        #endregion

        #region 产出板块

        /// <summary>
        /// 主播业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="liveAnchorId">主播id</param>
        /// <returns></returns>
        [HttpGet("liveAnchorBoardData")]
        public async Task<ResultData<List<LiveAnchorBoardVo>>> GetLiveAnchorBoard(DateTime? startDate,DateTime? endDate,int? liveAnchorId) {
            var dataList =await financialboardSerice.GetBoardLiveAnchorDataAsync(startDate,endDate,liveAnchorId);
            var resultList= dataList.Select(e => new LiveAnchorBoardVo {
                CompanyName=e.CompanyName,
                DealPrice=e.DealPrice,
                TotalServicePrice=e.TotalServicePrice,
                NewCustomerPrice=e.NewCustomerPrice,
                NewCustomerServicePrice=e.NewCustomerServicePrice,
                OldCustomerPrice=e.OldCustomerPrice,
                OldCustomerServicePrice=e.OldCustomerServicePrice,
                LiveAnchorName=e.LiveAnchorName
            }).OrderBy(e=>e.DealPrice).ToList();
            return ResultData<List<LiveAnchorBoardVo>>.Success().AddData("data", resultList);
        }
        
        /// <summary>
        /// 客服业绩
        /// </summary>
        /// <returns></returns>
        [HttpGet("customerServiceBoardData")]
        public async Task<ResultData<List<CustomerServiceBoardVo>>> GetCustomerServiceBoard(DateTime? startDate, DateTime? endDate) {
            var dataList = await financialboardSerice.GetBoardCustomerServiceDataAsync(startDate, endDate, null);
            var resultList = dataList.Select(e => new CustomerServiceBoardVo
            {
                CustomerServiceName = e.CustomerServiceName,
                DealPrice = e.DealPrice,
                TotalServicePrice = e.TotalServicePrice,
                NewCustomerPrice = e.NewCustomerPrice,
                NewCustomerServicePrice = e.NewCustomerServicePrice,
                OldCustomerPrice = e.OldCustomerPrice,
                OldCustomerServicePrice = e.OldCustomerServicePrice,
            }).OrderByDescending(e=>e.DealPrice).ToList();
            return ResultData<List<CustomerServiceBoardVo>>.Success().AddData("data", resultList);
        }

        #endregion



    }
}