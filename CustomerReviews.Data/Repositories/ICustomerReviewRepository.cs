using CustomerReviews.Data.Model;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Data.Repositories
{
    public interface ICustomerReviewRepository : IRepository
    {
        IQueryable<CustomerReviewEntity> CustomerReviews { get; }
        IQueryable<AverageProductRateEntity> AverageProductRates { get; }

        CustomerReviewEntity[] GetCustomerReviewsByIds(string[] ids);
        CustomerReviewEntity[] GetCustomerReviewsByProductId(string id);
        void DeleteCustomerReviews(string[] ids);

        AverageProductRateEntity[] GetAverageProductRatesByProductIds(string[] ids);
        void DeleteAverageProductRates(string[] ids);
    }
}