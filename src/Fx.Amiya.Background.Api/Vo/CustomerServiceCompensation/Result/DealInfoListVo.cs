using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.CustomerServiceCompensation.Result
{
    public class DealInfoListVo
    {
        /// <summary>
        /// 成交情况id
        /// </summary>
        public string DealId { get; set; }
        /// <summary>
        /// 内容平台订单id
        /// </summary>
        public string ContentPaltformOrderId { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal DealPrice { get; set; }

        /// <summary>
        /// 补单前金额
        /// </summary>
        public decimal BeforeReplenishmentPrice { get; set; }

        /// <summary>
        /// 补单前是否生成薪资
        /// </summary>
        public bool BeforeReplenishmentIsCreateBill { get; set; }
        /// <summary>
        /// 确认成交金额
        /// </summary>

        public decimal ConfirmDealPrice { get; set; }
        /// <summary>
        /// 业绩类型
        /// </summary>
        public int PerformanceType { get; set; }
        /// <summary>
        /// 业绩类型文本
        /// </summary>
        public string PerformanceTypeText { get; set; }
        /// <summary>
        /// 上传人
        /// </summary>
        public int CreateById { get; set; }
        /// <summary>
        /// 上传人id
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 是否成交
        /// </summary>
        public bool IsDeal { get; set; }
        /// <summary>
        /// 是否时辅助订单
        /// </summary>
        public bool IsSupportOrder { get; set; }
        /// <summary>
        /// 归属客服
        /// </summary>
        public int BelongEmpId { get; set; }
        /// <summary>
        /// 归属客服名称
        /// </summary>
        public string BelongEmpName { get; set; }
        /// <summary>
        /// 辅助客服
        /// </summary>
        public int SupportEmpId { get; set; }
        /// <summary>
        /// 辅助客服名
        /// </summary>
        public string SupportEmpName { get; set; }

        /// <summary>
        /// 提取状态
        /// </summary>
        public string IsCheckPerformance { get; set; }

        /// <summary>
        /// 主播名称
        /// </summary>
        public string LiveAnchorName { get; set; }
    }
}
