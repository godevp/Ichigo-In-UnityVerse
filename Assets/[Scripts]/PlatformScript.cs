using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlatformDirection
{
    HORIZONTALRIGHT,
    HORIZONTALLEFT,
    VERTICALUP,
    VERTICALDOWN,
    DIAGONAL_UP,
    DIAGONAL_DOWN,
    CUSTOM
}

/*
 
 Source file Name - PlatformScript.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: the core for movable platforms

 */
public class PlatformScript : MonoBehaviour
{
    public PlatformDirection direction;

    [Header("Movement Properties")]
    [Range(1.0f, 20.0f)]
    public float horizontalDistance = 8.0f;
    [Range(1.0f, 20.0f)]
    public float horizontalSpeed = 3.0f;
    [Range(1.0f, 20.0f)]
    public float verticalDistance = 8.0f;
    [Range(1.0f, 20.0f)]
    public float verticalSpeed = 3.0f;
    [Range(0.001f, 0.1f)]
    public float customSpeedFactor = 0.02f;

    [Header("Platform Path Points")]
    public List<Transform> pathPoints;

    private Vector2 startPoint;
    private Vector2 destinationPoint;
    private List<Vector2> pathList;
    private float timer;
    private int currentPointIndex;
    private float minusX;
    private float plusX;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        
        currentPointIndex = 0;
        startPoint = transform.position;
        pathList = new List<Vector2>(); // create an empty container of Vector2s

        // copy each pointPoint Transform to the pathList and add startPoint
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector2 point = new Vector2(pathPoints[i].localPosition.x + startPoint.x,
                                        pathPoints[i].localPosition.y + startPoint.y);
            pathList.Add(point);
        }
        pathList.Add(transform.position);
        minusX = startPoint.x - horizontalDistance/2;
        plusX = startPoint.x + horizontalDistance / 2;
        destinationPoint = pathList[currentPointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void FixedUpdate()
    {
        if (direction == PlatformDirection.CUSTOM)
        {
            if (timer <= 1.0f)
            {
                timer += customSpeedFactor;
            }

            if (timer >= 1.0f)
            {
                timer = 0.0f;

                currentPointIndex++;
                if (currentPointIndex >= pathList.Count)
                {
                    currentPointIndex = 0;
                }

                startPoint = transform.position;
                destinationPoint = pathList[currentPointIndex];
            }
        }
         
    }

    public void Move()
    {
        switch (direction)
        {
            case PlatformDirection.HORIZONTALRIGHT:
                transform.position = new Vector2(
        Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x, startPoint.y);
                break;
            case PlatformDirection.HORIZONTALLEFT:
                transform.position = new Vector2(
                 startPoint.x - Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance), startPoint.y);
                break;
            case PlatformDirection.VERTICALUP:
                transform.position = new Vector2(startPoint.x,
                    Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPoint.y);
                break;
            case PlatformDirection.VERTICALDOWN:
                transform.position = new Vector2(startPoint.x,
                    startPoint.y - Mathf.PingPong(verticalSpeed * Time.time, verticalDistance));
                break;
            case PlatformDirection.DIAGONAL_UP:
                transform.position = new Vector2(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPoint.y);
                break;
            case PlatformDirection.DIAGONAL_DOWN:
                transform.position = new Vector2(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    startPoint.y - Mathf.PingPong(verticalSpeed * Time.time, verticalDistance));
                break;
            case PlatformDirection.CUSTOM:
                transform.position = Vector2.Lerp(startPoint, destinationPoint, timer);
                break;
        }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.transform.SetParent(transform);
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        other.gameObject.transform.SetParent(null);
    }
}
