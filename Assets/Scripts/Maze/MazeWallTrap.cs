using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallTrap : MonoBehaviour
{

    public Transform body;
    public Transform weapon;
    private int damages = 10;
    private float nextShot = 0.0f;
    private float shootDelay = 1f;
    private LayerMask mask;

    void Start()
    {
        int playerLayer = 1 << LayerMask.NameToLayer("Player");
        int wallLayer = 1 << LayerMask.NameToLayer("Wall");
        mask = playerLayer | wallLayer;
    }

    void Update()
    {
        if(PlayerInView())
        {
            if(Time.time > nextShot)
            {
                nextShot = Time.time + shootDelay;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        // Play a sound

        // Inflict damages
        CharacterControl.instance.hurtbox.ReceiveDamage(damages);
    }

    private bool PlayerInView()
    {
        RaycastHit hit;

        Debug.DrawRay(weapon.position, weapon.TransformDirection(new Vector3(0f, 1f, 0f))*20f, Color.red); 

        if(Physics.Raycast(weapon.position, weapon.TransformDirection(new Vector3(0f, 1f, 0f)), out hit, 20f, mask))
        {
            if(hit.transform.name == "Player")
            {
                return true;
            }
        }

        return false;
    }
}
