﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Vo.ContentPlatFormOrderSend
{
    /// <summary>
    /// 内容平台订单改派
    /// </summary>
    public class UpdateContentPlatFormSendOrderInfoVo
    {
        /// <summary>
        /// 派单id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public string OrderId { get;set; }

        /// <summary>
        /// 医院编号
        /// </summary>
        public int HospitalId { get; set; }

        /// <summary>
        /// 是否未明确时间
        /// </summary>
        public bool IsUncertainDate { get; set; }

        /// <summary>
        /// 预约到院日期
        /// </summary>
        public DateTime? AppointmentDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 次派医院
        /// </summary>
        public List<int> OtherHospitalId { get; set; }
        /// <summary>
        /// 派单人
        /// </summary>
        public int SendBy { get; set; }
        /// <summary>
        /// 是否指定医院账户
        /// </summary>
        public bool IsSpecifyHospitalEmployee { get; set; }

        /// <summary>
        /// 医院账户id
        /// </summary>
        public int HospitalEmployeeId { get; set; }
    }
}
