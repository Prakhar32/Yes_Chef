using System;

namespace YesChef.Domain
{
    public static class OrderSizeSelectorService
    {
        private const int SmallOrderSize = 2;
        private const int LargeOrderSize = 3;

        private static readonly Random _random = new();

        public static int Pick() =>
            _random.Next(2) == 0 ? SmallOrderSize : LargeOrderSize;
    }
}
