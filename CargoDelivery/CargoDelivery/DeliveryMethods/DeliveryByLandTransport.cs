using CargoDelivery.Interfaces;
namespace CargoDelivery.DeliveryMethods
{
    public class DeliveryByLandTransport : IDelivery
    {
        public string DeliveryMethod()
        {
            return "Доставка наземным транспортом";
        }
    }
}
