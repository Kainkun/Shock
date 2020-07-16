using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : MonoBehaviour
{

    Vector3 startPos;
    public bool strafing;
    public float strafeWidth = 5;
    public float strafeSpeed = 1;
    float seed;

    private void Start()
    {
        startPos = transform.position;
        seed = Random.Range(-5000, 5000);
    }

    public void ToggleStrafe()
    {
        strafing = !strafing;
    }


    void Update()
    {
        if (strafing)
        {
            transform.position = startPos + new Vector3(strafeWidth * Mathf.PerlinNoise(strafeSpeed * Time.time + seed, 0), 0, 0);
        }
        else
            transform.position = startPos;
    }
}
