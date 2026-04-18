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

    public void Interact(PlayerHand hand)
    {
        if (hand.Held != null ) return;
        _hand = hand;
        refrigiratorOpen.Invoke();
    }

    public void HandleSelection(Type type)
    {
        IIngredient ingredient = InstantiatePrefab(type);
        _hand.TryPickUp(ingredient);
        _hand = null;
    }

    private IIngredient InstantiatePrefab(Type type)
    {
        if (type == typeof(RawVegetable)) return Instantiate(_rawVegetablePrefab);
        if (type == typeof(RawMeat)) return Instantiate(_rawMeatPrefab);
        return Instantiate(_rawCheesePrefab);
    }
}
