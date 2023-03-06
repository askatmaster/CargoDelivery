using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CargoDelivery.AbstractModels;
using CargoDelivery.DeliveryMethods;
using CargoDelivery.Enums;
using CargoDelivery.Helpers;
using CargoDelivery.Interfaces;
using CargoDelivery.Models;
namespace CargoDelivery
{
    class Program
    {
        private static void Main(string[] args)
        {
            var companies = CreateDeliveryCompanies();

            var products = CreateProducts();

            var client = RegisterClient();

            bool orderSent;
            do
            {
                var selectedCompany = SelectCompany(companies, client);

                var selectedDeliveryMethod = SelectDeliveryMethod(selectedCompany);

                var selectedProducts = SelectProducts(products);

                var selectAddress = SelectAddress();

                var selectedAdditionalOptions = SelectAdditionalOptions();

                var order = CreateOrder(selectedCompany, client, selectedProducts, selectedDeliveryMethod, selectedAdditionalOptions, selectAddress);

                var comfirm = ConfirmOrder(order);

                if (comfirm)
                {
                    order.SendOrder();
                    orderSent = true;
                }
                else
                {
                    orderSent = false;
                }
                Console.Clear();
            } while (!orderSent);
        }

        private static bool ConfirmOrder<TCompany, TClient, TProduct, TDeliveryMethod>(Order<TCompany, TClient, TProduct, TDeliveryMethod> order)
            where TCompany : DeliveryCompany
            where TClient : Client
            where TProduct : Product
            where TDeliveryMethod : IDelivery
        {
            bool result;
            do
            {
                Console.WriteLine("Ваш заказ!");
                Console.WriteLine($"ID Заказа: {order.Id}");
                Console.WriteLine($"Компания доставки: {order.Deliveryman.Name}");
                Console.WriteLine($"Адрес доставки: {order.AddressOfDelivery}");
                Console.WriteLine($"Клиент: {order.Client.Name}");
                Console.WriteLine($"Способ доставки: {order.DeliveryMethod.DeliveryMethod()}");
                Console.Write("Дополнительные опции: ");
                if (order.AdditionalOptions != null)
                    foreach (var option in order.AdditionalOptions)
                        Console.Write($"{EnumHelper<AdditionalOptions>.GetDisplayValue(option)}, ");
                Console.WriteLine();
                Console.WriteLine($"Текущее местоположение: {order.GetCurrentLocationOfOrder()}");
                Console.Write("Элементы заказа: ");
                foreach (var product in order.Products)
                    Console.Write($"{product.Name}, ");

                Console.WriteLine();
                Console.Write("Подтвердить заказ? (да/нет): ");
                var check = Console.ReadLine();
                switch(check)
                {
                    case "да":
                        result = true;

                        break;
                    case "нет":
                        return false;
                    default:
                        result = false;

                        break;
                }

                Console.Clear();
            } while (result == false);

            return result;
        }

        private static string SelectAddress()
        {
            Console.WriteLine("Укажите адрес доставки!");
            Console.Write("Адрес: ");

            var addess = Console.ReadLine();

            Console.Clear();

            return addess;
        }

        private static Order<TCompany, TClient, TProduct, TDeliveryMethod> CreateOrder<TCompany, TClient, TProduct, TDeliveryMethod>(TCompany company, TClient client, List<TProduct> products, TDeliveryMethod deliveryMethod, List<AdditionalOptions> options, string address)
            where TCompany : DeliveryCompany
            where TClient : Client
            where TProduct : Product
            where TDeliveryMethod : IDelivery
        {
            var order = new Order<TCompany, TClient, TProduct, TDeliveryMethod>
            {
                Id = Guid.NewGuid(),
                Client = client,
                Deliveryman = company,
                DeliveryMethod = deliveryMethod,
                Products = products
            };
            order.AddObserver(company);
            order.AddObserver(client);
            order.AdditionalOptions = options;
            order.AddressOfDelivery = address;
            order.SetCurrentLocationOfOrder("Кыргызстан, г.Бишкек, ул.Турусбекова 144");

            Console.Clear();
            return order;
        }

