using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text _textHeartCount;
    [SerializeField] private Text _textCoinCount;
    [SerializeField] private GameObject _restartPanel;

    public void UpdateHearts(float heartCount)
    {
        _textHeartCount.text = heartCount.ToString();
    }

    public void UpdateCoins(float coinCount)
    {
        _textCoinCount.text = coinCount.ToString();
    }

    public void ActivateRestartPanel()
    {
        _restartPanel.SetActive(true);
    }
}
