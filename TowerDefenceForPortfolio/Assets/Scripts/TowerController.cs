using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TowerController : MonoBehaviour
{
    [Serializable]
    public class TurrelArray
    {
        public GameObject[] turretSeriesIndexArray;
    }

    [SerializeField]
    private TurrelArray[] turretExamplesArray;

    [SerializeField]
    private Collider2D[] towerColliders;

    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject BuyPanelPrefab;
    [SerializeField]
    private GameObject ChangePanelPrefab;
    [SerializeField]
    private float panelPositionDistance;

    //[Serializable]
    private class Panels
    {
        public GameObject BuyPanel;
        public GameObject ChangePanel;
    }

    private Panels[] panels;

    private GameObject[] turrets;

    //[Serializable]
    private class TurrelTypes
    {
        public int turretExample;
        public int turretSeriesIndex;
    }

    private TurrelTypes[] turretTypes;

    private int currentTowerSide;

    // Start is called before the first frame update
    private void Awake()
    {
        turrets = new GameObject[towerColliders.Length];
        panels = new Panels[towerColliders.Length];
        turretTypes = new TurrelTypes[towerColliders.Length];
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
            panel.BuyPanel.SetActive(false);

            panel.ChangePanel = Instantiate(ChangePanelPrefab, Canvas.transform, false);
            var rectTransform2 = panel.ChangePanel.GetComponent<RectTransform>();
            rectTransform2.position = new Vector3(screenPosition.x, screenPosition.y + panelPositionDistance, 0);//Camera.main.WorldToScreenPoint(new Vector3(towerColliders[i].transform.position.x, towerColliders[i].transform.position.y, 0));
            panel.ChangePanel.SetActive(false);

            panels[i] = panel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                for (int i = 0; i < towerColliders.Length; i++)
                {

                    if (panels[i].BuyPanel.activeSelf)
                        panels[i].BuyPanel.SetActive(false);
                    if (panels[i].ChangePanel.activeSelf)
                        panels[i].ChangePanel.SetActive(false);
                }
            }


            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "TowerSide")
                {
                    for (int i = 0; i < towerColliders.Length; i++)
                    {
                        if (towerColliders[i] == hit.collider)
                        {
                            if (turrets[i] == null)
                            {
                                if (!panels[i].BuyPanel.activeSelf)
                                    panels[i].BuyPanel.SetActive(true);
                                else if (panels[i].BuyPanel.activeSelf)
                                    panels[i].BuyPanel.SetActive(false);
                            }
                            else if (turrets[i] != null)
                            {
                                if (!panels[i].ChangePanel.activeSelf)
                                    panels[i].ChangePanel.SetActive(true);
                                else if (panels[i].ChangePanel.activeSelf)
                                    panels[i].ChangePanel.SetActive(false);
                            }

                            currentTowerSide = i;

                            break;
                        }
                    }

                    //if (!EventSystem.current.IsPointerOverGameObject())
                    //Instantiate(turrel1[0], new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, 0), Quaternion.Euler(0, 0, 180));
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            //var panel = Instantiate(BuyPanel, Canvas.transform, false);//, Vector3.zero, Quaternion.identity);
            //var rectTransform = panel.GetComponent<RectTransform>();
            //rectTransform.position = Input.mousePosition;
            //Debug.Log(Input.mousePosition);
        }

    }

    public void Buy(int trackNum)
    {
        var currentTowerSidePosition = towerColliders[currentTowerSide].transform.position;

        turrets[currentTowerSide] = Instantiate(turretExamplesArray[trackNum].turretSeriesIndexArray[0], new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));
        panels[currentTowerSide].BuyPanel.SetActive(false);

        TurrelTypes turretType = new TurrelTypes();
        turretType.turretExample = trackNum;
        turretType.turretSeriesIndex = 0;

        turretTypes[currentTowerSide] = turretType;
    }

    public void Upgrade()
    {
        int turretExampleLength = turretExamplesArray[turretTypes[currentTowerSide].turretExample].turretSeriesIndexArray.Length - 1;

        if (turretTypes[currentTowerSide].turretSeriesIndex < turretExampleLength)
        {
            turretTypes[currentTowerSide].turretSeriesIndex++;

            Destroy(turrets[currentTowerSide]);
            var currentTowerSidePosition = towerColliders[currentTowerSide].transform.position;
            turrets[currentTowerSide] = Instantiate(turretExamplesArray[turretTypes[currentTowerSide].turretExample].turretSeriesIndexArray[turretTypes[currentTowerSide].turretSeriesIndex], new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));
            panels[currentTowerSide].ChangePanel.SetActive(false);

            if (turretTypes[currentTowerSide].turretSeriesIndex == turretExampleLength)
                panels[currentTowerSide].ChangePanel.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Sale()
    {
        Destroy(turrets[currentTowerSide]);
        turrets[currentTowerSide] = null;

        panels[currentTowerSide].ChangePanel.transform.GetChild(0).gameObject.SetActive(true);

        panels[currentTowerSide].ChangePanel.SetActive(false);

        Array.Clear(turretTypes, currentTowerSide, 1);
        //turretTypes[currentTowerSide].turretExample = -1;
        //turretTypes[currentTowerSide].turretSeriesIndex = -1;
    }
}
