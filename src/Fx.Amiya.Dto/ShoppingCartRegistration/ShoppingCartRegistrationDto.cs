using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ShoppingCartRegistration
{
    public class ShoppingCartRegistrationDto
    {
        public string Id { get; set; }

        public DateTime RecordDate { get; set; }
        public string ContentPlatFormId { get; set; }
        public string ContentPlatFormName { get; set; }
        public int LiveAnchorId { get; set; }
        public string LiveAnchorName { get; set; }
        /// <summary>
        /// 主播微信号id
        /// </summary>
        public string LiveAnchorWeChatId { get; set; }
        public string LiveAnchorWechatNo { get; set; }
        public string CustomerNickName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 隐藏手机号
        /// </summary>
        public string HiddenPhone{get;set; }
        /// <summary>
        /// 加密手机号
        /// </summary>
        public string EncryptPhone { get; set; }
        /// <summary>
        /// 辅助号码
        /// </summary>
        public string SubPhone { get; set; }
        /// <summary>
        /// 隐藏辅助号码
        /// </summary>
        public string HiddenSubPhone { get; set; }
        /// <summary>
        /// 加密辅助号码
        /// </summary>
        public string EncryptSubPhone { get; set; }

        public decimal Price { get; set; }
        public int ConsultationType { get; set; }
        public string ConsultationTypeText { get; set; }
        public bool IsAddWeChat { get; set; }
        public bool IsWriteOff { get; set; }
        public bool IsConsultation { get; set; }
        public DateTime? ConsultationDate { get; set; }
        public bool IsReturnBackPrice { get; set; }
        public string Remark { get; set; }
        public int CreateBy { get; set; }
        public string CreateByName { get; set; }
        public int? AssignEmpId { get; set; }
        public string AssignEmpName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? RefundDate { get; set; }
        public string RefundReason { get; set; }
        public DateTime? BadReviewDate { get; set; }
        public string BadReviewReason { get; set; }
        public string BadReviewContent { get; set; }
        public bool IsReContent { get; set; }
        public string ReContent { get; set; }
        public bool IsBadReview { get; set; }

        public bool IsCreateOrder { get; set; }
        /// <summary>
        /// 派单触达
        /// </summary>
        public bool IsSendOrder { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public int EmergencyLevel { get; set; }
        public string EmergencyLevelText { get; set; }
        /// <summary>
        /// 抖音客户来源
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// 抖音客户来源文本
        /// </summary>
        public string SourceText { get; set; }
        /// <summary>
        /// 带货产品类型
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// 带货产品类型文本
        /// </summary>
        public string ProductTypeText { get; set; }
        /// <summary>
        /// 基础主播信息id
        /// </summary>
        public string BaseLiveAnchorId { get; set; }
        /// <summary>
        /// 基础主播名称
        /// </summary>
        public string BaseLiveAnchorName { get; set; }
        /// <summary>
        /// 获客方式
        /// </summary>
        public int GetCustomerType { get; set; }
        /// <summary>
        /// 获客方式（文本）
        /// </summary>
        public string GetCustomerTypeText { get; set; }
        /// <summary>
        /// 小黄车登记顾客类型
        /// </summary>
        public int ShoppingCartRegistrationCustomerType { get; set; }

        /// <summary>
        /// 小黄车登记顾客类型文本
        /// </summary>
        public string ShoppingCartRegistrationCustomerTypeText { get; set; }
        public decimal AddPrice { get; set; }
        /// <summary>
        /// 归属渠道
        /// </summary>
        public int BelongChannel { get; set; }
        /// <summary>
        /// 归属渠道名称
        /// </summary>
        public string BelongChannelName { get; set; }
        /// <summary>
        /// 线索截图
        /// </summary>
        public string CluePicture { get; set; }
        /// <summary>
        /// 加v截图
        /// </summary>
        public string AddWechatPicture { get; set; }
        /// <summary>
        /// 加v人
        /// </summary>
        public string AddWechatEmpName { get; set; }
        /// <summary>
        /// 加v人
        /// </summary>
        public int? AddWechatEmpId{ get; set; }

        /// <summary>
        /// 是否为日不落直播顾客
        /// </summary>
        public bool IsRiBuLuoLiving { get; set; }

        /// <summary>
        /// 是否为历史顾客激活
        /// </summary>
        public bool IsHistoryCustomerActive { get; set; }
        /// <summary>
        /// 激活人
        /// </summary>

        public int? ActiveEmployeeId { get; set; }

        public string ActiveEmployeeName { get; set; }
        /// <summary>
        /// 客户微信号
        /// </summary>
        public string CustomerWechatNo { get; set; }
        /// <summary>
        /// 词条来源
        /// </summary>
        public string FromTitle { get; set; }
        /// <summary>
        /// 是否重复下单
        /// </summary>
        public bool IsRepeateCreateOrder { get; set; }
    }
}
