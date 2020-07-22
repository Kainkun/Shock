using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public static GameObject uiCanvas;
    public static GameObject crosshairCanvas;

    public static UIEquipmentSlots uiEquipmentSlots;

    static Text ammoCountUI;
    static Text healthUI;
    static Image damageBlink;


    private void Awake()
    {
        instance = this;

        uiCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UICanvas"));
        crosshairCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/CrosshairCanvas"));

        uiEquipmentSlots = uiCanvas.GetComponentInChildren<UIEquipmentSlots>();

        ammoCountUI = uiCanvas.transform.Find("AmmoCount").GetComponent<Text>();
        healthUI = uiCanvas.transform.Find("Health").GetComponent<Text>();
        damageBlink = uiCanvas.transform.Find("DamageBlink").GetComponent<Image>();
    }


    #region "UI Functions"
    public static void SetAmmoCountUI(float current, float total)
    {
        ammoCountUI.text = "Ammo: " + current + "/" + total;
    }

    public static void SetHealthUI(float current)
    {
        healthUI.text = "Health: " + current;
    }

    public static void SetDeadScreen()
    {
        damageBlink.enabled = true;
    }
    public static IEnumerator DamageBlink()
    {
        damageBlink.enabled = true;
        yield return new WaitForSeconds(0.1f);
        if(!Player.instance.dead)
            damageBlink.enabled = false;
    }
    #endregion

    #region "Scene Management"
    public static void LoadScene(int i) => SceneManager.LoadScene(i);
    public static void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    #endregion

    #region "Useful Functions"

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

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
    #endregion
}
