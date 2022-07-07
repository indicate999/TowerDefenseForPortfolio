using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        ChangeHealthBarPosition();
    }

    private void ChangeHealthBarPosition()
    {
        _slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + _offset);
    }

    public void SetHealthValue(float curruntHealth, float maxHealth)
    {
        _slider.gameObject.SetActive(curruntHealth < maxHealth);
        _slider.value = curruntHealth;
        _slider.maxValue = maxHealth;
    }
}
