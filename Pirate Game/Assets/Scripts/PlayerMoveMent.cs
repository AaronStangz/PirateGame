using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;
using UnityEngine.EventSystems;
using System.Threading;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveMent : NetworkBehaviour
{
    [Header("Spawning")]
    public float SpawnX = -5;
    public float Spawnz = 5;

    [Header("Player")]
    public GameObject PlayerModel;
    public GameObject CamHolder;
    public Transform CamHolderTransform;
    public Transform CamTransform;

    [Header("MoveMent")]
    public float WalkSpeed = 10f;
    public float SideSpeed = 8f;
    public float SprintSpeed = 15f;
    public float rotateSpeed = 100f;
    public float cameraSensitivity = 2.0f;
    public float cameraDistance = 5.0f;
    public float jumpForce = 500f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private float cameraRotationX = 0f;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PlayerModel.SetActive(true);
        CamHolder.SetActive(true);
        SpawnPosition();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        if (isOwned)
        {
            // Get input direction from the player
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

            // Rotate the camera
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
            cameraRotationX -= mouseY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            CamTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
            CamHolderTransform.Rotate(Vector3.up * mouseX);

            // Check if the player is grounded
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        }
        else
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (isOwned)
        {
            // Move the player
            Vector3 moveDirection = CamHolderTransform.TransformDirection(Vector3.forward);
            Vector3 moveDirectionL = CamHolderTransform.TransformDirection(Vector3.left);
            Vector3 moveDirectionR = CamHolderTransform.TransformDirection(Vector3.right);
            Vector3 moveDirectionS = CamHolderTransform.TransformDirection(Vector3.back);
            float speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : WalkSpeed;
            if (isOwned && Input.GetKey(KeyCode.W))
            {
                rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
            }
            if (isOwned && Input.GetKey(KeyCode.A))
            {
                rb.MovePosition(rb.position + moveDirectionL * SideSpeed * Time.fixedDeltaTime);
            }
            if (isOwned && Input.GetKey(KeyCode.D))
            {
                rb.MovePosition(rb.position + moveDirectionR * SideSpeed * Time.fixedDeltaTime);
            }
            if (isOwned && Input.GetKey(KeyCode.S))
            {
                rb.MovePosition(rb.position + moveDirectionS * SideSpeed * Time.fixedDeltaTime * 2f);
            }
            if (isOwned && Input.GetKey(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(transform.up * jumpForce);
            }
        }
    }
    public void SpawnPosition()
    {
        if (isOwned)
        {
            transform.position = new Vector3(Random.Range(SpawnX, Spawnz), 0.8f, Random.Range(-15, 7));
        }
    }
}
