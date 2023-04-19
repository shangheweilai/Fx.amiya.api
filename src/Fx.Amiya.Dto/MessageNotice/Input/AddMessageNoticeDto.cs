﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.MessageNotice.Input
{
    public class AddMessageNoticeDto
    {
        public int AcceptBy { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public int NoticeType { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string NoticeContent { get; set; }
    }
}
