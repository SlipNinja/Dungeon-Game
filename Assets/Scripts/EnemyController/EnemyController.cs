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

    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
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
        if (Vector3.SqrMagnitude(target.position - this.transform.position) <= distance)
            return;
        temp = (target.position - this.transform.position).normalized;
        myCharacterContreller.SimpleMove((temp + normal).normalized * speed);

        sprite.transform.rotation = Quaternion.LookRotation(temp);

    }

    void Combat()
    {
        if (Vector3.SqrMagnitude(target.position - this.transform.position) < firingDistance)
            return;
        timer += Time.deltaTime;

        if (timer < firingRate)
            return;
        timer = 0;
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].gameObject.activeInHierarchy)
            {
                bulletPool[i].Fire(bulletSpawn.position, temp, 10);

                return;
            }
        }
        BulletComponent go = Instantiate(myBulletType).GetComponent<BulletComponent>();
        bulletPool.Add(go);


        //  Debug.Break();
        go.Fire(bulletSpawn.position, temp, 10);

    }

    void ObstacleAvoidance()
    {
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
}