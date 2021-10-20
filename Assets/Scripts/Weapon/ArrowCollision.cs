using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{

    Rigidbody rigid;
    Collider collider;

    ArrowMovement arrowMovement;


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        arrowMovement = GetComponent<ArrowMovement>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            arrowMovement.isMovement = false;
            rigid.velocity = Vector3.zero;

            Destroy(rigid);
            Destroy(collider);
        }

    }

}
