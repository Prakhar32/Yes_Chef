public class PlayerHand
{
    public IIngredient Held { get; private set; }

    public bool TryPickUp(IIngredient ingredient)
    {
        if (Held != null) return false;
        Held = ingredient;
        return true;
    }

    public void Place()
    {
        Held = null;
    }

    public void Discard()
    {
        Held = null;
    }
}
