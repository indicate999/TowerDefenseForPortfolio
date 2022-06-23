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
    private float rotationZ;

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
    }

    void Update()
    {
        //Tracks move from point to point of the route
        transform.position = Vector3.MoveTowards(transform.position, TrackController.instance.trackRoutePoints[nextRoutePoint].position, speed * Time.deltaTime);

        if (transform.position == TrackController.instance.trackRoutePoints[nextRoutePoint].position)
        {
            nextRoutePoint++;
        }

        //If the track reaches the last point, it is immediately destroyed and the player receives a reward of coins.
        if (nextRoutePoint == TrackController.instance.trackRoutePoints.Length)
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


        if (nextRoutePoint < TrackController.instance.trackRoutePoints.Length - 1 && !isRotate)
        {
            //Here the current and future direction of the track is calculated depending on the location of the route points
            var currentDirection = TrackController.instance.RouteDirection(nextRoutePoint);
            var furureDirection = TrackController.instance.RouteDirection(nextRoutePoint + 1);

            //Depending on the current and future direction, the code calculates in which direction the track will turn
            //and the corresponding value of the angle of rotation along the z axis is assigned
            if (currentDirection == Vector3.up && transform.position.y > TrackController.instance.trackRoutePoints[nextRoutePoint].position.y - rotateStartDistance
                || currentDirection == Vector3.down && transform.position.y < TrackController.instance.trackRoutePoints[nextRoutePoint].position.y + rotateStartDistance)
            {
                if (furureDirection == Vector3.right)
                    rotationZ = -90;
                else if (furureDirection == Vector3.left)
                    rotationZ = 90;

                isRotate = true;
            }
            else if (currentDirection == Vector3.right && transform.position.x > TrackController.instance.trackRoutePoints[nextRoutePoint].position.x - rotateStartDistance
                || currentDirection == Vector3.left && transform.position.x < TrackController.instance.trackRoutePoints[nextRoutePoint].position.x + rotateStartDistance)
            {
                if (furureDirection == Vector3.up)
                    rotationZ = 0;
                else if (furureDirection == Vector3.down)
                    rotationZ = 180;

                isRotate = true;
            }
        }

        //Here the rotation occurs until the track takes the desired angle of rotation
        if (isRotate)
        {
            rotationTarget = Quaternion.Euler(0, 0, rotationZ);
            var roatationStep = speed * Time.deltaTime * rotationSpeed; ;
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rotationTarget, roatationStep);

            if (transform.GetChild(0).rotation == rotationTarget)
                isRotate = false;
        }
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
