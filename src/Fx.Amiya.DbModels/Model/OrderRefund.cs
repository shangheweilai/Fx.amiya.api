﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.DbModels.Model
{
    public class OrderRefund:BaseDbModel
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public string TradeId { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 审核状态(0,待审核,1审核通过,2审核失败)
        /// </summary>
        public byte CheckState { get; set; }
        /// <summary>
        /// 审核失败原因
        /// </summary>
        public string UncheckReason { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        public byte RefundState { get; set; }
        /// <summary>
        /// 退款失败原因
        /// </summary>
        public string RefundFailReason { get; set; }
        /// <summary>
        /// 是否是订单部分退款
        /// </summary>
        public bool IsPartial { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public byte ExchangeType { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 实际支付
        /// </summary>
        public decimal ActualPayAmount { get; set; }
        /// <summary>
        /// 退款发起时间
        /// </summary>
        public DateTime? RefundStartDate { get; set; }
        /// <summary>
        /// 退款回调时间
        /// </summary>
        public DateTime? RefundResultDate { get; set; }
        /// <summary>
        /// 退款交易订单号
        /// </summary>
        public string RefundTradeNo { get; set; }
    }
}
