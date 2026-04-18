using System;
using UnityEngine;
using UnityEngine.Events;

public class Refrigerator : MonoBehaviour, IInteractable
{
    [SerializeField] private RawVegetable _rawVegetablePrefab;
    [SerializeField] private RawMeat _rawMeatPrefab;
    [SerializeField] private RawCheese _rawCheesePrefab;

    public Action refrigiratorOpen;

    private PlayerHand _hand;

    private void Start()
    {
        _hand = FindFirstObjectByType<PlayerHand>();
        if (_hand == null)
        {
            Destroy(gameObject);
            throw new MissingComponentException($"{nameof(PlayerHand)} not found in scene.");
        }
    }

    public void Interact()
    {
        if (_hand.Held != null) return;
        refrigiratorOpen.Invoke();
    }

    public void HandleSelection(Type type)
    {
        IIngredient ingredient = InstantiatePrefab(type);
        _hand.TryPickUp(ingredient);
    }

    private IIngredient InstantiatePrefab(Type type)
    {
        if (type == typeof(RawVegetable)) return Instantiate(_rawVegetablePrefab);
        if (type == typeof(RawMeat)) return Instantiate(_rawMeatPrefab);
        return Instantiate(_rawCheesePrefab);
    }
}
