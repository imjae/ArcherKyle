using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFinalAttack : MonoBehaviour
{
    // 0 : Fire
    // 1 : Electric
    // 2 : Ice
    public GameObject[] effect;

    ElementController elementController;

    public Transform direction;
    public GameObject robotKyle;
    GameObject createEffect;

    private void Start()
    {
        direction = GameObject.Find("Camera").transform;
        robotKyle = GameObject.Find("Robot Kyle");
        elementController = robotKyle.GetComponent<ElementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 createPosition = transform.position;

        if (elementController.currentElement.Equals(ElementController.ELEMENT.FIRE))
            createEffect = effect[0];
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING))
            createEffect = effect[1];
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.ICE))
            createEffect = effect[2];


        // x축으로 -90 회전
        // y축 0
        Instantiate(createEffect, createPosition, Quaternion.Euler(-90f, direction.rotation.eulerAngles.y - 90, 0f), other.transform);
        // Debug.Log("트리거" + other.gameObject.name);
        // if (other.CompareTag("Ground"))
        //     Debug.Log(other.gameObject.name);
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }
}
