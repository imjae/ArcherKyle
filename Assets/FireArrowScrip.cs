using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowScrip : MonoBehaviour
{
    Vector3 localDirection;
    Vector3 worldDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // localDirection = transform.localPosition.normalized;
        // worldDirection = transform.TransformPoint(localDirection);
        // transform.forward = GetComponent<Rigidbody>().velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.transform.name);
    }
}
