﻿using Fx.Amiya.Background.Api.Vo.CustomerHospitalConsume;
using Fx.Amiya.Background.Api.Vo.CustomerIntegralOrderRefund;
using Fx.Amiya.Background.Api.Vo.OrderRefund;
using Fx.Amiya.Dto.CustomerIntegralOrderRefunds;
using Fx.Amiya.Dto.OrderRefund;
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
    [Route("api/[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class OrderRefundController : ControllerBase
    {
        private IOrderRefundService orderRefundService;
        private IHttpContextAccessor httpContextAccessor;

        public OrderRefundController(IOrderRefundService orderRefundService, IHttpContextAccessor httpContextAccessor)
        {
            this.orderRefundService = orderRefundService;
            this.httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 获取退款订单列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="checkState"></param>
        /// <param name="refundState"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        public async Task<ResultData<FxPageInfo<OrderRefundVo>>> ListWithPage(string keywords, byte? checkState, byte? refundState, int pageNum, int pageSize) {
            var list =await orderRefundService.GetListAsync( keywords,  checkState, refundState,pageNum, pageSize);
            FxPageInfo<OrderRefundVo> fxPageInfo = new FxPageInfo<OrderRefundVo>();
            fxPageInfo.TotalCount = list.TotalCount;
            fxPageInfo.List = list.List.Select(e=>new OrderRefundVo {  
                Id=e.Id,
                TradeId = e.TradeId,
                Remark = e.Remark,
                CheckState = e.CheckState,
                CheckStateText = e.CheckStateText,
                UncheckReason = e.UncheckReason,
                RefundState = e.RefundState,
                RefundStateText = e.RefundStateText,
                RefundFailReason = e.RefundFailReason,
                IsPartial = e.IsPartial,
                ExchangeType = e.ExchangeType,
                ExchageTypeText = e.ExchageTypeText,
                RefundAmount = e.RefundAmount,
                ActualPayAmount = e.ActualPayAmount,
                PayDate = e.PayDate,
                CheckByName=e.CheckByName,
                CreateDate = e.CreateDate,
                UpdateDate = e.UpdateDate
            });
            return ResultData<FxPageInfo<OrderRefundVo>>.Success().AddData("list",fxPageInfo);
        }
        /// <summary>
        /// 订单退款审核
        /// </summary>
        /// <param name="checkDto">输入参数</param>
        /// <returns></returns>
        [HttpPost("check")]
        public async Task<ResultData> IntegrationPayRefundAsync(OrderRefundCheckVo checkDto)
        {
            var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            OrderRefundCheckDto orderRefundCheck = new OrderRefundCheckDto();
            orderRefundCheck.CheckBy = employeeId;
            orderRefundCheck.Id = checkDto.Id;
            orderRefundCheck.UnCheckReason = checkDto.UnCheckReason;
            orderRefundCheck.CheckState = checkDto.CheckState;
            await orderRefundService.CheckAsync(orderRefundCheck);
            return ResultData.Success();
        }
        /// <summary>
        /// 获取审核情况（下拉框使用）
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCheckStateList")]
        [FxInternalAuthorize]
        public ResultData<List<CheckStateTypeVo>> GetCheckStateList()
        {
            var orderNatures = from d in orderRefundService.GetCheckStateType()
                               select new CheckStateTypeVo
                               {
                                   Id = d.Id,
                                   Name = d.Name
                               };
            return ResultData<List<CheckStateTypeVo>>.Success().AddData("checkStateList", orderNatures.ToList());
        }
        /// <summary>
        /// 获取审核情况（下拉框使用）
        /// </summary>
        /// <returns></returns>
        [HttpGet("getRefundStateList")]
        [FxInternalAuthorize]
        public ResultData<List<CheckStateTypeVo>> GetRefundStateList()
        {
            var orderNatures = from d in orderRefundService.GetRefundStateType()
                               select new CheckStateTypeVo
                               {
                                   Id = d.Id,
                                   Name = d.Name
                               };
            return ResultData<List<CheckStateTypeVo>>.Success().AddData("refundStateList", orderNatures.ToList());
        }
    }
}