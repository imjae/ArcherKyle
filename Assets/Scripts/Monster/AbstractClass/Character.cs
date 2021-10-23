using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private float _attackValue;
    private float _attackRange;
    private float _speedValue;

    public float AttackValue
    {
        get { return _attackValue; }
        set { _attackValue = value; }
    }
    public float AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }
    public float SpeedValue
    {
        get { return _speedValue; }
        set { _speedValue = value; }
    }
}
