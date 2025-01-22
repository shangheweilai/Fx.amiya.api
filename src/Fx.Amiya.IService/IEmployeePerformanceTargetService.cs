using Fx.Amiya.Dto.EmployeePerformanceTarget.Input;
using Fx.Amiya.Dto.EmployeePerformanceTarget.Result;
using Fx.Common;
using Fx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IEmployeePerformanceTargetService
    {
        Task<FxPageInfo<EmployeePerformanceTargetDto>> GetListAsync(QueryEmployeePerformanceTargetDto query);

        Task AddAsync(AddEmployeePerformanceTargetDto addDto);
        Task<EmployeePerformanceTargetDto> GetByIdAsync(string id);
        Task UpdateAsync(UpdateEmployeePerformanceTargetDto updateDto);
        Task DeleteAsync(string id);
        /// <summary>
        /// 根据年月和助理id获取有效的业绩目标
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<decimal> GetByEmpIdAndYearMonthAsync(int employeeId, int year, int month);
        /// <summary>
        /// 根据年月和助理id获取线索目标
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<int> GetClueTargetByEmpIdAndYearMonthAsync(int employeeId, int year, int month);
        /// <summary>
        /// 根据基础主播id获取有效/潜在 分诊,加v目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        Task<EmployeeTargetInfoDto> GetEmployeeTargetByBaseLiveAnchorIdAsync(int year, int month, string baseLiveAnchorId);
        /// <summary>
        /// 根据助理id集合获取助理目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        Task<List<EmployeeTargetInfoDto>> GetEmployeeTargetByAssistantIdListAsync(int year, int month, List<int> assistantIds);
        /// <summary>
        /// 根据助理id获取助理年目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="assistantIds"></param>
        /// <returns></returns>
        Task<List<EmployeeTargetInfoDto>> GetEmployeeTargetByAssistantIdAndYearListAsync(int year, int assistantIds);
    }
}
