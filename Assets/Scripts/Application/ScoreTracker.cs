using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private CustomerWindow[] _windows;
    [SerializeField] private GameTimer _timer;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _pauseScoreText;
    [SerializeField] private TextMeshProUGUI _pauseHighScoreText;

    private const string HighScoreKey = "HighScore";

    private int _score;

    private void Start()
    {
        foreach (CustomerWindow window in _windows)
        {
            CustomerWindow w = window;
            w.OnOrderChanged += order => { if (order.IsComplete) AddScore(w.GetScoreDelta()); };
        }

        _timer.OnExpired += CheckHighScore;
        UpdateDisplay();
        _pauseHighScoreText.text = $"High Score: {PlayerPrefs.GetInt(HighScoreKey, 0)}";
    }

    private void AddScore(int delta)
    {
        _score += delta;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _scoreText.text = _score.ToString();
        _pauseScoreText.text = $"Score: {_score}";
    }

    private void CheckHighScore()
    {
        int high = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (_score > high)
        {
            PlayerPrefs.SetInt(HighScoreKey, _score);
            high = _score;
        }
        _pauseHighScoreText.text = $"High Score: {high}";
    }
}
