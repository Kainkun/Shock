using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchAnimatorEvents : MonoBehaviour
{
    public void HitAttempt() => Player.instance.currentEquipment.GetComponent<Wrench>()?.AttemptHit();
    public void SwingDone() => Player.instance.currentEquipment.GetComponent<Wrench>()?.SwingDone();
}
