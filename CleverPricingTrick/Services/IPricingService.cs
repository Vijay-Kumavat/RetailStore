using static RetailStore.EF.Models.CleverPricingTrickModel;

namespace CleverPricingTrick.Services;

public interface IPricingService
{
    Task<List<UpdatedPrice>> UpdatedPricesAsync(List<CompetitorPrice> competitorPrices, List<DemandTrend> demandTrends);
}
