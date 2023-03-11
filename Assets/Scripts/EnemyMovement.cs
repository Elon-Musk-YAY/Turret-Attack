using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavePointIndex = 0;
    private Enemy enemy;


    private void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
    }


    private void Update()
    {
        if (target == null)
        {
            target = Waypoints.points[0];
        }
        Vector3 dir = target.position - transform.position;
        Vector3 calculation = dir.normalized * enemy.speed * Time.deltaTime;
        transform.Translate(new Vector3(calculation.x,0,calculation.z), Space.World);

        if (Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(target.position.x,0,target.position.z)) <= 0.5f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
    }
    void GetNextWaypoint()
    {
        if (wavePointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        wavePointIndex++;
        target = Waypoints.points[wavePointIndex];
    }

    void EndPath()
    {
        Destroy(gameObject);
        PlayerStats.Lives--;
        WaveSpawner.enemiesAlive--;

    }
}
