using CargoDelivery.Enums;
namespace CargoDelivery.Interfaces
{
    public interface IOrderObserver
    {
        void Update(string currentLocation, StateOfOrder currentStateOfOrder, string orderId);
    }
}
