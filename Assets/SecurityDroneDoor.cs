using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDroneDoor : MonoBehaviour
{
    [SerializeField] GameObject Drone;

    public void ReleaseDrone()
    {
        print("release");
    }
}
