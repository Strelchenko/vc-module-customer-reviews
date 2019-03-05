using CustomerReviews.Data.Model;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace CustomerReviews.Data.Repositories
{
    public class CustomerReviewRepository : EFRepositoryBase, ICustomerReviewRepository
    {
        public CustomerReviewRepository()
        {
        }

        public CustomerReviewRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }


        #region CustomerReview

        public IQueryable<CustomerReviewEntity> CustomerReviews => GetAsQueryable<CustomerReviewEntity>();

        public CustomerReviewEntity[] GetCustomerReviewsByIds(string[] ids)
        {
            return CustomerReviews.Where(x => ids.Contains(x.Id)).ToArray();
        }

        public CustomerReviewEntity[] GetCustomerReviewsByProductId(string id)
        {
            return CustomerReviews.Where(x => x.ProductId == id).ToArray();
        }

        public void DeleteCustomerReviews(string[] ids)
        {
            var items = GetCustomerReviewsByIds(ids);
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        #endregion

        #region AverageProductRate

        public IQueryable<AverageProductRateEntity> AverageProductRates => GetAsQueryable<AverageProductRateEntity>();

        public AverageProductRateEntity[] GetAverageProductRatesByProductIds(string[] ids)
        {
            return AverageProductRates.Where(x => ids.Contains(x.ProductId)).ToArray();
        }

        public void DeleteAverageProductRates(string[] ids)
        {
            var items = GetAverageProductRatesByProductIds(ids);
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerReviewEntity>().ToTable("CustomerReview").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<AverageProductRateEntity>().ToTable("AverageProductRate").HasKey(x => x.ProductId)
                .Property(x => x.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}