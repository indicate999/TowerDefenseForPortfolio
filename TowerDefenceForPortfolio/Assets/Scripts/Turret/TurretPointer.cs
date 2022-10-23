using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPointer : MonoBehaviour
{
    private int _turretExampleIndex;
    private int _turretSerieIndex;

    public int TurretExampleIndex { get { return _turretExampleIndex; } }
    public int TurretSerieIndex { get { return _turretSerieIndex; } }

    public void SetIndicesForNewTurret(int exampleIndex)
    {
        _turretExampleIndex = exampleIndex;
        _turretSerieIndex = 0;
    }

    public void IncreaseTurretSeriesIndex()
    {
        _turretSerieIndex++;
    }
}