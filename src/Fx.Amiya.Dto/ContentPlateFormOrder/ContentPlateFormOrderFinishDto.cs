using Fx.Amiya.Dto.ContentPlatFormOrderDealDetails.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ContentPlateFormOrder
{
    public class ContentPlateFormOrderFinishDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 是否成交
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 是否到院
        /// </summary>
        public bool IsToHospital { get; set; }
        /// <summary>
        /// 到院类型
        /// </summary>
        public int ToHospitalType { get; set; }
        /// <summary>
        /// 业绩类型
        /// </summary>
        public int DealPerformanceType { get; set; }
        /// <summary>
        /// 到院时间（最新）
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
        /// 未成交截图url
        /// </summary>
        public string UnDealPictureUrl { get; set; }
        /// <summary>
        /// 未成交原因
        /// </summary>
        public string UnDealReason { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime? DealDate { get; set; }
        /// <summary>
        /// 最终成交医院id
        /// </summary>
        public int? LastDealHospitalId { get; set; }

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
        /// 操作人，医院为0；
        /// </summary>
        public int EmpId { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int? ConsumptionType { get; set; }
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
        /// 邀约凭证
        /// </summary>
        public List<string> InvitationDocuments { get; set; }
        /// <summary>
        /// 派单编号
        /// </summary>
        public int SendOrderId { get; set; }
        public List<AddContentPlatFormOrderDealDetailsDto> AddContentPlatFormOrderDealDetailsDtoList { get; set; }
        /// <summary>
        /// 上一条成交情况id
        /// </summary>
        public string LastDealInfoId { get; set; }
    }
}
