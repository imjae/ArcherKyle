using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalZone : MonoBehaviour
{
    public MONSTER_TYPE monsterType;

    private void Start()
    {
        monsterType = MONSTER_TYPE.Archer;
    }
}
