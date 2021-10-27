using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatusController : MonoBehaviour
{
    private GameObject _monsterFace;

    public Text monsterName;
    public Text monsterHealth;
    public Text monsterAttackPoint;
    public Text monsterRange;
    public Text monsterWeak;

    public string MonsterName
    {
        get { return monsterName.text; }
        set => monsterName.text = $"{value}";
    }
    public string MonsterHealth
    {
        get { return monsterHealth.text; }
        set => monsterHealth.text = $"HP :  {value}";
    }
    public string MonsterAttackPoint
    {
        get { return monsterAttackPoint.text; }
        set => monsterAttackPoint.text = $"ATK :  {value}";
    }
    public string MonsterRange
    {
        get { return monsterRange.text; }
        set => monsterRange.text = $"RANGE :  {value}";
    }
    public string MonsterWeak
    {
        get { return monsterWeak.text; }
        set => monsterWeak.text = $"WEAK :  {value}";
    }

    public void ActiveMonsterStatus()
    {
        gameObject.SetActive(true);
    }

    public void UnActiveMonsterStatus()
    {
        gameObject.SetActive(false);
    }

    public void SetMonsterName(string name)
    {
        var textComponent = monsterName.GetComponent<Text>();
        textComponent.text = name;
    }
}
