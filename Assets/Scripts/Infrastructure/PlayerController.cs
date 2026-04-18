using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField] private InputAction _move;
    [SerializeField] private InputAction _interact;

    private IInteractable _nearby;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => EnableControls();
    private void OnDisable() => DisableControls();

    public void EnableControls()
    {
        _move.Enable();
        _interact.Enable();
        _interact.performed += OnInteract;
    }

    public void DisableControls()
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

    private void OnInteract(InputAction.CallbackContext _)
    {
        _nearby?.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        _nearby = other.GetComponentInParent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        _nearby = null;
    }
}
