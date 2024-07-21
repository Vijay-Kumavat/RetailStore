using RetailStore.EF.Data;

namespace CleverPricingTrick.Repositorys
{
    public class PricingRepository(AppDbContext context) : IPricingRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<RetailStore.EF.Models.CleverPricingTrickModel.UpdatedPrice>> UpdatedPricesAsync(string competitorPricesJson, string demandTrendsJson)
        {
            return await _context.UpdatedPricesPlanAsync(competitorPricesJson, demandTrendsJson);
        }
    }
}
