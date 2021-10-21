using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherArrow : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 direction;

    public bool isMovement;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        isMovement = true;

    }

    private void FixedUpdate()
    {
        if (name.Contains("Clone") && isMovement)
            rigid.velocity = transform.forward * 20f;

    }
}
