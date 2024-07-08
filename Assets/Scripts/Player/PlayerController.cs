using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Transform cubeHoldPosition; // Позиция для удержания куба

    public CubeManager cubeManager;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    private GameObject carriedCube;
    private bool carryingCube = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }

        // Handle cube interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!carryingCube)
            {
                PickupCube();
            }
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            if(carriedCube)
            {
                DropCube();
            }
        }
    }

    void PickupCube()
    {
        if (!carryingCube)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f))
            {
                if (hit.collider.CompareTag("Cube"))
                {
                    carriedCube = hit.collider.gameObject;
                    carriedCube.transform.SetParent(cubeHoldPosition);
                    carriedCube.transform.localPosition = Vector3.zero;
                    carriedCube.GetComponent<Rigidbody>().isKinematic = true; // Отключаем физику
                    carryingCube = true;
                    Transform spawnPoint = carriedCube.GetComponent<Cube>().spawnPoint;
                    cubeManager.CubePickedUp(spawnPoint);
                }
            }
        }
    }

    void DropCube()
    {
        if (carryingCube)
        {
            Vector3 dropPosition = transform.position + transform.forward * 2; // Позиция перед игроком
            Debug.Log("Dropping cube at position: " + dropPosition);
            carriedCube.transform.SetParent(null);
            carriedCube.transform.position = dropPosition;
            carriedCube.GetComponent<Rigidbody>().isKinematic = false; // Включаем физику
            carryingCube = false;
            carriedCube = null;
        }
    }

    Transform GetTargetPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (hit.collider.CompareTag("TargetZone"))
            {
                return hit.collider.transform;
            }
        }
        return null;
    }
}
