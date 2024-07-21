using SmartRestockingPlan.Repositorys;
using static RetailStore.EF.Models.SmartRestockingPlanModel;

namespace SmartRestockingPlan.Services
{
    public class RestockingService(ISalesRepository salesRepository) : IRestockingService
    {
        private readonly ISalesRepository _salesRepository = salesRepository;

        public async Task<List<ProductStock>> CalculateRestockingAsync(List<SaleTransaction> salesData)
        {
            return await _salesRepository.CalculateRestockingAsync(salesData);
        }
    }
}
