using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDTO : MonoBehaviour
{
    [SerializeField] private TurretData[] _turretExamples;

    public TurretData.Turret GetTurretByIndices(int exampleIndex, int serieIndex)
    {
        return _turretExamples[exampleIndex].TurretSeries[serieIndex];
    }

    public int GetTurretSeriesLength(int exampleIndex)
    {
        return _turretExamples[exampleIndex].TurretSeries.Length;
    }

    public AudioClip GetTurretAudioClip(int exampleIndex)
    {
        return _turretExamples[exampleIndex].AudioClip;
    }
}
