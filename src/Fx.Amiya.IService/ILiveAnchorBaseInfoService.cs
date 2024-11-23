using Fx.Amiya.Dto.LiveAnchorBaseInfo;
using Fx.Common;
using Fx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
   public interface ILiveAnchorBaseInfoService
    {

        /// <summary>
        /// 获取有效的主播基础信息列表
        /// </summary>
        /// <returns></returns>
        Task<List<LiveAnchorBaseInfoDto>> GetValidAsync(bool? isSelfLiveAnchor);
        /// <summary>
        /// 获取全部主播列表
        /// </summary>
        /// <param name="isSelfLiveAnchor"></param>
        /// <returns></returns>
        Task<List<LiveAnchorBaseInfoDto>> GetAllLiveAnchorAsync(bool? isSelfLiveAnchor);

        /// <summary>
        /// 获取名索医生列表
        /// </summary>
        /// <returns></returns>
        Task<List<LiveAnchorBaseInfoDto>> GetMingSuoLiveAnchorAsync();

        /// <summary>
        /// 获取主播基础信息列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<LiveAnchorBaseInfoDto>> GetListAsync(string name, bool valid, int pageNum, int pageSize);

        Task DeleteAsync(string id);
        Task AddAsync(AddLiveAnchorBaseInfoDto addDto);
        Task<LiveAnchorBaseInfoDto> GetByIdAsync(string id);
        Task<LiveAnchorBaseInfoDto> GetByNameAsync(string name);
        Task<List<LiveAnchorBaseInfoDto>> GetByIdAndIsSelfLiveAnchorAsync(string id, bool? isSelfLiveAnchor);
        Task UpdateAsync(UpdateLiveAnchorBaseInfoDto updateDto);
        Task<List<LiveAnchorBaseInfoDto>> GetCooperateLiveAnchorAsync(bool? isValid);

    }
}
