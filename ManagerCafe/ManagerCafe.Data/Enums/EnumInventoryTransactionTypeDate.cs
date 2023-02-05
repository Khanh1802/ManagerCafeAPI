using System.ComponentModel.DataAnnotations;

namespace ManagerCafe.Data.Enums
{
    public enum EnumInventoryTransactionTypeDate
    {
        [Display(Name = "Ngày")]
        Day = 1,

        [Display(Name = "Tháng")]
        Month = 2,

        [Display(Name = "Năm")]
        Year = 3,
    }
}
