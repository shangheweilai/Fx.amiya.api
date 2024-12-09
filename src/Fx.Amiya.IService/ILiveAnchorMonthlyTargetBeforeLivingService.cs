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
    public interface ILiveAnchorMonthlyTargetBeforeLivingService
    {
        Task<FxPageInfo<LiveAnchorMonthlyTargetBeforeLivingDto>> GetListWithPageAsync(int Year, int Month, int? LiveAnchorId, int employeeId, int pageNum, int pageSize);
        Task AddAsync(AddLiveAnchorMonthlyTargetBeforeLivingDto addDto);
        Task<List<LiveAnchorMonthlyTargetKeyAndValueDto>> GetIdAndNames(int year, int month);
        Task<LiveAnchorMonthlyTargetBeforeLivingDto> GetByIdAsync(string id);
        Task UpdateAsync(UpdateLiveAnchorMonthlyTargetBeforeLivingDto updateDto);
        Task EditAsync(UpdateLiveAnchorMonthlyBeforeLivingTargetRateAndNumDto editDto);
        Task DeleteAsync(string id);
        /// <summary>
        /// 获取直播前目标数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<LiveAnchorBeforeLivingTargetDto?> GetBeforeLivingTargetByYearAndMonthAsync(QueryBeforeLivingBusinessDataDto query);


        /// <summary>
        /// 根据条件获取主播IP直播前线索目标总计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<LiveAnchorBaseBusinessMonthTargetBeforeLivingDto> GetCluePerformanceTargetAsync(int year, int month, List<int> liveAnchorIds);

        /// <summary>
        /// 根据年份获取直播前月目标数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<LiveAnchorBaseBusinessMonthTargetBeforeLivingDto>> GetCluePerformanceTargetByYearAsync(int year, List<int> liveAnchorIds);

    }
}
