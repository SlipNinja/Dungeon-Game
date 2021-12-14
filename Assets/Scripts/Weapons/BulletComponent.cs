using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletComponent : MonoBehaviour
{
    #region Variables
    public Rigidbody myRigidbody;

    public UnityEvent collisionEvent;

    int bounce;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
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
        collisionEvent.Invoke();
       
          //  this.gameObject.SetActive(false);
    }

    public void Fire(Transform position, Vector3 direction, WeaponData myData)
    {

        this.gameObject.SetActive(true);
        this.transform.SetPositionAndRotation(position.position, position.rotation);
        myRigidbody.velocity = (direction.normalized * myData.speed);
    }

    public void Fire(Transform position, Vector3 direction, float speed)
    {

        this.gameObject.SetActive(true);
        this.transform.SetPositionAndRotation(position.position, position.rotation);
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