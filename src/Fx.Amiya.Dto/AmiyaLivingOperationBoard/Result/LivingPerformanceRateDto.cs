using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingPerformanceRateDto
    {
        /// <summary>
        /// 业绩贡献占比
        /// </summary>
        public List<KeyValuePair<string, decimal>> PerformanceRate { get; set; }
    }
}
