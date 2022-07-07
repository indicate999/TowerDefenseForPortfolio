using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private float _startHeartCount;
    private float _heartCount;

    [SerializeField] private float _startCoinCount;
    private float _coinCount;

    [SerializeField] private Main _main;

    private void Start()
    {
        StartStatsUpdate();
    }

    private void StartStatsUpdate()
    {
        _heartCount = _startHeartCount;
        _main.UpdateHearts(_heartCount);

        _coinCount = _startCoinCount;
        _main.UpdateCoins(_coinCount);
    }

    public void RemoveHeart()
    {
        _heartCount--;
        _main.UpdateHearts(_heartCount);

        if (_heartCount == 0)
        {
            _main.ActivateRestartPanel();
            Time.timeScale = 0;
        }
    }

    public void AddCoins(float earnPrice)
    {
        _coinCount += earnPrice;
        _main.UpdateCoins(_coinCount);
    }

    public bool RemoveCoins(float purchasePrice)
    {
        if (_coinCount >= purchasePrice)
        {
            _coinCount -= purchasePrice;
            _main.UpdateCoins(_coinCount);
            return true;
        }
        else
            return false;
    }
}
