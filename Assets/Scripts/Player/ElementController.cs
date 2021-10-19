using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : MonoBehaviour
{
    public enum ELEMENT
    {
        FIRE,
        ELECTRIC,
        ICE
    };

    public ELEMENT currentElement;

    GameObject[] elementArr;

    int childCount;
    int currentElementIndex;

    // Start is called before the first frame update
    void Start()
    {
        childCount = transform.childCount;
        elementArr = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
            elementArr[i] = transform.GetChild(i).gameObject;

        currentElementIndex = 0;
        SwitchingElement(currentElementIndex);
        currentElement = (ELEMENT)currentElementIndex;

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
        }
        else if (Input.GetButtonDown("nextElement"))
        {
            currentElementIndex++;

            if (currentElementIndex > elementArr.Length - 1)
                currentElementIndex = 0;
            SwitchingElement(currentElementIndex);
            currentElement = (ELEMENT)currentElementIndex;
        }
    }

    private void SwitchingElement(int index)
    {
        for (int i = 0; i < childCount; i++)
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

