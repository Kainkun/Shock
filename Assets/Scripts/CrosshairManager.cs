using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager crosshairManager;

    public static Image crosshairRing;
    public static Image crosshairDot;
    public static RectTransform crosshairRect;
    float noiseSeed;
    public static CrosshairSetting currentCrosshairSetting;

    public enum CrosshairMovementPattern { None, Hourglass, Horizontal, Random };

    private void Awake()
    {
        crosshairManager = this;
        crosshairRing = transform.GetChild(0).GetComponent<Image>();
        crosshairDot = transform.GetChild(1).GetComponent<Image>();
        crosshairRect = crosshairDot.GetComponent<RectTransform>();
        noiseSeed = Random.Range(-10000, 10000);
    }

    void Update()
    {
        CrosshairMovement();
    }

    protected virtual void CrosshairMovement()
    {
        if(currentCrosshairSetting == null)
        {
            crosshairRect.localPosition = Vector2.zero;
            return;
        }

        switch (currentCrosshairSetting.movementPattern)
        {
            case CrosshairMovementPattern.Hourglass:
                CrosshairFigureEight();
                break;
            case CrosshairMovementPattern.Horizontal:
                CrosshairHorizontal();
                break;
            case CrosshairMovementPattern.Random:
                CrosshairRandom();
                break;
            default:
                crosshairRect.localPosition = Vector2.zero;
                break;
        }
    }

    protected void CrosshairFigureEight()
    {
        crosshairRect.localPosition = currentCrosshairSetting.moveSize * new Vector2(Mathf.Sin(2 * Time.time * currentCrosshairSetting.speed) / 3, Mathf.Cos(Time.time * currentCrosshairSetting.speed));
    }

    protected void CrosshairRandom()
    {
        crosshairRect.localPosition = currentCrosshairSetting.moveSize * new Vector2(0.5f - Mathf.PerlinNoise(Time.time * currentCrosshairSetting.speed, noiseSeed), 0.5f - Mathf.PerlinNoise(1000 + noiseSeed, Time.time * currentCrosshairSetting.speed));
    }

    protected void CrosshairHorizontal()
    {
        crosshairRect.localPosition = currentCrosshairSetting.moveSize * new Vector2(Mathf.Sin(2 * Time.time * currentCrosshairSetting.speed), 0);
    }

    public static void RingColor(Color color) => crosshairRing.color = color;
    public static void DotColor(Color color) => crosshairDot.color = color;
}
