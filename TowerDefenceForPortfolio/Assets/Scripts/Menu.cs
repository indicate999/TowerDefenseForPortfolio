using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject ChooseLevelPanel;
    [SerializeField] private GameObject LevelButton;

    [SerializeField] private float levelButtonsXdistance;

    private GameObject[] activeLevelButtons;
    private int levelCount;

    private void Awake()
    {
        levelCount = SceneManager.sceneCountInBuildSettings - 1;
        activeLevelButtons = new GameObject[levelCount];
    }

    //When adding new levels to the build, the corresponding buttons in the menu automatically appear
    void Start()
    {
        float PosX = -((levelCount - 1) * levelButtonsXdistance) / 2;

        for (int i = 0; i < levelCount; i++)
        {
            activeLevelButtons[i] = Instantiate(LevelButton, ChooseLevelPanel.transform, false);
            int levelNum = i + 1;
            activeLevelButtons[i].transform.GetChild(0).GetComponent<Text>().text = levelNum.ToString();
            activeLevelButtons[i].GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(levelNum); });
            var rectTransform = activeLevelButtons[i].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(PosX, 0, 0);
            PosX += levelButtonsXdistance;
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
