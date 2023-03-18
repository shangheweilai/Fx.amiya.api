﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AestheticsDesignReport
{
    public class UpdateAestheticsDesignReportInfoDto
    {
        public string Id { get; set; }

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
        /// 经历描述
        /// </summary>
        public string HistoryDescribe { get; set; }
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
        /// 三张平面照片
        /// </summary>
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
    }
}
