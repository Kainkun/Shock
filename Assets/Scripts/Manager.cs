using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public static GameObject uiCanvas;
    public static GameObject crosshairCanvas;

    public static UIEquipmentSlots uiEquipmentSlots;

    static Image logBgUI;
    static TextMeshProUGUI logTextUI;
    static TextMeshProUGUI ammoCountUI;
    static TextMeshProUGUI healthUI;
    static TextMeshProUGUI helptextUI;
    static Image damageBlink;

    private static List<TextAsset> logs = new List<TextAsset>();



    private void Awake()
    {
        instance = this;

        uiCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UICanvas"));
        crosshairCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/CrosshairCanvas"));

        uiEquipmentSlots = uiCanvas.GetComponentInChildren<UIEquipmentSlots>();

        ammoCountUI = uiCanvas.transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        healthUI = uiCanvas.transform.Find("Health").GetComponent<TextMeshProUGUI>();
        damageBlink = uiCanvas.transform.Find("DamageBlink").GetComponent<Image>();
        logBgUI = uiCanvas.transform.Find("LogBGImage").GetComponent<Image>();
        logTextUI = logBgUI.transform.Find("LogText").GetComponent<TextMeshProUGUI>();
        logBgUI.gameObject.SetActive(false);
        helptextUI = uiCanvas.transform.Find("HelpText").GetComponent<TextMeshProUGUI>();
        helptextUI.gameObject.SetActive(false);

    }

    public static void AddLog(TextAsset textAsset)
    {
        logs.Add(textAsset);
        logTextUI.text = logs[logs.Count - 1].text;
    }

    public void ShowHelp(string helpText, float time)
    {
        if(helpCR != null)
            StopCoroutine(helpCR);
        helptextUI.text = helpText;
        helpCR = StartCoroutine(CR_ShowHelp(time));
    }
    Coroutine helpCR;
    IEnumerator CR_ShowHelp(float time)
    {
        helptextUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        helptextUI.gameObject.SetActive(false);
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

    public static void ShowAmmoCount(bool isOn = true)
    {
        ammoCountUI.gameObject.SetActive(isOn);
    }

    public static void ToggleLog()
    {
        logBgUI.gameObject.SetActive(!logBgUI.gameObject.activeSelf);
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

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
     {
         Vector3 AB = b - a;
         Vector3 AV = value - a;
         return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
     }

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public static float CalculatePathLength(NavMeshAgent agent, Vector3 goal)
    {
        NavMeshPath path = new NavMeshPath();

        if (agent.enabled)
            agent.CalculatePath(goal, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = agent.transform.position;
        allWayPoints[allWayPoints.Length - 1] = goal;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
    #endregion
}
