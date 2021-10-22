using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : MonoBehaviour
{
    public enum ELEMENT
    {
        FIRE,
        LIGHTNING,
        ICE
    };

    public ELEMENT currentElement;

    // 원소 배열
    GameObject[] elementArr;

    int count;
    int currentElementIndex;


    private PlayerAction playActionScript;
    private GameObject canvas;
    private RectTransform elementPanel;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        elementPanel = canvas.transform.Find("StatusPanel").Find("CurrentElementPanel").GetComponent<RectTransform>();

        count = elementPanel.childCount;
        elementArr = new GameObject[count];

        for (int i = 0; i < count; i++)
            elementArr[i] = elementPanel.GetChild(i).gameObject;

        currentElementIndex = 0;
        SwitchingElement(currentElementIndex);
        currentElement = (ELEMENT)currentElementIndex;

        playActionScript = GetComponent<PlayerAction>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("prevElement"))
        {
            currentElementIndex--;
            if (currentElementIndex < 0)
                currentElementIndex = elementArr.Length - 1;
            SwitchingElement(currentElementIndex);
            currentElement = (ELEMENT)currentElementIndex;
            if (playActionScript.currentEquipWeapon.Equals(PlayerAction.WEAPON.SWORD))
                playActionScript.ActiveSwordEffect();
        }
        else if (Input.GetButtonDown("nextElement"))
        {
            currentElementIndex++;

            if (currentElementIndex > elementArr.Length - 1)
                currentElementIndex = 0;
            SwitchingElement(currentElementIndex);
            currentElement = (ELEMENT)currentElementIndex;

            if (playActionScript.currentEquipWeapon.Equals(PlayerAction.WEAPON.SWORD))
                playActionScript.ActiveSwordEffect();
        }
    }

    private void SwitchingElement(int index)
    {
        for (int i = 0; i < count; i++)
        {
            if (i == index)
            {
                elementArr[i].SetActive(true);
            }
            else
            {
                elementArr[i].SetActive(false);
            }
        }
    }
}

