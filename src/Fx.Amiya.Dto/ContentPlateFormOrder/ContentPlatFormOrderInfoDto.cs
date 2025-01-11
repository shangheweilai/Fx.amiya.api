using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ContentPlateFormOrder
{
    public class ContentPlatFormOrderInfoDto
    {
        public string Id { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeText { get; set; }
        public int BelongMonth { get; set; }
        public decimal AddOrderPrice { get; set; }
        public string ContentPlateformId { get; set; }
        public string ContentPlatformName { get; set; }
        /// <summary>
        /// 面诊状态
        /// </summary>
        public int ConsultationType { get; set; }

        /// <summary>
        /// 面诊状态文本
        /// </summary>
        public string ConsultationTypeText { get; set; }

        public int? LiveAnchorId { get; set; }
        public string LiveAnchorName { get; set; }
        public string LiveAnchorWeChatNo { get; set; }
        public DateTime CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string EncryptPhone { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? AppointmentHospitalId { get; set; }
        public string AppointmentHospitalName { get; set; }
        public string GoodsId { get; set; }
        public string GoodsName { get; set; }

        public string GoodsDepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ThumbPictureUrl { get; set; }
        /// <summary>
        /// 主派咨询内容
        /// </summary>
        public string ConsultingContent { get; set; }
        /// <summary>
        /// 次派咨询内容
        /// </summary>
        public string ConsultingContent2 { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusText { get; set; }
        public bool IsToHospital { get; set; }

        /// <summary>
        /// 到院时间（最新）
        /// </summary>
        public DateTime? ToHospitalDate { get; set; }
        public int ToHospitalType { get; set; }
        public string ToHospitalTypeText { get; set; }
        /// <summary>
        /// 最终成交医院id
        /// </summary>
        public int? LastDealHospitalId { get; set; }
        public string LastDealHospital { get; set; }
        public decimal? DepositAmount { get; set; }
        public decimal? DealAmount { get; set; }
        public DateTime? DealDate { get; set; }
        public string UnDealReason { get; set; }
        public string LateProjectStage { get; set; }
        public string Remark { get; set; }
        public int? CheckState { get; set; }
        public string CheckStateText { get; set; }

        public decimal? CheckPrice { get; set; }
        public DateTime? CheckDate { get; set; }
        public string SendHospital { get; set; }
        public string Sender { get; set; }
        public DateTime? SendDate { get; set; }
        public decimal? SettlePrice { get; set; }
        public bool IsReturnBackPrice { get; set; }

        public decimal? ReturnBackPrice { get; set; }
        public DateTime? ReturnBackDate { get; set; }

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

        public int? CheckBy { get; set; }
        public string CheckByName { get; set; }
        public string CheckRemark { get; set; }
        public int? BelongEmpId { get; set; }
        public string BelongEmpName { get; set; }
        /// <summary>
        /// 订单来源（1：面诊卡，2：非面诊卡）
        /// </summary>
        public int? OrderSource { get; set; }
        /// <summary>
        /// 订单来源文本
        /// </summary>

        public string OrderSourceText { get; set; }

        /// <summary>
        /// 未派单原因
        /// </summary>
        public string UnSendReason { get; set; }

        /// <summary>
        /// 院方接诊人员
        /// </summary>
        public string AcceptConsulting { get; set; }

        /// <summary>
        /// 三方订单号
        /// </summary>
        public string OtherContentPlatFormOrderId { get; set; }
        /// <summary>
        /// 是否是重单深度订单
        /// </summary>
        public bool IsRepeatProfundityOrder { get; set; }
        /// <summary>
        /// 是否为辅助订单
        /// </summary>
        public bool IsSupportOrder { get; set; }

        /// <summary>
        /// 辅助客服
        /// </summary>
        public int SupportEmpId { get; set; }
        /// <summary>
        /// 辅助客服名称
        /// </summary>
        public string SupportEmpName { get; set; }
        /// <summary>
        /// 审核客服业绩金额
        /// </summary>
        public decimal? CustomerServiceSettlePrice { get; set; }
        /// <summary>
        /// 获客方式
        /// </summary>
        public int GetCustomerType { get; set; }
        /// <summary>
        /// 获客方式文本
        /// </summary>
        public string GetCustomerTypeText { get; set; }
        /// <summary>
        /// 归属部门
        /// </summary>
        public int BelongChannel { get; set; }
        /// <summary>
        /// 归属部门名称
        /// </summary>
        public string BelongChannelText { get; set; }

        /// <summary>
        /// 是否为日不落直播顾客
        /// </summary>
        public bool IsRiBuLuoLiving { get; set; }

        /// <summary>
        /// 归属公司
        /// </summary>
        public string OrderBelongCompany { get; set; }
    }
}
