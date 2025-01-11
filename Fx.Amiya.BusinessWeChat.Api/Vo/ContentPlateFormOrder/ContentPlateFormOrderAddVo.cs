using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.BusinessWeChat.Api.Vo.ContentPlateFormOrder
{
    /// <summary>
    /// 内容平台录单
    /// </summary>
    public class ContentPlateFormOrderAddVo
    {

        /// <summary>
        /// 订单类型(1：咨询:2：定金)
        /// </summary>
        [Required]
        public int OrderType { get; set; }

        /// <summary>
        /// 内容平台id（下单平台）
        /// </summary>
        [Required]
        public string ContentPlateFormId { get; set; }

        /// <summary>
        /// 归属月份
        /// </summary>
        public int BelongMonth { get; set; }

        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal AddOrderPrice { get; set; }

        /// <summary>
        /// 主播账号id
        /// </summary>
        [Required]
        public int LiveAnchorId { get; set; }
        /// <summary>
        /// 主播微信号
        /// </summary>
        public string LiveAnchorWeChatNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        [Required]
        public string CustomerName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string Phone { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WechatNumber { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// 预约门店
        /// </summary>
        [Required]
        public int AppointmentHospitalId { get; set; }


        /// <summary>
        /// 定金金额
        /// </summary>
        public decimal? DepositAmount { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        [Required]
        public string GoodsId { get; set; }
        /// <summary>
        /// 医院科室id
        /// </summary>
        [Required]
        public string HospitalDepartmentId { get; set; }
        /// <summary>
        /// 录单人员
        /// </summary>
        [Required]
        public int BelongEmpId { get; set; }
        /// <summary>
        /// 面诊员
        /// </summary>
        public int? ConsultationEmpId { get; set; }
        /// <summary>
        /// 面诊状态
        /// </summary>
        public int ConsultationType { get; set; }
        /// <summary>
        /// 主派咨询内容
        /// </summary>
        public string ConsultingContent { get; set; }
        /// <summary>
        /// 次派咨询内容
        /// </summary>
        public string ConsultingContent2 { get; set; }
        /// <summary>
        /// 后期项目铺垫
        /// </summary>
        public string LateProjectStage { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单来源（1：面诊卡，2：非面诊卡）
        /// </summary>
        public int? OrderSource { get; set; }

        /// <summary>
        /// 未派单原因
        /// </summary>
        public string UnSendReason { get; set; }

        /// <summary>
        /// 院方接诊人员
        /// </summary>
        public string AcceptConsulting { get; set; }

        /// <summary>
        /// 顾客照片（最多上传5张）
        /// </summary>
        public List<string> CustomerPictures { get; set; }
        /// <summary>
        /// 是否为辅助订单(企业微信目前写死为false)
        /// </summary>
        public bool IsSupportOrder { get; set; }

        /// <summary>
        /// 辅助客服（企业微信目前写死为0）
        /// </summary>
        public int SupportEmpId { get; set; }
        /// <summary>
        /// 获客方式
        /// </summary>
        public int GetCustomerType { get; set; }
        /// <summary>
        /// 客户来源
        /// </summary>
        public int CustomerSource { get; set; }

        /// <summary>
        /// 顾客类型
        /// </summary>
        public int CustomerType { get; set; }
        /// <summary>
        /// 归属部门
        /// </summary>
        public int BelongChannel { get; set; }

        /// <summary>
        /// 是否为日不落直播顾客
        /// </summary>
        public bool IsRiBuLuoLiving { get; set; }

        /// <summary>
        /// 归属公司
        /// </summary>
        public int BelongCompanyEnumId { get; set; }
    }
}
