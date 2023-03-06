using CargoDelivery.Interfaces;
namespace CargoDelivery.DeliveryMethods
{
    public class DeliveryByAviaTransport : IDelivery
    {
        public string DeliveryMethod()
        {
            return "Доставка авиатранспортом";
        }
    }
}
