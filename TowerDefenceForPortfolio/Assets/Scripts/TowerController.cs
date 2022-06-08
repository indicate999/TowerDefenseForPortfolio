using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    public GameObject[] turrel1;
    public GameObject[] turrel2;
    public Collider2D[] towerColliders;

    public GameObject Canvas;
    public GameObject BuyPanel;
    public GameObject ChangePanel;

    public float panelPositionDistance;

    private GameObject[] towers;
    private GameObject[,] panels;
    private int[,] turrelTypes;

    private int currentTowerSide;

    // Start is called before the first frame update
    private void Awake()
    {
        towers = new GameObject[towerColliders.Length];
        panels = new GameObject[towerColliders.Length, 2];
        turrelTypes = new int[2, towerColliders.Length];

        for(int i = 0; i < towerColliders.Length; i++)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(towerColliders[i].transform.position);
            
            panels[i, 0] = Instantiate(BuyPanel, Canvas.transform, false);
            var rectTransform = panels[i, 0].GetComponent<RectTransform>();
            rectTransform.position = new Vector3(screenPosition.x, screenPosition.y + panelPositionDistance);
            panels[i, 0].SetActive(false);

            panels[i, 1] = Instantiate(ChangePanel, Canvas.transform, false);
            var rectTransform2 = panels[i, 1].GetComponent<RectTransform>();
            rectTransform2.position = new Vector3(screenPosition.x, screenPosition.y + panelPositionDistance);//Camera.main.WorldToScreenPoint(new Vector3(towerColliders[i].transform.position.x, towerColliders[i].transform.position.y, 0));
            panels[i, 1].SetActive(false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
                            if (towers[i] == null)
                            {
                                if (!panels[i, 0].activeSelf)
                                    panels[i, 0].SetActive(true);
                                else if (panels[i, 0].activeSelf)
                                    panels[i, 0].SetActive(false);
                            }
                            else if (towers[i] != null)
                            {
                                if (!panels[i, 1].activeSelf)
                                    panels[i, 1].SetActive(true);
                                else if (panels[i, 1].activeSelf)
                                    panels[i, 1].SetActive(false);
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

        if (trackNum == 0)
        {
            towers[currentTowerSide] = Instantiate(turrel1[0], new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));
        }
        else if (trackNum == 1)
        {
            towers[currentTowerSide] = Instantiate(turrel2[0], new Vector3(currentTowerSidePosition.x, currentTowerSidePosition.y, 0), Quaternion.Euler(0, 0, 180));
        }

        turrelTypes[trackNum ,currentTowerSide] = 0;
        panels[currentTowerSide, 0].SetActive(false);
    }

    public void Upgrade()
    {
        if (towers[currentTowerSide] == turrel1[0])
            Debug.Log("ffff");
    }

    public void Sale()
    {
        Destroy(towers[currentTowerSide]);
        towers[currentTowerSide] = null;

        panels[currentTowerSide, 1].SetActive(false);
    }
}
