using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    private PlayerHand _hand;

    private void Start()
    {
        _hand = FindFirstObjectByType<PlayerHand>();
        if (_hand == null)
        {
            Destroy(gameObject);
            throw new MissingComponentException($"{nameof(PlayerHand)} not found in scene.");
        }
    }

    public void Interact()
    {
        if (_hand.Held is Component ingredient)
        {
            Destroy(ingredient.gameObject);
            _hand.Discard();
        }
    }
}
