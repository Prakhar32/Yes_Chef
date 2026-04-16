using UnityEngine;

namespace YesChef.Domain
{
    public class Trash : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerHand hand)
        {
            if (hand.Held is Component ingredient)
            {
                Destroy(ingredient.gameObject);
                hand.Discard();
            }
        }
    }
}
