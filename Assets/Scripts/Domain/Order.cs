using System;
using System.Collections.Generic;

namespace YesChef.Domain
{
    public sealed class Order
    {
        private readonly Dictionary<Type, int> _requirements;
        private readonly int _totalScore;

        public IReadOnlyDictionary<Type, int> Requirements => _requirements;

        public Order(IIngredient[] ingredients)
        {
            _requirements = new Dictionary<Type, int>();
            _totalScore = 0;
            foreach (var ingredient in ingredients)
            {
                var type = ingredient.GetType();
                _requirements.TryGetValue(type, out int count);
                _requirements[type] = count + 1;
                _totalScore += ingredient.ScoreValue;
            }
        }

        private Order(Dictionary<Type, int> requirements, int totalScore)
        {
            _requirements = requirements;
            _totalScore = totalScore;
        }

        public Order Receive(IIngredient ingredient)
        {
            var type = ingredient.GetType();
            if (!_requirements.TryGetValue(type, out int count))
                return this;

            var updated = new Dictionary<Type, int>(_requirements);
            if (count == 1)
                updated.Remove(type);
            else
                updated[type] = count - 1;

            return new Order(updated, _totalScore - ingredient.ScoreValue);
        }

        public int CalculateScore(float elapsedSeconds) =>
            _totalScore - (int)Math.Floor(elapsedSeconds);
    }
}
