using System;
using System.Collections.Generic;
using System.Text;

namespace Fx.Amiya.IService
{
    public enum AuthorizeStatusEnum
    {
        /// <summary>
        /// 管理端
        /// </summary>
        InternalAuthorize = 0,
        /// <summary>
        /// 医院端
        /// </summary>
        TenantAuhtorize = 1,

    }


    /// <summary>
    /// 预约状态
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// 待完成=1
        /// </summary>
        WaitAccomplish = 1,

        /// <summary>
        /// 已完成=2
        /// </summary>
        Finish = 2,

        /// <summary>
        /// 取消=3
        /// </summary>
        Cancel = 3,
        /// <summary>
        /// 已派单
        /// </summary>
        SendToHospital = 4
    }
    /// <summary>
    /// 预约叫车抵扣类型
    /// </summary>
    public enum AppointmentCarExchangeType
    {
        //积分抵扣
        PointDeduction = 0,
        //优惠券抵扣
        VoucherDeduction = 1
    }
    /// <summary>
    /// 预约车型
    /// </summary>
    public enum AppointmentCarType
    {
        //经济型
        EconomyType = 0,
        //舒适型
        ConfirmtableType = 1,
        //商务型
        BusinessType = 2,
        //豪华型
        LuxuriousType = 3
    }
    /// <summary>
    /// 预约叫车状态
    /// </summary>
    public enum AppointmentCarStatus
    {
        //提交处理
        Commit = 0,
        //取消预约
        Cancle = 1,
        //预约成功
        Completed = 2
    }

    public enum CheckType
    {
        /// <summary>
        /// 未审核
        /// </summary>
        NotChecked = 0,
        /// <summary>
        /// 审核不通过
        /// </summary>
        CheckNotPassed = 1,
        /// <summary>
        /// 审核通过
        /// </summary>
        CheckedSuccess = 2,
        /// <summary>
        /// 审核中
        /// </summary>
        Checking = 3,

    }

    /// <summary>
    /// 对账单审核记录生成薪资时的审核类型
    /// </summary>
    public enum ReconciliationDocumentSettleCheckType
    {
        /// <summary>
        /// 其他
        /// </summary>
        Others = 0,
        /// <summary>
        /// 自播达人审核
        /// </summary>
        SelfLiveAnchorCheck = 1,
        /// <summary>
        /// 供应链达人审核
        /// </summary>
        CooperationLiveAnchorCheck = 2,
        /// <summary>
        /// 三方订单升单审核
        /// </summary>
        OtherPlatformBuyAgainCheck = 3
    }

    /// <summary>
    /// 请求类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        Add = 0,
        /// <summary>
        /// 删除数据
        /// </summary>
        Delete = 1,
        /// <summary>
        /// 更新数据
        /// </summary>
        Update = 2,
        /// <summary>
        /// 查询数据
        /// </summary>
        Select = 3,
        /// <summary>
        /// 导出数据
        /// </summary>
        Export = 4,
        /// <summary>
        /// 支付回调
        /// </summary>
        Pay = 5,
        /// <summary>
        /// 用户登陆
        /// </summary>
        Login = 6

    }

    /// <summary>
    /// 请求来源
    /// </summary>
    public enum RequestSource
    {
        /// <summary>
        /// 啊美雅预约系统
        /// </summary>
        AmiyaBackground = 0,
        /// <summary>
        /// 啊美雅企业微信
        /// </summary>
        AmiyaBusinessWechat = 1
    }

    public enum SubmintType
    {
        /// <summary>
        /// 未提交
        /// </summary>
        UnSubmit = 0,
        /// <summary>
        /// 已提交
        /// </summary>
        Submited = 1,
    }

    /// <summary>
    /// 派单顺序
    /// </summary>
    public enum SendOrder
    {
        /// <summary>
        /// 首派
        /// </summary>
        First = 0,
        /// <summary>
        /// 次派
        /// </summary>
        Second = 1,
        /// <summary>
        /// 查重单
        /// </summary>
        SelectRepeatOrder = 2,
        /// <summary>
        /// 首派(微整)
        /// </summary>
        FirstMinorAdjustment = 3,
        /// <summary>
        /// 首派(整外)
        /// </summary>
        FirstAdjustmentOut = 4,
        /// <summary>
        /// 首派(新合作整外)
        /// </summary>
        FirstNewAdjustmentOut = 5,
        /// <summary>
        /// 次派(微整)
        /// </summary>
        SecondMinorAdjustment = 6,

    }
    /// <summary>
    /// 年服务费和保证金缴纳状态
    /// </summary>
    public enum YearServiceFeeOrSecurityDeposit
    {
        /// <summary>
        /// 已缴纳
        /// </summary>
        Paid = 0,
        /// <summary>
        /// 未缴纳
        /// </summary>
        UnPaid = 1,
        /// <summary>
        /// 未缴纳(走审批)
        /// </summary>
        UnPaidAndApproval = 2
    }

    public enum TagType
    {
        /// <summary>
        /// 规模标签=0
        /// </summary>
        ScaleTag,

        /// <summary>
        /// 设施标签=1
        /// </summary>
        FacilityTag
    }


    public enum WxEventType
    {
        /// <summary>
        /// 首次关注=0
        /// </summary>
        FirstSubscribe,

        /// <summary>
        /// 再次关注=1
        /// </summary>
        AgainSubscribe
    }


    public enum WxRspMsgType
    {
        /// <summary>
        /// 文本消息=1
        /// </summary>
        TextMessage = 1,

        /// <summary>
        /// 图文消息=2
        /// </summary>
        NewsMessage = 2,
    }



    public enum EmployeeType
    {
        /// <summary>
        /// 啊美雅员工
        /// </summary>
        AmiyaEmployee = 1,



        /// <summary>
        /// 医院员工
        /// </summary>
        HospitalEmployee = 2
    }






    /// <summary>
    /// 留言类型（留言/回复）
    /// </summary>
    public enum LeaveMessageType
    {
        /// <summary>
        /// 留言
        /// </summary>
        LeaveMessage = 1,

        /// <summary>
        /// 回复
        /// </summary>
        Reply = 2
    }

    /// <summary>
    /// 升单类型枚举
    /// </summary>
    public enum BuyAgainType
    {
        /// <summary>
        /// 升单5%
        /// </summary>
        FivePercent = 0,
        /// <summary>
        /// 升单30%
        /// </summary>
        ThirtyPercent = 1,
        /// <summary>
        /// 升单35%
        /// </summary>
        ThirtyFivePercent = 2,
        /// <summary>
        /// 升单10%
        /// </summary>
        TenPercent = 3,
        /// <summary>
        /// 升单15%
        /// </summary>
        FifteenPercent = 4,
        /// <summary>
        /// 升单20%
        /// </summary>
        TwentyPercent = 5,
        /// <summary>
        /// 升单50%
        /// </summary>
        FiftyPercent = 6,
    }

    /// <summary>
    /// 升单渠道
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        /// 医院添加
        /// </summary>
        Hospital = 0,
        /// <summary>
        /// 天猫
        /// </summary>
        TaoBao = 1,
        /// <summary>
        /// 抖音
        /// </summary>
        TikTok = 2,
        /// <summary>
        /// 抖音代运营
        /// </summary>
        TikTokThirdpartnar = 3,

    }

    /// <summary>
    /// 下单平台
    /// </summary>
    public enum AppType
    {
        /// <summary>
        /// 天猫=0
        /// </summary>
        Tmall,

        /// <summary>
        /// 京东=1
        /// </summary>
        JD,

        /// <summary>
        /// 小程序=2
        /// </summary>
        MiniProgram,

        /// <summary>
        /// 公众号（微分销平台）=3
        /// </summary>
        WeChatOfficialAccount,
        /// <summary>
        /// 抖音=4
        /// </summary>
        Douyin,
        /// <summary>
        /// 啊美雅企业微信=5
        /// </summary>
        AmiyaBusinessWechat,
        /// <summary>
        /// 医院端企业微信=6
        /// </summary>
        HospitalBusinessWechat,
        /// <summary>
        /// 微信视频号=7
        /// </summary>
        WeChatVideo,
        /// <summary>
        /// 抖音本地生活
        /// </summary>
        TikTokLocal = 8,
        /// <summary>
        /// 官网
        /// </summary>
        OfficialWebsite = 9
    }

    #region 内容平台相关枚举
    /// <summary>
    /// 内容平台订单的订单类型
    /// </summary>
    public enum ContentPlateFormOrderType
    {
        /// <summary>
        /// 咨询=1
        /// </summary>
        SEEK_ADVICE = 1,

        /// <summary>
        /// 定金=2
        /// </summary>
        BARGAIN_MONEY = 2,

        /// <summary>
        /// 预约
        /// </summary>
        APPOINTMENT = 3,
    }

    /// <summary>
    /// 内容平台订单面诊状态
    /// </summary>
    public enum ContentPlateFormOrderConsultationType
    {

        /// <summary>
        /// 照片(独立)面诊=1
        /// </summary>
        IndependentFollowUp = 1,

        /// <summary>
        /// 视频面诊=2
        /// </summary>
        Collaboration = 2,
        /// <summary>
        /// 语音面诊=3
        /// </summary>
        Voice = 3,
        ///// <summary>
        ///// 未面诊
        ///// </summary>
        //UnConsulation = 4,
        /// <summary>
        /// 其他=0
        /// </summary>
        OTHER = 0,

    }


    /// <summary>
    /// 内容平台订单到院类型
    /// </summary>
    public enum ContentPlateFormOrderToHospitalType
    {
        /// <summary>
        /// 初诊=1
        /// </summary>
        FIRST_SEEK_ADVICE = 1,

        /// <summary>
        /// 复诊=2
        /// </summary>
        AGAIN_SEEK_ADVICE = 2,
        /// <summary>
        /// 再消费=3
        /// </summary>
        AGAIN_CONSUMPTION = 3,
        /// <summary>
        /// 退款=4
        /// </summary>
        REFUND = 4,
        /// <summary>
        /// 其他
        /// </summary>
        OTHER = 0,
    }

    /// <summary>
    /// 内容平台订单的订单来源
    /// </summary>
    public enum ContentPlateFormOrderSource
    {
        /// <summary>
        /// 面诊卡
        /// </summary>
        Consultation_Card = 1,

        /// <summary>
        /// 非面诊卡
        /// </summary>
        Other = 2,
        /// <summary>
        /// 美肤卡
        /// </summary>
        BeautifulSkinCard = 3,
        /// <summary>
        /// 直播间
        /// </summary>
        LivingRoom = 4,

        /// <summary>
        /// 短视频
        /// </summary>
        ShortVideo = 5,

        /// <summary>
        /// 私信
        /// </summary>
        PersonalLetter = 6,
    }

    /// <summary>
    /// 内容平台的订单状态
    /// </summary>
    public enum ContentPlateFormOrderStatus
    {
        /// <summary>
        /// 未派单=1
        /// </summary>
        HaveOrder = 1,

        /// <summary>
        /// 已派单=2
        /// </summary>
        SendOrder = 2,

        /// <summary>
        /// 已接单
        /// </summary>
        ConfirmOrder = 3,

        /// <summary>
        /// 已成交=4
        /// </summary>
        OrderComplete = 4,

        /// <summary>
        /// 医院重单-不可深度=5
        /// </summary>
        RepeatOrder = 5,

        /// <summary>
        /// 未成交=6
        /// </summary>
        WithoutCompleteOrder = 6,
        /// <summary>
        /// 医院重单-可深度
        /// </summary>
        RepeatOrderProfundity = 7
    }

    /// <summary>
    /// 业绩类型
    /// </summary>
    public enum ContentPlateFormOrderDealPerformanceType
    {
        /// <summary>
        /// 其他
        /// </summary>
        Others = 0,
        /// <summary>
        /// 助理激活
        /// </summary>
        AssistantActivate = 1,
        /// <summary>
        /// VIP管家激活
        /// </summary>
        VIPHousekeeperActivate = 2,
        /// <summary>
        /// 机构报单
        /// </summary>
        HospitalDeclaration = 3,
        /// <summary>
        /// 助理稽查
        /// </summary>
        AssistantCheck = 4,
        /// <summary>
        /// 财务稽查
        /// </summary>
        FinanceCheck = 5,
        /// <summary>
        /// VIP管家稽查
        /// </summary>
        VIPHousekeeperCheck = 6,
        /// <summary>
        /// 机构报单-API
        /// </summary>
        HospitalDeclarationInApi = 7,
        /// <summary>
        /// 助理补单
        /// </summary>
        CustomerServiceReplenishmentOrder = 8
    }
    /// <summary>
    /// 助理薪资类型
    /// </summary>
    public enum PerformanceType
    {
        /// <summary>
        /// 成交业绩
        /// </summary>
        Deal = 1,
        ///// <summary>
        ///// 退款业绩
        ///// </summary>
        //Refund = 2,
        /// <summary>
        /// 稽查业绩
        /// </summary>
        Check = 3
    }
    #endregion

    /// <summary>
    /// 订单性质
    /// </summary>
    public enum OrderNatureType
    {
        /// <summary>
        /// 正常下单=0
        /// </summary>
        RegularOrder,

        /// <summary>
        /// 朋友推荐=1
        /// </summary>
        FriendRecommended,

        /// <summary>
        /// 私域合作=2
        /// </summary>
        PrivateDomainCooperation,

        /// <summary>
        /// 财务添加=3
        /// </summary>
        FinancialInsert,

    }

    /// <summary>
    /// 初始化类型
    /// </summary>
    public enum InitializerType
    {
        /// <summary>
        /// 天猫
        /// </summary>
        Tmall,

        /// <summary>
        /// 京东
        /// </summary>
        JD,

        /// <summary>
        /// 积分=2
        /// </summary>
        Integration
    }


    public enum OrderFrom
    {
        /// <summary>
        /// 下单平台
        /// </summary>
        ThirdPartyOrder = 1,

        /// <summary>
        /// 内容平台
        /// </summary>
        ContentPlatFormOrder = 2,

        /// <summary>
        /// 客户升单
        /// </summary>
        BuyAgainOrder = 3
    }

    /// <summary>
    /// 直播需求优先级
    /// </summary>
    public enum LiveRequirementPriorityLevel
    {
        /// <summary>
        /// 一般=0
        /// </summary>
        Ordinary,


        /// <summary>
        /// 紧急
        /// </summary>
        Urgent
    }


    /// <summary>
    /// 直播需求状态
    /// </summary>
    public enum LiveRequirementStatus
    {
        /// <summary>
        /// 未响应=0
        /// </summary>
        UnResponse,

        /// <summary>
        /// 拒绝=1
        /// </summary>
        Refuse,

        /// <summary>
        /// 作废=2
        /// </summary>
        Cancel,

        /// <summary>
        /// 未处理=3
        /// </summary>
        UnTreated,

        /// <summary>
        /// 已处理待确认=4
        /// </summary>
        UnConfirm,

        /// <summary>
        /// 已处理=5
        /// </summary>
        Treated,
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 虚拟订单
        /// </summary>
        VirtualOrder,

        /// <summary>
        /// 实物订单
        /// </summary>
        MaterialOrder,
    }


    /// <summary>
    /// 派单留言方类型
    /// </summary>
    public enum SendOrderMessageBoardType
    {
        /// <summary>
        /// 啊美雅=0
        /// </summary>
        Amiya,

        /// <summary>
        /// 医院=1
        /// </summary>
        Hospital
    }

    /// <summary>
    /// 医院查看电话订单类型
    /// </summary>
    public enum CheckPhoneRecordOrderType
    {
        /// <summary>
        /// 正常交易订单
        /// </summary>
        TradeOrder,

        /// <summary>
        /// 内容平台订单
        /// </summary>
        ContentPlatformOrder
    }
    /// <summary>
    /// 小程序名称
    /// </summary>
    public enum MiniprogramName
    {
        /// <summary>
        /// 上合未来
        /// </summary>
        ShangHeWeiLai = 0,
        /// <summary>
        /// 啊美雅美容
        /// </summary>
        AmeiyaMR = 1,
        /// <summary>
        /// 刀刀气质美学
        /// </summary>
        DaoDaoQZMX = 2,
        /// <summary>
        /// 吉娜气质美学
        /// </summary>
        JiNaQZMX = 3
    }
    /// <summary>
    /// 小程序类型
    /// </summary>
    public enum MiniprogramType
    {
        /// <summary>
        /// 上合未来
        /// </summary>
        ShangHeWeiLai = 0,
        /// <summary>
        /// 啊美雅美容
        /// </summary>
        AmeiyaMR = 1,
        /// <summary>
        /// 刀刀气质美学
        /// </summary>
        DaoDaoQZMX = 2,
        /// <summary>
        /// 吉娜气质美学
        /// </summary>
        JiNaQZMX = 3

    }

    /// <summary>
    /// 小程序首页展示的标签
    /// </summary>
    public enum MiniprogramIndexTag
    {
        /// <summary>
        /// 美妆
        /// </summary>
        Cosmetics = 0,
        /// <summary>
        /// 护肤
        /// </summary>
        SkinCare = 1,
        /// <summary>
        /// 饰品
        /// </summary>
        Jewelry = 2,
        /// <summary>
        /// 生活
        /// </summary>
        Life
    }


    /// <summary>
    /// 追踪回访提报状态
    /// </summary>
    public enum SendStatus
    {
        /// <summary>
        /// 未提报
        /// </summary>
        UnSend = 0,
        /// <summary>
        /// 已提报
        /// </summary>
        Sended = 1,
        /// <summary>
        /// 跟进中
        /// </summary>
        FollowingUp = 2,
        /// <summary>
        /// 跟进完成
        /// </summary>
        FollowUpFinished = 3,
        /// <summary>
        /// 跟进失败
        /// </summary>
        FollowUpFailed = 4,
    }
    public enum InventoryStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// 盘平
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 盘盈
        /// </summary>
        Profit = 2,
        /// <summary>
        /// 盘亏
        /// </summary>
        Loss = 3,
    }
    public enum EmergencyLevel
    {
        //三级线索
        Ignorable = 0,

        //二级线索
        Generally = 2,
        //一级线索
        Important = 3,
        //非常重要
        // VeryImportant=4
    }
    public enum RechargeStatus
    {
        //待付款/余额退款初始化
        PendingPayment = 0,
        //成功(充值成功/余额退款审核通过)
        Success = 1,
        //取消(充值取消/余额退款未通过)
        Cacncel = 2,
        //失败(已付款,但账号余额异常修改余额失败或订单状态改变导致修改状态失败)
        Fail = 3
    }
    /// <summary>
    /// 预约状态
    /// </summary>
    public enum AppointmentStatusType
    {
        //预约处理
        Process = 0,
        //预约取消
        Cancel = 1,
        //预约成功
        Success = 2
    }
    /// <summary>
    /// 预约类型
    /// </summary>
    public enum AppointmentType
    {
        ///// <summary>
        ///// 其他
        ///// </summary>
        //Other = 0,
        /// <summary>
        /// 视频设计预约
        /// </summary>
        VideoAppointment = 1,
        /// <summary>
        /// 到院接诊预约
        /// </summary>
        ToHospitalAppointment = 2,
        ///// <summary>
        ///// 未知
        ///// </summary>
        //Unknow = 3
    }
    /// <summary>
    /// 成交情况消费类型
    /// </summary>
    public enum ConsumptionType
    {

        /// <summary>
        /// 定金消费
        /// </summary>
        Deposit = 0,
        /// <summary>
        /// 成交消费
        /// </summary>
        Deal = 1,
        /// <summary>
        /// 退款消费
        /// </summary>
        Refund = 2,
        /// <summary>
        /// 其他
        /// </summary>
        OTHER = 3,
        /// <summary>
        /// 欠款回收
        /// </summary>
        DebtCollection = 4,
    }

    /// <summary>
    /// 抖音客户来源
    /// </summary>
    public enum TiktokCustomerSource
    {
        /// <summary>
        /// 短视频
        /// </summary>
        ShortVideo = 0,
        /// <summary>
        /// 直播间
        /// </summary>
        LiveRoom = 1,
        /// <summary>
        /// 粉丝群
        /// </summary>
        FansGroup = 2,
        /// <summary>
        /// 私信
        /// </summary>
        PrivateMessage = 3,
        /// <summary>
        /// 智能AI
        /// </summary>
        AI = 4,
        /// <summary>
        /// 其他
        /// </summary>
        OTHER = 5,

        /// <summary>
        /// 产品转化
        /// </summary>
        ProductionTransform = 6,

        /// <summary>
        /// 抖音小风车
        /// </summary>
        TikTokShoppingCart = 7,
        /// <summary>
        /// 视频号福袋
        /// </summary>
        WechatVideoLuckyBag = 8,
        /// <summary>
        /// 直播间公屏
        /// </summary>
        LivingRoomScreen = 9,
        /// <summary>
        /// 老带新
        /// </summary>
        OldTakeNewCustomer = 10,
        /// <summary>
        /// 日不落直播
        /// </summary>
        RiBuLuoLiving = 11,
    }


    /// <summary>
    /// 小黄车客户类型
    /// </summary>
    public enum ShoppingCartRegistionCustomerSource
    {
        /// <summary>
        /// 其他
        /// </summary>
        Other = 0,
        /// <summary>
        /// 医美
        /// </summary>
        AestheticMedicine = 1,
        /// <summary>
        /// 带货
        /// </summary>
        TakeGoods = 2,
    }



    /// <summary>
    /// 带货商品类型
    /// </summary>
    public enum TakeGoodsType
    {
        /// <summary>
        /// 下单
        /// </summary>
        CreateOrder = 0,
        /// <summary>
        /// 退款
        /// </summary>
        ReturnBackOrder = 1,
    }
    /// <summary>
    /// 产品类型
    /// </summary>
    public enum ShoppingCartProductType
    {
        //其他
        Ohter = 0,
        //超声炮
        Peninsula = 1,
        //护肤品
        SkinCareProduct = 2,
        //彩妆
        MakeUpProduct = 3,
        //饰品&衣服
        AccessoryAndClothes = 4
    }
    /// <summary>
    /// 获客方式
    /// </summary>
    public enum ShoppingCartGetCustomerType
    {
        //其他
        Ohter = 0,
        //自主获客
        OneSelfGetCustomer = 1,
        //组内分诊
        GroupDistribute = 2
    }

    /// <summary>
    /// 面诊方式
    /// </summary>
    public enum ShoppingCartConsultationType
    {
        /// <summary>
        /// 视频
        /// </summary>
        Video = 1,
        /// <summary>
        /// 照片
        /// </summary>
        Picture = 2,
        /// <summary>
        /// 私信
        /// </summary>
        DirectMessages = 3,
        /// <summary>
        /// 其他
        /// </summary>
        Others = 4,
        /// <summary>
        /// 粉丝群
        /// </summary>
        FansGroup = 5,
        /// <summary>
        /// 短视频
        /// </summary>
        ShortVideo = 6,
        /// <summary>
        /// 产品转化
        /// </summary>
        ProductionTransform = 7,
    }

    public enum ShootingAndClipVideoType
    {
        /// <summary>
        /// 广告片
        /// </summary>
        AdvertisingFilm = 1,
        /// <summary>
        /// 口播
        /// </summary>
        MouthBroadcast = 2,

        /// <summary>
        /// vlong
        /// </summary>
        Vlong = 3,
        /// <summary>
        ///  企业宣传文化片
        /// </summary>
        EnterprisePropagandaCultureFilm = 4,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 0,
    }
    public enum CheckState
    {
        /// <summary>
        /// 待审核
        /// </summary>
        CheckPending = 0,
        /// <summary>
        /// 审核通过
        /// </summary>
        CheckSuccess = 1,
        /// <summary>
        /// 审核失败
        /// </summary>
        CheckFail = 2
    }
    public enum RefundState
    {
        /// <summary>
        /// 待退款
        /// </summary>
        RefundPending = 0,
        /// <summary>
        /// 退款成功
        /// </summary>
        RefundSuccess = 1,
        /// <summary>
        /// 退款失败
        /// </summary>
        RefundFail = 2,
        /// <summary>
        /// 退款中
        /// </summary>
        Refunding = 3
    }

    /// <summary>
    /// 标签分类
    /// </summary>
    public enum TagCategory
    {
        /// <summary>
        /// 用户标签
        /// </summary>
        UserTag = 0,
        /// <summary>
        /// 商品标签
        /// </summary>
        GoodsTag = 1,
        /// <summary>
        /// 面部标签
        /// </summary>
        FaceTag = 2
    }
    /// <summary>
    /// 美学设计报告状态
    /// </summary>
    public enum AestheticsDesignReportStatus
    {
        /// <summary>
        /// 已提交
        /// </summary>
        Commit = 0,
        /// <summary>
        /// 已设计
        /// </summary>
        Desgined
    }
    /// <summary>
    /// 美学设计报告图片方向
    /// </summary>
    public enum PictureDirectionType
    {
        /// <summary>
        /// 正面图片
        /// </summary>
        Front = 0,
        /// <summary>
        /// 侧面图片
        /// </summary>
        Side = 1,
        /// <summary>
        /// 图片
        /// </summary>
        Picture = 2,
    }


    /// <summary>
    /// 财务对账单状态
    /// </summary>
    public enum ReconciliationDocumentsStateEnum
    {
        /// <summary>
        /// 已提交
        /// </summary>
        Submited = 0,
        /// <summary>
        /// 待确认
        /// </summary>
        WaitingConfirmed = 1,
        /// <summary>
        /// 问题账单
        /// </summary>
        QuestionDocument = 2,
        /// <summary>
        /// 对账完成
        /// </summary>
        Successful = 3,
        /// <summary>
        /// 回款完成
        /// </summary>
        ReturnBackPriceSuccessful = 4,
    }


    /// <summary>
    /// 票据类型
    /// </summary>
    public enum BillTypeTextEnum
    {
        /// <summary>
        /// 医美票据
        /// </summary>
        BeautyClinic = 0,
        /// <summary>
        /// 其他票据
        /// </summary>
        Other = 1,
    }
    /// <summary>
    /// 票据回款状态
    /// </summary>
    public enum BillReturnBackStateTextEnum
    {
        /// <summary>
        /// 未回款
        /// </summary>
        UnReturnBack = 0,
        /// <summary>
        /// 回款中
        /// </summary>
        ReturnBacking = 1,
        /// <summary>
        /// 已回款
        /// </summary>
        ReturnBackSuccessful = 2,
    }

    public enum MessageNoticeMessageTextEnum
    {
        /// <summary>
        /// 订单通知
        /// </summary>
        OrderNotice = 1,
        /// <summary>
        /// 日程通知
        /// </summary>
        ScheduleNotice = 2,
        /// <summary>
        /// 操作通知
        /// </summary>
        OperationNotice = 3,
        /// <summary>
        /// 分诊通知
        /// </summary>
        DistributeInterviewNotice = 4,
        /// <summary>
        /// 系统消息
        /// </summary>
        BannerNotice = 5,
    }



    /// <summary>
    /// 申请类型
    /// </summary>
    public enum ContentPlatformOrderAddWorkType
    {
        ///// <summary>
        ///// 录单申请
        ///// </summary>
        AddOrderApply = 1,
        /// <summary>
        /// 改绑申请
        /// </summary>
        UpdateBindApply = 2,
    }
    #region{医院板块枚举类}

    /// <summary>
    /// 医院成交类型
    /// </summary>
    public enum HospitalDealType
    {
        ///// <summary>
        ///// 收费
        ///// </summary>
        Charge = 0,
        /// <summary>
        /// 退费
        /// </summary>
        Refund = 1,
    }


    /// <summary>
    /// 医院消费类型
    /// </summary>
    public enum HospitalConsumptionType
    {
        ///// <summary>
        ///// 交预交金
        ///// </summary>
        PayAdvance = 0,
        /// <summary>
        /// 办卡
        /// </summary>
        ApplyCard = 1,
        /// <summary>
        /// 项目收费
        /// </summary>
        ProjectCharge = 2,
        /// <summary>
        /// 划价单收费
        /// </summary>
        RateSheetCharge = 3,
        /// <summary>
        /// 卡类欠款回收
        /// </summary>
        CardArrearsRecovery = 4,
        /// <summary>
        /// 欠款回收
        /// </summary>
        CollectionOfArrears = 5,
    }


    /// <summary>
    /// 医院退款类型
    /// </summary>
    public enum HospitalRefundType
    {
        ///// <summary>
        ///// 退预交金
        ///// </summary>
        RefundAdvance = 0,
        /// <summary>
        /// 退卡
        /// </summary>
        RefundCard = 1,
        /// <summary>
        /// 退项目
        /// </summary>
        RefundProjectCharge = 2,
        /// <summary>
        /// 退划价单
        /// </summary>
        RefundRateSheetCharge = 3,
        /// <summary>
        /// 退多余欠款
        /// </summary>
        RefundExcessDebt = 4,
    }
    #endregion
    public enum HospitalBoardDataType
    {
        /// <summary>
        /// 累计数据
        /// </summary>
        Accumulate = 0,
        /// <summary>
        /// 当月数据
        /// </summary>
        ThisMonth = 1
    }
    #region RFM相关
    public enum RFM
    {
        /// <summary>
        /// 低
        /// </summary>
        Low = 0,
        /// <summary>
        /// 高
        /// </summary>
        High = 1,
        /// <summary>
        /// VIP
        /// </summary>
        VIP = 2
    }
    /// <summary>
    /// RFM标签等级
    /// </summary>
    public enum RFMTagLevel
    {
        /// <summary>
        /// 超级VIP
        /// </summary>
        RV = 0,
        /// <summary>
        /// 重要价值客户
        /// </summary>
        R1 = 1,
        /// <summary>
        /// 重要发展客户
        /// </summary>
        R2 = 2,
        /// <summary>
        /// 重要保持客户
        /// </summary>
        R3 = 3,
        /// <summary>
        /// 重要挽留客户
        /// </summary>
        R4 = 4,
        /// <summary>
        /// 一般价值客户
        /// </summary>
        R5 = 5,
        /// <summary>
        /// 一般发展客户
        /// </summary>
        R6 = 6,
        /// <summary>
        /// 一般保持客户
        /// </summary>
        R7 = 7,
        /// <summary>
        /// 一般挽留客户
        /// </summary>
        R8 = 8,

    }
    #endregion
    /// <summary>
    /// 礼品发放类型
    /// </summary>
    public enum GiftSendType
    {
        /// <summary>
        /// 客户自己领取
        /// </summary>
        CustomerRecieve = 0,
        /// <summary>
        /// 客服发送
        /// </summary>
        CustomerServiceSend = 1
    }
    /// <summary>
    /// 飞书多维表格类型
    /// </summary>
    public enum FeishuTableType
    {
        /// <summary>
        /// 短视频数据
        /// </summary>
        VideoData = 1,
        /// <summary>
        /// 评论数据
        /// </summary>
        Comments = 2,
        /// <summary>
        /// 粉丝数据
        /// </summary>
        FansData = 3
    }
    /// <summary>
    /// 未加V原因
    /// </summary>
    public enum UnAddWechatReason
    {
        /// <summary>
        /// 其他
        /// </summary>
        Other = 0,
        /// <summary>
        /// 号码未获取
        /// </summary>
        NoPhoneNUmber = 1,
        /// <summary>
        /// 未接/拒接电话
        /// </summary>
        RefuseCall = 2,
        /// <summary>
        /// 只要产品
        /// </summary>
        OnlyForGoods = 3,
        /// <summary>
        /// 不想咨询
        /// </summary>
        NoConsulation = 4,
        /// <summary>
        /// 加V未通过
        /// </summary>
        AddWechatFail = 5,
        /// <summary>
        /// 错拍链接
        /// </summary>
        ErrorPurche = 6,
        /// <summary>
        /// 非本人拍
        /// </summary>
        UnMySelf = 7
    }
    /// <summary>
    /// 小黄车客户归属渠道
    /// </summary>
    public enum BelongChannel
    {
        Other = 0,
        /// <summary>
        /// 直播前
        /// </summary>
        LiveBefore = 1,
        /// <summary>
        /// 直播中
        /// </summary>
        Living = 2,
        /// <summary>
        /// 直播后
        /// </summary>
        LiveAfter = 3
    }
    /// <summary>
    /// 直播中带货订单状态
    /// </summary>
    public enum LivingTakeGoodsOrderStatus
    {
        /// <summary>
        /// 已付款
        /// </summary>
        Payed = 3,
        /// <summary>
        /// 待发货
        /// </summary>
        WaitSendOrder = 4,
        /// <summary>
        /// 已发货
        /// </summary>
        SendOrder = 5,
        /// <summary>
        /// 已收货
        /// </summary>
        Delivery = 8,
        /// <summary>
        /// 退款
        /// </summary>
        Refund = 9,
        /// <summary>
        /// 订单完成(即已结算)
        /// </summary>
        Complete = 11

    }
}
