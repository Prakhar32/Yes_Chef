using System;
using UnityEngine;

namespace YesChef.Domain
{
    public class Refrigerator : MonoBehaviour, IInteractable
    {
        [SerializeField] private RefrigeratorUI _ui;
        [SerializeField] private RawVegetable _rawVegetablePrefab;
        [SerializeField] private RawMeat _rawMeatPrefab;
        [SerializeField] private RawCheese _rawCheesePrefab;

        private PlayerHand _hand;

        public void Init(RefrigeratorUI ui, RawVegetable rawVegetable, RawMeat rawMeat, RawCheese rawCheese)
        {
            _ui = ui;
            _rawVegetablePrefab = rawVegetable;
            _rawMeatPrefab = rawMeat;
            _rawCheesePrefab = rawCheese;
        }

        public void Interact(PlayerHand hand)
        {
            if (hand.Held != null) return;
            _hand = hand;
            _ui.OnIngredientSelected += OnIngredientSelected;
            _ui.OnCancelled += OnCancelled;
            _ui.Open();
        }

        private void OnIngredientSelected(Type type)
        {
            _ui.OnIngredientSelected -= OnIngredientSelected;
            _ui.OnCancelled -= OnCancelled;
            var ingredient = InstantiatePrefab(type);
            _hand.TryPickUp(ingredient);
            _ui.Close();
            _hand = null;
        }

        private IIngredient InstantiatePrefab(Type type)
        {
            if (type == typeof(RawVegetable)) return Instantiate(_rawVegetablePrefab);
            if (type == typeof(RawMeat)) return Instantiate(_rawMeatPrefab);
            return Instantiate(_rawCheesePrefab);
        }

        private void OnCancelled()
        {
            _ui.OnIngredientSelected -= OnIngredientSelected;
            _ui.OnCancelled -= OnCancelled;
            _ui.Close();
            _hand = null;
        }
    }
}
