using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public List<BulletComponent> bulletPool = new List<BulletComponent>();
    public List<EnemyController> enemyPool = new List<EnemyController>();
    public List<GameObject> enemyTypes = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }


    public void SpawnEnemy(Vector3 position)
    {

        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].gameObject.activeInHierarchy)
            {
                enemyPool[i].transform.position = position;
                enemyPool[i].RestartAgent();
                return;
            }
        }
        EnemyController temp = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)]).GetComponent<EnemyController>();

        temp.transform.position = position;
        enemyPool.Add(temp);

    }
}