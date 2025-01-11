using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.LiveAnchorMonthlyTarget;
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
    public class LiveAnchorMonthlyTargetAfterLivingService : ILiveAnchorMonthlyTargetAfterLivingService
    {
        private IDalLiveAnchorMonthlyTargetAfterLiving dalLiveAnchorMonthlyTargetAfterLiving;
        private ILiveAnchorService _liveanchorService;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private IEmployeeBindLiveAnchorService employeeBindLiveAnchorService;

        public LiveAnchorMonthlyTargetAfterLivingService(IDalLiveAnchorMonthlyTargetAfterLiving dalLiveAnchorMonthlyTargetAfterLiving,
            ILiveAnchorService liveAnchorService,
            IEmployeeBindLiveAnchorService employeeBindLiveAnchorService,
            IAmiyaEmployeeService amiyaEmployeeService)
        {
            this.dalLiveAnchorMonthlyTargetAfterLiving = dalLiveAnchorMonthlyTargetAfterLiving;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.employeeBindLiveAnchorService = employeeBindLiveAnchorService;
            _liveanchorService = liveAnchorService;
        }



        public async Task<FxPageInfo<LiveAnchorMonthlyTargetAfterLivingDto>> GetListWithPageAsync(int Year, int Month, int? LiveAnchorId, int employeeId, int pageNum, int pageSize)
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
                var liveAnchorMonthlyTargetAfterLiving = from d in dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Include(e => e.LiveAnchor)
                                                         where (d.Year == Year)
                                                         && (d.Month == Month)
                                                         && (liveAnchorIds.Count == 0 || liveAnchorIds.Contains(d.LiveAnchorId))
                                                         select new LiveAnchorMonthlyTargetAfterLivingDto
                                                         {
                                                             Id = d.Id,
                                                             Year = d.Year,
                                                             Month = d.Month,
                                                             MonthlyTargetName = d.MonthlyTargetName,
                                                             LiveAnchorId = d.LiveAnchorId,
                                                             LiveAnchorName = d.LiveAnchor.Name,
                                                             ContentPlatFormId = d.LiveAnchor.ContentPlateFormId,
                                                             AddWechatTarget = d.AddWechatTarget,
                                                             CumulativeAddWechat = d.CumulativeAddWechat,
                                                             AddWechatCompleteRate = d.AddWechatCompleteRate,
                                                             ConsultationCardConsumedTarget = d.ConsultationCardConsumedTarget,
                                                             CumulativeConsultationCardConsumed = d.CumulativeConsultationCardConsumed,
                                                             ConsultationCardConsumedCompleteRate = d.ConsultationCardConsumedCompleteRate,
                                                             ConsultationCardConsumedTarget2 = d.ConsultationCardConsumedTarget2,
                                                             CumulativeConsultationCardConsumed2 = d.CumulativeConsultationCardConsumed2,
                                                             ConsultationCardConsumedCompleteRate2 = d.ConsultationCardConsumedCompleteRate2,
                                                             ActivateHistoricalConsultationTarget = d.ActivateHistoricalConsultationTarget,
                                                             CumulativeActivateHistoricalConsultation = d.CumulativeActivateHistoricalConsultation,
                                                             ActivateHistoricalConsultationCompleteRate = d.ActivateHistoricalConsultationCompleteRate,
                                                             SendOrderTarget = d.SendOrderTarget,
                                                             CumulativeSendOrder = d.CumulativeSendOrder,
                                                             SendOrderCompleteRate = d.SendOrderCompleteRate,
                                                             NewCustomerVisitTarget = d.NewCustomerVisitTarget,
                                                             CumulativeNewCustomerVisit = d.CumulativeNewCustomerVisit,
                                                             NewCustomerVisitCompleteRate = d.NewCustomerVisitCompleteRate,
                                                             OldCustomerVisitTarget = d.OldCustomerVisitTarget,
                                                             CumulativeOldCustomerVisit = d.CumulativeOldCustomerVisit,
                                                             OldCustomerVisitCompleteRate = d.OldCustomerVisitCompleteRate,
                                                             VisitTarget = d.VisitTarget,
                                                             CumulativeVisit = d.CumulativeVisit,
                                                             VisitCompleteRate = d.VisitCompleteRate,
                                                             NewCustomerDealTarget = d.NewCustomerDealTarget,
                                                             CumulativeNewCustomerDealTarget = d.CumulativeNewCustomerDealTarget,
                                                             NewCustomerDealRate = d.NewCustomerDealRate,
                                                             OldCustomerDealTarget = d.OldCustomerDealTarget,
                                                             CumulativeOldCustomerDealTarget = d.CumulativeOldCustomerDealTarget,
                                                             OldCustomerDealRate = d.OldCustomerDealRate,
                                                             DealTarget = d.DealTarget,
                                                             CumulativeDealTarget = d.CumulativeDealTarget,
                                                             DealRate = d.DealRate,
                                                             PerformanceTarget = d.PerformanceTarget,
                                                             CumulativePerformance = d.CumulativePerformance,
                                                             PerformanceCompleteRate = d.PerformanceCompleteRate,
                                                             NewCustomerPerformanceTarget = d.NewCustomerPerformanceTarget,
                                                             NewCustomerPerformanceCompleteRate = d.NewCustomerPerformanceCompleteRate,
                                                             CumulativeNewCustomerPerformance = d.CumulativeNewCustomerPerformance,
                                                             OldCustomerPerformanceTarget = d.OldCustomerPerformanceTarget,
                                                             OldCustomerPerformanceCompleteRate = d.OldCustomerPerformanceCompleteRate,
                                                             CumulativeOldCustomerPerformance = d.CumulativeOldCustomerPerformance,
                                                             SubsequentPerformanceCompleteRate = d.SubsequentPerformanceCompleteRate,
                                                             SubsequentPerformanceTarget = d.SubsequentPerformanceTarget,
                                                             CumulativeSubsequentPerformance = d.CumulativeSubsequentPerformance,
                                                             MinivanRefundTarget = d.MinivanRefundTarget,
                                                             CumulativeMinivanRefund = d.CumulativeMinivanRefund,
                                                             MinivanRefundCompleteRate = d.MinivanRefundCompleteRate,
                                                             MiniVanBadReviewsTarget = d.MiniVanBadReviewsTarget,
                                                             CumulativeMiniVanBadReviews = d.CumulativeMiniVanBadReviews,
                                                             MiniVanBadReviewsCompleteRate = d.MiniVanBadReviewsCompleteRate,
                                                             CreateDate = d.CreateDate,
                                                             EffectivePerformanceTarget = d.EffectivePerformanceTarget,
                                                             CumulativeEffectivePerformance = d.CumulativeEffectivePerformance,
                                                             EffectivePerformanceCompleteRate = d.EffectivePerformanceCompleteRate,
                                                             PotentialPerformanceTarget = d.PotentialPerformanceTarget,
                                                             CumulativePotentialPerformance = d.CumulativePotentialPerformance,
                                                             PotentialPerformanceCompleteRate = d.PotentialPerformanceCompleteRate,
                                                             DistributeConsulationTarget = d.DistributeConsulationTarget,
                                                             CumulativeDistributeConsulation = d.CumulativeDistributeConsulation,
                                                             DistributeConsulationCompleteRate = d.DistributeConsulationCompleteRate,
                                                             CluesTarget = d.CluesTarget,
                                                             CumulativeClues = d.CumulativeClues,
                                                             CluesCompleteRate = d.CluesCompleteRate
                                                         };

                FxPageInfo<LiveAnchorMonthlyTargetAfterLivingDto> liveAnchorMonthlyTargetAfterLivingPageInfo = new FxPageInfo<LiveAnchorMonthlyTargetAfterLivingDto>();
                liveAnchorMonthlyTargetAfterLivingPageInfo.TotalCount = await liveAnchorMonthlyTargetAfterLiving.CountAsync();
                liveAnchorMonthlyTargetAfterLivingPageInfo.List = await liveAnchorMonthlyTargetAfterLiving.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                return liveAnchorMonthlyTargetAfterLivingPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task AddAsync(AddLiveAnchorMonthlyTargetAfterLivingDto addDto)
        {
            try
            {
                LiveAnchorMonthlyTargetAfterLiving liveAnchorMonthlyTarget = new LiveAnchorMonthlyTargetAfterLiving();
                liveAnchorMonthlyTarget.Id = Guid.NewGuid().ToString();
                liveAnchorMonthlyTarget.Year = addDto.Year;
                liveAnchorMonthlyTarget.Month = addDto.Month;
                liveAnchorMonthlyTarget.MonthlyTargetName = addDto.MonthlyTargetName;
                liveAnchorMonthlyTarget.LiveAnchorId = addDto.LiveAnchorId;


                liveAnchorMonthlyTarget.AddWechatTarget = addDto.AddWechatTarget;
                liveAnchorMonthlyTarget.CumulativeAddWechat = 0;
                liveAnchorMonthlyTarget.AddWechatCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.ConsultationCardConsumedTarget = addDto.ConsultationCardConsumedTarget;
                liveAnchorMonthlyTarget.CumulativeConsultationCardConsumed = 0;
                liveAnchorMonthlyTarget.ConsultationCardConsumedCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.ConsultationCardConsumedTarget2 = addDto.ConsultationCardConsumedTarget2;
                liveAnchorMonthlyTarget.CumulativeConsultationCardConsumed2 = 0;
                liveAnchorMonthlyTarget.ConsultationCardConsumedCompleteRate2 = 0.00M;
                liveAnchorMonthlyTarget.ActivateHistoricalConsultationTarget = addDto.ActivateHistoricalConsultationTarget;
                liveAnchorMonthlyTarget.CumulativeActivateHistoricalConsultation = 0;
                liveAnchorMonthlyTarget.ActivateHistoricalConsultationCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.SendOrderTarget = addDto.SendOrderTarget;
                liveAnchorMonthlyTarget.CumulativeSendOrder = 0;
                liveAnchorMonthlyTarget.SendOrderCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.NewCustomerVisitTarget = addDto.NewCustomerVisitTarget;
                liveAnchorMonthlyTarget.CumulativeNewCustomerVisit = 0;
                liveAnchorMonthlyTarget.NewCustomerVisitCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.OldCustomerVisitTarget = addDto.OldCustomerVisitTarget;
                liveAnchorMonthlyTarget.CumulativeOldCustomerVisit = 0;
                liveAnchorMonthlyTarget.OldCustomerVisitCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.VisitTarget = addDto.VisitTarget;
                liveAnchorMonthlyTarget.CumulativeVisit = 0;
                liveAnchorMonthlyTarget.VisitCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.NewCustomerDealTarget = addDto.NewCustomerDealTarget;
                liveAnchorMonthlyTarget.CumulativeNewCustomerDealTarget = 0;
                liveAnchorMonthlyTarget.NewCustomerDealRate = 0.00M;

                liveAnchorMonthlyTarget.OldCustomerDealTarget = addDto.OldCustomerDealTarget;
                liveAnchorMonthlyTarget.CumulativeOldCustomerDealTarget = 0;
                liveAnchorMonthlyTarget.OldCustomerDealRate = 0.00M;

                liveAnchorMonthlyTarget.DealTarget = addDto.DealTarget;
                liveAnchorMonthlyTarget.CumulativeDealTarget = 0;
                liveAnchorMonthlyTarget.DealRate = 0.00M;

                liveAnchorMonthlyTarget.PerformanceTarget = addDto.PerformanceTarget;
                liveAnchorMonthlyTarget.CumulativePerformance = 0.00M;
                liveAnchorMonthlyTarget.PerformanceCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.NewCustomerPerformanceTarget = addDto.NewCustomerPerformanceTarget;
                liveAnchorMonthlyTarget.CumulativeNewCustomerPerformance = 0.00M;
                liveAnchorMonthlyTarget.NewCustomerPerformanceCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.OldCustomerPerformanceTarget = addDto.OldCustomerPerformanceTarget;
                liveAnchorMonthlyTarget.CumulativeOldCustomerPerformance = 0.00M;
                liveAnchorMonthlyTarget.OldCustomerPerformanceCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.SubsequentPerformanceTarget = addDto.SubsequentPerformanceTarget;
                liveAnchorMonthlyTarget.CumulativeSubsequentPerformance = 0.00M;
                liveAnchorMonthlyTarget.SubsequentPerformanceCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.MinivanRefundTarget = addDto.MinivanRefundTarget;
                liveAnchorMonthlyTarget.CumulativeMinivanRefund = 0;
                liveAnchorMonthlyTarget.MinivanRefundCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.MiniVanBadReviewsTarget = addDto.MiniVanBadReviewsTarget;
                liveAnchorMonthlyTarget.CumulativeMiniVanBadReviews = 0;
                liveAnchorMonthlyTarget.MiniVanBadReviewsCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.EffectivePerformanceTarget = addDto.EffectivePerformanceTarget;
                liveAnchorMonthlyTarget.CumulativeEffectivePerformance = 0.00m;
                liveAnchorMonthlyTarget.EffectivePerformanceCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.PotentialPerformanceTarget = addDto.PotentialPerformanceTarget;
                liveAnchorMonthlyTarget.CumulativeEffectivePerformance = 0.00m;
                liveAnchorMonthlyTarget.PotentialPerformanceCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.DistributeConsulationTarget = addDto.DistributeConsulationTarget;
                liveAnchorMonthlyTarget.CumulativeDistributeConsulation = 0;
                liveAnchorMonthlyTarget.DistributeConsulationCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.CluesTarget = addDto.CluesTarget;
                liveAnchorMonthlyTarget.CumulativeClues = 0;
                liveAnchorMonthlyTarget.CluesCompleteRate = 0.00m;

                liveAnchorMonthlyTarget.CreateDate = DateTime.Now;

                await dalLiveAnchorMonthlyTargetAfterLiving.AddAsync(liveAnchorMonthlyTarget, true);
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
                var liveAnchorMonthlyTargetAfterLiving = from d in dalLiveAnchorMonthlyTargetAfterLiving.GetAll()
                                                         where (d.Year == year && d.Month == month)
                                                         select new LiveAnchorMonthlyTargetKeyAndValueDto
                                                         {
                                                             Id = d.Id,
                                                             Name = d.MonthlyTargetName
                                                         };
                return liveAnchorMonthlyTargetAfterLiving.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<LiveAnchorMonthlyTargetAfterLivingDto> GetByIdAsync(string id)
        {
            try
            {
                var liveAnchorMonthlyTarget = await dalLiveAnchorMonthlyTargetAfterLiving.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (liveAnchorMonthlyTarget == null)
                {
                    throw new Exception("直播中主播月度运营目标情况编号错误！");
                }

                LiveAnchorMonthlyTargetAfterLivingDto liveAnchorMonthlyTargetDto = new LiveAnchorMonthlyTargetAfterLivingDto();
                liveAnchorMonthlyTargetDto.Id = liveAnchorMonthlyTarget.Id;
                liveAnchorMonthlyTargetDto.Year = liveAnchorMonthlyTarget.Year;
                liveAnchorMonthlyTargetDto.Month = liveAnchorMonthlyTarget.Month;
                liveAnchorMonthlyTargetDto.MonthlyTargetName = liveAnchorMonthlyTarget.MonthlyTargetName;
                liveAnchorMonthlyTargetDto.LiveAnchorId = liveAnchorMonthlyTarget.LiveAnchorId;

                liveAnchorMonthlyTargetDto.AddWechatTarget = liveAnchorMonthlyTarget.AddWechatTarget;
                liveAnchorMonthlyTargetDto.CumulativeAddWechat = liveAnchorMonthlyTarget.CumulativeAddWechat;
                liveAnchorMonthlyTargetDto.AddWechatCompleteRate = liveAnchorMonthlyTarget.AddWechatCompleteRate;
                liveAnchorMonthlyTargetDto.ConsultationCardConsumedTarget = liveAnchorMonthlyTarget.ConsultationCardConsumedTarget;
                liveAnchorMonthlyTargetDto.CumulativeConsultationCardConsumed = liveAnchorMonthlyTarget.CumulativeConsultationCardConsumed;
                liveAnchorMonthlyTargetDto.ConsultationCardConsumedCompleteRate = liveAnchorMonthlyTarget.ConsultationCardConsumedCompleteRate;
                liveAnchorMonthlyTargetDto.ConsultationCardConsumedTarget2 = liveAnchorMonthlyTarget.ConsultationCardConsumedTarget2;
                liveAnchorMonthlyTargetDto.CumulativeConsultationCardConsumed2 = liveAnchorMonthlyTarget.CumulativeConsultationCardConsumed2;
                liveAnchorMonthlyTargetDto.ConsultationCardConsumedCompleteRate2 = liveAnchorMonthlyTarget.ConsultationCardConsumedCompleteRate2;
                liveAnchorMonthlyTargetDto.ActivateHistoricalConsultationTarget = liveAnchorMonthlyTarget.ActivateHistoricalConsultationTarget;
                liveAnchorMonthlyTargetDto.CumulativeActivateHistoricalConsultation = liveAnchorMonthlyTarget.CumulativeActivateHistoricalConsultation;
                liveAnchorMonthlyTargetDto.ActivateHistoricalConsultationCompleteRate = liveAnchorMonthlyTarget.ActivateHistoricalConsultationCompleteRate;
                liveAnchorMonthlyTargetDto.SendOrderTarget = liveAnchorMonthlyTarget.SendOrderTarget;
                liveAnchorMonthlyTargetDto.CumulativeSendOrder = liveAnchorMonthlyTarget.CumulativeSendOrder;
                liveAnchorMonthlyTargetDto.SendOrderCompleteRate = liveAnchorMonthlyTarget.SendOrderCompleteRate;

                liveAnchorMonthlyTargetDto.NewCustomerVisitTarget = liveAnchorMonthlyTarget.NewCustomerVisitTarget;
                liveAnchorMonthlyTargetDto.CumulativeNewCustomerVisit = liveAnchorMonthlyTarget.CumulativeNewCustomerVisit;
                liveAnchorMonthlyTargetDto.NewCustomerVisitCompleteRate = liveAnchorMonthlyTarget.NewCustomerVisitCompleteRate;

                liveAnchorMonthlyTargetDto.OldCustomerVisitTarget = liveAnchorMonthlyTarget.OldCustomerVisitTarget;
                liveAnchorMonthlyTargetDto.CumulativeOldCustomerVisit = liveAnchorMonthlyTarget.CumulativeOldCustomerVisit;
                liveAnchorMonthlyTargetDto.OldCustomerVisitCompleteRate = liveAnchorMonthlyTarget.OldCustomerVisitCompleteRate;

                liveAnchorMonthlyTargetDto.VisitTarget = liveAnchorMonthlyTarget.VisitTarget;
                liveAnchorMonthlyTargetDto.CumulativeVisit = liveAnchorMonthlyTarget.CumulativeVisit;
                liveAnchorMonthlyTargetDto.VisitCompleteRate = liveAnchorMonthlyTarget.VisitCompleteRate;

                liveAnchorMonthlyTargetDto.NewCustomerDealTarget = liveAnchorMonthlyTarget.NewCustomerDealTarget;
                liveAnchorMonthlyTargetDto.CumulativeNewCustomerDealTarget = liveAnchorMonthlyTarget.CumulativeNewCustomerDealTarget;
                liveAnchorMonthlyTargetDto.NewCustomerDealRate = liveAnchorMonthlyTarget.NewCustomerDealRate;

                liveAnchorMonthlyTargetDto.OldCustomerDealTarget = liveAnchorMonthlyTarget.OldCustomerDealTarget;
                liveAnchorMonthlyTargetDto.CumulativeOldCustomerDealTarget = liveAnchorMonthlyTarget.CumulativeOldCustomerDealTarget;
                liveAnchorMonthlyTargetDto.OldCustomerDealRate = liveAnchorMonthlyTarget.OldCustomerDealRate;

                liveAnchorMonthlyTargetDto.DealTarget = liveAnchorMonthlyTarget.DealTarget;
                liveAnchorMonthlyTargetDto.CumulativeDealTarget = liveAnchorMonthlyTarget.CumulativeDealTarget;
                liveAnchorMonthlyTargetDto.DealRate = liveAnchorMonthlyTarget.DealRate;



                liveAnchorMonthlyTargetDto.NewCustomerPerformanceTarget = liveAnchorMonthlyTarget.NewCustomerPerformanceTarget;
                liveAnchorMonthlyTargetDto.CumulativeNewCustomerPerformance = liveAnchorMonthlyTarget.CumulativeNewCustomerPerformance;
                liveAnchorMonthlyTargetDto.NewCustomerPerformanceCompleteRate = liveAnchorMonthlyTarget.NewCustomerPerformanceCompleteRate;

                liveAnchorMonthlyTargetDto.SubsequentPerformanceTarget = liveAnchorMonthlyTarget.SubsequentPerformanceTarget;
                liveAnchorMonthlyTargetDto.CumulativeSubsequentPerformance = liveAnchorMonthlyTarget.CumulativeSubsequentPerformance;
                liveAnchorMonthlyTargetDto.SubsequentPerformanceCompleteRate = liveAnchorMonthlyTarget.SubsequentPerformanceCompleteRate;

                liveAnchorMonthlyTargetDto.OldCustomerPerformanceTarget = liveAnchorMonthlyTarget.OldCustomerPerformanceTarget;
                liveAnchorMonthlyTargetDto.CumulativeOldCustomerPerformance = liveAnchorMonthlyTarget.CumulativeOldCustomerPerformance;
                liveAnchorMonthlyTargetDto.OldCustomerPerformanceCompleteRate = liveAnchorMonthlyTarget.OldCustomerPerformanceCompleteRate;

                liveAnchorMonthlyTargetDto.PerformanceTarget = liveAnchorMonthlyTarget.PerformanceTarget;
                liveAnchorMonthlyTargetDto.CumulativePerformance = liveAnchorMonthlyTarget.CumulativePerformance;
                liveAnchorMonthlyTargetDto.PerformanceCompleteRate = liveAnchorMonthlyTarget.PerformanceCompleteRate;
                liveAnchorMonthlyTargetDto.MinivanRefundTarget = liveAnchorMonthlyTarget.MinivanRefundTarget;
                liveAnchorMonthlyTargetDto.CumulativeMinivanRefund = liveAnchorMonthlyTarget.CumulativeMinivanRefund;
                liveAnchorMonthlyTargetDto.MinivanRefundCompleteRate = liveAnchorMonthlyTarget.MinivanRefundCompleteRate;
                liveAnchorMonthlyTargetDto.MiniVanBadReviewsTarget = liveAnchorMonthlyTarget.MiniVanBadReviewsTarget;
                liveAnchorMonthlyTargetDto.CumulativeMiniVanBadReviews = liveAnchorMonthlyTarget.CumulativeMiniVanBadReviews;
                liveAnchorMonthlyTargetDto.MiniVanBadReviewsCompleteRate = liveAnchorMonthlyTarget.MiniVanBadReviewsCompleteRate;
                liveAnchorMonthlyTargetDto.CreateDate = liveAnchorMonthlyTarget.CreateDate;

                liveAnchorMonthlyTargetDto.EffectivePerformanceTarget = liveAnchorMonthlyTarget.EffectivePerformanceTarget;
                liveAnchorMonthlyTargetDto.CumulativeEffectivePerformance = liveAnchorMonthlyTarget.CumulativeEffectivePerformance;
                liveAnchorMonthlyTargetDto.EffectivePerformanceCompleteRate = liveAnchorMonthlyTarget.EffectivePerformanceCompleteRate;
                liveAnchorMonthlyTargetDto.PotentialPerformanceTarget = liveAnchorMonthlyTarget.PotentialPerformanceTarget;
                liveAnchorMonthlyTargetDto.CumulativePotentialPerformance = liveAnchorMonthlyTarget.CumulativePotentialPerformance;
                liveAnchorMonthlyTargetDto.PotentialPerformanceCompleteRate = liveAnchorMonthlyTarget.PotentialPerformanceCompleteRate;

                liveAnchorMonthlyTargetDto.DistributeConsulationTarget = liveAnchorMonthlyTarget.DistributeConsulationTarget;
                liveAnchorMonthlyTargetDto.CumulativeDistributeConsulation = liveAnchorMonthlyTarget.CumulativeDistributeConsulation;
                liveAnchorMonthlyTargetDto.DistributeConsulationCompleteRate = liveAnchorMonthlyTarget.DistributeConsulationCompleteRate;

                liveAnchorMonthlyTargetDto.CluesTarget = liveAnchorMonthlyTarget.CluesTarget;
                liveAnchorMonthlyTargetDto.CumulativeClues = liveAnchorMonthlyTarget.CumulativeClues;
                liveAnchorMonthlyTargetDto.CluesCompleteRate = liveAnchorMonthlyTarget.CluesCompleteRate;

                var liveAnchor = await _liveanchorService.GetByIdAsync(liveAnchorMonthlyTargetDto.LiveAnchorId);
                liveAnchorMonthlyTargetDto.ContentPlatFormId = liveAnchor.ContentPlateFormId;
                liveAnchorMonthlyTargetDto.CreateDate = liveAnchorMonthlyTarget.CreateDate;
                return liveAnchorMonthlyTargetDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateAsync(UpdateLiveAnchorMonthlyTargetAfterLivingDto updateDto)
        {
            try
            {
                var liveAnchorMonthlyTarget = await dalLiveAnchorMonthlyTargetAfterLiving.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (liveAnchorMonthlyTarget == null)
                    throw new Exception("直播中主播月度运营目标情况编号错误！");

                liveAnchorMonthlyTarget.Year = updateDto.Year;
                liveAnchorMonthlyTarget.Month = updateDto.Month;
                liveAnchorMonthlyTarget.MonthlyTargetName = updateDto.MonthlyTargetName;
                liveAnchorMonthlyTarget.LiveAnchorId = updateDto.LiveAnchorId;
                liveAnchorMonthlyTarget.AddWechatTarget = updateDto.AddWechatTarget;
                liveAnchorMonthlyTarget.ConsultationCardConsumedTarget = updateDto.ConsultationCardConsumedTarget;
                liveAnchorMonthlyTarget.ConsultationCardConsumedTarget2 = updateDto.ConsultationCardConsumedTarget2;
                liveAnchorMonthlyTarget.ActivateHistoricalConsultationTarget = updateDto.ActivateHistoricalConsultationTarget;
                liveAnchorMonthlyTarget.SendOrderTarget = updateDto.SendOrderTarget;
                liveAnchorMonthlyTarget.NewCustomerVisitTarget = updateDto.NewCustomerVisitTarget;
                liveAnchorMonthlyTarget.OldCustomerVisitTarget = updateDto.OldCustomerVisitTarget;
                liveAnchorMonthlyTarget.VisitTarget = updateDto.VisitTarget;
                liveAnchorMonthlyTarget.NewCustomerDealTarget = updateDto.NewCustomerDealTarget;
                liveAnchorMonthlyTarget.OldCustomerDealTarget = updateDto.OldCustomerDealTarget;
                liveAnchorMonthlyTarget.DealTarget = updateDto.DealTarget;
                liveAnchorMonthlyTarget.MinivanRefundTarget = updateDto.MinivanRefundTarget;
                liveAnchorMonthlyTarget.MiniVanBadReviewsTarget = updateDto.MiniVanBadReviewsTarget;
                liveAnchorMonthlyTarget.NewCustomerPerformanceTarget = updateDto.NewCustomerPerformanceTarget;
                liveAnchorMonthlyTarget.SubsequentPerformanceTarget = updateDto.SubsequentPerformanceTarget;
                liveAnchorMonthlyTarget.OldCustomerPerformanceTarget = updateDto.OldCustomerPerformanceTarget;
                liveAnchorMonthlyTarget.PerformanceTarget = updateDto.PerformanceTarget;
                liveAnchorMonthlyTarget.EffectivePerformanceTarget = updateDto.EffectivePerformanceTarget;
                liveAnchorMonthlyTarget.PotentialPerformanceTarget = updateDto.PotentialPerformanceTarget;
                liveAnchorMonthlyTarget.DistributeConsulationTarget = updateDto.DistributeConsulationTarget;
                liveAnchorMonthlyTarget.CluesTarget = updateDto.CluesTarget;
                await dalLiveAnchorMonthlyTargetAfterLiving.UpdateAsync(liveAnchorMonthlyTarget, true);
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
        public async Task EditAsync(UpdateLiveAnchorMonthlyAfterLivingTargetRateAndNumDto editDto)
        {
            try
            {
                var liveAnchorMonthlyTargetAfterLiving = await dalLiveAnchorMonthlyTargetAfterLiving.GetAll().SingleOrDefaultAsync(e => e.Id == editDto.Id);
                if (liveAnchorMonthlyTargetAfterLiving == null)
                    throw new Exception("直播中主播月度运营目标情况编号错误！");


                #region #加V量
                liveAnchorMonthlyTargetAfterLiving.CumulativeAddWechat += editDto.CumulativeAddWechat;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeAddWechat <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.AddWechatCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.AddWechatCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeAddWechat) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.AddWechatTarget)) * 100, 2);
                }
                #endregion


                #region #99消耗卡
                liveAnchorMonthlyTargetAfterLiving.CumulativeConsultationCardConsumed += editDto.CumulativeConsultationCardConsumed;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeConsultationCardConsumed <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.ConsultationCardConsumedCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.ConsultationCardConsumedCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeConsultationCardConsumed) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.ConsultationCardConsumedTarget)) * 100, 2);
                }
                #endregion


                #region #199消耗卡
                liveAnchorMonthlyTargetAfterLiving.CumulativeConsultationCardConsumed2 += editDto.CumulativeConsultationCardConsumed2;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeConsultationCardConsumed2 <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.ConsultationCardConsumedCompleteRate2 = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.ConsultationCardConsumedCompleteRate2 = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeConsultationCardConsumed2) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.ConsultationCardConsumedTarget2)) * 100, 2);
                }
                #endregion

                #region #激活历史面诊数量
                liveAnchorMonthlyTargetAfterLiving.CumulativeActivateHistoricalConsultation += editDto.CumulativeActivateHistoricalConsultation;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeActivateHistoricalConsultation <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.ActivateHistoricalConsultationCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.ActivateHistoricalConsultationCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeActivateHistoricalConsultation) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.ActivateHistoricalConsultationTarget)) * 100, 2);
                }
                #endregion

                #region #派单量
                liveAnchorMonthlyTargetAfterLiving.CumulativeSendOrder += editDto.CumulativeSendOrder;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeSendOrder <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.SendOrderCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.SendOrderCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeSendOrder) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.SendOrderTarget)) * 100, 2);
                }
                #endregion

                #region #新客上门数
                liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerVisit += editDto.CumulativeNewCustomerVisit;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerVisit <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.NewCustomerVisitCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.NewCustomerVisitCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerVisit) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.NewCustomerVisitTarget)) * 100, 2);
                }
                #endregion

                #region #老客上门数
                liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerVisit += editDto.CumulativeOldCustomerVisit;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerVisit <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.OldCustomerVisitCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.OldCustomerVisitCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerVisit) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.OldCustomerVisitTarget)) * 100, 2);
                }
                #endregion

                #region #上门数
                liveAnchorMonthlyTargetAfterLiving.CumulativeVisit += editDto.CumulativeVisit;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeVisit <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.VisitCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.VisitCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeVisit) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.VisitTarget)) * 100, 2);
                }
                #endregion

                #region #新诊成交数
                liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerDealTarget += editDto.CumulativeNewCustomerDealTarget;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerDealTarget <= 0 || liveAnchorMonthlyTargetAfterLiving.CumulativeVisit <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.NewCustomerDealRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.NewCustomerDealRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerDealTarget) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeVisit)) * 100, 2);
                }
                #endregion

                #region #老客成交数
                liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerDealTarget += editDto.CumulativeOldCustomerDealTarget;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerDealTarget <= 0 || liveAnchorMonthlyTargetAfterLiving.CumulativeVisit <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.OldCustomerDealRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.OldCustomerDealRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerDealTarget) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeVisit)) * 100, 2);
                }
                #endregion

                #region #成交数
                liveAnchorMonthlyTargetAfterLiving.CumulativeDealTarget += editDto.CumulativeDealTarget;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeDealTarget <= 0 || liveAnchorMonthlyTargetAfterLiving.CumulativeVisit <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.DealRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.DealRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeDealTarget) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeVisit)) * 100, 2);
                }
                #endregion

                #region #小黄车退单
                liveAnchorMonthlyTargetAfterLiving.CumulativeMinivanRefund += editDto.CumulativeMinivanRefund;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeMinivanRefund <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.MinivanRefundCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.MinivanRefundCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeMinivanRefund) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.MinivanRefundTarget)) * 100, 2);
                }
                #endregion

                #region #小黄车差评
                liveAnchorMonthlyTargetAfterLiving.CumulativeMiniVanBadReviews += editDto.CumulativeMiniVanBadReviews;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeMiniVanBadReviews <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.MiniVanBadReviewsCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.MiniVanBadReviewsCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeMiniVanBadReviews) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.MiniVanBadReviewsTarget)) * 100, 2);
                }
                #endregion

                #region #新诊业绩量
                liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerPerformance += editDto.CumulativeNewCustomerPerformance;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerPerformance <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.NewCustomerPerformanceCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.NewCustomerPerformanceCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeNewCustomerPerformance) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.NewCustomerPerformanceTarget)) * 100, 2);
                }
                #endregion

                #region #复诊业绩量
                liveAnchorMonthlyTargetAfterLiving.CumulativeSubsequentPerformance += editDto.CumulativeSubsequentPerformance;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeSubsequentPerformance <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.SubsequentPerformanceCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.SubsequentPerformanceCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeSubsequentPerformance) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.SubsequentPerformanceTarget)) * 100, 2);
                }
                #endregion

                #region #老客业绩量
                liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerPerformance += editDto.CumulativeOldCustomerPerformance;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerPerformance <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.OldCustomerPerformanceCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.OldCustomerPerformanceCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeOldCustomerPerformance) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.OldCustomerPerformanceTarget)) * 100, 2);
                }
                #endregion

                #region #业绩量
                liveAnchorMonthlyTargetAfterLiving.CumulativePerformance += editDto.CumulativePerformance;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativePerformance <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.PerformanceCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.PerformanceCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativePerformance) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.PerformanceTarget)) * 100, 2);
                }
                #endregion
                #region 有效业绩

                liveAnchorMonthlyTargetAfterLiving.CumulativeEffectivePerformance += editDto.EffectivePerformance;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeEffectivePerformance <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.EffectivePerformanceCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.EffectivePerformanceCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeEffectivePerformance) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.EffectivePerformanceTarget)) * 100, 2);
                }

                #endregion

                #region 潜在业绩

                liveAnchorMonthlyTargetAfterLiving.CumulativePotentialPerformance += editDto.PotentialPerformance;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativePotentialPerformance <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.PotentialPerformanceCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.PotentialPerformanceCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativePotentialPerformance) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.PotentialPerformanceTarget)) * 100, 2);
                }

                #endregion
                #region 分诊量
                liveAnchorMonthlyTargetAfterLiving.CumulativeDistributeConsulation += editDto.DistributeConsulation;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeDistributeConsulation <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.DistributeConsulationCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.DistributeConsulationCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeDistributeConsulation) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.DistributeConsulationTarget)) * 100, 2);
                }
                #endregion
                #region 线索量
                liveAnchorMonthlyTargetAfterLiving.CumulativeClues += editDto.CumulativeClues;
                if (liveAnchorMonthlyTargetAfterLiving.CumulativeClues <= 0)
                {
                    liveAnchorMonthlyTargetAfterLiving.CluesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetAfterLiving.CluesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CumulativeClues) / Convert.ToDecimal(liveAnchorMonthlyTargetAfterLiving.CluesTarget)) * 100, 2);
                }
                #endregion


                await dalLiveAnchorMonthlyTargetAfterLiving.UpdateAsync(liveAnchorMonthlyTargetAfterLiving, true);
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
                var liveAnchorMonthlyTargetAfterLiving = await dalLiveAnchorMonthlyTargetAfterLiving.GetAll().SingleOrDefaultAsync(e => e.Id == id);

                if (liveAnchorMonthlyTargetAfterLiving == null)
                    throw new Exception("直播中主播月度运营目标情况编号错误");

                await dalLiveAnchorMonthlyTargetAfterLiving.DeleteAsync(liveAnchorMonthlyTargetAfterLiving, true);
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
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
            LiveAnchorMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorMonthTargetPerformanceDto
            {
                TotalPerformanceTarget = await performance.SumAsync(t => t.PerformanceTarget),
                OldCustomerPerformanceTarget = await performance.SumAsync(t => t.OldCustomerPerformanceTarget),
                NewCustomerPerformanceTarget = await performance.SumAsync(t => t.NewCustomerPerformanceTarget),

            };
            return performanceInfoDto;
        }
        /// <summary>
        /// 根据年份获取主播业绩目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<List<AmiyaOperationBoardCluesChannelTargetDto>> GetPerformanceByYearAsync(int year, List<int> liveAnchorIds, bool? isOldCustomer)
        {
            if (isOldCustomer == true)
            {
                var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new AmiyaOperationBoardCluesChannelTargetDto
                {
                    Month = e.Month,
                    TotalPerformanceTarget = e.OldCustomerPerformanceTarget,
                    LiveAnchorId = e.LiveAnchorId,
                })
                .ToList();
                return performance;
            }
            else if (isOldCustomer == false)
            {
                var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new AmiyaOperationBoardCluesChannelTargetDto
                {
                    Month = e.Month,
                    TotalPerformanceTarget = e.NewCustomerPerformanceTarget,
                    LiveAnchorId = e.LiveAnchorId,

                })
                .ToList();
                return performance;
            }
            else
            {
                var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new AmiyaOperationBoardCluesChannelTargetDto
                {
                    Month = e.Month,
                    TotalPerformanceTarget = e.PerformanceTarget,
                    LiveAnchorId = e.LiveAnchorId,
                    CluesTarget = e.CluesTarget,

                })
                .ToList();
                return performance;
            }
        }
        public async Task<LiveAnchorMonthTargetPerformanceDto> GetPerformanceTargetAsync(int year, int month, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new
                {
                    PerformanceTarget = e.PerformanceTarget,
                    EffectivePerformanceTarget = e.EffectivePerformanceTarget,
                    PotentialPerformanceTarget = e.PotentialPerformanceTarget,
                    OldCustomerPerformanceTarget = e.OldCustomerPerformanceTarget,
                    NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                    RefundCardTarget = e.MinivanRefundTarget,
                    DistributeConsulationTarget = e.DistributeConsulationTarget,
                    AddWechatTarget = e.AddWechatTarget,
                    SendOrderTarget = e.SendOrderTarget,
                    NewCustomerVisitTarget = e.NewCustomerVisitTarget,
                    BaseLiveAnchorId = e.LiveAnchor.LiveAnchorBaseId,
                    CluesTaget = e.CluesTarget,

                })
                .ToList();
            LiveAnchorMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorMonthTargetPerformanceDto
            {
                TotalPerformanceTarget = performance.Sum(t => t.PerformanceTarget),
                OldCustomerPerformanceTarget = performance.Sum(t => t.OldCustomerPerformanceTarget),
                NewCustomerPerformanceTarget = performance.Sum(t => t.NewCustomerPerformanceTarget),
                EffectivePerformance = performance.Sum(t => t.EffectivePerformanceTarget),
                PotentialPerformance = performance.Sum(t => t.PotentialPerformanceTarget),
                MinivanRefundTarget = performance.Sum(e => e.RefundCardTarget),
                AddWechatTarget = performance.Sum(e => e.AddWechatTarget),
                SendOrderTarget = performance.Sum(e => e.SendOrderTarget),
                NewCustomerVisitTarget = performance.Sum(e => e.NewCustomerVisitTarget),
                DistributeConsulationTarget = performance.Sum(t => t.DistributeConsulationTarget),
                CluesTarget = performance.Sum(e => e.CluesTaget),
            };
            return performanceInfoDto;
        }
        /// <summary>
        /// 根据基础主播获取直播后月经营目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        public async Task<List<LiveAnchorMonthTargetPerformanceDto>> GetPerformanceTargetByBaseLiveAnchorIdAsync(int year, int month, List<string> baseLiveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Include(x => x.LiveAnchor).Where(t => t.Year == year && t.Month == month)
                .Where(t => baseLiveAnchorIds.Contains(t.LiveAnchor.LiveAnchorBaseId))
                .Select(e => new
                {
                    PerformanceTarget = e.PerformanceTarget,
                    EffectivePerformanceTarget = e.EffectivePerformanceTarget,
                    PotentialPerformanceTarget = e.PotentialPerformanceTarget,
                    OldCustomerPerformanceTarget = e.OldCustomerPerformanceTarget,
                    NewCustomerPerformanceTarget = e.NewCustomerPerformanceTarget,
                    DistributeConsulationTarget = e.DistributeConsulationTarget,
                    AddWechatTarget = e.AddWechatTarget,
                    BasbaseLiveAnchorId = e.LiveAnchor.LiveAnchorBaseId,
                    SendOrderTarget = e.SendOrderTarget,
                    NewCustomerVisitTarget = e.NewCustomerVisitTarget,
                    OldCustomerVisitTarget = e.OldCustomerVisitTarget,
                    NewCustomerDealTarget = e.NewCustomerDealTarget,
                    OldCustomerDealTarget = e.OldCustomerDealTarget,
                    RefundCardTarget = e.MinivanRefundTarget,
                })
                .ToList();
            var dataList = performance.GroupBy(e => e.BasbaseLiveAnchorId).Select(e => new LiveAnchorMonthTargetPerformanceDto
            {
                TotalPerformanceTarget = e.Sum(t => t.PerformanceTarget),
                OldCustomerPerformanceTarget = e.Sum(t => t.OldCustomerPerformanceTarget),
                NewCustomerPerformanceTarget = e.Sum(t => t.NewCustomerPerformanceTarget),
                EffectivePerformance = e.Sum(t => t.EffectivePerformanceTarget),
                PotentialPerformance = e.Sum(t => t.PotentialPerformanceTarget),
                DistributeConsulationTarget = e.Sum(t => t.DistributeConsulationTarget),
                AddWechatTarget = e.Sum(e => e.AddWechatTarget),
                SendOrderTarget = e.Sum(e => e.SendOrderTarget),
                NewCustomerVisitTarget = e.Sum(e => e.NewCustomerVisitTarget),
                OldCustomerVisitTarget = e.Sum(e => e.OldCustomerVisitTarget),
                NewCustomerDealTarget = e.Sum(e => e.NewCustomerDealTarget),
                OldCustomerDealTarget = e.Sum(e => e.OldCustomerDealTarget),
                MinivanRefundTarget = e.Sum(e => e.RefundCardTarget),
                BaseLiveAbchorId = e.Key
            }).ToList();

            return dataList;
        }

        /// <summary>
        /// 根据主播基础id按年月获取数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <returns></returns>
        public async Task<GroupPerformanceListDto> GetLiveAnchorBaseIdPerformance(int year, int month, string liveAnchorBaseId)
        {
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Include(x => x.LiveAnchor).Where(t => t.Year == year && t.Month == month && t.LiveAnchor.LiveAnchorBaseId == liveAnchorBaseId);
            GroupPerformanceListDto performanceInfoDto = new GroupPerformanceListDto
            {
                GroupPerformance = await performance.SumAsync(t => t.CumulativePerformance),
                GroupTargetPerformance = await performance.SumAsync(t => t.PerformanceTarget),
            };
            return performanceInfoDto;
        }


        /// <summary>
        /// 基础经营看板业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetBasePerformanceTargetAsync(int year, int month, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
            LiveAnchorBaseBusinessMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetPerformanceDto
            {
                AddWeChatTarget = await performance.SumAsync(t => t.AddWechatTarget),
                ConsulationCardConsumedTarget = await performance.SumAsync(t => t.ConsultationCardConsumedTarget + t.ConsultationCardConsumedTarget2),
                HistoryConsulationCardConsumedTarget = await performance.SumAsync(t => t.ActivateHistoricalConsultationTarget),
                ConsulationCardRefundTarget = await performance.SumAsync(t => t.MinivanRefundTarget),

            };
            return performanceInfoDto;
        }

        /// <summary>
        /// 根据时间获取直播后业绩目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<LiveAnchorBaseBusinessMonthTargetPerformanceDto> GetAfterLivingTargetByDateAsync(int year, int month)
        {
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year && t.Month == month);
            LiveAnchorBaseBusinessMonthTargetPerformanceDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetPerformanceDto
            {
                AddWeChatTarget = await performance.SumAsync(t => t.AddWechatTarget),
                ConsulationCardConsumedTarget = await performance.SumAsync(t => t.ConsultationCardConsumedTarget + t.ConsultationCardConsumedTarget2),
                HistoryConsulationCardConsumedTarget = await performance.SumAsync(t => t.ActivateHistoricalConsultationTarget),
                ConsulationCardRefundTarget = await performance.SumAsync(t => t.MinivanRefundTarget),
                SendOrderTarget = await performance.SumAsync(x => x.SendOrderTarget),
                NewCustomerVisitTarget = await performance.SumAsync(x => x.NewCustomerVisitTarget),
                NewCustomerDealTarget = await performance.SumAsync(x => x.NewCustomerDealTarget)
            };
            return performanceInfoDto;
        }


        /// <summary>
        /// 派单成交看板业绩目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        /// <returns></returns>
        public async Task<LiveAnchorBaseBusinessMonthTargetSendOrDealDto> GetSendOrDealTargetAsync(int year, int month, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetAfterLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
            LiveAnchorBaseBusinessMonthTargetSendOrDealDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetSendOrDealDto
            {
                SendOrderTarget = await performance.SumAsync(t => t.SendOrderTarget),
                TotalVisitTarget = await performance.SumAsync(t => t.VisitTarget),
                NewCustomerVisitTarget = await performance.SumAsync(t => t.NewCustomerVisitTarget),
                OldCustomerVisitTarget = await performance.SumAsync(t => t.OldCustomerVisitTarget),
                TotalDealTarget = await performance.SumAsync(t => t.DealTarget),
                NewCustomerDealTarget = await performance.SumAsync(t => t.NewCustomerDealTarget),
                OldCustomerDealTarget = await performance.SumAsync(t => t.OldCustomerDealTarget),

            };
            return performanceInfoDto;
        }
    }
}
