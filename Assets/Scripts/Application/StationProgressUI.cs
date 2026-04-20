using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StationProgressUI : MonoBehaviour
{
    [SerializeField] private Image _radialFill;

    private void Start()
    {
        _radialFill.gameObject.SetActive(false);
    }

    public void StartFill(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(Fill(duration));
    }

    private IEnumerator Fill(float duration)
    {
        _radialFill.fillAmount = 0f;
        _radialFill.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _radialFill.fillAmount = elapsed / duration;
            yield return null;
        }
        _radialFill.gameObject.SetActive(false);
    }
}
