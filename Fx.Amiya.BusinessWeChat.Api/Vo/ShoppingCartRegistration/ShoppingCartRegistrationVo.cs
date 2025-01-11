using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.BusinessWeChat.Api.Vo.ShoppingCartRegistration
{
    public class ShoppingCartRegistrationVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RecordDate { get; set; }
        public string ContentPlatFormId { get; set; }
        /// <summary>
        /// 来源渠道
        /// </summary>
        public string ContentPlatFormName { get; set; }
        public int LiveAnchorId { get; set; }
        /// <summary>
        /// 主播IP
        /// </summary>
        public string LiveAnchorName { get; set; }
        /// <summary>
        /// 主播微信号
        /// </summary>
        public string LiveAnchorWechatNo { get; set; }

        /// <summary>
        /// 主播微信号id
        /// </summary>
        public string LiveAnchorWeChatId { get; set; }
        /// <summary>
        /// 抖音昵称
        /// </summary>
        public string CustomerNickName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 隐藏手机号
        /// </summary>
        public string HiddenPhone { get; set; }
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
        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 面诊方式
        /// </summary>
        public int ConsultationType { get; set; }

        /// <summary>
        /// 面诊方式文本
        /// </summary>
        public string ConsultationTypeText { get; set; }

        /// <summary>
        /// 录单触达
        /// </summary>
        public bool IsCreateOrder { get; set; }
        /// <summary>
        /// 派单触达
        /// </summary>
        public bool IsSendOrder { get; set; }
        /// <summary>
        /// 是否加V
        /// </summary>
        public bool IsAddWeChat { get; set; }
        /// <summary>
        /// 是否核销
        /// </summary>
        public bool IsWriteOff { get; set; }
        /// <summary>
        /// 是否面诊
        /// </summary>
        public bool IsConsultation { get; set; }
        /// <summary>
        /// 面诊时间
        /// </summary>
        public DateTime? ConsultationDate { get; set; }
        /// <summary>
        /// 是否退款
        /// </summary>
        public bool IsReturnBackPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人员id
        /// </summary>
        public int CreateByEmpId { get; set; }
        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 指派接诊人员id
        /// </summary>
        public int? AssignEmpId { get; set; }
        /// <summary>
        /// 指派接诊人员
        /// </summary>
        public string AssignEmpName { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundDate { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string RefundReason { get; set; }
        /// <summary>
        /// 差评时间
        /// </summary>
        public DateTime? BadReviewDate { get; set; }
        /// <summary>
        /// 差评原因
        /// </summary>
        public string BadReviewReason { get; set; }
        /// <summary>
        /// 差评内容
        /// </summary>
        public string BadReviewContent { get; set; }
        /// <summary>
        /// 是否追评
        /// </summary>
        public bool IsReContent { get; set; }
        /// <summary>
        /// 追评内容
        /// </summary>
        public string ReContent { get; set; }
        /// <summary>
        /// 是否差评
        /// </summary>
        public bool IsBadReview { get; set; }
        /// <summary>
        /// 重要程度
        /// </summary>
        public int EmergencyLevel { get; set; }
        /// <summary>
        /// 重要程度名称
        /// </summary>
        public string EmergencyLevelText { get; set; }
        /// <summary>
        /// 抖音新增客户来源
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// 抖音新增客户来源文本
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
        /// 获客方式(文本)
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
        /// 是否为日不落直播顾客
        /// </summary>
        public bool IsRiBuLuoLiving { get; set; }
        /// <summary>
        /// 客户微信号
        /// </summary>
        public string CustomerWechatNo { get; set; }
        /// <summary>
        /// 词条来源
        /// </summary>
        public string FromTitle { get; set; }
        /// <summary>
        /// 归属公司
        /// </summary>
        public string BelongCompany { get; set; }
    }
}
