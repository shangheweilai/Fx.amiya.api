using Fx.Amiya.Dto.AmiyaLivingOperationBoard;
using Fx.Amiya.Dto.AmiyaLivingOperationBoard.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IAmiyaLivingOperationBoardService
    {
        /// <summary>
        /// 直播中客资和新客业绩
        /// </summary>
        /// <returns></returns>
        Task<LivingCustomerAndPerformanceDataDto> GetLivingCustomerAndPerformanceDataAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中客资和业绩折线图
        /// </summary>
        /// <returns></returns>
        Task<LivingCustomerAndPerformanceBrokenLineDataDto> GetLivingCustomerAndPerformanceBrokenLineDataAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中漏斗图数据
        /// </summary>
        /// <returns></returns>
        Task<LivingFilterDataDto> GetLivingFilterDataAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中转化周期数据
        /// </summary>
        /// <returns></returns>
        Task<LivingCycleDataDto> GetLivingCycleDataAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中线索目标完成率
        /// </summary>
        /// <returns></returns>
        Task<LivingClueTargetDataDto> GetLivingClueTargetDataAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中业绩贡献占比
        /// </summary>
        /// <returns></returns>
        Task<LivingPerformanceRateDto> GetLivingPerformanceRateAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中平台账号获客占比
        /// </summary>
        /// <returns></returns>
        Task<LivingContentplatformClueDataDto> GetLivingContentplatformClueDataAsync(QueryLivingDataDto query);
        /// <summary>
        /// 直播中平台账号业绩占比
        /// </summary>
        /// <returns></returns>
        Task<LivingContentplatformPerformanceDataDto> GetLivingContentplatformPerformanceDataAsync(QueryLivingDataDto query);
        
    }
}
