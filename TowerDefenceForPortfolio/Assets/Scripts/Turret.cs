using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Turret : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackRadius;

    [SerializeField] private float _purchasePrice;
    public float PurchasePrice { get { return _purchasePrice; } }

    [SerializeField] private float _sellingPrice;
    public float SellingPrice { get { return _sellingPrice; } }

    private GameObject _targetTrack;

    private bool _isFire = false;

    private Animator _animator;
    private AudioSource _audiosource;
    private TrackController _trackController;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audiosource = GetComponent<AudioSource>();
        _trackController = FindObjectOfType<TrackController>();
    }

    private void Update()
    {
        SearchAttackTarget();
        StopAttackIfTargetWasDestroyed();
        AttackProcess();
    }

    private void SearchAttackTarget()
    {
        if (!_isFire)
        {
            IEnumerable<GameObject> list = _trackController.ActiveTracks;
            IEnumerable<GameObject> result = list.Reverse();

            foreach (var track in result)
            {
                StartAttackIfTrackWithinReach(track);
            }
        }
    }

    private void StartAttackIfTrackWithinReach(GameObject track)
    {
        bool canFire = GetDistanceToTarget(track.transform) <= _attackRadius;
        if (canFire)
            StartFire(track);
    }

    private void StopAttackIfTargetWasDestroyed()
    {
        if (_isFire && !_trackController.ActiveTracks.Contains(_targetTrack))
            StopFire();
    }

    private void AttackProcess()
    {
        if (_isFire)
        {
            TurretRotationTowardsTarget();
            TargetTakeDamage();
            StopAttackIfTargetOutOfReach();
        }
    }

    private void TurretRotationTowardsTarget()
    {
        Vector3 dir = _targetTrack.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void TargetTakeDamage()
    {
        _targetTrack.GetComponent<Track>().TakeDamage(_damage * _attackSpeed * Time.deltaTime);
    }

    private void StopAttackIfTargetOutOfReach()
    {
        if(GetDistanceToTarget(_targetTrack.transform) > _attackRadius)
                StopFire();
    }

    private float GetDistanceToTarget(Transform target)
    {
        return Vector3.Distance(target.position, transform.position);
    }

    private void StartFire(GameObject track)
    {
        _animator.SetBool("isFire", true);
        _isFire = true;
        _targetTrack = track;

        _audiosource.loop = true;
        _audiosource.Play();
    }

    private void StopFire()
    {
        _animator.SetBool("isFire", false);
        _isFire = false;
        _targetTrack = null;

        _audiosource.loop = false;
        _audiosource.Stop();
    }
}
