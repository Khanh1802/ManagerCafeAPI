﻿using System.ComponentModel.DataAnnotations;

namespace ManagerCafe.Data.Enums
{
    public enum EnumInventoryTransation
    {
        [Display(Name = "Nhập kho")]
        Import = 1,

        [Display(Name = "Xuất kho")]
        Export = 2,
    }
}
