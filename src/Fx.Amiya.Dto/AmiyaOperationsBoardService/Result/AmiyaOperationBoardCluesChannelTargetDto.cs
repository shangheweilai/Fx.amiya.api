using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaOperationsBoardService.Result
{
    public class AmiyaOperationBoardCluesChannelTargetDto
    {

        public decimal TotalPerformanceTarget { get; set; }
        public int CluesTarget { get; set; }

        public int Month { get; set; }
        public int LiveAnchorId { get; set; }
    }
}
