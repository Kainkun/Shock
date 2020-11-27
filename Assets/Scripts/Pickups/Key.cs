using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public KeyData keyData;
    public KeyData getKey()
    {
        Destroy(gameObject);
        return keyData;
    }
}
