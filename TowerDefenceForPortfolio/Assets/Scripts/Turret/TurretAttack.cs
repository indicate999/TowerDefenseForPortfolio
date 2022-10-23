using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class TurretAttack : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _cirleCollider;

    private float _damage;
    private float _attackSpeed;
    private float _attackRadius;

    private List<GameObject> _availableEnemies = new List<GameObject>();

    private GameObject _targetEnemy;

    private bool _isFire = false;

    public event Action StartedAttack;
    public event Action StoppedAttack;


    private void Start()
    {
        SetAttackRadiusInCollider();
    }

    private void Update()
    {
        SearchAttackTarget();
        StopAttackIfTargetNotAvailable();
        AttackProcess();
    }

    private void SetAttackRadiusInCollider()
    {
        _cirleCollider.radius = _attackRadius;
    }

    private void SearchAttackTarget()
    {
        if (!_isFire)
        {
            if (_availableEnemies.Any())
            {
                _targetEnemy = _availableEnemies.OrderBy(s => s.GetComponent<EnemyContainer>().EnemyId).First();

                StartedAttack?.Invoke();
                _isFire = true;
            }
        }
    }

    private void StopAttackIfTargetNotAvailable()
    {
        if (_isFire && !_availableEnemies.Find(s => s == _targetEnemy))
        {
            StoppedAttack?.Invoke();
            _targetEnemy = null;
            _isFire = false;
        }
    }

    private void AttackProcess()
    {
        if (_isFire)
        {
            TurretRotationTowardsTarget();
            TargetTakeDamage();
        }
    }

    private void TurretRotationTowardsTarget()
    {
        Vector3 dir = _targetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void TargetTakeDamage()
    {
        _targetEnemy.GetComponent<HealthComponent>().TakeDamage(_damage * _attackSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            _availableEnemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            _availableEnemies.Remove(collision.gameObject);
        }
    }

    public void SetAttackParameters(float damage, float attackSpeed, float attackRadius)
    {
        _damage = damage;
        _attackSpeed = attackSpeed;
        _attackRadius = attackRadius;

        SetAttackRadiusInCollider();
    }
}