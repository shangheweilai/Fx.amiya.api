using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ContentPlateFormOrder
{
    /// <summary>
    /// 内容平台订单
    /// </summary>
    public class ContentPlateFormOrderVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        
        public string Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 订单类型(1：咨询:2：定金)
        /// </summary>

        public int OrderType { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>


        public string OrderTypeText { get; set; }

        /// <summary>
        /// 归属月份
        /// </summary>
        public int BelongMonth { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 订单状态文本
        /// </summary>
        public string OrderStatusText { get; set; }

        /// <summary>
        /// 内容平台id（下单平台）
        /// </summary>

        public string  ContentPlateFormId { get; set; }

        /// <summary>
        /// 主播账号id
        /// </summary>
        
        public int LiveAnchorId { get; set; }
        /// <summary>
        /// 主播微信号
        /// </summary>
        public string LiveAnchorWeChatNo { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>

        public string GoodsId { get; set; }
        /// <summary>
        /// 医院科室id
        /// </summary>
        public string HospitalDepartmentId { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        
        public string CustomerName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        
        public string Phone { get; set; }
        /// <summary>
        /// 加密手机号
        /// </summary>
        public string EncryptPhone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }
        /// <summary>
        /// 微信号id
        /// </summary>
        public string LiveAnchorBaseWechatId { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WechatNumber { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal AddOrderPrice { get; set; }
        /// <summary>
        /// 面诊员
        /// </summary>

        public int ConsultationEmpId { get; set; }

        /// <summary>
        /// 面诊类型
        /// </summary>
        public int ConsultationType { get; set; }
        /// <summary>
        /// 面诊类型文本
        /// </summary>
        public string ConsultationTypeText { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// 预约门店id
        /// </summary>
        
        public int AppointmentHospitalId { get; set; }


        /// <summary>
        /// 定金金额
        /// </summary>
        public decimal? DepositAmount { get; set; }
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
        /// 审核状态
        /// </summary>
        public int? CheckState { get; set; }
        /// <summary>
        /// 审核金额
        /// </summary>

        public decimal? CheckPrice { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>

        public decimal? SettlePrice { get; set; }


        /// <summary>
        /// 订单来源（1：面诊卡，2：非面诊卡）
        /// </summary>
        public int? OrderSource { get; set; }

        /// <summary>
        /// 订单来源
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
        /// 顾客照片（最多上传5张）
        /// </summary>
        public List<string> CustomerPictures { get; set; }


        /// <summary>
        /// 录单时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 医院网咨人员
        /// </summary>
        public string NetWorkConsulationName { get; set; }
        /// <summary>
        /// 医院现场咨询人员
        /// </summary>
        public string SceneConsulationName { get; set; }

        /// <summary>
        /// 派单时间
        /// </summary>
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// 派单人id
        /// </summary>
        public int? SendBy { get; set; }
        /// <summary>
        /// 派单人
        /// </summary>
        public string SendByName { get; set; }
        /// <summary>
        /// 派单医院
        /// </summary>
        public string SendHospitalName { get; set; }
        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime? DealDate { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal? DealAmount { get; set; }
        /// <summary>
        /// 业绩类型
        /// </summary>
        public string DealPerformanceTypeText { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// 预约医院
        /// </summary>
        public string AppointmentHospitalName { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 下单平台
        /// </summary>
        public string ContentPlateFormName { get; set; }
        /// <summary>
        /// 医院科室
        /// </summary>
        public string HospitalDepartmentName { get; set; }
        /// <summary>
        /// 面诊员名字
        /// </summary>
        public string ConsultationEmpName { get; set; }
        /// <summary>
        /// 主播
        /// </summary>
        public string LiveAnchorName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string CheckStateText { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ThumbPicture { get; set; }
        /// <summary>
        /// 归属客服id
        /// </summary>
        public int? BelongEmpId { get; set; }

        /// <summary>
        /// 归属客服
        /// </summary>
        public string BelongEmpName { get; set; }
        /// <summary>
        /// 最终成交医院id
        /// </summary>
        public int? LastDealHospitalId { get; set; }
        /// <summary>
        /// 最终成交医院
        /// </summary>
        public string LastDealHospitalName { get; set; }
        /// <summary>
        /// 三方订单号
        /// </summary>
        public string OtherContentPlatFormOrderId { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        public int? CheckBy { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string CheckByName { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? CheckDate { get; set; }

        /// <summary>
        /// 是否回款
        /// </summary>
        public bool IsReturnBackPrice { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public decimal? ReturnBackPrice { get; set; }
        /// <summary>
        /// 回款时间
        /// </summary>
        public DateTime? ReturnBackDate { get; set; }
        /// <summary>
        /// 是否到院
        /// </summary>
        public bool IsToHospital { get; set; }
        /// <summary>
        /// 到院类型
        /// </summary>
        public int ToHospitalType { get; set; }
        /// <summary>
        /// 到院类型文本
        /// </summary>

        public string ToHospitalTypeText { get; set; }
        /// <summary>
        /// 到院时间（最新）
        /// </summary>
        public DateTime? ToHospitalDate { get; set; }
        /// <summary>
        /// 未成交截图url
        /// </summary>
        public string UnDealPictureUrl { get; set; }
        /// <summary>
        /// 未成交原因
        /// </summary>
        public string UnDealReason { get; set; }
        /// <summary>
        /// 成交凭证
        /// </summary>
        public string DealPictureUrl { get; set; }

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
        /// 是否是重单可深度订单
        /// </summary>
        public bool IsRepeatProfundityOrder { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsCreateBill { get; set; }
        /// <summary>
        /// 开票公司
        /// </summary>
        public string CreateBillCompany { get; set; }
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
        /// 客户来源
        /// </summary>
        public int CustomerSource { get; set; }

        /// <summary>
        /// 客户来源文本
        /// </summary>
        public string CustomerSourceText { get; set; }

        /// <summary>
        /// 顾客类型
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// 顾客类型文本
        /// </summary>
        public string CustomerTypeText { get; set; }
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
        /// 归属公司id
        /// </summary>
        public int BelongCompanyEnumId { get; set; }

        /// <summary>
        /// 归属公司名称
        /// </summary>
        public string BelongCompany { get; set; }
    }
}
