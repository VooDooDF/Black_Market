using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Black_Market
{
    internal class Event
    {
        Random random = new Random();

        public void RandomEvent(Market market, Player player)
        {
            int randomEvent = random.Next(1, 4);
            var products = market.GetProductsOnMarket();
            int randomProductIndex = random.Next(0, products.Count);
            Guid productId = products[randomProductIndex].Id;

            switch (randomEvent)
            {
                case 1:
                    Console.WriteLine($"Случайное событие! \nТовар {products[randomProductIndex].Name} стал дороже на 30%!");
                    market.ChangePrices(productId, 1.3m);
                    break;
                case 2:
                    Console.WriteLine($"Случайное событие! \nТовар {products[randomProductIndex].Name} стал дешевле на 30%!");
                    market.ChangePrices(productId, 0.7m);
                    break;
                case 3:
                    Console.WriteLine($"Случайное событие! \nВы получаете товар {products[randomProductIndex].Name}!");
                    player.Inventory.AddProduct(productId, 1);
                    break;
                default:
                    Console.WriteLine("Нет случайного события сегодня.");
                    break;
            }
        }
    }
}
