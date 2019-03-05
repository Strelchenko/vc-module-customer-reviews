using CustomerReviews.Core.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerReviews.Data.Model
{
    public class AverageProductRateEntity
    {
        public float AverageRate { get; set; }

        [Required]
        [StringLength(128)]
        public string ProductId { get; set; }

        public virtual AverageProductRate ToModel(AverageProductRate averageProductRate)
        {
            if (averageProductRate == null)
                throw new ArgumentNullException(nameof(averageProductRate));

            averageProductRate.AverageRate = AverageRate;
            averageProductRate.ProductId = ProductId;

            return averageProductRate;
        }

        public virtual AverageProductRateEntity FromModel(AverageProductRate averageProductRate)
        {
            if (averageProductRate == null)
                throw new ArgumentNullException(nameof(averageProductRate));

            AverageRate = averageProductRate.AverageRate;
            ProductId = averageProductRate.ProductId;

            return this;
        }

        public virtual void Patch(AverageProductRateEntity target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.AverageRate = AverageRate;
        }
    }
}