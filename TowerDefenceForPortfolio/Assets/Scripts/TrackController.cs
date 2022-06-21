using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackController : MonoBehaviour
{
    public Transform[] trackRoutePoints;
    [SerializeField] private GameObject[] trackExamples;
    //public float rotateStartDistance;

    [HideInInspector] public List<GameObject> activeTracks = new List<GameObject>();

    private int waveNum = 1;

    //public Text textCoinCount;
    //public float startCoinCount;
    //[HideInInspector] public float coinCount;

    [HideInInspector] public static TrackController instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //coinCount = startCoinCount;
        //textCoinCount.text = coinCount.ToString();
        //StartCoroutine(CreateTracks(0, 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTracks.Count == 0)
        {
            int randomTrackType = Random.Range(0, trackExamples.Length);
            int trackCount = 0;
            if (randomTrackType == 0)
            {
                trackCount = waveNum * 2;
            }
            else if (randomTrackType == 1)
            {
                trackCount = waveNum;
            }
            StartCoroutine(CreateTracks(randomTrackType, trackCount));
            waveNum++;
        }
    }

    IEnumerator CreateTracks(int trackType, int trackCount)
    {
        float delay = trackExamples[trackType].GetComponent<Track>().delay;

        for (int i = 0; i < trackCount; i++)
        {
            activeTracks.Add(Instantiate(trackExamples[trackType], trackRoutePoints[0].position, Quaternion.identity));
            yield return new WaitForSeconds(delay);
        }
    }

    //private void CreateHalfTrack()
    //{
    //    Instantiate(tracks[0], trackPoints[0].position, Quaternion.identity);
    //}
}
