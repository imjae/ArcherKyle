using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonsterHealthBarSlider : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();

        Debug.Log(slider.fillRect.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
