using VirtoCommerce.Domain.Commerce.Model.Search;

namespace CustomerReviews.Core.Model
{
    public class AverageProductRateSearchCriteria : SearchCriteriaBase
    {
        public string[] ProductIds { get; set; }
    }
}