using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    private void Reset()
    {
        maxInteractDistance = 100;
    }



    /*    protected override void CrosshairMovement()
        {
            base.CrosshairRandom();
            //base.CrosshairFigureEight();
        }*/


    public override void Interact()
    {
        base.Interact();
    }

}
