using System.Collections;
using UnityEngine;

namespace YesChef.Domain
{
    public class StoveSlot : MonoBehaviour, IInteractable
    {
        public CookedMeat cookedMeatPrefab;

        private const float Duration = 6f;
        private RawMeat _cooking;
        private CookedMeat _ready;

        private bool IsReady => _ready != null;
        private bool IsIdle => _cooking == null && _ready == null;

        public void Interact(PlayerHand hand)
        {
            if (IsReady && hand.Held == null)
            {
                hand.TryPickUp(_ready);
                _ready = null;
                return;
            }

            if (IsIdle && hand.Held is RawMeat raw)
            {
                _cooking = raw;
                hand.Place();
                StartCoroutine(Cook());
            }
        }

        private IEnumerator Cook()
        {
            yield return new WaitForSeconds(Duration);
            _cooking.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            Destroy(_cooking.gameObject);
            _cooking = null;
            _ready = Instantiate(cookedMeatPrefab, position, rotation);
        }
    }
}
