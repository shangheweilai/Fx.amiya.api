using Fx.Amiya.Dto;
using Fx.Amiya.Dto.AmiyaOperationsBoardService;
using Fx.Amiya.Dto.AmiyaOperationsBoardService.Result;
using Fx.Amiya.Dto.AssistantHomePage.Input;
using Fx.Amiya.Dto.Performance;
using Fx.Amiya.Dto.ShoppingCartRegistration;
using Fx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IShoppingCartRegistrationService
    {
        Task<FxPageInfo<ShoppingCartRegistrationDto>> GetListWithPageAsync(DateTime? startDate, DateTime? endDate, int? LiveAnchorId, bool? isCreateOrder, int? createBy, bool? isSendOrder, int? employeeId, bool? isAddWechat, bool? isWriteOff, bool? isConsultation, bool? isReturnBackPrice, string keyword, string contentPlatFormId, int pageNum, int pageSize, decimal? minPrice, decimal? maxPrice, int? AdmissionId, DateTime? startRefundTime, DateTime? endRefundTime, DateTime? startBadReviewTime, DateTime? endBadReviewTime, int? ShoppingCartRegistrationCustomerType, int? emergencyLevel, bool? isBadReview, string baseLiveAnchorId, int? source, int? belongDepartment);
        Task AddAsync(AddShoppingCartRegistrationDto addDto);

        Task AddListAsync(List<AddShoppingCartRegistrationDto> addDtoList);
        Task<ShoppingCartRegistrationDto> GetByIdAsync(string id);
        Task<ShoppingCartRegistrationDto> GetByPhoneAsync(string phone, int createBy);
        Task<ShoppingCartRegistrationDto> GetAddOrderPriceByPhoneAndLiveAnchorIdAsync(string phone, int liveAnchorId);
        Task<List<ShoppingCartRegistrationDto>> GetByPhoneAsync(string phone);
        Task UpdateAsync(UpdateShoppingCartRegistrationDto updateDto);

        Task AssignAsync(string id, int assignBy);
        /// <summary>
        /// 录单触达
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task UpdateCreateOrderAsync(string phone);
        /// <summary>
        /// 派单触达
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task UpdateSendOrderAsync(string phone);
        Task DeleteAsync(string id);
        /// <summary>
        /// 输出紧急程度列表
        /// </summary>
        /// <returns></returns>
        List<EmergencyLevelDto> GetEmergencyLevelList();
        /// <summary>
        /// 获取客户来源
        /// </summary>
        /// <returns></returns>
        List<BaseKeyValueDto<int>> GetCustomerSourceList(string contentPlatFormId, int? channel);

        /// <summary>
        /// 获取客户类型列表
        /// </summary>
        /// <returns></returns>
        List<BaseKeyValueDto<int>> GetCustomerTypeList();
        /// <summary>
        /// 获取带货产品类型列表
        /// </summary>
        /// <returns></returns>
        List<BaseKeyValueDto<int>> GetShoppingCartTakeGoodsProductTypeList();
        /// <summary>
        /// 获取面诊方式列表
        /// </summary>
        /// <returns></returns>
        List<BaseKeyValueDto<int>> GetShoppingCartConsultationTypeText();

        /// <summary>
        /// 获取获客方式
        /// </summary>
        /// <returns></returns>
        List<BaseKeyValueDto<int>> GetShoppingCartGetCustomerTypeText();
        /// <summary>
        /// 获取归属部门
        /// </summary>
        /// <returns></returns>
        List<BaseIdAndNameDto<int>> GetBelongDepartmentList();
        /// <summary>
        /// 根据创建人与时间线获取医美/带货客资加v量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GetShoppingCartRegistionAddWechatNumDto> GetShoppingCartRegistionAddWechatNumAsync(QueryAddWeChatDto query);
        #region 【日数据业绩生成】

        /// <summary>
        /// 根据主播ID获取当日面诊卡情况
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetDialyConsulationCardInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate);

        /// <summary>
        /// 根据主播ID获取当日加v派单情况
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetDialyAddWeChatInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate);
        /// <summary>
        /// 获取直播后小黄车相关日运营数据
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <param name="recordDate"></param>
        /// <returns></returns>
        Task<AfterLiveDataDto> GetAfterLiveDataByLiveAnchorIdAsync(int liveAnchorId, DateTime recordDate);
        /// <summary>
        /// 根据条件获取今日小黄车退款量
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetDialyYellowCardRefundInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate);

        /// <summary>
        /// 根据条件获取今日小黄车差评量
        /// </summary>
        /// <param name="liveAnchorId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetDialyYellowCardBadReviewInfoByLiveAnchorId(int liveAnchorId, DateTime recordDate);

        #endregion


        #region 【报表相关】
        Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistrationReportAsync(DateTime? startDate, DateTime? endDate, int? emergencyLevel, int? LiveAnchorId, bool? isCreateOrder, bool? isSendOrder, int? employeeId, bool? isAddWechat, bool? isWriteOff, bool? isConsultation, bool? isReturnBackPrice, string keyword, string contentPlatFormId, bool isHidePhone, string baseLiveAnchorId, int? source);
        #endregion

        #region 【业绩数据】
        /// <summary>
        /// 根据条件获取小黄车登记业绩（照片面诊业绩与视频面诊业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistrationListByLiveAnchorNameAndIsVideoAsync(int year, int month, bool isVideo, List<int> liveAnchorIds);

        /// <summary>
        /// 获取面诊卡库存
        /// </summary>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistrationInventoryAsync(List<int> liveAnchorIds);


        /// <summary>
        /// 根据条件获取基础经营看板信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isHistoryConsulationCardConsumed">是否为历史面诊卡消耗数据</param>
        /// <param name="isConsulationCardRefund">是否为历史面诊卡退单数据</param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetBaseBusinessPerformanceByLiveAnchorNameAsync(int year, int month, bool? isHistoryConsulationCardConsumed, bool? isConsulationCardRefund, List<int> liveAnchorIds);


        /// <summary>
        /// 根据平台/有效潜在获取小黄车登记业绩（新经营看板业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetNewBaseBusinessPerformanceByLiveAnchorNameAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, string contentPlatFormId, string liveAnchorBaseId);
        /// <summary>
        /// 根据基础主播获取获取潜在/有效 加v,分诊
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetPerformanceByBaseLiveAnchorIdAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, string baseLiveAnchorId);
        /// <summary>
        /// 根据助理id集合获取获取潜在/有效 加v,分诊
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="contentPlatFormId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList);
        /// <summary>
        /// 根据条件获取助理小黄车业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetShopCartRegisterPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, bool? isEffectiveCustomerData, List<int> assistantIdList);

        /// <summary>
        /// 根据条件获取小黄车照片/视频面诊业绩折线图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isVideo"></param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<PerformanceInfoByDateDto>> GetPictureOrVideoConsultationBrokenLineAsync(int year, int month, bool isVideo, List<int> liveAnchorIds);

        /// <summary>
        /// 根据条件获取小黄车登记业绩（经营看板业绩）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isHistoryConsulationCardConsumed">是否为历史面诊卡消耗数据</param>
        /// <param name="isConsulationCardRefund">是否为历史面诊卡退单数据</param>
        /// <param name="liveAnchorIds"></param>
        /// <returns></returns>
        Task<List<PerformanceBrokenLine>> GetBaseBusinessPerformanceByLiveAnchorNameBrokenLineAsync(int year, int month, bool? isHistoryConsulationCardConsumed, bool? isConsulationCardRefund, bool? isAddWechat, bool? isConsulation, List<int> liveAnchorIds);

        /// <summary>
        /// 根据时间获取下卡，退卡，分诊，加v数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<GetCustomerDataDto> GetCustomerDataAsync(DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// 根据条件获取小黄车登记列表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetShoppingCartRegistionDataByRecordDate(DateTime startDate, DateTime endDate, string liveAnchorBaseId);
        #endregion
        #region 啊美雅运营看板
        /// <summary>
        /// 获取指标转化基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="isEffective"></param>
        /// <returns></returns>
        Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetIndicatorConversionDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, bool? isEffective, bool? isCurrentMonth = null);
        /// <summary>
        /// 获取分诊新客转化情况基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="isEffective"></param>
        /// <param name="isOldCustomer"></param>
        /// <returns></returns>
        Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetCurrentMonthNewCustomerConversionDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, bool? isEffective, bool isOldCustomer);
        /// <summary>
        /// 根据助理id获取指标转化情况
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <param name="isEffective"></param>
        /// <returns></returns>

        Task<List<ShoppingCartRegistrationIndicatorBaseDataDto>> GetIndicatorConversionDataByAssistantIdsAsync(DateTime startDate, DateTime endDate, List<int> assistantIds, bool? isEffective);
        /// <summary>
        /// 获取历史新客转化情况
        /// </summary>
        /// <param name="startData"></param>
        /// <param name="endDate"></param>
        /// <param name="isOldCustomer"></param>
        /// <returns></returns>
        Task<CompanyNewCustomerConversionBaseDataDto> GetHistoryNewCustomerConversionDataAsync(DateTime startData, DateTime endDate, string baseLiveAnchorId, bool? isOldCustomer, bool? isEffective);
        #endregion
        #region 助理首页

        Task<List<ShoppingCartRegistrationDto>> GetAsistantMonthPerformanceDataAsync(QueryAssistantHomePageDataDto query);

        #endregion
        #region 转化
        /// <summary>
        /// 获取流量和客户转化基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        Task<ShoppingCartRegistrationIndicatorBaseDataDto> GetFlowAndCustomerTransformDataAsync(DateTime startDate, DateTime endDate, string baseLiveAnchorId, List<string> contentPlatformIds);
        /// <summary>
        /// 获取助理流量和客户转化基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="baseLiveAnchorId"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationIndicatorBaseDataDto>> GetAssitantFlowAndCustomerTransformDataAsync(DateTime startDate, DateTime endDate, bool? isCurrentMonth, string baseLiveAnchorId, List<string> contentPlatformIds);
        #endregion
        #region 助理看板
        /// <summary>
        /// 根据助理获取客资人数
        /// </summary>
        /// <returns></returns>
        Task<AssistantDistributeConsulationTypeDataDto> GetDistributeConsulationTypeDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList, bool? isAddWechat = null);
        /// <summary>
        /// 根据助理获取有效/潜在客资人数
        /// </summary>
        /// <returns></returns>
        Task<EffOrPotAssistantDistributeConsulationDataDto> GetEffOrPotDistributeConsulationTypeDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList);
        /// <summary>
        /// 根据助理获取助理分诊折线图基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        Task<List<BaseKeyValueDto<string, int>>> GetDistributeConsulationTypeBrokenLineDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIdList);
        #endregion
        #region 行政客服看板
        /// <summary>
        /// 获取行政客服客资数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantId"></param>
        /// <param name="isAddWechat"></param>
        /// <returns></returns>
        Task<AssistantDistributeConsulationTypeDataDto> GetAdminCustomerDistributeConsulationTypeDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIds, bool? isAddWechat = null);
        /// <summary>
        /// 获取行政客服分诊折线图基础数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        Task<List<BaseKeyValueDto<string, int>>> GetAdminCustomerDistributeConsulationTypeBrokenLineDataAsync(DateTime startDate, DateTime endDate, List<int> assistantIds);

        /// <summary>
        /// 根据条件获取行政客服小黄车业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isEffectiveCustomerData"></param>
        /// <param name="assistantIdList"></param>
        /// <returns></returns>
        Task<List<ShoppingCartRegistrationDto>> GetAdminCustomerShopCartRegisterPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, List<int> assistantIds,BelongChannel? belongChannel=null);
        Task<List<ShoppingCartRegistrationDto>> GetBeforeLiveShopCartRegisterPerformanceByAssistantIdListAsync(DateTime startDate, DateTime endDate, string baseId, List<int> assistantIds, BelongChannel? belongChannel = null);
        #endregion
    }
}
