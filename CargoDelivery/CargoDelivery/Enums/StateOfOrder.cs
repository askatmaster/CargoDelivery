using System.ComponentModel.DataAnnotations;
namespace CargoDelivery.Enums
{
    public enum StateOfOrder
    {
        [Display(Name = "Отправлен")]
        Sent = 1,

        [Display(Name = "Отменен")]
        Canceled = 2,

        [Display(Name = "Доставлен")]
        Delivered = 3
    }
}
