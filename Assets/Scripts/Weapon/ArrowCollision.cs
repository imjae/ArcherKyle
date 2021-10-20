using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{

    Rigidbody rigid;
    Collider col;

    ArrowMovement arrowMovement;

    [SerializeField]
    private GameObject arrowExplosion;


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        arrowMovement = GetComponent<ArrowMovement>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {

            arrowMovement.isMovement = false;
            rigid.velocity = Vector3.zero;

            Destroy(rigid);
            Destroy(col);

            Instantiate(arrowExplosion, transform.position, Quaternion.identity);

        }

    }

}
