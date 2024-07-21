using static RetailStore.EF.Models.CleverPricingTrickModel;

namespace CleverPricingTrick.Repositorys;

public interface IPricingRepository
{
    Task<List<UpdatedPrice>> UpdatedPricesAsync(string competitorPricesJson, string demandTrendsJson);
}
