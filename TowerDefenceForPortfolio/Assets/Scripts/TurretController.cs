using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TurretController : MonoBehaviour
{
    [Serializable]
    private class TurretArray
    {
        public GameObject[] TurretSeries;
    }

    [SerializeField] private TurretArray[] _turretExamples;

    [SerializeField] private Collider2D[] _towerColliders;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _buyPanelPrefab;
    [SerializeField] private GameObject _changePanelPrefab;
    [SerializeField] private Vector3 _panelOfsset;

    private class Panels
    {
        public GameObject BuyPanel;
        public GameObject ChangePanel;
    }

    private Panels[] _panels;

    private GameObject[] _activeTurrets;

    private class TurrelTypes
    {
        public int TurretExampleIndex;
        public int TurretSeriesIndex;
    }

    private TurrelTypes[] _turretTypes;

    private int _currentTowerSideIndex;

    private Vector2 _screenPoint;

    private bool _isPoint = false;

    [SerializeField] private SoundEffector _soundEffector;
    [SerializeField] private Stats _stats;

    private void Awake()
    {
        _activeTurrets = new GameObject[_towerColliders.Length];
        _panels = new Panels[_towerColliders.Length];
        _turretTypes = new TurrelTypes[_towerColliders.Length];
    }

    private void Start()
    {
        CreateTurretUIPanels();
    }

    private void Update()
    {
        GetClickPosition();
        ClickProcessing();
    }

    private void CreateTurretUIPanels()
    {
        for (int i = 0; i < _towerColliders.Length; i++)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(_towerColliders[i].transform.position);
            var panelPosition = new Vector3(screenPosition.x, screenPosition.y, 0) + _panelOfsset;

            Panels panel = new Panels();
            panel.BuyPanel = Instantiate(_buyPanelPrefab, _canvas.transform, false);
            var rectTransform = panel.BuyPanel.GetComponent<RectTransform>();
            rectTransform.position = panelPosition;

            for (int a = 0; a < _turretExamples.Length; a++)
            {
                panel.BuyPanel.transform.GetChild(a).GetChild(0).GetComponent<Text>().text
                    = _turretExamples[a].TurretSeries[0].GetComponent<Turret>().PurchasePrice.ToString();
            }

            panel.BuyPanel.SetActive(false);

            panel.ChangePanel = Instantiate(_changePanelPrefab, _canvas.transform, false);
            var rectTransform2 = panel.ChangePanel.GetComponent<RectTransform>();
            rectTransform2.position = panelPosition;

            panel.ChangePanel.SetActive(false);

            _panels[i] = panel;
        }
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
            DeactivateAllTurretUIPanelsAfterClick();
            ActivateCurrentlyClickedTurretUIPanel();
        }
    }

    private void DeactivateAllTurretUIPanelsAfterClick()
    {
        for (int i = 0; i < _towerColliders.Length; i++)
        {
            if (_panels[i].BuyPanel.activeSelf)
                _panels[i].BuyPanel.SetActive(false);
            if (_panels[i].ChangePanel.activeSelf)
                _panels[i].ChangePanel.SetActive(false);
        }
    }

    private void ActivateCurrentlyClickedTurretUIPanel()
    {
        RaycastHit2D hit = Physics2D.Raycast(_screenPoint, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "TowerSide")
            {
                for (int i = 0; i < _towerColliders.Length; i++)
                {
                    if (_towerColliders[i] == hit.collider)
                    {
                        if (_activeTurrets[i] == null)
                            _panels[i].BuyPanel.SetActive(true);
                        else if (_activeTurrets[i] != null)
                            _panels[i].ChangePanel.SetActive(true);

                        _currentTowerSideIndex = i;

                        break;
                    }
                }
            }
        }

        _isPoint = false;
    }

    public void BuyTurret(int trackNum)
    {
        float buyPurchase = _turretExamples[trackNum].TurretSeries[0].GetComponent<Turret>().PurchasePrice;

        if (_stats.RemoveCoins(buyPurchase))
        {
            _soundEffector.PLayBuildingSound();

            var currentTowerSidePosition = _towerColliders[_currentTowerSideIndex].transform.position;

            _activeTurrets[_currentTowerSideIndex] = Instantiate(_turretExamples[trackNum].TurretSeries[0]
                , new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));

            _panels[_currentTowerSideIndex].BuyPanel.SetActive(false);

            TurrelTypes turretType = new TurrelTypes();
            turretType.TurretExampleIndex = trackNum;
            turretType.TurretSeriesIndex = 0;

            _turretTypes[_currentTowerSideIndex] = turretType;

            UpdateChangePanel();
        }
    }

    public void UpgradeTurret()
    {
        int turretSeriesLength = _turretExamples[ _turretTypes[_currentTowerSideIndex].TurretExampleIndex ].TurretSeries.Length - 1;

        float upgradePurchase 
            = _turretExamples[ _turretTypes[_currentTowerSideIndex].TurretExampleIndex ].TurretSeries[ _turretTypes[_currentTowerSideIndex].TurretSeriesIndex + 1 ]
        .GetComponent<Turret>().PurchasePrice;

        if (_stats.RemoveCoins(upgradePurchase))
        {
            _soundEffector.PLayBuildingSound();

            _turretTypes[_currentTowerSideIndex].TurretSeriesIndex++;

            if (_turretTypes[_currentTowerSideIndex].TurretSeriesIndex < turretSeriesLength)
                UpdateChangePanel();
            else
                _panels[_currentTowerSideIndex].ChangePanel.transform.GetChild(0).gameObject.SetActive(false);

            Destroy(_activeTurrets[_currentTowerSideIndex]);
            var currentTowerSidePosition = _towerColliders[_currentTowerSideIndex].transform.position;
            _activeTurrets[_currentTowerSideIndex] = Instantiate(_turretExamples[ _turretTypes[_currentTowerSideIndex].TurretExampleIndex ]
                .TurretSeries[ _turretTypes[_currentTowerSideIndex].TurretSeriesIndex ]
                , new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));

            _panels[_currentTowerSideIndex].ChangePanel.SetActive(false);
        }
    }

    public void SaleTurret()
    {
        var sellingPrice = _turretExamples[ _turretTypes[_currentTowerSideIndex].TurretExampleIndex ]
            .TurretSeries[ _turretTypes[_currentTowerSideIndex].TurretSeriesIndex ]
        .GetComponent<Turret>().SellingPrice;

        _stats.AddCoins(sellingPrice);

        _soundEffector.PLaySaleSound();

        Destroy(_activeTurrets[_currentTowerSideIndex]);
        _activeTurrets[_currentTowerSideIndex] = null;

        var upgradeButton = _panels[_currentTowerSideIndex].ChangePanel.transform.GetChild(0).gameObject;
        if (!upgradeButton.activeSelf)
            upgradeButton.SetActive(true);

        _panels[_currentTowerSideIndex].ChangePanel.SetActive(false);

        Array.Clear(_turretTypes, _currentTowerSideIndex, 1);
    }

    private void UpdateChangePanel()
    {
        _panels[_currentTowerSideIndex].ChangePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
        = _turretExamples[ _turretTypes[_currentTowerSideIndex].TurretExampleIndex ]
        .TurretSeries[ _turretTypes[_currentTowerSideIndex].TurretSeriesIndex + 1 ]
        .GetComponent<Turret>().PurchasePrice.ToString();

        _panels[_currentTowerSideIndex].ChangePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text
            = _turretExamples[_turretTypes[ _currentTowerSideIndex].TurretExampleIndex ]
            .TurretSeries[ _turretTypes[_currentTowerSideIndex].TurretSeriesIndex ]
            .GetComponent<Turret>().SellingPrice.ToString();
    }
}
