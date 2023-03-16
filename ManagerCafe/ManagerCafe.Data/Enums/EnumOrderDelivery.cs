using System.ComponentModel.DataAnnotations;

namespace ManagerCafe.Data.Enums
{
    public enum EnumOrderDelivery
    {
        [Display(Name = "Visit")]
        Visit = 1,
        [Display(Name = "Ship")]
        Ship = 2,
    }
}
