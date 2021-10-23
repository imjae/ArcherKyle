using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MONSTER_TYPE
{
    Archer,
    Swordsman,
    Mage,
    King,
    Grunt,
    Seeker
}

public class MonsterGenerator : MonsterFactory<MONSTER_TYPE>
{
    [SerializeField]
    private GameObject archer;
    [SerializeField]
    private GameObject swordsman;
    [SerializeField]
    private GameObject mage;
    [SerializeField]
    private GameObject king;
    [SerializeField]
    private GameObject grunt;
    [SerializeField]
    private GameObject seeker;

    protected override Monster Create(MONSTER_TYPE _type)
    {
        Monster monster = null;

        switch (_type)
        {
            case MONSTER_TYPE.Archer:
                monster = Instantiate(this.archer).GetComponent<Monster>();
                break;
            case MONSTER_TYPE.Swordsman:
                monster = Instantiate(this.swordsman).GetComponent<Monster>();
                break;
            case MONSTER_TYPE.Mage:
                monster = Instantiate(this.mage).GetComponent<Monster>();
                break;
            case MONSTER_TYPE.King:
                monster = Instantiate(this.king).GetComponent<Monster>();
                break;
            case MONSTER_TYPE.Grunt:
                monster = Instantiate(this.grunt).GetComponent<Monster>();
                break;
            case MONSTER_TYPE.Seeker:
                monster = Instantiate(this.seeker).GetComponent<Monster>();
                break;
        }

        return monster;
    }
}
