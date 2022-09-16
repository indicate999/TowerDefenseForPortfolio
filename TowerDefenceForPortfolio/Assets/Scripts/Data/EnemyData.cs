using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "My Assets/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _maxHealthAmount;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotateStartDistance;
    [SerializeField] private float _creationDelay;
    [SerializeField] private float _reward;
    [SerializeField] private int _spawnMultiplier;

    public Sprite Sprite { get { return _sprite; } }
    public float MaxHealthAmount { get { return _maxHealthAmount; } }
    public float Speed { get { return _speed; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public float RotateStartDistance { get { return _rotateStartDistance; } }
    public float CreationDelay { get { return _creationDelay; } }
    public float Reward { get { return _reward; } }
    public int SpawnMultiplier { get { return _spawnMultiplier; } }
}
