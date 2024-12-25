using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ContentPlatFormOrderSend
{
    /// <summary>
    /// 内容平台派单处理情况
    /// </summary>
    public class ContentPlatFormOrderDealInfoDto
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
        /// 客户手机号
        /// </summary>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderCreateDate { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 派单时间
        /// </summary>
        public DateTime? SendDate { get; set; }


        /// <summary>
        /// 平台
        public string ContentPlatFormName { get; set; }
        /// <summary>
        /// 主播
        /// </summary>
        public string LiveAnchorName { get; set; }
        /// <summary>
        /// 主播id
        /// </summary>
        public int LiveAnchorId { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 客户昵称
        /// </summary>
        public string CustomerNickName { get; set; }

        /// <summary>
        /// 客户手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 加密手机号
        /// </summary>
        public string EncryptPhone { get; set; }

        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal? AddOrderPrice { get; set; }

        /// <summary>
        /// 是否到院
        /// </summary>
        public bool IsToHospital { get; set; }

        /// <summary>
        /// 到院类型
        /// </summary>
        public int ToHospitalType { get; set; }

        /// <summary>
        /// 到院类型文本
        /// </summary>
        public string ToHospitalTypeText { get; set; }

        /// <summary>
        /// 到院时间
        /// </summary>
        public DateTime? ToHospitalDate { get; set; }

        /// <summary>
        /// 是否成交
        /// </summary>
        public bool IsDeal { get; set; }
        /// <summary>
        /// 最终成交医院id
        /// </summary>
        public int? LastDealHospitalId { get; set; }
        public string LastDealHospital { get; set; }

        /// <summary>
        /// 截图
        /// </summary>
        public string DealPicture { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime? DealDate { get; set; }
        /// <summary>
        /// 业绩类型
        /// </summary>
        public int DealPerformanceType { get; set; }
        /// <summary>
        /// 业绩类型文本
        /// </summary>
        public string DealPerformanceTypeText { get; set; }

        /// <summary>
        /// 三方订单号
        /// </summary>
        public string OtherAppOrderId { get; set; }

        /// <summary>
        /// 上一条成交编号
        /// </summary>
        public string LastDealInfoId { get; set; }

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
        /// 审核状态
        /// </summary>
        public int? CheckState { get; set; }

        /// <summary>
        /// 审核状态文本
        /// </summary>
        public string CheckStateText { get; set; }
        /// <summary>
        /// 审核金额
        /// </summary>

        public decimal? CheckPrice { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? CheckDate { get; set; }

        /// <summary>
        /// 信息服务费
        /// </summary>
        public decimal? InformationPrice { get; set; }
        /// <summary>
        /// 系统使用费
        /// </summary>
        public decimal? SystemUpdatePrice { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>

        public decimal? SettlePrice { get; set; }
        /// <summary>
        /// 审核客服结算金额
        /// </summary>

        public decimal CustomerServiceSettlePrice { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int? CheckBy { get; set; }

        /// <summary>
        /// 审核人名称
        /// </summary>
        public string CheckByEmpName { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string CheckRemark { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsCreateBill { get; set; }
        /// <summary>
        /// 开票公司id
        /// </summary>
        public string BelongCompany { get; set; }
        /// <summary>
        /// 开票公司名称
        /// </summary>
        public string BelongCompanyName { get; set; }
        /// <summary>
        /// 是否回款
        /// </summary>

        public bool IsReturnBackPrice { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>

        public decimal? ReturnBackPrice { get; set; }
        /// <summary>
        /// 回款日期
        /// </summary>
        public DateTime? ReturnBackDate { get; set; }

        /// <summary>
        /// 面诊状态id
        /// </summary>
        public int? ConsultationType { get; set; }
        /// <summary>
        /// 面诊状态文本
        /// </summary>
        public string ConsultationTypeText { get; set; }

        /// <summary>
        /// 创建人（0为医院）
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateByEmpName { get; set; }
        /// <summary>
        /// 邀约凭证
        /// </summary>
        public List<string> InvitationDocuments { get; set; }

        /// <summary>
        /// 对账单id
        /// </summary>
        public string ReconciliationDocumentsId { get; set; }


        /// <summary>
        /// 归属主播
        /// </summary>
        public string BelongLiveAnchor { get; set; }
        /// <summary>
        /// 是否是重单可深度订单
        /// </summary>
        public bool IsRepeatProfundityOrder { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int? ConsumptionType { get; set; }
        /// <summary>
        /// 消费类型文本
        /// </summary>
        public string ConsumptionTypeText { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string ContentPlatFormId { get; set; }
        /// <summary>
        /// 基础主播id
        /// </summary>
        public string BaseLiveAnchorId { get; set; }
        /// <summary>
        /// 基础主播名称
        /// </summary>
        public string BaseLiveAnchorName { get; set; }
        /// <summary>
        /// 归属客服id
        /// </summary>
        public int BelongEmployeeId { get; set; }

        /// <summary>
        /// 是否为辅助订单
        /// </summary>
        public bool IsSupportOrder { get; set; }

        /// <summary>
        /// 辅助客服id
        /// </summary>
        public int SupportEmpId { get; set; }
    }
}
