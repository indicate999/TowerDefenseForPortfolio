using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyService : MonoBehaviour
{
    private float _coinCount;

    [SerializeField] GameSettingsDTO _gameSettingsDTO;
    [SerializeField] private UI _ui;

    private GameSettingsData _gameSettings;

    private void Start()
    {
        _gameSettings = _gameSettingsDTO.GameSettings;

        StartCoinsUpdate();
    }

    private void StartCoinsUpdate()
    {
        _coinCount = _gameSettings.StartCoinCount;
        _ui.UpdateCoins(_coinCount);
    }

    public bool EnoughCoinsCheck(float price)
    {
        return price <= _coinCount;
    }

    public void AddCoins(float earnPrice)
    {
        _coinCount += earnPrice;
        _ui.UpdateCoins(_coinCount);
    }

    public void RemoveCoins(float purchasePrice)
    {
        if (_coinCount >= purchasePrice)
        {
            _coinCount -= purchasePrice;
            _ui.UpdateCoins(_coinCount);
        }
    }
}
