using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Result
{
    public class LivingAccountPerformanceRateDataVo
    {
        /// <summary>
        /// 总业绩
        /// </summary>
        public int TotalPerformance { get; set; }
        /// <summary>
        /// 平台业绩占比
        /// </summary>
        public List<LivingDepartmentContentPlatformPerformanceRateDataItemVo> DepartmentContentPlatformClueRate { get; set; }
    }
    public class LivingDepartmentContentPlatformPerformanceRateDataItemVo
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
