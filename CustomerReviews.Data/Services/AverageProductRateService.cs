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
    public class AverageProductRateService : ServiceBase, IAverageProductRateService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;

        public AverageProductRateService(Func<ICustomerReviewRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public AverageProductRate[] GetByProductIds(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                return repository.GetAverageProductRatesByProductIds(ids).Select(x =>
                    x.ToModel(AbstractTypeFactory<AverageProductRate>.TryCreateInstance())).ToArray();
            }
        }

        public void SaveAverageProductRates(AverageProductRate item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var alreadyExistEntities = repository.GetAverageProductRatesByProductIds(new[] { item.ProductId });

                    var sourceEntity = AbstractTypeFactory<AverageProductRateEntity>.TryCreateInstance()
                        .FromModel(item);
                    var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.ProductId == sourceEntity.ProductId);
                    if (targetEntity != null)
                    {
                        changeTracker.Attach(targetEntity);
                        sourceEntity.Patch(targetEntity);
                    }
                    else
                    {
                        repository.Add(sourceEntity);
                    }

                    CommitChanges(repository);
                }
            }
        }

        public void RecountAverageProductRate()
        {
            using (var repository = _repositoryFactory())
            {
                var averageRates = repository.AverageProductRates;

                foreach (var averageRate in averageRates)
                {
                    var reviews = repository.GetCustomerReviewsByProductId(averageRate.ProductId)
                        .Select(x => x.Rate).ToArray();

                    if (reviews.Any())
                    {
                        averageRate.AverageRate = (float)reviews.Average();
                    }
                    else
                    {
                        repository.DeleteAverageProductRates(new[] { averageRate.ProductId });
                    }
                }

                CommitChanges(repository);
            }
        }
    }
}