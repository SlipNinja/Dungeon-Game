using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    #region Variables
    public Transform gunRoot;
    public Transform bulletSpawn;
    public ItemDetector itemDetector;

    public GameObject myBulletType;

    public List<BulletComponent> bulletPool = new List<BulletComponent>();
    WeaponComponent currentWeapon;
    bool shootCommand;
    bool shootCooldown = false;
    bool grabCommand;
    Coroutine cooldownCoroutine;
    #endregion

    #region MonoBehaviour
    void Update()
    {
        Grab();
        Shoot();
    }
    #endregion

    #region Methods
    /// <summary>
    /// grab a new weapon, drop the previous one
    /// </summary>
    public void Grab()
    {
        if (!grabCommand)
            return;

        if (itemDetector.gunList.Count == 0)
        {
            grabCommand = false;
            return;
        }
        if (currentWeapon != null)
        {
            currentWeapon.transform.parent = null;
            currentWeapon.myRigidbody.isKinematic = false;
            currentWeapon.myRigidbody.useGravity = true;
            currentWeapon.grabbed = false;
            currentWeapon.Reposition();
        }
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);
        shootCooldown = false;
        currentWeapon = itemDetector.gunList[0];
        bulletSpawn = currentWeapon.spawnBullet;
        currentWeapon.transform.parent = gunRoot;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localEulerAngles = Vector3.zero;
        currentWeapon.myRigidbody.isKinematic = true;
        currentWeapon.myRigidbody.useGravity = false;
        currentWeapon.grabbed = true;
        itemDetector.gunList.Remove(itemDetector.gunList[0]);

        grabCommand = false;

    }
    /// <summary>
    /// shoots a bullet everytime its called
    /// </summary>
    public void Shoot()
    {
        if (!shootCommand)
            return;


        if (!currentWeapon || shootCooldown)
        {
            shootCommand = false;
            return;
        }

        switch (currentWeapon.data.weaponData.myGunType)
        {
            case GunType.pistol:
                PistolFire();
                break;
            case GunType.shotgun:
                for (int i = 0; i < 6; i++)
                {
                    PistolFire();
                   
                }
                break;
            case GunType.semiauto:
              StartCoroutine  (SemiautomticFire());
                break;
            default:
                break;
        }

        shootCommand = false;
        cooldownCoroutine = StartCoroutine(Cooldown(currentWeapon.data.weaponData.firingSpeed));
    }

   void PistolFire()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].gameObject.activeInHierarchy)
            {
                bulletPool[i].Fire(bulletSpawn.position, bulletSpawn.transform.forward , currentWeapon.data.weaponData);
                shootCommand = false;
                return;
            }
        }
        BulletComponent temp = Instantiate(myBulletType).GetComponent<BulletComponent>();
        bulletPool.Add(temp);

        Vector3 dir = bulletSpawn.transform.TransformDirection(new Vector3(ReturnRandom(), ReturnRandom(), 1));
        Debug.Log(dir);
        Debug.Log(bulletSpawn.transform.forward);   
        Debug.DrawRay(bulletSpawn.position, bulletSpawn.transform.forward, Color.blue, 1);
        Debug.DrawRay(bulletSpawn.position, dir, Color.red, 1);
      //  Debug.Break();
        temp.Fire(bulletSpawn.position, dir, currentWeapon.data.weaponData);
    }

    IEnumerator SemiautomticFire()
    {
        for (int i = 0; i < 3; i++)
        {
            PistolFire();
            yield return new WaitForSeconds (0.1f);
        }
    }

    public IEnumerator Cooldown(float cooldown)
    {
        shootCooldown = true;

        yield return new WaitForSeconds(cooldown);
        shootCooldown = false;

    }

    float ReturnRandom()
    {
        return Random.Range(-currentWeapon.data.weaponData.noise, currentWeapon.data.weaponData.noise);
    }
    #endregion

    #region GetSet
    public bool ShootCommand { set { shootCommand = value; } }
    public bool GrabCommand { set { grabCommand = value; } }
    #endregion
}