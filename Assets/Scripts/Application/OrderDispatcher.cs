using UnityEngine;

public class OrderDispatcher : MonoBehaviour
{
    [SerializeField] private CustomerWindowUI[] _windows;

    private void Start()
    {
        foreach (CustomerWindowUI ui in _windows)
            ui.OnReady += onWindowReady;
    }

    private void onWindowReady(CustomerWindow window)
    {
        window.Open(OrderFactory.CreateRandom());
    }
}
