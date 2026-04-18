using System.Collections;
using UnityEngine;

public class ChoppingTable : MonoBehaviour, IInteractable
{
    public ChoppedVegetable choppedVegetablePrefab;

    private const float Duration = 2f;
    private RawVegetable _chopping;
    private ChoppedVegetable _ready;
    private PlayerHand _hand;

    private bool IsReady => _ready != null;
    private bool IsIdle => _chopping == null && _ready == null;

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
        if (IsReady && _hand.Held == null)
        {
            _hand.TryPickUp(_ready);
            _ready = null;
            return;
        }

        if (IsIdle && _hand.Held is RawVegetable raw)
        {
            _chopping = raw;
            _hand.Place();
            _chopping.transform.position = transform.position;
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
