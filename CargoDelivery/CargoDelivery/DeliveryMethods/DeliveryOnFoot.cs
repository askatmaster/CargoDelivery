using CargoDelivery.Interfaces;
namespace CargoDelivery.DeliveryMethods
{
    public class DeliveryOnFoot : IDelivery
    {
        public string DeliveryMethod()
        {
            return "Доставка пешком";
        }
    }
}
