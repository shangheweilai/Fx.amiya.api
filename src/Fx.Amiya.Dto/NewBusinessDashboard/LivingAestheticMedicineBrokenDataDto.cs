﻿using Fx.Amiya.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.NewBusinessDashboard
{
    public class LivingAestheticMedicineBrokenDataDto
    {
        /// <summary>
        /// 设计卡下单
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> DesignCardOrderData { get; set; }
        /// <summary>
        /// 设计卡退单
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> DesignCardRefundData { get; set; }
        /// <summary>
        /// 设计卡实际
        /// </summary>
        public List<PerformanceBrokenLineListInfoDto> DesignCardActualData { get; set; }
    }
}
