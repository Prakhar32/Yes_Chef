using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public IIngredient Held { get; private set; }
    private MonoBehaviour _held;

    public bool TryPickUp(IIngredient ingredient)
    {
        if (Held != null) return false;
        Held = ingredient;
        _held = (MonoBehaviour)ingredient;
        _held.transform.SetParent(transform, false);
        _held.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        return true;
    }

    public void Place()
    {
        _held.transform.SetParent(null, true);
        Held = null;
        _held = null;
    }

    public void Discard()
    {
        Held = null;
        _held = null;
    }
}
