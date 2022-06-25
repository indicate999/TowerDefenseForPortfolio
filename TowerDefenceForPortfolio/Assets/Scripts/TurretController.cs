using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TurretController : MonoBehaviour
{
    //All kinds of turrets are stored here
    [Serializable]
    private class TurretArray
    {
        public GameObject[] turretSeries;
    }

    [SerializeField] private TurretArray[] turretExamples;

    //Here are stored all the colliders of the towers where you can put the turret
    [SerializeField] private Collider2D[] towerColliders;

    //Here are all the ui elements used to work with turrets
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject BuyPanelPrefab;
    [SerializeField] private GameObject ChangePanelPrefab;
    [SerializeField] private float panelPositionDistance;

    private class Panels
    {
        public GameObject BuyPanel;
        public GameObject ChangePanel;
    }

    private Panels[] panels;

    //Here are stored all the turrets that are on the scene
    private GameObject[] activeTurrets;

    //It stores information in indexes about all the turrets on the scene. In which tower it is located, what type of the turret
    private class TurrelTypes
    {
        public int turretExampleIndex;
        public int turretSeriesIndex;
    }

    private TurrelTypes[] turretTypes;

    //An index is stored here that determines the location of the currently selected turret
    private int currentTowerSideIndex;

    //This is the point where the player clicks with a touch or mouse.
    private Vector2 screenPoint;

    //This variable indicates whether the player clicked on the tower
    private bool isPoint = false;

    private Main main;
    private SoundEffector soundEffector;

    private void Awake()
    {
        activeTurrets = new GameObject[towerColliders.Length];
        panels = new Panels[towerColliders.Length];
        turretTypes = new TurrelTypes[towerColliders.Length];

        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
        soundEffector = GameObject.FindGameObjectWithTag("SoundEffector").GetComponent<SoundEffector>();
    }

    //In this method, all the ui elements necessary for working with turrets are generated
    void Start()
    {
        for (int i = 0; i < towerColliders.Length; i++)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(towerColliders[i].transform.position);
            var panelPosition = new Vector3(screenPosition.x, screenPosition.y + panelPositionDistance, 0);

            Panels panel = new Panels();
            panel.BuyPanel = Instantiate(BuyPanelPrefab, Canvas.transform, false);
            var rectTransform = panel.BuyPanel.GetComponent<RectTransform>();
            rectTransform.position = panelPosition;

            for (int a = 0; a < turretExamples.Length; a++)
            {
                panel.BuyPanel.transform.GetChild(a).GetChild(0).GetComponent<Text>().text
                    = turretExamples[a].turretSeries[0].GetComponent<Turret>().purchasePrice.ToString();
            }

            panel.BuyPanel.SetActive(false);

            panel.ChangePanel = Instantiate(ChangePanelPrefab, Canvas.transform, false);
            var rectTransform2 = panel.ChangePanel.GetComponent<RectTransform>();
            rectTransform2.position = panelPosition;

            panel.ChangePanel.SetActive(false);

            panels[i] = panel;
        }
    }

    void Update()
    {
        //Here, they calculate whether the player clicked on the tower and set the variables.
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began 
            && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            isPoint = true;
        }
        else if (Input.GetMouseButtonDown(0) && Input.touchCount == 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPoint = true;
        }

        //This code is executed if the player clicks on the tower
        if (isPoint)
        {
            //All ui elements of previous towers are removed here if they were active
            for (int i = 0; i < towerColliders.Length; i++)
            {
                if (panels[i].BuyPanel.activeSelf)
                    panels[i].BuyPanel.SetActive(false);
                if (panels[i].ChangePanel.activeSelf)
                    panels[i].ChangePanel.SetActive(false);
            }

            //Here, ui elements are activated at the tower that the player clicked on. If the turret is not bought,
            //then the buy panel is opened, if the tower is already purchased,
            //then the change panel opens where you can upgrade the turret or sell it
            RaycastHit2D hit = Physics2D.Raycast(screenPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "TowerSide")
                {
                    for (int i = 0; i < towerColliders.Length; i++)
                    {
                        if (towerColliders[i] == hit.collider)
                        {
                            if (activeTurrets[i] == null)
                                panels[i].BuyPanel.SetActive(true);
                            else if (activeTurrets[i] != null)
                                panels[i].ChangePanel.SetActive(true);

                            currentTowerSideIndex = i;

                            break;
                        }
                    }
                }
            }

            isPoint = false;
        }
    }

    //This method is called when player click the turret purchase button
    public void BuyTurret(int trackNum)
    {
        //With the help of an array that store information about all the turrets present on the scene,
        //we get access to the information of the turret instance. In the future, this will be used frequently
        float buyPurchase = turretExamples[trackNum].turretSeries[0].GetComponent<Turret>().purchasePrice;

        //If the player has enough money to buy a turret, then its cost is deducted from the player's account and
        //an instance of the turret is added to the scene, which is also added to the List.
        //Information is also stored in the indexes about the location of the turret and its type.
        if (Stats.coinCount >= buyPurchase)
        {
            Stats.coinCount -= buyPurchase;
            main.UpdateCoins();

            soundEffector.PLayBuildingSound();

            var currentTowerSidePosition = towerColliders[currentTowerSideIndex].transform.position;

            activeTurrets[currentTowerSideIndex] = Instantiate(turretExamples[trackNum].turretSeries[0]
                , new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));

            panels[currentTowerSideIndex].BuyPanel.SetActive(false);

            TurrelTypes turretType = new TurrelTypes();
            turretType.turretExampleIndex = trackNum;
            turretType.turretSeriesIndex = 0;

            turretTypes[currentTowerSideIndex] = turretType;

            UpdateChangePanel();
        }
    }

    //This method is called when player click the turret upgrade button
    public void UpgradeTurret()
    {
        int turretSeriesLength = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries.Length - 1;

        float upgradePurchase 
            = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex + 1]
        .GetComponent<Turret>().purchasePrice;

        //If the player has enough money to upgrade the turret, then its cost is deducted from the player's account,
        //the current turret is removed and its more upgraded version is created
        if (Stats.coinCount >= upgradePurchase)
        {
            Stats.coinCount -= upgradePurchase;
            main.UpdateCoins();

            soundEffector.PLayBuildingSound();

            turretTypes[currentTowerSideIndex].turretSeriesIndex++;


            if (turretTypes[currentTowerSideIndex].turretSeriesIndex < turretSeriesLength)
                UpdateChangePanel();
            else
                //If the turret has been upgraded to the maximum, then the upgrade button is deactivated
                panels[currentTowerSideIndex].ChangePanel.transform.GetChild(0).gameObject.SetActive(false);

            Destroy(activeTurrets[currentTowerSideIndex]);
            var currentTowerSidePosition = towerColliders[currentTowerSideIndex].transform.position;
            activeTurrets[currentTowerSideIndex] = Instantiate(turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex]
                , new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));

            panels[currentTowerSideIndex].ChangePanel.SetActive(false);
        }
    }

    //This method is called when player click the turret sale button
    public void SaleTurret()
    {
        //When a player sells a turret, he receives money on his account, and the sold turret is removed;
        Stats.coinCount += turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex]
        .GetComponent<Turret>().sellingPrice;
        main.UpdateCoins();

        soundEffector.PLaySaleSound();

        Destroy(activeTurrets[currentTowerSideIndex]);
        activeTurrets[currentTowerSideIndex] = null;

        //If the tower upgrade button was deactivated due to the fact that the tower was upgraded to the maximum,
        //then after selling the tower, the upgrade button will be activated again
        var upgradeButton = panels[currentTowerSideIndex].ChangePanel.transform.GetChild(0).gameObject;
        if (!upgradeButton.activeSelf)
            upgradeButton.SetActive(true);

        panels[currentTowerSideIndex].ChangePanel.SetActive(false);

        Array.Clear(turretTypes, currentTowerSideIndex, 1);
    }

    //This method is needed to update the cost of upgrading and selling a turret in ui elements after buying or upgrading a turret
    private void UpdateChangePanel()
    {
        panels[currentTowerSideIndex].ChangePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
        = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex + 1]
        .GetComponent<Turret>().purchasePrice.ToString();

        panels[currentTowerSideIndex].ChangePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text
            = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex]
            .GetComponent<Turret>().sellingPrice.ToString();
    }
}
