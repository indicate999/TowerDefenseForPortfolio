using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private HealthComponent _levelHealth;
    [SerializeField] private UI _ui;

    [SerializeField] private GameSettingsDTO _gameSettingsDTO;

    private GameSettingsData _gameSettings;

    void Start()
    {
        _gameSettings = _gameSettingsDTO.GameSettings;

        SetLevelMaxHealthAmount();
        SubscribeLevelHealthEvents();
    }

    private void SetLevelMaxHealthAmount()
    {
        _levelHealth.MaxHealthAmount = _gameSettings.StartHeartCount;
        _ui.UpdateHearts(_gameSettings.StartHeartCount);
    }

    private void SubscribeLevelHealthEvents()
    {
        _levelHealth.GotHurt += UpdateLifes;
        _levelHealth.Died += GameOver;
    }

    private void UpdateLifes(GameObject obj, float healthAmount, float maxHealthAmount)
    {
        _ui.UpdateHearts(healthAmount);
    }

    private void GameOver(GameObject obj)
    {
        _ui.ActivateRestartPanel();
        Time.timeScale = 0;
    }
}
