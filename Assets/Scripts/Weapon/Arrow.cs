using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum ARROW_TYPE
    {
        FIRE,
        LIGHTNING,
        ICE
    }

    public ARROW_TYPE currentArrowType;
    public float attackPoint { get; set; }
}
