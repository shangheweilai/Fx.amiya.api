﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.LivingTakeGoodsOrder.Result
{
    public class LivingTakeGoodsOrderVo
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public string GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单状态文本
        /// </summary>
        public string OrderStatusText { get; set; }
        /// <summary>
        /// 主播名称
        /// </summary>
        public string LiveanchorName { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal DealPrice { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int GoodsCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}