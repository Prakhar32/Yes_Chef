using System.Collections;
using UnityEngine;

namespace YesChef.Domain
{
    public class ChoppingTable : MonoBehaviour, IInteractable
    {
        public ChoppedVegetable choppedVegetablePrefab;

        private const float Duration = 2f;
        private RawVegetable _chopping;
        private ChoppedVegetable _ready;

        private bool IsReady => _ready != null;
        private bool IsIdle => _chopping == null && _ready == null;

        public void Interact(PlayerHand hand)
        {
            if (IsReady && hand.Held == null)
            {
                hand.TryPickUp(_ready);
                _ready = null;
                return;
            }

            if (IsIdle && hand.Held is RawVegetable raw)
            {
                _chopping = raw;
                hand.Place();
                StartCoroutine(Chop());
            }
        }

        private IEnumerator Chop()
        {
            yield return new WaitForSeconds(Duration);
            _chopping.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            Destroy(_chopping.gameObject);
            _chopping = null;
            _ready = Instantiate(choppedVegetablePrefab, position, rotation);
        }
    }
}
