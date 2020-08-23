using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDroneDoor : MonoBehaviour
{
    [SerializeField] GameObject Drone;
    bool cooldown;
    public float cooldownTime;
    float currentCooldownTime;

    private void Update()
    {
        if(cooldown)
            currentCooldownTime += Time.deltaTime;
        if(currentCooldownTime >= cooldownTime)
        {
            cooldown = false;
            currentCooldownTime = 0;
        }
    }

    public void ReleaseDrone()
    {
        if (cooldown)
            return;


        print("release");
        cooldown = true;
    }
}
