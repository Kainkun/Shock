using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAnimatorEvents : MonoBehaviour
{
    public void ReloadDone() => Player.instance.currentEquipment.GetComponent<Gun>()?.Reload();
}
