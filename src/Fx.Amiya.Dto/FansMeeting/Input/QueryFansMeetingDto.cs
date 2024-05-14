﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.FansMeeting.Input
{
    public class QueryFansMeetingDto : BaseQueryDto
    {

        /// <summary>
        /// 医院id
        /// </summary>
        public int? HospitalId { get; set; }
        public int empLoyeeId { get; set; }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool Valid { get; set; }
    }
}
