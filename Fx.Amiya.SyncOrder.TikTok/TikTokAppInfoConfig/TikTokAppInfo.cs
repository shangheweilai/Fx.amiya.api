﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.SyncOrder.TikTok.TikTokAppInfoConfig
{
    public class TikTokAppInfo
    {
        public int Id { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string AccessToken { get; set; }
        public DateTime? AuthorizeDate { get; set; }
        public byte AppType { get; set; }
        public DateTime ExpireDate { get; set; }
        public string RefreshToken { get; set; }
    }
}