﻿using Fx.Amiya.Dto.WechatVideoOrder;
using Fx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IWeChatVideoOrderService
    {
        /// <summary>
        /// 添加视频号订单
        /// </summary>
        /// <returns></returns>
        Task AddAsync(List<WechatVideoAddDto> add);
        /// <summary>
        /// 获取视频号订单列表
        /// </summary>
        /// <returns></returns>
        Task<FxPageInfo<WechatVideoOrderInfoDto>> GetListByPageAsync(string keyWord,DateTime? startDate,DateTime? endDate,int? belongLiveAnchorId,string status,int? orderType,int pageSize,int pageNum  );
        /// <summary>
        /// 获取视频号带货数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="liveAnchorId"></param>
        /// <param name="goodsName"></param>
        /// <param name="takeGoodsType"></param>
        /// <returns></returns>
        Task<AutoCompleteDataDto> AutoCompleteDataAsync(DateTime date,int liveAnchorId,string goodsName,int takeGoodsType);
        /// <summary>
        /// 视频号手机号解密
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<string>  EncryptPhoneAsync(string orderId);
    }
}
