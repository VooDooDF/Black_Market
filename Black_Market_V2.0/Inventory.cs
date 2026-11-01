using Black_Market;
using System;
using System.Collections.Generic;
using System.Text;

namespace Black_Market
{
    internal class Inventory
    {
        public int Capacity { get; private set; }
        Dictionary<Guid, int> QuantityOnPlayer { get; }
        private readonly ProductCatalog catalog;

        // Конструктор класса Inventory
        public Inventory(ProductCatalog productCatalog)
        {
            catalog = productCatalog;
            Capacity = 10;
            QuantityOnPlayer = new Dictionary<Guid, int>();
        }

        // Метод для проверки наличия свободного места в инвентаре
        public bool HasSpace(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return Capacity - CurrentCount() >= count;
        }

        // Метод для добавления продукта в инвентарь
        public void AddProduct(Guid productId, int productCount)
        {
            // Проверка на отрицательное количество продуктов
            if (productCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productCount));
            }

            // Проверка на свободное место в инвентаре
            if (HasSpace(productCount))
            {
                throw new InvalidOperationException("Отсутствует место в инвентаре.");
            }

            // Добавление продукта в инвентарь
            if (QuantityOnPlayer.ContainsKey(productId))
            {
                QuantityOnPlayer[productId] += productCount;
            }
            else
            {
                QuantityOnPlayer.Add(productId, productCount);
            }
        }

        // Метод для удаления продукта из инвентаря
        public void RemoveProduct(Guid productId, int productCount)
        {
            // Проверка на отрицательное количество продуктов
            if (productCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productCount));
            }

            // Проверкa на наличие продукта в инвентаре
            if (!QuantityOnPlayer.ContainsKey(productId))
            {
                throw new InvalidOperationException("Продукт не найден в инвентаре.");
            }

            int currentCount = QuantityOnPlayer[productId];
            if (currentCount < productCount)
            {
                throw new InvalidOperationException("Недостаточно продуктов в инвентаре.");
            }

            // Удаление продукта из инвентаря
            QuantityOnPlayer[productId] -= productCount;
            if (QuantityOnPlayer[productId] <= 0)
            {
                QuantityOnPlayer.Remove(productId);
            }   
        }

        // Метод для получения количества продукта в инвентаре
        public int GetQuantity(Guid productId)
        {
            if (QuantityOnPlayer.ContainsKey(productId))
            {
                return QuantityOnPlayer[productId];
            }
            else
            {
                return 0;
            }
        }

        // Метод для получения текущего количества продуктов в инвентаре
        public int CurrentCount()
        {
            return QuantityOnPlayer.Values.Sum();
        }

        // Метод для получения списка продуктов в инвентаре
        public List<Product?> GetProductsOnInventory()
        {
            return QuantityOnPlayer.Keys.Select(id => catalog.FindProductById(id)).Where(p => p != null).ToList();
        }

        // Метод для увеличения вместимости инвентаря
        public int IncreaseCapacity()
        {
            Capacity++;
            return Capacity;
        }
    }
}
