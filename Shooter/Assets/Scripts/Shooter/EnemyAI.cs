using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public string waypointTag = "Waypoint";
    public float waitTimeMin = 1f;
    public float waitTimeMax = 3f;

    public float chaseRange = 20f;
    public float losePlayerRange = 30f;
    private Transform[] waypoints;
    private NavMeshAgent agent;
    private Transform player;
    private int currentWaypointIndex;
    private float waitTimer;
    private bool waiting;
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float shootRange = 100f;
    private float fireCooldown = 3f;

    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }
        if (waypoints.Length > 0)
        {
            GoToNextWaypoint();
        }
        else
        {
            Debug.LogWarning("No waypoints found in scene with tag: " + waypointTag + " Consult Dylan if confused");
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol(distanceToPlayer);
                break;
            case State.Chase:
                Chase(distanceToPlayer);
                break;
        }
    }


    void Patrol(float distanceToPlayer)
    {
        
        if (distanceToPlayer <= chaseRange)
        {
            currentState = State.Chase;
            return;
        }

        
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= Random.Range(waitTimeMin, waitTimeMax))
            {
                GoToNextWaypoint();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waiting = true;
            waitTimer = 0f;
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        currentWaypointIndex = Random.Range(0, waypoints.Length); 
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        waiting = false;
    }

    void Chase(float distanceToPlayer)
    {
        agent.SetDestination(player.position);

        if (distanceToPlayer > losePlayerRange)
            {
                currentState = State.Patrol;
                GoToNextWaypoint();
            }
        if (distanceToPlayer <= shootRange)
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            //ShootAtPlayer();
            fireCooldown = 1f / fireRate;
        }
    }
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        if (hitObject.CompareTag("Player"))
        {

            SceneManager.LoadScene(0);
        }
    }
    /*void ShootAtPlayer()
    {
        if (enemyBulletPrefab == null || firePoint == null) return;


        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);

        Vector3 direction = (player.position + Vector3.up * 1.5f) - firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);


        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * bullet.GetComponent<Bullet>().projectileVelocity;
        }
    }*/
}
