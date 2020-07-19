using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : Equipment
{
    public float fireRate = 1;
    public float ammoCapacity = 36;
    public float magazineCapacity = 6;
    public float currentAmmoCount = 36;
    public float currentMagazineCount = 6;
    public GameObject hitEffectPs;
    public GameObject critHitEffectPs;

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
            Manager.ammoCountUI.text = "Ammo: " + value + "/" + CurrentAmmoCount;
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
            Manager.ammoCountUI.text = "Ammo: " + CurrentMagazineCount + "/" + value;
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

        Manager.ammoCountUI.text = "Ammo: " + CurrentMagazineCount + "/" + CurrentAmmoCount;

        base.Start();
    }


    private void Reset()
    {
        maxInteractDistance = 100;
    }

    public override void Interact() //How all guns shoot
    {
        if (CurrentMagazineCount <= 0)
            return;

        CurrentMagazineCount--;

        RaycastHit hit = GetCrosshairRay();
        if (hit.transform != null)
        {
            Destroy(Instantiate(hitEffectPs, hit.point, Quaternion.identity), 5);
            if(hit.transform.name == "Head")//TEMP CODE TEMP CODE TEMP CODE TEMP CODE TEMP CODE TEMP CODE TEMP CODE TEMP CODE TEMP CODE TEMP CODE 
                Destroy(Instantiate(critHitEffectPs, hit.point, Quaternion.identity), 5);
            StartCoroutine(ShootTrail(hit.point));
        }
        else
        {
            StartCoroutine(ShootTrail(mainCamera.transform.position + mainCamera.transform.forward * 50));
        }


        Shoot();
    }

    protected virtual void Shoot() { }//For Inherited Guns

    public virtual void Reload()
    {
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
}
