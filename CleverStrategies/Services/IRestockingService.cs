using RetailStore.EF.Models;
using static RetailStore.EF.Models.SmartRestockingPlanModel;

namespace SmartRestockingPlan.Services
{
    public interface IRestockingService
    {
        Task<List<ProductStock>> CalculateRestockingAsync(List<SaleTransaction> salesData);
    }
}
