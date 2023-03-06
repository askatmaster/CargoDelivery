using System;
using System.Collections.Generic;
using CargoDelivery.AbstractModels;
using CargoDelivery.Enums;
using CargoDelivery.Interfaces;
namespace CargoDelivery.Models
{
    public class DeliveryCompany : Company,
                                   IOrderObserver
    {
        public List<IDelivery> DeliveryMethods { get; set; }

        public DeliveryCompany(string name)
        {
            Name = name;
        }

        public void Update(string currentLocation, StateOfOrder currentStateOfOrder, string orderId)
        {
            Console.WriteLine("Оповещение компании");

            var status = currentStateOfOrder switch
            {
                StateOfOrder.Sent => "Заказ отправлен",
                StateOfOrder.Delivered => "Заказ доставлен",
                StateOfOrder.Canceled => "Заказ отменен",
                _ => "Доступ разрешен"
            };

            Console.WriteLine($"Заказ №: {orderId}\n" + $"Текущее местоположение: {currentLocation}\n" + $"Статус {status}");
            Console.WriteLine();
        }
    }
}