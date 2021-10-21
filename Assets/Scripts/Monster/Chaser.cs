using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어를 쫓는 추격꾼
public class Chaser : Monster
{
    public enum Type
    {
        Archer,
        Swordsman,
        Mage,
        Grunt,
        King
    }
}
