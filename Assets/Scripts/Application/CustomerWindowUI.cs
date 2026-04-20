using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerWindowUI : MonoBehaviour
{
    [SerializeField] private CustomerWindow _window;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Transform _ingredientsContainer;
    [SerializeField] private TextMeshProUGUI _ingredientLabelPrefab;

    public event System.Action<CustomerWindow> OnReady;

    private TextMeshProUGUI[] _ingredientLabels;
    private const int MaxIngredients = 3;

    private void Start()
    {
        _ingredientLabels = new TextMeshProUGUI[MaxIngredients];
        for (int i = 0; i < MaxIngredients; i++)
        {
            _ingredientLabels[i] = Instantiate(_ingredientLabelPrefab, _ingredientsContainer);
            _ingredientLabels[i].gameObject.SetActive(false);
        }

        _window.OnOrderChanged += orderUpdated;
        OnReady?.Invoke(_window);
    }

    private void orderUpdated(Order order)
    {
        if(order.IsComplete)
            orderComplete();
        else
            orderRemaining(order);
    }

    private void orderRemaining(Order order)
    {
        _scoreText.gameObject.SetActive(false);
        IReadOnlyList<Type> remaining = order.GetRemainingIngredients();
        for (int i = 0; i < MaxIngredients; i++)
        {
            bool active = i < remaining.Count;
            _ingredientLabels[i].gameObject.SetActive(active);
            if (active)
                _ingredientLabels[i].text = remaining[i].Name;
        }
    }

    private void orderComplete()
    {
        for (int i = 0; i < MaxIngredients; i++)
            _ingredientLabels[i].gameObject.SetActive(false);
        _scoreText.gameObject.SetActive(true);
        _scoreText.text = $"+{_window.GetScoreDelta()}";
        StartCoroutine(fadeScore());
    }

    private IEnumerator fadeScore()
    {
        float duration = 2f;
        float elapsed = 0f;
        Color color = _scoreText.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - elapsed / duration;
            _scoreText.color = color;
            yield return null;
        }
        _scoreText.gameObject.SetActive(false);
        color.a = 1f;
        _scoreText.color = color;
        OnReady?.Invoke(_window);
    }
}
