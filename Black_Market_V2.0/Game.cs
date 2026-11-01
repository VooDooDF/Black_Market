using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Game
    {
        static ProductCatalog catalog = new ProductCatalog();
        static Random random = new Random();
        static Player player = new Player("Player", 1000, catalog);
        static Market market = new Market(catalog);

        static int currentDay = 1;
        static int maxDays = 10;

        static private void ShowMarket()
        {
            Console.WriteLine("BLACK MARKET");
            Console.WriteLine($"День: {currentDay}/{maxDays}");
            Console.WriteLine($"Баланс: {player.Money}$");
            Console.WriteLine($"Инвентарь: {player.Inventory.CurrentCount()}/{player.Inventory.Capacity}");
            Console.WriteLine();
            Console.WriteLine("====РЫНОК====");
            for (int i = 0; i < market.GetProductsOnMarket(catalog).Count; i++)
            {
                var product = market.GetProductsOnMarket(catalog)[i];
                Console.WriteLine($"{i + 1}. {product.Name} - {product.Price}$ - {market.GetProductQuantity(product.Id)} шт.");
            }
        }

        static public void Start()
        {
            while (true)
            {
                Console.WriteLine("\n==ГЛАВНОЕ МЕНЮ==");
                Console.WriteLine("1. Посмотреть рынок");
                Console.WriteLine("2. Купить товар");
                Console.WriteLine("3. Продать товар");
                Console.WriteLine("4. Мой инвентарь");
                Console.WriteLine("5. Следующий день");
                Console.WriteLine("0. Выйти из игры");

                Console.Write("Выберите действие: ");

                int choice = int.TryParse(Console.ReadLine(), out choice) ? choice : -1;
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        ShowMarket();
                        break;
                    case 2:
                        Console.Clear();
                        BuyProduct();
                        break;
                    case 3:
                        Console.Clear();
                        SellProduct();
                        break;
                    case 4:
                        Console.Clear();
                        ShowInventory();
                        break;
                    case 5:
                        Console.Clear();
                        if (NextDay())
                        {
                            return;
                        }
                        break;
                    case 0:
                        Console.WriteLine("Выход из игры...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static public void BuyProduct()
        {
            int productIndex;
            int quantity;

            Console.WriteLine("Выберите товар для покупки:");
            var productsOnMarket = market.GetProductsOnMarket(catalog);
            for (int i = 0; i < productsOnMarket.Count; i++)
            {
                var product = productsOnMarket[i];
                Console.WriteLine($"{i + 1}. {product.Name} - {product.Price}$ - {market.GetProductQuantity(product.Id)} шт.");
            }

            Console.Write("Введите номер товара: ");
            if (!int.TryParse(Console.ReadLine(), out productIndex))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }
            productIndex -= 1;
            
            if (productIndex < 0 || productIndex >= productsOnMarket.Count)
            {
                Console.WriteLine("Неверный номер товара.");
                return;
            }

            Console.Write("Введите количество для покупки: ");
            if (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }

            Guid productId = productsOnMarket[productIndex].Id;

            Console.WriteLine(market.TryBuy(player, productId, quantity));
        }

        static public void SellProduct()
        {
            int productIndex;
            int quantity;

            Console.WriteLine("Выберите товар для продажи:");
            var productsOnMarket = market.GetProductsOnMarket(catalog);
            for (int i = 0; i < productsOnMarket.Count; i++)
            {
                var product = productsOnMarket[i];
                Console.WriteLine($"{i + 1}. {product.Name} - {product.Price}$ - {market.GetProductQuantity(product.Id)} шт.");
            }

            Console.Write("Введите номер товара: ");
            if (!int.TryParse(Console.ReadLine(), out productIndex))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }
            productIndex -= 1;

            Console.Write("Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }

            Guid productId = productsOnMarket[productIndex].Id;

            Console.WriteLine(market.TrySell(player, productId, quantity));
        }

        static public void ShowInventory()
        {
            Console.WriteLine($"Баланс: {player.Money}$");
            Console.WriteLine($"Занимаемый объем: {player.Inventory.CurrentCount()}/{player.Inventory.Capacity}");
            Console.WriteLine("Ваш инвентарь:");
            for (int i = 0; i < player.Inventory.GetProductsOnInventory().Count; i++)
            {
                var product = player.Inventory.GetProductsOnInventory()[i];
                if (player.Inventory.GetQuantity(product.Id) > 0)
                {
                    Console.WriteLine($"{i + 1}. {product.Name} - {product.Price}$ - {player.Inventory.GetQuantity(product.Id)} шт.");
                }
            }
        }

        static public bool NextDay()
        {
            if (currentDay >= maxDays)
            {
                EndGame();
                return true;
            }

            currentDay++;

            player.Inventory.IncreaseCapacity();
            //RandomEvent();

            Console.WriteLine($"Наступил день {currentDay}");
            return false;
        }

        

        static public void EndGame()
        {
            int balance = (int)player.Money;
            var productsOnMarket = market.GetProductsOnMarket(catalog);
            for (int i = 0; i < productsOnMarket.Count; i++)
            {
                var product = productsOnMarket[i];
                balance += (int)(product.Price * market.GetProductQuantity(product.Id));
                market.RemoveProduct(product.Id, market.GetProductQuantity(product.Id));
            }

            if (balance >= 2000)
            {
                Console.WriteLine("Поздравляем! Вы достигли цели и заработали больше 2000$.");
                Console.WriteLine($"Ваш итоговый баланс: {balance}$");
                return;
            }

            Console.WriteLine("Игра окончена! Вы достигли максимального количества дней.");
            Console.WriteLine($"Ваш итоговый баланс: {balance}$");
            return;
        }
    }
}
