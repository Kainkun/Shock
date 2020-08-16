using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteracted;
    public float animationTime = 0.15f;
    Vector3 startPosition;
    public Vector3 endPosition;
    Quaternion startRotation;
    public Vector3 endRotation;
    Transform moveAxis;

    private void Awake()
    {
        moveAxis = transform.parent;
        startPosition = moveAxis.localPosition;
        startRotation = moveAxis.localRotation;
        if(isInteracted)
            t = 1;
    }

    public virtual void Interact()
    {
        isInteracted = !isInteracted;
    }

    float t;
    private void Update()
    {
        if (isInteracted)
            t += Time.deltaTime / animationTime;
        else
            t -= Time.deltaTime / animationTime;

        t = Mathf.Clamp01(t);

        moveAxis.localPosition = Vector3.Lerp(startPosition, endPosition, t);
        moveAxis.localRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(endRotation), t);  
    }
}
