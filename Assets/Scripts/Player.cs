using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public CrosshairSetting crosshairSettingNone;
    public Vector2 mouseSensitivty = Vector2.one;

    float currentMoveSpeed;
    public float sneekSpeed = 50;
    public float walkSpeed = 200;
    public float sprintSpeed = 400;
    public float jumpforce = 100;
    public float maxInteractDistance = 20;
    public float maxHealth = 100;
    float currentHealth;
    public bool dead;
    public GameObject[] startingEquipment;

    float xRotation = 0;
    Vector2 movementInput;
    Transform equipmentPosition;
    [HideInInspector]
    public Equipment currentEquipment;
    Rigidbody rb;
    [HideInInspector]
    public Camera mainCamera;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        /*if(GetComponentInChildren<Equipment>() != null)
            currentEquipment = GetComponentInChildren<Equipment>();*/
        rb = GetComponent<Rigidbody>();
        equipmentPosition = GameObject.Find("EquipmentPosition").transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        Manager.SetHealthUI(currentHealth);
        currentMoveSpeed = walkSpeed;

        for (int i = 0; i < startingEquipment.Length; i++)
        {
            if(startingEquipment[i] == null)
            {
                Manager.uiEquipmentSlots.AddSlot();
                continue;
            }

            Equipment E = Instantiate(startingEquipment[i], equipmentPosition, false).GetComponent<Equipment>();
            Manager.uiEquipmentSlots.AddSlot(E);

            E.GetComponent<Collider>().enabled = false;
            E.enabled = true;
            if(i != 0)
                E.gameObject.SetActive(false);
        }
        if(Manager.uiEquipmentSlots.SlotCount > 0)
        {
            Manager.uiEquipmentSlots.CurrentSlotIndex = 0;
            if (Manager.uiEquipmentSlots.CurrentSlot.Equipment != null)
            {
                currentEquipment = Manager.uiEquipmentSlots.CurrentSlot.Equipment;
                CrosshairManager.currentCrosshairSetting = currentEquipment.crosshairSetting;
            }
            else
                CrosshairManager.currentCrosshairSetting = crosshairSettingNone;
        }

/*        if (currentEquipment != null)
        {
            Manager.uiEquipmentSlots.AddSlot(currentEquipment);
            Manager.uiEquipmentSlots.CurrentSlotIndex = 0;
        }*/
    }


    protected void Update()
    {
        if (!dead)
        {
            MouseLook();
            MovementInput();
            InteractionInput();

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, maxInteractDistance))
                if (hit.collider.GetComponent<PhysicalButton>() || hit.collider.GetComponent<Equipment>())
                    CrosshairManager.RingColor(Color.red);
                else
                    CrosshairManager.RingColor(Color.white);
            else
                CrosshairManager.RingColor(Color.white);
        }

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
            currentEquipment?.GetComponent<Gun>()?.AttemptReload();
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
            if(!currentEquipment)
                CrosshairManager.currentCrosshairSetting = crosshairSettingNone;
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
        Manager.SetHealthUI(currentHealth);
        if (currentHealth <= 0)
            Die();
    }
    public void Heal(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Manager.SetHealthUI(currentHealth);
    }
    void Die()
    {
        if (dead)
            return;
        Manager.SetDeadScreen();
        dead = true;
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(2);
        Manager.ReloadScene();
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
    public void CrosshairSpeed(float x) => currentEquipment.crosshairSetting.speed = Mathf.Max(0, currentEquipment.crosshairSetting.speed + x);
    public void CrosshairSize(float x) => currentEquipment.crosshairSetting.moveSize = Mathf.Max(0, currentEquipment.crosshairSetting.moveSize + x);
    public void SetCrosshairMovementMode(int x) => currentEquipment.crosshairSetting.movementPattern = (CrosshairManager.CrosshairMovementPattern)x;


}
