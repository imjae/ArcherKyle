using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFeild : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthSystem>().regenerate = true;
            other.GetComponent<PlayerEnergySystem>().regenerate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthSystem>().regenerate = false;
            other.GetComponent<PlayerEnergySystem>().regenerate = false;
        }
    }
}
