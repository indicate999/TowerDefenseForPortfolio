using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private float maxHealthAmount;
    private float healthAmount;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotateStartDistance;

    public float creationDelay;

    [SerializeField] private float reward;

    private int nextRoutePoint = 1;

    private bool isRotate = false;

    private Quaternion rotationTarget;
    private float activeRotationZ;

    private float rotationZRight = -90;
    private float rotationZLeft = 90;
    private float rotationZUp = 0;
    private float rotationZDown = 180;

    private Vector3 normalizedNextRoutePointPosition;

    private Main main;
    private SoundEffector soundEffector;

    private void Awake()
    {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
        soundEffector = GameObject.FindGameObjectWithTag("SoundEffector").GetComponent<SoundEffector>();
    }

    void Start()
    {
        healthAmount = maxHealthAmount;

        //In Start method, the rotation of the track is set depending on the direction of its further movement.
        var startDirection = RouteDirection(1);
        float startZRotation = 0;

        if (startDirection == Vector3.right)
            startZRotation = rotationZRight;
        else if (startDirection == Vector3.left)
            startZRotation = rotationZLeft;
        if (startDirection == Vector3.up)
            startZRotation = rotationZUp;
        else if (startDirection == Vector3.down)
            startZRotation = rotationZDown;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startZRotation);

        //In order not to set the variable (normalizedNextRoutePointPosition) to the position of the next route point every frame,
        //the value of the variable is set only at the start and every time the track reaches the route point and the next route point changes
        SetNormalizedNextRoutePointPosition();
    }

    void Update()
    {
        //Tracks move from point to point of the route
        transform.position = Vector3.MoveTowards(transform.position, normalizedNextRoutePointPosition, speed * Time.deltaTime);

        if (transform.position == normalizedNextRoutePointPosition)
        {
            nextRoutePoint++;

            //In order not to set the variable (normalizedNextRoutePointPosition) to the position of the next route point every frame,
            //the value of the variable is set only at the start and every time the track reaches the route point and the next route point changes
            if (nextRoutePoint < TrackController.instance.trackRoutePoints.Length)
                SetNormalizedNextRoutePointPosition();
            //If the track reaches the last point, it is immediately destroyed and the player receives a reward of coins.
            else if (nextRoutePoint == TrackController.instance.trackRoutePoints.Length)
            {
                TrackController.instance.activeTracks.Remove(this.gameObject);
                Destroy(this.gameObject);

                Stats.heartCount--;
                main.UpdateHearts();

                if (Stats.heartCount == 0)
                {
                    main.RestartPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }

        if (nextRoutePoint < TrackController.instance.trackRoutePoints.Length - 1 && !isRotate)
        {
            //Here the current and future direction of the track is calculated depending on the location of the route points
            var currentDirection = RouteDirection(nextRoutePoint);
            var furureDirection = RouteDirection(nextRoutePoint + 1);

            //Depending on the current and future direction, the code calculates in which direction the track will turn
            //and the corresponding value of the angle of rotation along the z axis is assigned
            if (currentDirection == Vector3.up && transform.position.y > TrackController.instance.trackRoutePoints[nextRoutePoint].position.y - rotateStartDistance
                || currentDirection == Vector3.down && transform.position.y < TrackController.instance.trackRoutePoints[nextRoutePoint].position.y + rotateStartDistance)
            {
                if (furureDirection == Vector3.right)
                    activeRotationZ = rotationZRight;
                else if (furureDirection == Vector3.left)
                    activeRotationZ = rotationZLeft;

                isRotate = true;
            }
            else if (currentDirection == Vector3.right && transform.position.x > TrackController.instance.trackRoutePoints[nextRoutePoint].position.x - rotateStartDistance
                || currentDirection == Vector3.left && transform.position.x < TrackController.instance.trackRoutePoints[nextRoutePoint].position.x + rotateStartDistance)
            {
                if (furureDirection == Vector3.up)
                    activeRotationZ = rotationZUp;
                else if (furureDirection == Vector3.down)
                    activeRotationZ = rotationZDown;

                isRotate = true;
            }
        }

        //Here the rotation occurs until the track takes the desired angle of rotation
        if (isRotate)
        {
            rotationTarget = Quaternion.Euler(0, 0, activeRotationZ);
            var roatationStep = speed * Time.deltaTime * rotationSpeed; ;
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rotationTarget, roatationStep);

            if (transform.GetChild(0).rotation == rotationTarget)
                isRotate = false;
        }
    }

    //Sets a variable (normalizedNextRoutePointPosition) to the position of the next route point
    private void SetNormalizedNextRoutePointPosition()
    {
        var nextRoutePointPosition = TrackController.instance.trackRoutePoints[nextRoutePoint].position;
        normalizedNextRoutePointPosition = new Vector3(nextRoutePointPosition.x, nextRoutePointPosition.y, 0);
    }

    //This method calculates the direction between the current and previous route points
    public Vector3 RouteDirection(int point)
    {
        var heading = TrackController.instance.trackRoutePoints[point].position - TrackController.instance.trackRoutePoints[point - 1].position;
        return heading.normalized;
    }

    //This method is called when damage is received by the track. If the health of the track reaches zero,
    //the track is destroyed and the player receives a reward in coins.
    public void GetDamage(float damage)
    {
        healthAmount -= damage;

        if (healthAmount > 0)
        {
            transform.GetChild(1).gameObject.GetComponent<HealthBar>().SetHealthValue(healthAmount, maxHealthAmount);
        }
        else if (healthAmount <= 0)
        {
            soundEffector.PLayTrackExposionSound();
            
            TrackController.instance.activeTracks.Remove(this.gameObject);
            Destroy(this.gameObject);

            Stats.coinCount += reward;
            main.UpdateCoins();
        }
    }
}
