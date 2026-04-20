using System.Collections;
using UnityEngine;

public class OrderDispatcher : MonoBehaviour
{
    [SerializeField] private CustomerWindowUI[] _windows;
    private const float RespawnDelay = 3f;

    private void Start()
    {
        foreach (CustomerWindowUI ui in _windows)
            ui.OnReady += onWindowReady;
    }

    private void onWindowReady(CustomerWindow window)
    {
        StartCoroutine(openAfterDelay(window));
    }

    private IEnumerator openAfterDelay(CustomerWindow window)
    {
        yield return new WaitForSeconds(RespawnDelay);
        window.Open(OrderFactory.CreateRandom());
    }
}
