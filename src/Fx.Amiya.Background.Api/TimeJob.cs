using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Common;
using Fx.Amiya.Core.Dto.Integration;
using Fx.Amiya.Core.Interfaces.Integration;
using Fx.Amiya.Core.Interfaces.MemberCard;
using Fx.Amiya.Dto.TmallOrder;
using Fx.Amiya.IService;
using Fx.Amiya.SyncOrder.Core;
using Fx.Amiya.SyncOrder.Tmall;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pomelo.AspNetCore.TimedJob;
using System.Threading;
using Fx.Amiya.Core.Dto.Goods;
using Fx.Amiya.SyncOrder.TikTok;
using Fx.Amiya.Dto.TikTokOrder;
using Microsoft.Extensions.DependencyInjection;
using Fx.Amiya.Dto.HospitalOperationIndicator;
using Fx.Amiya.SyncOrder.WeChatVideo;
using Fx.Amiya.Dto.WechatVideoOrder;
using Fx.Amiya.Dto.MessageNotice.Input;
using Fx.Amiya.Dto.BindCustomerService;
using Fx.Infrastructure.DataAccess;
using Fx.Amiya.SyncFeishuMultidimensionalTable;
using Fx.Amiya.Dto.TikTokShortVideoData;
using Fx.Amiya.SyncFeishuMultidimensionalTable.FeishuAppConfig;
using Fx.Amiya.Dto.TikTokShortVideoData.Input;
using Fx.Amiya.Dto.Track;

