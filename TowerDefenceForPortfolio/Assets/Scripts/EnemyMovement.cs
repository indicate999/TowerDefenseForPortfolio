using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform[] _enemyRoutePoints;
    private float _speed;

    private Vector3 _normalizedByZAxisNextRoutePointPosition;
    private int _nextRoutePoint = 1;

    public int NextRoutePoint { get { return _nextRoutePoint; } }

    private void Start()
    {
        SetNormalizedByZAxisNextRoutePointPosition();
    }

    private void Update()
    {
        ChangeEnemyPosition();
        ActionsAfterReachingRoutePoint();
    }

    public void SetMovementParameters(Transform[] trackRoutePoints, float speed)
    {
        _enemyRoutePoints = trackRoutePoints;
        _speed = speed;
    }

    private void ChangeEnemyPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, _normalizedByZAxisNextRoutePointPosition, _speed * Time.deltaTime);
    }

    private void ActionsAfterReachingRoutePoint()
    {
        if (transform.position == _normalizedByZAxisNextRoutePointPosition)
        {
            _nextRoutePoint++;

            if (_nextRoutePoint < _enemyRoutePoints.Length)
                SetNormalizedByZAxisNextRoutePointPosition();
            else if (_nextRoutePoint >= _enemyRoutePoints.Length)
            {
                ActionsAfterReachingLastRoutePoint();
            }
        }
    }

    private void ActionsAfterReachingLastRoutePoint()
    {
        gameObject.SetActive(false);

        //_trackController.RemoveElementFromActiveTracks(this.gameObject);
        //Destroy(this.gameObject);

        //_stats.RemoveHeart();
    }

    private void SetNormalizedByZAxisNextRoutePointPosition()
    {
        var nextRoutePointPosition = _enemyRoutePoints[_nextRoutePoint].position;
        _normalizedByZAxisNextRoutePointPosition = new Vector3(nextRoutePointPosition.x, nextRoutePointPosition.y, 0);
    }
}
