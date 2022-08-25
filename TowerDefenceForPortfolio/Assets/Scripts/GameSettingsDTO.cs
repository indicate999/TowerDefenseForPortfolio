using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsDTO : MonoBehaviour
{
    [SerializeField] private GameSettingsData _gameSettings;

    public GameSettingsData GameSettings { get { return _gameSettings; } }
}
