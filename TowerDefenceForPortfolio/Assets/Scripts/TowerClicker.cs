using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerClicker : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;

    private Vector2 _screenPoint;
    private bool _isPoint = false;

    private TowerActivator _activator;

    void Start()
    {

    }

    private void Update()
    {
        GetClickPosition();
        ClickProcessing();
    }

    private void GetClickPosition()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
            && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            _screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            _isPoint = true;
        }
        else if (Input.GetMouseButtonDown(0) && Input.touchCount == 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            _screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _isPoint = true;
        }
    }

    private void ClickProcessing()
    {
        if (_isPoint)
        {
            DeactivateLastTowerUIPanelsAfterClick();
            ActivateCurrentlyClickedTurretUIPanel();
        }
    }

    private void DeactivateLastTowerUIPanelsAfterClick()
    {
        if (_activator != null)
        {
            if (_activator.GetBuyPanelActiveStatus())
                _activator.SetActiveBuyPanel(false);
            if (_activator.GetChangePanelActiveStatus())
                _activator.SetActiveChangePanel(false);
        }
    }

    private void ActivateCurrentlyClickedTurretUIPanel()
    {
        RaycastHit2D hit = Physics2D.Raycast(_screenPoint, Vector2.zero, 50, _layerMask);
        if (hit.collider != null)
        {
            var uiComponent = hit.collider.gameObject.GetComponent<TowerUIComponent>();
            _activator = hit.collider.gameObject.GetComponent<TowerActivator>();

            if (!uiComponent.IsActiveTurret())
                _activator.SetActiveBuyPanel(true);
            else
                _activator.SetActiveChangePanel(true);
        }

        _isPoint = false;
    }
}
