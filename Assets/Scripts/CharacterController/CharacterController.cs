using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public float speed = 10f;
    [Header("Jump")]
    public float jumpHeight = 3f;
    public Transform groundChecker;
    public LayerMask groundMask;
    public float groundCheckRadius = 0.4f;

    [Header("Camera")]
    public float mouseSensitivity = 100f;

    Rigidbody myRigidbody;
    ConstantForce myConstantForce;
    float xRotation = 0f;
    Vector2 characterMovement;
    Vector2 mouseMovement;
    Vector3 customGravity = new Vector3(0, -9.81f, 0);
    bool isGrounded = false;
    bool jumpCommand;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myConstantForce = GetComponent<ConstantForce>();
    }
    
    private void Update()
    {
        CameraControl();
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundMask);

        CharacterMovement();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundChecker.position, groundCheckRadius);
    }
    #endregion

    #region Methods

    /// <summary>
    /// Controls the camera through the values in mouseMovement
    /// </summary>
    void CameraControl()
    {
        float mouseX = mouseMovement.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseMovement.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);
    }
    /// <summary>
    /// Controls the players avatar through the values in characterMovement
    /// </summary>
    void CharacterMovement()
    {
        myRigidbody.velocity = this.transform.TransformDirection(new Vector3(characterMovement.x * speed, myRigidbody.velocity.y, characterMovement.y * speed));
        if (isGrounded && jumpCommand)
        {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight), myRigidbody.velocity.z);
            jumpCommand = false;
        }
        else
        {
            jumpCommand = false;
        }
    }

    void AdjustGravity()
    {
        if (!isGrounded && myRigidbody.velocity.y < 0)
        {
            myConstantForce.force = customGravity * 2f;
        }
        else
        {
            myConstantForce.force = customGravity;
        }
    }
    #endregion

    #region GetSet
    public Vector2 CharacterDirection { set { characterMovement = value; } }
    public Vector2 MouseMovement { set { mouseMovement = value; } }
    public bool JumpCommand { set { jumpCommand = value; } }
    #endregion
}