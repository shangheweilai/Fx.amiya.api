using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingAccountPerformanceRateDataDto
    {
        /// <summary>
        /// 平台业绩占比
        /// </summary>
        public List<LivingDepartmentContentPlatformPerformanceRateDataItemDto> DepartmentContentPlatformClueRate { get; set; }
    }
    public class LivingDepartmentContentPlatformPerformanceRateDataItemDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
