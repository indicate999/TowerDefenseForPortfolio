using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    private float _damage;
    private float _attackSpeed;
    private float _attackRadius;

    private Queue<GameObject> _enemies = new Queue<GameObject>();

    private bool isFire = false;

    void Start()
    {

    }

    private void Update()
    {
        SearchAttackTarget();
        StopAttackIfTargetWasDestroyed();
        AttackProcess();
    }

    public void SetAttackParameters(float damage, float attackSpeed, float attackRadius)
    {
        _damage = damage;
        _attackSpeed = attackSpeed;
        _attackRadius = attackRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            _enemies.Enqueue(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            _enemies.Dequeue();
        }
    }

    private void SearchAttackTarget()
    {
        //if (!_isFire)
        //{
        //IEnumerable<GameObject> list = _trackController.ActiveTracks;
        //IEnumerable<GameObject> result = list.Reverse();

        //foreach (var track in result)
        //{
        //    StartAttackIfTrackWithinReach(track);
        //}
        //}
    }

    private void StartAttackIfTrackWithinReach(GameObject track)
    {
        bool canFire = GetDistanceToTarget(track.transform) <= _attackRadius;
        if (canFire)
            StartFire(track);
    }

    private void StopAttackIfTargetWasDestroyed()
    {
        //if (_isFire && !_trackController.ActiveTracks.Contains(_targetTrack))
        //   StopFire();
    }

    private void AttackProcess()
    {
        //if (_isFire)
        //{
        TurretRotationTowardsTarget();
        TargetTakeDamage();
        StopAttackIfTargetOutOfReach();
        //}
    }

    private void TurretRotationTowardsTarget()
    {
        //Vector3 dir = _targetTrack.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void TargetTakeDamage()
    {
        //_targetTrack.GetComponent<Track>().TakeDamage(_damage * _attackSpeed * Time.deltaTime);
    }

    private void StopAttackIfTargetOutOfReach()
    {
        //if (GetDistanceToTarget(_targetTrack.transform) > _attackRadius)
        StopFire();
    }

    private float GetDistanceToTarget(Transform target)
    {
        return Vector3.Distance(target.position, transform.position);
    }

    private void StartFire(GameObject track)
    {
        //_animator.SetBool("isFire", true);
        //_isFire = true;
        //_targetTrack = track;

        //_audiosource.loop = true;
        //_audiosource.Play();
    }

    private void StopFire()
    {
        //_animator.SetBool("isFire", false);
        //_isFire = false;
        //_targetTrack = null;

        //_audiosource.loop = false;
        //_audiosource.Stop();
    }
}