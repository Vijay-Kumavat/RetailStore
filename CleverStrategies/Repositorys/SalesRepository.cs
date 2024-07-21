using RetailStore.EF.Data;
using RetailStore.EF.Models;
using static RetailStore.EF.Models.SmartRestockingPlanModel;

namespace SmartRestockingPlan.Repositorys;

public class SalesRepository(AppDbContext context) : ISalesRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<ProductStock>> CalculateRestockingAsync(List<SaleTransaction> salesData)
    {
        return await _context.CalculateRestockPlanAsync(salesData);
    }
}
