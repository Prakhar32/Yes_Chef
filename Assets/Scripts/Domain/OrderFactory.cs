using System;

namespace YesChef.Domain
{
    public static class OrderFactory
    {
        private static readonly Random _random = new();

        private static readonly IIngredient[] _pool =
        {
            new ChoppedVegetable(),
            new CookedMeat(),
            new RawCheese(),
        };

        public static Order CreateRandom()
        {
            int count = OrderSizeSelectorService.Pick();
            IIngredient[] ingredients = new IIngredient[count];
            for (int i = 0; i < count; i++)
                ingredients[i] = _pool[_random.Next(_pool.Length)];
            return new Order(ingredients);
        }
    }
}
