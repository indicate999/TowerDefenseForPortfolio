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
    private float startZrotation;

    [HideInInspector] public static TrackController instance;

    private void Awake()
    {
        instance = this;
    }

    //In Start method, the rotation of the track is set depending on the direction of its further movement.
    private void Start()
    {
        var startDirection = RouteDirection(1);

        if (startDirection == Vector3.right)
            startZrotation = -90;
        else if (startDirection == Vector3.left)
            startZrotation = 90;
        if (startDirection == Vector3.up)
            startZrotation = 0;
        else if (startDirection == Vector3.down)
            startZrotation = 180;
    }

    //In the Update method, tracks are created on the scene by calling carutina
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
            activeTracks.Add(Instantiate(trackExamples[trackType], trackRoutePoints[0].position, Quaternion.Euler(0, 0, startZrotation)));
            yield return new WaitForSeconds(delay);
        }
    }

    //This method calculates the direction between the current and previous route points
    public Vector3 RouteDirection(int point)
    {
        var heading = TrackController.instance.trackRoutePoints[point].position - TrackController.instance.trackRoutePoints[point - 1].position;
        return heading.normalized;
    }
}
