using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    Rigidbody rigid;
    Collider colli;
    Vector3 direction;

    public bool isMovement;

    void OnEnable()
    {
        Invoke(nameof(DeactiveDelay), 3f);
        
        colli = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        isMovement = true;
        // colli.enabled = false;
    }

    void OnDisable()
    {
        ObjectPooler.ReturnToPool(this.gameObject);
        CancelInvoke();
    }

    private void FixedUpdate()
    {
        if (this.gameObject.activeSelf && isMovement)
        {
            colli.enabled = true;
            rigid.velocity = transform.forward * 20f;
        }
    }

    void DeactiveDelay() => gameObject.SetActive(false);
}
