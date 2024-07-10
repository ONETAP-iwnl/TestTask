using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Variable")]
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    [Space(3f)]
    [Header("MainCamera")]
    [SerializeField]
    private protected Camera playerCamera;


    [Space(3f)]
    [Header("Positon")]
    [SerializeField]
    private protected Transform cubeHoldPosition;

    private Cube carriedCubeScript;
    public CubeInsertionManager insertionManager;

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
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);


        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }


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

        if(Input.GetKeyDown(KeyCode.F))
        {
            if (carriedCube != null)
            {
                InsertCubeInZone();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool isCorrect = insertionManager.CheckCorrectPlacement();
            if (isCorrect)
            {
                Debug.Log("правильно расставленные кубики!");
            }
            else
            {
                Debug.Log("кубики расставлены неправильно");
            }
        }
    }

    void PickupCube()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Cube"))
            {
                carriedCube = hit.collider.gameObject;
                carriedCubeScript = carriedCube.GetComponent<Cube>();
                if(!carriedCubeScript.IsDropped)
                {
                    carryingCube = Instantiate(hit.collider.gameObject, cubeHoldPosition.position, cubeHoldPosition.rotation);
                    carriedCube.transform.SetParent(cubeHoldPosition);
                    carriedCube.transform.localPosition = Vector3.zero;
                    carriedCube.GetComponent<Rigidbody>().isKinematic = true;
                    carryingCube = true;

                }
                else if(carriedCubeScript.IsDropped)
                {
                    carriedCube.transform.SetParent(cubeHoldPosition);
                    carriedCube.transform.localPosition = Vector3.zero;
                    carriedCube.GetComponent<Rigidbody>().isKinematic = true;
                    carryingCube = true;
                }
            }
        }
    }

    void DropCube()
    {
        carriedCubeScript = carriedCube.GetComponent<Cube>();
        if (carryingCube && carriedCube != null)
        {
            carriedCube.GetComponent<Rigidbody>().isKinematic = false;
            carriedCube.transform.SetParent(null);
            carryingCube = false;
            carriedCube = null;
            carriedCubeScript.IsDropped = true;
        }
    }

    void InsertCubeInZone()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 5f))
        {
            if (hit.collider.CompareTag("TargetZone"))
            {
                int zoneIndex = System.Array.IndexOf(insertionManager.targetZones, hit.collider.gameObject);
                if (zoneIndex >= 0)
                {
                    if (!insertionManager.IsZoneOccupied(zoneIndex))
                    {
                        carriedCube.transform.SetParent(insertionManager.targetZones[zoneIndex].transform);
                        carriedCube.transform.localPosition = Vector3.zero;
                        Rigidbody rb = carriedCube.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true; // Disable physics
                        }
                        insertionManager.InsertCube(carriedCube, zoneIndex);
                        carriedCube = null;
                        carryingCube = false;
                    }
                    else
                    {
                        Debug.Log("таргет зона занята уже!!");
                    }
                }
            }
        }
    }
}