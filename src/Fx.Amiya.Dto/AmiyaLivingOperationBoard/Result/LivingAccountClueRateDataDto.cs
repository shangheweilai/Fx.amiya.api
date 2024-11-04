using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaLivingOperationBoard
{
    public class LivingAccountClueRateDataDto
    {
        /// <summary>
        /// 总线索
        /// </summary>
        public int TotalClue { get; set; }
        /// <summary>
        /// 平台获客占比
        /// </summary>
        public List<LivingDepartmentContentPlatformClueRateDataItemDto> DepartmentContentPlatformClueRate { get; set; }
    }
    public class LivingDepartmentContentPlatformClueRateDataItemDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Performance { get; set; }
    }
}
