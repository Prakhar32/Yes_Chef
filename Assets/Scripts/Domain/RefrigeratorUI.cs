using System;
using UnityEngine;

namespace YesChef.Domain
{
    public class RefrigeratorUI : MonoBehaviour
    {
        private static readonly Type[] Items =
        {
            typeof(RawVegetable),
            typeof(RawMeat),
            typeof(RawCheese),
        };

        private const int BackIndex = 3;
        private const int ItemCount = 4; // 3 ingredients + Back

        public event Action<Type> OnIngredientSelected;
        public event Action OnCancelled;

        public bool IsOpen { get; private set; }
        public int SelectedIndex { get; private set; }

        public void Open()
        {
            IsOpen = true;
            SelectedIndex = 0;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void NavigateNext()
        {
            SelectedIndex = (SelectedIndex + 1) % ItemCount;
        }

        public void NavigatePrev()
        {
            SelectedIndex = (SelectedIndex - 1 + ItemCount) % ItemCount;
        }

        public void Confirm()
        {
            if (SelectedIndex == BackIndex)
            {
                OnCancelled.Invoke();
                return;
            }

            OnIngredientSelected.Invoke(Items[SelectedIndex]);
        }

        public void Cancel()
        {
            OnCancelled.Invoke();
        }
    }
}
