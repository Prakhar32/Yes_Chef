using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField] private InputAction _move;
    [SerializeField] private InputAction _interact;

    private Rigidbody _rb;
    private SphereCollider _collider;
    private readonly Collider[] _hitBuffer = new Collider[8];

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
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
        int count = Physics.OverlapSphereNonAlloc(transform.position, _collider.radius, _hitBuffer);
        for (int i = 0; i < count; i++)
        {
            Collider hit = _hitBuffer[i];
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }
        }
    }
}
