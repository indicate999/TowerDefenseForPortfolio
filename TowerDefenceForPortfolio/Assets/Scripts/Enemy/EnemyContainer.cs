using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _enemySpriteRenderer;
    [SerializeField] private EnemyMovement _enemyMovementComponent;
    [SerializeField] private EnemyRotation _enemyRotationComponent;
    [SerializeField] private EnemyPointer _enemyPointerComponent;
    [SerializeField] private HealthComponent _healthComponent;
    private int _enemyId;
    private float _reward;

    public SpriteRenderer EnemySpriteRenderer { get { return _enemySpriteRenderer; } }
    public EnemyMovement EnemyMovementComponent { get { return _enemyMovementComponent; } }
    public EnemyRotation EnemyRotationComponent { get { return _enemyRotationComponent; } }
    public EnemyPointer EnemyPointerComponent { get { return _enemyPointerComponent; } }
    public HealthComponent HealthComponent { get { return _healthComponent; } }
    public int EnemyId { get { return _enemyId; } set { _enemyId = value; } }
    public float Reward { get { return _reward; } set { _reward = value; } }

}
