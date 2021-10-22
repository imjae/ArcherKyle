using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatusController : MonoBehaviour
{
    public GameObject monsterFace;
    public GameObject monsterName;

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
