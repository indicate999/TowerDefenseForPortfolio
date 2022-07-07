using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private float _maxHealthAmount;
    private float _healthAmount;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotateStartDistance;

    [SerializeField] private float _creationDelay;
    public float CreationDelay{ get { return _creationDelay; } }

    [SerializeField] private float _reward;

    private int _nextRoutePoint = 1;

    private bool _isRotate = false;

    private Quaternion _rotationTarget;
    private float _activeRotationZ;

    private const float _rotationZRight = -90;
    private const float _rotationZLeft = 90;
    private const float _rotationZUp = 0;
    private const float _rotationZDown = 180;

    private Vector3 _normalizedByZAxisNextRoutePointPosition;

    private Transform[] _trackRoutePoints;

    private SoundEffector _soundEffector;
    private Stats _stats;
    private TrackController _trackController;

    private void Awake()
    {
        _soundEffector = (SoundEffector)FindObjectOfType(typeof(SoundEffector));
        _stats = (Stats)FindObjectOfType(typeof(Stats));
        _trackController = (TrackController)FindObjectOfType(typeof(TrackController));

        _trackRoutePoints = _trackController.TrackRoutePoints;

        _healthAmount = _maxHealthAmount;
    }

    private void Start()
    {
        SetStartTrackRotation();
    }

    private void Update()
    {
        ChangeTrackPosition();
        ActionsAfterReachingRoutePoint();
        SetTrackRotationValue();
        MakeTrackRotation();
    }

    private void SetStartTrackRotation()
    {
        var startDirection = RouteDirection(1);
        float startZRotation = 0;

        if (startDirection == Vector3.right)
            startZRotation = _rotationZRight;
        else if (startDirection == Vector3.left)
            startZRotation = _rotationZLeft;
        if (startDirection == Vector3.up)
            startZRotation = _rotationZUp;
        else if (startDirection == Vector3.down)
            startZRotation = _rotationZDown;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startZRotation);

        SetNormalizedByZAxisNextRoutePointPosition();
    }

    private void ChangeTrackPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, _normalizedByZAxisNextRoutePointPosition, _speed * Time.deltaTime);
    }

    private void ActionsAfterReachingRoutePoint()
    {
        if (transform.position == _normalizedByZAxisNextRoutePointPosition)
        {
            _nextRoutePoint++;

            if (_nextRoutePoint < _trackRoutePoints.Length)
                SetNormalizedByZAxisNextRoutePointPosition();
            else if (_nextRoutePoint == _trackRoutePoints.Length)
            {
                ActionsAfterReachingLastRoutePoint();
            }
        }
    }

    private void ActionsAfterReachingLastRoutePoint()
    {
        _trackController.RemoveElementFromActiveTracks(this.gameObject);
        Destroy(this.gameObject);

        _stats.RemoveHeart();
    }

    private void SetTrackRotationValue()
    {
        if (_nextRoutePoint < _trackRoutePoints.Length - 1 && !_isRotate)
        {
            var currentDirection = RouteDirection(_nextRoutePoint);
            var furureDirection = RouteDirection(_nextRoutePoint + 1);

            float rotateUpStartDistance = _trackRoutePoints[_nextRoutePoint].position.y - _rotateStartDistance;
            bool isRotateUpStarting = transform.position.y > rotateUpStartDistance;

            float rotateDownStartDistance = _trackRoutePoints[_nextRoutePoint].position.y + _rotateStartDistance;
            bool isRotateDownStarting = transform.position.y < rotateDownStartDistance;

            float rotateRightStartDistance = _trackRoutePoints[_nextRoutePoint].position.x - _rotateStartDistance;
            bool isRotateRightStarting = transform.position.x > rotateRightStartDistance;

            float rotateLeftStartDistance = _trackRoutePoints[_nextRoutePoint].position.x + _rotateStartDistance;
            bool isRotateLeftStarting = transform.position.x < rotateLeftStartDistance;

            if (currentDirection == Vector3.up && isRotateUpStarting || currentDirection == Vector3.down && isRotateDownStarting)
            {
                if (furureDirection == Vector3.right)
                    _activeRotationZ = _rotationZRight;
                else if (furureDirection == Vector3.left)
                    _activeRotationZ = _rotationZLeft;

                _isRotate = true;
            }
            else if (currentDirection == Vector3.right && isRotateRightStarting || currentDirection == Vector3.left && isRotateLeftStarting)
            {
                if (furureDirection == Vector3.up)
                    _activeRotationZ = _rotationZUp;
                else if (furureDirection == Vector3.down)
                    _activeRotationZ = _rotationZDown;

                _isRotate = true;
            }
        }
    }

    private void MakeTrackRotation()
    {
        if (_isRotate)
        {
            _rotationTarget = Quaternion.Euler(0, 0, _activeRotationZ);
            var roatationStep = _speed * Time.deltaTime * _rotationSpeed; ;
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, _rotationTarget, roatationStep);

            if (transform.GetChild(0).rotation == _rotationTarget)
                _isRotate = false;
        }
    }

    private void SetNormalizedByZAxisNextRoutePointPosition()
    {
        var nextRoutePointPosition = _trackRoutePoints[_nextRoutePoint].position;
        _normalizedByZAxisNextRoutePointPosition = new Vector3(nextRoutePointPosition.x, nextRoutePointPosition.y, 0);
    }

    public Vector3 RouteDirection(int point)
    {
        var heading = _trackRoutePoints[point].position - _trackRoutePoints[point - 1].position;
        return heading.normalized;
    }

    public void GetDamage(float damage)
    {
        _healthAmount -= damage;

        if (_healthAmount > 0)
        {
            transform.GetChild(1).gameObject.GetComponent<HealthBar>().SetHealthValue(_healthAmount, _maxHealthAmount);
        }
        else if (_healthAmount <= 0)
        {
            _soundEffector.PLayTrackExposionSound();
            
            _trackController.RemoveElementFromActiveTracks(this.gameObject);
            Destroy(this.gameObject);

            _stats.AddCoins(_reward);
        }
    }
}
