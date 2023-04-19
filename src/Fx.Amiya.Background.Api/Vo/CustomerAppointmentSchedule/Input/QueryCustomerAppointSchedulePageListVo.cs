﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.CustomerAppointmentSchedule.Input
{
    public class QueryCustomerAppointSchedulePageListVo:BaseQueryVo
    {
        /// <summary>
        /// 重要程度
        /// </summary>
        public int? ImportantType { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool? IsFinish { get; set; }

        /// <summary>
        /// 预约类型
        /// </summary>
        public int? AppointmentType { get; set; }

    }
}
