﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fx.Amiya.Dto.AmiyaPositionInfo
{
   public class UpdateAmiyaPositionInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        /// <summary>
        /// 是否为管理员
        /// </summary>
        public bool IsDirector { get; set; }
        /// <summary>
        /// 是否可查看数据中心
        /// </summary>
        public bool ReadDataCenter { get; set; }
        /// <summary>
        /// 查看主播数据
        /// </summary>
        public bool ReadLiveAnchorData { get; set; }
        /// <summary>
        /// 读取数据中心直播达人数据
        /// </summary>
        public bool ReadSelfLiveAnchorData { get; set; }
        /// <summary>
        /// 读取数据中心合作达人数据
        /// </summary>
        public bool ReadCooperateLiveAnchorData { get; set; }
        /// <summary>
        /// 读取数据中心带货板块数据
        /// </summary>
        public bool ReadTakeGoodsData { get; set; }
    }
}
