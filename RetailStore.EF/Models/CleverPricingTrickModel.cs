namespace RetailStore.EF.Models;

public class CleverPricingTrickModel
{
    public class CompetitorPrice
    {
        public required string ProductID { get; set; }
        public required decimal Price { get; set; }
    }

    public class DemandTrend
    {
        public required string ProductID { get; set; }
        public required string Trend { get; set; }
    }

    public class UpdatedPrice
    {
        public required string ProductID { get; set; }
        public required decimal Price { get; set; }
    }
}
