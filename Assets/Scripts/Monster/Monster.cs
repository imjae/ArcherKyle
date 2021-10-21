using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Character
{
    protected HealthSystem healthSystem { get; set; }

    protected virtual void TargetDetection(Transform target)
    {

    }
}
