using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected float attackValue { get; set; }
    protected float attackRange { get; set; }
    protected float speedValue { get; set; }
    protected Vector3 currentPosition { get; set; }
}
