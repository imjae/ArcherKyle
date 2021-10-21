using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Chaser
{
    // protected float healthPoint { get; set; }
    // protected float maxHealthPoint { get; set; }
    // protected float energyPoint { get; set; }
    // protected float maxEnergyPoint { get; set; }
    // protected float attackValue { get; set; }
    // protected float speedValue { get; set; }
    // protected Vector3 currentPosition { get; set; }

    private void Start()
    {

        healthSystem = new HealthSystem()
        {
            hitPoint = 100f,
            maxHitPoint = 100f,
            regenerate = true,
            regen = 0.1f,
            regenUpdateInterval = 1f,
            GodMode = false
        };

        attackValue = 10f;
        attackRange = 10f;
    }

    protected override void PlayerDetection()
    {
        base.PlayerDetection();
    }
}
