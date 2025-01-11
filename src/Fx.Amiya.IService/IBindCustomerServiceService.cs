using Fx.Amiya.Dto.BindCustomerService;
using Fx.Amiya.Dto.WxAppConfig;
using Fx.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IBindCustomerServiceService
    {
        /// <summary>
        /// 添加绑定客服
        /// </summary>
        /// <param name="addDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task AddAsync(AddBindCustomerServiceDto addDto, int employeeId);
        /// <summary>
        /// 根据手机号获取绑定客户详情
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<BindCustomerServiceDto> GetEmployeeDetailsByPhoneAsync(string phone);

        /// <summary>
        /// 获取客户池客服下的手机号（分页）
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<BindCustomerServiceDto>> GetPublicPoolPhoneAsync(DateTime? startDate, DateTime? endDate, string keyWord, int pageNum, int pageSize);
        Task<BindCustomerServiceDto> GetByIdAsync(int id);

        /// <summary>
        /// 获取我的客户
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<MyCustomerInfoDto> GetCustomerCountByEmployeeIdAsync(int employeeId);

        /// <summary>
        /// 根据手机号获取归属客服id
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<int> GetEmployeeIdByPhone(string phone);

        /// <summary>
        /// 根据手机号获取客户手机号是否有多个
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<List<string>> GetEmployeePhoneByPhone(string phone);

        /// <summary>
        /// 根据手机号获取归属客服名称
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<string> GetBindCustomerServiceNameByPhone(string phone);
        /// <summary>
        /// 根据主播名称和消费金额获取名下总客户情况
        /// </summary>
        /// <param name="liveAnchorName"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task<int> GetBindCustomerServiceCountByLiveAnchorNameAndPricePhone(string liveAnchorName, decimal price);

        /// <summary>
        /// 根据绑定助理和消费金额获取名下总客户情况
        /// </summary>
        /// <param name="assistantId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task<int> GetBindCustomerServiceCountByAssistantAndPricePhone(int assistantId, decimal price);
        Task<string?> UpdateAsync(UpdateBindCustomerServiceDto updateDto, int employeeId);

        /// <summary>
        /// 小程序绑定客户时修改绑定客服的userId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task UpdateBindUserIdAsync(string customerId,string appid=null);
        /// <summary>
        /// 内容平台与升单成交加入成交金额
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        Task UpdateConsumePriceAsync(string phone, decimal Price, int Channel, string newLiveAnchor, string newWeChatNo, string newContentPlatForm, int AllOrderCount);

        /// <summary>
        /// 扣除客户消费累计金额与订单数
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="Price"></param>
        /// <param name="Channel"></param>
        /// <returns></returns>
        Task ReduceConsumePriceAsync(string phone, decimal Price, int Channel);
        /// <summary>
        /// 内容平台与升单成交加入成交金额并更新最近消费所属主播
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        Task UpdateConsumePriceAndLiveAnchorAsync(string phone, decimal Price, int Channel, int AllOrderCount, string LiveAnchorName);

        /// <summary>
        /// 添加绑定客服
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task OnlyAddFistlyAsync(AddBindCustomerServiceFirstlyDto addDto);

        /// <summary>
        /// 获取消费金额大于0的顾客
        /// </summary>
        /// <returns></returns>
        Task<List<BindCustomerServiceDto>> GetAllCustomerAsync();

        /// <summary>
        /// 根据条件获取客户RFM模型数据
        /// </summary>
        /// <param name="bindCustomerServiceIds"></param>
        /// <returns></returns>
        Task<List<BindCustomerServiceRfmDataDto>> GetAllCustomerByRFMAsync(List<int> bindCustomerServiceIds);

        /// <summary>
        /// 根据RFM条件获取客户RFM详情数据
        /// </summary>
        /// <param name="rfmType"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<BindCustomerServiceDto>> GetAllCustomerByRFMTypeAsync(List<int> bindCustomerServiceIds, int rfmType, int pageNum, int pageSize);

        /// <summary>
        /// 修改客户RFM等级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rFMLevel"></param>
        /// <returns></returns>
        Task UpdateCustomerRFMLevelAsync(int id, int rFMLevel);

        /// <summary>
        /// 修改累计发放礼品情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateCustomerRFMLevelAsync(int id);

        /// <summary>
        /// 添加RFM更新记录
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task AddRFMTypeUpdateLogAsync(AddBindCustomerRFMLevelUpdateLog addDto);

        /// <summary>
        /// 获取客户RFM等级更新记录
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="keyWord"></param>
        /// <param name="customerServiceId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<BindCustomerRFMLevelUpdateLogDto>> GetCustomerRFMTypeUpdateDataAsync(DateTime? startDate, DateTime? endDate, string keyWord, int? customerServiceId, int pageNum, int pageSize);
    }
}
