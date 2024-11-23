using Fx.Amiya.Dto.AmiyaLivingOperationBoard;
using Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Input;
using Fx.Amiya.Dto.AmiyaMingSuoOperationBoard.Result;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IAmiyaMingSuoOperationBoardService
    {
        Task<OperationMingSuoAchievementDataDto> GetTotalAchievementAndDateScheduleAsync(QueryOperationDataDto query);
        Task<ResultMingSuoOperationDataDto> GetMingSuoFilterDataAsync(QueryMingSuoFilterDataDto query);

        Task<MingSuoTransformCycleDataDto> GetLiveAnchorTransformCycleDataAsync(QueryOperationDataDto query);

        Task<MingSuoClueTargetDataDto> GetMingSuoClueAndPerformanceTargetDataAsync(QueryMingSuoCompleteDataDto query);

        Task<AssiatantTargetCompleteAndPerformanceRateDto> GetAssiatantTargetCompleteAndPerformanceRateDataAsync(QueryMingSuoAssistantPerformanceDto query);

        Task<MingSuoContentplatformClueDataDto> GetMingSuoContentplatformClueDataAsync(QueryMingSuoAssistantPerformanceDto query);

        Task<MingSuoContentplatformPerformanceDataDto> GetMingSuoContentplatformPerformanceDataAsync(QueryMingSuoAssistantPerformanceDto query);
    }
}
