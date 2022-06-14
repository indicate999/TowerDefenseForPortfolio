using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform[] trackPoints;
    [SerializeField]
    private GameObject[] tracks;
    //public float rotateStartDistance;

    [HideInInspector]
    public List<GameObject> activeTracks = new List<GameObject>();

    [HideInInspector]
    public static GameController instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(CreateTracks(0, 6));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreateTracks(int trackType, int trackCount)
    {
        float delay = tracks[trackType].GetComponent<Track>().delay;

        for (int i = 0; i < trackCount; i++)
        {
            activeTracks.Add(Instantiate(tracks[trackType], trackPoints[0].position, Quaternion.identity));
            yield return new WaitForSeconds(delay);
        }
    }

    //private void CreateHalfTrack()
    //{
    //    Instantiate(tracks[0], trackPoints[0].position, Quaternion.identity);
    //}
}
