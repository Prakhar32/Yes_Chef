using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private CustomerWindow[] _windows;
    [SerializeField] private GameTimer _timer;

    private const string HighScoreKey = "HighScore";

    private TextMeshProUGUI _text;
    private int _score;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        foreach (CustomerWindow window in _windows)
        {
            CustomerWindow w = window;
            w.OnOrderChanged += order => { if (order.IsComplete) AddScore(w.GetScoreDelta()); };
        }

        _timer.OnExpired += CheckHighScore;
        UpdateDisplay();
    }

    private void AddScore(int delta)
    {
        _score += delta;
        UpdateDisplay();
    }

    private void UpdateDisplay() => _text.text = _score.ToString();

    private void CheckHighScore()
    {
        int high = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (_score > high)
            PlayerPrefs.SetInt(HighScoreKey, _score);
    }
}
