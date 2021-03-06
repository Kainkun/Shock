﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player instance;

    public CrosshairSetting crosshairSettingNone;
    public Vector2 mouseSensitivty = Vector2.one;

    public LayerMask InteractionLayer;
    float currentMoveSpeed;
    public float sneekSpeed = 50;
    public float walkSpeed = 200;
    public float sprintSpeed = 400;
    public float jumpforce = 100;
    public float maxInteractDistance = 20;
    public float maxHealth = 100;
    float currentHealth;
    [HideInInspector]
    public bool dead;
    public GameObject[] startingEquipment;
    public HashSet<KeyData> keys = new HashSet<KeyData>();

    public float pistolAmmoCount;
    public float rifleAmmoCount;

    [HideInInspector]
    float xRotation = 0;
    Vector2 movementInput;
    Transform equipmentPosition;
    [HideInInspector]
    public Equipment currentEquipment;
    Rigidbody rb;
    [HideInInspector]
    public Camera mainCamera;
    float movementSoundDistance;
    float EquipmentSoundDistance;
    public float movementSoundMultiplyer = 2;
    Transform soundRadius;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        /*if(GetComponentInChildren<Equipment>() != null)
            currentEquipment = GetComponentInChildren<Equipment>();*/
        rb = GetComponent<Rigidbody>();
        equipmentPosition = GameObject.Find("EquipmentPosition").transform;
        soundRadius = GameObject.Find("SoundRadius").transform;
    }

    void Start()
    {
        //monster = GameObject.Find("Monster").GetComponent<NavMeshAgent>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        Manager.SetHealthUI(currentHealth);
        currentMoveSpeed = walkSpeed;

        for (int i = 0; i < startingEquipment.Length; i++)
        {
            if (startingEquipment[i] == null)
            {
                Manager.uiEquipmentSlots.AddSlot();
                continue;
            }

            Equipment E = Instantiate(startingEquipment[i], equipmentPosition, false).GetComponent<Equipment>();
            Manager.uiEquipmentSlots.AddSlot(E);

            E.GetComponent<Collider>().enabled = false;
            E.enabled = true;
            if (i != 0)
                E.gameObject.SetActive(false);
        }
        if (Manager.uiEquipmentSlots.SlotCount > 0)
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

        UpdateAmmoUI();

        /*        if (currentEquipment != null)
                {
                    Manager.uiEquipmentSlots.AddSlot(currentEquipment);
                    Manager.uiEquipmentSlots.CurrentSlotIndex = 0;
                }*/
    }

    NavMeshAgent monster;
    float f;
    protected void Update()
    {
        f = Mathf.Lerp(f, GetLoudestSoundDistance(), Time.deltaTime * 10);
        soundRadius.localScale = new Vector3(f, 0.1f, f);

        if (Input.GetMouseButtonDown(0) && (Cursor.lockState != CursorLockMode.Locked || Cursor.visible != false))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        MouseLook();
        MovementInput();
        InteractionInput();

        RaycastHit hit;
        if (Physics.SphereCast(mainCamera.transform.position, .1f, mainCamera.transform.forward, out hit, maxInteractDistance, InteractionLayer))
            if (hit.collider.GetComponent<PhysicalButton>() || hit.collider.GetComponent<Interactable>() || hit.collider.tag == "Pickup")
                CrosshairManager.RingColor(Color.red);
            else
                CrosshairManager.RingColor(Color.white);
        else
            CrosshairManager.RingColor(Color.white);

    }

    private void FixedUpdate()
    {
        Movement();
    }

    void MouseLook()
    {
        if (dead)
            return;

        float mouseX = mouseSensitivty.x * Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = mouseSensitivty.y * Input.GetAxis("Mouse Y") * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up, mouseX);
    }
    void MovementInput()
    {
        if (dead)
        {
            movementInput = Vector2.zero;
            return;
        }

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

        if (movementInput.magnitude > 0)
            movementSoundDistance = currentMoveSpeed / 100 * movementSoundMultiplyer;
        else
            movementSoundDistance = 0;
    }
    void InteractionInput()
    {
        if (dead)
            return;

        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.SphereCast(mainCamera.transform.position, .1f, mainCamera.transform.forward, out hit, maxInteractDistance, InteractionLayer))
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
                    currentEquipment.animator.enabled = true;

                    UpdateAmmoUI();
                }
                else if (hit.collider.GetComponent<Interactable>())
                {
                    hit.collider.GetComponent<Interactable>().Interact();
                }
                else if (hit.collider.GetComponent<AmmoPickup>())
                {
                    AmmoPickup ap = hit.collider.GetComponent<AmmoPickup>();
                    switch (hit.collider.GetComponent<AmmoPickup>().ammoType)
                    {
                        case AmmoPickup.AmmoType.Pistol:
                            pistolAmmoCount += ap.Pickup();
                            break;
                        case AmmoPickup.AmmoType.Rifle:
                            rifleAmmoCount += ap.Pickup();
                            break;
                    }
                    currentEquipment?.GetComponent<Gun>()?.RefreshAmmoCountUI();
                }
                else if (hit.collider.GetComponent<Key>())
                {
                    keys.Add(hit.collider.GetComponent<Key>().getKey());
                }
                else if (hit.collider.GetComponent<Log>())
                {
                    Manager.AddLog(hit.collider.GetComponent<Log>().GetTextAsset());
                    Manager.instance.ShowHelp("Press G to toggle logs", 3);
                }

                hit.collider.GetComponent<PhysicalButton>()?.Press();

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentEquipment?.Interact();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentEquipment?.GetComponent<Gun>()?.AttemptReload();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Manager.ToggleLog();
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
            if (!currentEquipment)
                CrosshairManager.currentCrosshairSetting = crosshairSettingNone;

            UpdateAmmoUI();
        }

    }

    bool GetKey(KeyData key) => keys.Add(key);
    public bool HasKey(KeyData requiredKey) => keys.Contains(requiredKey);

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

    public void MakeEquipmentSound(float volume)
    {
        if (currentEquipmentSound != null)
            StopCoroutine(currentEquipmentSound);
        currentEquipmentSound = StartCoroutine(EquipmentSound(volume));
    }

    Coroutine currentEquipmentSound;
    IEnumerator EquipmentSound(float volume)
    {
        EquipmentSoundDistance = volume;
        yield return new WaitForSeconds(0.2f);
        EquipmentSoundDistance = 0;
    }

    public float GetLoudestSoundDistance()
    {
        return movementSoundDistance > EquipmentSoundDistance ? movementSoundDistance : EquipmentSoundDistance;
    }

    void UpdateAmmoUI()
    {
        if (currentEquipment?.GetComponent<Gun>())
        {
            Manager.ShowAmmoCount(true);
        }
        else
        {
            Manager.ShowAmmoCount(false);
        }
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

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetLoudestSoundDistance());
    }

}
