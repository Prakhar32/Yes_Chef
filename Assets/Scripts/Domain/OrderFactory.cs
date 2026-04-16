using System;

public static class OrderFactory
{
    private static readonly Random _random = new();

    private static readonly Type[] _pool =
    {
        typeof(ChoppedVegetable),
        typeof(CookedMeat),
        typeof(RawCheese),
    };

    public static Order CreateRandom()
    {
        int count = OrderSizeSelectorService.Pick();
        Type[] types = new Type[count];
        for (int i = 0; i < count; i++)
            types[i] = _pool[_random.Next(_pool.Length)];
        return new Order(types);
    }
}
