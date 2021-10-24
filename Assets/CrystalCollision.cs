using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCollision : MonoBehaviour
{
    private int hitCount;
    private ParticleSystem explosion;
    private bool isDestroy;

    [SerializeField]
    private GameObject seeker;

    private void Start()
    {
        hitCount = 0;
        isDestroy = false;
        explosion = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " / " + other.name);

        if (other.CompareTag("MonsterAttack") && other.name.Equals("SeekerSword"))
        {
            hitCount++;
            Debug.Log("Å©¸®½ºÅ» °ø°Ý ¹ÞÀº È¹¼ö : " + hitCount);
            if (hitCount == 5 && !isDestroy)
            {
                isDestroy = true;
                explosion.Play();
                StartCoroutine(SwitchDelayIntoAction(1f, () =>
               {
                   Destroy(this.gameObject);
                   var seeker = GetRootObject(other.transform);
                   var seekerScript = seeker.GetComponent<SkeletonSeeker>();
                   seekerScript.currentTarget = seekerScript.targetQueue.Dequeue();
               }));

            }
        }

    }

    private IEnumerator SwitchDelayIntoAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    private GameObject GetRootObject(Transform transform)
    {
        if (transform.name.Equals("Bip001"))
        {
            Debug.Log(transform.parent.name);
            return transform.parent.gameObject;
        }
        else
        {
            return GetRootObject(transform.parent);
        }
    }
}
