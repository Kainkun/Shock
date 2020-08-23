using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecurityCamera : MonoBehaviour
{
    float maxDistance;
    public Transform angle1;
    public Transform angle2;
    Light spotlight;
    Player player;
    float cameraFOV;
    Vector3 targetLookDirection;
    Color currentColor;
    Color targetColor;
    public Transform cameraAxis;
    public Transform cameraLens;
    Material cameraLensMaterial;
    float idleSpeed = 4.5f;
    float pauseTime = 1.5f;
    float timeSeeingPlayer;
    public UnityEvent releaseSecurity;
    float securityCooldownTime = 10;



    void Start()
    {
        player = Player.instance;
        spotlight = GetComponentInChildren<Light>();
        cameraLensMaterial = cameraLens.GetComponent<MeshRenderer>().material;
        cameraFOV = spotlight.spotAngle;
        maxDistance = spotlight.range;
        currentColor = Color.red;
        idleLerpT = Manager.InverseLerp(angle1.forward, angle2.forward, cameraAxis.forward);
        StartCoroutine(idleMovement());
    }

    void Update()
    {
        if (IsPlayerSeen())
        {
            TargetPlayer();
            targetColor = Color.white;
            idleLerpT = Manager.InverseLerp(angle1.forward, angle2.forward, cameraAxis.forward);
            timeSeeingPlayer += Time.deltaTime;
        }
        else
        {
            TargetIdle();
            targetColor = Color.red;
            timeSeeingPlayer = 0;
        }

        Security();
        Movement();
        ChangeColor();
    }

    IEnumerator idleMovement()
    {
        while (true)
        {
            while (idleLerpT < 1)
            {
                idleLerpT += Time.deltaTime / idleSpeed;
                yield return null;
            }
            yield return new WaitForSeconds(pauseTime);
            while (idleLerpT > 0)
            {
                idleLerpT -= Time.deltaTime / idleSpeed;
                yield return null;
            }
            yield return new WaitForSeconds(pauseTime);
            yield return null;
        }
    }


    void TargetPlayer()
    {
        targetLookDirection = GetDirectionToTransform(player.mainCamera.transform);
    }

    void TargetIdle()
    {
        speedT = Mathf.Lerp(speedT, (idleLerpT - T) * tension, dampen);
        T += speedT;
        targetLookDirection = Vector3.LerpUnclamped(angle1.forward, angle2.forward, T);
    }


    float tension = 0.5f;
    float dampen = 0.2f;
    float T;
    float speedT;
    float idleLerpT;
    void Movement() => cameraAxis.forward = Vector3.Lerp(cameraAxis.forward, targetLookDirection.normalized, Time.deltaTime * 3);
    Vector3 GetDirectionToTransform(Transform transform) => transform.position - spotlight.transform.position;
    float GetAngleBetweenObjectAndSpotlight(Transform transform) => Vector3.Angle(GetDirectionToTransform(transform), spotlight.transform.forward);
    bool IsObjectInFOV(Transform transform) => GetAngleBetweenObjectAndSpotlight(transform) <= cameraFOV / 2;
    bool IsPlayerSeen() => CheckIfObjectIsSeen(player.transform) || CheckIfObjectIsSeen(player.mainCamera.transform); //looks for head and feet
    bool CheckIfObjectIsSeen(Transform transform, string tag = "Player")
    {
        RaycastHit hit;
        if (Physics.Raycast(spotlight.transform.position, GetDirectionToTransform(transform), out hit, maxDistance))
            if (hit.transform.tag == tag && IsObjectInFOV(transform))
                return true;

        return false;
    }



    bool onCooldown;
    float currentSecurityCooldownTime;
    void Security()
    {
        if (timeSeeingPlayer > 3 && !onCooldown)
        {
            releaseSecurity.Invoke();
            onCooldown = true;
        }
        if (onCooldown)
            currentSecurityCooldownTime += Time.deltaTime;
        if (currentSecurityCooldownTime >= securityCooldownTime)
        {
            onCooldown = false;
            currentSecurityCooldownTime = 0;
        }
    }



    void ChangeColor()
    {
        //lerp to color
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 5);
        cameraLensMaterial.SetColor("_EmisColor", currentColor);
        spotlight.color = currentColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(angle1.position, angle1.forward * maxDistance);
        Gizmos.DrawRay(angle2.position, angle2.forward * maxDistance);
    }
}
