using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "My Assets/Game Settings Data")]
public class GameSettingsData : ScriptableObject
{
    [SerializeField] private float _startHeartCount;
    [SerializeField] private float _startCoinCount;

    public float StartHeartCount { get { return _startHeartCount; } }
    public float StartCoinCount { get { return _startCoinCount; } }
}
