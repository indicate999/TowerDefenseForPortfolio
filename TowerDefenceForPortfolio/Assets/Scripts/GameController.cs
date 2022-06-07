using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform[] trackPoints;
    public GameObject[] tracks;
    //public float rotateStartDistance;

    [HideInInspector]
    public static GameController instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        

        CreateHalfTrack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateHalfTrack()
    {
        Instantiate(tracks[0], trackPoints[0].position, Quaternion.identity);
    }
}
