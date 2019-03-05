using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Core.Model
{
    public class CustomerReview : AuditableEntity
    {
        public string AuthorNickname { get; set; }
        public string Content { get; set; }
        public string ProductId { get; set; }
        public int Rate { get; set; }
    }
}