using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Arrow
{
    FireArrow()
    {
        attackPoint = 30f;
        currentArrowType = ARROW_TYPE.FIRE;
    }

    private void OnCollisionEnter(Collision other)
    {
    }
}
