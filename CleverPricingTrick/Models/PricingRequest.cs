using static RetailStore.EF.Models.CleverPricingTrickModel;

namespace CleverPricingTrick.Models;

public class PricingRequest
{
    public required List<CompetitorPrice> CompetitorPrices { get; set; }
    public required List<DemandTrend> DemandTrends { get; set; }
}
