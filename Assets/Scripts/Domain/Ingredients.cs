namespace YesChef.Domain
{
    public interface IIngredient
    {
        int ScoreValue { get; }
    }

    public sealed class RawVegetable : IIngredient
    {
        public int ScoreValue => 20;
    }

    public sealed class ChoppedVegetable : IIngredient
    {
        public int ScoreValue => 20;
    }

    public sealed class RawMeat : IIngredient
    {
        public int ScoreValue => 30;
    }

    public sealed class CookedMeat : IIngredient
    {
        public int ScoreValue => 30;
    }

    public sealed class RawCheese : IIngredient
    {
        public int ScoreValue => 10;
    }
}
