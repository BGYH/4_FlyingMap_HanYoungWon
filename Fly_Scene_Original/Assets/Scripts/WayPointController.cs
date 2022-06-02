using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointController : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f; //minimum distance to the waypoint(최소거리)
    private int lastWaypointIndex;


    private float movementSpeed = 3.0f; //이동속도
    // Start is called before the first frame update
    void Start()
    {
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        // Debug.Log("Distance : " + distance);
        CheckDistanceToWayPoint(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position,movementStep);
        
    
    }

    void CheckDistanceToWayPoint(float currentDistance)
    {
        if(currentDistance<= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }

    }

    void UpdateTargetWaypoint()
    {

    }

}


