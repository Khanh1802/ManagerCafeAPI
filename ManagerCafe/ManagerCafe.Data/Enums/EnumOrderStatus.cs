using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ManagerCafe.Data.Enums
{
    public enum EnumOrderStatus
    {
        [Display(Name = "Unpaid")]
        Unpaid = 1,
        [Display(Name = "Paid")]
        Paid = 2,
    }
}
