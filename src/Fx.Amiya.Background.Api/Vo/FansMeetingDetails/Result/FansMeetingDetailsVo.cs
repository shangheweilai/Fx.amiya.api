using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.FansMeetingDetails.Result
{
    public class FansMeetingDetailsVo:BaseVo
    {
        /// <summary>
        /// 粉丝见面会id
        /// </summary>
        public string FansMeetingId { get; set; }
        /// <summary>
        /// 粉丝见面会名称
        /// </summary>

        public string FansMeetingName { get; set; }
        /// <summary>
        /// 订单号（可空）
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// 详细时间
        /// </summary>
        public string AppointmentDetailsDate { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 客户质量（0：一般；1：优质）
        /// </summary>
        public int CustomerQuantity { get; set; }

        /// <summary>
        /// 客户质量名称
        /// </summary>
        public string CustomerQuantityText { get; set; }
        /// <summary>
        /// 新/老客
        /// </summary>
        public bool IsOldCustomer { get; set; }
        /// <summary>
        /// 啊美雅助理id
        /// </summary>
        public int AmiyaConsulationId { get; set; }
        /// <summary>
        /// 啊美雅助理名称
        /// </summary>
        public string AmiyaConsulationName { get; set; }
        /// <summary>
        /// 医院现场咨询
        /// </summary>
        public string HospitalConsulationName { get; set; }
        /// <summary>
        /// 客户所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 行程信息
        /// </summary>
        public string TravelInformation { get; set; }

        /// <summary>
        /// 是否接车
        /// </summary>
        public bool IsNeedDriver { get; set; }
        /// <summary>
        /// 酒店安排
        /// </summary>
        public string HotelPlan { get; set; }

        /// <summary>
        /// 预估消费
        /// </summary>
        public decimal PlanConsumption { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 顾客照片
        /// </summary>
        public string CustomerPictureUrl { get; set; }
        /// <summary>
        /// 是否成交
        /// </summary>
        public bool IsDeal { get; set; }
        /// <summary>
        /// 是否到院
        /// </summary>
        public bool IsToHospital { get; set; }
        /// <summary>
        /// 累计成交金额
        /// </summary>
        public decimal CumulativeDealPrice { get; set; }
        /// <summary>
        /// 未成交原因
        /// </summary>
        public string UnDealReason { get; set; }

        /// <summary>
        /// 见面会铺垫项目
        /// </summary>
        public string FansMeetingProject { get; set; }

        /// <summary>
        /// 追踪内容
        /// </summary>
        public string FollowUpContent { get; set; }

        /// <summary>
        /// 下次邀约时间
        /// </summary>
        public DateTime? NextAppointmentDate { get; set; }
        /// <summary>
        /// 是否需要机构协助邀约
        /// </summary>
        public bool IsNeedHospitalHelp { get; set; }

        /// <summary>
        /// 是否需要助理协助
        /// </summary>
        public bool IsNeedCustomerServiceHelp { get; set; }
        /// <summary>
        /// 医院会员卡号
        /// </summary>
        public string HospitalMemberCardId { get; set; }
    }
    /// <summary>
    /// 粉丝见面会详情导出类
    /// </summary>
    public class ExportFansMeetingDetailsVo 
    {
        /// <summary>
        /// 医院卡号
        /// </summary>
        [Description("医院卡号")]
        public string HospitalMemberCardId { get; set; }
        /// <summary>
        /// 客户昵称
        /// </summary>
        [Description("客户昵称")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户手机号
        /// </summary>
        [Description("客户手机号")]
        public string Phone { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        [Description("预约时间")]
        public DateTime? AppointmentDate { get; set; }
        /// <summary>
        /// 详细时间
        /// </summary>
        [Description("详细时间")]
        public string AppointmentDetailsDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 新/老客
        /// </summary>
        [Description("新/老客")]
        public string IsOldCustomer { get; set; }
        /// <summary>
        /// 啊美雅助理名称
        /// </summary>
        [Description("啊美雅助理")]
        public string AmiyaConsulationName { get; set; }
        /// <summary>
        /// 客户所在城市
        /// </summary>
        [Description("客户所在城市")]
        public string City { get; set; }
        /// <summary>
        /// 是否接车
        /// </summary>
        [Description("是否接车")]
        public string IsNeedDriver { get; set; }


        /// <summary>
        /// 是否到院
        /// </summary>

        [Description("是否到院")]
        public string IsToHospital { get; set; }
        /// <summary>
        /// 是否成交
        /// </summary>

        [Description("是否成交")]
        public string IsDeal { get; set; }
        /// <summary>
        /// 预估消费
        /// </summary>
        [Description("预估消费")]
        public decimal PlanConsumption { get; set; }
        /// <summary>
        /// 顾客照片
        /// </summary>
        [Description("顾客照片")]
        public string CustomerPictureUrl { get; set; }
        /// <summary>
        /// 客户质量（0：一般；1：优质）
        /// </summary>
        [Description("客户质量")]
        public string CustomerQuantity { get; set; }
        /// <summary>
        /// 医院现场咨询
        /// </summary>
        [Description("医院现场咨询")]
        public string HospitalConsulationName { get; set; }
        /// <summary>
        /// 行程信息
        /// </summary>
        [Description("行程信息")]
        public string TravelInformation { get; set; }

        /// <summary>
        /// 酒店安排
        /// </summary>
        [Description("酒店安排")]
        public string HotelPlan { get; set; }

        /// <summary>
        /// 见面会铺垫项目
        /// </summary>
        [Description("见面会铺垫项目")]
        public string FansMeetingProject { get; set; }

        /// <summary>
        /// 追踪内容
        /// </summary>
        [Description("追踪内容")]
        public string FollowUpContent { get; set; }

        /// <summary>
        /// 下次邀约时间
        /// </summary>
        [Description("下次邀约时间")]
        public DateTime? NextAppointmentDate { get; set; }
        /// <summary>
        /// 是否需要机构协助邀约
        /// </summary>
        [Description("是否需要机构协助邀约")]
        public string IsNeedHospitalHelp { get; set; }
        /// <summary>
        /// 是否需要机构协助邀约
        /// </summary>
        [Description("是否需要助理协助邀约")]
        public string IsNeedCustomerServiceHelp { get; set; }
    }
}
