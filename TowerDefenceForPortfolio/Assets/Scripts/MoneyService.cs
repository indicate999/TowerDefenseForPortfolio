using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyService : MonoBehaviour
{
    private float _coinCount;

    private GameSettingsData _gameSettings;
    private UI _ui;

    // Start is called before the first frame update
    void Awake()
    {
        _gameSettings = FindObjectOfType<GameSettingsDTO>().GameSettings;
        _ui = FindObjectOfType<UI>();
    }

    private void Start()
    {
        StartCoinsUpdate();
    }

    private void StartCoinsUpdate()
    {
        _coinCount = _gameSettings.StartCoinCount;
        _ui.UpdateCoins(_coinCount);
    }

    public bool EnoughCoinsCheck(float price)
    {
        if (price <= _coinCount)
            return true;
        else
            return false;
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
