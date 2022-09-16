using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetBarValue(float curruntHealth, float maxHealth)
    {
        _slider.gameObject.SetActive(curruntHealth < maxHealth);
        _slider.value = curruntHealth;
        _slider.maxValue = maxHealth;
    }
}
