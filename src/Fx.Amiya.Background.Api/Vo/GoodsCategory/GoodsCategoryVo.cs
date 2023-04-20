﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.GoodsCategory
{
    public class GoodsCategoryVo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SimpleCode { get; set; }
        /// <summary>
        /// 展示方向
        /// </summary>
        public int ShowDirectionType { get; set; }
        public string ShowDirectionTypeName { get; set; }
        public bool Valid { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string CreateName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public string UpdateName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否是热门商品
        /// </summary>
        public bool IsHot { get; set; }
        /// <summary>
        /// 归属小程序appid
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 归属小程序名称
        /// </summary>
        public string MiniprogramName { get; set; }
        /// <summary>
        /// 是否是品牌分类
        /// </summary>
        public bool IsBrand { get; set; }
        /// <summary>
        /// 类别图片
        /// </summary>
        public string CategoryImg { get; set; }
    }
}
