using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace CustomerReviews.Data.Services
{
    public class CustomerReviewService : ServiceBase, ICustomerReviewService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;
        private readonly IAverageProductRateService _averageProductRateService;

        public CustomerReviewService(Func<ICustomerReviewRepository> repositoryFactory,
            IAverageProductRateService averageProductRateService)
        {
            _repositoryFactory = repositoryFactory;
            _averageProductRateService = averageProductRateService;
        }

        public CustomerReview[] GetByIds(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                return repository.GetCustomerReviewsByIds(ids)
                    .Select(x => x.ToModel(AbstractTypeFactory<CustomerReview>.TryCreateInstance())).ToArray();
            }
        }

        public void SaveCustomerReviews(CustomerReview[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var alreadyExistEntities =
                        repository.GetCustomerReviewsByIds(items.Where(m => !m.IsTransient()).Select(x => x.Id)
                            .ToArray());
                    foreach (var derivativeContract in items)
                    {
                        if (derivativeContract.Rate < 1 || derivativeContract.Rate > 5)
                            continue;

                        var sourceEntity = AbstractTypeFactory<CustomerReviewEntity>.TryCreateInstance()
                            .FromModel(derivativeContract, pkMap);
                        var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == sourceEntity.Id);
                        if (targetEntity != null)
                        {
                            changeTracker.Attach(targetEntity);
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            repository.Add(sourceEntity);
                            _averageProductRateService.SaveAverageProductRates(new AverageProductRate
                            {
                                ProductId = sourceEntity.ProductId,
                                AverageRate = sourceEntity.Rate
                            });
                        }
                    }

                    CommitChanges(repository);
                    pkMap.ResolvePrimaryKeys();
                    _averageProductRateService.RecountAverageProductRate();
                }
            }
        }

        public void DeleteCustomerReviews(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                repository.DeleteCustomerReviews(ids);
                CommitChanges(repository);
                _averageProductRateService.RecountAverageProductRate();
            }
        }
    }
}