using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ShoppingCartRegistration
{
    public class AddShoppingCartRegistrationVo
    {
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// 来源渠道id
        /// </summary>
        public string ContentPlatFormId { get; set; }
        /// <summary>
        /// 主播ID
        /// </summary>
        public int LiveAnchorId { get; set; }
        /// <summary>
        /// 主播微信号
        /// </summary>
        public string LiveAnchorWechatNo { get; set; }
        /// <summary>
        /// 客户抖音昵称
        /// </summary>
        public string CustomerNickName { get; set; }
        /// <summary>
        /// 客户手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 辅助号码
        /// </summary>
        public string SubPhone { get; set; }
        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 面诊方式
        /// </summary>
        public int ConsultationType { get; set; }
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
        /// 是否派单
        /// </summary>
        public bool SendOrder { get; set; }
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
        /// 指派人员
        /// </summary>
        public int? AssignEmpId { get; set; }
        /// <summary>
        /// 是否差评
        /// </summary>
        public bool IsBadReview { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public int EmergencyLevel { get; set; }
        /// <summary>
        /// 抖音客户来源
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 带货产品类型
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// 获客方式
        /// </summary>
        public int GetCustomerType { get; set; }

        /// <summary>
        /// 小黄车登记顾客类型
        /// </summary>
        public int ShoppingCartRegistrationCustomerType { get; set; }
        /// <summary>
        /// 归属渠道
        /// </summary>
        public int BelongChannel { get; set; }
        /// <summary>
        /// 线索截图
        /// </summary>
        public string CluePicture { get; set; }
        /// <summary>
        /// 加v截图
        /// </summary>
        public string AddWechatPicture { get; set; }

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
        /// 是否重复下单
        /// </summary>
        public bool IsRepeateCreateOrder { get; set; }

        /// <summary>
        /// 归属公司
        /// </summary>
        public int BelongCompany { get; set; }
    }
}
