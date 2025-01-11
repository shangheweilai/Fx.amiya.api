using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ContentPlateFormOrder
{
    /// <summary>
    /// 内容平台订单列表
    /// </summary>
    public class ContentPlatFormOrderInfoVo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        public string Id { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        [Description("预约时间")]
        public string AppointmentDate { get; set; }
        /// <summary>
        /// 归属客服
        /// </summary>
        [Description("归属客服")]
        public string BelongEmpName { get; set; }
        /// <summary>
        /// 客户昵称
        /// </summary>
        [Description("客户昵称")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 加密电话
        /// </summary>
        [Description("加密电话")]
        public string EncryptPhone { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Description("城市")]
        public string City { get; set; }

        /// <summary>
        /// 新/老客业绩
        /// </summary>
        [Description("新/老客业绩")]
        public string IsOldCustomer { get; set; }
        /// <summary>
        /// 下单金额
        /// </summary>
        [Description("下单金额")]
        public decimal AddOrderPrice { get; set; }
        /// <summary>
        /// 预约医院
        /// </summary>
        [Description("预约医院")]
        public string AppointmentHospitalName { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        [Description("项目")]
        public string GoodsName { get; set; }
        /// <summary>
        /// 项目图片
        /// </summary>
        [Description("项目图片")]
        public string ThumbPictureUrl { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        [Description("科室")]
        public string DepartmentName { get; set; }
        /// <summary>
        /// 主派咨询内容
        /// </summary>
        [Description("主派咨询内容")]
        public string ConsultingContent { get; set; }
        /// <summary>
        /// 次派咨询内容
        /// </summary>
        [Description("次派咨询内容")]
        public string ConsultingContent2 { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        [Description("下单时间")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 下单平台
        /// </summary>
        [Description("下单平台")]
        public string ContentPlatformName { get; set; }
        /// <summary>
        /// 主播ip账号
        /// </summary>
        [Description("主播ip账号")]
        public string LiveAnchorName { get; set; }
        /// <summary>
        /// 主播微信号
        /// </summary>
        [Description("主播微信号")]
        public string LiveAnchorWeChatNo { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Description("订单类型")]
        public string OrderTypeText { get; set; }
        /// <summary>
        /// 归属月份
        /// </summary>
        [Description("归属月份")]

        public int BelongMonth { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [Description("订单状态")]
        public string OrderStatusText { get; set; }

        /// <summary>
        /// 三方订单号
        /// </summary>
        [Description("抖店订单号")]
        public string OtherContentPlatFormOrderId { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        [Description("订单来源")]
        public string OrderSourceText { get; set; }
        /// <summary>
        /// 派单医院
        /// </summary>
        [Description("派单医院")]
        public string SendHospital { get; set; }
        /// <summary>
        /// 派单人
        /// </summary>
        [Description("派单人")]
        public string Sender { get; set; }
        /// <summary>
        /// 派单时间
        /// </summary>
        [Description("派单时间")]
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// 面诊类型
        /// </summary>
        [Description("面诊类型")]
        public string ConsultationType { get; set; }
        /// <summary>
        /// 院方接诊人员
        /// </summary>
        [Description("接诊咨询")]
        public string AcceptConsulting { get; set; }
        /// <summary>
        /// 定金金额
        /// </summary>
        [Description("定金金额")]
        public decimal? DepositAmount { get; set; }
        /// <summary>
        /// 是否到院
        /// </summary>
        [Description("是否到院")]
        public bool IsToHospital { get; set; }

        /// <summary>
        /// 最近到院时间
        /// </summary>
        [Description("最近到院时间")]
        public DateTime? ToHospitalDate { get; set; }
        /// <summary>
        /// 最终成交医院
        /// </summary>
        [Description("最终成交医院")]
        public string LastDealHospital { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        [Description("成交金额")]
        public decimal? DealAmount { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        [Description("成交时间")]
        public DateTime? DealDate { get; set; }
        /// <summary>
        /// 未成交原因
        /// </summary>
        [Description("未成交原因")]
        public string UnDealReason { get; set; }
        /// <summary>
        /// 后期项目铺垫
        /// </summary>
        [Description("后期项目铺垫")]
        public string LateProjectStage { get; set; }

        /// <summary>
        /// 未派单原因
        /// </summary>
        [Description("未派单原因")]
        public string UnSendReason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }

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
        /// 结算金额
        /// </summary>
        [Description("结算金额")]
        public decimal? SettlePrice { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        [Description("审核日期")]
        public DateTime? CheckDate { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Description("审核人")]

        public string CheckByName { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [Description("审核备注")]
        public string CheckRemark { get; set; }

        /// <summary>
        /// 是否回款
        /// </summary>
        [Description("是否回款")]
        public bool IsReturnBackPrice { get; set; }

        /// <summary>
        /// 回款金额
        /// </summary>
        [Description("回款金额")]
        public decimal? ReturnBackPrice { get; set; }
        /// <summary>
        /// 回款时间
        /// </summary>
        [Description("回款时间")]
        public DateTime? ReturnBackDate { get; set; }
        /// <summary>
        /// 是否是重单深度订单
        /// </summary>
        [Description("是否是重单深度订单")]
        public bool IsRepeatProfundityOrder { get; set; }

        /// <summary>
        /// 是否为辅助订单
        /// </summary>
        [Description("是否为辅助订单")]
        public bool IsSupportOrder { get; set; }
        /// <summary>
        /// 辅助客服名称
        /// </summary>
        [Description("辅助客服名称")]
        public string SupportEmpName { get; set; }
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
