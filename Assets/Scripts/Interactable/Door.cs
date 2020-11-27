using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked;
    public KeyData requiredKey;
    public float slideDistance;
    Vector3 startPos;
    Vector3 endPos;

    private void Start()
    {
        
    }

    public virtual void Interact()
    {
        if (!requiredKey || Player.instance.HasKey(requiredKey))
        {

        }
    }

    private void Update()
    {
        foreach (var child in transform)
        {

        }
    }
}
