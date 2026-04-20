using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float _duration = 180f;

    public event System.Action OnExpired;

    private TextMeshProUGUI _text;
    private float _remaining;
    private bool _running;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _remaining = _duration;
        _running = true;
        UpdateDisplay();
    }

    private void Update()
    {
        if (!_running) return;

        _remaining -= Time.deltaTime;
        if (_remaining <= 0f)
        {
            _remaining = 0f;
            _running = false;
            UpdateDisplay();
            OnExpired?.Invoke();
            return;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay() => _text.text = Mathf.CeilToInt(_remaining).ToString();
}
