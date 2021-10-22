using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private float _attackValue;
    private float _attackRange;
    private float _speedValue;

    protected float AttackValue
    {
        get { return _attackValue; }
        set { _attackValue = value; }
    }
    protected float AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }
    protected float SpeedValue
    {
        get { return _speedValue; }
        set { _speedValue = value; }
    }
}
