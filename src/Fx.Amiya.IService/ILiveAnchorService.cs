using Fx.Amiya.Dto.LiveAnchor;
using Fx.Common;
using Fx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
   public interface ILiveAnchorService
    {
        /// <summary>
        /// 根据主播基础id获取有效的主播列表
        /// </summary>
        /// <param name="liveAnchorBaseId"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetValidListByLiveAnchorBaseIdAsync(string liveAnchorBaseId);
        /// <summary>
        /// 根据主播基础id集合获取有效的主播列表
        /// </summary>
        /// <param name="liveAnchorBaseId"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetValidListByLiveAnchorBaseIdAsync(List<string> liveAnchorBaseId);
        /// <summary>
        /// 根据内容平台id获取有效的主播列表
        /// </summary>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetValidListByContentPlatFormIdAsync(string contentPlatFormId, int employeeId);

        /// <summary>
        /// 获取有效的主播列表
        /// </summary>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetValidAsync();

        /// <summary>
        /// 获取主播列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<LiveAnchorDto>> GetListAsync(string name, int employeeId,string contentPlatformId, bool valid, int pageNum, int pageSize);
        /// <summary>
        /// 根据平台和主播名字获取主播信息
        /// </summary>
        /// <param name="contentPlatFormId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetValidListByContentPlatFormIdAndNameAsync(string contentPlatFormId, string name);
        Task DeleteAsync(int id);
        Task AddAsync(AddLiveAnchorDto addDto);
        Task<LiveAnchorDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateLiveAnchorDto updateDto);
        /// <summary>
        /// 根据主播基础信息id获取有效主播ip
        /// </summary>
        /// <param name="baseInfoId"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetLiveAnchorListByBaseInfoId(string baseInfoId);
        /// <summary>
        /// 根据基础主播id获取主播ip(不区分有效无效)
        /// </summary>
        /// <param name="baseInfoId"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetAllLiveAnchorListByBaseInfoId(string baseInfoId);

        /// <summary>
        ///  根据基础主播id集合获取主播ip(不区分有效无效)
        /// </summary>
        /// <param name="baseInfoIds"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetAllLiveAnchorListByBaseInfoIds(List<string> baseInfoIds);

        /// <summary>
        /// 获取非自播达人的主播IP
        /// </summary>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetNotSelfLiveAnchor();
        /// <summary>
        /// 根据主播基础信息id集合获取所有主播id
        /// </summary>
        /// <param name="baseInfoId"></param>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetLiveAnchorListByBaseInfoIdListAsync(List<string> baseInfoId);
        /// <summary>
        /// 获取需要同步视频号订单的主播信息
        /// </summary>
        /// <returns></returns>
        Task<List<LiveAnchorDto>> GetWechatVideoOrderLiveAnchorIdAsync();
        /// <summary>
        /// 根据平台id和基础主播id获取主播ip的id
        /// </summary>
        /// <returns></returns>
        Task<List<int>> GetLiveAnchorIdsByContentPlatformIdAndBaseId(string contentPlatformId,string baseId);
        /// <summary>
        /// 判断主播是否归属于自播达人
        /// </summary>
        /// <param name="liveANchorId"></param>
        /// <returns></returns>
        Task<bool> IsBelongSelfLiveAnchorAsync(int liveAnchorId);

    }
}
