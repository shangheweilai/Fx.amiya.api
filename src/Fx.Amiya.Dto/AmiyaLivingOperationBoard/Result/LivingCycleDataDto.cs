using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingCycleDataDto
    {
        /// <summary>
        /// 分诊派单转化周期柱状图数据
        /// </summary>
        public List<KeyValuePair<string, int>> SendCycleData { get; set; }
        /// <summary>
        /// 分诊上门/成交转化周期柱状图数据
        /// </summary>
        public List<KeyValuePair<string, int>> ToHospitalCycleData { get; set; }
    }
}
