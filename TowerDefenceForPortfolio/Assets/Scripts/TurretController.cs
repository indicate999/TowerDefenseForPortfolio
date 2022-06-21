using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TurretController : MonoBehaviour
{
    [Serializable]
    public class TurretArray
    {
        public GameObject[] turretSeries;
    }

    [SerializeField] private TurretArray[] turretExamples;

    [SerializeField] private Collider2D[] towerColliders;

    public static int towerCollidersArrayLenght;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject BuyPanelPrefab;
    [SerializeField] private GameObject ChangePanelPrefab;
    [SerializeField] private float panelPositionDistance;

    //[Serializable]
    private class Panels
    {
        public GameObject BuyPanel;
        public GameObject ChangePanel;
    }

    private Panels[] panels;

    private GameObject[] activeTurrets;

    //[Serializable]
    private class TurrelTypes
    {
        public int turretExampleIndex;
        public int turretSeriesIndex;
    }

    private TurrelTypes[] turretTypes;

    private int currentTowerSideIndex;

    private Vector2 screenPoint;
    private bool isPoint = false;
    //private bool isScenePoint = false;

    private Main main;
    //private SoundEffector soundEffector;

    //public static bool[] isTurretSound;

    // Start is called before the first frame update
    private void Awake()
    {
        activeTurrets = new GameObject[towerColliders.Length];
        panels = new Panels[towerColliders.Length];
        turretTypes = new TurrelTypes[towerColliders.Length];
        //isTurretSound = new bool[towerColliders.Length];
        //towerCollidersArrayLenght = towerColliders.Length;

        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
        //soundEffector = GameObject.FindGameObjectWithTag("SoundEffector").GetComponent<SoundEffector>();
    }

    void Start()
    {
        for (int i = 0; i < towerColliders.Length; i++)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(towerColliders[i].transform.position);
            //Debug.Log(BuyPanelPrefab.name);
            Panels panel = new Panels();
            panel.BuyPanel = Instantiate(BuyPanelPrefab, Canvas.transform, false);
            var rectTransform = panel.BuyPanel.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(screenPosition.x, screenPosition.y + panelPositionDistance, 0);

            for (int a = 0; a < turretExamples.Length; a++)
            {
                panel.BuyPanel.transform.GetChild(a).GetChild(0).GetComponent<Text>().text = turretExamples[a].turretSeries[0].GetComponent<Turret>().purchasePrice.ToString();
            }

            panel.BuyPanel.SetActive(false);

            panel.ChangePanel = Instantiate(ChangePanelPrefab, Canvas.transform, false);
            var rectTransform2 = panel.ChangePanel.GetComponent<RectTransform>();
            rectTransform2.position = new Vector3(screenPosition.x, screenPosition.y + panelPositionDistance, 0);//Camera.main.WorldToScreenPoint(new Vector3(towerColliders[i].transform.position.x, towerColliders[i].transform.position.y, 0));
            panel.ChangePanel.SetActive(false);

            panels[i] = panel;
        }

        //SoundEffector.isTurretSound[0] = true;
        //soundEffector.PlayTurretAttackSound(0);


        //soundEffector.PlayTurretAttackSound(0);
        //SoundEffector.isTurretSound[1] = true;
        //soundEffector.PlayTurretAttackSound(1);
    }

    // Update is called once per frame
    void Update()
    {


        //if (Input.touchCount > 0)//(Input.GetMouseButtonDown(0))
        //{

        //Debug.Log("ddd");

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //Debug.Log("1111");
            screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            isPoint = true;

            //if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            //    isScenePoint = true;
        }
        else if (Input.GetMouseButtonDown(0) && Input.touchCount == 0 && Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("2222");
            screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPoint = true;

            //if (!EventSystem.current.IsPointerOverGameObject())
            //    isScenePoint = true;
        }

        if (isPoint) //&& isScenePoint)
        {
            //Debug.Log("3333");
            //if (isScenePoint)//(!EventSystem.current.IsPointerOverGameObject() && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            //{
            for (int i = 0; i < towerColliders.Length; i++)
            {

                if (panels[i].BuyPanel.activeSelf)
                    panels[i].BuyPanel.SetActive(false);
                if (panels[i].ChangePanel.activeSelf)
                    panels[i].ChangePanel.SetActive(false);
            }
            //}

            //Debug.Log(screenPoint);
            //Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 touchPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(screenPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "TowerSide")// && isScenePoint)
                {
                    for (int i = 0; i < towerColliders.Length; i++)
                    {
                        if (towerColliders[i] == hit.collider)
                        {
                            if (activeTurrets[i] == null)
                            {
                                if (!panels[i].BuyPanel.activeSelf)
                                    panels[i].BuyPanel.SetActive(true);
                                else if (panels[i].BuyPanel.activeSelf)
                                    panels[i].BuyPanel.SetActive(false);
                            }
                            else if (activeTurrets[i] != null)
                            {
                                if (!panels[i].ChangePanel.activeSelf)
                                    panels[i].ChangePanel.SetActive(true);
                                else if (panels[i].ChangePanel.activeSelf)
                                    panels[i].ChangePanel.SetActive(false);
                            }

                            currentTowerSideIndex = i;

                            break;
                        }
                    }

                    //if (!EventSystem.current.IsPointerOverGameObject())
                    //Instantiate(turrel1[0], new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, 0), Quaternion.Euler(0, 0, 180));
                }
            }

            isPoint = false;
            //isScenePoint = false;
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //var panel = Instantiate(BuyPanel, Canvas.transform, false);//, Vector3.zero, Quaternion.identity);
        //var rectTransform = panel.GetComponent<RectTransform>();
        //rectTransform.position = Input.mousePosition;
        //Debug.Log(Input.mousePosition);
        //}

    }

    public void BuyTurret(int trackNum)
    {
        float buyPurchase = turretExamples[trackNum].turretSeries[0].GetComponent<Turret>().purchasePrice;

        if (Stats.coinCount >= buyPurchase)
        {
            Stats.coinCount -= buyPurchase;
            main.UpdateCoins();

            var currentTowerSidePosition = towerColliders[currentTowerSideIndex].transform.position;

            activeTurrets[currentTowerSideIndex] = Instantiate(turretExamples[trackNum].turretSeries[0], new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));
            //activeTurrets[currentTowerSideIndex].GetComponent<Turret>().turretTowerSideIndex = currentTowerSideIndex;
            //activeTurrets[currentTowerSideIndex].GetComponent<Turret>().turretExampleIndex = trackNum;

            panels[currentTowerSideIndex].BuyPanel.SetActive(false);

            TurrelTypes turretType = new TurrelTypes();
            turretType.turretExampleIndex = trackNum;
            turretType.turretSeriesIndex = 0;

            turretTypes[currentTowerSideIndex] = turretType;

            UpdateChangePanel();

            //isTurretSound[currentTowerSideIndex] = true;
            //soundEffector.PlayTurretAttackSound(trackNum, currentTowerSideIndex);
            //SoundEffector.isTurretSound[trackNum] = true;
            //soundEffector.PlayTurretAttackSound(trackNum);

            //SoundEffector.isTurretSound[0] = true;
            //soundEffector.PlayTurretAttackSound(0);

        }
    }

    public void UpgradeTurret()
    {
        int turretExampleLength = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries.Length - 1;

        float upgradePurchase = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex + 1]
        .GetComponent<Turret>().purchasePrice;

        if (Stats.coinCount >= upgradePurchase) //&& turretTypes[currentTowerSideIndex].turretSeriesIndex < turretExampleLength)
        {
            Stats.coinCount -= upgradePurchase;
            main.UpdateCoins();

            turretTypes[currentTowerSideIndex].turretSeriesIndex++;

            if (turretTypes[currentTowerSideIndex].turretSeriesIndex < turretExampleLength)
                UpdateChangePanel();

            Destroy(activeTurrets[currentTowerSideIndex]);
            var currentTowerSidePosition = towerColliders[currentTowerSideIndex].transform.position;
            activeTurrets[currentTowerSideIndex] = Instantiate(turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex], new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));
            //activeTurrets[currentTowerSideIndex].GetComponent<Turret>().turretTowerSideIndex = currentTowerSideIndex;
            //activeTurrets[currentTowerSideIndex].GetComponent<Turret>().turretTowerSideIndex = currentTowerSideIndex;

            panels[currentTowerSideIndex].ChangePanel.SetActive(false);

            if (turretTypes[currentTowerSideIndex].turretSeriesIndex == turretExampleLength)
                panels[currentTowerSideIndex].ChangePanel.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SaleTurret()
    {
        Stats.coinCount += turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex]
        .GetComponent<Turret>().sellingPrice;
        main.UpdateCoins();

        Destroy(activeTurrets[currentTowerSideIndex]);
        activeTurrets[currentTowerSideIndex] = null;
        //SoundEffector.isTurretSound[currentTowerSideIndex] = false;

        panels[currentTowerSideIndex].ChangePanel.transform.GetChild(0).gameObject.SetActive(true);

        panels[currentTowerSideIndex].ChangePanel.SetActive(false);

        Array.Clear(turretTypes, currentTowerSideIndex, 1);
        //turretTypes[currentTowerSide].turretExample = -1;
        //turretTypes[currentTowerSide].turretSeriesIndex = -1;
        //isTurretSound[currentTowerSideIndex] = false;
    }

    private void UpdateChangePanel()
    {
        //int turretExampleLength = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries.Length - 1;

        panels[currentTowerSideIndex].ChangePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
        = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex + 1]
        .GetComponent<Turret>().purchasePrice.ToString();

        panels[currentTowerSideIndex].ChangePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text
            = turretExamples[turretTypes[currentTowerSideIndex].turretExampleIndex].turretSeries[turretTypes[currentTowerSideIndex].turretSeriesIndex]
            .GetComponent<Turret>().sellingPrice.ToString();
    }
}
