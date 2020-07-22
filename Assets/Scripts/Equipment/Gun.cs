using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : Weapon
{
    public float fireRate = 1;
    public float ammoCapacity = 36;
    public float magazineCapacity = 6;
    public float currentAmmoCount = 36;
    public float currentMagazineCount = 6;

    public bool reloading;

    public Transform shootPoint;
    LineRenderer lr;

    public float CurrentMagazineCount
    {
        get { return currentMagazineCount; }
        set
        {
            if (value < 0)
                CurrentAmmoCount = 0;
            else
                currentMagazineCount = value;
            Manager.SetAmmoCountUI(currentMagazineCount, currentAmmoCount);
        }
    }

    public float CurrentAmmoCount
    {
        get { return currentAmmoCount; }
        set
        {
            if (value < 0)
                CurrentAmmoCount = 0;
            else
                currentAmmoCount = value;
            Manager.SetAmmoCountUI(currentMagazineCount, currentAmmoCount);
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

        Manager.SetAmmoCountUI(currentMagazineCount, currentAmmoCount);

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
        animator.Play("PistolIdle");
        reloading = false;
    }
}
