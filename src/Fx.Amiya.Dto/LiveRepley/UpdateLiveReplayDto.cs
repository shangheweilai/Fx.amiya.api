﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.LiveRepley
{
    public class UpdateLiveReplayDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 平台id
        /// </summary>
        public string ContentPlatformId { get; set; }

        /// <summary>
        /// 主播id
        /// </summary>
        public int LiveAnchorId { get; set; }

        /// <summary>
        /// 直播时间
        /// </summary>
        public DateTime LiveDate { get; set; }
        /// <summary>
        /// 直播时长
        /// </summary>
        public int LiveDuration { get; set; }
        /// <summary>
        /// gmv
        /// </summary>
        public decimal GMV { get; set; }
        /// <summary>
        /// 直播人员
        /// </summary>
        public string LivePersonnel { get; set; }
    }
}
