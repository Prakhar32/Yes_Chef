using System.Collections;
using UnityEngine;

public class StoveSlot : MonoBehaviour, IInteractable
{
    public CookedMeat cookedMeatPrefab;

    [SerializeField]
    private Transform _ingredientPlacement;
    public UnityEngine.Events.UnityEvent<float> OnProcessingStarted;

    private RawMeat _cooking;
    private CookedMeat _ready;
    private PlayerHand _hand;

    private bool IsReady => _ready != null;
    private bool IsIdle => _cooking == null && _ready == null;

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

        if (IsIdle && _hand.Held is RawMeat raw)
        {
            _cooking = raw;
            _hand.Place();
            _cooking.transform.position = _ingredientPlacement.position;
            OnProcessingStarted.Invoke(GameConstants.CookingDuration);
            StartCoroutine(Cook());
        }
    }

    private IEnumerator Cook()
    {
        yield return new WaitForSeconds(GameConstants.CookingDuration);
        _cooking.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        Destroy(_cooking.gameObject);
        _cooking = null;
        _ready = Instantiate(cookedMeatPrefab, position, rotation);
    }
}
