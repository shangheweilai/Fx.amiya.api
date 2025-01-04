using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.DbModels.Model
{
    public class ContentPlatformOrderDealInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string ContentPlatFormOrderId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否到院
        /// </summary>
        public bool IsToHospital { get; set; }

        /// <summary>
        /// 到院类型
        /// </summary>
        public int ToHospitalType { get; set; }

        /// <summary>
        /// 到院时间
        /// </summary>
        public DateTime? ToHospitalDate { get; set; }
        /// <summary>
        /// 最终成交医院id
        /// </summary>
        public int? LastDealHospitalId { get; set; }

        /// <summary>
        /// 是否成交
        /// </summary>
        public bool IsDeal { get; set; }


        /// <summary>
        /// 截图
        /// </summary>
        public string DealPicture { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 业绩类型
        /// </summary>
        public int DealPerformanceType { get; set; }



        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime? DealDate { get; set; }

        /// <summary>
        /// 三方订单号
        /// </summary>
        public string OtherAppOrderId { get; set; }

        /// <summary>
        /// 新客/老客
        /// </summary>
        public bool IsOldCustomer { get; set; }

        /// <summary>
        /// 是否陪诊
        /// </summary>
        public bool IsAcompanying { get; set; }

        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal CommissionRatio { get; set; }

        /// <summary>
        /// 创建人（0为医院）
        /// </summary>
        public int CreateBy { get; set; }

        public int? CheckState { get; set; }

        public decimal? CheckPrice { get; set; }
        public DateTime? CheckDate { get; set; }

        /// <summary>
        /// 信息服务费
        /// </summary>
        public decimal? InformationPrice { get; set; }
        /// <summary>
        /// 系统使用费
        /// </summary>
        public decimal? SystemUpdatePrice { get; set; }
        public decimal? SettlePrice { get; set; }
        /// <summary>
        /// 审核客服结算金额
        /// </summary>

        public decimal? CustomerServiceSettlePrice { get; set; }
        public int? CheckBy { get; set; }
        public string CheckRemark { get; set; }

        public bool IsReturnBackPrice { get; set; }

        public decimal? ReturnBackPrice { get; set; }
        public DateTime? ReturnBackDate { get; set; }

        public string ReconciliationDocumentsId { get; set; }
        /// <summary>
        /// 是否重单可深度
        /// </summary>
        public bool IsRepeatProfundityOrder { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsCreateBill { get; set; }
        /// <summary>
        /// 开票公司id
        /// </summary>
        public string BelongCompany { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int? ConsumptionType { get; set; }

        public ContentPlatformOrder ContentPlatFormOrder { get; set; }
        /// <summary>
        /// 上一条成交情况id（当业绩类型为“助理补单时有值）
        /// </summary>
        public string LastDealInfoId { get; set; }

        /// <summary>
        /// 上一条成交单创建时间
        /// </summary>
        public DateTime? LastDealInfoCreateDate { get; set; }

        /// <summary>
        /// 补单时间
        /// </summary>
        public DateTime? ReplenishmentCreateDate { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get; set; }
    }
}
