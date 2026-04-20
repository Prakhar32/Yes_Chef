public class CustomerWindow : UnityEngine.MonoBehaviour, IInteractable
{
    public event System.Action<Order> OnOrderChanged;

    private Order Order;
    public bool IsEmpty => Order == null || Order.IsComplete;
    private float _elapsedTime;

    private PlayerHand _hand;

    private void Start()
    {
        _hand = FindFirstObjectByType<PlayerHand>();
        if (_hand == null)
        {
            Destroy(gameObject);
            throw new UnityEngine.MissingComponentException($"{nameof(PlayerHand)} not found in scene.");
        }
    }

    private void Update()
    {
        if (Order != null && !Order.IsComplete)
            _elapsedTime += UnityEngine.Time.deltaTime;
    }

    public void Open(Order order)
    {
        Order = order;
        _elapsedTime = 0f;
        OnOrderChanged(Order);
    }

    public void Interact()
    {
        if (Order == null) return;
        if (_hand.Held == null) return;

        Order updated = Order.Receive(_hand.Held);
        if (updated == Order) return;

        UnityEngine.MonoBehaviour ingredient = (UnityEngine.MonoBehaviour)_hand.Held;
        _hand.Place();
        ingredient.gameObject.SetActive(false);
        Order = updated;
        OnOrderChanged(Order);
    }

    public float ElapsedSeconds => _elapsedTime;

    public int GetScoreDelta() => Order.Score - UnityEngine.Mathf.FloorToInt(_elapsedTime);
}
