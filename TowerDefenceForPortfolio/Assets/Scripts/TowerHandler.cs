using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerUIComponent), typeof(TowerButtons))]
public class TowerHandler : MonoBehaviour
{
    private TowerUIComponent _uiComponent;
    private TowerButtons _buttons;

    private void Awake()
    {
        _uiComponent = GetComponent<TowerUIComponent>();
        _buttons = GetComponent<TowerButtons>();
    }

    private void Start()
    {
        for (int i = 0; i < _buttons.BuyButtons.Length; i++)
        {
            int num = i;
            _buttons.BuyButtons[i].onClick.AddListener(delegate { _uiComponent.BuyTurret(num); });
        }

        _buttons.UpgradeButton.onClick.AddListener(_uiComponent.UpgradeTurret);
        _buttons.SaleButton.onClick.AddListener(_uiComponent.SaleTurret);
    }
}
