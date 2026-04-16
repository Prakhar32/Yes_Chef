using System;
using System.Collections.Generic;

namespace YesChef.Domain
{
    public sealed class Order
    {
        private readonly List<Type> _pending;

        public IReadOnlyCollection<Type> Pending => _pending.AsReadOnly();
        public int Score { get; }
        public bool IsComplete => _pending.Count == 0;

        public Order(Type[] types)
        {
            foreach (var type in types)
                if (!typeof(IIngredient).IsAssignableFrom(type))
                    throw new ArgumentException($"{type.Name} does not implement IIngredient");
            _pending = new List<Type>(types);
            Score = 0;
        }

        private Order(List<Type> pending, int score)
        {
            _pending = pending;
            Score = score;
        }

        public Order Receive(IIngredient ingredient)
        {
            var type = ingredient.GetType();
            var updated = new List<Type>(_pending);
            if (!updated.Remove(type)) return this;
            return new Order(updated, Score + ingredient.ScoreValue);
        }
    }
}
