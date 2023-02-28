using System.ComponentModel.DataAnnotations;

namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class CreateProductDto : IValidatableObject
    {
        [Required]
        public string Name { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PriceBuy > PriceSell)
            {
                yield return new ValidationResult("Price buy must greater than price sell", 
                    new[] { nameof(PriceBuy), nameof(PriceSell) });
            }
        }
    }
}
