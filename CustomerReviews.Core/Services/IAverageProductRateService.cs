using CustomerReviews.Core.Model;

namespace CustomerReviews.Core.Services
{
    public interface IAverageProductRateService
    {
        AverageProductRate[] GetByProductIds(string[] ids);

        void SaveAverageProductRates(AverageProductRate item);

        void DeleteAverageProductRates(string[] ids);
        void RecountAverageProductRate();
    }
}