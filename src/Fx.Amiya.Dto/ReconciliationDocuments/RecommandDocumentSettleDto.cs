﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ReconciliationDocuments
{
    public class RecommandDocumentSettleDto
    {
        public string Id { get; set; }
        public string RecommandDocumentId { get; set; }
        public bool IsCerateBill { get; set; }
        public string BelongCompany { get; set; }
        public string BelongCompany2 { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string OrderId { get; set; }
        /// <summary>
        /// 订单项目
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        public string DealInfoId { get; set; }
        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime? DealDate { get; set; }
        public int OrderFrom { get; set; }
        public string OrderFromText { get; set; }

        public decimal OrderPrice { get; set; }
        /// <summary>
        /// 对账金额
        /// </summary>
        public decimal? RecolicationPrice { get; set; }
        public bool IsOldCustomer { get; set; }

        public string IsOldCustomerText { get; set; }
        public decimal InformationPrice { get; set; }
        public decimal SystemUpdatePrice { get; set; }

        public decimal ReturnBackPrice { get; set; }
        /// <summary>
        /// 审核客服结算金额
        /// </summary>

        public decimal CustomerServiceSettlePrice { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsSettle { get; set; }
        public DateTime? SettleDate { get; set; }
        /// <summary>
        /// 归属主播账号(可空)
        /// </summary>
        public int? BelongLiveAnchorAccount { get; set; }
        public string BelongLiveAnchor { get; set; }
        /// <summary>
        /// 归属客服(可空)
        /// </summary>
        public int? BelongEmpId { get; set; }
        public string BelongEmpName { get; set; }
        /// <summary>
        /// 业绩上传人员
        /// </summary>
        public int? CreateEmpId { get; set; }
        /// <summary>
        /// 业绩上传人员名称
        /// </summary>
        public string CreateEmpName { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateByEmpName { get; set; }
        /// <summary>
        /// 账单类型(false为入账，true为出账)
        /// </summary>
        public bool AccountType { get; set; }

        public string AccountTypeText { get; set; }

        /// <summary>
        /// 出入账金额
        /// </summary>
        public decimal AccountPrice { get; set; }
        /// <summary>
        /// 薪资审核状态
        /// </summary>
        public int CompensationCheckState { get; set; }
        /// <summary>
        /// 薪资审核状态文本
        /// </summary>
        public string CompensationCheckStateText { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? CheckDate { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        public string CheckRemark { get; set; }

        /// <summary>
        /// 审核归属客服
        /// </summary>
        public int? CheckBelongEmpId { get; set; }
        public string CheckBelongEmpName { get; set; }

    }
}
