using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    CharacterController myCharacterContreller;
    public Transform target;
    Vector3 temp;
    Vector3 normal;
    public float distance = 3f;
    public GameObject sprite;
    public LayerMask groundMask;
    public float radius = 0.4f;
    public float avoidanceRange = 1;

    public float firingRate = 1;
    public float firingDistance = 15;
    public GameObject myBulletType;
    public Transform bulletSpawn;
    List<BulletComponent> bulletPool = new List<BulletComponent>();
    HealthComponent myHealth;
    float timer = 0;
    public float currentFloor;
    private LayerMask mask;
    bool dead = false;

    // Start is called before the first frame update
    void Awake()
    {
        int playerLayer = 1 << LayerMask.NameToLayer("Player");
        int wallLayer = 1 << LayerMask.NameToLayer("Wall");
        mask = playerLayer | wallLayer;

        myHealth = GetComponentInChildren<HealthComponent>();
        myCharacterContreller = GetComponent<CharacterController>();
        target = FindObjectOfType<CharacterControl>().transform;
        timer = firingRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterControl.instance.currentFloor != currentFloor)
            return;

        if(PlayerInView())// Do nothing while not seeing the player
        {
            Debug.Log("Player in view !");
            ObstacleAvoidance();
            CharacterMovement();
            Combat();
        } 
    }

    void CharacterMovement()
    {
        
        temp = (target.position - this.transform.position).normalized; 

        sprite.transform.rotation = Quaternion.LookRotation(temp);
        if (Vector3.Distance(target.position ,this.transform.position) <= distance)
            return;
        if (dead)
            return;
        myCharacterContreller.SimpleMove((temp + normal).normalized * speed);

    }

    void Combat()
    {
        if (Vector3.Distance(target.position, this.transform.position) > firingDistance)
            return;

        timer += Time.deltaTime;

        if (timer < firingRate)
            return;

        if (Physics.Raycast(this.transform.position, sprite.transform.TransformDirection(new Vector3(0f, 0f, 1f)), out RaycastHit hit, firingDistance + 1f, mask))
        {
            if (hit.transform.name == "Player")
            {
            }
            else
            {

                return;
            }
        }
        timer = 0;


        if (PoolManager.instance)
        {
            for (int i = 0; i < PoolManager.instance.bulletPoolEnemy.Count; i++)
            {
                if (!PoolManager.instance.bulletPoolEnemy[i].gameObject.activeInHierarchy)
                {
                    PoolManager.instance.bulletPoolEnemy[i].Fire(bulletSpawn, temp, 10);

                    return;
                }
            }
            BulletComponent go = Instantiate(myBulletType).GetComponent<BulletComponent>();
            PoolManager.instance.bulletPoolEnemy.Add(go);


            //  Debug.Break();
            go.Fire(bulletSpawn, temp, 10);
        }
        else
        {
            for (int i = 0; i < bulletPool.Count; i++)
            {
                if (!bulletPool[i].gameObject.activeInHierarchy)
                {
                    bulletPool[i].Fire(bulletSpawn, temp, 10);

                    return;
                }
            }
            BulletComponent go = Instantiate(myBulletType).GetComponent<BulletComponent>();
            bulletPool.Add(go);


            //  Debug.Break();
            go.Fire(bulletSpawn, temp, 10);
        }
    }

    private bool PlayerInView()
    {
        RaycastHit hit;
        Vector3 targetDir = target.position - this.transform.position;

        if(Physics.Raycast(transform.position, targetDir, out hit, 100f, mask))
        {
            if(hit.transform.name.Contains("Player"))
            {
                //lastSeenPlayerPosition = player.position;
                return true;
            }
        }
        return false;
    }

    void ObstacleAvoidance()
    {
        if (Vector3.SqrMagnitude(target.position - this.transform.position) <= distance)
            return;
        normal = Vector3.zero;
        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position, radius, temp, out hit, avoidanceRange, groundMask))
        {
            normal = hit.normal.normalized;
            Debug.DrawRay(hit.point, normal, Color.blue);
            Debug.DrawRay(hit.point, temp, Color.red);
            Debug.DrawRay(hit.point, (temp + normal).normalized, Color.green);
            return;
            // Debug.Break();
        }
    }
    public void Death()
    {
        myHealth.enabled = false;
           dead = true;
    }
    public void RestartAgent(int floor)
    {
        this.gameObject.SetActive(true);
        myHealth.RestoreHealth();
        currentFloor = floor;
        dead = false;
        myHealth.enabled = true;
    }
}