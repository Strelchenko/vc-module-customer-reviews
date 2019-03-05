using CustomerReviews.Core.Model;
using VirtoCommerce.Domain.Commerce.Model.Search;

namespace CustomerReviews.Core.Services
{
    public interface IAverageProductRateSearchService
    {
        GenericSearchResult<AverageProductRate> SearchAverageProductRate(AverageProductRateSearchCriteria criteria);
    }
}