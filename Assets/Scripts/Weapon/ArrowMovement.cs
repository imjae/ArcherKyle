using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    // ���� ���� : RobotKyle
    public Transform horizontalDirection;
    // ���� ���� : Camera
    public Transform verticalDirection;

    Rigidbody rigid;

    Vector3 direction;


    public Vector3 prevPosition;
    public Vector3 prevVelocity;

    public bool isMovement;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        isMovement = true;

    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (name.Contains("Clone") && isMovement)
            rigid.velocity = transform.forward * 20f;
    }
}
