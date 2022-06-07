using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float rotateStartDistance;

    private int i = 1;

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

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = GameController.instance.trackPoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, GameController.instance.trackPoints[i].position, speed * Time.deltaTime);


        if (transform.position == GameController.instance.trackPoints[i].position)
        {
            i++;
        }

        //if (transform.position == GameController.instance.trackPoints[GameController.instance.trackPoints.Length - 1].position)
        if (i == GameController.instance.trackPoints.Length)
        {
            transform.position = GameController.instance.trackPoints[0].position;
            i = 1;
        }


        if (i < GameController.instance.trackPoints.Length - 1)
        {
            var currentDirection = RouteDirection(i);

            if (currentDirection == Vector3.up)
            {

                if (transform.position.y > GameController.instance.trackPoints[i].position.y - rotateStartDistance && transform.position.y < GameController.instance.trackPoints[i].position.y)
                {
                    if (!isRotate)
                    {
                        var furureDirection = RouteDirection(i + 1);
                        if (furureDirection == Vector3.right)
                            rotatedirection = RotateDirection.Right;
                        else if (furureDirection == Vector3.left)
                            rotatedirection = RotateDirection.Left;
                    }

                    isRotate = true;
                }
            }
            else if (currentDirection == Vector3.right)
            {
                if (transform.position.x > GameController.instance.trackPoints[i].position.x - rotateStartDistance && transform.position.x < GameController.instance.trackPoints[i].position.x)
                {
                    if (!isRotate)
                    {
                        var furureDirection = RouteDirection(i + 1);
                        if (furureDirection == Vector3.up)
                            rotatedirection = RotateDirection.Up;
                        else if (furureDirection == Vector3.down)
                            rotatedirection = RotateDirection.Down;
                    }

                    isRotate = true;
                }
            }
            else if (currentDirection == Vector3.left)
            {
                if (transform.position.x < GameController.instance.trackPoints[i].position.x + rotateStartDistance && transform.position.x > GameController.instance.trackPoints[i].position.x)
                {
                    if (!isRotate)
                    {
                        var furureDirection = RouteDirection(i + 1);
                        if (furureDirection == Vector3.up)
                            rotatedirection = RotateDirection.Up;
                        else if (furureDirection == Vector3.down)
                            rotatedirection = RotateDirection.Down;
                    }

                    isRotate = true;
                }
            }

        }

        if (isRotate)
        {
            var roatationStep = speed * Time.deltaTime * rotationSpeed; ;

            if (rotatedirection == RotateDirection.Right)
            {
                rotationTarget = Quaternion.Euler(0, 0, -90);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, roatationStep);
            }
            else if (rotatedirection == RotateDirection.Left)
            {
                rotationTarget = Quaternion.Euler(0, 0, 90);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, roatationStep);
            }
            else if (rotatedirection == RotateDirection.Up)
            {
                rotationTarget = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, roatationStep);
            }
            else if (rotatedirection == RotateDirection.Down)
            {
                rotationTarget = Quaternion.Euler(0, 0, 180);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, roatationStep);
            }

            if (transform.rotation == rotationTarget)
                isRotate = false;
        }
    }

    private Vector3 RouteDirection(int i)
    {
        var heading = GameController.instance.trackPoints[i].position - GameController.instance.trackPoints[i - 1].position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        return direction;
    }
}
