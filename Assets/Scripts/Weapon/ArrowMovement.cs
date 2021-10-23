using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    Rigidbody rigid;
    Collider colli;
    Vector3 direction;

    public bool isMovement;

    void Start()
    {
        colli = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        isMovement = true;
        colli.enabled = false;

    }

    private void FixedUpdate()
    {
        if (name.Contains("Clone") && isMovement)
        {
            colli.enabled = true;
            rigid.velocity = transform.forward * 20f;
        }
    }
}
