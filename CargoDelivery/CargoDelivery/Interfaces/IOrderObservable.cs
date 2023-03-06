namespace CargoDelivery.Interfaces
{
    public interface IOrderObservable
    {
        void AddObserver(IOrderObserver o);
        void RemoveObserver(IOrderObserver o);
        void NotifyObservers();
    }
}
