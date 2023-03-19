using ManagerCafe.Data.Enums;
using ManagerCafe.Share.Commons;
using System.ComponentModel.DataAnnotations;

namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class FilterProductDto : PaginationDto //, IValidatableObject
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }

        [Required]
        public EnumProductFilter? Choice { get; set; }
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Choice == null)
        //    {
        //        yield return new ValidationResult("Requied Choice", new[] { nameof(Choice) });
        //    }
        //}
    }
}
