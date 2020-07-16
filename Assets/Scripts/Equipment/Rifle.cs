using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    private void Reset()
    {
        crosshairMoveSize = 30;
        crosshairSpeed = 3;
        maxInteractDistance = 100;
    }

    new void Update()
    {
        base.Update();
    }
}
