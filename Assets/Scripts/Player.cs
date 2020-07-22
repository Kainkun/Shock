using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 mouseSensitivty = Vector2.one;
    float currentMoveSpeed;
    public float sneekSpeed = 50;
    public float walkSpeed = 200;
    public float sprintSpeed = 400;
    public float jumpforce = 100;
    public float maxInteractDistance = 20;
    public float maxHealth = 100;
    float currentHealth;
    bool dead;

    float xRotation = 0;
    Vector2 movementInput;
    Transform equipmentPosition;

    public static Player instance;
    Rigidbody rb;
    Equipment currentEquipment;
    [HideInInspector]
    public Camera mainCamera;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        if(GetComponentInChildren<Equipment>() != null)
            currentEquipment = GetComponentInChildren<Equipment>();
        rb = GetComponent<Rigidbody>();
        equipmentPosition = GameObject.Find("EquipmentPosition").transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        currentMoveSpeed = walkSpeed;

        if (currentEquipment != null)
        {
            Manager.uiEquipmentSlots.AddSlot(currentEquipment);
            Manager.uiEquipmentSlots.CurrentSlotIndex = 0;
        }
    }


    protected void Update()
    {
        MouseLook();
        MovementInput();
        InteractionInput();

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, maxInteractDistance))
            if (hit.collider.GetComponent<PhysicalButton>())
                Manager.crosshairRing.color = Color.red;
            else
                Manager.crosshairRing.color = Color.white;
        else
            Manager.crosshairRing.color = Color.white;
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

        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            currentMoveSpeed = sprintSpeed;
        else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
            currentMoveSpeed = sneekSpeed;
        else
            currentMoveSpeed = walkSpeed;
    }
    void InteractionInput()
    {
        RaycastHit hit;

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, maxInteractDistance))
            {
                if (hit.collider.GetComponent<Equipment>())
                {
                    currentEquipment?.gameObject.SetActive(false);

                    hit.collider.GetComponent<Equipment>().enabled = true;
                    Manager.uiEquipmentSlots.AddSlot(hit.collider.GetComponent<Equipment>());
                    Manager.uiEquipmentSlots.CurrentSlotIndex = Manager.uiEquipmentSlots.SlotCount - 1;

                    currentEquipment = hit.collider.GetComponent<Equipment>();
                    hit.collider.enabled = false;
                    currentEquipment.transform.parent = equipmentPosition;
                    currentEquipment.transform.localPosition = Vector3.zero;
                    currentEquipment.transform.localRotation = Quaternion.identity;
                }

                hit.collider.GetComponent<PhysicalButton>()?.Press();

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentEquipment?.Interact();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentEquipment && currentEquipment.GetComponent<Gun>())
            {
                currentEquipment.GetComponent<Gun>().Reload();
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && Manager.uiEquipmentSlots.SlotCount > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                Manager.uiEquipmentSlots.CurrentSlotIndex--;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                Manager.uiEquipmentSlots.CurrentSlotIndex++;

            currentEquipment?.gameObject.SetActive(false);
            currentEquipment = Manager.uiEquipmentSlots.CurrentSlot.Equipment;
            currentEquipment?.gameObject.SetActive(true);
        }

    }
    void Movement()
    {
        Vector3 vel;
        vel = currentMoveSpeed * Time.fixedDeltaTime * ((transform.forward * movementInput.y) + (transform.right * movementInput.x));
        vel.y = rb.velocity.y;
        rb.velocity = vel;
    }
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }

    public void TakeDamage(float damage)
    {
        if (dead)
            return;

        StartCoroutine(Manager.DamageBlink());

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0)
            Die();
    }
    public void Heal(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    void Die()
    {
        if (dead)
            return;
        dead = true;

    }

    public void Sensitivity(float x)
    {
        mouseSensitivty.x = Mathf.Max(10, mouseSensitivty.x + x);
        mouseSensitivty.y = Mathf.Max(10, mouseSensitivty.y + x);
        if (mouseSensitivty.x < 1)
            mouseSensitivty.x = 1;
        if (mouseSensitivty.y < 1)
            mouseSensitivty.y = 1;
    }
    public void CrosshairSpeed(float x) {currentEquipment.crosshairSpeed = Mathf.Max(0, currentEquipment.crosshairSpeed + x) ; }
    public void CrosshairSize(float x) {currentEquipment.crosshairMoveSize = Mathf.Max(0, currentEquipment.crosshairMoveSize + x); }
    public void SetCrosshairMovementMode(int x)
    {
        currentEquipment.currentCrosshairMovement = (Equipment.CrosshairMovementMode)x;
    }


}
