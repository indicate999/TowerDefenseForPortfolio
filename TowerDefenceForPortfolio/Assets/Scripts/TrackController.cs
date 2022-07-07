using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackController : MonoBehaviour
{
    [SerializeField] private Transform[] _trackRoutePoints;
    public Transform[] TrackRoutePoints { get { return _trackRoutePoints; } }

    [SerializeField] private GameObject[] _trackExamples;

    private List<GameObject> _activeTracks = new List<GameObject>();
    public List<GameObject> ActiveTracks { get { return _activeTracks; } }

    private int _waveNum = 1;

    private void Update()
    {
        CreateTrackWave();
    }

    private void CreateTrackWave()
    {
        if (_activeTracks.Count == 0)
        {
            int randomTrackType = Random.Range(0, _trackExamples.Length);
            int trackCount = 0;
            if (randomTrackType == 0)
            {
                trackCount = _waveNum * 2;
            }
            else if (randomTrackType == 1)
            {
                trackCount = _waveNum;
            }
            StartCoroutine(CreateTracks(randomTrackType, trackCount));
            _waveNum++;
        }
    }

    IEnumerator CreateTracks(int trackType, int trackCount)
    {
        float delay = _trackExamples[trackType].GetComponent<Track>().CreationDelay;

        for (int i = 0; i < trackCount; i++)
        {
            _activeTracks.Add(Instantiate(_trackExamples[trackType], new Vector3(_trackRoutePoints[0].position.x, _trackRoutePoints[0].position.y, 0), Quaternion.identity));
            yield return new WaitForSeconds(delay);
        }
    }

    public void RemoveElementFromActiveTracks(GameObject TrackObj)
    {
        _activeTracks.Remove(TrackObj);
    }
}
