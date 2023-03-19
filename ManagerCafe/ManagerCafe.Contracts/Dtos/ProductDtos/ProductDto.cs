using System.ComponentModel.DataAnnotations.Schema;
using ManagerCafe.Contracts.Dtos.InventoryDtos;

namespace ManagerCafe.Contracts.Dtos.ProductDtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal PriceBuy { get; set; }
        public decimal PriceSell { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public List<ProductInventoryDto> Inventories { get; set; }
    }
}
