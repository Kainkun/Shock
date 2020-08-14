using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    #region movement varaibles
    public Transform securityCameraAxis;
    public Transform lightPosition;
    float lightAngle;
    public Transform angle1;
    public Transform angle2;
    public float speed = 1;
    public float pauseTime = 1;
    public float maxDistance = 10;
    public float tension = 0.5f;
    [Range(0f, 1f)]
    public float dampen = 0.2f;
    Transform player;
    #endregion
    
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
        lightPosition.GetComponent<Light>().range = maxDistance;
        lightAngle = lightPosition.GetComponent<Light>().spotAngle;
    }

    float T;
    float speedT;
    float targetT;
    void Update()
    {
        //targetT = (Mathf.Sin(Time.time * 2 * Mathf.PI / loopTime) + 1) / 2;
        speedT = Mathf.Lerp(speedT, (targetT - T) * tension, dampen);
        T += speedT;
        securityCameraAxis.localRotation = Quaternion.SlerpUnclamped(angle1.localRotation, angle2.localRotation, T);

        float playerCameraAngle = Vector3.Angle(player.position - lightPosition.position, lightPosition.forward);
        RaycastHit hit;
        if (Physics.Raycast(lightPosition.position, player.position - lightPosition.position, out hit, maxDistance))
        {
            if (hit.transform.tag == "Player" && playerCameraAngle <= lightAngle)
            {
                //cam sees player
            }
        }

        float emisT = Mathf.InverseLerp(lightAngle, 0, playerCameraAngle);
        ApplyEmisIntensity(Mathf.Lerp(0.1f, 30, Mathf.Pow(emisT,3)));
    }

    IEnumerator camMovement()
    {
        while (true)
        {
            while (targetT < 1)
            {
                targetT += Time.deltaTime * speed;
                yield return null;
            }
            yield return new WaitForSeconds(pauseTime);
            while (targetT > 0)
            {
                targetT -= Time.deltaTime * speed;
                yield return null;
            }
            yield return new WaitForSeconds(pauseTime);
            yield return null;
        }
    }

    void ApplyEmisIntensity(float intensity)
    {
        mat.SetFloat("EmisIntensity", intensity*100f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(angle1.position, angle1.forward * maxDistance);
        Gizmos.DrawRay(angle2.position, angle2.forward * maxDistance);
    }
}
