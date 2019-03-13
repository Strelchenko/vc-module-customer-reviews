using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Migrations;
using CustomerReviews.Data.Repositories;
using CustomerReviews.Data.Services;
using System;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Testing.Bases;
using Xunit;

namespace CustomerReviews.Test
{
    public class CustomerReviewsTests : FunctionalTestBase
    {
        private const string ProductId = "testProductId";
        private const string CustomerReviewId = "testId";

        public CustomerReviewsTests()
        {
            ConnectionString = "VirtoCommerce";
        }

        [Fact]
        public void CanDoCRUDandSearch()
        {
            // Read non-existing item
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);

            // Create
            var item = new CustomerReview
            {
                Id = CustomerReviewId,
                ProductId = ProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "initial data seed",
                AuthorNickname = "John Doe",
                Content = "Liked that",
                Rate = 5
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });

            getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(CustomerReviewId, item.Id);

            // Update
            var updatedContent = "Updated content";
            Assert.NotEqual(updatedContent, item.Content);

            item.Content = updatedContent;
            CustomerReviewService.SaveCustomerReviews(new[] { item });
            getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(updatedContent, item.Content);

            // Search
            Assert.Throws<ArgumentNullException>(() => CustomerReviewSearchService.SearchCustomerReviews(null));

            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { ProductId } };
            var searchResult = CustomerReviewSearchService.SearchCustomerReviews(criteria);

            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.TotalCount);
            Assert.Single(searchResult.Results);

            // Delete
            CanDeleteCustomerReviews();
        }

        [Fact]
        public void CanDeleteCustomerReviews()
        {
            CustomerReviewService.DeleteCustomerReviews(new[] { CustomerReviewId });

            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        [Fact]
        public void CanRecountAverageProductRate()
        {
            // Create review for test
            var item = new CustomerReview
            {
                Id = CustomerReviewId,
                ProductId = ProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "initial data seed",
                AuthorNickname = "John Doe",
                Content = "Liked that",
                Rate = 5
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });

            // Check result
            var getByIdsResult = AverageProductRateService.GetByProductIds(new[] { ProductId });
            Assert.Single(getByIdsResult);

            var avgRate = getByIdsResult[0];
            Assert.Equal(ProductId, avgRate.ProductId);
            Assert.Equal(item.Rate, avgRate.AverageRate);

            // Delete test review
            CanDeleteCustomerReviews();
        }

        [Fact]
        public void CanNotSaveCustomerReviews()
        {
            // Try create review for test
            var item = new CustomerReview
            {
                Id = CustomerReviewId,
                ProductId = ProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "initial data seed",
                AuthorNickname = "John Doe",
                Content = "Liked that",
                Rate = 10
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });

            // Check result
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        private ICustomerReviewSearchService CustomerReviewSearchService
        {
            get { return new CustomerReviewSearchService(GetRepository, CustomerReviewService); }
        }

        protected IAverageProductRateService AverageProductRateService
        {
            get { return new AverageProductRateService(GetRepository); }
        }

        private ICustomerReviewService CustomerReviewService
        {
            get { return new CustomerReviewService(GetRepository, AverageProductRateService); }
        }

        protected ICustomerReviewRepository GetRepository()
        {
            var repository = new CustomerReviewRepository(ConnectionString, new EntityPrimaryKeyGeneratorInterceptor(),
                new AuditableInterceptor(null));
            EnsureDatabaseInitialized(() => new CustomerReviewRepository(ConnectionString),
                () => Database.SetInitializer(new SetupDatabaseInitializer<CustomerReviewRepository, Configuration>()));
            return repository;
        }
    }
}