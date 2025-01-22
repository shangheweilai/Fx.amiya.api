using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.EmployeePerformanceTarget.Input;
using Fx.Amiya.Dto.EmployeePerformanceTarget.Result;
using Fx.Amiya.Dto.FinancialBoard;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Fx.Infrastructure.DataAccess;
using jos_sdk_net.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class EmployeePerformanceTargetService : IEmployeePerformanceTargetService
    {
        private readonly IDalEmployeePerformanceTarget dalEmployeePerformanceTarget;
        private readonly IAmiyaEmployeeService amiyaEmployeeService;
        public EmployeePerformanceTargetService(IDalEmployeePerformanceTarget dalEmployeePerformanceTarget, IAmiyaEmployeeService amiyaEmployeeService)
        {
            this.dalEmployeePerformanceTarget = dalEmployeePerformanceTarget;
            this.amiyaEmployeeService = amiyaEmployeeService;
        }



        /// <summary>
        /// 根据条件获取助理业绩目标信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<EmployeePerformanceTargetDto>> GetListAsync(QueryEmployeePerformanceTargetDto query)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            if (query.EndDate.HasValue)
            {
                year = query.EndDate.Value.Year;
                month = query.EndDate.Value.Month;
            }
            var cmployeePerformanceTargets = from d in dalEmployeePerformanceTarget.GetAll().Include(x => x.AmiyaEmployee)
                                             where (!query.EmployeeId.HasValue || d.EmployeeId == query.EmployeeId.Value)
                                             && (d.Valid == true)
                                             && (!query.EndDate.HasValue || d.BelongYear == year && d.BelongMonth == month)
                                             select new EmployeePerformanceTargetDto
                                             {
                                                 Id = d.Id,
                                                 CreateDate = d.CreateDate,
                                                 EmployeeId = d.EmployeeId,
                                                 EmployeeName = d.AmiyaEmployee.Name,
                                                 UpdateDate = d.UpdateDate,
                                                 Valid = d.Valid,
                                                 DeleteDate = d.DeleteDate,
                                                 BelongMonth = d.BelongMonth,
                                                 BelongYear = d.BelongYear,
                                                 EffectiveConsulationCardTarget = d.EffectiveConsulationCardTarget,
                                                 PotentialConsulationCardTarget = d.PotentialConsulationCardTarget,
                                                 EffectiveAddWechatTarget = d.EffectiveAddWechatTarget,
                                                 PotentialAddWechatTarget = d.PotentialAddWechatTarget,
                                                 SendOrderTarget = d.SendOrderTarget,
                                                 OldCustomerVisitTarget = d.OldCustomerVisitTarget,
                                                 NewCustomerVisitTarget = d.NewCustomerVisitTarget,
                                                 NewCustomerDealTarget = d.NewCustomerDealTarget,
                                                 OldCustomerDealTarget = d.OldCustomerDealTarget,
                                                 NewCustomerPerformanceTarget = d.NewCustomerPerformanceTarget,
                                                 OldCustomerPerformanceTarget = d.OldCustomerPerformanceTarget,
                                                 PerformanceTarget = d.PerformanceTarget,
                                                 CluesRegisterTarget = d.CluesRegisterTarget
                                             };
            FxPageInfo<EmployeePerformanceTargetDto> cmployeePerformanceTargetPageInfo = new FxPageInfo<EmployeePerformanceTargetDto>();
            cmployeePerformanceTargetPageInfo.TotalCount = await cmployeePerformanceTargets.CountAsync();
            cmployeePerformanceTargetPageInfo.List = await cmployeePerformanceTargets.OrderByDescending(x => x.CreateDate).Skip((query.PageNum.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value).ToListAsync();
            return cmployeePerformanceTargetPageInfo;
        }


        /// <summary>
        /// 添加助理业绩目标
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddEmployeePerformanceTargetDto addDto)
        {
            try
            {
                var isExist = await dalEmployeePerformanceTarget.GetAll().Where(x => x.EmployeeId == addDto.EmployeeId && x.BelongMonth == addDto.BelongMonth && addDto.BelongYear == addDto.BelongYear && x.Valid == true).CountAsync();
                if (isExist > 0)
                {
                    throw new Exception("已存在该员工选择月份的业绩目标，请勿重复添加！");
                }
                EmployeePerformanceTarget cmployeePerformanceTarget = new EmployeePerformanceTarget();
                cmployeePerformanceTarget.Id = Guid.NewGuid().ToString();
                cmployeePerformanceTarget.CreateDate = DateTime.Now;
                cmployeePerformanceTarget.EmployeeId = addDto.EmployeeId;
                cmployeePerformanceTarget.Valid = true;
                cmployeePerformanceTarget.BelongYear = addDto.BelongYear;
                cmployeePerformanceTarget.BelongMonth = addDto.BelongMonth;
                cmployeePerformanceTarget.EffectiveConsulationCardTarget = addDto.EffectiveConsulationCardTarget;
                cmployeePerformanceTarget.PotentialConsulationCardTarget = addDto.PotentialConsulationCardTarget;
                cmployeePerformanceTarget.EffectiveAddWechatTarget = addDto.EffectiveAddWechatTarget;
                cmployeePerformanceTarget.PotentialAddWechatTarget = addDto.PotentialAddWechatTarget;
                cmployeePerformanceTarget.SendOrderTarget = addDto.SendOrderTarget;
                cmployeePerformanceTarget.OldCustomerVisitTarget = addDto.OldCustomerVisitTarget;
                cmployeePerformanceTarget.NewCustomerVisitTarget = addDto.NewCustomerVisitTarget;
                cmployeePerformanceTarget.NewCustomerDealTarget = addDto.NewCustomerDealTarget;
                cmployeePerformanceTarget.OldCustomerDealTarget = addDto.OldCustomerDealTarget;
                cmployeePerformanceTarget.NewCustomerPerformanceTarget = addDto.NewCustomerPerformanceTarget;
                cmployeePerformanceTarget.OldCustomerPerformanceTarget = addDto.OldCustomerPerformanceTarget;
                cmployeePerformanceTarget.PerformanceTarget = addDto.PerformanceTarget;
                cmployeePerformanceTarget.CluesRegisterTarget = addDto.CluesRegisterTarget;
                await dalEmployeePerformanceTarget.AddAsync(cmployeePerformanceTarget, true);

            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }



        public async Task<EmployeePerformanceTargetDto> GetByIdAsync(string id)
        {
            var result = await dalEmployeePerformanceTarget.GetAll().Include(x => x.AmiyaEmployee).Where(x => x.Id == id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new EmployeePerformanceTargetDto();
            }

            EmployeePerformanceTargetDto returnResult = new EmployeePerformanceTargetDto();
            returnResult.Id = result.Id;
            returnResult.CreateDate = result.CreateDate;
            returnResult.EmployeeId = result.EmployeeId;
            returnResult.EmployeeName = result.AmiyaEmployee.Name;
            returnResult.Valid = result.Valid;
            returnResult.BelongYear = result.BelongYear;
            returnResult.BelongMonth = result.BelongMonth;
            returnResult.EffectiveAddWechatTarget = result.EffectiveAddWechatTarget;
            returnResult.PotentialAddWechatTarget = result.PotentialAddWechatTarget;
            returnResult.EffectiveConsulationCardTarget = result.EffectiveConsulationCardTarget;
            returnResult.PotentialConsulationCardTarget = result.PotentialConsulationCardTarget;
            returnResult.SendOrderTarget = result.SendOrderTarget;
            returnResult.OldCustomerVisitTarget = result.OldCustomerVisitTarget;
            returnResult.NewCustomerVisitTarget = result.NewCustomerVisitTarget;
            returnResult.NewCustomerDealTarget = result.NewCustomerDealTarget;
            returnResult.OldCustomerDealTarget = result.OldCustomerDealTarget;
            returnResult.NewCustomerPerformanceTarget = result.NewCustomerPerformanceTarget;
            returnResult.OldCustomerPerformanceTarget = result.OldCustomerPerformanceTarget;
            returnResult.PerformanceTarget = result.PerformanceTarget;
            returnResult.CluesRegisterTarget = result.CluesRegisterTarget;

            return returnResult;
        }



        /// <summary>
        /// 修改助理业绩目标
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateEmployeePerformanceTargetDto updateDto)
        {
            var result = await dalEmployeePerformanceTarget.GetAll().Where(x => x.Id == updateDto.Id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("未找到助理业绩目标信息");
            var isExist = await dalEmployeePerformanceTarget.GetAll().Where(x => x.EmployeeId == updateDto.EmployeeId && x.BelongMonth == updateDto.BelongMonth && updateDto.BelongYear == updateDto.BelongYear && x.Id != updateDto.Id && x.Valid == true).CountAsync();
            if (isExist > 0)
            {
                throw new Exception("已存在该员工选择月份的业绩目标，请检查数据后重新提交！");
            }
            result.EmployeeId = updateDto.EmployeeId;
            result.BelongYear = updateDto.BelongYear;
            result.BelongMonth = updateDto.BelongMonth;
            result.EffectiveConsulationCardTarget = updateDto.EffectiveConsulationCardTarget;
            result.PotentialConsulationCardTarget = updateDto.PotentialConsulationCardTarget;
            result.EffectiveAddWechatTarget = updateDto.EffectiveAddWechatTarget;
            result.PotentialAddWechatTarget = updateDto.PotentialAddWechatTarget;
            result.SendOrderTarget = updateDto.SendOrderTarget;
            result.NewCustomerVisitTarget = updateDto.NewCustomerVisitTarget;
            result.OldCustomerVisitTarget = updateDto.OldCustomerVisitTarget;
            result.NewCustomerDealTarget = updateDto.NewCustomerDealTarget;
            result.OldCustomerDealTarget = updateDto.OldCustomerDealTarget;
            result.NewCustomerPerformanceTarget = updateDto.NewCustomerPerformanceTarget;
            result.OldCustomerPerformanceTarget = updateDto.OldCustomerPerformanceTarget;
            result.PerformanceTarget = updateDto.PerformanceTarget;
            result.UpdateDate = DateTime.Now;
            result.CluesRegisterTarget = updateDto.CluesRegisterTarget;
            await dalEmployeePerformanceTarget.UpdateAsync(result, true);
        }

        /// <summary>
        /// 作废助理业绩目标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            try
            {
                var result = await dalEmployeePerformanceTarget.GetAll().SingleOrDefaultAsync(e => e.Id == id && e.Valid == true);
                if (result == null)
                    throw new Exception("未找到助理业绩目标信息");
                result.Valid = false;
                result.DeleteDate = DateTime.Now;
                await dalEmployeePerformanceTarget.UpdateAsync(result, true);

            }
            catch (Exception er)
            {
                throw new Exception(er.Message.ToString());
            }
        }

        /// <summary>
        /// 根据年月和助理id获取有效的业绩目标
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<decimal> GetByEmpIdAndYearMonthAsync(int employeeId, int year, int month)
        {
            var result = await dalEmployeePerformanceTarget.GetAll().Include(x => x.AmiyaEmployee).Where(x => x.EmployeeId == employeeId && x.BelongYear == year && x.BelongMonth == month && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return 0.00M;
            }
            return result.PerformanceTarget;
        }

        /// <summary>
        /// 根据年月和助理id获取线索目标
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<int> GetClueTargetByEmpIdAndYearMonthAsync(int employeeId, int year, int month)
        {
            var result = await dalEmployeePerformanceTarget.GetAll().Include(x => x.AmiyaEmployee).Where(x => x.EmployeeId == employeeId && x.BelongYear == year && x.BelongMonth == month && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return 0;
            }
            return result.CluesRegisterTarget;
        }
        /// <summary>
        /// 根据基础主播id获取有效/潜在 分诊,加v目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        public async Task<EmployeeTargetInfoDto> GetEmployeeTargetByBaseLiveAnchorIdAsync(int year, int month, string baseLiveAnchorId)
        {
            var ids = (await amiyaEmployeeService.GetByLiveAnchorBaseIdAsync(baseLiveAnchorId)).Select(e => e.Id);
            var target = dalEmployeePerformanceTarget.GetAll().Where(e => e.Valid == true && e.BelongMonth == month && e.BelongYear == year && ids.Contains(e.EmployeeId));
            EmployeeTargetInfoDto targetInfo = new EmployeeTargetInfoDto();
            targetInfo.EffectiveAddWechatTarget = target.Sum(e => e.EffectiveAddWechatTarget);
            targetInfo.PotentialAddWechatTarget = target.Sum(e => e.PotentialAddWechatTarget);
            targetInfo.EffectiveConsulationCardTarget = target.Sum(e => e.EffectiveConsulationCardTarget);
            targetInfo.PotentialConsulationCardTarget = target.Sum(e => e.PotentialConsulationCardTarget);
            return targetInfo;
        }
        /// <summary>
        /// 根据助理id集合获取助理目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<EmployeeTargetInfoDto>> GetEmployeeTargetByAssistantIdListAsync(int year, int month, List<int> assistantIds)
        {

            return await dalEmployeePerformanceTarget.GetAll()
                .Where(e => e.Valid == true && e.BelongMonth == month && e.BelongYear == year && assistantIds.Contains(e.EmployeeId))
                .Select(e => new EmployeeTargetInfoDto
                {
                    EmployeeId = e.EmployeeId,
                    EffectiveAddWechatTarget = e.EffectiveAddWechatTarget,
                    PotentialAddWechatTarget = e.PotentialAddWechatTarget,
                    EffectiveConsulationCardTarget = e.EffectiveConsulationCardTarget,
                    PotentialConsulationCardTarget = e.PotentialConsulationCardTarget,
                    OldCustomerPerformanceTarget = e.OldCustomerPerformanceTarget,
                    NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                    PerformanceTarget = e.PerformanceTarget,
                    NewCustomerVisitTarget = e.NewCustomerVisitTarget,
                    OldCustomerVisitTarget = e.OldCustomerVisitTarget,
                    SendOrderTarget = e.SendOrderTarget,
                    NewCustomerDealNumTarget = e.NewCustomerDealTarget,
                    OldCustomerDealNumTarget = e.OldCustomerDealTarget
                }).ToListAsync();
        }

        /// <summary>
        /// 根据助理id获取助理年目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<EmployeeTargetInfoDto>> GetEmployeeTargetByAssistantIdAndYearListAsync(int year, int assistantIds)
        {

            return await dalEmployeePerformanceTarget.GetAll()
                .Where(e => e.Valid == true && e.BelongYear == year && assistantIds == e.EmployeeId)
                .Select(e => new EmployeeTargetInfoDto
                {
                    BelongYear = e.BelongYear,
                    BelongMonth = e.BelongMonth,
                    EmployeeId = e.EmployeeId,
                    EffectiveAddWechatTarget = e.EffectiveAddWechatTarget,
                    PotentialAddWechatTarget = e.PotentialAddWechatTarget,
                    EffectiveConsulationCardTarget = e.EffectiveConsulationCardTarget,
                    PotentialConsulationCardTarget = e.PotentialConsulationCardTarget,
                    OldCustomerPerformanceTarget = e.OldCustomerPerformanceTarget,
                    NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                    PerformanceTarget = e.PerformanceTarget,
                    NewCustomerVisitTarget = e.NewCustomerVisitTarget,
                    OldCustomerVisitTarget = e.OldCustomerVisitTarget,
                    SendOrderTarget = e.SendOrderTarget,
                    NewCustomerDealNumTarget = e.NewCustomerDealTarget,
                    OldCustomerDealNumTarget = e.OldCustomerDealTarget
                }).ToListAsync();
        }
    }
}