namespace Fx.Amiya.Background.Api
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class TimeJob : Job
    {
        private IOrderService orderService;
        private ISyncOrder syncOrder;
        private IUnitOfWork unitOfWork;
        private ISyncWeiFenXiaoOrder _syncWeiFenXiaoOrder;
        private ISyncTikTokOrder _syncTikTokOrder;
        private FxAppGlobal _fxAppGlobal;
        private IIntegrationAccount integrationAccountService;
        private ICustomerService customerService;
        private IMessageNoticeService messageNoticeService;
        private ICustomerAppointmentScheduleService customerAppointmentScheduleService;
        private IMemberCard memberCardService;
        private IMemberRankInfo memberRankInfoService;
        private IBindCustomerServiceService _bindCustomerService;
        private ITikTokOrderInfoService _tikTokOrderInfoService;
        private IServiceProvider _serviceProvider;
        private ISyncWeChatVideoOrder _syncWeChatVideoOrder;
        private IWeChatVideoOrderService weChatVideoOrderService;
        private IOrderAppInfoService orderAppInfoService;
        private ISyncFeishuMultidimensionalTable syncFeishuMultidimensionalTable;
        private ITikTokShortVideoDataService tikTokShortVideoDataService;
        private ITrackService trackService;
        private IHospitalInfoService hospitalInfoService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderService"></param>
        /// <param name="syncOrder"></param>
        /// <param name="syncWeiFenXiaoOrder"></param>
        /// <param name="fxAppGlobal"></param>
        /// <param name="bindCustomerService"></param>
        /// <param name="integrationAccountService"></param>
        /// <param name="customerService"></param>
        /// <param name="memberCardService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="syncTikTokOrder"></param>
        /// <param name="customerAppointmentScheduleService"></param>
        /// <param name="messageNoticeService"></param>
        /// <param name="memberRankInfoService"></param>
        /// <param name="tokOrderInfoService"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="syncWeChatVideoOrder"></param>
        /// <param name="weChatVideoOrderService"></param>
        /// <param name="orderAppInfoService"></param>
        public TimeJob(IOrderService orderService, ISyncOrder syncOrder, ISyncWeiFenXiaoOrder syncWeiFenXiaoOrder, FxAppGlobal fxAppGlobal, IBindCustomerServiceService bindCustomerService,
          IIntegrationAccount integrationAccountService,
            ICustomerService customerService,
             IMemberCard memberCardService,
             IUnitOfWork unitOfWork,
             IHospitalInfoService hospitalInfoService,
             ISyncTikTokOrder syncTikTokOrder,
             ITrackService trackService,
             ICustomerAppointmentScheduleService customerAppointmentScheduleService,
             IMessageNoticeService messageNoticeService,
             IMemberRankInfo memberRankInfoService, ITikTokOrderInfoService tokOrderInfoService, IServiceProvider serviceProvider, ISyncWeChatVideoOrder syncWeChatVideoOrder = null, IWeChatVideoOrderService weChatVideoOrderService = null, IOrderAppInfoService orderAppInfoService = null, ISyncFeishuMultidimensionalTable syncFeishuMultidimensionalTable = null, ITikTokShortVideoDataService tikTokShortVideoDataService = null)
        {
            this.orderService = orderService;
            this.syncOrder = syncOrder;
            this.trackService = trackService;
            _fxAppGlobal = fxAppGlobal;
            this.messageNoticeService = messageNoticeService;
            this.customerAppointmentScheduleService = customerAppointmentScheduleService;
            _syncWeiFenXiaoOrder = syncWeiFenXiaoOrder;
            this.integrationAccountService = integrationAccountService;
            this.customerService = customerService;
            this.unitOfWork = unitOfWork;
            this.memberCardService = memberCardService;
            _syncTikTokOrder = syncTikTokOrder;
            this.memberRankInfoService = memberRankInfoService;
            _bindCustomerService = bindCustomerService;
            _tikTokOrderInfoService = tokOrderInfoService;
            _serviceProvider = serviceProvider;
            _syncWeChatVideoOrder = syncWeChatVideoOrder;
            this.weChatVideoOrderService = weChatVideoOrderService;
            this.orderAppInfoService = orderAppInfoService;
            this.syncFeishuMultidimensionalTable = syncFeishuMultidimensionalTable;
            this.tikTokShortVideoDataService = tikTokShortVideoDataService;
            this.hospitalInfoService = hospitalInfoService;
        }


        /// <summary>
        /// Begin:开始时间，Interval：隔多长时间执行一次（单位毫秒），SkipWhileExecuting：是否等待上个任务执行完再开始执行
        /// </summary>
        [Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 20, SkipWhileExecuting = true)]
        public async Task Run()
        {
            try
            {
                List<AmiyaOrder> orderList = new List<AmiyaOrder>();
                List<TikTokOrder> tikTokOrderList = new List<TikTokOrder>();
                DateTime date = DateTime.Now;
                //if (_fxAppGlobal.AppConfig.SyncOrderConfig.Tmall == true)
                //{
                //    ////获取天猫发生改变的订单，开始时间和结束时间不能超过一天
                //    var tmallOrderResult = await syncOrder.TranslateTradesSoldChangedOrders(date.AddMinutes(-15), date);
                //    orderList.AddRange(tmallOrderResult);
                //}
                //if (_fxAppGlobal.AppConfig.SyncOrderConfig.WeiFenXiao == true)
                //{
                //    ////获取微分销发生改变的订单，开始时间和结束时间不能超过一天
                //    var weiFenXiaoOrderResult = await _syncWeiFenXiaoOrder.TranslateTradesSoldChangedOrders(date.AddMinutes(-15), date);
                //    orderList.AddRange(weiFenXiaoOrderResult);
                //}

                ////获取抖音发生改变的订单，开始时间和结束时间不能超过一天
                if (_fxAppGlobal.AppConfig.SyncOrderConfig.DouYin == true)
                {
                    ////获取抖音发生改变的订单，开始时间和结束时间不能超过一天
                    //刀刀-啊美雅生活
                    var douYinOrderResult = await _syncTikTokOrder.TranslateTradesSoldChangedOrders(date.AddMinutes(-15), date, 5);
                    tikTokOrderList.AddRange(douYinOrderResult);

                    //吉娜-啊美雅时尚
                    var douYinOrderResult2 = await _syncTikTokOrder.TranslateTradesSoldChangedOrders(date.AddMinutes(-15), date, 111);
                    tikTokOrderList.AddRange(douYinOrderResult2);

                    //刀刀-刀刀气质美学
                    //var douYinOrderResult3 = await _syncTikTokOrder.TranslateTradesSoldChangedOrders(date.AddMinutes(-15), date, 5);
                    //tikTokOrderList.AddRange(douYinOrderResult3);

                    //吉娜-吉娜JINA
                    var douYinOrderResult4 = await _syncTikTokOrder.TranslateTradesSoldChangedOrders(date.AddMinutes(-15), date, 1);
                    tikTokOrderList.AddRange(douYinOrderResult4);

                }
                List<WechatVideoOrder> wechatVideoOrderList = new List<WechatVideoOrder>();
                if (_fxAppGlobal.AppConfig.SyncOrderConfig.WechatVideo == true)
                {
                    //获取视频号订单
                    var liveAnchorIds = await orderAppInfoService.GetOrderAppinfosByType((byte)AppType.WeChatVideo);
                    foreach (var item in liveAnchorIds)
                    {
                        if (!item.BelongLiveAnchorId.HasValue)
                        {
                            continue;
                        }
                        wechatVideoOrderList.AddRange(await _syncWeChatVideoOrder.TranslateTradesSoldChangedOrders(DateTime.Now, DateTime.Now, item.BelongLiveAnchorId.Value));
                    }
                }


                List<OrderInfoAddDto> amiyaOrderList = new List<OrderInfoAddDto>();
                List<TikTokOrderAddDto> tikTokOrderAddList = new List<TikTokOrderAddDto>();
                List<WechatVideoAddDto> wechatVideoList = new List<WechatVideoAddDto>();
                List<ConsumptionIntegrationDto> consumptionIntegrationList = new List<ConsumptionIntegrationDto>();
                foreach (var order in orderList)
                {
                    OrderInfoAddDto amiyaOrder = new OrderInfoAddDto();
                    amiyaOrder.Id = order.Id;
                    amiyaOrder.GoodsName = order.GoodsName;
                    amiyaOrder.GoodsId = order.GoodsId;
                    amiyaOrder.Phone = order.Phone;
                    amiyaOrder.AppointmentHospital = order.AppointmentHospital;
                    amiyaOrder.StatusCode = order.StatusCode;
                    amiyaOrder.ActualPayment = order.ActualPayment;
                    amiyaOrder.CreateDate = order.CreateDate;
                    amiyaOrder.UpdateDate = order.UpdateDate;
                    amiyaOrder.WriteOffDate = order.WriteOffDate;
                    amiyaOrder.ThumbPicUrl = order.ThumbPicUrl;
                    amiyaOrder.BuyerNick = order.BuyerNick;
                    amiyaOrder.AppType = order.AppType;
                    if (order.AppType == 0)
                    {
                        amiyaOrder.AccountReceivable = order.AccountReceivable;
                    }
                    else
                    {
                        amiyaOrder.AccountReceivable = order.ActualPayment;
                    }
                    amiyaOrder.IsAppointment = order.IsAppointment;
                    amiyaOrder.OrderType = order.OrderType;
                    amiyaOrder.Quantity = order.Quantity;
                    amiyaOrder.ExchangeType = (byte)ExchangeType.ThirdPartyPayment;
                    int belongEmpId = await _bindCustomerService.GetEmployeeIdByPhone(order.Phone);
                    if (belongEmpId > 0)
                    { amiyaOrder.BelongEmpId = belongEmpId; }
                    amiyaOrderList.Add(amiyaOrder);
                    //计算积分
                    if (order.StatusCode == "TRADE_FINISHED" && order.ActualPayment >= 1 && !string.IsNullOrWhiteSpace(order.Phone))
                    {

                        bool isIntegrationGenerateRecord = await integrationAccountService.GetIsIntegrationGenerateRecordByOrderIdAsync(order.Id);
                        if (isIntegrationGenerateRecord == true)
                            continue;

                        var customerId = await customerService.GetCustomerIdByPhoneAsync(order.Phone);
                        if (string.IsNullOrWhiteSpace(customerId))
                            continue;

                        ConsumptionIntegrationDto consumptionIntegration = new ConsumptionIntegrationDto();
                        consumptionIntegration.CustomerId = customerId;
                        consumptionIntegration.OrderId = order.Id;
                        consumptionIntegration.AmountOfConsumption = (decimal)order.ActualPayment;
                        consumptionIntegration.Date = date;

                        var memberCard = await memberCardService.GetMemberCardHandelByCustomerIdAsync(customerId);
                        if (memberCard != null)
                        {
                            consumptionIntegration.Quantity = Math.Floor(memberCard.GenerateIntegrationPercent * (decimal)order.ActualPayment);
                            consumptionIntegration.Percent = memberCard.GenerateIntegrationPercent;
                        }
                        else
                        {
                            var memberRank = await memberRankInfoService.GetMinGeneratePercentMemberRankInfoAsync();
                            consumptionIntegration.Quantity = Math.Floor(memberRank.GenerateIntegrationPercent * (decimal)order.ActualPayment);
                            consumptionIntegration.Percent = memberRank.GenerateIntegrationPercent;

                        }

                        if (consumptionIntegration.Quantity > 0)
                            consumptionIntegrationList.Add(consumptionIntegration);
                        var findInfo = await _bindCustomerService.GetEmployeeIdByPhone(order.Phone);
                        if (findInfo != 0)
                        {
                            await _bindCustomerService.UpdateConsumePriceAsync(order.Phone, order.ActualPayment.Value, (int)OrderFrom.ThirdPartyOrder, "", "", "天猫", 1);
                        }
                    }
                }
                //抖店订单
                foreach (var order in tikTokOrderList)
                {
                    TikTokOrderAddDto tikTokOrder = new TikTokOrderAddDto();
                    tikTokOrder.Id = order.Id;
                    tikTokOrder.GoodsName = order.GoodsName;
                    tikTokOrder.GoodsId = order.GoodsId;
                    tikTokOrder.AppointmentHospital = order.AppointmentHospital;
                    tikTokOrder.StatusCode = order.StatusCode;
                    tikTokOrder.ActualPayment = order.ActualPayment;
                    tikTokOrder.CreateDate = order.CreateDate;
                    tikTokOrder.UpdateDate = order.UpdateDate;
                    tikTokOrder.WriteOffDate = order.WriteOffDate;
                    tikTokOrder.FinishDate = order.FinishDate;
                    tikTokOrder.BelongLiveAnchorId = order.BelongLiveAnchorId;
                    tikTokOrder.ThumbPicUrl = order.ThumbPicUrl;
                    tikTokOrder.AppType = order.AppType;
                    tikTokOrder.AccountReceivable = order.ActualPayment;
                    tikTokOrder.IsAppointment = order.IsAppointment;
                    tikTokOrder.OrderType = order.OrderType;
                    tikTokOrder.Quantity = order.Quantity;
                    tikTokOrder.ExchangeType = (byte)ExchangeType.ThirdPartyPayment;
                    tikTokOrder.TikTokUserId = order.TikTokUserId;
                    tikTokOrder.CipherPhone = order.CipherPhone;
                    tikTokOrder.CipherName = order.CipherName;
                    tikTokOrderAddList.Add(tikTokOrder);
                }
                foreach (var item in wechatVideoOrderList)
                {
                    WechatVideoAddDto add = new WechatVideoAddDto();
                    add.Id = item.Id;
                    add.GoodsName = item.GoodsName;
                    add.GoodsId = item.GoodsId;
                    add.Phone = item.Phone;
                    add.StatusCode = item.StatusCode;
                    add.ActualPayment = item.ActualPayment;
                    add.AccountReceivable = item.AccountReceivable;
                    add.CreateDate = item.CreateDate.Value;
                    add.UpdateDate = item.UpdateDate;
                    add.ThumbPicUrl = item.ThumbPicUrl;
                    add.BuyerNick = item.BuyerNick;
                    add.OrderType = item.OrderType;
                    add.Quantity = item.Quantity;
                    add.BelongLiveAnchorId = item.BelongLiveAnchorId;
                    wechatVideoList.Add(add);
                }
                await orderService.AddOrderAsync(amiyaOrderList);
                await _tikTokOrderInfoService.AddAsync(tikTokOrderAddList);
                await weChatVideoOrderService.AddAsync(wechatVideoList);
                foreach (var item in consumptionIntegrationList)
                {
                    await integrationAccountService.AddByConsumptionAsync(item);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }
        ///// <summary>
        ///// 修改运营指标提报/批注状态(一天运行一次)【运营指标板块线上暂未使用】
        ///// </summary>
        ///// <returns></returns>
        //[Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 60 * 24 + 60 * 1000, SkipWhileExecuting = true)]
        //public async Task HandleOperationIndicatorAsync()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var hospitalOperationIndicator = scope.ServiceProvider.GetService<IHospitalOperationIndicatorService>();
        //        var indicatorSendToHospital = scope.ServiceProvider.GetService<IIndicatorSendHospitalService>();
        //        var indicatorList = await hospitalOperationIndicator.GetUnSumbitAndUnRemarkIndicatorAsync();
        //        var expireIndicatorList = await hospitalOperationIndicator.GetUnValidIndicatorAsync();
        //        foreach (var indicator in indicatorList)
        //        {
        //            var status = await indicatorSendToHospital.SubmitAndRemarkStatusAsync(indicator.Id);
        //            UpdateSubmitAndRemarkStatus update = new UpdateSubmitAndRemarkStatus();
        //            if (status.RemarkStatus == true)
        //            {
        //                update.RemarkStatus = true;
        //            }
        //            if (status.SumbitStatus == true)
        //            {
        //                update.SubmitStatus = true;
        //            }
        //            update.Id = indicator.Id;
        //            await hospitalOperationIndicator.UpdateRemarkAndSubmitStatusAsync(update);
        //        }
        //        foreach (var item in expireIndicatorList)
        //        {
        //            await hospitalOperationIndicator.DeleteAsync(item.Id);
        //        }
        //    }
        //}

        /// <summary>
        /// 新增消息通知(一天运行一次)
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        [Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 2, SkipWhileExecuting = true)]//3分钟运行一次
        //[Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 60 * 24 + 60 * 1000, SkipWhileExecuting = true)]
        public async Task OrderMessageNotice()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var contentPlateFormOrderService = scope.ServiceProvider.GetService<IContentPlateFormOrderService>();
                #region 【派单通知】
                #region【七日派单通知】
                int sevenDay = 7;
                var sendOrderSevenDay = await contentPlateFormOrderService.GetSendOrderByDateList(sevenDay);
                foreach (var sevendaysendDateOrder in sendOrderSevenDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (sevendaysendDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = sevendaysendDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = sevendaysendDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + sevendaysendDateOrder.Id + " 已派单超过" + sevenDay + "日，请及时跟进~";
                    //消息通知
                    await messageNoticeService.AddAsync(addMessageNoticeDto);

                    DealAfterAddTrackDto dealAfterAddTrackDto = new DealAfterAddTrackDto();
                    dealAfterAddTrackDto.Phone = sevendaysendDateOrder.Phone;
                    dealAfterAddTrackDto.EmployeeId = addMessageNoticeDto.AcceptBy;
                    dealAfterAddTrackDto.Days = sevenDay;
                    //添加派单回访
                    await trackService.AddWaitTrackUnToHospitalAsync(dealAfterAddTrackDto);

                }
                #endregion
                #region【十五日派单通知】
                int fifteenDays = 15;
                var sendOrderFifteenDay = await contentPlateFormOrderService.GetSendOrderByDateList(fifteenDays);
                foreach (var fifTeendaysendDateOrder in sendOrderFifteenDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (fifTeendaysendDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = fifTeendaysendDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = fifTeendaysendDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + fifTeendaysendDateOrder.Id + " 已派单超过" + fifteenDays + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);

                    DealAfterAddTrackDto dealAfterAddTrackDto = new DealAfterAddTrackDto();
                    dealAfterAddTrackDto.Phone = fifTeendaysendDateOrder.Phone;
                    dealAfterAddTrackDto.EmployeeId = addMessageNoticeDto.AcceptBy;
                    dealAfterAddTrackDto.Days = fifteenDays;
                    //添加派单回访
                    await trackService.AddWaitTrackUnToHospitalAsync(dealAfterAddTrackDto);
                }
                #endregion
                #region【三十日派单通知】
                int thirtyDays = 30;
                var sendOrderThirtyDay = await contentPlateFormOrderService.GetSendOrderByDateList(thirtyDays);
                foreach (var ThirtydaysendDateOrder in sendOrderThirtyDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (ThirtydaysendDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = ThirtydaysendDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = ThirtydaysendDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + ThirtydaysendDateOrder.Id + " 已派单超过" + thirtyDays + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);

                    DealAfterAddTrackDto dealAfterAddTrackDto = new DealAfterAddTrackDto();
                    dealAfterAddTrackDto.Phone = ThirtydaysendDateOrder.Phone;
                    dealAfterAddTrackDto.EmployeeId = addMessageNoticeDto.AcceptBy;
                    dealAfterAddTrackDto.Days = thirtyDays;
                    //添加派单回访
                    await trackService.AddWaitTrackUnToHospitalAsync(dealAfterAddTrackDto);
                }
                #endregion
                #region【九十日派单通知】
                int nintyDays = 90;
                var sendOrderNintyDay = await contentPlateFormOrderService.GetSendOrderByDateList(nintyDays);
                foreach (var NintydaysendDateOrder in sendOrderNintyDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (NintydaysendDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = NintydaysendDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = NintydaysendDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + NintydaysendDateOrder.Id + " 已派单超过" + nintyDays + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                    DealAfterAddTrackDto dealAfterAddTrackDto = new DealAfterAddTrackDto();
                    dealAfterAddTrackDto.Phone = NintydaysendDateOrder.Phone;
                    dealAfterAddTrackDto.EmployeeId = addMessageNoticeDto.AcceptBy;
                    dealAfterAddTrackDto.Days = nintyDays;
                    //添加派单回访
                    await trackService.AddWaitTrackUnToHospitalAsync(dealAfterAddTrackDto);
                }
                #endregion
                #region【一百八十日派单通知】
                int oneHundredAndEightDays = 180;
                var sendOrderoneHundredAndEightDay = await contentPlateFormOrderService.GetSendOrderByDateList(oneHundredAndEightDays);
                foreach (var oneHundredAndEightdaysendDateOrder in sendOrderoneHundredAndEightDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (oneHundredAndEightdaysendDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = oneHundredAndEightdaysendDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = oneHundredAndEightdaysendDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + oneHundredAndEightdaysendDateOrder.Id + " 已派单超过" + oneHundredAndEightDays + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                    DealAfterAddTrackDto dealAfterAddTrackDto = new DealAfterAddTrackDto();
                    dealAfterAddTrackDto.Phone = oneHundredAndEightdaysendDateOrder.Phone;
                    dealAfterAddTrackDto.EmployeeId = addMessageNoticeDto.AcceptBy;
                    dealAfterAddTrackDto.Days = oneHundredAndEightDays;
                    //添加派单回访
                    await trackService.AddWaitTrackUnToHospitalAsync(dealAfterAddTrackDto);
                }
                #endregion
                #endregion

                #region 【成交通知】

                #region【三十日成交通知】
                int thirtyDaysDeal = 30;
                var dealOrderThirtyDay = await contentPlateFormOrderService.GetOrderDealByDateList(thirtyDaysDeal);
                foreach (var ThirtydaydealDateOrder in dealOrderThirtyDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (ThirtydaydealDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = ThirtydaydealDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = ThirtydaydealDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + ThirtydaydealDateOrder.Id + " 已成交超过" + thirtyDaysDeal + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                }
                #endregion
                #region【四十五日成交通知】
                int fourtyFiveDaysDeal = 45;
                var dealOrderFourtyFiveDay = await contentPlateFormOrderService.GetOrderDealByDateList(fourtyFiveDaysDeal);
                foreach (var FourtyFivedaydealDateOrder in dealOrderFourtyFiveDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (FourtyFivedaydealDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = FourtyFivedaydealDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = FourtyFivedaydealDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + FourtyFivedaydealDateOrder.Id + " 已成交超过" + fourtyFiveDaysDeal + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                }
                #endregion
                #region【六十日成交通知】
                int sixtyFiveDaysDeal = 60;
                var dealOrderSixtyDay = await contentPlateFormOrderService.GetOrderDealByDateList(sixtyFiveDaysDeal);
                foreach (var SixtydaydealDateOrder in dealOrderSixtyDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (SixtydaydealDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = SixtydaydealDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = SixtydaydealDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + SixtydaydealDateOrder.Id + " 已成交超过" + sixtyFiveDaysDeal + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                }
                #endregion
                #region【九十日成交通知】
                int ninetyFiveDaysDeal = 90;
                var dealOrderNinetyDay = await contentPlateFormOrderService.GetOrderDealByDateList(ninetyFiveDaysDeal);
                foreach (var NinetydaydealDateOrder in dealOrderNinetyDay)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    if (NinetydaydealDateOrder.IsSupportOrder == true)
                    {
                        addMessageNoticeDto.AcceptBy = NinetydaydealDateOrder.SupportEmpId;
                    }
                    else
                    {
                        addMessageNoticeDto.AcceptBy = NinetydaydealDateOrder.BelongEmpId.Value;
                    }
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OrderNotice;
                    addMessageNoticeDto.NoticeContent = "您的订单：" + NinetydaydealDateOrder.Id + " 已成交超过" + ninetyFiveDaysDeal + "日，请及时跟进~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                }
                #endregion
                #endregion

                #region【重要日程通知】
                var todayCustomerAppointmentSchedule = await customerAppointmentScheduleService.GetTodayImportantScheduleAsync();
                foreach (var todayCustomerAppointScheduleInfo in todayCustomerAppointmentSchedule)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    addMessageNoticeDto.AcceptBy = todayCustomerAppointScheduleInfo.CreateBy;
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.ScheduleNotice;
                    addMessageNoticeDto.NoticeContent = "您今日有'" + todayCustomerAppointScheduleInfo.AppointmentTypeText + "'待处理，客户昵称：" + todayCustomerAppointScheduleInfo.CustomerName + ",联系方式：" + todayCustomerAppointScheduleInfo.Phone + "，请到客户预约日程中处理~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);
                }
                #endregion

                #region【机构合同到期通知(发送角色：特定人员：宇哥账户，张宇苍账户)】
                var todayHospitalExpired = await hospitalInfoService.GetAboutToExpiredListAsync();
                foreach (var aboutToExpiredHospital in todayHospitalExpired.List)
                {
                    AddMessageNoticeDto addMessageNoticeDto = new AddMessageNoticeDto();
                    addMessageNoticeDto.AcceptBy = 1;//上线后62改为1
                    addMessageNoticeDto.NoticeType = (int)MessageNoticeMessageTextEnum.OperationNotice;
                    addMessageNoticeDto.NoticeContent = "'" + aboutToExpiredHospital.Name + "'的合同即将到期，请到医院列表中进行处理~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto);

                    AddMessageNoticeDto addMessageNoticeDto2 = new AddMessageNoticeDto();
                    addMessageNoticeDto2.AcceptBy = 291;
                    addMessageNoticeDto2.NoticeType = (int)MessageNoticeMessageTextEnum.OperationNotice;
                    addMessageNoticeDto2.NoticeContent = "'" + aboutToExpiredHospital.Name + "'的合同即将到期，请到医院列表中进行处理~";
                    await messageNoticeService.AddAsync(addMessageNoticeDto2);
                }
                #endregion
            }
        }

        /// <summary>
        /// 新增客户RFM数据更改
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        [Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 5, SkipWhileExecuting = true)]//5分钟运行一次
        public async Task BindCustomerServiceRFMUpdate()
        {
            unitOfWork.BeginTransaction();
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var bindCustomerServiceService = scope.ServiceProvider.GetService<IBindCustomerServiceService>();
                    //获取所有消费过的顾客
                    var allConsumedCustomer = await bindCustomerServiceService.GetAllCustomerAsync();
                    int OldRFMLevel = 0;
                    int NewRFMLevel = 0;
                    foreach (var x in allConsumedCustomer)
                    {
                        int RValue = 0;
                        int FValue = 0;
                        int MValue = 0;
                        OldRFMLevel = x.RfmType;
                        //计算距今消费时间
                        if (x.ConsumptionDate < 91)
                        {
                            RValue = (int)RFM.High;
                        }
                        else
                        {
                            RValue = (int)RFM.Low;
                        }
                        if (x.AllOrderCount > 1)
                        {
                            FValue = (int)RFM.High;
                        }
                        else
                        {
                            FValue = (int)RFM.Low;
                        }
                        switch (x.AllPrice)
                        {
                            case < 100000.00M:
                                MValue = (int)RFM.Low;
                                break;
                            case < 300000.00M:
                                MValue = (int)RFM.High;
                                break;
                            case >= 300000.00M:
                                MValue = (int)RFM.VIP;
                                break;
                        }

                        switch (RValue, FValue, MValue)
                        {
                            case ((int)RFM.High, (int)RFM.High, (int)RFM.High):
                                NewRFMLevel = (int)RFMTagLevel.R1;
                                break;
                            case ((int)RFM.High, (int)RFM.Low, (int)RFM.High):
                                NewRFMLevel = (int)RFMTagLevel.R2;
                                break;
                            case ((int)RFM.Low, (int)RFM.High, (int)RFM.High):
                                NewRFMLevel = (int)RFMTagLevel.R3;
                                break;
                            case ((int)RFM.Low, (int)RFM.Low, (int)RFM.High):
                                NewRFMLevel = (int)RFMTagLevel.R4;
                                break;
                            case ((int)RFM.High, (int)RFM.High, (int)RFM.Low):
                                NewRFMLevel = (int)RFMTagLevel.R5;
                                break;
                            case ((int)RFM.High, (int)RFM.Low, (int)RFM.Low):
                                NewRFMLevel = (int)RFMTagLevel.R6;
                                break;
                            case ((int)RFM.Low, (int)RFM.High, (int)RFM.Low):
                                NewRFMLevel = (int)RFMTagLevel.R7;
                                break;
                            case ((int)RFM.Low, (int)RFM.Low, (int)RFM.Low):
                                NewRFMLevel = (int)RFMTagLevel.R8;
                                break;
                            default:
                                NewRFMLevel = (int)RFMTagLevel.RV;
                                break;
                        }
                        if (OldRFMLevel != NewRFMLevel)
                        {
                            //修改绑定客服RFM登记
                            await bindCustomerServiceService.UpdateCustomerRFMLevelAsync(x.Id, NewRFMLevel);
                            //新增修改记录
                            AddBindCustomerRFMLevelUpdateLog addBindCustomerRFMLevelUpdateLog = new AddBindCustomerRFMLevelUpdateLog();
                            addBindCustomerRFMLevelUpdateLog.BindCustomerServiceId = x.Id;
                            addBindCustomerRFMLevelUpdateLog.CustomerServiceId = x.CustomerServiceId;
                            addBindCustomerRFMLevelUpdateLog.From = OldRFMLevel;
                            addBindCustomerRFMLevelUpdateLog.To = NewRFMLevel;
                            await bindCustomerServiceService.AddRFMTypeUpdateLogAsync(addBindCustomerRFMLevelUpdateLog);
                        }
                    }
                    unitOfWork.Commit();
                }
            }
            catch (Exception err)
            {
                unitOfWork.RollBack();

            }
        }
        /// <summary>
        /// 同步多维表格短视频数据
        /// </summary>
        /// <returns></returns>
        [Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 60 * 6, SkipWhileExecuting = true)]//每6小时运行一次
        public async Task SyncMultidimensionalTableDataAsync()
        {
            var liveAnchorIds = await syncFeishuMultidimensionalTable.GetLiveAnchorIdsAsync();
            List<ShortVideoDataInfo> list = new List<ShortVideoDataInfo>();
            foreach (var id in liveAnchorIds)
            {
                var tableLiveAnchors = await syncFeishuMultidimensionalTable.GetTableLiveAnchorIdsAsync(id.AppId, FeishuTableType.VideoData);
                foreach (var tableid in tableLiveAnchors)
                {
                    var dataList = await syncFeishuMultidimensionalTable.GetShortVideoDataByCodeAsync(id.LiveAnchorId, tableid);
                    list.AddRange(dataList);
                }
            }

            if (list.Count > 0)
            {
                var data = list.Select(e => new AddTikTokShortVideoDataDto
                {
                    VideoId = e.VideoId,
                    PlayNum = e.PlayNum,
                    Title = e.Title,
                    Like = e.Like,
                    Comments = e.Comments,
                    BelongLiveAnchorId = e.BelongLiveAnchorId
                }).ToList();
                await tikTokShortVideoDataService.AddListAsync(data);
            }
        }
        /// <summary>
        /// 同步多维表格短视频评论数据
        /// </summary>
        /// <returns></returns>
        [Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 60 * 6, SkipWhileExecuting = true)]//每6小时运行一次
        public async Task SyncMultidimensionalTableCommentsDataAsync()
        {
            var liveAnchorIds = await syncFeishuMultidimensionalTable.GetLiveAnchorIdsAsync();
            List<ShortVideocommentsInfo> list = new List<ShortVideocommentsInfo>();
            foreach (var id in liveAnchorIds)
            {
                var tableLiveAnchors = await syncFeishuMultidimensionalTable.GetTableLiveAnchorIdsAsync(id.AppId, FeishuTableType.Comments);
                foreach (var tableid in tableLiveAnchors)
                {
                    var dataList = await syncFeishuMultidimensionalTable.GetShortVideoCommentsAsync(id.LiveAnchorId, tableid);
                    list.AddRange(dataList);
                }
            }

            if (list.Count > 0)
            {
                var data = list.Select(e => new AddTikTokShortVideoCommentsDto
                {
                    CommentsId = e.CommentsId,
                    CommentsUserId = e.CommentsUserId,
                    CommentsUserName = e.CommentsUserName,
                    LikeCount = e.LikeCount,
                    CommentsDate = e.CommentsDate,
                    Comments = e.Comments,
                    BelongLiveAnchorId = e.BelongLiveAnchorId.Value
                }).ToList();
                await tikTokShortVideoDataService.AddCommentsListAsync(data);
            }
        }
        /// <summary>
        /// 同步多维表格短视频粉丝数据
        /// </summary>
        /// <returns></returns>
        [Invoke(Begin = "00:00:00", Interval = 1000 * 60 * 60 * 6, SkipWhileExecuting = true)]//每6小时运行一次
        public async Task SyncMultidimensionalTableFansDataAsync()
        {
            var liveAnchorIds = await syncFeishuMultidimensionalTable.GetLiveAnchorIdsAsync();
            List<ShortVideoFansDataInfo> list = new List<ShortVideoFansDataInfo>();
            foreach (var id in liveAnchorIds)
            {
                var tableLiveAnchors = await syncFeishuMultidimensionalTable.GetTableLiveAnchorIdsAsync(id.AppId, FeishuTableType.FansData);
                foreach (var tableid in tableLiveAnchors)
                {
                    var dataList = await syncFeishuMultidimensionalTable.GetShortVideoFansDataAsync(id.LiveAnchorId, tableid);
                    list.AddRange(dataList);
                }
            }

            if (list.Count > 0)
            {
                var data = list.Select(e => new AddTikTokFansDataDto
                {
                    StatsDate = e.StatsDate,
                    NewFansCount = e.NewFansCount,
                    TotalFansCount = e.TotalFansCount,
                    BelongLiveAnchorId = e.BelongLiveAnchorId.Value
                }).ToList();
                await tikTokShortVideoDataService.AddFansListAsync(data);
            }
        }

    }
}
