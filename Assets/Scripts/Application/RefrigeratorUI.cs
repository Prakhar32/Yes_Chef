using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RefrigiratorUI : MonoBehaviour
{
    public Refrigerator refrigerator;
    public PlayerController controller;

    [SerializeField]
    private InputAction _navigate;

    [SerializeField]
    private InputAction _confirm;

    [SerializeField]
    private GameObject _panel;

    [SerializeField]
    private Text[] _labels;

    private static readonly Type[] Items =
    {
        typeof(RawVegetable),
        typeof(RawMeat),
        typeof(RawCheese),
    };

    private static readonly Color Selected = Color.yellow;
    private static readonly Color Normal = Color.white;

    internal const int BackIndex = 3;
    private const int ItemCount = 4;

    private int _currentindex;

    private void Awake()
    {
        _panel.SetActive(false);
        refrigerator.refrigiratorOpen += Open;
    }

    private void Open()
    {
        _currentindex = 0;
        _panel.SetActive(true);
        RefreshHighlight();
        controller.DisableControls();
        EnableUIControls();
    }

    private void Close()
    {
        _panel.SetActive(false);
        DisableUIControls();
        controller.EnableControls();
    }

    private void EnableUIControls()
    {
        _navigate.Enable();
        _confirm.Enable();
        _navigate.performed += OnNavigate;
        _confirm.performed += OnConfirm;
    }

    private void DisableUIControls()
    {
        _navigate.performed -= OnNavigate;
        _confirm.performed -= OnConfirm;
        _navigate.Disable();
        _confirm.Disable();
    }

    private void OnConfirm(InputAction.CallbackContext _)
    {
        if (_currentindex != BackIndex)
            refrigerator.HandleSelection(Items[_currentindex]);
        Close();
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        _currentindex += (int)context.ReadValue<float>();
        _currentindex = Mathf.Clamp(_currentindex, 0, ItemCount - 1);
        RefreshHighlight();
    }

    private void RefreshHighlight()
    {
        for (int i = 0; i < _labels.Length; i++)
            _labels[i].color = i == _currentindex ? Selected : Normal;
    }
}
