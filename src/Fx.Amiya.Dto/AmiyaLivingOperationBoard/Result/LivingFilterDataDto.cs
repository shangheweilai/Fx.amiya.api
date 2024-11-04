using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingFilterDataDto
    {
        /// <summary>
        /// 当前组
        /// </summary>
        public LivingFilterDataItemDto CurrentGroup { get; set; }
        /// <summary>
        /// 公司
        /// </summary>

        public LivingFilterDataItemDto Company { get; set; }
    }
    public class LivingFilterDataItemDto
    {
        /// <summary>
        /// 加v率
        /// </summary>
        public decimal? AddWeChatRate { get; set; }
        /// <summary>
        /// 加v率健康值
        /// </summary>
        public decimal AddWeChatRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 派单率
        /// </summary>
        public decimal? SendOrderRate { get; set; }
        /// <summary>
        /// 派单率健康值
        /// </summary>
        public decimal SendOrderRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 上门率
        /// </summary>
        public decimal? ToHospitalRate { get; set; }
        /// <summary>
        /// 上门率健康值
        /// </summary>
        public decimal ToHospitalRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 成交率
        /// </summary>
        public decimal? DealRate { get; set; }
        /// <summary>
        /// 成交率健康值
        /// </summary>
        public decimal DealRateHealthValueThisMonth { get; set; }
        /// <summary>
        /// 派单周期
        /// </summary>
        public int SendCycle { get; set; }
        /// <summary>
        /// 上门周期
        /// </summary>
        public int HospitalCycle { get; set; }
        /// <summary>
        /// 漏斗图数据
        /// </summary>
        public List<LivingFilterDetailDataDto> DataList { get; set; }

    }
    /// <summary>
    /// 业绩输出详情
    /// </summary>
    public class LivingFilterDetailDataDto
    {
        /// <summary>
        /// 标识码
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }
    }
}
