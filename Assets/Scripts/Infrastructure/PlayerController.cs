using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField] private InputAction _move;
    [SerializeField] private InputAction _interact;

    private readonly PlayerHand _hand = new PlayerHand();
    private IInteractable _nearby;
    private Rigidbody _rb;

    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void OnEnable()
    {
        _move.Enable();
        _interact.Enable();
        _interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _move.Disable();
        _interact.Disable();
        _interact.performed -= OnInteract;
    }

    private void FixedUpdate()
    {
        Vector2 input = _move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0f, input.y).normalized * speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + move);
    }

    private void OnInteract(InputAction.CallbackContext _) => _nearby?.Interact(_hand);

    private void OnTriggerEnter(Collider other)
    {
        if (_nearby == null)
            _nearby = other.GetComponentInParent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<IInteractable>() == _nearby)
            _nearby = null;
    }
}
