using Fx.Amiya.Dto.CustomerServiceCheckPerformance.Input;
using Fx.Amiya.Dto.CustomerServiceCheckPerformance.Result;
using Fx.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface ICustomerServiceCheckPerformanceService
    {
        Task<FxPageInfo<CustomerServiceCheckPerformanceDto>> GetListAsync(QueryCustomerServiceCheckPerformanceDto query);
        Task AddAsync(AddCustomerServiceCheckPerformanceDto addDto);
        Task AddListAsync(List<AddCustomerServiceCheckPerformanceDto> addDto);
        Task<CustomerServiceCheckPerformanceDto> GetByIdAsync(string id);
        Task<CustomerServiceCheckPerformanceDto> GetByDealIdAsync(string dealId);
        Task UpdateAsync(UpdateCustomerServiceCheckPerformanceDto updateDto);
        Task DeleteAsync(string id);
        Task AddCustomerServiceCompensationIdAsync(List<string> ids, string customerServiceCompensationId, int CustomerServiceCompensationEmpId);
        Task RemoveCustomerServiceCompensationIdAsync(string customerServiceCompensationId);
    }
}
