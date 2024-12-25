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
    public class LiveAnchorMonthlyTargetBeforeLivingService : ILiveAnchorMonthlyTargetBeforeLivingService
    {
        private IDalLiveAnchorMonthlyTargetBeforeLiving dalLiveAnchorMonthlyTargetBeforeLiving;
        private ILiveAnchorService _liveanchorService;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private IEmployeeBindLiveAnchorService employeeBindLiveAnchorService;
        private IDalAmiyaEmployee dalAmiyaEmployee;

        public LiveAnchorMonthlyTargetBeforeLivingService(IDalLiveAnchorMonthlyTargetBeforeLiving dalLiveAnchorMonthlyTargetBeforeLiving,
            ILiveAnchorService liveAnchorService,
            IEmployeeBindLiveAnchorService employeeBindLiveAnchorService,
            IAmiyaEmployeeService amiyaEmployeeService, IDalAmiyaEmployee dalAmiyaEmployee)
        {
            this.dalLiveAnchorMonthlyTargetBeforeLiving = dalLiveAnchorMonthlyTargetBeforeLiving;
            _amiyaEmployeeService = amiyaEmployeeService;
            this.employeeBindLiveAnchorService = employeeBindLiveAnchorService;
            _liveanchorService = liveAnchorService;
            this.dalAmiyaEmployee = dalAmiyaEmployee;
        }



        public async Task<FxPageInfo<LiveAnchorMonthlyTargetBeforeLivingDto>> GetListWithPageAsync(int Year, int Month, int? LiveAnchorId, int employeeId, int pageNum, int pageSize)
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
                var liveAnchorMonthlyTargetBeforeLiving = from d in dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Include(e => e.LiveAnchor)
                                                          where (d.Year == Year)
                                                          && (d.Month == Month)
                                                          && (liveAnchorIds.Count == 0 || liveAnchorIds.Contains(d.LiveAnchorId))
                                                          select new LiveAnchorMonthlyTargetBeforeLivingDto
                                                          {
                                                              Id = d.Id,
                                                              Year = d.Year,
                                                              Month = d.Month,
                                                              MonthlyTargetName = d.MonthlyTargetName,
                                                              LiveAnchorId = d.LiveAnchorId,
                                                              LiveAnchorName = d.LiveAnchor.Name,
                                                              ContentPlatFormId = d.LiveAnchor.ContentPlateFormId,

                                                              ZhihuReleaseTarget = d.ZhihuReleaseTarget,
                                                              CumulativeZhihuRelease = d.CumulativeZhihuRelease,
                                                              ZhihuReleaseCompleteRate = d.ZhihuReleaseCompleteRate,
                                                              ZhihuFlowinvestmentTarget = d.ZhihuFlowinvestmentTarget,
                                                              CumulativeZhihuFlowinvestment = d.CumulativeZhihuFlowinvestment,
                                                              ZhihuFlowinvestmentCompleteRate = d.ZhihuFlowinvestmentCompleteRate,

                                                              VideoReleaseTarget = d.VideoReleaseTarget,
                                                              CumulativeVideoRelease = d.CumulativeVideoRelease,
                                                              VideoReleaseCompleteRate = d.VideoReleaseCompleteRate,
                                                              VideoFlowinvestmentTarget = d.VideoFlowinvestmentTarget,
                                                              CumulativeVideoFlowinvestment = d.CumulativeVideoFlowinvestment,
                                                              VideoFlowinvestmentCompleteRate = d.VideoFlowinvestmentCompleteRate,

                                                              TikTokReleaseTarget = d.TikTokReleaseTarget,
                                                              CumulativeTikTokRelease = d.CumulativeTikTokRelease,
                                                              TikTokReleaseCompleteRate = d.TikTokReleaseCompleteRate,
                                                              TikTokFlowinvestmentTarget = d.TikTokFlowinvestmentTarget,
                                                              CumulativeTikTokFlowinvestment = d.CumulativeTikTokFlowinvestment,
                                                              TikTokFlowinvestmentCompleteRate = d.TikTokFlowinvestmentCompleteRate,
                                                              TikTokShowcaseIncomeTarget = d.TikTokShowcaseIncomeTarget,
                                                              CumulativeTikTokShowcaseIncome = d.CumulativeTikTokShowcaseIncome,
                                                              TikTokShowcaseIncomeCompleteRate = d.TikTokShowcaseIncomeCompleteRate,



                                                              XiaoHongShuReleaseTarget = d.XiaoHongShuReleaseTarget,
                                                              CumulativeXiaoHongShuRelease = d.CumulativeXiaoHongShuRelease,
                                                              XiaoHongShuReleaseCompleteRate = d.XiaoHongShuReleaseCompleteRate,
                                                              XiaoHongShuFlowinvestmentTarget = d.XiaoHongShuFlowinvestmentTarget,
                                                              CumulativeXiaoHongShuFlowinvestment = d.CumulativeXiaoHongShuFlowinvestment,
                                                              XiaoHongShuFlowinvestmentCompleteRate = d.XiaoHongShuFlowinvestmentCompleteRate,

                                                              SinaWeiBoReleaseTarget = d.SinaWeiBoReleaseTarget,
                                                              CumulativeSinaWeiBoRelease = d.CumulativeSinaWeiBoRelease,
                                                              SinaWeiBoReleaseCompleteRate = d.SinaWeiBoReleaseCompleteRate,
                                                              SinaWeiBoFlowinvestmentTarget = d.SinaWeiBoFlowinvestmentTarget,
                                                              CumulativeSinaWeiBoFlowinvestment = d.CumulativeSinaWeiBoFlowinvestment,
                                                              SinaWeiBoFlowinvestmentCompleteRate = d.SinaWeiBoFlowinvestmentCompleteRate,

                                                              ReleaseTarget = d.ReleaseTarget,
                                                              CumulativeRelease = d.CumulativeRelease,
                                                              ReleaseCompleteRate = d.ReleaseCompleteRate,
                                                              FlowInvestmentTarget = d.FlowInvestmentTarget,
                                                              CumulativeFlowInvestment = d.CumulativeFlowInvestment,
                                                              FlowInvestmentCompleteRate = d.FlowInvestmentCompleteRate,
                                                              CreateDate = d.CreateDate,
                                                              TikTokCluesTarget = d.TikTokCluesTarget,
                                                              CumulativeTikTokClues = d.CumulativeTikTokClues,
                                                              TikTokCluesCompleteRate = d.TikTokCluesCompleteRate,
                                                              TikTokIncreaseFansTarget = d.TikTokIncreaseFansTarget,
                                                              CumulativeTikTokIncreaseFans = d.CumulativeTikTokIncreaseFans,
                                                              TikTokIncreaseFanseCompleteRate = d.TikTokIncreaseFanseCompleteRate,



                                                              TikTokIncreaseFansFeesTarget = d.TikTokIncreaseFansFeesTarget,
                                                              CumulativeTikTokIncreaseFansFees = d.CumulativeTikTokIncreaseFansFees,
                                                              TikTokIncreaseFansFeesCompleteRate = d.TikTokIncreaseFansFeesCompleteRate,
                                                              XiaoHongShuShowcaseIncomeTarget = d.XiaoHongShuShowcaseIncomeTarget,
                                                              CumulativeXiaoHongShuShowcaseIncome = d.CumulativeXiaoHongShuShowcaseIncome,
                                                              XiaoHongShuShowcaseIncomeCompleteRate = d.XiaoHongShuShowcaseIncomeCompleteRate,
                                                              XiaoHongShuCluesTarget = d.XiaoHongShuCluesTarget,
                                                              CumulativeXiaoHongShuClues = d.CumulativeXiaoHongShuClues,
                                                              XiaoHongShuCluesCompleteRate = d.XiaoHongShuCluesCompleteRate,
                                                              XiaoHongShuIncreaseFansTarget = d.XiaoHongShuIncreaseFansTarget,
                                                              CumulativeXiaoHongShuIncreaseFans = d.CumulativeXiaoHongShuIncreaseFans,
                                                              XiaoHongShuIncreaseFanseCompleteRate = d.XiaoHongShuIncreaseFanseCompleteRate,



                                                              XiaoHongShuIncreaseFansFeesTarget = d.XiaoHongShuIncreaseFansFeesTarget,
                                                              CumulativeXiaoHongShuIncreaseFansFees = d.CumulativeXiaoHongShuIncreaseFansFees,
                                                              XiaoHongShuIncreaseFansFeesCompleteRate = d.XiaoHongShuIncreaseFansFeesCompleteRate,
                                                              VideoShowcaseIncomeTarget = d.VideoShowcaseIncomeTarget,
                                                              CumulativeVideoShowcaseIncome = d.CumulativeVideoShowcaseIncome,
                                                              VideoShowcaseIncomeCompleteRate = d.VideoShowcaseIncomeCompleteRate,
                                                              VideoCluesTarget = d.VideoCluesTarget,
                                                              CumulativeVideoClues = d.CumulativeVideoClues,
                                                              VideoCluesCompleteRate = d.VideoCluesCompleteRate,
                                                              VideoIncreaseFansTarget = d.VideoIncreaseFansTarget,
                                                              CumulativeVideoIncreaseFans = d.CumulativeVideoIncreaseFans,
                                                              VideoIncreaseFanseCompleteRate = d.VideoIncreaseFanseCompleteRate,



                                                              VideoIncreaseFansFeesTarget = d.VideoIncreaseFansFeesTarget,
                                                              CumulativeVideoIncreaseFansFees = d.CumulativeVideoIncreaseFansFees,
                                                              VideoIncreaseFansFeesCompleteRate = d.VideoIncreaseFansFeesCompleteRate,
                                                              TikTokShowCaseFeeTarget = d.TikTokShowCaseFeeTarget,
                                                              CumulativeTikTokShowCaseFee = d.CumulativeTikTokShowCaseFee,
                                                              TikTokShowCaseFeeCompleteRate = d.TikTokShowCaseFeeCompleteRate,
                                                              XiaoHongShuShowCaseFeeTarget = d.XiaoHongShuShowCaseFeeTarget,
                                                              CumulativeXiaoHongShuShowCaseFee = d.CumulativeXiaoHongShuShowCaseFee,
                                                              XiaoHongShuShowCaseFeeCompleteRate = d.XiaoHongShuShowCaseFeeCompleteRate,
                                                              VideoShowCaseFeeTarget = d.VideoShowCaseFeeTarget,
                                                              CumulativeVideoShowCaseFee = d.CumulativeVideoShowCaseFee,
                                                              VideoShowCaseFeeCompleteRate = d.VideoShowCaseFeeCompleteRate,
                                                              OwnerId=d.OwnerId
                                                          };

                FxPageInfo<LiveAnchorMonthlyTargetBeforeLivingDto> liveAnchorMonthlyTargetBeforeLivingPageInfo = new FxPageInfo<LiveAnchorMonthlyTargetBeforeLivingDto>();
                liveAnchorMonthlyTargetBeforeLivingPageInfo.TotalCount = await liveAnchorMonthlyTargetBeforeLiving.CountAsync();
                liveAnchorMonthlyTargetBeforeLivingPageInfo.List = await liveAnchorMonthlyTargetBeforeLiving.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                var s = dalAmiyaEmployee.GetAll().Select(e => new { e.Id, e.Name }).ToList();
                foreach (var item in liveAnchorMonthlyTargetBeforeLivingPageInfo.List) { 
                    item.OwnerName=s.Where(e=>e.Id==item.OwnerId).FirstOrDefault()?.Name??"";
                }
                return liveAnchorMonthlyTargetBeforeLivingPageInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 计算完成率
        /// </summary>
        /// <param name="cumulative"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private decimal CalcalculatCompleteRate(decimal cumulative, decimal target)
        {
            return Math.Round(cumulative / target, 2);
        }

        public async Task AddAsync(AddLiveAnchorMonthlyTargetBeforeLivingDto addDto)
        {
            try
            {
                LiveAnchorMonthlyTargetBeforeLiving liveAnchorMonthlyTarget = new LiveAnchorMonthlyTargetBeforeLiving();
                liveAnchorMonthlyTarget.Id = Guid.NewGuid().ToString();
                liveAnchorMonthlyTarget.Year = addDto.Year;
                liveAnchorMonthlyTarget.Month = addDto.Month;
                liveAnchorMonthlyTarget.MonthlyTargetName = addDto.MonthlyTargetName;
                liveAnchorMonthlyTarget.LiveAnchorId = addDto.LiveAnchorId;

                liveAnchorMonthlyTarget.ZhihuReleaseTarget = addDto.ZhihuReleaseTarget;
                liveAnchorMonthlyTarget.CumulativeZhihuRelease = 0;
                liveAnchorMonthlyTarget.ZhihuReleaseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.ZhihuFlowinvestmentTarget = addDto.ZhihuFlowinvestmentTarget;
                liveAnchorMonthlyTarget.CumulativeZhihuFlowinvestment = 0;
                liveAnchorMonthlyTarget.ZhihuFlowinvestmentCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.VideoReleaseTarget = addDto.VideoReleaseTarget;
                liveAnchorMonthlyTarget.CumulativeVideoRelease = 0;
                liveAnchorMonthlyTarget.VideoReleaseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.VideoFlowinvestmentTarget = addDto.VideoFlowinvestmentTarget;
                liveAnchorMonthlyTarget.CumulativeVideoFlowinvestment = 0;
                liveAnchorMonthlyTarget.VideoFlowinvestmentCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.TikTokReleaseTarget = addDto.TikTokReleaseTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokRelease = 0;
                liveAnchorMonthlyTarget.TikTokReleaseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.TikTokFlowinvestmentTarget = addDto.TikTokFlowinvestmentTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokFlowinvestment = 0;
                liveAnchorMonthlyTarget.TikTokFlowinvestmentCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.TikTokShowcaseIncomeTarget = addDto.TikTokShowcaseIncomeTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokShowcaseIncome = 0.00M;
                liveAnchorMonthlyTarget.TikTokShowcaseIncomeCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.XiaoHongShuReleaseTarget = addDto.XiaoHongShuReleaseTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuRelease = 0;
                liveAnchorMonthlyTarget.XiaoHongShuReleaseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.XiaoHongShuFlowinvestmentTarget = addDto.XiaoHongShuFlowinvestmentTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuFlowinvestment = 0;
                liveAnchorMonthlyTarget.XiaoHongShuFlowinvestmentCompleteRate = 0.00M;



                liveAnchorMonthlyTarget.SinaWeiBoReleaseTarget = addDto.SinaWeiBoReleaseTarget;
                liveAnchorMonthlyTarget.CumulativeSinaWeiBoRelease = 0;
                liveAnchorMonthlyTarget.SinaWeiBoReleaseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.SinaWeiBoFlowinvestmentTarget = addDto.SinaWeiBoFlowinvestmentTarget;
                liveAnchorMonthlyTarget.CumulativeSinaWeiBoFlowinvestment = 0;
                liveAnchorMonthlyTarget.SinaWeiBoFlowinvestmentCompleteRate = 0.00M;


                liveAnchorMonthlyTarget.ReleaseTarget = addDto.ReleaseTarget;
                liveAnchorMonthlyTarget.CumulativeRelease = 0;
                liveAnchorMonthlyTarget.ReleaseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.FlowInvestmentTarget = addDto.FlowInvestmentTarget;
                liveAnchorMonthlyTarget.CumulativeFlowInvestment = 0;
                liveAnchorMonthlyTarget.FlowInvestmentCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.TikTokCluesTarget = addDto.TikTokCluesTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokClues = 0;
                liveAnchorMonthlyTarget.TikTokCluesCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.TikTokIncreaseFansTarget = addDto.TikTokIncreaseFansTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokIncreaseFans = 0;
                liveAnchorMonthlyTarget.TikTokIncreaseFanseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.TikTokIncreaseFansFeesTarget = addDto.TikTokIncreaseFansFeesTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokIncreaseFansFees = 0;
                liveAnchorMonthlyTarget.TikTokIncreaseFansFeesCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.XiaoHongShuShowcaseIncomeTarget = addDto.XiaoHongShuShowcaseIncomeTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuShowcaseIncome = 0;
                liveAnchorMonthlyTarget.XiaoHongShuShowcaseIncomeCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.XiaoHongShuCluesTarget = addDto.XiaoHongShuCluesTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuClues = 0;
                liveAnchorMonthlyTarget.XiaoHongShuCluesCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansTarget = addDto.XiaoHongShuIncreaseFansTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuIncreaseFans = 0;
                liveAnchorMonthlyTarget.XiaoHongShuIncreaseFanseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansFeesTarget = addDto.XiaoHongShuIncreaseFansFeesTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuIncreaseFansFees = 0;
                liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansFeesCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.VideoShowcaseIncomeTarget = addDto.VideoShowcaseIncomeTarget;
                liveAnchorMonthlyTarget.CumulativeVideoShowcaseIncome = 0;
                liveAnchorMonthlyTarget.VideoShowcaseIncomeCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.VideoCluesTarget = addDto.VideoCluesTarget;
                liveAnchorMonthlyTarget.CumulativeVideoClues = 0;
                liveAnchorMonthlyTarget.VideoCluesCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.VideoIncreaseFansTarget = addDto.VideoIncreaseFansTarget;
                liveAnchorMonthlyTarget.CumulativeVideoIncreaseFans = 0;
                liveAnchorMonthlyTarget.VideoIncreaseFanseCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.VideoIncreaseFansFeesTarget = addDto.VideoIncreaseFansFeesTarget;
                liveAnchorMonthlyTarget.CumulativeVideoIncreaseFansFees = 0;
                liveAnchorMonthlyTarget.VideoIncreaseFansFeesCompleteRate = 0.00M;

                liveAnchorMonthlyTarget.TikTokShowCaseFeeTarget = addDto.TikTokShowCaseFeeTarget;
                liveAnchorMonthlyTarget.CumulativeTikTokShowCaseFee = 0;
                liveAnchorMonthlyTarget.TikTokShowCaseFeeCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.XiaoHongShuShowCaseFeeTarget = addDto.XiaoHongShuShowCaseFeeTarget;
                liveAnchorMonthlyTarget.CumulativeXiaoHongShuShowCaseFee = 0;
                liveAnchorMonthlyTarget.XiaoHongShuShowCaseFeeCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.VideoShowCaseFeeTarget = addDto.VideoShowCaseFeeTarget;
                liveAnchorMonthlyTarget.CumulativeVideoShowCaseFee = 0;
                liveAnchorMonthlyTarget.VideoShowCaseFeeCompleteRate = 0.00M;
                liveAnchorMonthlyTarget.OwnerId = addDto.OwnerId;
                liveAnchorMonthlyTarget.CreateDate = DateTime.Now;

                await dalLiveAnchorMonthlyTargetBeforeLiving.AddAsync(liveAnchorMonthlyTarget, true);
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
                var liveAnchorMonthlyTargetBeforeLiving = from d in dalLiveAnchorMonthlyTargetBeforeLiving.GetAll()
                                                          where (d.Year == year && d.Month == month)
                                                          select new LiveAnchorMonthlyTargetKeyAndValueDto
                                                          {
                                                              Id = d.Id,
                                                              Name = d.MonthlyTargetName
                                                          };
                return liveAnchorMonthlyTargetBeforeLiving.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<LiveAnchorMonthlyTargetBeforeLivingDto> GetByIdAsync(string id)
        {
            try
            {
                var liveAnchorMonthlyTarget = await dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (liveAnchorMonthlyTarget == null)
                {
                    throw new Exception("直播前主播月度运营目标情况编号错误！");
                }

                LiveAnchorMonthlyTargetBeforeLivingDto liveAnchorMonthlyTargetDto = new LiveAnchorMonthlyTargetBeforeLivingDto();
                liveAnchorMonthlyTargetDto.Id = liveAnchorMonthlyTarget.Id;
                liveAnchorMonthlyTargetDto.Year = liveAnchorMonthlyTarget.Year;
                liveAnchorMonthlyTargetDto.Month = liveAnchorMonthlyTarget.Month;
                liveAnchorMonthlyTargetDto.MonthlyTargetName = liveAnchorMonthlyTarget.MonthlyTargetName;
                liveAnchorMonthlyTargetDto.LiveAnchorId = liveAnchorMonthlyTarget.LiveAnchorId;

                liveAnchorMonthlyTargetDto.TikTokReleaseTarget = liveAnchorMonthlyTarget.TikTokReleaseTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokRelease = liveAnchorMonthlyTarget.CumulativeTikTokRelease;
                liveAnchorMonthlyTargetDto.TikTokReleaseCompleteRate = liveAnchorMonthlyTarget.TikTokReleaseCompleteRate;
                liveAnchorMonthlyTargetDto.TikTokFlowinvestmentTarget = liveAnchorMonthlyTarget.TikTokFlowinvestmentTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokFlowinvestment = liveAnchorMonthlyTarget.CumulativeTikTokFlowinvestment;
                liveAnchorMonthlyTargetDto.TikTokFlowinvestmentCompleteRate = liveAnchorMonthlyTarget.TikTokFlowinvestmentCompleteRate;
                liveAnchorMonthlyTargetDto.TikTokShowcaseIncomeTarget = liveAnchorMonthlyTarget.TikTokShowcaseIncomeTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokShowcaseIncome = liveAnchorMonthlyTarget.CumulativeTikTokShowcaseIncome;
                liveAnchorMonthlyTargetDto.TikTokShowcaseIncomeCompleteRate = liveAnchorMonthlyTarget.TikTokShowcaseIncomeCompleteRate;
                liveAnchorMonthlyTargetDto.TikTokCluesTarget = liveAnchorMonthlyTarget.TikTokCluesTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokClues = liveAnchorMonthlyTarget.CumulativeTikTokClues;
                liveAnchorMonthlyTargetDto.TikTokCluesCompleteRate = liveAnchorMonthlyTarget.TikTokCluesCompleteRate;
                liveAnchorMonthlyTargetDto.TikTokIncreaseFansTarget = liveAnchorMonthlyTarget.TikTokIncreaseFansTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokIncreaseFans = liveAnchorMonthlyTarget.CumulativeTikTokIncreaseFans;
                liveAnchorMonthlyTargetDto.TikTokIncreaseFanseCompleteRate = liveAnchorMonthlyTarget.TikTokIncreaseFanseCompleteRate;




                liveAnchorMonthlyTargetDto.TikTokIncreaseFansFeesTarget = liveAnchorMonthlyTarget.TikTokIncreaseFansFeesTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokIncreaseFansFees = liveAnchorMonthlyTarget.CumulativeTikTokIncreaseFansFees;
                liveAnchorMonthlyTargetDto.TikTokIncreaseFansFeesCompleteRate = liveAnchorMonthlyTarget.TikTokIncreaseFansFeesCompleteRate;
                liveAnchorMonthlyTargetDto.TikTokShowCaseFeeTarget = liveAnchorMonthlyTarget.TikTokShowCaseFeeTarget;
                liveAnchorMonthlyTargetDto.CumulativeTikTokShowCaseFee = liveAnchorMonthlyTarget.CumulativeTikTokShowCaseFee;
                liveAnchorMonthlyTargetDto.TikTokShowCaseFeeCompleteRate = liveAnchorMonthlyTarget.TikTokShowCaseFeeCompleteRate;



                liveAnchorMonthlyTargetDto.XiaoHongShuReleaseTarget = liveAnchorMonthlyTarget.XiaoHongShuReleaseTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuRelease = liveAnchorMonthlyTarget.CumulativeXiaoHongShuRelease;
                liveAnchorMonthlyTargetDto.XiaoHongShuReleaseCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuReleaseCompleteRate;
                liveAnchorMonthlyTargetDto.XiaoHongShuFlowinvestmentTarget = liveAnchorMonthlyTarget.XiaoHongShuFlowinvestmentTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuFlowinvestment = liveAnchorMonthlyTarget.CumulativeXiaoHongShuFlowinvestment;
                liveAnchorMonthlyTargetDto.XiaoHongShuFlowinvestmentCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuFlowinvestmentCompleteRate;
                liveAnchorMonthlyTargetDto.XiaoHongShuShowcaseIncomeTarget = liveAnchorMonthlyTarget.XiaoHongShuShowcaseIncomeTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuShowcaseIncome = liveAnchorMonthlyTarget.CumulativeXiaoHongShuShowcaseIncome;
                liveAnchorMonthlyTargetDto.XiaoHongShuShowcaseIncomeCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuShowcaseIncomeCompleteRate;
                liveAnchorMonthlyTargetDto.XiaoHongShuCluesTarget = liveAnchorMonthlyTarget.XiaoHongShuCluesTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuClues = liveAnchorMonthlyTarget.CumulativeXiaoHongShuClues;
                liveAnchorMonthlyTargetDto.XiaoHongShuCluesCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuCluesCompleteRate;
                liveAnchorMonthlyTargetDto.XiaoHongShuIncreaseFansTarget = liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuIncreaseFans = liveAnchorMonthlyTarget.CumulativeXiaoHongShuIncreaseFans;
                liveAnchorMonthlyTargetDto.XiaoHongShuIncreaseFanseCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuIncreaseFanseCompleteRate;


                liveAnchorMonthlyTargetDto.XiaoHongShuIncreaseFansFeesTarget = liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansFeesTarget;
                liveAnchorMonthlyTargetDto.XiaoHongShuIncreaseFansFeesTarget = liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansFeesTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuIncreaseFansFees = liveAnchorMonthlyTarget.CumulativeXiaoHongShuIncreaseFansFees;
                liveAnchorMonthlyTargetDto.XiaoHongShuIncreaseFansFeesCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansFeesCompleteRate;
                liveAnchorMonthlyTargetDto.XiaoHongShuShowCaseFeeTarget = liveAnchorMonthlyTarget.XiaoHongShuShowCaseFeeTarget;
                liveAnchorMonthlyTargetDto.CumulativeXiaoHongShuShowCaseFee = liveAnchorMonthlyTarget.CumulativeXiaoHongShuShowCaseFee;
                liveAnchorMonthlyTargetDto.XiaoHongShuShowCaseFeeCompleteRate = liveAnchorMonthlyTarget.XiaoHongShuShowCaseFeeCompleteRate;

                liveAnchorMonthlyTargetDto.SinaWeiBoReleaseTarget = liveAnchorMonthlyTarget.SinaWeiBoReleaseTarget;
                liveAnchorMonthlyTargetDto.CumulativeSinaWeiBoRelease = liveAnchorMonthlyTarget.CumulativeSinaWeiBoRelease;
                liveAnchorMonthlyTargetDto.SinaWeiBoReleaseCompleteRate = liveAnchorMonthlyTarget.SinaWeiBoReleaseCompleteRate;
                liveAnchorMonthlyTargetDto.SinaWeiBoFlowinvestmentTarget = liveAnchorMonthlyTarget.SinaWeiBoFlowinvestmentTarget;
                liveAnchorMonthlyTargetDto.CumulativeSinaWeiBoFlowinvestment = liveAnchorMonthlyTarget.CumulativeSinaWeiBoFlowinvestment;
                liveAnchorMonthlyTargetDto.SinaWeiBoFlowinvestmentCompleteRate = liveAnchorMonthlyTarget.SinaWeiBoFlowinvestmentCompleteRate;
                liveAnchorMonthlyTargetDto.ZhihuReleaseTarget = liveAnchorMonthlyTarget.ZhihuReleaseTarget;
                liveAnchorMonthlyTargetDto.CumulativeZhihuRelease = liveAnchorMonthlyTarget.CumulativeZhihuRelease;
                liveAnchorMonthlyTargetDto.ZhihuReleaseCompleteRate = liveAnchorMonthlyTarget.ZhihuReleaseCompleteRate;
                liveAnchorMonthlyTargetDto.ZhihuFlowinvestmentTarget = liveAnchorMonthlyTarget.ZhihuFlowinvestmentTarget;
                liveAnchorMonthlyTargetDto.CumulativeZhihuFlowinvestment = liveAnchorMonthlyTarget.CumulativeZhihuFlowinvestment;
                liveAnchorMonthlyTargetDto.ZhihuFlowinvestmentCompleteRate = liveAnchorMonthlyTarget.ZhihuFlowinvestmentCompleteRate;
                liveAnchorMonthlyTargetDto.VideoReleaseTarget = liveAnchorMonthlyTarget.VideoReleaseTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoRelease = liveAnchorMonthlyTarget.CumulativeVideoRelease;
                liveAnchorMonthlyTargetDto.VideoReleaseCompleteRate = liveAnchorMonthlyTarget.VideoReleaseCompleteRate;
                liveAnchorMonthlyTargetDto.VideoFlowinvestmentTarget = liveAnchorMonthlyTarget.VideoFlowinvestmentTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoFlowinvestment = liveAnchorMonthlyTarget.CumulativeVideoFlowinvestment;
                liveAnchorMonthlyTargetDto.VideoFlowinvestmentCompleteRate = liveAnchorMonthlyTarget.VideoFlowinvestmentCompleteRate;
                liveAnchorMonthlyTargetDto.VideoShowcaseIncomeTarget = liveAnchorMonthlyTarget.VideoShowcaseIncomeTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoShowcaseIncome = liveAnchorMonthlyTarget.CumulativeVideoShowcaseIncome;
                liveAnchorMonthlyTargetDto.VideoShowcaseIncomeCompleteRate = liveAnchorMonthlyTarget.VideoShowcaseIncomeCompleteRate;
                liveAnchorMonthlyTargetDto.VideoCluesTarget = liveAnchorMonthlyTarget.VideoCluesTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoClues = liveAnchorMonthlyTarget.CumulativeVideoClues;
                liveAnchorMonthlyTargetDto.VideoCluesCompleteRate = liveAnchorMonthlyTarget.VideoCluesCompleteRate;
                liveAnchorMonthlyTargetDto.VideoIncreaseFansTarget = liveAnchorMonthlyTarget.VideoIncreaseFansTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoIncreaseFans = liveAnchorMonthlyTarget.CumulativeVideoIncreaseFans;
                liveAnchorMonthlyTargetDto.VideoIncreaseFanseCompleteRate = liveAnchorMonthlyTarget.VideoIncreaseFanseCompleteRate;

                liveAnchorMonthlyTargetDto.VideoIncreaseFansFeesTarget = liveAnchorMonthlyTarget.VideoIncreaseFansFeesTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoIncreaseFansFees = liveAnchorMonthlyTarget.CumulativeVideoIncreaseFansFees;
                liveAnchorMonthlyTargetDto.VideoIncreaseFansFeesCompleteRate = liveAnchorMonthlyTarget.VideoIncreaseFansFeesCompleteRate;
                liveAnchorMonthlyTargetDto.VideoShowCaseFeeTarget = liveAnchorMonthlyTarget.VideoShowCaseFeeTarget;
                liveAnchorMonthlyTargetDto.CumulativeVideoShowCaseFee = liveAnchorMonthlyTarget.CumulativeVideoShowCaseFee;
                liveAnchorMonthlyTargetDto.VideoShowCaseFeeCompleteRate = liveAnchorMonthlyTarget.VideoShowCaseFeeCompleteRate;
                liveAnchorMonthlyTargetDto.ReleaseTarget = liveAnchorMonthlyTarget.ReleaseTarget;
                liveAnchorMonthlyTargetDto.CumulativeRelease = liveAnchorMonthlyTarget.CumulativeRelease;
                liveAnchorMonthlyTargetDto.ReleaseCompleteRate = liveAnchorMonthlyTarget.ReleaseCompleteRate;
                liveAnchorMonthlyTargetDto.FlowInvestmentTarget = liveAnchorMonthlyTarget.FlowInvestmentTarget;
                liveAnchorMonthlyTargetDto.CumulativeFlowInvestment = liveAnchorMonthlyTarget.CumulativeFlowInvestment;
                liveAnchorMonthlyTargetDto.FlowInvestmentCompleteRate = liveAnchorMonthlyTarget.FlowInvestmentCompleteRate;
                liveAnchorMonthlyTargetDto.CreateDate = liveAnchorMonthlyTarget.CreateDate;
                liveAnchorMonthlyTargetDto.Id=liveAnchorMonthlyTarget.Id;
                liveAnchorMonthlyTargetDto.OwnerId= liveAnchorMonthlyTarget.OwnerId;
                if (liveAnchorMonthlyTarget.OwnerId != null) {
                    liveAnchorMonthlyTargetDto.OwnerName=dalAmiyaEmployee.GetAll().Where(e=>e.Id== liveAnchorMonthlyTarget.OwnerId).FirstOrDefault()?.Name??"";
                }
                var liveAnchor = await _liveanchorService.GetByIdAsync(liveAnchorMonthlyTargetDto.LiveAnchorId);
                liveAnchorMonthlyTargetDto.ContentPlatFormId = liveAnchor.ContentPlateFormId;
                return liveAnchorMonthlyTargetDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task UpdateAsync(UpdateLiveAnchorMonthlyTargetBeforeLivingDto updateDto)
        {
            try
            {
                var liveAnchorMonthlyTarget = await dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (liveAnchorMonthlyTarget == null)
                    throw new Exception("直播前主播月度运营目标情况编号错误！");

                liveAnchorMonthlyTarget.Year = updateDto.Year;
                liveAnchorMonthlyTarget.Month = updateDto.Month;
                liveAnchorMonthlyTarget.MonthlyTargetName = updateDto.MonthlyTargetName;
                liveAnchorMonthlyTarget.LiveAnchorId = updateDto.LiveAnchorId;

                liveAnchorMonthlyTarget.TikTokReleaseTarget = updateDto.TikTokReleaseTarget;
                liveAnchorMonthlyTarget.ZhihuReleaseTarget = updateDto.ZhihuReleaseTarget;
                liveAnchorMonthlyTarget.XiaoHongShuReleaseTarget = updateDto.XiaoHongShuReleaseTarget;
                liveAnchorMonthlyTarget.SinaWeiBoReleaseTarget = updateDto.SinaWeiBoReleaseTarget;
                liveAnchorMonthlyTarget.VideoReleaseTarget = updateDto.VideoReleaseTarget;
                liveAnchorMonthlyTarget.TikTokShowcaseIncomeTarget = updateDto.TikTokShowcaseIncomeTarget;

                liveAnchorMonthlyTarget.TikTokFlowinvestmentTarget = updateDto.TikTokFlowinvestmentTarget;
                liveAnchorMonthlyTarget.ZhihuFlowinvestmentTarget = updateDto.ZhihuFlowinvestmentTarget;
                liveAnchorMonthlyTarget.XiaoHongShuFlowinvestmentTarget = updateDto.XiaoHongShuFlowinvestmentTarget;
                liveAnchorMonthlyTarget.SinaWeiBoFlowinvestmentTarget = updateDto.SinaWeiBoFlowinvestmentTarget;
                liveAnchorMonthlyTarget.VideoFlowinvestmentTarget = updateDto.VideoFlowinvestmentTarget;

                liveAnchorMonthlyTarget.ReleaseTarget = updateDto.ReleaseTarget;
                liveAnchorMonthlyTarget.FlowInvestmentTarget = updateDto.FlowInvestmentTarget;
                liveAnchorMonthlyTarget.XiaoHongShuShowcaseIncomeTarget = updateDto.XiaoHongShuShowcaseIncomeTarget;
                liveAnchorMonthlyTarget.XiaoHongShuCluesTarget = updateDto.XiaoHongShuCluesTarget;
                liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansTarget = updateDto.XiaoHongShuIncreaseFansTarget;
                liveAnchorMonthlyTarget.XiaoHongShuIncreaseFansFeesTarget = updateDto.XiaoHongShuIncreaseFansFeesTarget;

                liveAnchorMonthlyTarget.VideoShowcaseIncomeTarget = updateDto.VideoShowcaseIncomeTarget;
                liveAnchorMonthlyTarget.VideoCluesTarget = updateDto.VideoCluesTarget;
                liveAnchorMonthlyTarget.VideoIncreaseFansTarget = updateDto.VideoIncreaseFansTarget;
                liveAnchorMonthlyTarget.VideoIncreaseFansFeesTarget = updateDto.VideoIncreaseFansFeesTarget;

                liveAnchorMonthlyTarget.TikTokCluesTarget = updateDto.TikTokCluesTarget;
                liveAnchorMonthlyTarget.TikTokIncreaseFansTarget = updateDto.TikTokIncreaseFansTarget;
                liveAnchorMonthlyTarget.TikTokIncreaseFansFeesTarget = updateDto.TikTokIncreaseFansFeesTarget;

                liveAnchorMonthlyTarget.TikTokShowCaseFeeTarget = updateDto.TikTokShowcaseFeeTarget;
                liveAnchorMonthlyTarget.XiaoHongShuShowCaseFeeTarget = updateDto.XiaoHongShuShowcaseFeeTarget;
                liveAnchorMonthlyTarget.VideoShowCaseFeeTarget = updateDto.VideoShowcaseFeeTarget;
                liveAnchorMonthlyTarget.OwnerId = updateDto.OwnerId;
                await dalLiveAnchorMonthlyTargetBeforeLiving.UpdateAsync(liveAnchorMonthlyTarget, true);
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
        public async Task EditAsync(UpdateLiveAnchorMonthlyBeforeLivingTargetRateAndNumDto editDto)
        {
            try
            {
                var liveAnchorMonthlyTargetBeforeLiving = await dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().SingleOrDefaultAsync(e => e.Id == editDto.Id);
                if (liveAnchorMonthlyTargetBeforeLiving == null)
                    throw new Exception("直播前主播月度运营目标情况编号错误！");

                #region #知乎发布
                liveAnchorMonthlyTargetBeforeLiving.CumulativeZhihuRelease += editDto.CumulativeZhihuRelease;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeZhihuRelease <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.ZhihuReleaseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.ZhihuReleaseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeZhihuRelease) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.ZhihuReleaseTarget)) * 100, 2);
                }
                #endregion
                #region #知乎投流
                liveAnchorMonthlyTargetBeforeLiving.CumulativeZhihuFlowinvestment += editDto.CumulativeZhihuFlowinvestment;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeZhihuFlowinvestment <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.ZhihuFlowinvestmentCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.ZhihuFlowinvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeZhihuFlowinvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.ZhihuFlowinvestmentTarget)) * 100, 2);
                }
                #endregion

                #region #视频号发布
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoRelease += editDto.CumulativeVideoRelease;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoRelease <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoReleaseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoReleaseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoRelease) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoReleaseTarget)) * 100, 2);
                }
                #endregion
                #region #视频号投流
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoFlowinvestment += editDto.CumulativeVideoFlowinvestment;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoFlowinvestment <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoFlowinvestmentCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoFlowinvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoFlowinvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoFlowinvestmentTarget)) * 100, 2);
                }
                #endregion
                #region 视频号橱窗收入
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoShowcaseIncome += editDto.CumulativeVideoShowcaseIncome;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoShowcaseIncome <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoShowcaseIncomeCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoShowcaseIncomeCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoShowcaseIncome) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoShowcaseIncomeTarget)) * 100, 2);
                }
                #endregion
                #region 视频号涨粉
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoIncreaseFans += editDto.CumulativeVideoIncreaseFans;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoIncreaseFans <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoIncreaseFanseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoIncreaseFanseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoIncreaseFans) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoIncreaseFansTarget)) * 100, 2);
                }
                #endregion
                #region 视频号涨粉费用
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoIncreaseFansFees += editDto.CumulativeVideoIncreaseFansFees;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoIncreaseFansFees <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoIncreaseFansFeesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoIncreaseFansFeesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoIncreaseFansFees) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoIncreaseFansFeesTarget)) * 100, 2);
                }
                #endregion
                #region 视频号线索
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoClues += editDto.CumulativeVideoClues;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoClues <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoCluesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoCluesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoClues) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoCluesTarget)) * 100, 2);
                }
                #endregion

                #region 视频号橱窗付费
                liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoShowCaseFee += editDto.CumulativeVideoShowCaseFee;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoShowCaseFee <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoShowCaseFeeCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.VideoShowCaseFeeCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeVideoShowCaseFee) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.VideoShowCaseFeeTarget)) * 100, 2);
                }
                #endregion
                #region #抖音发布
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokRelease += editDto.CumulativeTikTokRelease;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokRelease <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokReleaseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokReleaseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokRelease) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokReleaseTarget)) * 100, 2);
                }
                #endregion

                #region #抖音投流
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokFlowinvestment += editDto.CumulativeTikTokFlowinvestment;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokFlowinvestment <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokFlowinvestmentCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokFlowinvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokFlowinvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokFlowinvestmentTarget)) * 100, 2);
                }
                #endregion

                #region 抖音橱窗收入
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowcaseIncome += editDto.CumulativeTikTokShowcaseIncome;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowcaseIncome <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokShowcaseIncomeCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokShowcaseIncomeCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowcaseIncome) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokShowcaseIncomeTarget)) * 100, 2);
                }
                #endregion
                #region 抖音涨粉
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokIncreaseFans += editDto.CumulativeTikTokIncreaseFans;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokIncreaseFans <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokIncreaseFanseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokIncreaseFanseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokIncreaseFans) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokIncreaseFansTarget)) * 100, 2);
                }
                #endregion
                #region 抖音涨粉费用
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokIncreaseFansFees += editDto.CumulativeTikTokIncreaseFansFees;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokIncreaseFansFees <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokIncreaseFansFeesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokIncreaseFansFeesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokIncreaseFansFees) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokIncreaseFansFeesTarget)) * 100, 2);
                }
                #endregion
                #region 抖音线索
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokClues += editDto.CumulativeTikTokClues;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokClues <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokCluesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokCluesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokClues) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokCluesTarget)) * 100, 2);
                }
                #endregion

                #region 抖音橱窗付费
                liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowCaseFee += editDto.CumulativeTikTokShowCaseFee;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowCaseFee <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowCaseFee = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.TikTokShowCaseFeeCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeTikTokShowCaseFee) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.TikTokShowCaseFeeTarget)) * 100, 2);
                }
                #endregion

                #region #小红书发布
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuRelease += editDto.CumulativeXiaoHongShuRelease;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuRelease <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuReleaseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuReleaseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuRelease) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuReleaseTarget)) * 100, 2);
                }
                #endregion
                #region #小红书投流
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuFlowinvestment += editDto.CumulativeXiaoHongShuFlowinvestment;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuFlowinvestment <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuFlowinvestmentCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuFlowinvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuFlowinvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuFlowinvestmentTarget)) * 100, 2);
                }
                #endregion
                #region 小红书橱窗收入
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuShowcaseIncome += editDto.CumulativeXiaoHongShuShowcaseIncome;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuShowcaseIncome <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuShowcaseIncomeCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuShowcaseIncomeCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuShowcaseIncome) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuShowcaseIncomeTarget)) * 100, 2);
                }
                #endregion

                #region 小红书涨粉
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuIncreaseFans += editDto.CumulativeXiaoHongShuIncreaseFans;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuIncreaseFans <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuIncreaseFanseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuIncreaseFanseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuIncreaseFans) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuIncreaseFansTarget)) * 100, 2);
                }
                #endregion
                #region 小红书涨粉费用
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuIncreaseFansFees += editDto.CumulativeXiaoHongShuIncreaseFansFees;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuIncreaseFansFees <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuIncreaseFansFeesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuIncreaseFansFeesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuIncreaseFansFees) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuIncreaseFansFeesTarget)) * 100, 2);
                }
                #endregion
                #region 小红书线索
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuClues += editDto.CumulativeXiaoHongShuClues;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuClues <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuCluesCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuCluesCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuClues) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuCluesTarget)) * 100, 2);
                }
                #endregion



                #region 小红书橱窗付费
                liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuShowCaseFee += editDto.CumulativeXiaoHongShuShowCaseFee;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuShowCaseFee <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuShowCaseFeeCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuShowCaseFeeCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeXiaoHongShuShowCaseFee) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.XiaoHongShuShowCaseFeeTarget)) * 100, 2);
                }

                #endregion

                #region #微博发布
                liveAnchorMonthlyTargetBeforeLiving.CumulativeSinaWeiBoRelease += editDto.CumulativeSinaWeiBoRelease;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeSinaWeiBoRelease <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.SinaWeiBoReleaseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.SinaWeiBoReleaseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeSinaWeiBoRelease) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.SinaWeiBoReleaseTarget)) * 100, 2);
                }
                #endregion
                #region #微博投流
                liveAnchorMonthlyTargetBeforeLiving.CumulativeSinaWeiBoFlowinvestment += editDto.CumulativeSinaWeiBoFlowinvestment;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeSinaWeiBoFlowinvestment <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.SinaWeiBoFlowinvestmentCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.SinaWeiBoFlowinvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeSinaWeiBoFlowinvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.SinaWeiBoFlowinvestmentTarget)) * 100, 2);
                }
                #endregion

                #region #发布
                liveAnchorMonthlyTargetBeforeLiving.CumulativeRelease += editDto.CumulativeRelease;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeRelease <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.ReleaseCompleteRate = 0.00M;
                }
                else
                {
                    liveAnchorMonthlyTargetBeforeLiving.ReleaseCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeRelease) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.ReleaseTarget)) * 100, 2);
                }
                #endregion

                #region #运营渠道投流
                liveAnchorMonthlyTargetBeforeLiving.CumulativeFlowInvestment += editDto.CumulativeFlowInvestment;
                if (liveAnchorMonthlyTargetBeforeLiving.CumulativeFlowInvestment <= 0)
                {
                    liveAnchorMonthlyTargetBeforeLiving.FlowInvestmentCompleteRate = 0.00M;
                }
                else
                {

                    liveAnchorMonthlyTargetBeforeLiving.FlowInvestmentCompleteRate = Math.Round((Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.CumulativeFlowInvestment) / Convert.ToDecimal(liveAnchorMonthlyTargetBeforeLiving.FlowInvestmentTarget)) * 100, 2);
                }
                #endregion


                await dalLiveAnchorMonthlyTargetBeforeLiving.UpdateAsync(liveAnchorMonthlyTargetBeforeLiving, true);
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
                var liveAnchorMonthlyTargetBeforeLiving = await dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().SingleOrDefaultAsync(e => e.Id == id);

                if (liveAnchorMonthlyTargetBeforeLiving == null)
                    throw new Exception("直播前主播月度运营目标情况编号错误");

                await dalLiveAnchorMonthlyTargetBeforeLiving.DeleteAsync(liveAnchorMonthlyTargetBeforeLiving, true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// 获取直播前目标数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<LiveAnchorBeforeLivingTargetDto?> GetBeforeLivingTargetByYearAndMonthAsync(QueryBeforeLivingBusinessDataDto query)
        {
            var res = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Where(e => e.Year == query.Year && e.Month == query.Month);
            if (!string.IsNullOrEmpty(query.BaseLiveAnchorId))
            {
                res = res.Where(e => e.LiveAnchor.LiveAnchorBaseId == query.BaseLiveAnchorId);
            }
            if (res.ToList().Count == 0) return null;
            LiveAnchorBeforeLivingTargetDto liveAnchorBeforeLivingTargetDto = new LiveAnchorBeforeLivingTargetDto();
            if (query.ShowTikokData)
            {
                liveAnchorBeforeLivingTargetDto.IncreaseFansFeesTarget += res.Sum(e => e.TikTokIncreaseFansFeesTarget);
                liveAnchorBeforeLivingTargetDto.IncreaseFansTarget += res.Sum(e => e.TikTokIncreaseFansTarget);
                liveAnchorBeforeLivingTargetDto.ShowcaseIncomeTarget += res.Sum(e => e.TikTokShowcaseIncomeTarget);
                liveAnchorBeforeLivingTargetDto.ShowcaseFeeTarget += res.Sum(e => e.TikTokShowCaseFeeTarget);
                liveAnchorBeforeLivingTargetDto.CluesTarget += res.Sum(e => e.TikTokCluesTarget);
                liveAnchorBeforeLivingTargetDto.SendNumTarget += res.Sum(e => e.TikTokReleaseTarget);
            }
            if (query.ShowWechatVideoData)
            {
                liveAnchorBeforeLivingTargetDto.IncreaseFansFeesTarget += res.Sum(e => e.VideoIncreaseFansFeesTarget);
                liveAnchorBeforeLivingTargetDto.IncreaseFansTarget += res.Sum(e => e.VideoIncreaseFansTarget);
                liveAnchorBeforeLivingTargetDto.ShowcaseIncomeTarget += res.Sum(e => e.VideoShowcaseIncomeTarget);
                liveAnchorBeforeLivingTargetDto.ShowcaseFeeTarget += res.Sum(e => e.VideoShowCaseFeeTarget);
                liveAnchorBeforeLivingTargetDto.CluesTarget += res.Sum(e => e.VideoCluesTarget);
                liveAnchorBeforeLivingTargetDto.SendNumTarget += res.Sum(e => e.VideoReleaseTarget);
            }
            if (query.ShowXiaoHongShuData)
            {
                liveAnchorBeforeLivingTargetDto.IncreaseFansFeesTarget += res.Sum(e => e.XiaoHongShuIncreaseFansFeesTarget);
                liveAnchorBeforeLivingTargetDto.IncreaseFansTarget += res.Sum(e => e.XiaoHongShuIncreaseFansTarget);
                liveAnchorBeforeLivingTargetDto.ShowcaseIncomeTarget += res.Sum(e => e.XiaoHongShuShowcaseIncomeTarget);
                liveAnchorBeforeLivingTargetDto.ShowcaseFeeTarget += res.Sum(e => e.XiaoHongShuShowCaseFeeTarget);
                liveAnchorBeforeLivingTargetDto.CluesTarget += res.Sum(e => e.XiaoHongShuCluesTarget);
                liveAnchorBeforeLivingTargetDto.SendNumTarget += res.Sum(e => e.XiaoHongShuReleaseTarget);
            }
            return liveAnchorBeforeLivingTargetDto;
        }

        /// <summary>
        /// 根据年份获取主播IP直播前线索目标总计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<List<AmiyaOperationBoardCluesChannelTargetDto>> GetCluePerformanceTargetByYearAsync(int year, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Where(t => t.Year == year)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new AmiyaOperationBoardCluesChannelTargetDto
                {
                    CluesTarget = e.TikTokCluesTarget + e.XiaoHongShuCluesTarget + e.VideoCluesTarget,
                    Month=e.Month,
                    LiveAnchorId=e.LiveAnchorId
                })
                .ToList();
            return performance;
        }


        /// <summary>
        /// 根据条件获取主播IP直播前线索目标总计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        public async Task<LiveAnchorBaseBusinessMonthTargetBeforeLivingDto> GetCluePerformanceTargetAsync(int year, int month, List<int> liveAnchorIds)
        {
            var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Where(t => t.Year == year && t.Month == month)
                .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
                .Select(e => new LiveAnchorBaseBusinessMonthTargetBeforeLivingDto
                {
                    CluesTarget = e.TikTokCluesTarget + e.XiaoHongShuCluesTarget + e.VideoCluesTarget,
                })
                .ToList();
            LiveAnchorBaseBusinessMonthTargetBeforeLivingDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetBeforeLivingDto
            {
                CluesTarget = performance.Sum(t => t.CluesTarget),
            };
            return performanceInfoDto;
        }

        ///// <summary>
        ///// 获取带货业绩
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="liveAnchorIds"></param>
        ///// <returns></returns>
        //public async Task<List<PerformanceInfoByDateDto>> GetLiveAnchorCommercePerformance(int year, int month, List<int> liveAnchorIds)
        //{
        //    var list = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll()
        //        .Where(o => o.Year == year && o.Month >= 1 && o.Month <= month)
        //        .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId))
        //        .GroupBy(o => o.Month).OrderBy(o => o.Key).Select(o => new PerformanceInfoByDateDto
        //        {
        //            Date = o.Key.ToString(),
        //            PerfomancePrice = o.Sum(o => o.CumulativeCargoSettlementCommission)
        //        }).ToList();
        //    return list;
        //}

        ///// <summary>
        ///// 获取业绩目标
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        ///// <returns></returns>
        //public async Task<LiveAnchorMonthTargetBeforeLivingPerformanceDto> GetPerformance(int year, int month, List<int> liveAnchorIds)
        //{
        //    var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Where(t => t.Year == year && t.Month == month)
        //        .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
        //    LiveAnchorMonthTargetBeforeLivingPerformanceDto performanceInfoDto = new LiveAnchorMonthTargetBeforeLivingPerformanceDto
        //    {
        //        TotalPerformanceTargetBeforeLiving = await performance.SumAsync(t => t.PerformanceTargetBeforeLiving),
        //        CommercePerformanceTargetBeforeLiving = await performance.SumAsync(t => t.CargoSettlementCommissionTargetBeforeLiving),
        //        OldCustomerPerformanceTargetBeforeLiving = await performance.SumAsync(t => t.OldCustomerPerformanceTargetBeforeLiving),
        //        NewCustomerPerformanceTargetBeforeLiving = await performance.SumAsync(t => t.NewCustomerPerformanceTargetBeforeLiving),
        //        CommerceCompletePerformance = await performance.SumAsync(t => t.CumulativeCargoSettlementCommission),

        //    };
        //    return performanceInfoDto;
        //}


        ///// <summary>
        ///// 根据平台id按年月获取数据
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="contentPlatFormId">内容平台id</param>
        ///// <returns></returns>
        //public async Task<GroupPerformanceListDto> GetCooperationLiveAnchorPerformance(int year, int month, string contentPlatFormId)
        //{
        //    var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Include(x => x.LiveAnchor).Where(t => t.Year == year && t.Month == month && t.LiveAnchor.ContentPlateFormId == contentPlatFormId);
        //    GroupPerformanceListDto performanceInfoDto = new GroupPerformanceListDto
        //    {
        //        GroupPerformance = await performance.SumAsync(t => t.CumulativePerformance),
        //        GroupTargetBeforeLivingPerformance = await performance.SumAsync(t => t.PerformanceTargetBeforeLiving),
        //    };
        //    return performanceInfoDto;
        //}

        ///// <summary>
        ///// 根据主播基础id按年月获取数据
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="contentPlatFormId">内容平台id</param>
        ///// <returns></returns>
        //public async Task<GroupPerformanceListDto> GetLiveAnchorBaseIdPerformance(int year, int month, string liveAnchorBaseId)
        //{
        //    var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Include(x => x.LiveAnchor).Where(t => t.Year == year && t.Month == month && t.LiveAnchor.LiveAnchorBaseId == liveAnchorBaseId);
        //    GroupPerformanceListDto performanceInfoDto = new GroupPerformanceListDto
        //    {
        //        GroupPerformance = await performance.SumAsync(t => t.CumulativePerformance),
        //        GroupTargetBeforeLivingPerformance = await performance.SumAsync(t => t.PerformanceTargetBeforeLiving),
        //    };
        //    return performanceInfoDto;
        //}



        ///// <summary>
        ///// 根据主播基础id按年月获取折线图
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="contentPlatFormId">内容平台id</param>
        ///// <returns></returns>
        //public async Task<List<PerformanceBrokenLine>> GetLiveAnchorPerformanceByBaseIdBrokenLineAsync(int year, string liveAnchorBaseId)
        //{
        //    var orderinfo = await dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Include(x => x.LiveAnchor).Where(o => o.Year == year && o.LiveAnchor.LiveAnchorBaseId == liveAnchorBaseId).ToListAsync();

        //    return orderinfo.GroupBy(x => x.Month).Select(x => new PerformanceBrokenLine { Date = x.Key.ToString(), PerfomancePrice = x.Sum(z => z.CumulativePerformance) }).ToList();
        //}


        ///// <summary>
        ///// 基础经营看板业绩
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        ///// <returns></returns>
        //public async Task<LiveAnchorBaseBusinessMonthTargetBeforeLivingPerformanceDto> GetBasePerformanceTargetBeforeLivingAsync(int year, int month, List<int> liveAnchorIds)
        //{
        //    var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Where(t => t.Year == year && t.Month == month)
        //        .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
        //    LiveAnchorBaseBusinessMonthTargetBeforeLivingPerformanceDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetBeforeLivingPerformanceDto
        //    {
        //        AddWeChatTargetBeforeLiving = await performance.SumAsync(t => t.AddWechatTargetBeforeLiving),
        //        ConsulationCardTargetBeforeLiving = await performance.SumAsync(t => t.ConsultationTargetBeforeLiving + t.ConsultationTargetBeforeLiving2),
        //        ConsulationCardConsumedTargetBeforeLiving = await performance.SumAsync(t => t.ConsultationCardConsumedTargetBeforeLiving + t.ConsultationCardConsumedTargetBeforeLiving2),
        //        HistoryConsulationCardConsumedTargetBeforeLiving = await performance.SumAsync(t => t.ActivateHistoricalConsultationTargetBeforeLiving),
        //        ConsulationCardRefundTargetBeforeLiving = await performance.SumAsync(t => t.MinivanRefundTargetBeforeLiving),

        //    };
        //    return performanceInfoDto;
        //}

        ///// <summary>
        ///// 派单成交看板业绩目标
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="month"></param>
        ///// <param name="liveAnchorIds">各个平台的主播ID集合</param>
        ///// <returns></returns>
        //public async Task<LiveAnchorBaseBusinessMonthTargetBeforeLivingSendOrDealDto> GetSendOrDealTargetBeforeLivingAsync(int year, int month, List<int> liveAnchorIds)
        //{
        //    var performance = dalLiveAnchorMonthlyTargetBeforeLiving.GetAll().Where(t => t.Year == year && t.Month == month)
        //        .Where(o => liveAnchorIds.Count == 0 || liveAnchorIds.Contains(o.LiveAnchorId));
        //    LiveAnchorBaseBusinessMonthTargetBeforeLivingSendOrDealDto performanceInfoDto = new LiveAnchorBaseBusinessMonthTargetBeforeLivingSendOrDealDto
        //    {
        //        SendOrderTargetBeforeLiving = await performance.SumAsync(t => t.SendOrderTargetBeforeLiving),
        //        TotalVisitTargetBeforeLiving = await performance.SumAsync(t => t.VisitTargetBeforeLiving),
        //        NewCustomerVisitTargetBeforeLiving = await performance.SumAsync(t => t.NewCustomerVisitTargetBeforeLiving),
        //        OldCustomerVisitTargetBeforeLiving = await performance.SumAsync(t => t.OldCustomerVisitTargetBeforeLiving),
        //        TotalDealTargetBeforeLiving = await performance.SumAsync(t => t.DealTargetBeforeLiving),
        //        NewCustomerDealTargetBeforeLiving = await performance.SumAsync(t => t.NewCustomerDealTargetBeforeLiving),
        //        OldCustomerDealTargetBeforeLiving = await performance.SumAsync(t => t.OldCustomerDealTargetBeforeLiving),

        //    };
        //    return performanceInfoDto;
        //}

    }
}
