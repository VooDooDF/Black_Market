using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Game
    {
        static Random random = new Random();
        static Player player = new Player();
        static Market market = new Market();

        static int currentDay = 1;
        static int maxDays = 10;

        static private void ShowMarket()
        {
            int inventoryCount = 0;
            foreach (var product in market.products)
            {
                inventoryCount += product.quantityOnPlayer;
            }
            Console.WriteLine("BLACK MARKET");
            Console.WriteLine($"День: {currentDay}/{maxDays}");
            Console.WriteLine($"Баланс: {player.Money}$");
            Console.WriteLine($"Инвентарь: {inventoryCount}/{player.InventoryCapacity}");
            Console.WriteLine();
            Console.WriteLine("====РЫНОК====");
            for (int i = 0; i < market.products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {market.products[i].name} - {market.products[i].price}$ - {market.products[i].quantityOnMarket}");
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
            for (int i = 0; i < market.products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {market.products[i].name} - {market.products[i].price}$ - {market.products[i].quantityOnMarket} шт.");
            }

            Console.Write("Введите номер товара: ");
            if (!int.TryParse(Console.ReadLine(), out productIndex))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }
            productIndex -= 1;

            Console.Write("Введите количество для покупки: ");
            if (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }

            Console.WriteLine(market.TryBuy(player, productIndex, quantity));
        }

        static public void SellProduct()
        {
            int productIndex;
            int quantity;

            Console.WriteLine("Выберите товар для продажи:");
            for (int i = 0; i < market.products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {market.products[i].name} - {market.products[i].price}$ - {market.products[i].quantityOnPlayer} шт.");
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

            Console.WriteLine(market.TrySell(player, productIndex, quantity));
        }

        static public void ShowInventory()
        {
            int inventoryCount = 0;
            foreach (var product in market.products)
            {
                inventoryCount += product.quantityOnPlayer;
            }

            Console.WriteLine($"Баланс: {player.Money}$");
            Console.WriteLine($"Занимаемый объем: {inventoryCount}/{player.InventoryCapacity}");
            Console.WriteLine("Ваш инвентарь:");
            for (int i = 0; i < market.products.Count; i++)
            {
                if (market.products[i].quantityOnPlayer > 0)
                {
                    Console.WriteLine($"{i + 1}. {market.products[i].name} - {market.products[i].price}$ - {market.products[i].quantityOnPlayer} шт.");
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

            player.IncreaseInventoryCapacity();
            market.ChangePrices();
            RandomEvent();
            market.ChangeQuantities();

            Console.WriteLine($"Наступил день {currentDay}");
            return false;
        }

        static public void RandomEvent() 
        {
            int randomEvent = random.Next(1, 4);
            int randomProductIndex = random.Next(0, market.products.Count);

            switch (randomEvent)
            {
                case 1:
                    Console.WriteLine($"Случайное событие! \nТовар {market.products[randomProductIndex].name} стал дороже на 30%!");
                    market.MarketEvent(randomProductIndex, 1.3);
                    break;
                case 2:
                    Console.WriteLine($"Случайное событие! \nТовар {market.products[randomProductIndex].name} стал дешевле на 30%!");
                    market.MarketEvent(randomProductIndex, 0.7);
                    break;
                case 3:
                    Console.WriteLine($"Случайное событие! \nВы получаете товар {market.products[randomProductIndex].name}!");
                    Console.WriteLine(market.GetProduct(player, randomProductIndex));
                    break;
                default:
                    Console.WriteLine("Нет случайного события сегодня.");
                    break;
            }
        }

        static public void EndGame()
        {
            int balance = (int)player.Money;
            for (int i = 0; i < market.products.Count; i++)
            {
                balance += (int)(market.products[i].price * market.products[i].quantityOnPlayer);
                market.products[i].quantityOnPlayer = 0;
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
