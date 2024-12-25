using Fx.Common.Utils;
using Fx.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fx.Amiya.Service
{
    public static class ServiceClass
    {


        /// <summary>
        /// 获取订单状态文本
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetOrderStatusText(string status)
        {
            string statusText = "";
            switch (status)
            {
                case "WAIT_BUYER_PAY":
                    statusText = "等待买家付款";
                    break;

                case "WAIT_SELLER_SEND_GOODS":
                    statusText = "等待卖家发货";

                    break;
                case "WAIT_BUYER_CONFIRM_GOODS":
                    statusText = "等待买家确认收货";
                    break;

                case "TRADE_BUYER_SIGNED":
                    statusText = "买家已签收";
                    break;
                case "TRADE_BUYER_PAID":
                    statusText = "买家已付款";
                    break;

                case "TRADE_FINISHED":
                    statusText = "交易成功";
                    break;

                case "TRADE_CLOSED":
                    statusText = "退款成功交易关闭";
                    break;

                case "TRADE_CLOSED_BY_TAOBAO":
                    statusText = "付款前交易关闭";
                    break;

                //-------
                case "TRADE_CANCELED":
                    statusText = "取消";
                    break;

                case "LOCKED":
                    statusText = "已锁定";
                    break;

                case "POP_ORDER_PAUSE POP":
                    statusText = "暂停";
                    break;

                case "REFUNDING":
                    statusText = "退款中";
                    break;
                case "SEEK_ADVICE":
                    statusText = "咨询订单";
                    break;

                case "BARGAIN_MONEY":
                    statusText = "定金订单";
                    break;
                case "CHECK_FAIL":
                    statusText = "审核未通过";
                    break;
                case "PARTIAL_REFUND":
                    statusText = "部分退款";
                    break;
            }
            return statusText;
        }

        /// <summary>
        /// 获取抖店订单状态文本
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetTikTokOrderStatusText(string status)
        {
            string statusText = "";
            switch (status)
            {
                case "PENDING_REFUND":
                    statusText = "待退款";
                    break;
                case "REFUNDING":
                    statusText = "退款中";
                    break;
                case "TRADE_CLOSED_AFTER_REFUND":
                    statusText = "退款成功交易关闭";
                    break;
                case "REFUND_FAIL":
                    statusText = "退款失败";
                    break;
                case "WAIT_BUYER_PAY":
                    statusText = "等待买家付款";
                    break;

                case "WAIT_SELLER_SEND_GOODS":
                    statusText = "等待卖家发货";
                    break;
                case "WAIT_BUYER_CONFIRM_GOODS":
                    statusText = "等待买家收货";
                    break;

                case "TRADE_FINISHED":
                    statusText = "交易成功";
                    break;

                case "TRADE_CLOSED":
                    statusText = "订单付款前取消";
                    break;
                case "UNKNOW":
                    statusText = "未知状态";
                    break;
            }
            return statusText;
        }

        #region 获取小程序模块订单状态文本展示

        /// <summary>
        /// 获取订单状态文本（虚拟订单）
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetMiniOrderStatusText(string status)
        {
            string statusText = "";
            switch (status)
            {
                case "WAIT_BUYER_PAY":
                    statusText = "待付款";
                    break;

                case "WAIT_SELLER_SEND_GOODS":
                    statusText = "待核销";

                    break;
                case "WAIT_BUYER_CONFIRM_GOODS":
                    statusText = "待核销";
                    break;

                case "TRADE_BUYER_SIGNED":
                    statusText = "待核销";
                    break;

                case "TRADE_FINISHED":
                    statusText = "已完成";
                    break;

                case "TRADE_CLOSED":
                    statusText = "已关闭";
                    break;

                case "TRADE_CLOSED_BY_TAOBAO":
                    statusText = "已关闭";
                    break;

                //-------
                case "TRADE_CANCELED":
                    statusText = "已取消";
                    break;

                case "LOCKED":
                    statusText = "已锁定";
                    break;

                case "POP_ORDER_PAUSE POP":
                    statusText = "暂停";
                    break;

                case "REFUNDING":
                    statusText = "退款中";
                    break;

                case "REFUND_FAIL":
                    statusText = "已关闭";
                    break;
                case "SEEK_ADVICE":
                    statusText = "咨询订单";
                    break;

                case "BARGAIN_MONEY":
                    statusText = "定金订单";
                    break;
            }
            return statusText;
        }

        /// <summary>
        /// 获取订单状态文本（实物订单）
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetMiniGoodsOrderStatusText(string status)
        {
            string statusText = "";
            switch (status)
            {
                case "WAIT_BUYER_PAY":
                    statusText = "未兑换";
                    break;

                case "WAIT_SELLER_SEND_GOODS":
                    statusText = "兑换成功";

                    break;
                case "WAIT_BUYER_CONFIRM_GOODS":
                    statusText = "兑换成功";
                    break;

                case "TRADE_BUYER_SIGNED":
                    statusText = "兑换成功";
                    break;

                case "TRADE_FINISHED":
                    statusText = "兑换成功";
                    break;

                case "TRADE_CLOSED":
                    statusText = "已关闭";
                    break;

                case "TRADE_CLOSED_BY_TAOBAO":
                    statusText = "已关闭";
                    break;

                //-------
                case "TRADE_CANCELED":
                    statusText = "已取消";
                    break;

                case "LOCKED":
                    statusText = "已锁定";
                    break;

                case "POP_ORDER_PAUSE POP":
                    statusText = "暂停";
                    break;

                case "REFUNDING":
                    statusText = "退款中";
                    break;

                case "REFUND_FAIL":
                    statusText = "已关闭";
                    break;
            }
            return statusText;
        }
        #endregion

        public static string GetAppTypeText(byte appType)
        {
            string typeText = "";
            switch (appType)
            {
                case 0:
                    typeText = "天猫";
                    break;

                case 1:
                    typeText = "京东";
                    break;

                case 2:
                    typeText = "小程序";
                    break;

                case 3:
                    typeText = "公众号";
                    break;
                case 4:
                    typeText = "抖音";
                    break;
                case 5:
                    typeText = "啊美雅企业微信";
                    break;
                case 6:
                    typeText = "医院端企业微信";
                    break;
                case 7:
                    typeText = "视频号";
                    break;
                case 8:
                    typeText = "抖音本地生活";
                    break;
                case 9:
                    typeText = "官网下单";
                    break;
            }
            return typeText;
        }

        #region 内容平台订单相关枚举
        public static string GetContentPlateFormOrderTypeText(byte appType)
        {
            string typeText = "";
            switch (appType)
            {
                case 1:
                    typeText = "咨询订单";
                    break;

                case 2:
                    typeText = "定金订单";
                    break;
                case 3:
                    typeText = "预约订单";
                    break;

            }
            return typeText;
        }

        public static string GetContentPlateFormOrderConsultationTypeText(int ConsultationType)
        {
            string typeText = "";
            switch (ConsultationType)
            {

                case 1:
                    typeText = "(助理)照片面诊";
                    break;
                case 2:
                    typeText = "(主播)视频面诊";
                    break;
                case 3:
                    typeText = "(主播)语音面诊";
                    break;
                //case 4:
                //    typeText = "未面诊";
                    break;
                case 0:
                    typeText = "其他";
                    break;

            }
            return typeText;
        }
        public static string GetContentPlateFormOrderStatusText(byte appType)
        {
            string typeText = "";
            switch (appType)
            {
                case 1:
                    typeText = "未派单";
                    break;

                case 2:
                    typeText = "已派单";
                    break;

                case 3:
                    typeText = "已接单";
                    break;

                case 4:
                    typeText = "已成交";
                    break;

                case 5:
                    typeText = "重单-不可深度";
                    break;

                case 6:
                    typeText = "未成交";
                    break;
                case 7:
                    typeText = "重单-可深度";
                    break;
            }
            return typeText;
        }

        public static string GetContentPlateFormOrderDealPerformanceType(int performanceType)
        {
            string typeText = "";
            switch (performanceType)
            {
                case 0:
                    typeText = "其他";
                    break;
                case 1:
                    typeText = "助理激活";
                    break;

                case 2:
                    typeText = "VIP管家激活";
                    break;
                case 3:
                    typeText = "机构报单";
                    break;
                case 4:
                    typeText = "助理稽查";
                    break;
                case 5:
                    typeText = "财务稽查";
                    break;
                case 6:
                    typeText = "VIP管家稽查";
                    break;
                case 7:
                    typeText = "机构报单-API（勿选）";
                    break;
                case 8:
                    typeText = "助理补单";
                    break;
            }
            return typeText;
        }

        #endregion

        /// <summary>
        /// 获取订单类型文本
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public static string GetOrderTypeText(long? orderType)
        {
            string orderTypeText = "";
            switch (orderType)
            {
                case 0:
                    orderTypeText = "虚拟订单";
                    break;

                case 1:
                    orderTypeText = "实物订单";
                    break;
            }
            return orderTypeText;
        }

        /// <summary>
        /// 视频号订单状态转换
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetWechatVideoOrderStatusText(int status)
        {
            string statusText = "";
            switch (status)
            {
                case 10:
                    statusText = "WAIT_BUYER_PAY";
                    break;

                case 20:
                    statusText = "WAIT_SELLER_SEND_GOODS";
                    break;
                case 21:
                    statusText = "WAIT_SELLER_SEND_GOODS";
                    break;
                case 30:
                    statusText = "WAIT_BUYER_CONFIRM_GOODS";
                    break;
                case 100:
                    statusText = "TRADE_FINISHED";
                    break;
                case 200:
                    statusText = "TRADE_CLOSED";
                    break;
                case 250:
                    statusText = "TRADE_CLOSED_BY_TAOBAO";
                    break;
            }
            return statusText;
        }
        /// <summary>
        /// 获取视频号订单类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWechatOrderTypeText(long type)
        {
            string orderType = "";
            switch (type)
            {
                case 0L:
                    orderType = "实物订单";
                    break;
                case 1L:
                    orderType = "虚拟订单";
                    break;
                default:
                    orderType = "未知类型";
                    break;
            }
            return orderType;
        }
        /// <summary>
        /// 获取抖点订单类型文本
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public static string GetTikTokOrderTypeText(byte orderType)
        {
            string orderTypeText = "";
            switch (orderType)
            {
                case 0:
                    orderTypeText = "实物订单";
                    break;

                case 2:
                    orderTypeText = "虚拟订单";
                    break;
                case 4:
                    orderTypeText = "电子券poi核销";
                    break;
                case 5:
                    orderTypeText = "三方核销";
                    break;
            }
            return orderTypeText;
        }
        /// <summary>
        /// 获取重要程度文本,0可忽略，1轻微，2一般，3重要，4非常重要
        /// </summary>
        /// <param name="emergencyLevel"></param>
        /// <returns></returns>
        public static string GetShopCartRegisterEmergencyLevelText(int emergencyLevel)
        {
            string emergencyLevelText = "";
            switch (emergencyLevel)
            {
                case 0:
                    emergencyLevelText = "三级线索";
                    break;
                /*case 1:
                    emergencyLevelText = "轻微";
                    break;*/
                case 2:
                    emergencyLevelText = "二级线索";
                    break;
                case 3:
                    emergencyLevelText = "一级线索";
                    break;
                /*case 4:
                    emergencyLevelText = "非常重要";
                    break;*/
                default:
                    emergencyLevelText = "三级线索";
                    break;

            }
            return emergencyLevelText;
        }

        /// <summary>
        /// 获取订单性质文本
        /// </summary>
        /// <param name="orderNature"></param>
        /// <returns></returns>
        public static string GetOrderNatureText(byte orderNature)
        {
            string orderTypeText = "";
            switch (orderNature)
            {
                case 0:
                    orderTypeText = "正常下单";
                    break;

                case 1:
                    orderTypeText = "朋友推荐";
                    break;
                case 2:
                    orderTypeText = "私域合作";
                    break;
                case 3:
                    orderTypeText = "财务添加";
                    break;
            }
            return orderTypeText;
        }
        /// <summary>
        /// 获取小程序名称
        /// </summary>
        public static string GetMiniprogramNameText(int miniprogram)
        {
            string miniprogramName = "";
            switch (miniprogram)
            {
                case 0:
                    miniprogramName = "上合未来";
                    break;
                case 1:
                    miniprogramName = "啊美雅美容";
                    break;
                case 2:
                    miniprogramName = "刀刀气质美学";
                    break;
                case 3:
                    miniprogramName = "吉娜气质美学";
                    break;
                default:
                    miniprogramName = "其他";
                    break;
            }
            return miniprogramName;
        }
        /// <summary>
        /// 获取小程序appId
        /// </summary>
        public static string GetMiniprogramAppId(int miniprogram)
        {
            string miniprogramAppId = "";
            switch (miniprogram)
            {
                case 0:
                    miniprogramAppId = "wx695942e4818de445";
                    break;
                case 1:
                    miniprogramAppId = "wx8747b7f34c0047eb";
                    break;
                case 2:
                    miniprogramAppId = "ddjz";
                    break;
                case 3:
                    miniprogramAppId = "jnqz";
                    break;
                default:
                    miniprogramAppId = "";
                    break;
            }
            return miniprogramAppId;
        }
        /// <summary>
        /// 根据appid获取小程序名称
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static string GetMiniprogramNameByAppId(string appid)
        {
            string miniprogramName = "";
            switch (appid)
            {
                case "wx695942e4818de445":
                    miniprogramName = "上合未来";
                    break;
                case "wx8747b7f34c0047eb":
                    miniprogramName = "啊美雅美容";
                    break;
                case "ddqz":
                    miniprogramName = "刀刀气质美学";
                    break;
                case "jnqz":
                    miniprogramName = "吉娜气质美学";
                    break;
                default:
                    miniprogramName = "共享商品";
                    break;
            }
            return miniprogramName;
        }
        /// <summary>
        /// 根据appid获取小程序类型
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static int? GetMiniprogramTypeByAppId(string appid)
        {
            int? miniprogramType = null;
            switch (appid)
            {
                case "wx695942e4818de445":
                    miniprogramType = 0;
                    break;
                case "wx8747b7f34c0047eb":
                    miniprogramType = 1;
                    break;
                case "ddqz":
                    miniprogramType = 2;
                    break;
                case "jnqz":
                    miniprogramType = 3;
                    break;
                default:
                    miniprogramType = null;
                    break;
            }
            return miniprogramType;
        }
        /// <summary>
        /// 根据appid获取绑定的主播
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static int? GetLiveAnchorIdByAppId(string appid)
        {
            int? liveAnchorId = null;
            switch (appid)
            {
                case "ddqz":
                    liveAnchorId = 1;
                    break;
                case "jnqz":
                    liveAnchorId = 16;
                    break;
                default:
                    liveAnchorId = null;
                    break;
            }
            return liveAnchorId;
        }
        /// <summary>
        /// 获取标签id
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string GetTagId(int tag)
        {
            string tagId = "";
            switch (tag)
            {
                case 0:
                    tagId = "mzid";
                    break;
                case 1:
                    tagId = "hfid";
                    break;
                case 2:
                    tagId = "8dd5b4e7-ab20-42c9-9371-18349cd047df";
                    break;
                case 3:
                    tagId = "shid";
                    break;
            }
            return tagId;
        }

        /// <summary>
        /// 获取成交情况消费类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetConsumptionTypeText(int? type)
        {
            string consumptionType = "";
            switch (type)
            {
                case 0:
                    consumptionType = "定金消费";
                    break;
                case 1:
                    consumptionType = "成交消费";
                    break;
                case 2:
                    consumptionType = "退款消费";
                    break;
                case 3:
                    consumptionType = "其他消费";
                    break;
                case 4:
                    consumptionType = "欠款回收";
                    break;
                default:
                    consumptionType = "未知";
                    break;
            }
            return consumptionType;

        }

        /// <summary>
        /// 获取抖音新增客户来源类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTiktokCustomerSourceText(int? type)
        {
            string sourceText = "";
            switch (type)
            {
                case 0:
                    sourceText = "短视频";
                    break;
                case 1:
                    sourceText = "直播间";
                    break;
                case 2:
                    sourceText = "粉丝群";
                    break;
                case 3:
                    sourceText = "私信";
                    break;
                case 4:
                    sourceText = "智能AI";
                    break;
                case 5:
                    sourceText = "其他";
                    break;
                case 6:
                    sourceText = "产品转化";
                    break;
                case 7:
                    sourceText = "小风车";
                    break;
                case 8:
                    sourceText = "福袋";
                    break;
                case 9:
                    sourceText = "直播间公屏";
                    break;
                case 10:
                    sourceText = "老带新";
                    break;
                case 11:
                    sourceText = "日不落直播";
                    break;
                default:
                    sourceText = "";
                    break;
            }
            return sourceText;

        }


        /// <summary>
        /// 获取小黄车带货产品类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetShoppingCartTakeGoodsProductTypeText(int? type)
        {
            string sourceText = "";
            switch (type)
            {
                case 0:
                    sourceText = "其他";
                    break;
                case 1:
                    sourceText = "超声炮";
                    break;
                case 2:
                    sourceText = "护肤品";
                    break;
                case 3:
                    sourceText = "彩妆";
                    break;
                case 4:
                    sourceText = "饰品&衣服";
                    break;
                default:
                    sourceText = "";
                    break;
            }
            return sourceText;

        }

        /// <summary>
        /// 获取带货商品类型
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerTakeGoodsTypeText(int takeGoodsType)
        {
            string takeGoodsTypeText = "";
            switch (takeGoodsType)
            {
                case 0:
                    takeGoodsTypeText = "下单";
                    break;

                case 1:
                    takeGoodsTypeText = "退款";
                    break;

            }
            return takeGoodsTypeText;
        }


        /// <summary>
        /// 获取小黄车获客方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetShoppingCartGetCustomerTypeText(int type)
        {
            string sourceText = "";
            switch (type)
            {
                case 0:
                    sourceText = "其他";
                    break;
                case 1:
                    sourceText = "自主获客";
                    break;
                case 2:
                    sourceText = "组内分诊";
                    break;
            }
            return sourceText;

        }

        /// <summary>
        /// 获取小黄车顾客类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetShoppingCartCustomerTypeText(int type)
        {
            string sourceText = "";
            switch (type)
            {
                case 0:
                    sourceText = "其他";
                    break;
                case 1:
                    sourceText = "医美顾客";
                    break;
                case 2:
                    sourceText = "带货顾客";
                    break;
            }
            return sourceText;

        }


        public static string GetOrderFromText(int orderFrom)
        {
            string orderTypeText = "";
            switch (orderFrom)
            {
                case 0:
                    orderTypeText = "未知";
                    break;

                case 1:
                    orderTypeText = "下单平台";
                    break;
                case 2:
                    orderTypeText = "内容平台";
                    break;
                case 3:
                    orderTypeText = "消费追踪";
                    break;
            }
            return orderTypeText;
        }


        /// <summary>
        /// 获取交易类型文本
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public static string GetExchangeTypeText(byte exchangeType)
        {
            string orderTypeText = "";
            switch (exchangeType)
            {
                case 0:
                    orderTypeText = "积分支付";
                    break;
                case 1:
                    orderTypeText = "三方支付";
                    break;
                case 2:
                    orderTypeText = "线下支付";
                    break;
                case 3:
                    orderTypeText = "余额支付";
                    break;
                case 4:
                    orderTypeText = "微信支付";
                    break;
                case 5:
                    orderTypeText = "支付宝支付";
                    break;
                case 6:
                    orderTypeText = "慧收钱支付";
                    break;
                case 7:
                    orderTypeText = "积分加钱支付";
                    break;
                case 8:
                    orderTypeText = "杉德支付";
                    break;
                default:
                    orderTypeText = "未知";
                    break;
            }
            return orderTypeText;
        }




        /// <summary>
        /// 获取消费追踪类型文本
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerConsumeTypeText(byte consumeType)
        {
            string consumeTypeText = "";
            switch (consumeType)
            {
                case 0:
                    consumeTypeText = "当天其他消费";
                    break;

                case 1:
                    consumeTypeText = "再消费";
                    break;
            }
            return consumeTypeText;
        }

        /// <summary>
        /// 获取升单渠道文本
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerConsumeChannelText(int channel)
        {
            string channelTypeText = "";
            switch (channel)
            {
                case 0:
                    channelTypeText = "医院";
                    break;

                case 1:
                    channelTypeText = "天猫";
                    break;

                case 2:
                    channelTypeText = "抖音";
                    break;

                case 3:
                    channelTypeText = "抖音代运营";
                    break;
            }
            return channelTypeText;
        }

        /// <summary>
        /// 获取内容平台到院类型文本
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerContentPlatFormOrderToHospitalTypeText(int toHospitalType)
        {
            string toHospitalTypeText = "";
            switch (toHospitalType)
            {
                case 1:
                    toHospitalTypeText = "初诊";
                    break;

                case 2:
                    toHospitalTypeText = "复诊";
                    break;

                case 3:
                    toHospitalTypeText = "再消费";

                    break;
                case 4:
                    toHospitalTypeText = "退款";
                    break;

                case 0:
                    toHospitalTypeText = "其他";
                    break;

            }
            return toHospitalTypeText;
        }

        /// <summary>
        /// 获取内容平台订单来源
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerContentPlatFormOrderSourceText(int channel)
        {
            string channelTypeText = "";
            switch (channel)
            {
                case 1:
                    channelTypeText = "面诊卡";
                    break;

                case 2:
                    channelTypeText = "非面诊卡";
                    break;
                case 3:
                    channelTypeText = "美肤卡";
                    break;
                case 4:
                    channelTypeText = "直播间";
                    break;
                case 5:
                    channelTypeText = "短视频";
                    break;
                case 6:
                    channelTypeText = "私信";
                    break;
            }
            return channelTypeText;
        }

        /// <summary>
        /// 获取升单类型
        /// </summary>
        /// <param name="BuyAgainType"></param>
        /// <returns></returns>
        public static string GetBuyAgainTypeText(int BuyAgainType)
        {
            string BuyAgainTypeText = "";
            switch (BuyAgainType)
            {
                case 0:
                    BuyAgainTypeText = "升单5%";
                    break;

                case 1:
                    BuyAgainTypeText = "升单30%";
                    break;
                case 2:
                    BuyAgainTypeText = "升单35%";
                    break;
                case 3:
                    BuyAgainTypeText = "升单10%";
                    break;
                case 4:
                    BuyAgainTypeText = "升单15%";
                    break;
                case 5:
                    BuyAgainTypeText = "升单20%";
                    break;
                case 6:
                    BuyAgainTypeText = "升单50%";
                    break;
            }
            return BuyAgainTypeText;
        }
        /// <summary>
        /// 获取获得积分类型
        /// </summary>
        /// <param name="IntegrationType"></param>
        /// <returns></returns>
        public static string GetIntegrationTypeText(int IntegrationType)
        {
            string BuyAgainTypeText = "";
            switch (IntegrationType)
            {
                case 0:
                    BuyAgainTypeText = "消费产生积分";
                    break;

                case 1:
                    BuyAgainTypeText = "赠送积分";
                    break;
                case 2:
                    BuyAgainTypeText = "活动积分";
                    break;
                case 3:
                    BuyAgainTypeText = "期初积分";
                    break;
                case 4:
                    BuyAgainTypeText = "退费归还积分";
                    break;
                case 5:
                    BuyAgainTypeText = "退还礼品积分";
                    break;
                case 6:
                    BuyAgainTypeText = "新用户赠送";
                    break;
                case 7:
                    BuyAgainTypeText = "积分修正";
                    break;
            }
            return BuyAgainTypeText;
        }
        /// <summary>
        /// 获取使用积分类型
        /// </summary>
        /// <param name="IntegrationType"></param>
        /// <returns></returns>
        public static string GetUseIntegrationTypeText(int IntegrationType)
        {
            string BuyAgainTypeText = "";
            switch (IntegrationType)
            {
                case 0:
                    BuyAgainTypeText = "消费抵用";
                    break;

                case 1:
                    BuyAgainTypeText = "归还信用积分";
                    break;
                case 2:
                    BuyAgainTypeText = "积分过期";
                    break;
                case 3:
                    BuyAgainTypeText = "退费";
                    break;
                case 4:
                    BuyAgainTypeText = "转介绍人退费";
                    break;
                case 5:
                    BuyAgainTypeText = "兑换礼品";
                    break;
                case 6:
                    BuyAgainTypeText = "客服修正积分";
                    break;
            }
            return BuyAgainTypeText;
        }
        /// <summary>
        /// 获取审核情况
        /// </summary>
        /// <param name="BuyAgainType"></param>
        /// <returns></returns>
        public static string GetCheckTypeText(int CheckType)
        {
            string CheckTypeText = "";
            switch (CheckType)
            {
                case 0:
                    CheckTypeText = "未审核";
                    break;

                case 1:
                    CheckTypeText = "审核不通过";
                    break;
                case 2:
                    CheckTypeText = "审核通过";
                    break;
                case 3:
                    CheckTypeText = "审核中";
                    break;
            }
            return CheckTypeText;
        }

        /// <summary>
        /// 获取对账单审核记录生成薪资时的审核类型
        /// </summary>
        /// <param name="BuyAgainType"></param>
        /// <returns></returns>
        public static string GetReconciliationDocumentSettleCheckType(int CheckType)
        {
            string CheckTypeText = "";
            switch (CheckType)
            {
                case 0:
                    CheckTypeText = "其他";
                    break;
                case 1:
                    CheckTypeText = "自播达人审核";
                    break;
                case 2:
                    CheckTypeText = "供应链达人审核";
                    break;
                case 3:
                    CheckTypeText = "三方订单升单审核";
                    break;
            }
            return CheckTypeText;
        }

        /// <summary>
        /// 获取接口访问类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetRequestTypeText(int type)
        {
            string requestTypeText = "";
            switch (type)
            {
                case 0:
                    requestTypeText = "数据添加";
                    break;
                case 1:
                    requestTypeText = "数据删除";
                    break;
                case 2:
                    requestTypeText = "数据更改";
                    break;
                case 3:
                    requestTypeText = "数据查询";
                    break;
                case 4:
                    requestTypeText = "数据导出";
                    break;
                case 5:
                    requestTypeText = "支付回调";
                    break;
                case 6:
                    requestTypeText = "用户登陆";
                    break;
                default:
                    requestTypeText = "其他";
                    break;
            }
            return requestTypeText;
        }
        /// <summary>
        /// 操作来源
        /// </summary>
        /// <param name="requestSource"></param>
        /// <returns></returns>
        public static string GetRequestSourceText(int requestSource)
        {
            string requestSourceText = "";
            switch (requestSource)
            {
                case 0:
                    requestSourceText = "啊美雅预约系统";
                    break;
                case 1:
                    requestSourceText = "啊美雅企业微信";
                    break;
                default:
                    requestSourceText = "其他";
                    break;
            }
            return requestSourceText;

        }

        /// <summary>
        /// 获取派单顺序
        /// </summary>
        /// <returns></returns>
        public static string GetSendOrderText(int sendOrder)
        {
            string sendOrderText = "";
            switch (sendOrder)
            {
                case 0:
                    sendOrderText = "首派";
                    break;
                case 1:
                    sendOrderText = "次派";
                    break;
                case 2:
                    sendOrderText = "查重单";
                    break;
                case 3:
                    sendOrderText = "首派(微整)";
                    break;
                case 4:
                    sendOrderText = "首派(整外)";
                    break;
                case 5:
                    sendOrderText = "首派(新合作整外)";
                    break;
                case 6:
                    sendOrderText = "次派(微整)";
                    break;
                default:
                    sendOrderText = "";
                    break;
            }
            return sendOrderText;
        }

        /// <summary>
        /// 获取年费,保证金缴纳状态
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetYearServiceFeeOrSecurityDepositText(int type)
        {
            string text = "";
            switch (type)
            {
                case 0:
                    text = "已缴纳";
                    break;
                case 1:
                    text = "未缴纳";
                    break;
                case 2:
                    text = "未缴纳(走审批)";
                    break;
                default:
                    text = "";
                    break;
            }
            return text;
        }

        /// <summary>
        /// 获取退款订审核情况
        /// </summary>
        /// <param name="CheckType"></param>
        /// <returns></returns>
        public static string GetOrderRefundCheckTypeText(int CheckType)
        {
            string CheckTypeText = "";
            switch (CheckType)
            {
                case 0:
                    CheckTypeText = "待审核";
                    break;

                case 1:
                    CheckTypeText = "审核通过";
                    break;
                case 2:
                    CheckTypeText = "审核未通过";
                    break;
            }
            return CheckTypeText;
        }


        /// <summary>
        /// 获取退款订单退款情况
        /// </summary>
        /// <param name="CheckType"></param>
        /// <returns></returns>
        public static string GetRefundStateText(int CheckType)
        {
            string CheckTypeText = "";
            switch (CheckType)
            {
                case 0:
                    CheckTypeText = "待退款";
                    break;

                case 1:
                    CheckTypeText = "退款成功";
                    break;
                case 2:
                    CheckTypeText = "退款失败";
                    break;
                case 3:
                    CheckTypeText = "退款中";
                    break;
            }
            return CheckTypeText;
        }

        /// <summary>
        /// 获取提交情况
        /// </summary>
        /// <param name="BuyAgainType"></param>
        /// <returns></returns>
        public static string GetSubmitTypeText(int CheckType)
        {
            string CheckTypeText = "";
            switch (CheckType)
            {
                case 0:
                    CheckTypeText = "未提交";
                    break;

                case 1:
                    CheckTypeText = "已提交";
                    break;
            }
            return CheckTypeText;
        }

        /// <summary>
        /// 获取提报涨停
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerSendStatusText(int sendStatus)
        {
            string channelTypeText = "";
            switch (sendStatus)
            {
                case 0:
                    channelTypeText = "未提报";
                    break;

                case 1:
                    channelTypeText = "已提报";
                    break;

                case 2:
                    channelTypeText = "跟进中";
                    break;

                case 3:
                    channelTypeText = "跟进完成";
                    break;

                case 4:
                    channelTypeText = "跟进失败";
                    break;
            }
            return channelTypeText;
        }

        /// <summary>
        /// 获取抵用券类型
        /// </summary>
        /// <param name="orderFrom"></param>
        /// <returns></returns>
        public static string GetConsumptionVoucherType(int consumptionVoucherType)
        {
            string consumptionVoucherTypeText = "";
            switch (consumptionVoucherType)
            {
                case 0:
                    consumptionVoucherTypeText = "现金抵用券";
                    break;

                case 1:
                    consumptionVoucherTypeText = "虚拟商品抵用券";
                    break;

                case 2:
                    consumptionVoucherTypeText = "积分抵用券";
                    break;
                case 3:
                    consumptionVoucherTypeText = "预约叫车抵用券";
                    break;
                case 4:
                    consumptionVoucherTypeText = "折扣抵用券";
                    break;
            }
            return consumptionVoucherTypeText;
        }

        /// <summary>
        /// 获取美学设计报告状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetAestheticsDesignReportStatus(int status)
        {
            string statusText = "";
            switch (status)
            {
                case 0:
                    statusText = "已提交";
                    break;
                case 1:
                    statusText = "已设计";
                    break;
            }
            return statusText;
        }

        /// <summary>
        /// 获取美学设计图片方向
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string PictureDirectionTypeText(int type)
        {
            string typeText = "";
            switch (type)
            {
                case 0:
                    typeText = "正面照片";
                    break;
                case 1:
                    typeText = "侧面照片";
                    break;
                case 2:
                    typeText = "图片";
                    break;
            }
            return typeText;
        }

        /// <summary>
        /// 获取抵用券类型
        /// </summary>
        /// <param name="orderFrom"></param>
        /// <returns></returns>
        public static string GetTagCategoryType(int consumptionVoucherType)
        {
            string tagCategory = "";
            switch (consumptionVoucherType)
            {
                case 0:
                    tagCategory = "用户标签";
                    break;

                case 1:
                    tagCategory = "商品标签";
                    break;
                case 2:
                    tagCategory = "面部标签";
                    break;
            }
            return tagCategory;
        }

        /// <summary>
        /// 获取预约状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetAppointmentStatusTypeText(int status)
        {
            var statusText = "";
            switch (status)
            {
                case 0:
                    statusText = "预约处理";
                    break;
                case 1:
                    statusText = "预约取消";
                    break;
                case 2:
                    statusText = "预约成功";
                    break;
                default:
                    statusText = "未知";
                    break;
            }
            return statusText;
        }


        /// <summary>
        /// 获取预约类型
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetAppointmentTypeText(int type)
        {
            var statusText = "";
            switch (type)
            {
                //case 0:
                //    statusText = "其他";
                //    break;
                case 1:
                    statusText = "视频设计预约";
                    break;
                case 2:
                    statusText = "到院接诊预约";
                    break;
                default:
                    statusText = "未知";
                    break;
            }
            return statusText;
        }

        /// <summary>
        /// 获取医院成交类型
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetHospitalDealTypeText(int type)
        {
            var statusText = "";
            switch (type)
            {
                case 0:
                    statusText = "收费";
                    break;
                case 1:
                    statusText = "退费";
                    break;
                default:
                    statusText = "未知";
                    break;
            }
            return statusText;
        }

        /// <summary>
        /// 获取医院消费类型
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetHospitalConsumptionTypeText(int consumptionType)
        {
            var statusText = "";
            switch (consumptionType)
            {
                case 0:
                    statusText = "交预交金";
                    break;
                case 1:
                    statusText = "办卡";
                    break;
                case 2:
                    statusText = "项目收费";
                    break;
                case 3:
                    statusText = "划价单收费";
                    break;
                case 4:
                    statusText = "卡类欠款回收";
                    break;
                case 5:
                    statusText = "欠款回收";
                    break;
                default:
                    statusText = "未知";
                    break;
            }
            return statusText;
        }

        /// <summary>
        /// 获取医院退款类型类型
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetHospitalRefundTypeText(int refundType)
        {
            var statusText = "";
            switch (refundType)
            {
                case 0:
                    statusText = "退预交金";
                    break;
                case 1:
                    statusText = "退卡";
                    break;
                case 2:
                    statusText = "退项目";
                    break;
                case 3:
                    statusText = "退划价单";
                    break;
                case 4:
                    statusText = "退多余欠款";
                    break;
                default:
                    statusText = "未知";
                    break;
            }
            return statusText;
        }
        /// <summary>
        /// 获取抵用券类型
        /// </summary>
        /// <param name="orderFrom"></param>
        /// <returns></returns>
        public static string GetAppointCArStatusType(int consumptionVoucherType)
        {
            string appointCArStatusType = "";
            switch (consumptionVoucherType)
            {
                case 0:
                    appointCArStatusType = "已提交";
                    break;

                case 1:
                    appointCArStatusType = "已处理";
                    break;

                case 2:
                    appointCArStatusType = "已取消";
                    break;
                case 3:
                    appointCArStatusType = "已完成";
                    break;
            }
            return appointCArStatusType;
        }

        /// <summary>
        /// 获取盘库状态
        /// </summary>
        /// <param name="orderFrom"></param>
        /// <returns></returns>
        public static string GetInventoryStateText(int inventoryState)
        {
            string inventoryStateText = "";
            switch (inventoryState)
            {
                case 1:
                    inventoryStateText = "盘平";
                    break;

                case 2:
                    inventoryStateText = "盘盈";
                    break;

                case 3:
                    inventoryStateText = "盘亏";
                    break;
            }
            return inventoryStateText;
        }
        /// <summary>
        /// 预约车型
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        public static string GetAppointmentCarTypeText(int carType)
        {
            string carTypeText = "";
            switch (carType)
            {
                case 0:
                    carTypeText = "经济型";
                    break;
                case 1:
                    carTypeText = "舒适型";
                    break;
                case 2:
                    carTypeText = "商务型";
                    break;
                case 3:
                    carTypeText = "豪华型";
                    break;
            }
            return carTypeText;
        }

        /// <summary>
        /// 预约叫车状态
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        public static string GetAppointmentCarStatusText(int status)
        {
            string carStatusText = "";
            switch (status)
            {
                case 0:
                    carStatusText = "提交处理";
                    break;
                case 1:
                    carStatusText = "取消预约";
                    break;
                case 2:
                    carStatusText = "预约成功";
                    break;
                default:
                    carStatusText = "未知";
                    break;
            }
            return carStatusText;
        }


        /// <summary>
        /// 获取预约叫车抵扣方式
        /// </summary>
        /// <param name="exchageType"></param>
        /// <returns></returns>
        public static string GetAppointmentCarExchageType(int exchageType)
        {
            string exchangeTypeText = "";
            switch (exchageType)
            {
                case 0:
                    exchangeTypeText = "积分";
                    break;
                case 1:
                    exchangeTypeText = "优惠券";
                    break;
            }
            return exchangeTypeText;
        }


        /// <summary>
        /// 获取面诊方式
        /// </summary>
        /// <param name="consulationType"></param>
        /// <returns></returns>
        public static string GetConsulationTypeText(int consulationType)
        {
            string inventoryStateText = "";
            switch (consulationType)
            {
                case 1:
                    inventoryStateText = "视频";
                    break;

                case 2:
                    inventoryStateText = "照片";
                    break;

                case 3:
                    inventoryStateText = "私信";
                    break;

                case 4:
                    inventoryStateText = "其他";
                    break;

                case 5:
                    inventoryStateText = "粉丝群";
                    break;
                case 6:
                    inventoryStateText = "短视频";
                    break;
                case 7:
                    inventoryStateText = "产品转化";
                    break;
            }
            return inventoryStateText;
        }
        /// <summary>
        /// 获取助理薪资类型
        /// </summary>
        /// <param name="consulationType"></param>
        /// <returns></returns>
        public static string GetPerformanceTypeText(int performanceType)
        {
            string inventoryStateText = "";
            switch (performanceType)
            {
                case 1:
                    inventoryStateText = "成交业绩";
                    break;

                //case 2:
                //    inventoryStateText = "退款业绩";
                //    break;

                case 3:
                    inventoryStateText = "稽查业绩";
                    break;

            }
            return inventoryStateText;
        }


        /// <summary>
        /// 获取消息通知类型
        /// </summary>
        /// <param name="consulationType"></param>
        /// <returns></returns>
        public static string GetNoticeTypeText(int noticeType)
        {
            string inventoryStateText = "";
            switch (noticeType)
            {
                case 1:
                    inventoryStateText = "订单通知";
                    break;

                case 2:
                    inventoryStateText = "日程通知";
                    break;

                case 3:
                    inventoryStateText = "操作通知";
                    break;

                case 4:
                    inventoryStateText = "分诊通知";
                    break;

                case 5:
                    inventoryStateText = "系统消息";
                    break;
            }
            return inventoryStateText;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">需要加密的内容</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string content, string encryptKey)
        {
            if (string.IsNullOrWhiteSpace(content))
                return "";
            DesHelper desUtil = new DesHelper(encryptKey);
            return desUtil.EncryptToBase64String(content);
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptContent">需解密的加密内容</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns></returns>
        public static string Decrypto(string encryptContent, string encryptKey)
        {
            DesHelper desUtil = new DesHelper(encryptKey);
            return desUtil.DecryptFromBase64String(encryptContent);
        }


        /// <summary>
        /// 获取不完整电话，中间四位用*代替
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string GetIncompletePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "";
            if (phone.Length != 11)
            {
                return phone;
            }
            return phone.Substring(0, 3) + "****" + phone.Substring(phone.Length - 4);
        }

        /// <summary>
        /// 获取客户昵称隐藏版
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string GetIncompleteCustomerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "";
            if (name.Length > 1)
            {
                return name.Substring(0, 1) + "****" + name.Substring(name.Length - 0);
            }
            else
            {
                return "*";
            }
        }

        public static int GetAge(DateTime? birthdayInfo)
        {
            int age = 0;
            if (birthdayInfo != null)
            {
                DateTime date = DateTime.Now;
                DateTime birthday = (DateTime)birthdayInfo;
                age = date.Year - birthday.Year;
                if (date.Month < birthday.Month || (date.Month == birthday.Month && date.Day < birthday.Day))
                    age--;
            }
            return age;
        }

        /// <summary>
        /// 根据会员卡编号获取会员卡所需成长值范围
        /// </summary>
        /// <param name="rankcode"></param>
        /// <returns></returns>
        public static Dictionary<string, decimal> GetMemberCradScope(string rankcode)
        {
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            switch (rankcode)
            {
                case "62":
                    dic = new Dictionary<string, decimal>
                    {
                        ["min_amount"] = 0,
                        ["max_amount"] = 200
                    };
                    break;
                case "68":
                    dic = new Dictionary<string, decimal>
                    {
                        ["min_amount"] = 200,
                        ["max_amount"] = 500
                    };
                    break;
                case "70":
                    dic = new Dictionary<string, decimal>
                    {
                        ["min_amount"] = 500,
                        ["max_amount"] = 10000000
                    };
                    break;
                default: throw new Exception("请输入正确的会员卡编码");
            }
            return dic;
        }
        /// <summary>
        /// 根据会员卡编号获取会员卡名称
        /// </summary>
        /// <param name="rankcode"></param>
        /// <returns></returns>
        public static string GetMemberCradName(string rankcode)
        {
            string rankCodeText = "";
            switch (rankcode)
            {
                case "62":
                    rankCodeText = "普通会员";
                    break;
                case "68":
                    rankCodeText = "MEIYA白金会员";
                    break;
                case "70":
                    rankCodeText = "MEIYA黑金会员";
                    break;
                default: throw new Exception("请输入正确的会员卡编码");
            }
            return rankCodeText;
        }
        /// <summary>
        /// 获取录单申请类型
        /// </summary>
        /// <param name="rankcode"></param>
        /// <returns></returns>
        public static string GetContentPlatformOrderAddWorkTypeText(int contentPlatformOrderAddWorkType)
        {
            string contentPlatformOrderAddWorkTypeText = "";
            switch (contentPlatformOrderAddWorkType)
            {
                case 1:
                    contentPlatformOrderAddWorkTypeText = "录单申请";
                    break;
                case 2:
                    contentPlatformOrderAddWorkTypeText = "改绑申请";
                    break;
            }
            return contentPlatformOrderAddWorkTypeText;
        }
        /// <summary>
        /// 根据类型exchangetype获取支付方式名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetRechargeExchangeTypeText(int type)
        {
            string typeText = "";
            switch (type)
            {
                case 0:
                    typeText = "支付宝";
                    break;
                case 1:
                    typeText = "微信";
                    break;
                case 2:
                    typeText = "商品退款";
                    break;
                case 3:
                    typeText = "储值赠送";
                    break;
                case 4:
                    typeText = "微信支付";
                    break;
                default:
                    typeText = "其他";
                    break;
            }
            return typeText;
        }
        /// <summary>
        /// 根据支付类型编码获取支付类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int GetRechargeTypeByExchangeCode(string code)
        {
            int type = 0;
            switch (code)
            {
                case "ALIPAY":
                    type = 0;
                    break;
                case "WECHAT":
                    type = 1;
                    break;
                case "HSQ":
                    type = 4;
                    break;
                default:
                    type = -1;
                    break;
            }
            return type;
        }
        /// <summary>
        /// 获取充值状态文本
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetRechargeStatusText(int status)
        {
            string statusText = "";
            switch (status)
            {
                case 0:
                    statusText = "待付款";
                    break;
                case 1:
                    statusText = "成功";
                    break;
                case 2:
                    statusText = "取消";
                    break;
                case 3:
                    statusText = "支付中";
                    break;
                default:
                    statusText = "其他";
                    break;
            };
            return statusText;
        }

        /// <summary>
        /// 获取拍剪组视频类型文本
        /// </summary>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public static string GerShootingAndClipVideoTypeText(int type)
        {
            string channelTypeText = "";
            switch (type)
            {

                case 1:
                    channelTypeText = "广告片";
                    break;

                case 2:
                    channelTypeText = "口播";
                    break;

                case 3:
                    channelTypeText = "vlong";
                    break;

                case 4:
                    channelTypeText = "企业宣传文化片";
                    break;
                case 0:
                    channelTypeText = "其他";
                    break;
            }
            return channelTypeText;
        }

        /// <summary>
        /// 财务对账单状态
        /// </summary>
        /// <param name="reconciliationDocumentsState"></param>
        /// <returns></returns>
        public static string ReconciliationDocumentsStateText(int reconciliationDocumentsState)
        {
            string typeText = "";
            switch (reconciliationDocumentsState)
            {
                case 0:
                    typeText = "已提交";
                    break;

                case 1:
                    typeText = "待确认";
                    break;

                case 2:
                    typeText = "问题账单";
                    break;

                case 3:
                    typeText = "对账完成";
                    break;
                case 4:
                    typeText = "回款完成";
                    break;
            }
            return typeText;
        }

        /// <summary>
        /// 票据类型
        /// </summary>
        /// <param name="reconciliationDocumentsState"></param>
        /// <returns></returns>
        public static string GetBillTypeText(int BillType)
        {
            string typeText = "";
            switch (BillType)
            {
                case 0:
                    typeText = "医美票据";
                    break;

                case 1:
                    typeText = "其他票据";
                    break;

            }
            return typeText;
        }

        /// <summary>
        /// 票据回款状态
        /// </summary>
        /// <param name="reconciliationDocumentsState"></param>
        /// <returns></returns>
        public static string GetBillReturnBackStateText(int BillReturnBackState)
        {
            string typeText = "";
            switch (BillReturnBackState)
            {
                case 0:
                    typeText = "未回款";
                    break;

                case 1:
                    typeText = "回款中";
                    break;

                case 2:
                    typeText = "已回款";
                    break;

            }
            return typeText;
        }
        #region RFM相关
        /// <summary>
        /// 获取R,F,M值对应的文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetRFMText(int value)
        {
            string str = "";
            switch (value)
            {
                case 0:
                    str = "低";
                    break;
                case 1:
                    str = "高";
                    break;
                case 2:
                    str = "VIP";
                    break;
                default:
                    str = "";
                    break;
            }
            return str;
        }

        public static string GetRFMTagText(int tag)
        {
            string str = "";
            switch (tag)
            {
                case 0:
                    str = "超级VIP（RV）";
                    break;
                case 1:
                    str = "重要价值客户(R1)";
                    break;
                case 2:
                    str = "重要保持客户(R2)";
                    break;
                case 3:
                    str = "重要发展客户(R3)";
                    break;
                case 4:
                    str = "重要挽留客户(R4)";
                    break;
                case 5:
                    str = "一般价值客户(R5)";
                    break;
                case 6:
                    str = "一般保持客户(R6)";
                    break;
                case 7:
                    str = "一般发展客户(R7)";
                    break;
                case 8:
                    str = "一般挽留客户(R8)";
                    break;
                default:
                    str = "/";
                    break;
            }
            return str;
        }

        public static int GetRFMTagByName(string tag)
        {
            int t = 1;
            switch (tag)
            {
                case "RV":
                    t = 0;
                    break;
                case "R1":
                    t = 1;
                    break;
                case "R2":
                    t = 2;
                    break;
                case "R3":
                    t = 3;
                    break;
                case "R4":
                    t = 4;
                    break;
                case "R5":
                    t = 5;
                    break;
                case "R6":
                    t = 6;
                    break;
                case "R7":
                    t = 7;
                    break;
                case "R8":
                    t = 8;
                    break;

            }
            return t;
        }
        #endregion
        /// <summary>
        /// 礼物发放类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GiftSendTypeText(int type)
        {
            string text = "";
            switch (type)
            {
                case 0:
                    text = "自行领取";
                    break;
                case 1:
                    text = "系统发放";
                    break;
                default:
                    text = "其他";
                    break;
            }
            return text;
        }

        /// <summary>
        /// 顾客质量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CustomerQuantityText(int type)
        {
            string text = "";
            switch (type)
            {
                case 0:
                    text = "一般";
                    break;
                case 1:
                    text = "优质";
                    break;
                default:
                    text = "其他";
                    break;
            }
            return text;
        }
        /// <summary>
        /// 未加V原因
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string UnAddWechatReasonText(int type)
        {
            string text = "";
            switch (type)
            {
                case 0:
                    text = "其他";
                    break;
                case 1:
                    text = "号码未获取";
                    break;
                case 2:
                    text = "未接/拒接电话";
                    break;
                case 3:
                    text = "只要产品";
                    break;
                case 4:
                    text = "不想咨询";
                    break;
                case 5:
                    text = "加V未通过";
                    break;
                case 6:
                    text = "错拍链接";
                    break;
                case 7:
                    text = "非本人拍";
                    break;
                default:
                    text = "未知原因";
                    break;
            }
            return text;
        }
        /// <summary>
        /// 小黄车客户归属渠道
        /// </summary>
        /// <param name="belongChannel"></param>
        /// <returns></returns>
        public static string BelongChannelText(int belongChannel)
        {
            string text = "";
            switch (belongChannel)
            {
                case 1:
                    text = "直播前";
                    break;
                case 2:
                    text = "直播中";
                    break;
                case 3:
                    text = "直播后";
                    break;
                default:
                    text = "未归属";
                    break;
            }
            return text;
        }
        /// <summary>
        /// 直播中带货订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string LivingTakeGoodsOrderStatusText(int status)
        {
            string text = "";
            switch (status)
            {
                case 3:
                    text = "已付款";
                    break;
                case 4:
                    text = "待发货";
                    break;
                case 5:
                    text = "已发货";
                    break;
                case 8:
                    text = "已收货";
                    break;
                case 9:
                    text = "已退款";
                    break;
                case 11:
                    text = "订单完成";
                    break;
                default:
                    text = "";
                    break;
            }
            return text;
        }
    }
}
