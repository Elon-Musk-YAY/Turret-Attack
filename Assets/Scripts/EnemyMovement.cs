using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavePointIndex = 0;
    private Enemy enemy;
    private Vector3 explodeVector;

    //[SerializeField]
    private Transform _t;


    private void Start()
    {
        _t = transform;
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
        explodeVector = GameManager.Instance.endExploder.gameObject.transform.position;
    }

    Vector3 dir, calculation;

    private void Update()
    {
        if (target == null)
        {
            target = Waypoints.points[0];
        }
        dir = target.position - _t.position;
        calculation = enemy.speed * Time.deltaTime * dir.normalized;
        _t.Translate(new Vector3(calculation.x, 0, calculation.z), Space.World);

        if (Vector3.Distance(new Vector3(_t.position.x, 0, _t.position.z), new Vector3(target.position.x, 0, target.position.z)) <= WaveSpawner.Instance.nextWaypointErr)
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
        if ((enemy.isMegaBoss || enemy.isFINALBOSS) && enemy.health <= 0)
        {
            return;
        }
        if (enemy.isFINALBOSS)
        {
            PlayerStats.Lives-= PlayerStats.Lives;
        } else
        {
            PlayerStats.Lives--;
        }
        if (GameManager.Instance.endExploder == null)
        {
            Destroy(gameObject);
            return;
        }
        if (SettingsManager.All())
        {

            GameObject blowup = Instantiate(GameManager.Instance.absorbtionEffect, explodeVector, Quaternion.identity);
            Destroy(blowup, 5.3f);
            blowup.SetActive(false);
            blowup.GetComponent<ParticleSystemRenderer>().material = enemy.GetComponent<Renderer>().material;
            blowup.SetActive(true);
        }
        PlayerStats.Money += (long)System.Math.Round(0.2d * enemy.worth);
        Destroy(gameObject);
        TraumaInducer t = GameManager.Instance.gameCamera.GetComponent<TraumaInducer>();
        if (PlayerStats.Lives <= 0)
        {
            if (SettingsManager.All())
            {
                GameManager.Instance.endExploder.Main();
            } else
            {
                Destroy(GameManager.Instance.endExploder.gameObject);
            }
            t.heavyShake();
        } else
        {
            t.lightShake();
        }
        WaveSpawner.enemiesAlive--;

    }
}
