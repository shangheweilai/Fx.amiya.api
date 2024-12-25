using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.LiveAnchorMonthlyTarget;
using Fx.Amiya.Dto.NewBusinessDashboard;
using Fx.Amiya.Dto.Performance;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using jos_sdk_net.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class LiveAnchorMonthlyTargetLivingService : ILiveAnchorMonthlyTargetLivingService
    {
        private IDalLiveAnchorMonthlyTargetLiving dalLiveAnchorMonthlyTargetLiving;
        private ILiveAnchorService _liveanchorService;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private IEmployeeBindLiveAnchorService employeeBindLiveAnchorService;

        public LiveAnchorMonthlyTargetLivingService(IDalLiveAnchorMonthlyTargetLiving dalLiveAnchorMonthlyTargetLiving,
            ILiveAnchorService liveAnchorService,
            IEmployeeBindLiveAnchorService employeeBindLiveAnchorService,
            IAmiyaEmployeeService amiyaEmployeeService)
        {
            this.dalLiveAnchorMonthlyTargetLiving = dalLiveAnchorMonthlyTargetLiving;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.employeeBindLiveAnchorService = employeeBindLiveAnchorService;
            _liveanchorService = liveAnchorService;
        }



        public async Task<FxPageInfo<LiveAnchorMonthlyTargetLivingDto>> GetListWithPageAsync(int Year, int Month, int? LiveAnchorId, int employeeId, int pageNum, int pageSize)
        {
            try
            {
                List<int> liveAnchorIds = new List<int>();
                if (LiveAnchorId.HasValue)
                {
                    liveAnchorIds.Add(LiveAnchorId.Value);
                }
                else
                {
                    var empInfo = await _amiyaEmployeeService.GetByIdAsync(employeeId);
                    if (empInfo.PositionId == 19)
                    {
                        var bindLiveAnchorInfo = await employeeBindLiveAnchorService.GetByEmpIdAsync(employeeId);
                        foreach (var x in bindLiveAnchorInfo)
                        {
                            liveAnchorIds.Add(x.LiveAnchorId);
                        }
                    }
                }
                var liveAnchorMonthlyTargetLiving = from d in dalLiveAnchorMonthlyTargetLiving.GetAll().Include(e => e.LiveAnchor)
                                                    where (d.Year == Year)
                                                    && (d.Month == Month)
                                                    && (liveAnchorIds.Count == 0 || liveAnchorIds.Contains(d.LiveAnchorId))
                                                    select new LiveAnchorMonthlyTargetLivingDto
                                                    {
                                                        Id = d.Id,
                                                        Year = d.Year,
                                                        Month = d.Month,
                                                        MonthlyTargetName = d.MonthlyTargetName,
                                                        LiveAnchorId = d.LiveAnchorId,
                                                        LiveAnchorName = d.LiveAnchor.Name,
                                                        ContentPlatFormId = d.LiveAnchor.ContentPlateFormId,
                                                        LivingRoomCumulativeFlowInvestment = d.LivingRoomCumulativeFlowInvestment,
                                                        LivingRoomFlowInvestmentTarget = d.LivingRoomFlowInvestmentTarget,
                                                        LivingRoomFlowInvestmentCompleteRate = d.LivingRoomFlowInvestmentCompleteRate,

                                                        ConsultationTarget = d.ConsultationTarget,
                                                        CumulativeConsultation = d.CumulativeConsultation,
                                                        ConsultationCompleteRate = d.ConsultationCompleteRate,

                                                        CumulativeConsultation2 = d.CumulativeConsultation2,
                                                        ConsultationTarget2 = d.ConsultationTarget2,
                                                        ConsultationCompleteRate2 = d.ConsultationCompleteRate2,

                                                        CargoSettlementCommissionTarget = d.CargoSettlementCommissionTarget,
                                                        CumulativeCargoSettlementCommission = d.CumulativeCargoSettlementCommission,
                                                        CargoSettlementCommissionCompleteRate = d.CargoSettlementCommissionCompleteRate,

                                                        CreateDate = d.CreateDate,

                                                        LivingRefundCardTarget = d.LivingRefundCardTarget,
                                                        CumulativeLivingRefundCard = d.CumulativeLivingRefundCard,
                                                        LivingRefundCardCompleteRate = d.LivingRefundCardCompleteRate,

                                                        GMVTarget = d.GMVTarget,
                                                        CumulativeGMV = d.CumulativeGMV,
                                                        GMVTargetCompleteRate = d.GMVTargetCompleteRate,

                                                        EliminateCardGMVTarget = d.EliminateCardGMVTarget,
                                                        CumulativeEliminateCardGMV = d.CumulativeEliminateCardGMV,
                                                        EliminateCardGMVTargetCompleteRate = d.EliminateCardGMVTargetCompleteRate,

                                                        RefundGMVTarget = d.RefundGMVTarget,
                                                        CumulativeRefundGMV = d.CumulativeRefundGMV,
                                                        RefundGMVTargetCompleteRate = d.RefundGMVTargetCompleteRate,

                                                        CluesTarget = d.CluesTarget,
                                                        CumulativeClues = d.CumulativeClues,
                                                        CluesTargetCompleteRate = d.CluesTargetCompleteRate

                                                    };

                FxPageInfo<LiveAnchorMonthlyTargetLivingDto> liveAnchorMonthlyTargetLivingPageInfo = new FxPageInfo<LiveAnchorMonthlyTargetLivingDto>();
                liveAnchorMonthlyTargetLivingPageInfo.TotalCount = await liveAnchorMonthlyTargetLiving.CountAsync();
                liveAnchorMonthlyTargetLivingPageInfo.List = await liveAnchorMonthlyTargetLiving.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                return liveAnchorMonthlyTargetLivingPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task AddAsync(AddLiveAnchorMonthlyTargetLivingDto addDto)
        {
            try
            {
                LiveAnchorMonthlyTargetLiving liveAnchorMonthlyTarget = new LiveAnchorMonthlyTargetLiving();
                liveAnchorMonthlyTarget.Id = Guid.NewGuid().ToString();
                liveAnchorMonthlyTarget.Year = addDto.Year;
                liveAnchorMonthlyTarget.Month = addDto.Month;
                liveAnchorMonthlyTarget.MonthlyTargetName = addDto.MonthlyTargetName;
                liveAnchorMonthlyTarget.LiveAnchorId = addDto.LiveAnchorId;
                liveAnchorMonthlyTarget.LivingRoomFlowInvestmentTarget = addDto.LivingRoomFlowInvestmentTarget;
                liveAnchorMonthlyTarget.LivingRoomCumulativeFlowInvestment = 0;
                liveAnchorMonthlyTarget.LivingRoomFlowInvestmentCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.ConsultationTarget = addDto.ConsultationTarget;
                liveAnchorMonthlyTarget.CumulativeConsultation = 0;
                liveAnchorMonthlyTarget.ConsultationCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.ConsultationTarget2 = addDto.ConsultationTarget2;
                liveAnchorMonthlyTarget.CumulativeConsultation2 = 0;
                liveAnchorMonthlyTarget.ConsultationCompleteRate2 = 0.00M;

                liveAnchorMonthlyTarget.CargoSettlementCommissionTarget = addDto.CargoSettlementCommissionTarget;
                liveAnchorMonthlyTarget.CumulativeCargoSettlementCommission = 0.00M;
                liveAnchorMonthlyTarget.CargoSettlementCommissionCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.LivingRefundCardTarget = addDto.LivingRefundCardTarget;
                liveAnchorMonthlyTarget.CumulativeLivingRefundCard = 0;
                liveAnchorMonthlyTarget.LivingRefundCardCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.GMVTarget = addDto.GMVTarget;
                liveAnchorMonthlyTarget.CumulativeGMV = 0.00m;
                liveAnchorMonthlyTarget.GMVTargetCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.EliminateCardGMVTarget = addDto.EliminateCardGMVTarget;
                liveAnchorMonthlyTarget.CumulativeEliminateCardGMV = 0.00m;
                liveAnchorMonthlyTarget.EliminateCardGMVTargetCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.RefundGMVTarget = addDto.RefundGMVTarget;
                liveAnchorMonthlyTarget.CumulativeRefundGMV = 0.00m;
                liveAnchorMonthlyTarget.RefundGMVTargetCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.CluesTarget = addDto.CluesTarget;
                liveAnchorMonthlyTarget.CumulativeClues = 0;
                liveAnchorMonthlyTarget.CluesTargetCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.CreateDate = DateTime.Now;

                await dalLiveAnchorMonthlyTargetLiving.AddAsync(liveAnchorMonthlyTarget, true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<LiveAnchorMonthlyTargetKeyAndValueDto>> GetIdAndNames(int year, int month)
        {
            try
            {
                var liveAnchorMonthlyTargetLiving = from d in dalLiveAnchorMonthlyTargetLiving.GetAll()
                                                    where (d.Year == year && d.Month == month)
                                                    select new LiveAnchorMonthlyTargetKeyAndValueDto
                                                    {
                                                        Id = d.Id,
                                                        Name = d.MonthlyTargetName
                                                    };
                return liveAnchorMonthlyTargetLiving.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<LiveAnchorMonthlyTargetLivingDto> GetByIdAsync(string id)
        {
            try
            {
                var liveAnchorMonthlyTarget = await dalLiveAnchorMonthlyTargetLiving.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (liveAnchorMonthlyTarget == null)
                {
                    throw new Exception("直播中主播月度运营目标情况编号错误！");
                }

                LiveAnchorMonthlyTargetLivingDto liveAnchorMonthlyTargetDto = new LiveAnchorMonthlyTargetLivingDto();
                liveAnchorMonthlyTargetDto.Id = liveAnchorMonthlyTarget.Id;
                liveAnchorMonthlyTargetDto.Year = liveAnchorMonthlyTarget.Year;
                liveAnchorMonthlyTargetDto.Month = liveAnchorMonthlyTarget.Month;
                liveAnchorMonthlyTargetDto.MonthlyTargetName = liveAnchorMonthlyTarget.MonthlyTargetName;
                liveAnchorMonthlyTargetDto.LiveAnchorId = liveAnchorMonthlyTarget.LiveAnchorId;

                liveAnchorMonthlyTargetDto.LivingRoomCumulativeFlowInvestment = liveAnchorMonthlyTarget.LivingRoomCumulativeFlowInvestment;
                liveAnchorMonthlyTargetDto.LivingRoomFlowInvestmentTarget = liveAnchorMonthlyTarget.LivingRoomFlowInvestmentTarget;
                liveAnchorMonthlyTargetDto.LivingRoomFlowInvestmentCompleteRate = liveAnchorMonthlyTarget.LivingRoomFlowInvestmentCompleteRate;

                liveAnchorMonthlyTargetDto.ConsultationTarget = liveAnchorMonthlyTarget.ConsultationTarget;
                liveAnchorMonthlyTargetDto.ConsultationCompleteRate = liveAnchorMonthlyTarget.ConsultationCompleteRate;
                liveAnchorMonthlyTargetDto.CumulativeConsultation = liveAnchorMonthlyTarget.CumulativeConsultation;
                liveAnchorMonthlyTargetDto.ConsultationTarget2 = liveAnchorMonthlyTarget.ConsultationTarget2;
                liveAnchorMonthlyTargetDto.ConsultationCompleteRate2 = liveAnchorMonthlyTarget.ConsultationCompleteRate2;
                liveAnchorMonthlyTargetDto.CumulativeConsultation2 = liveAnchorMonthlyTarget.CumulativeConsultation2;

                liveAnchorMonthlyTargetDto.CargoSettlementCommissionTarget = liveAnchorMonthlyTarget.CargoSettlementCommissionTarget;
                liveAnchorMonthlyTargetDto.CumulativeCargoSettlementCommission = liveAnchorMonthlyTarget.CumulativeCargoSettlementCommission;
                liveAnchorMonthlyTargetDto.CargoSettlementCommissionCompleteRate = liveAnchorMonthlyTarget.CargoSettlementCommissionCompleteRate;

                liveAnchorMonthlyTargetDto.LivingRefundCardTarget = liveAnchorMonthlyTarget.LivingRefundCardTarget;
                liveAnchorMonthlyTargetDto.CumulativeLivingRefundCard = liveAnchorMonthlyTarget.CumulativeLivingRefundCard;
                liveAnchorMonthlyTargetDto.LivingRefundCardCompleteRate = liveAnchorMonthlyTarget.LivingRefundCardCompleteRate;

                liveAnchorMonthlyTargetDto.GMVTarget = liveAnchorMonthlyTarget.GMVTarget;
                liveAnchorMonthlyTargetDto.CumulativeGMV = liveAnchorMonthlyTarget.CumulativeGMV;
                liveAnchorMonthlyTargetDto.GMVTargetCompleteRate = liveAnchorMonthlyTarget.GMVTargetCompleteRate;

                liveAnchorMonthlyTargetDto.EliminateCardGMVTarget = liveAnchorMonthlyTarget.EliminateCardGMVTarget;
                liveAnchorMonthlyTargetDto.CumulativeEliminateCardGMV = liveAnchorMonthlyTarget.CumulativeEliminateCardGMV;
                liveAnchorMonthlyTargetDto.EliminateCardGMVTargetCompleteRate = liveAnchorMonthlyTarget.EliminateCardGMVTargetCompleteRate;

                liveAnchorMonthlyTargetDto.RefundGMVTarget = liveAnchorMonthlyTarget.RefundGMVTarget;
                liveAnchorMonthlyTargetDto.CumulativeRefundGMV = liveAnchorMonthlyTarget.CumulativeRefundGMV;
                liveAnchorMonthlyTargetDto.RefundGMVTargetCompleteRate = liveAnchorMonthlyTarget.RefundGMVTargetCompleteRate;

                liveAnchorMonthlyTargetDto.CluesTarget = liveAnchorMonthlyTarget.CluesTarget;
                liveAnchorMonthlyTargetDto.CumulativeClues = liveAnchorMonthlyTarget.CumulativeClues;
                liveAnchorMonthlyTargetDto.CluesTargetCompleteRate = liveAnchorMonthlyTarget.CluesTargetCompleteRate;

                liveAnchorMonthlyTargetDto.CreateDate = liveAnchorMonthlyTarget.CreateDate;
                var liveAnchor = await _liveanchorService.GetByIdAsync(liveAnchorMonthlyTargetDto.LiveAnchorId);
                liveAnchorMonthlyTargetDto.ContentPlatFormId = liveAnchor.ContentPlateFormId;
                return liveAnchorMonthlyTargetDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateAsync(UpdateLiveAnchorMonthlyTargetLivingDto updateDto)
        {
            try
            {
                var liveAnchorMonthlyTarget = await dalLiveAnchorMonthlyTargetLiving.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (liveAnchorMonthlyTarget == null)
                    throw new Exception("直播中主播月度运营目标情况编号错误！");

                liveAnchorMonthlyTarget.Year = updateDto.Year;
                liveAnchorMonthlyTarget.Month = updateDto.Month;
                liveAnchorMonthlyTarget.MonthlyTargetName = updateDto.MonthlyTargetName;
                liveAnchorMonthlyTarget.LiveAnchorId = updateDto.LiveAnchorId;

                liveAnchorMonthlyTarget.LivingRoomFlowInvestmentTarget = updateDto.LivingRoomFlowInvestmentTarget;
                liveAnchorMonthlyTarget.ConsultationTarget = updateDto.ConsultationTarget;
                liveAnchorMonthlyTarget.ConsultationTarget2 = updateDto.ConsultationTarget2;
                liveAnchorMonthlyTarget.CargoSettlementCommissionTarget = updateDto.CargoSettlementCommissionTarget;
                liveAnchorMonthlyTarget.LivingRefundCardTarget = updateDto.LivingRefundCardTarget;
                liveAnchorMonthlyTarget.GMVTarget = updateDto.GMVTarget;
                liveAnchorMonthlyTarget.EliminateCardGMVTarget = updateDto.EliminateCardGMVTarget;
                liveAnchorMonthlyTarget.RefundGMVTarget = updateDto.RefundGMVTarget;
                liveAnchorMonthlyTarget.CluesTarget = updateDto.CluesTarget;

                await dalLiveAnchorMonthlyTargetLiving.UpdateAsync(liveAnchorMonthlyTarget, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 更新每日数据时调用并且添加累计信息
        /// </summary>
        /// <param name="editDto"></param>
        /// <returns></returns>
        public async Task EditAsync(UpdateLiveAnchorMonthlyLivingTargetRateAndNumDto editDto)
        {
            try
            {
                var liveAnchorMonthlyTargetLiving = await dalLiveAnchorMonthlyTargetLiving.GetAll().SingleOrDefaultAsync(e => e.Id == editDto.Id);
                if (liveAnchorMonthlyTargetLiving == null)
                    throw new Exception("直播中主播月度运营目标情况编号错误！");

                #region #直播间投流
                liveAnchorMonthlyTargetLiving.LivingRoomCumulativeFlowInvestment += editDto.LivingRoomCumulativeFlowInvestment;
                if (liveAnchorMonthlyTargetLiving.LivingRoomCumulativeFlowInvestment <= 0)
                {
                    liveAnchorMonthlyTargetLiving.LivingRoomFlowInvestmentCompleteRate = 0.00M;
                }
                else
                {

                    liveAnchorMonthlyTargetLiving.LivingRoomFlowInvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.LivingRoomCumulativeFlowInvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.LivingRoomFlowInvestmentTarget)) * 100, 2);
                }
                #endregion

                #region #99面诊卡
                liveAnchorMonthlyTargetLiving.CumulativeConsultation += editDto.CumulativeConsultation;
                if (liveAnchorMonthlyTargetLiving.CumulativeConsultation <= 0)
                {
                    liveAnchorMonthlyTargetLiving.ConsultationCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.ConsultationCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeConsultation) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.ConsultationTarget)) * 100, 2);
                }
                #endregion

                #region #199面诊卡
                liveAnchorMonthlyTargetLiving.CumulativeConsultation2 += editDto.CumulativeConsultation2;
                if (liveAnchorMonthlyTargetLiving.CumulativeConsultation2 <= 0)
                {
                    liveAnchorMonthlyTargetLiving.ConsultationCompleteRate2 = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.ConsultationCompleteRate2 = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeConsultation2) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.ConsultationTarget2)) * 100, 2);
                }
                #endregion

                #region #带货佣金结算
                liveAnchorMonthlyTargetLiving.CumulativeCargoSettlementCommission += editDto.CumulativeCargoSettlementCommission;
                if (liveAnchorMonthlyTargetLiving.CumulativeCargoSettlementCommission <= 0)
                {
                    liveAnchorMonthlyTargetLiving.CargoSettlementCommissionCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.CargoSettlementCommissionCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeCargoSettlementCommission) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CargoSettlementCommissionTarget)) * 100, 2);
                }
                #endregion

                #region 退卡量

                liveAnchorMonthlyTargetLiving.CumulativeLivingRefundCard += editDto.RefundCard;
                if (liveAnchorMonthlyTargetLiving.CumulativeLivingRefundCard <= 0)
                {
                    liveAnchorMonthlyTargetLiving.LivingRefundCardCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.LivingRefundCardCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeLivingRefundCard) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.LivingRefundCardTarget)) * 100, 2);
                }

                #endregion

                #region GMV

                liveAnchorMonthlyTargetLiving.CumulativeGMV += editDto.GMV;
                if (liveAnchorMonthlyTargetLiving.CumulativeGMV <= 0)
                {
                    liveAnchorMonthlyTargetLiving.GMVTargetCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.GMVTargetCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeGMV) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.GMVTarget)) * 100, 2);
                }

                #endregion

                #region 去卡量GMV

                liveAnchorMonthlyTargetLiving.CumulativeEliminateCardGMV += editDto.EliminateCardGMV;
                if (liveAnchorMonthlyTargetLiving.CumulativeEliminateCardGMV <= 0)
                {
                    liveAnchorMonthlyTargetLiving.EliminateCardGMVTargetCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.EliminateCardGMVTargetCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeEliminateCardGMV) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.EliminateCardGMVTarget)) * 100, 2);
                }

                #endregion

                #region 退款gmv

                liveAnchorMonthlyTargetLiving.CumulativeRefundGMV += editDto.RefundGMV;
                if (liveAnchorMonthlyTargetLiving.CumulativeRefundGMV <= 0)
                {
                    liveAnchorMonthlyTargetLiving.RefundGMVTargetCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.RefundGMVTargetCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeRefundGMV) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.RefundGMVTarget)) * 100, 2);
                }

                #endregion

                #region 线索量

                liveAnchorMonthlyTargetLiving.CumulativeClues += editDto.Clues;
                if (liveAnchorMonthlyTargetLiving.CumulativeClues <= 0)
                {
                    liveAnchorMonthlyTargetLiving.CluesTargetCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetLiving.CluesTargetCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CumulativeClues) / Convert.ToDecimal(liveAnchorMonthlyTargetLiving.CluesTarget)) * 100, 2);
                }

                #endregion

                await dalLiveAnchorMonthlyTargetLiving.UpdateAsync(liveAnchorMonthlyTargetLiving, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var liveAnchorMonthlyTargetLiving = await dalLiveAnchorMonthlyTargetLiving.GetAll().SingleOrDefaultAsync(e => e.Id == id);

                if (liveAnchorMonthlyTargetLiving == null)
                    throw new Exception("直播中主播月度运营目标情况编号错误");

                await dalLiveAnchorMonthlyTargetLiving.DeleteAsync(liveAnchorMonthlyTargetLiving, true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 获取业绩目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<LiveAnchorMonthTargetPerformanceDto> GetPerformance(int year, int month, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
            LiveAnchorMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorMonthTargetPerformanceDto
            {
                CommercePerformanceTarget = await performance.SumAsync(t => t.CargoSettlementCommissionTarget),
                CommerceCompletePerformance = await performance.SumAsync(t => t.CumulativeCargoSettlementCommission),

            };
            return performanceInfoDto;
        }



        /// <summary>
        /// 获取带货业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<PerformanceInfoByDateDto>> GetLiveAnchorCommercePerformance(int year, int month, List<int> liveAnchorIds)
        {
            var list = dalLiveAnchorMonthlyTargetLiving.GetAll()
                .Where(o => o.Year == year && o.Month >= 1 && o.Month <= month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .GroupBy(o => o.Month).OrderBy(o => o.Key).Select(o => new PerformanceInfoByDateDto
                {
                    Date = o.Key.ToString(),
                    PerfomancePrice = o.Sum(o => o.CumulativeCargoSettlementCommission)
                }).ToList();
            return list;
        }
        /// <summary>
        /// 基础经营看板的面诊卡下单业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetBasePerformanceTargetAsync(int year, int month, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
            LiveAnchorBaseBusinessMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetPerformanceDto
            {
                ConsulationCardTarget = await performance.SumAsync(t => t.ConsultationTarget + t.ConsultationTarget2),
                GMVTarget = await performance.SumAsync(e => e.GMVTarget),
                LivingRoomFlowInvestmentTarget = await performance.SumAsync(e => e.LivingRoomFlowInvestmentTarget),
                LivingRoomCumulativeFlowInvestment = await performance.SumAsync(e => e.LivingRoomCumulativeFlowInvestment),
                RefundGMVTarget = await performance.SumAsync(e => e.RefundGMVTarget)
            };
            return performanceInfoDto;
        }
        /// <summary>
        /// 根据年份和主播id获取设计卡下单目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<AmiyaOperationBoardCluesChannelTargetDto>> GetCluesTargetByYearAsync(int year, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetLiving.GetAll().Where(t => t.Year == year)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId)).Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new AmiyaOperationBoardCluesChannelTargetDto
                {
                    CluesTarget = e.ConsultationTarget + e.ConsultationTarget2,
                    Month = e.Month,
                    LiveAnchorId = e.LiveAnchorId
                })
                .ToList();
            return performance;
        }

        /// <summary>
        /// 根据时间获取下卡量目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetConsulationCardAddTargetByDateAsync(int year, int month)
        {
            var performance = dalLiveAnchorMonthlyTargetLiving.GetAll().Where(t => t.Year == year && t.Month == month);
            LiveAnchorBaseBusinessMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetPerformanceDto
            {
                ConsulationCardTarget = await performance.SumAsync(t => t.ConsultationTarget + t.ConsultationTarget2),
                LivingRefundCardTarget = await performance.SumAsync(t => t.LivingRefundCardTarget)
            };
            return performanceInfoDto;
        }
        /// <summary>
        /// 根据时间获取各主播下卡量目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorBaseBusinessMonthTargetPerformanceDto>> GetConsulationCardAddTargetByDateAsync(int year, int month, List<string> baseLiveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetLiving.GetAll().Include(x => x.LiveAnchor).Where(t => t.Year == year && t.Month == month).Where(e => baseLiveAnchorIds.Contains(e.LiveAnchor.LiveAnchorBaseId));
            var dataList = performance.GroupBy(e => e.LiveAnchor.LiveAnchorBaseId).Select(e => new LiveAnchorBaseBusinessMonthTargetPerformanceDto
            {
                ConsulationCardTarget = e.Sum(t => t.ConsultationTarget + t.ConsultationTarget2),
                LivingRefundCardTarget = e.Sum(t => t.LivingRefundCardTarget),
                BaseLiveAnchorId = e.Key
            }).ToList();
            return dataList;
        }
        /// <summary>
        /// 根据主播id集合获取指定月份的月目标id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<string>> GetTargetIdsAsync(int year, int month, List<int> liveAnchorIds)
        {
            return dalLiveAnchorMonthlyTargetLiving.GetAll().Where(e => e.Year == year && e.Month == month && liveAnchorIds.Contains(e.LiveAnchorId)).Select(e => e.Id).ToList();
        }
        /// <summary>
        /// 获取直播中目标信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<LivingTargetDto> GetTargetAsync(QueryLivingBusinessDataDto queryDto)
        {
            var res = dalLiveAnchorMonthlyTargetLiving.GetAll().Where(e => e.Year == queryDto.Year && e.Month == queryDto.Month);
            if (!string.IsNullOrEmpty(queryDto.BaseLiveAnchorId))
            {
                res = res.Where(e => e.LiveAnchor.LiveAnchorBaseId == queryDto.BaseLiveAnchorId);
            }
            if (res.ToList().Count == 0) return null;
            LivingTargetDto livingTargetDto = new LivingTargetDto();
            if (queryDto.ShowTikokData)
            {
                var tiktok = res.Where(e => e.LiveAnchor.ContentPlateFormId == "4e4e9564-f6c3-47b6-a7da-e4518bab66a1");
                livingTargetDto.OrderGMVTarget += tiktok.Sum(e => e.EliminateCardGMVTarget);
                livingTargetDto.RefundGMVTarget += tiktok.Sum(e => e.RefundGMVTarget);
                livingTargetDto.ActualReturnBackMoneyTarget += tiktok.Sum(e => e.CumulativeCargoSettlementCommission);
                livingTargetDto.InvestFlowTarget += tiktok.Sum(e => e.LivingRoomFlowInvestmentTarget);
                livingTargetDto.DesignCardOrderTarget += tiktok.Sum(e => e.ConsultationTarget) + res.Sum(e => e.ConsultationTarget2);
                livingTargetDto.DesignCardRefundTarget += tiktok.Sum(e => e.LivingRefundCardTarget);
            }
            if (queryDto.ShowWechatVideoData)
            {
                var wechat = res.Where(e => e.LiveAnchor.ContentPlateFormId == "9196b247-1ab9-4d0c-a11e-a1ef09019878");
                livingTargetDto.OrderGMVTarget += wechat.Sum(e => e.EliminateCardGMVTarget);
                livingTargetDto.RefundGMVTarget += wechat.Sum(e => e.RefundGMVTarget);
                livingTargetDto.ActualReturnBackMoneyTarget += wechat.Sum(e => e.CumulativeCargoSettlementCommission);
                livingTargetDto.InvestFlowTarget += wechat.Sum(e => e.LivingRoomFlowInvestmentTarget);
                livingTargetDto.DesignCardOrderTarget += wechat.Sum(e => e.ConsultationTarget) + wechat.Sum(e => e.ConsultationTarget2);
                livingTargetDto.DesignCardRefundTarget += wechat.Sum(e => e.LivingRefundCardTarget);
            }

            return livingTargetDto;
        }
    }
}
