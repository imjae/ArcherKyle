using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    // 수평 방향 : RobotKyle
    public Transform horizontalDirection;
    // 수직 방향 : Camera
    public Transform verticalDirection;

    Rigidbody rigid;

    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        // direction = Vector3.Scale(transform.position, new Vector3(1, 0, 1));

        // Debug.Log($"{transform.position} / {transform.TransformPoint(transform.position)}");

        // StartCoroutine(DestroyArrow());
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = transform.forward * 50f;
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"트리거!!! {other.transform.name}");
    // }

    // private void OnCollisionEnter(Collision other)
    // {
    //     Debug.Log($"콜리전 !!! !{other.transform.name}");
    // }

    IEnumerator DestroyArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Destroy(this);
        }
    }
}
