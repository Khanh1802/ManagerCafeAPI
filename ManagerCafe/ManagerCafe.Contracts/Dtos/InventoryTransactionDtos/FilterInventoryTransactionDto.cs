﻿using ManagerCafe.Commons;
using ManagerCafe.Data.Enums;

namespace ManagerCafe.Contracts.Dtos.InventoryTransactionDtos
{
    public class FilterInventoryTransactionDto : PaginationDto
    {
        public EnumInventoryTransation Type { get; set; }
        public EnumInventoryTransactionTypeDate TypeDate { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductName { get; set; }
    }
}
