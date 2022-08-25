using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtons : MonoBehaviour
{
    [SerializeField] private Button[] _buyButtons;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _saleButton;

    public Button[] BuyButtons { get { return _buyButtons; } }
    public Button UpgradeButton { get { return _upgradeButton; } }
    public Button SaleButton { get { return _saleButton; } }

    public bool GetUpgradeButtonActiveStatus()
    {
        return _upgradeButton.gameObject.activeSelf;
    }

    public void SetActiveUpgradeButton(bool activeState)
    {
        _upgradeButton.gameObject.SetActive(activeState);
    }
}
