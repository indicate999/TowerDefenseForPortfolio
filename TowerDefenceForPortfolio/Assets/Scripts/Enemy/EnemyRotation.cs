using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyRotation : MonoBehaviour
{
    [SerializeField] private Transform _enemyBody;

    private Transform[] _enemyRoutePoints;
    private float _speed;
    private float _rotationSpeed;
    private float _rotateStartDistance;

    private bool _isRotate = false;

    private Quaternion _rotationTarget;
    private float _activeRotationZ;

    private const float _rotationZRight = -90;
    private const float _rotationZLeft = 90;
    private const float _rotationZUp = 0;
    private const float _rotationZDown = 180;

    private EnemyMovement _enemyMovement;

    private void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();

        SetStartEnemyRotation();
    }

    private void Update()
    {
        SetEnemyRotationValue();
        MakeEnemyRotation();
    }

    private void SetStartEnemyRotation()
    {
        var startDirection = RouteDirection(1);
        float startZRotation = 0;

        if (startDirection == Vector3.right)
            startZRotation = _rotationZRight;
        else if (startDirection == Vector3.left)
            startZRotation = _rotationZLeft;
        else if (startDirection == Vector3.up)
            startZRotation = _rotationZUp;
        else if (startDirection == Vector3.down)
            startZRotation = _rotationZDown;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startZRotation);
    }

    private void SetEnemyRotationValue()
    {
        if (_enemyMovement.NextRoutePoint < _enemyRoutePoints.Length - 1 && !_isRotate)
        {
            var currentDirection = RouteDirection(_enemyMovement.NextRoutePoint);
            var furureDirection = RouteDirection(_enemyMovement.NextRoutePoint + 1);

            float rotateUpStartDistance = _enemyRoutePoints[_enemyMovement.NextRoutePoint].position.y - _rotateStartDistance;
            bool isRotateUpStarting = transform.position.y > rotateUpStartDistance;

            float rotateDownStartDistance = _enemyRoutePoints[_enemyMovement.NextRoutePoint].position.y + _rotateStartDistance;
            bool isRotateDownStarting = transform.position.y < rotateDownStartDistance;

            float rotateRightStartDistance = _enemyRoutePoints[_enemyMovement.NextRoutePoint].position.x - _rotateStartDistance;
            bool isRotateRightStarting = transform.position.x > rotateRightStartDistance;

            float rotateLeftStartDistance = _enemyRoutePoints[_enemyMovement.NextRoutePoint].position.x + _rotateStartDistance;
            bool isRotateLeftStarting = transform.position.x < rotateLeftStartDistance;

            if (currentDirection == Vector3.up && isRotateUpStarting || currentDirection == Vector3.down && isRotateDownStarting)
            {
                if (furureDirection == Vector3.right)
                    _activeRotationZ = _rotationZRight;
                else if (furureDirection == Vector3.left)
                    _activeRotationZ = _rotationZLeft;

                _rotationTarget = Quaternion.Euler(0, 0, _activeRotationZ);
                _isRotate = true;
            }
            else if (currentDirection == Vector3.right && isRotateRightStarting || currentDirection == Vector3.left && isRotateLeftStarting)
            {
                if (furureDirection == Vector3.up)
                    _activeRotationZ = _rotationZUp;
                else if (furureDirection == Vector3.down)
                    _activeRotationZ = _rotationZDown;

                _rotationTarget = Quaternion.Euler(0, 0, _activeRotationZ);
                _isRotate = true;
            }
        }
    }

    private void MakeEnemyRotation()
    {
        if (_isRotate)
        {
            var roatationStep = _speed * _rotationSpeed * Time.deltaTime;
            _enemyBody.rotation = Quaternion.RotateTowards(_enemyBody.rotation, _rotationTarget, roatationStep);

            if (_enemyBody.rotation == _rotationTarget)
                _isRotate = false;
        }
    }

    public Vector3 RouteDirection(int point)
    {
        var heading = _enemyRoutePoints[point].position - _enemyRoutePoints[point - 1].position;
        return heading.normalized;
    }

    public void SetRotationParameters(Transform[] trackRoutePoints, float speed, float rotationSpeed, float rotateStartDistance)
    {
        _enemyRoutePoints = trackRoutePoints;
        _speed = speed;
        _rotationSpeed = rotationSpeed;
        _rotateStartDistance = rotateStartDistance;
    }
}
