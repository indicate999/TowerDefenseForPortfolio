using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(TurretAttack))]
public class TurretSound : MonoBehaviour
{
    private AudioSource _audioSource;
    private TurretAttack _turretAttack;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _turretAttack = GetComponent<TurretAttack>();

        SubscribeTurretAttackEvents();
    }

    private void SubscribeTurretAttackEvents()
    {
        _turretAttack.StartedAttack += StartPlayAttackSound;
        _turretAttack.StoppedAttack += StopPlayAttackSound;
    }

    private void StartPlayAttackSound()
    {
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void StopPlayAttackSound()
    {
        _audioSource.loop = false;
        _audioSource.Stop();
    }
}
