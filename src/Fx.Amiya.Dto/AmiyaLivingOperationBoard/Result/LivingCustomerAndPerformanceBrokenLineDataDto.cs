using Fx.Amiya.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingCustomerAndPerformanceBrokenLineDataDto
    {
        // <summary>
        /// 线索数据
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> ClueData { get; set; }
        /// <summary>
        /// 业绩数据
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> PerformanceData { get; set; }
    }
}
