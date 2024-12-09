using Fx.Amiya.Background.Api.Vo.ContentPlatFormOrderDealDetails.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ContentPlateFormOrder
{
    public class ContentPlateFormOrderFinishVo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否成交
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 成交医院
        /// </summary>
        public int? LastDealHospitalId { get; set; }
        /// <summary>
        /// 是否到院（成交则默认true）
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
        /// 成交金额
        /// </summary>
        public decimal? DealAmount { get; set; }
        /// <summary>
        /// 后期项目铺垫
        /// </summary>
        public string LastProjectStage { get; set; }

        /// <summary>
        /// 成交截图url
        /// </summary>
        public string DealPictureUrl { get; set; }
        /// <summary>
        /// 未成交原因
        /// </summary>
        public string UnDealReason { get; set; }
        /// <summary>
        /// 未成交截图url
        /// </summary>
        public string UnDealPictureUrl { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime? DealDate { get; set; }
        /// <summary>
        /// 业绩类型
        /// </summary>
        public int? DealPerformanceType { get; set; }
        /// <summary>
        /// 是否陪诊
        /// </summary>
        public bool IsAcompanying { get; set; }

        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal CommissionRatio { get; set; }

        /// <summary>
        /// 三方订单号
        /// </summary>
        public string OtherContentPlatFormOrderId { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int? ConsumptionType { get; set; }

        /// <summary>
        /// 邀约凭证
        /// </summary>
        public List<string> InvitationDocuments { get; set; }
        /// <summary>
        /// 粉丝见面会id
        /// </summary>
        public string FansMeetingId { get; set; }

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
        /// 派单编号
        /// </summary>
        public int SendOrderId { get; set; }

        /// <summary>
        /// 成交明细
        /// </summary>
        public List<AddContentPlatFormOrderDealDetailsVo> AddContentPlatFormOrderDealDetailsVoList { get; set; }

        /// <summary>
        /// 上一条成交情况id
        /// </summary>
        public string LastDealInfoId { get; set; }
    }
}
