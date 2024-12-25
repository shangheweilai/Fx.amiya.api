using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.LiveAnchorMonthlyTarget;
using Fx.Amiya.Dto.NewBusinessDashboard;
using Fx.Amiya.Dto.Performance;
using Fx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface ILiveAnchorMonthlyTargetLivingService
    {
        Task<FxPageInfo<LiveAnchorMonthlyTargetLivingDto>> GetListWithPageAsync(int Year, int Month, int? LiveAnchorId, int employeeId, int pageNum, int pageSize);
        Task AddAsync(AddLiveAnchorMonthlyTargetLivingDto addDto);
        Task<List<LiveAnchorMonthlyTargetKeyAndValueDto>> GetIdAndNames(int year, int month);
        Task<LiveAnchorMonthlyTargetLivingDto> GetByIdAsync(string id);
        Task UpdateAsync(UpdateLiveAnchorMonthlyTargetLivingDto updateDto);
        Task EditAsync(UpdateLiveAnchorMonthlyLivingTargetRateAndNumDto editDto);
        Task DeleteAsync(string id);

        Task<LiveAnchorMonthTargetPerformanceDto> GetPerformance(int year, int month, List<int> liveAnchorIds);

        /// <summary>
        /// 按月获取主播带货业绩
        /// </summary>
        /// <returns></returns>
        Task<List<PerformanceInfoByDateDto>> GetLiveAnchorCommercePerformance(int year, int month, List<int> liveAnchorIds);

        Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetBasePerformanceTargetAsync(int year, int month, List<int> liveAnchorIds);
        /// <summary>
        /// 根据年份和主播id获取设计卡下单目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<AmiyaOperationBoardCluesChannelTargetDto>> GetCluesTargetByYearAsync(int year, List<int> liveAnchorIds);

        Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetConsulationCardAddTargetByDateAsync(int year, int month);

        Task<List<LiveAnchorBaseBusinessMonthTargetPerformanceDto>> GetConsulationCardAddTargetByDateAsync(int year, int month, List<string> baseLiveAnchorIds);
        /// <summary>
        /// 根据主播id集合获取指定月份的月目标id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<string>> GetTargetIdsAsync(int year, int month,List<int> liveAnchorIds);
        /// <summary>
        /// 获取直播中目标信息
        /// </summary>
        /// <returns></returns>
        Task<LivingTargetDto> GetTargetAsync(QueryLivingBusinessDataDto queryDto);
    }
}
