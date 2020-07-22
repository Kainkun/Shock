using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public static GameObject uiCanvas;
    public static GameObject crosshairCanvas;

    public static Image crosshairRing;
    public static Image crosshairDot;

    public static Text ammoCountUI;
    public static Image damageBlink;


    private void Awake()
    {
        instance = this;

        uiCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UICanvas"));
        crosshairCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/CrosshairCanvas"));

        crosshairRing = crosshairCanvas.transform.Find("ring").GetComponent<Image>();
        crosshairDot = crosshairCanvas.transform.Find("dot").GetComponent<Image>();
        ammoCountUI = uiCanvas.transform.Find("AmmoCount").GetComponent<Text>();
        damageBlink = uiCanvas.transform.Find("DamageBlink").GetComponent<Image>();
    }

    void Start()
    {

    }


    void Update()
    {

    }

    public static IEnumerator DamageBlink()
    {
        damageBlink.enabled = true;
        yield return new WaitForSeconds(0.1f);
        damageBlink.enabled = false;
    }

    public static float InverseLerpUnclamped(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }

    public static float Remap(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = Mathf.InverseLerp(iMin, iMax, v);
        return Mathf.Lerp(oMin, oMax, t);
    }

    public static float RemapUnclamped(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = InverseLerpUnclamped(iMin, iMax, v);
        return Mathf.LerpUnclamped(oMin, oMax, t);
    }
}
