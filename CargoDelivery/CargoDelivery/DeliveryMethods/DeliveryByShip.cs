using CargoDelivery.Interfaces;
namespace CargoDelivery.DeliveryMethods
{
    public class DeliveryByShip : IDelivery
    {
        public string DeliveryMethod()
        {
            return "Доставка на корабле";
        }
    }
}
