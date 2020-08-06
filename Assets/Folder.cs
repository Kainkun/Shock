using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder : MonoBehaviour
{
    private void Awake()
    {
        HashSet<Transform> list = new HashSet<Transform>();
        foreach (Transform child in transform)
            if (child.parent = transform)
                list.Add(child);

        foreach (Transform t in list)
            t.parent = null;

        Destroy(gameObject);
    }
}
