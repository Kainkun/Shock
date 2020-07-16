using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    private void Reset()
    {
        crosshairMoveSize = 30;
        crosshairSpeed = 3;
        maxInteractDistance = 100;
    }

 

/*    protected override void CrosshairMovement()
    {
        base.CrosshairRandom();
        //base.CrosshairFigureEight();
    }*/

    protected override void Shoot()
    {

    }

}
