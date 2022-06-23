using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private Text textHeartCount;
    [SerializeField] private Text textCoinCount;
    public GameObject RestartPanel;

    //With the help of these methods, the UI text of hearts and coins is updated
    public void UpdateHearts()
    {
        textHeartCount.text = Stats.heartCount.ToString();
    }

    public void UpdateCoins()
    {
        textCoinCount.text = Stats.coinCount.ToString();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
