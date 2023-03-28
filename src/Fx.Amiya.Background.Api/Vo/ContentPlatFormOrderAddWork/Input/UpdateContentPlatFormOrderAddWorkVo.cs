﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ContentPlatFormOrderAddWork.Input
{
    public class UpdateContentPlatFormOrderAddWorkVo
    {
        public string Id { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public int AcceptBy { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public int HospitalId { get; set; }
        /// <summary>
        /// 申请理由
        /// </summary>
        public string SendRemark { get; set; }
    }
}