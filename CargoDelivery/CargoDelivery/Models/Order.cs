using System;
using System.Collections.Generic;
using CargoDelivery.AbstractModels;
using CargoDelivery.Enums;
using CargoDelivery.Interfaces;
namespace CargoDelivery.Models
{
    public class Order<TCompany, TClient, TProduct, TDeliveryMethod> : IOrderObservable
        where TCompany : DeliveryCompany
        where TClient  : Client
        where TProduct : Product
        where TDeliveryMethod : IDelivery
    {
        public Guid Id { get; set; }

        public List<TProduct> Products { get; set; }

        public TClient Client { get; set; }

        public TCompany Deliveryman { get; set; }

        public string AddressOfDelivery { get; set; }

        private string CurrentLocationOfOrder { get; set; }

        private StateOfOrder StateOfOrder { get; set; }

        public TDeliveryMethod DeliveryMethod { get; set; }

        public List<AdditionalOptions> AdditionalOptions { get; set; }

        public List<IOrderObserver> Observers { get; set; } = new List<IOrderObserver>();

        public string GetCurrentLocationOfOrder() => CurrentLocationOfOrder;

        public StateOfOrder GetStateOfOrder() => StateOfOrder;

        public void SetCurrentLocationOfOrder(string currentLocation)
        {
            CurrentLocationOfOrder = currentLocation;

            if (CurrentLocationOfOrder == AddressOfDelivery)
                StateOfOrder = StateOfOrder.Delivered;

            NotifyObservers();
        }

        public void SendOrder()
        {
            StateOfOrder = StateOfOrder.Sent;
            NotifyObservers();
        }

        public void CancelOrder()
        {
            StateOfOrder = StateOfOrder.Canceled;
            NotifyObservers();
        }

        public void DeliveredOrder()
        {
            StateOfOrder = StateOfOrder.Delivered;
            NotifyObservers();
        }

        public void AddObserver(IOrderObserver newOrderObserver) => Observers.Add(newOrderObserver);

        public void RemoveObserver(IOrderObserver oldOrderObserver) => Observers.Remove(oldOrderObserver);


        public void NotifyObservers()
        {
            foreach (var observer in Observers)
                observer.Update(CurrentLocationOfOrder, StateOfOrder, Id.ToString());
        }
    }
}
