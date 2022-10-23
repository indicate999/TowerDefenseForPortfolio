using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(TurretAttack))]
public class TurretAnimation : MonoBehaviour
{
    private Animator _animator;
    private TurretAttack _turretAttack;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _turretAttack = GetComponent<TurretAttack>();

        SubscribeTurretAttackEvents();
    }

    private void SubscribeTurretAttackEvents()
    {
        _turretAttack.StartedAttack += StartAttackAnimation;
        _turretAttack.StoppedAttack += StopAttackAnimation;
    }

    private void StartAttackAnimation()
    {
        _animator.SetBool("isFire", true);
    }

    private void StopAttackAnimation()
    {
        _animator.SetBool("isFire", false);
    }
}
