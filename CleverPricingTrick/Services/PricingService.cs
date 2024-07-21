using CleverPricingTrick.Repositorys;
using Newtonsoft.Json;
using static RetailStore.EF.Models.CleverPricingTrickModel;

namespace CleverPricingTrick.Services;

public class PricingService(IPricingRepository pricingRepository) : IPricingService
{
    private readonly IPricingRepository _pricingRepository = pricingRepository;

    public async Task<List<UpdatedPrice>> UpdatedPricesAsync(List<CompetitorPrice> competitorPrices, List<DemandTrend> demandTrends)
    {
        var competitorPricesJson = JsonConvert.SerializeObject(competitorPrices);
        var demandTrendsJson = JsonConvert.SerializeObject(demandTrends);

        return await _pricingRepository.UpdatedPricesAsync(competitorPricesJson, demandTrendsJson);
    }
}
