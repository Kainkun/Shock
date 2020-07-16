using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    public UnityEvent PressButton;

    public void Press()
    {
        PressButton.Invoke();
    }
}