        private static List<AdditionalOptions> SelectAdditionalOptions()
        {
            var selectedOptions = new List<AdditionalOptions>();

            do
            {
                var numOfProduct = 0;

                Console.WriteLine("Укажите дополнительные опции:");
                foreach (var option in Enum.GetValues<AdditionalOptions>())
                    Console.WriteLine($"{numOfProduct++}. {EnumHelper<AdditionalOptions>.GetDisplayValue(option)}");

                Console.Write("Укажите номера опций(через запятую): ");

                var nums = Console.ReadLine();
                if (string.IsNullOrEmpty(nums))
                    return null;

                var numsOfSelectedOptions = nums.Split(',');

                try
                {
                    selectedOptions.AddRange(numsOfSelectedOptions.Select(num => Enum.GetValues<AdditionalOptions>()[int.Parse(num)]));
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Ошибка, повторите попытку");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            } while (!selectedOptions.Any());

            return selectedOptions;
        }

        private static List<Product> SelectProducts(List<Product> products)
        {
            var selectedProducts = new List<Product>();
            do
            {
                var numOfProduct = 0;

                Console.WriteLine("Выберите товары для доставки:");
                foreach (var product in products)
                    Console.WriteLine($"{numOfProduct++}. {product.Name}");

                Console.Write("Укажите номера продуктов(через запятую): ");
                var numsOfSelectedProducts = Console.ReadLine()?.Split(',');

                try
                {
                    selectedProducts.AddRange(numsOfSelectedProducts.Select(num => products[int.Parse(num)]));
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Ошибка, повторите попытку");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                Console.Clear();
            } while (!selectedProducts.Any());

            return selectedProducts;
        }

        public static IDelivery SelectDeliveryMethod<TCompany>(TCompany selectedCompany) where TCompany : DeliveryCompany
        {
            IDelivery selectedMethod = null;
            do
            {
                Console.WriteLine($"Компания {selectedCompany.Name} предостовляет следующие способы доставки:");

                var numOfMethod = 0;

                foreach (var method in selectedCompany.DeliveryMethods)
                    Console.WriteLine($"{numOfMethod++}. {method.DeliveryMethod()}");

                try
                {
                    Console.Write("Выберите способ доставки(номер): ");
                    var numOfSelectedMethod = int.Parse(Console.ReadLine());
                    selectedMethod = selectedCompany.DeliveryMethods[numOfSelectedMethod];
                    if (selectedMethod == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Выбран неверный метод!");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Ошибка, повторите попытку");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                Console.Clear();
            } while (selectedMethod == null);

            Console.Clear();

            return selectedMethod;
        }

        private static TCompany SelectCompany<TCompany, TUser>(List<TCompany> companies, TUser client) where TCompany : Company where TUser : User
        {
            TCompany selectCompany;

            do
            {
                Console.WriteLine($"Добро пожаловать,{client.Name}!");
                Console.WriteLine();

                Console.WriteLine("Выберите компанию для доставки груза");
                foreach (var сompany in companies)
                    Console.WriteLine($"-{сompany.Name}");

                Console.WriteLine();

                Console.Write("Введите название: ");

                var companyName = Console.ReadLine()?.ToLower();

                selectCompany = companies.FirstOrDefault(x => x.Name.ToLower() == companyName);

                if (selectCompany != null)
                    continue;

                Console.Clear();
                Console.WriteLine("Выбрана неверная компания!");
                Thread.Sleep(1000);
                Console.Clear();
            } while (selectCompany == null);

            Console.Clear();

            return selectCompany;
        }

        public static Client RegisterClient()
        {
            Console.WriteLine("Введите ваше имя");

            var clientName = Console.ReadLine();
            var client = new Client(clientName);
            Console.Clear();

            return client;
        }

        public static List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Phone("IPhone"),
                new GraphicsCard("Radeon RX 580"),
                new Laptop("Xiaomi Mi Air 13")
            };
        }

        public static List<DeliveryCompany> CreateDeliveryCompanies()
        {
            return new List<DeliveryCompany>
            {
                new DeliveryCompany("Yandex")
                {
                    DeliveryMethods = new List<IDelivery>
                    {
                        new DeliveryOnFoot(),
                        new DeliveryByLandTransport()
                    }
                },
                new DeliveryCompany("Delivery Club")
                {
                    DeliveryMethods = new List<IDelivery>
                    {
                    new DeliveryOnFoot(),
                    new DeliveryByAviaTransport()
                    }
                },
                new DeliveryCompany("Federal Express")
                {
                    DeliveryMethods = new List<IDelivery>
                    {
                        new DeliveryOnFoot(),
                        new DeliveryByAviaTransport(),
                        new DeliveryByLandTransport(),
                        new DeliveryByShip()
                    }
                },
                new DeliveryCompany("Akuma")
                {
                    DeliveryMethods = new List<IDelivery>
                    {
                        new DeliveryByLandTransport()
                    }
                }
            };
        }
    }
}