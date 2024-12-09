using Fx.Amiya.Dto;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Input;
using Fx.Amiya.Dto.AssistantHomePage.Input;
using Fx.Amiya.Dto.AssistantHomePage.Result;
using Fx.Amiya.Dto.ContentPlateFormOrder;
using Fx.Amiya.Dto.ContentPlatFormOrderSend;
using Fx.Amiya.Dto.FinancialBoard;
using Fx.Amiya.Dto.Performance;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.Dto.TmallOrder;
using Fx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IContentPlatFormOrderDealInfoService
    {
        Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetOrderListWithPageAsync(DateTime? startDate, DateTime? endDate, DateTime? sendStartDate, DateTime? sendEndDate, int? consultationType, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, bool? isToHospital, DateTime? tohospitalStartDate, DateTime? toHospitalEndDate, DateTime? dealStartDate, DateTime? dealEndDate, int? toHospitalType, bool? isDeal, int? lastDealHospitalId, bool? isAccompanying, bool? isOldCustomer, int? CheckState, bool? isReturnBakcPrice, DateTime? returnBackPriceStartDate, DateTime? returnBackPriceEndDate, int? customerServiceId, string keyWord, int employeeId, string createBillCompanyId, bool? isCreateBill, int pageNum, int pageSize, bool? dataFromint, int? consumptionType);
        Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetSimpleOrderListWithPageAsync(DateTime? startDate, DateTime? endDate, bool? isDeal, int? lastDealHospitalId, string keyWord, int employeeId, int pageNum, int pageSize);
        Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetContentPlatFormOrderDealInfoByReconciliationDocumentsIdAsync(string reconciliationDocumentsId, int pageNum, int pageSize);
        Task<List<ContentPlatFormOrderDealInfoDto>> GetOrderDealInfoListReportAsync(DateTime? startDate, DateTime? endDate, DateTime? sendStartDate, DateTime? sendEndDate, decimal? minAddOrderPrice, decimal? maxAddOrderPrice, int? consultationType, bool? isToHospital, DateTime? tohospitalStartDate, DateTime? toHospitalEndDate, int? toHospitalType, bool? isDeal, int? lastDealHospitalId, bool? isAccompanying, bool? isOldCustomer, int? CheckState, DateTime? checkStartDate, DateTime? checkEndDate, bool? isCreateBill, bool? isReturnBakcPrice, DateTime? returnBackPriceStartDate, DateTime? returnBackPriceEndDate, int? customerServiceId, string belongCompanyId, string keyWord, int employeeId, bool hidePhone, int? consumptionType,List<int?> liveAnchorIds);
        Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> GetListWithPageAsync(string contentPlafFormOrderId, int pageNum, int pageSize);
        /// <summary>
        /// 医院端获取成交情况
        /// </summary>
        /// <param name="contentPlafFormOrderId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="hodpitalId"></param>
        /// <returns></returns>
        Task<FxPageInfo<ContentPlatFormOrderDealInfoDto>> HospitalGetListWithPageAsync(string contentPlafFormOrderId, int pageNum, int pageSize,int hospitalId);
        Task<string> AddAsync(AddContentPlatFormOrderDealInfoDto addDto);
        Task<ContentPlatFormOrderDealInfoDto> GetByIdAsync(string id);

        /// <summary>
        /// 根据三方标识获取成交情况
        /// </summary>
        /// <param name="otherAppOrder"></param>
        /// <returns></returns>
        Task<ContentPlatFormOrderDealInfoDto> GetByOtherAppOrderIdAsync(string otherAppOrder);

        Task UpdateAsync(UpdateContentPlatFormOrderDealInfoDto updateDto);

        /// <summary>
        /// 更新新/老客数据
        /// </summary>
        /// <param name="orderDealId"></param>
        /// <param name="isOldCustomer"></param>
        /// <returns></returns>
        Task UpdateIsOldCustomerAsync(string orderDealId, bool isOldCustomer);

        Task DeleteAsync(string id);
        Task<List<ContentPlatFormOrderDealInfoDto>> GetByOrderIdAsync(string orderId);
        Task CheckAsync(UpdateContentPlatFormOrderDealInfoDto updateDto);

        Task SettleAsync(UpdateContentPlatFormOrderDealInfoDto updateDto);
        /// <summary>
        /// 回款
        /// </summary>
        /// <param name="reconciliationDocumentsList"></param>
        /// <returns></returns>
        Task SettleListAsync(ReturnBackOrderDto returnBackOrder);
        /// <summary>
        /// 获取当日上门成交业绩
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetTodaySendPerformanceAsync(int liveAnchorId, DateTime recordDate);
        /// <summary>
        /// 查询总业绩
        /// </summary>
        /// <param name="IsOldCustomer">新老客</param>
        /// <returns></returns>
        Task<decimal> GetPerformance(bool? IsOldCustomer);
        /// <summary>
        /// 新客上门总人数
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetNewCustomerToHospitalCount();
        /// <summary>
        /// 新客成交总人数
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetNewCustomerDealCount();

        /// <summary>
        /// 根据到院id获取当日上门成交业绩
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetTodaySendPerformanceByHospitalIdAsync(List<int> hospitalId, DateTime recordDate);
        /// <summary>
        /// 根据到院id获取指定时间内的上门成交业绩
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetSendPerformanceByHospitalIdAndDateTimeAsync(QueryHospitalTransformDataDto query);
        /// <summary>>
        /// 根据到院id获取当月上门成交业绩
        /// </summary>
        /// <param name="hospitalIds"></param>
        /// <param name="recordDate"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetMonthSendPerformanceByHospitalIdListAsync(List<int> hospitalIds, DateTime recordDate);

        /// <summary>
        /// 根据到院id和时间获取当月上门成交业绩
        /// </summary>
        /// <param name="hospitalIds"></param>
        /// <param name="recordStartDate"></param>
        /// <param name="recordEndDate"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetMonthSendPerformanceByHospitalIdListAsync(List<int> hospitalIds, DateTime recordStartDate, DateTime recordEndDate);

        /// <summary>
        ///  根据到院id，时间和手机号获取当月上门成交业绩
        /// </summary>
        /// <param name="hospitalIds"></param>
        /// <param name="recordStartDate"></param>
        /// <param name="recordEndDate"></param>
        /// <param name="phoneList"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GeVisitAndDealNumByHospitalIdAndPhoneListAsync(List<int> hospitalIds, DateTime recordStartDate, DateTime recordEndDate, List<string> phoneList);
        /// <summary>
        /// 根据到院id和时间获取指定月份成交量
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="recordDate"></param>
        /// <returns></returns>
        Task<int> GetMonthSendPerformanceByHospitalIdAsync(int hospitalId, int year, int month);
        /// <summary>
        /// 根据到院id和时间获取指定年份成交量
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<int> GetYearSendPerformanceByHospitalIdAsync(int hospitalId, int year);
        /// <summary>
        /// 根据到院id获取总成交量
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        Task<int> GetSendPerformanceByHospitalIdAsync(int hospitalId);
        /// <summary>
        /// 根据到院id与月份获取上门成交业绩
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetSendPerformanceByHospitalIdAndMonthAsync(int hospitalId, int year, int month);
        /// <summary>
        /// 获取医院对账业绩
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        Task<List<FinancialHospitalDealPriceBoardDto>> GetHospitalDealPriceDataAsync(DateTime? startDate, DateTime? endDate, int? hospitalId, int pageNum, int pageSize);
        /// <summary>
        /// 获取消费类型
        /// </summary>
        /// <returns></returns>
        Task<List<BaseKeyValueDto<int>>> GetConsumptionTypeAsync();

        #region【业绩板块】

        /// <summary>
        /// 根据主播获取指定年月的业绩(可选择是否筛选新老客)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldCustomer">新客/老客</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceByYearAndMonth(int year, int month, bool? isOldCustomer, List<int> LiveAnchorIds);

        /// <summary>
        /// 获取派单成交业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend">历史/当月派单,true为历史派单当月成交，false为当月派单当月成交</param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetSendAndDealPerformanceByYearAndMonth(int year, int month, bool? isOldSend, List<int> liveAnchorIds);

        /// <summary>
        /// 按月筛选新老客数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="isCustomer">筛选新老客(传null不筛选)</param>
        /// <param name="LiveAnchorIds">各个平台主播id集合</param>
        /// <returns></returns>
        Task<List<PerformanceInfoByDateDto>> GetPerformanceBrokenLineAsync(int year, int month, bool? isCustomer, List<int> LiveAnchorIds);

        /// <summary>
        /// 获取成交情况折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldSend"></param>
        /// <returns></returns>
        Task<List<PerformanceBrokenLine>> GetHistoryAndThisMonthOrderPerformance(int year, int month, bool? isOldSend, List<int> liveAnchorIds);


        /// <summary>
        /// 根据主播获取分组独立/协助业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isAssist"></param>
        /// <param name="LiveAnchorIds"></param>
        /// <param name="amiyaEmployeeId"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetIndependentOrAssistPerformanceByYearAndMonth(int year, int month, bool? isAssist, List<int> LiveAnchorIds, int amiyaEmployeeId);

        /// <summary>
        /// 获取主播客单价
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isOldCustomer"></param>
        /// <param name="LiveAnchorIds"></param>
        /// <returns></returns>
        Task<List<PerformanceBrokenLine>> GetGuestUnitPricePerformanceBrokenLineAsync(int year, int month, bool? isOldCustomer, List<int> LiveAnchorIds);

        /// <summary>
        /// 根据条件获取独立/协助业绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isAssist"></param>
        /// <param name="LiveAnchorIds"></param>
        /// <param name="amiyaEmployeeId"></param>
        /// <returns></returns>
        Task<List<PerformanceBrokenLine>> GetIndependenceOrAssistAsync(int year, int month, bool? isAssist, List<int> LiveAnchorIds, int amiyaEmployeeId);


        #endregion

        #region 【新业绩板块】
        /// <summary>
        /// 根据精确时间线主播获取啊美雅业绩返回新老客等数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isOldCustomer"></param>
        /// <param name="LiveAnchorIds"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceByDateAsync(DateTime startDate, DateTime endDate, List<int> LiveAnchorIds);

        /// <summary>
        /// 根据精确时间线主播获取啊美雅业绩返回主播id和到院类型
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="LiveAnchorIds">主播id集合()传空查所有</param>
        /// <returns></returns>
        Task<List<PerformanceDto>> GetPerformanceByDateAndLiveAnchorIdsAsync(DateTime startDate, DateTime endDate, List<int> LiveAnchorIds);
        /// <summary>
        /// 根据精确时间线获取业绩详情
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="LiveAnchorIds"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceDetailByDateAsync(DateTime startDate, DateTime endDate, List<int> LiveAnchorIds);
        /// <summary>
        /// 根据助理id集合获取业绩数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantId"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetPerformanceDetailByDateAndAssistantIdListAsync(DateTime startDate, DateTime endDate, List<int> assistantId);

        /// <summary>
        /// 根据精确时间线获取派单成交业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isOldSend"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetSendAndDealPerformanceAsync(DateTime startDate, DateTime endDate, bool? isOldSend, List<int> liveAnchorIds);
        #endregion

        #region 【分组业绩板块】
        #endregion


        #region 全国机构top10运营占比
        /// <summary>
        /// 获取总业绩业绩前10的医院业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenHospitalTotalPerformance();

        /// <summary>
        /// 获取新客业绩前10的医院业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenNewCustomerPerformance();


        /// <summary>
        /// 获取老客业绩前10的医院业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenOldCustomerPerformance();


        /// <summary>
        /// 获取新客上门前10的医院业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenNewCustomerToHospitalPformance();

        /// <summary>
        /// 获取新客成交人数前10的医院业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoHospitalPerformanceDto>> GetTopTenNewCustomerDealPerformance();
        #endregion

        #region 城市top10运营占比
        /// <summary>
        /// 获取总业绩业绩前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityTotalPerformance();

        /// <summary>
        /// 获取新客业绩前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityNewCustomerPerformance();


        /// <summary>
        /// 获取老客业绩前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityOldCustomerPerformance();


        /// <summary>
        /// 获取新客上门前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityNewCustomerToHospitalPformance();

        /// <summary>
        /// 获取新客成交人数前10的城市业绩数据
        /// </summary>
        /// <returns></returns>
        Task<List<ContentPlateformOrderDealInfoCityPerformanceDto>> GetTopTenCityNewCustomerDealPerformance();
        #endregion

        #region 【对账单板块】
        Task<List<UnCheckHospitalOrderDto>> GetUnCheckHospitalOrderAsync(DateTime? startDate, DateTime? endDate, int? hospitalId);
        #endregion

        #region 财务看板板块

        /// <summary>
        /// 根据客服id获取财务看板客服业绩信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerServiceId"></param>
        /// <returns></returns>
        Task<CustomerServiceBoardDataDto> GetCustomerServiceBoardDataByCustomerServiceIdAsync(DateTime? startDate, DateTime? endDate, int customerServiceId);
        /// <summary>
        /// 根据主播id获取客服录入成交单业绩信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<LiveAnchorBoardDataDto>> GetLiveAnchorPriceByLiveAnchorIdAsync(DateTime? startDate, DateTime? endDate, List<int> liveAnchorIds);
        /// <summary>
        /// 根据主播id获取归属客服业绩信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerServiceId"></param>
        /// <returns></returns>
        Task<List<CustomerServiceBoardDataDto>> GetCustomerServiceBelongBoardDataByCustomerServiceIdAsync(DateTime? startDate, DateTime? endDate, int? customerServiceId);

        #endregion
        #region 助理首页

        Task<FxPageInfo<TodayToHospitalDataDto>> GetTodayToHospitalDataAsync(QueryAssistantHomePageDataDto query);
        Task<MonthPerformanceCompleteSituationDataDto> GetAssistantMonthPerformanceDataAsync(QueryAssistantHomePageDataDto query);

        #endregion
        #region【系统端运营看板】
        /// <summary>
        /// 根据时间获取简单的成交业绩（区分新老客）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="liveAnchorIds"></param>
        /// <param name="isOldCustomer"></param>
        /// <returns></returns>
        Task<List<ContentPlatFormOrderDealInfoDto>> GetSimplePerformanceDetailByDateAsync(int year, List<int> liveAnchorIds, bool? isOldCustomer);

        /// <summary>
        /// 根据成交编号获取成交登记日期
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        Task<DateTime> GetDealCraeteDateByDealIdAsync(string dealId);

        /// <summary>
        /// 根据时间和助理id获取当前总业绩值
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<decimal> GetTotalPerformanceByEmployeeAndDateAsync(DateTime startDate, DateTime endDate, int employeeId);
        #endregion

        #region 【枚举下拉框】
        List<BaseIdAndNameDto> GetOrderDealPerformanceTypeList();

        #endregion
    }
}
