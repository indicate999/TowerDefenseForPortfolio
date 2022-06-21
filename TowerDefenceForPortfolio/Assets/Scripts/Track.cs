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

    public float delay;

    public float reward;

    private int nextRoutePoint = 1;

    private enum RotateDirection
    {
        Right,
        Left,
        Up,
        Down
    }

    private bool isRotate = false;
    private RotateDirection rotatedirection;

    private Quaternion rotationTarget;
    private float rotationZ;

    private Main main;
    //private SceneController sceneController;
    private SoundEffector soundEffector;

    //[HideInInspector]
    //public bool isUnderAttack = false;

    private void Awake()
    {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
        //sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        soundEffector = GameObject.FindGameObjectWithTag("SoundEffector").GetComponent<SoundEffector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = maxHealthAmount;
        //transform.position = GameController.instance.trackPoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("sss");
        transform.position = Vector3.MoveTowards(transform.position, TrackController.instance.trackRoutePoints[nextRoutePoint].position, speed * Time.deltaTime);


        if (transform.position == TrackController.instance.trackRoutePoints[nextRoutePoint].position)
        {
            nextRoutePoint++;
        }

        //if (transform.position == GameController.instance.trackPoints[GameController.instance.trackPoints.Length - 1].position)
        if (nextRoutePoint == TrackController.instance.trackRoutePoints.Length)
        {
            //transform.position = TrackController.instance.trackRoutePoints[0].position;
            TrackController.instance.activeTracks.Remove(this.gameObject);
            Destroy(this.gameObject);

            Stats.heartCount--;
            main.UpdateHearts();

            if (Stats.heartCount == 0)
            {
                main.RestartPanel.SetActive(true);
                Time.timeScale = 0;
            }
            //nextRoutePoint = 1;
        }


        if (nextRoutePoint < TrackController.instance.trackRoutePoints.Length - 1 && !isRotate)
        {
            var currentDirection = RouteDirection(nextRoutePoint);

            if (currentDirection == Vector3.up && transform.position.y > TrackController.instance.trackRoutePoints[nextRoutePoint].position.y - rotateStartDistance)
            {

                //if (transform.position.y > GameController.instance.trackPoints[i].position.y - rotateStartDistance)
                //{
                //if (!isRotate)
                //{
                var furureDirection = RouteDirection(nextRoutePoint + 1);
                if (furureDirection == Vector3.right)
                    rotatedirection = RotateDirection.Right;
                else if (furureDirection == Vector3.left)
                    rotatedirection = RotateDirection.Left;
                //}

                isRotate = true;
                //}
            }
            else if (currentDirection == Vector3.right && transform.position.x > TrackController.instance.trackRoutePoints[nextRoutePoint].position.x - rotateStartDistance)
            {
                //if (transform.position.x > GameController.instance.trackPoints[i].position.x - rotateStartDistance)
                //{
                //if (!isRotate)
                //{
                var furureDirection = RouteDirection(nextRoutePoint + 1);
                if (furureDirection == Vector3.up)
                    rotatedirection = RotateDirection.Up;
                else if (furureDirection == Vector3.down)
                    rotatedirection = RotateDirection.Down;
                //}

                isRotate = true;
                //}
            }
            else if (currentDirection == Vector3.left && transform.position.x < TrackController.instance.trackRoutePoints[nextRoutePoint].position.x + rotateStartDistance)
            {
                //if (transform.position.x < GameController.instance.trackPoints[i].position.x + rotateStartDistance)
                //{
                //if (!isRotate)
                //{
                var furureDirection = RouteDirection(nextRoutePoint + 1);
                if (furureDirection == Vector3.up)
                    rotatedirection = RotateDirection.Up;
                else if (furureDirection == Vector3.down)
                    rotatedirection = RotateDirection.Down;
                //}

                isRotate = true;
                //}
            }

            //isRotate = true;
            if (isRotate)
            {

                if (rotatedirection == RotateDirection.Right)
                    rotationZ = -90;
                else if (rotatedirection == RotateDirection.Left)
                    rotationZ = 90;
                else if (rotatedirection == RotateDirection.Up)
                    rotationZ = 0;
                else if (rotatedirection == RotateDirection.Down)
                    rotationZ = 180;

                rotationTarget = Quaternion.Euler(0, 0, rotationZ);
            }

        }

        if (isRotate)
        {
            

            //if (rotatedirection == RotateDirection.Right)
            //    rotationZ = -90;
            //else if (rotatedirection == RotateDirection.Left)
            //    rotationZ = 90;
            //else if (rotatedirection == RotateDirection.Up)
            //    rotationZ = 0;
            //else if (rotatedirection == RotateDirection.Down)
            //    rotationZ = 180;
             

            var roatationStep = speed * Time.deltaTime * rotationSpeed; ;
            //rotationTarget = Quaternion.Euler(0, 0, rotationZ);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, rotationTarget, roatationStep);

            if (transform.GetChild(0).rotation == rotationTarget)
                isRotate = false;
        }
    }

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
            
            //TrackController.instance.textCoinCount.text = TrackController.instance.coinCount.ToString();
            TrackController.instance.activeTracks.Remove(this.gameObject);
            Destroy(this.gameObject);

            Stats.coinCount += reward;
            main.UpdateCoins();
        }

    }

    private Vector3 RouteDirection(int point)
    {
        var heading = TrackController.instance.trackRoutePoints[point].position - TrackController.instance.trackRoutePoints[point - 1].position;
        //var distance = heading.magnitude;
        //var direction = heading / distance;
        return heading.normalized;//direction;
    }
}
