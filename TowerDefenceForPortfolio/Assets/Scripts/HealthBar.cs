using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 offset;

    //HelthBar is a child of the tracks and its position changes relative to the parent
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    //When the track receives damage this method is called and the health track display is updated in HelthBar
    public void SetHealthValue(float curruntHealth, float maxHealth)
    {
        slider.gameObject.SetActive(curruntHealth < maxHealth);
        slider.value = curruntHealth;
        slider.maxValue = maxHealth;
    }
}
