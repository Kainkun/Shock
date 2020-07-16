using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    GameObject crosshair;
    RectTransform crosshairRect;
    float noiseSeed;

    public float crosshairMoveSize = 1;
    public float crosshairSpeed = 1;
    public float maxInteractDistance = 10;

    protected Camera mainCamera;

    protected virtual void Awake()
    {
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        crosshairRect = crosshair.GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        noiseSeed = Random.Range(-10000,10000);
        mainCamera = Player.instance.mainCamera;
    }

    protected virtual void Update()
    {
        CrosshairMovement();
    }

    public bool hourglassToggle;
    protected virtual void CrosshairMovement() {
        if (hourglassToggle)
            CrosshairFigureEight();
        else
            CrosshairRandom();
    }
    public virtual void Interact() { }

    protected RaycastHit GetCrosshairRay()
    {
        Vector3 crosshairScreenPos = RectTransformUtility.WorldToScreenPoint(null, crosshairRect.position);
        Ray ray = mainCamera.ScreenPointToRay(crosshairScreenPos);
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
        crosshairRect.localPosition = crosshairMoveSize * new Vector2(0.5f - Mathf.PerlinNoise(Time.time * crosshairSpeed, noiseSeed), 0.5f - Mathf.PerlinNoise(Time.time * crosshairSpeed, 1000 + noiseSeed));
    }

}
