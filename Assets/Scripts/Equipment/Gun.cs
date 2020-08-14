using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : Weapon
{
    public AmmoPickup.AmmoType ammoType;
    public float fireRate = 1;
    public float ammoCapacity = 72;
    public float magazineCapacity = 6;
    public float currentMagazineCount = 0;

    public bool reloading;

    public Transform shootPoint;
    LineRenderer lr;

    public float CurrentAmmoCount
    {
        get
        {
            switch (ammoType)
            {
                case AmmoPickup.AmmoType.Pistol:
                    return Player.instance.pistolAmmoCount;
                case AmmoPickup.AmmoType.Rifle:
                    return Player.instance.rifleAmmoCount;
                default:
                    Debug.LogError("Invalid AmmoType");
                    return 0;
            }
        }
        set
        {
            if (value < 0)
                switch (ammoType)
                {
                    case AmmoPickup.AmmoType.Pistol:
                        Player.instance.pistolAmmoCount = 0;
                        break;
                    case AmmoPickup.AmmoType.Rifle:
                        Player.instance.rifleAmmoCount = 0;
                        break;
                    default:
                        Debug.LogError("Invalid AmmoType");
                        break;
                }
            else
                switch (ammoType)
                {
                    case AmmoPickup.AmmoType.Pistol:
                        Player.instance.pistolAmmoCount = value;
                        break;
                    case AmmoPickup.AmmoType.Rifle:
                        Player.instance.rifleAmmoCount = value;
                        break;
                    default:
                        Debug.LogError("Invalid AmmoType");
                        break;
                }
            RefreshAmmoCountUI();
        }
    }

    public float CurrentMagazineCount
    {
        get { return currentMagazineCount; }
        set
        {
            if (value < 0)
                switch (ammoType)
                {
                    case AmmoPickup.AmmoType.Pistol:
                        Player.instance.pistolAmmoCount = 0;
                        break;
                    case AmmoPickup.AmmoType.Rifle:
                        Player.instance.rifleAmmoCount = 0;
                        break;
                    default:
                        Debug.LogError("Invalid AmmoType");
                        break;
                }
            else
                currentMagazineCount = value;
            RefreshAmmoCountUI();
        }
    }

    protected override void Awake()
    {
        lr = GetComponent<LineRenderer>();

        base.Awake();
    }

    protected override void Start()
    {
        CurrentMagazineCount = magazineCapacity;

        RefreshAmmoCountUI();

        base.Start();
    }


    private void Reset()
    {
        maxInteractDistance = 100;
    }

    public override void Interact() //How all guns shoot
    {
        if (reloading || CurrentMagazineCount <= 0)
            return;

        animator.SetTrigger("Shoot");
        Player.instance.MakeEquipmentSound(interactionLoudnessDistance);
        CurrentMagazineCount--;

        Ray ray;
        RaycastHit hit = GetCrosshairHit(out ray);
        if (hit.transform != null)
            StartCoroutine(ShootTrail(hit.point));
        else
            StartCoroutine(ShootTrail(mainCamera.transform.position + ray.direction * 50));

        base.Interact();
    }


    public virtual void AttemptReload()
    {
        if (currentMagazineCount < magazineCapacity)
        {
            reloading = true;
            animator.SetTrigger("Reload");
            //currentEquipment.GetComponent<Gun>().Reload();
        }
    }

    public virtual void Reload()
    {
        reloading = false;
        float magazineEmptySpace = magazineCapacity - CurrentMagazineCount;
        float reloadCount = Mathf.Min(magazineEmptySpace, CurrentAmmoCount);
        CurrentAmmoCount -= reloadCount;
        CurrentMagazineCount += reloadCount;
    }

    IEnumerator ShootTrail(Vector3 hitPosition)
    {
        lr.SetPosition(1, shootPoint.position);
        lr.SetPosition(0, hitPosition);
        lr.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lr.enabled = false;
    }
    private void OnDisable()
    {
        reloading = false;
    }

    public void RefreshAmmoCountUI()
    {
        Manager.SetAmmoCountUI(currentMagazineCount, CurrentAmmoCount);
    }
}
