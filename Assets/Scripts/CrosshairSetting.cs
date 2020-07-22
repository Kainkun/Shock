using UnityEngine;

[CreateAssetMenu(fileName = "Crosshair Setting", menuName = "ScriptableObjects/Crosshair Setting", order = 1)]
public class CrosshairSetting : ScriptableObject
{
    public float moveSize = 1;
    public float speed = 1;
    public CrosshairManager.CrosshairMovementPattern movementPattern;
}