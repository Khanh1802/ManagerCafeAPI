using System.ComponentModel.DataAnnotations;

namespace ManagerCafe.Data.Enums
{
    public enum EnumWareHouseFilter
    {
        [Display(Name = "Ngày tăng dần")]
        DateAsc = 0,

        [Display(Name = "Ngày giảm dần")]
        DateDesc = 1,
    }
}
