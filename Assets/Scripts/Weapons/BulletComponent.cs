using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    #region Variables
    public Rigidbody myRigidbody;


    int bounce;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
    #endregion

    #region Methods
    private void OnTriggerEnter(Collider collision)
    {
        if ( collision.GetComponentInChildren<HealthComponent>())
        {
            HealthComponent temp = collision.GetComponentInChildren<HealthComponent>();

            temp.ReceiveDamage();
        }

            this.gameObject.SetActive(false);
    }

    public void Fire(Vector3 position, Vector3 direction, WeaponData myData)
    {

        this.gameObject.SetActive(true);
        this.transform.position = position;
        myRigidbody.velocity = (direction.normalized * myData.speed);
    }

    public void Fire(Vector3 position, Vector3 direction, float speed)
    {

        this.gameObject.SetActive(true);
        this.transform.position = position;
        myRigidbody.velocity = (direction.normalized * speed);
    }


    IEnumerator Cooldown()
    {
        this.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForEndOfFrame();

        this.GetComponent<Collider>().enabled = true;
    }
    #endregion
}