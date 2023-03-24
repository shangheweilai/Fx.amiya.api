﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AestheticsDesignReport
{
    public class AestheticsDesignReportAndDesignInfoDto
    {
        public string Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CustomerId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 是否有医美经历
        /// </summary>
        public bool? HasAestheticMedicineHistory { get; set; }
        /// <summary>
        /// 微创调整的部位以及所用材料
        /// </summary>
        public string HistoryDescribe1 { get; set; }
        /// <summary>
        /// 整形调整的部位
        /// </summary>
        public string HistoryDescribe2 { get; set; }
        /// <summary>
        /// 皮肤做过的仪器或项目
        /// </summary>
        public string HistoryDescribe3 { get; set; }
        /// <summary>
        /// 是否接受手术
        /// </summary>
        public bool? WhetherAcceptOperation { get; set; }
        /// <summary>
        /// 是否有过过敏史或其他疾病
        /// </summary>
        public bool? WhetherAllergyOrOtherDisease { get; set; }
        /// <summary>
        /// 过敏或疾病描述
        /// </summary>
        public string AllergyOrOtherDiseaseDescribe { get; set; }
        /// <summary>
        /// 变美需求
        /// </summary>
        public string BeautyDemand { get; set; }
        /// <summary>
        /// 预算
        /// </summary>
        public decimal Budget { get; set; }
        /// <summary>
        /// 正面照片
        /// </summary>
        public string FrontPicture { get; set; }
        /// <summary>
        /// 侧面照片
        /// </summary>
        public string SidePicture { get; set; }
        /// <summary>
        /// 美学设计报告状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText { get; set; }
        /// <summary>
        /// 设计信息
        /// </summary>
        public DesignInfo Design { get; set; }
    }
    public class DesignInfo {
        public string Id { get; set; }
        /// <summary>
        /// 美学设计报告id
        /// </summary>
        public string AestheticsDesignReportId { get; set; }
        /// <summary>
        /// 设计内容
        /// </summary>
        public string Design { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public int HospitalId { get; set; }
        /// <summary>
        /// 推荐医院简称
        /// </summary>
        public string SimpleHospitalName { get; set; }
        /// <summary>
        /// 推荐医生
        /// </summary>
        public string RecommendDoctor { get; set; }
        /// <summary>
        /// 图片标签
        /// </summary>
        public List<BaseKeyValueDto> PictureTags { get; set; }
        /// <summary>
        /// 调整后的侧面图片
        /// </summary>
        public string SidePicture { get; set; }
        /// <summary>
        /// 调整后的正面图片
        /// </summary>
        public string FrontPicture { get; set; }
    }
}
