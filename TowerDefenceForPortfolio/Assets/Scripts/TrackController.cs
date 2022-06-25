using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackController : MonoBehaviour
{
    public Transform[] trackRoutePoints;

    [SerializeField] private GameObject[] trackExamples;

    //Here are stored all the tracks that are on the scene
    [HideInInspector] public List<GameObject> activeTracks = new List<GameObject>();

    private int waveNum = 1;

    [HideInInspector] public static TrackController instance;

    private void Awake()
    {
        instance = this;
    }

    //In the Update method, tracks are created on the scene by calling carutina.
    //Every time all tracks are destroyed and the list of active tracks is empty, a new wave starts with the creation of new tracks.
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

    //In this carutin, tracks are created with a certain delay.
    IEnumerator CreateTracks(int trackType, int trackCount)
    {
        float delay = trackExamples[trackType].GetComponent<Track>().creationDelay;

        for (int i = 0; i < trackCount; i++)
        {
            activeTracks.Add(Instantiate(trackExamples[trackType], new Vector3(trackRoutePoints[0].position.x, trackRoutePoints[0].position.y, 0), Quaternion.identity));
            yield return new WaitForSeconds(delay);
        }
    }
}
