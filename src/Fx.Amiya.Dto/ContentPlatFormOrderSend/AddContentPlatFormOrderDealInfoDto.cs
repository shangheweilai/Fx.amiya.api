using Fx.Amiya.Dto.ContentPlatFormOrderDealDetails.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.ContentPlatFormOrderSend
{
    public class AddContentPlatFormOrderDealInfoDto
    {


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
        /// 到院时间（最新）
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
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 是否重单可深度
        /// </summary>
        public bool IsRepeatProfundityOrder { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int? ConsumptionType { get; set; }

        /// <summary>
        /// 邀约凭证
        /// </summary>
        public List<string> InvitationDocuments { get; set; }
        public List<AddContentPlatFormOrderDealDetailsDto> AddContentPlatFormOrderDealDetailsDtoList { get; set; }

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
    }
}
