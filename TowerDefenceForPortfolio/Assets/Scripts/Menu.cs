using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _chooseLevelPanel;
    [SerializeField] private GameObject _levelButton;

    [SerializeField] private float _levelButtonsXdistance;

    private int _levelCount;
    private GameObject[] _activeLevelButtons;

    private void Awake()
    {
        _levelCount = SceneManager.sceneCountInBuildSettings - 1;
        _activeLevelButtons = new GameObject[_levelCount];
    }

    private void Start()
    {
        LevelButtonsCreation();
    }

    private void LevelButtonsCreation()
    {
        float PosX = -((_levelCount - 1) * _levelButtonsXdistance) / 2;

        for (int i = 0; i < _levelCount; i++)
        {
            _activeLevelButtons[i] = Instantiate(_levelButton, _chooseLevelPanel.transform, false);
            int levelNum = i + 1;
            _activeLevelButtons[i].transform.GetChild(0).GetComponent<Text>().text = levelNum.ToString();
            _activeLevelButtons[i].GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(levelNum); });
            var rectTransform = _activeLevelButtons[i].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(PosX, 0, 0);
            PosX += _levelButtonsXdistance;
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
