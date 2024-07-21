using static RetailStore.EF.Models.SmartRestockingPlanModel;

namespace SmartRestockingPlan.Repositorys;

public interface ISalesRepository
{
    Task<List<ProductStock>> CalculateRestockingAsync(List<SaleTransaction> salesData);
}
