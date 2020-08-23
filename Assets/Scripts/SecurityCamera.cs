using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    float maxDistance;
    public Transform angle1;
    public Transform angle2;
    Light spotlight;
    Player player;
    float cameraFOV;
    Vector3 targetLookDirection;
    public Transform cameraAxis;

    void Start()
    {
        player = Player.instance;
        spotlight = GetComponentInChildren<Light>();
        cameraFOV = spotlight.spotAngle;
        maxDistance = spotlight.range;
    }

    void Update()
    {
        if (IsPlayerSeen())
            TargetPlayer();
        else
            TargetIdle();

        Movement();
        ChangeColor();
    }

    IEnumerator cameraLoop()
    {
        yield return new WaitForSeconds(1);
    }


    void TargetPlayer()
    {
        targetLookDirection = GetDirectionToTransform(player.mainCamera.transform);
    }

    void TargetIdle()
    {
        //set target to idle movement
    }

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


    void ChangeColor()
    {
        //lerp to color
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(angle1.position, angle1.forward * maxDistance);
        Gizmos.DrawRay(angle2.position, angle2.forward * maxDistance);
    }
}
