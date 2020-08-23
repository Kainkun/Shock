using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class oldSecurityCamera : MonoBehaviour
{
    public UnityEvent releaseSecurity;
    public Transform lightTransform;
    float alertTime;
    float timeNotSeeingPlayer;

    #region movement varaibles
    public Transform securityCameraAxis;
    float lightAngle;
    public Transform angle1;
    public Transform angle2;
    public float speed = 1;
    float currentSpeedPercent = 1;
    public float pauseTime = 1;
    public float maxDistance = 10;
    public float tension = 0.5f;
    [Range(0f, 1f)]
    public float dampen = 0.2f;
    Transform player;
    #endregion

    public Color searchingColor;
    public Color alertColor;

    public MeshRenderer camLensMeshR;
    Material mat;

    private void Awake()
    {
        mat = camLensMeshR.GetComponent<MeshRenderer>().material;
    }

    void Start()
    {
        player = Player.instance.transform;
        StartCoroutine(camMovement());
        lightTransform.GetComponent<Light>().range = maxDistance;
        lightAngle = lightTransform.GetComponent<Light>().spotAngle;
        lightTransform.GetComponent<Light>().color = searchingColor;
    }

    float colorT = 0;
    float T;
    float speedT;
    float targetT;
    void Update()
    {
        //targetT = (Mathf.Sin(Time.time * 2 * Mathf.PI / loopTime) + 1) / 2;
        speedT = Mathf.Lerp(speedT, (targetT - T) * tension, dampen);
        T += speedT;
        securityCameraAxis.localRotation = Quaternion.SlerpUnclamped(angle1.localRotation, angle2.localRotation, T);

        float playerCameraAngle = Vector3.Angle(player.position - lightTransform.position, lightTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(lightTransform.position, player.position - lightTransform.position, out hit, maxDistance))
        {
            if (hit.transform.tag == "Player" && playerCameraAngle <= lightAngle - 20)
            {
                currentSpeedPercent = 0;
                timeNotSeeingPlayer = 0;

                colorT += Time.deltaTime * 2;
                //can see player
            }
            else
            {
                colorT -= Time.deltaTime * 2;
            }
            colorT = Mathf.Clamp01(colorT);
            lightTransform.GetComponent<Light>().color = Color.Lerp(searchingColor, alertColor, colorT);
        }
        else
        {
            timeNotSeeingPlayer += Time.deltaTime;
        }

        float emisT = Mathf.InverseLerp(lightAngle, 0, playerCameraAngle);
        ApplyEmisIntensity(Mathf.Lerp(0.1f, 30, Mathf.Pow(emisT, 3)));

        if (colorT >= 1)
        {
            alertTime += Time.deltaTime;
        }
        if (timeNotSeeingPlayer > 4)
        {
            alertTime = 0;
            currentSpeedPercent = 1;
        }
        if (alertTime > 2)
        {
            releaseSecurity.Invoke();
        }
    }

    IEnumerator camMovement()
    {
        while (true)
        {
            while (targetT < 1)
            {
                targetT += Time.deltaTime * speed * currentSpeedPercent;
                yield return null;
            }
            yield return new WaitForSeconds(pauseTime);
            while (targetT > 0)
            {
                targetT -= Time.deltaTime * speed * currentSpeedPercent;
                yield return null;
            }
            yield return new WaitForSeconds(pauseTime);
            yield return null;
        }
    }

    void ApplyEmisIntensity(float intensity)
    {
        mat.SetFloat("EmisIntensity", intensity * 100f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(angle1.position, angle1.forward * maxDistance);
        Gizmos.DrawRay(angle2.position, angle2.forward * maxDistance);
    }
}
