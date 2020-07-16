using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 mouseSensitivty = Vector2.one;
    public float moveSpeed = 300;
    public float jumpforce = 100;
    public float maxInteractDistance = 20;

    float xRotation = 0;
    Vector2 movementInput;

    public static Player instance;
    Rigidbody rb;
    Equipment currentEquipment;
    [HideInInspector]
    public Camera mainCamera;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        currentEquipment = GetComponentInChildren<Equipment>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    protected void Update()
    {
        MouseLook();
        MovementInput();
        InteractionInput();
    }

    private void FixedUpdate()
    {
        Movement();
        
    }


    void MouseLook()
    {
        float mouseX = mouseSensitivty.x * Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = mouseSensitivty.y * Input.GetAxis("Mouse Y") * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up, mouseX);
    }

    void MovementInput()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
        movementInput = Vector2.ClampMagnitude(movementInput, 1);
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    void InteractionInput()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, maxInteractDistance))
            {
                if (hit.collider.GetComponent<PhysicalButton>())
                {
                    hit.collider.GetComponent<PhysicalButton>().Press();
                    return;
                }
            }


            currentEquipment.Interact();
        }
    }

    void Movement()
    {
        Vector3 vel;
        vel = moveSpeed * Time.fixedDeltaTime * ((transform.forward * movementInput.y) + (transform.right * movementInput.x));
        vel.y = rb.velocity.y;
        rb.velocity = vel;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }






    public void Sensitivity(float x)
    {
        mouseSensitivty.x += x;
        mouseSensitivty.y += x;
        if (mouseSensitivty.x < 1)
            mouseSensitivty.x = 1;
        if (mouseSensitivty.y < 1)
            mouseSensitivty.y = 1;
    }
    public void CrosshairSpeed(float x) {currentEquipment.crosshairSpeed += x;}
    public void CrosshairSize(float x) {currentEquipment.crosshairMoveSize += x;}
    public void MovementPatternHourglass()
    {
        currentEquipment.hourglassToggle = true;
    }
    public void MovementPatternRandom()
    {
        currentEquipment.hourglassToggle = false;
    }



}
