﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.OrderReport
{
    public class ContentPlatFormOrderDealInfoReportVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public string Id { get; set; }

        /// <summary>
        /// 登记时间
        /// </summary>
        [Description("登记时间")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public string ContentPlatFormOrderId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        [Description("下单时间")]
        public DateTime OrderCreateDate { get; set; }

        /// <summary>
        /// 派单时间
        /// </summary>
        [Description("派单时间")]
        public DateTime SendOrderDate { get; set; }


        /// <summary>
        /// 平台
        /// </summary>
        [Description("平台")]
        public string ContentPlatFormName { get; set; }
        /// <summary>
        /// 主播
        /// </summary>
        [Description("主播")]
        public string LiveAnchorName { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        [Description("项目")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 客户昵称
        /// </summary>
        [Description("客户昵称")]
        public string CustomerNickName { get; set; }
        /// <summary>
        /// 客户手机号
        /// </summary>
        [Description("客户手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 是否到院
        /// </summary>
        [Description("是否到院")]
        public string IsToHospital { get; set; }


        /// <summary>
        /// 到院类型
        /// </summary>
        [Description("到院类型")]
        public string ToHospitalTypeText { get; set; }

        /// <summary>
        /// 到院时间
        /// </summary>
        [Description("到院时间")]
        public DateTime? TohospitalDate { get; set; }

        /// <summary>
        /// 成交医院
        /// </summary>
        [Description("成交医院")]
        public string LastDealHospital { get; set; }

        /// <summary>
        /// 是否成交
        /// </summary>
        [Description("是否成交")]
        public string IsDeal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        [Description("成交金额")]
        public decimal Price { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        [Description("成交时间")]
        public DateTime? DealDate { get; set; }

        /// <summary>
        /// 三方订单号
        /// </summary>
        [Description("三方订单号")]
        public string OtherOrderId { get; set; }

        /// <summary>
        /// 新老客业绩
        /// </summary>
        [Description("新老客业绩")]
        public string IsOldCustomer { get; set; }

        /// <summary>
        /// 是否陪诊
        /// </summary>
        [Description("是否陪诊")]
        public string IsAcompanying { get; set; }

        /// <summary>
        /// 佣金比例
        /// </summary>
        [Description("佣金比例")]
        public decimal CommissionRatio { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [Description("审核状态")]
        public string CheckStateText { get; set; }
        /// <summary>
        /// 审核金额
        /// </summary>
        [Description("审核金额")]
        public decimal? CheckPrice { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        [Description("审核日期")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        [Description("结算金额")]
        public decimal? SettlePrice { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        [Description("审核人")]
        public string CheckBy { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [Description("审核备注")]
        public string CheckRemark { get; set; }
        /// <summary>
        /// 是否回款
        /// </summary>
        [Description("是否回款")]

        public string IsReturnBackPrice { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        [Description("回款金额")]

        public decimal? ReturnBackPrice { get; set; }
        /// <summary>
        /// 回款日期
        /// </summary>
        [Description("回款日期")]
        public DateTime? ReturnBackDate { get; set; }
        /// <summary>
        /// 面诊员
        /// </summary>
        [Description("面诊员")]
        public string ConsultationEmpName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("跟进人员")]
        public string CreateByEmpName { get; set; }
    }
}
