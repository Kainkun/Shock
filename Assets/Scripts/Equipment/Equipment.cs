using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    RectTransform crosshairRect;
    float noiseSeed;

    public float crosshairMoveSize = 1;
    public float crosshairSpeed = 1;
    public float maxInteractDistance = 10;
    public enum CrosshairMovementMode { None, Hourglass, Horizontal, Random};
    public CrosshairMovementMode currentCrosshairMovement;

    protected Camera mainCamera;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        crosshairRect = Manager.crosshairDot.GetComponent<RectTransform>();
        noiseSeed = Random.Range(-10000,10000);
        mainCamera = Player.instance.mainCamera;
        currentCrosshairMovement = Equipment.CrosshairMovementMode.Random;
    }

    protected virtual void Update()
    {
        CrosshairMovement();
    }

    protected virtual void CrosshairMovement() {
        if (currentCrosshairMovement == CrosshairMovementMode.Hourglass)
            CrosshairFigureEight();
        else if (currentCrosshairMovement == CrosshairMovementMode.Horizontal)
            CrosshairHorizontal();
        else if (currentCrosshairMovement == CrosshairMovementMode.Random)
            CrosshairRandom();
        else
            crosshairRect.localPosition = Vector2.zero;
    }
    public virtual void Interact() { }

    protected RaycastHit GetCrosshairHit()
    {
        Ray r;
        return GetCrosshairHit(out r);
    }
    protected RaycastHit GetCrosshairHit(out Ray outRay)
    {
        Vector3 crosshairScreenPos = RectTransformUtility.WorldToScreenPoint(null, crosshairRect.position);
        Ray ray = mainCamera.ScreenPointToRay(crosshairScreenPos);
        outRay = ray;
        Debug.DrawRay(ray.GetPoint(0), ray.direction, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractDistance))
        {
            return hit;
        }
        else
            return new RaycastHit();
    }

    protected RaycastHit GetStraightRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, maxInteractDistance))
        {
            Debug.DrawRay(mainCamera.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            return hit;
        }
        else
            return new RaycastHit();

    }



    protected void CrosshairFigureEight()
    {
        crosshairRect.localPosition = crosshairMoveSize * new Vector2(Mathf.Sin(2 * Time.time * crosshairSpeed) / 3, Mathf.Cos(Time.time * crosshairSpeed));
    }

    protected void CrosshairRandom()
    {
        crosshairRect.localPosition = crosshairMoveSize * new Vector2(0.5f - Mathf.PerlinNoise(Time.time * crosshairSpeed, noiseSeed), 0.5f - Mathf.PerlinNoise(1000 + noiseSeed, Time.time * crosshairSpeed));
    }

    protected void CrosshairHorizontal()
    {
        crosshairRect.localPosition = crosshairMoveSize * new Vector2(Mathf.Sin(2 * Time.time * crosshairSpeed), 0);
    }

}
