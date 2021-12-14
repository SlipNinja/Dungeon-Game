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
    // Start is called before the first frame update
    void Start()
    {
        myHealth = GetComponentInChildren<HealthComponent>();
           myCharacterContreller = GetComponent<CharacterController>();
        target = FindObjectOfType<CharacterControl>().transform;
        timer = firingRate;
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleAvoidance();
        CharacterMovement();
        Combat();
    }

    void CharacterMovement()
    {
        
        temp = (target.position - this.transform.position).normalized; 

        sprite.transform.rotation = Quaternion.LookRotation(temp);
        if (Vector3.Distance(target.position ,this.transform.position) <= distance)
            return;
        myCharacterContreller.SimpleMove((temp + normal).normalized * speed);

    }

    void Combat()
    {
        if (Vector3.Distance(target.position , this.transform.position) > firingDistance)
            return;
        timer += Time.deltaTime;

        if (timer < firingRate)
            return;
        timer = 0;
      

        if (PoolManager.instance)
        {
            for (int i = 0; i < PoolManager.instance.bulletPool.Count; i++)
            {
                if (!PoolManager.instance.bulletPool[i].gameObject.activeInHierarchy)
                {
                    PoolManager.instance.bulletPool[i].Fire(bulletSpawn, temp, 10);

                    return;
                }
            }
            BulletComponent go = Instantiate(myBulletType).GetComponent<BulletComponent>();
            PoolManager.instance.bulletPool.Add(go);


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

    public void RestartAgent()
    {
        this.gameObject.SetActive(true);
        myHealth.RestoreHealth();
    }
}