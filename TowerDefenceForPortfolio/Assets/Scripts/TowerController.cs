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

    public GameObject collider1;

    private Camera cam;

    // Start is called before the first frame update
    private void Awake()
    {
        towers = new GameObject[towerColliders.Length];
        panels = new GameObject[towerColliders.Length, 2];

        //cam = GetComponent<Camera>();

        for(int i = 0; i < towerColliders.Length; i++)
        {
            //Debug.Log(towerColliders[i].transform.position);
            //Debug.Log(towerColliders[i].transform.localPosition);
            Debug.Log(Camera.main.WorldToScreenPoint(towerColliders[i].transform.position));
            Debug.Log("_____");

            panels[i, 0] = Instantiate(BuyPanel, Canvas.transform, false);
            var rectTransform = panels[i, 0].GetComponent<RectTransform>();
            rectTransform.position = Camera.main.WorldToScreenPoint(towerColliders[i].transform.position);//new Vector3(towerColliders[i].transform.position.x, towerColliders[i].transform.position.y, 0));
            Debug.Log(rectTransform.position);
            Debug.Log("gggggg");

            panels[i, 1] = Instantiate(ChangePanel, Canvas.transform, false);
            var rectTransform2 = panels[i, 1].GetComponent<RectTransform>();
            rectTransform2.position = Camera.main.WorldToScreenPoint(towerColliders[i].transform.position);//Camera.main.WorldToScreenPoint(new Vector3(towerColliders[i].transform.position.x, towerColliders[i].transform.position.y, 0));

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
                    //if (!EventSystem.current.IsPointerOverGameObject())
                        Instantiate(turrel1[0], new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, 0), Quaternion.Euler(0, 0, 180));
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var panel = Instantiate(BuyPanel, Canvas.transform, false);//, Vector3.zero, Quaternion.identity);
            var rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.position = Input.mousePosition;
            Debug.Log(Input.mousePosition);
        }
    }
}
