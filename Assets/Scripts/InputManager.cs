using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CharacterController characterController;

    public WeaponControl weaponControl;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        characterController.CharacterDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        characterController.MouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetButtonDown("Jump"))
        {
            characterController.JumpCommand = true;
        }

        if (Input.GetButtonDown("Fire1") )
        {
            weaponControl.ShootCommand = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            weaponControl.GrabCommand = true;
        }
    }
}