using UnityEngine;

namespace YesChef.Domain
{
    public interface IIngredient
    {
        int ScoreValue { get; }
    }

    public class RawVegetable : MonoBehaviour, IIngredient { public int ScoreValue => 20; }
    public class ChoppedVegetable : MonoBehaviour, IIngredient { public int ScoreValue => 20; }
    public class RawMeat : MonoBehaviour, IIngredient { public int ScoreValue => 30; }
    public class CookedMeat : MonoBehaviour, IIngredient { public int ScoreValue => 30; }
    public class RawCheese : MonoBehaviour, IIngredient { public int ScoreValue => 10; }
}
