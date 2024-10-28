using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.DbModels.Model
{
    public class CustomerServiceCompensation : BaseDbModel
    {

        /// <summary>
        /// 创建人id
        /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 账单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 归属客服
        /// </summary>
        public int BelongEmpId { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal OtherPrice { get; set; }
        /// <summary>
        /// 费用备注
        /// </summary>
        public string Remark { get; set; }

        public AmiyaEmployee CreateByEmployee { get; set; }

        public AmiyaEmployee BelongEmployee { get; set; }
        /// <summary>
        /// 底薪
        /// </summary>
        public decimal Salary { get; set; }
        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal CustomerServicePerformance { get; set; }

        /// <summary>
        /// 老带新奖励金额
        /// </summary>
        public Decimal OldTakeNewCustomerPrice { get; set; }

        #region 助理

        /// <summary>
        /// 上门率
        /// </summary>
        public decimal ToHospitalRate { get; set; }
        /// <summary>
        /// 上门率奖励
        /// </summary>
        public decimal ToHospitalRateReword { get; set; }
        /// <summary>
        /// 复购率
        /// </summary>
        public decimal RepeatPurchasesRate { get; set; }
        /// <summary>
        /// 复购率奖励
        /// </summary>
        public decimal RepeatPurchasesRateReword { get; set; }
        /// <summary>
        /// 新客上门奖励
        /// </summary>
        public decimal NewCustomerToHospitalReword { get; set; }
        /// <summary>
        /// 老客上门奖励
        /// </summary>
        public decimal OldCustomerToHospitalReword { get; set; }
        /// <summary>
        /// 目标达成奖励
        /// </summary>
        public decimal TargetFinishReword { get; set; }
        #endregion

        #region 行政客服
        /// <summary>
        /// 当前组加v率达成情况（若低于健康值则扣款300）
        /// </summary>
        public decimal AddWechatCompletePrice { get; set; }

        /// <summary>
        /// 个人线索登记完成率（高于特定值奖励500，低于特定值扣款300）
        /// </summary>
        public decimal AddClueCompletePrice { get; set; }
        /// <summary>
        /// 医美客资加V业绩
        /// </summary>
        public decimal BeautyAddWechatPrice { get; set; }
        /// <summary>
        /// 带货客资加V业绩
        /// </summary>
        public decimal TakeGoodsAddWechatPrice { get; set; }

        /// <summary>
        /// 引导面诊卡下单金额
        /// </summary>
        public decimal ConsulationCardPrice { get; set; }

        /// <summary>
        /// 引导面诊卡下单加v金额
        /// </summary>
        public decimal ConsulationCardAddWechatPrice { get; set; }

        /// <summary>
        /// 供应链达人派单提成金额
        /// </summary>
        public decimal CooperationLiveAnchorSendOrderPrice { get; set; }

        /// <summary>
        /// 供应链达人上门提成金额
        /// </summary>
        public decimal CooperationLiveAnchorToHospitalPrice { get; set; }

        #endregion
        /// <summary>
        /// 特定医院上门奖励金额
        /// </summary>
        public decimal SpecialHospitalVisitPrice { get; set; }
        /// <summary>
        /// 其他扣款
        /// </summary>
        public decimal OtherChargebacks { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Verison { get; set; }
    }
}
