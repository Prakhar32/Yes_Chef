using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    public void Pause()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }

    public void Quit() => Application.Quit();
}
