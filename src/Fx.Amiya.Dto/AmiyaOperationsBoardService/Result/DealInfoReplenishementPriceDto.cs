using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaOperationsBoardService.Result
{
    public class DealInfoReplenishementPriceDto
    {
        public string Phone { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public DateTime LastDealIdCraeteDate { get; set; }
        public bool IsToHospital { get; set; }
        public bool IsOldCustomer { get; set; }
    }
}
