using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public enum AmmoType {Pistol, Rifle};
    public AmmoType ammoType;
    public float ammoCount = 12;

    public float Pickup()
    {
        Destroy(gameObject);
        return ammoCount;
    }
}
