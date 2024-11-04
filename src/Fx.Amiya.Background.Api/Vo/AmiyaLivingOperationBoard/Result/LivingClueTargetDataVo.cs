using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Result
{
    public class LivingClueTargetDataVo
    {
        /// <summary>
        /// 目标完成率
        /// </summary>
        public List<KeyValuePair<string, decimal>> ClueTargetComplete { get; set; }
    }
}
