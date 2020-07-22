using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    private void Reset()
    {
        maxInteractDistance = 100;
    }

    new void Update()
    {
        base.Update();
    }
}
