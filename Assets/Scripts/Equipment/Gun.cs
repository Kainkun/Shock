using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Gun : Equipment
{
    public float fireRate = 1;
    public float ammoCapacity = 10;
    float ammoCount;
    public GameObject hitEffectPs;
    public GameObject critHitEffectPs;

    public Text ammoCountUI;
    public Transform shootPoint;
    LineRenderer lr;

    public float AmmoCount
    {
        get { return ammoCount; }
        set
        {
            ammoCount = AmmoCount;
            ammoCountUI.text = "Ammo: " + AmmoCount;
        }
    }

    protected override void Awake()
    {
        lr = GetComponent<LineRenderer>();

        base.Awake();
    }

    protected override void Start()
    {

        AmmoCount = ammoCapacity;

        base.Start();
    }


    private void Reset()
    {
        maxInteractDistance = 100;
    }

    public override void Interact() //How all guns shoot
    {
        AmmoCount--;
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

    IEnumerator ShootTrail(Vector3 hitPosition)
    {
        lr.SetPosition(0, shootPoint.position);
        lr.SetPosition(1, hitPosition);
        lr.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lr.enabled = false;
    }
}
