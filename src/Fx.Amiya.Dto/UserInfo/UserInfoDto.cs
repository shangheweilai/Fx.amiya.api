﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fx.Amiya.Dto.UserInfo
{
   public class UserInfoDto
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }

        /// <summary>
        /// 性别编号
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// 性别文本
        /// </summary>
        public string Sex { get; set; }
        public string Avatar { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string UnionId { get; set; }
        public string Area { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public string PersonalSignature { get; set; }

        /// <summary>
        /// 是否需要授权用户信息
        /// </summary>
        public bool IsAuthorizationUserInfo { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public string SuperiorId { get; set; }
    }
}
