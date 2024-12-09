using Fx.Amiya.Dto.LiveAnchorMonthlyTarget;
using Fx.Amiya.Dto.Performance;
using Fx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface ILiveAnchorMonthlyTargetAfterLivingService
    {
        Task<FxPageInfo<LiveAnchorMonthlyTargetAfterLivingDto>> GetListWithPageAsync(int Year, int Month, int? LiveAnchorId, int employeeId,int pageNum, int pageSize);
        Task AddAsync(AddLiveAnchorMonthlyTargetAfterLivingDto addDto);
        Task<List<LiveAnchorMonthlyTargetKeyAndValueDto>> GetIdAndNames(int year, int month);
        Task<LiveAnchorMonthlyTargetAfterLivingDto> GetByIdAsync(string id);
        Task UpdateAsync(UpdateLiveAnchorMonthlyTargetAfterLivingDto updateDto);
        Task EditAsync(UpdateLiveAnchorMonthlyAfterLivingTargetRateAndNumDto editDto);
        Task DeleteAsync(string id);

        /// <summary>
        /// 获取业绩相关数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<LiveAnchorMonthTargetPerformanceDto> GetPerformance(int year,int month, List<int> liveAnchorIds);


        /// <summary>
        /// 根据年份获取业绩数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<LiveAnchorMonthTargetPerformanceDto>> GetPerformanceByYearAsync(int year, List<int> liveAnchorIds, bool? isOldCustomer);
        /// <summary>
        /// 获取直播后业绩目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<LiveAnchorMonthTargetPerformanceDto> GetPerformanceTargetAsync(int year, int month, List<int> liveAnchorIds);
        /// <summary>
        /// 根据基础主播获取直播后月经营目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        Task<List<LiveAnchorMonthTargetPerformanceDto>> GetPerformanceTargetByBaseLiveAnchorIdAsync(int year, int month, List<string> baseLiveAnchorIds);
        /// <summary>
        /// 根据主播基础id按年月获取数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorBaseId"></param>
        /// <returns></returns>
        Task<GroupPerformanceListDto> GetLiveAnchorBaseIdPerformance(int year, int month, string liveAnchorBaseId);
            


        /// <summary>
        /// 基础业绩获取
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetBasePerformanceTargetAsync(int year, int month, List<int> liveAnchorIds);

        Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetAfterLivingTargetByDateAsync(int year, int month);


        /// <summary>
        /// 获取派单成交业绩目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<LiveAnchorBaseBusinessMonthTargetSendOrDealDto> GetSendOrDealTargetAsync(int year, int month, List<int> liveAnchorIds);
    }
}
