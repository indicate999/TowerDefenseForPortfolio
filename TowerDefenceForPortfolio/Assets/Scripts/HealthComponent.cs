using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
    private float _maxHealthAmount;
    private float _healthAmount;

    public float MaxHealthAmount 
    { 
        set {
            _maxHealthAmount = value;
            _healthAmount = _maxHealthAmount;
        } 
    }

    public event Action<float, float> GotHurt;
    public event Action Died;

    public void TakeDamage(float damage)
    {
        _healthAmount -= damage;

        if (_healthAmount > 0)
        {
            GotHurt?.Invoke(_healthAmount, _maxHealthAmount);
        }
        else if (_healthAmount <= 0)
        {
            Died?.Invoke();
        }
    }
}