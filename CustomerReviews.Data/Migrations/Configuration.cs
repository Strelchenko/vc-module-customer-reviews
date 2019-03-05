using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using System;
using System.Data.Entity.Migrations;

namespace CustomerReviews.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<CustomerReviewRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(CustomerReviewRepository context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var now = DateTime.UtcNow;
            context.AddOrUpdate(new CustomerReviewEntity { Id = "1", ProductId = "3e716e66afd342e0a9c49e9bb2739a7d", CreatedDate = now, CreatedBy = "initial data seed", AuthorNickname = "Andrew Peters", Content = "Super!", Rate = 5 });
            context.AddOrUpdate(new CustomerReviewEntity { Id = "2", ProductId = "3e716e66afd342e0a9c49e9bb2739a7d", CreatedDate = now, CreatedBy = "initial data seed", AuthorNickname = "Mr. Pumpkin", Content = "So so", Rate = 3 });
            context.AddOrUpdate(new CustomerReviewEntity { Id = "3", ProductId = "3e716e66afd342e0a9c49e9bb2739a7d", CreatedDate = now, CreatedBy = "initial data seed", AuthorNickname = "John Doe", Content = "Liked that", Rate = 4 });
        }
    }
}
