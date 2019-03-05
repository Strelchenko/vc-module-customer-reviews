using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Repositories;
using System;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace CustomerReviews.Data.Services
{
    public class AverageProductRateSearchService : ServiceBase, IAverageProductRateSearchService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;
        private readonly IAverageProductRateService _averageProductRateService;

        public AverageProductRateSearchService(Func<ICustomerReviewRepository> repositoryFactory,
            IAverageProductRateService averageProductRateService)
        {
            _repositoryFactory = repositoryFactory;
            _averageProductRateService = averageProductRateService;
        }

        public GenericSearchResult<AverageProductRate> SearchAverageProductRate(
            AverageProductRateSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException($"{nameof(criteria)} must be set");
            }

            var retVal = new GenericSearchResult<AverageProductRate>();

            using (var repository = _repositoryFactory())
            {
                var query = repository.AverageProductRates;

                if (!criteria.ProductIds.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.ProductIds.Contains(x.ProductId));
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[]
                        {new SortInfo {SortColumn = "ProductId", SortDirection = SortDirection.Descending}};
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();

                var averageProductRateIds = query.Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .Select(x => x.ProductId)
                    .ToList();

                retVal.Results = _averageProductRateService.GetByProductIds(averageProductRateIds.ToArray())
                    .OrderBy(x => averageProductRateIds.IndexOf(x.ProductId)).ToList();
                return retVal;
            }
        }
    }
}