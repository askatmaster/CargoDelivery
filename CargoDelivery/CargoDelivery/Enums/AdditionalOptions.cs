using System.ComponentModel.DataAnnotations;
namespace CargoDelivery.Enums
{
    public enum AdditionalOptions
    {
        [Display(Name = "Срочная доставка")]
        ExpressDelivery = 0,

        [Display(Name = "Хрупкий продукт")]
        FragileProduct = 1,

        [Display(Name = "Скоропортящийся продукт")]
        PerishableProduct = 2,

        [Display(Name = "Токсичный продукт")]
        ToxicProduct = 3,

        [Display(Name = "Радиоактивный продукт")]
        Radioactive = 4,

        [Display(Name = "Вызрывоопасный продукт")]
        ExplosiveProduct = 5,

        [Display(Name = "Огнеопасный продукт")]
        FlammableProduct = 6,

        [Display(Name = "Биологическая опасность")]
        BiologicalHazard = 7,

        [Display(Name = "Военный груз")]
        MilitaryCargo = 8
    }
}
