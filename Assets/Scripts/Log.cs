using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Log : MonoBehaviour
{
    [SerializeField]
    TextAsset textAsset;
    [SerializeField, TextArea(5, 20)]
    string logText;

    private void OnValidate()
    {
        if (textAsset != null)
            logText = textAsset.text;
        else
            logText = "";
    }

    private void OnDrawGizmosSelected()
    {
        if (textAsset != null)
            logText = textAsset.text;
        else
            logText = "";
    }

    public TextAsset GetTextAsset()
    {
        Destroy(gameObject);
        return textAsset;
    }

}
