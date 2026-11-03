using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Market
    {
        Random random = new Random();
        Dictionary<Guid, int> QuantityOnMarket { get; }
        private readonly ProductCatalog catalog;

        // Конструктор класса Market
        public Market(ProductCatalog catalog)
        {
            this.catalog = catalog;
            QuantityOnMarket = new Dictionary<Guid, int>();

            // Инициализация продуктов на рынке с рандомным количеством
            foreach (var product in catalog.Products)
            {
                int initialQuantity = random.Next(1, 10);
                QuantityOnMarket[product.Id] = initialQuantity;
            }
        }

        // Метод для получения списка продуктов на рынке
        public List<Product> GetProductsOnMarket(ProductCatalog catalog)
        {
            List<Product> productsOnMarket = new List<Product>();
            foreach (var product in catalog.Products)
            {
                if (QuantityOnMarket.ContainsKey(product.Id))
                {
                    productsOnMarket.Add(product);
                }
            }
            return productsOnMarket;
        }

        // Метод для получения количества продукта на рынке
        public int GetProductQuantity(Guid productId)
        {
            if (QuantityOnMarket.TryGetValue(productId, out int quantity))
            {
                return quantity;
            }

            return 0;
        }

        // Метод для добавления продукта на рынок
        public void AddProduct(Guid productId, int productCount)
        {
            if (productCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productCount));
            }

            if (QuantityOnMarket.ContainsKey(productId))
            {
                QuantityOnMarket[productId] += productCount;
            }
            else
            {
                QuantityOnMarket[productId] = productCount;
            }
        }

        // Метод для удаления продукта с рынка
        public void RemoveProduct(Guid productId, int productCount)
        {
            if (productCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productCount));
            }

            if (!QuantityOnMarket.ContainsKey(productId))
            {
                throw new InvalidOperationException("Продукт не найден на рынке.");
            }

            if (QuantityOnMarket[productId] < productCount)
            {
                throw new InvalidOperationException("Недостаточно продуктов на рынке.");
            }
            
            QuantityOnMarket[productId] -= productCount;
            if (QuantityOnMarket[productId] <= 0)
            {
                QuantityOnMarket.Remove(productId);
         
            }
        }

        // Покупка продукта игроком
        public string TryBuy(Player player, Guid productId, int quantity)
        {
            Product? product = catalog.FindProductById(productId);

            if (quantity <= 0)
            {
                return "Количество должно быть положительным.";
            }

            if (product == null)
            {
                throw new InvalidOperationException("Товар не найден.");
            }

            decimal totalPrice = product.Price * quantity;

            if (QuantityOnMarket.TryGetValue(productId, out int availableQuantity))
            {
                if (availableQuantity < quantity)
                {
                    return "Недостаточно товара на рынке.";
                }
            }
            else
            {
                return "Товар не найден на рынке.";
            }

            if (player.Money < totalPrice)
            {
                return "Недостаточно средств для покупки.";
            }

            if (!player.Inventory.HasSpace(quantity))
            {
                return "Недостаточно места в инвентаре.";
            }

            // Успешная покупка
            player.Inventory.AddProduct(productId, quantity);
            RemoveProduct(productId, quantity);
            player.SubtractMoney(totalPrice);

            return "Покупка продукта";
        }

        // Продажа продукта игроком
        public string TrySell(Player player, Guid productId, int quantity)
        {
            Product? product = catalog.FindProductById(productId);

            if (quantity <= 0)
            {
                return "Количество должно быть положительным.";
            }

            if (product == null)
            {
                throw new InvalidOperationException("Товар не найден.");
            }

            decimal totalPrice = product.Price * quantity;

            if (player.Inventory.GetQuantity(productId) < quantity)
            {
                return "Недостаточно товара в инвентаре.";
            }

            // Успешная продажа
            player.Inventory.RemoveProduct(productId, quantity);
            AddProduct(productId, quantity);
            player.AddMoney(totalPrice);
            
            return "Продажа продукта";
        }

        public void ChangePrices()
        {
            // Случайное изменение цены от -20% до +20%
            foreach (var product in catalog.Products)
            {
                int changePercentage = random.Next(-20, 21);
                decimal newPrice = Math.Round(product.Price * (1 + changePercentage / 100m), 2);
                newPrice = Math.Max(Product.MinPrice, newPrice);
                product.ChangePrice(newPrice);
            }
        }

        // Метод для изменения количества продуктов на рынке
        public void ChangeQuantities()
        {
            foreach (var product in catalog.Products)
            {
                int change = random.Next(-2, 3);
                int quantity = GetProductQuantity(product.Id);
                int newQuantity = Math.Max(0, quantity + change);
                if (newQuantity > 0)
                {
                    QuantityOnMarket[product.Id] = newQuantity;
                }
                else
                {
                    QuantityOnMarket.Remove(product.Id);
                }
            }
        }
    }
}
