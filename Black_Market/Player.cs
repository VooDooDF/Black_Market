using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Player
    {
        public int InventoryCapacity { get; private set; } = 10;
        public int Money { get; private set; } = 1000;

        public int AddMoney(int amount)
        {   
            if (amount < 0)
            {
                throw new ArgumentException("Значение для добавления не может быть отрицательным.");
            }

            Money += amount;
            return Money;
        }

        public int SubtractMoney(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Значение для добавления не может быть отрицательным.");
            }

            if (amount > Money)
            {
                throw new InvalidOperationException("Недостаточно средств для вычитания.");
            }

            Money -= amount;
            return Money;
        }

        public int IncreaseInventoryCapacity()
        {
            InventoryCapacity += 1;
            return InventoryCapacity;
        }
    }
}
