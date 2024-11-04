using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.AmiyaLivingOperationBoard.Result
{
    public class LivingAccountClueRateDataVo
    {
        /// <summary>
        /// 总线索
        /// </summary>
        public int TotalClue { get; set; }
        /// <summary>
        /// 平台获客占比
        /// </summary>
        public List<LivingDepartmentContentPlatformClueRateDataItemVo> DepartmentContentPlatformClueRate { get; set; }
    }
    public class LivingDepartmentContentPlatformClueRateDataItemVo
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
