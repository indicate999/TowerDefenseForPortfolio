using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerActivator : MonoBehaviour
{
    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private GameObject _changePanel;

    public bool GetBuyPanelActiveStatus()
    {
        return _buyPanel.activeSelf;
    }

    public bool GetChangePanelActiveStatus()
    {
        return _changePanel.activeSelf;
    }

    public void SetActiveBuyPanel(bool activeState)
    {
        _buyPanel.SetActive(activeState);
    }

    public void SetActiveChangePanel(bool activeState)
    {
        _changePanel.SetActive(activeState);
    }

}
