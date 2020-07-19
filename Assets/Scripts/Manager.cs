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


    private void Awake()
    {
        instance = this;

        uiCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UICanvas"));
        crosshairCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/CrosshairCanvas"));

        crosshairRing = crosshairCanvas.transform.Find("ring").GetComponent<Image>();
        crosshairDot = crosshairCanvas.transform.Find("dot").GetComponent<Image>();
        ammoCountUI = uiCanvas.transform.Find("AmmoCount").GetComponent<Text>();
    }

    void Start()
    {

    }


    void Update()
    {
        
    }
}
