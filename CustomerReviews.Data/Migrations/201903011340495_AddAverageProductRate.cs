namespace CustomerReviews.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAverageProductRate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AverageProductRate",
                c => new
                {
                    ProductId = c.String(nullable: false, maxLength: 128),
                    AverageRate = c.Single(nullable: false),
                })
                .PrimaryKey(t => t.ProductId);

            AddColumn("dbo.CustomerReview", "Rate", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerReview", "IsActive");
        }
    }
}
