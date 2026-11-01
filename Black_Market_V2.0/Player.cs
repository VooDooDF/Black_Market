using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Player
    {
        public string Name { get; private set; }
        public decimal Money { get; private set; }
        public Inventory Inventory { get; }
        private readonly ProductCatalog catalog;

        // Конструктор класса Player
        public Player(string name, decimal money, ProductCatalog productCatalog)
        {
            catalog = productCatalog;
            if (money < 0)
            {
                throw new ArgumentException("Начальная сумма денег не может быть отрицательной.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя игрока не может быть пустым или состоять только из пробелов.");
            }

            Name = name;
            Money = money;
            Inventory = new Inventory(catalog);
        }

        // Метод для добавления денег игроку
        public decimal AddMoney(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Значение для добавления не может быть отрицательным или равным нулю.");
            }

            Money += amount;
            return Money;
        }

        // Метод для вычитания денег у игрока
        public decimal SubtractMoney(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Значение для вычитания не может быть отрицательным или равным нулю.");
            }

            if (amount > Money)
            {
                throw new InvalidOperationException("Недостаточно средств для вычитания.");
            }

            Money -= amount;
            return Money;
        } 
    }
}
