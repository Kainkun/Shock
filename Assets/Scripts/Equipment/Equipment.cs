using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public CrosshairSetting crosshairSetting;
    public float maxInteractDistance = 10;
    public Sprite Icon;

    protected Camera mainCamera;

    protected virtual void Awake() { }
    protected virtual void Start()
    {
        mainCamera = Player.instance.mainCamera;
    }
    protected virtual void Update() { }
    protected virtual void OnEnable()
    {
        CrosshairManager.currentCrosshairSetting = crosshairSetting;
    }

    public virtual void Interact() { }

    protected RaycastHit GetCrosshairHit()
    {
        Ray r;
        return GetCrosshairHit(out r);
    }
    protected RaycastHit GetCrosshairHit(out Ray outRay)
    {
        Vector3 crosshairScreenPos = RectTransformUtility.WorldToScreenPoint(null, CrosshairManager.crosshairRect.position);
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

}
