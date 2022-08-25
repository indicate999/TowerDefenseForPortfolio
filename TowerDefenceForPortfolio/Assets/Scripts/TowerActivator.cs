using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerActivator : MonoBehaviour
{
    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private GameObject _changePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
