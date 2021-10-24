using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlinkText : MonoBehaviour
{
    public GameObject restartKeyText;

    private bool isAnykey;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetTimeScale(1f);
        restartKeyText = GameObject.Find("Canvas").transform.Find("pressKey").gameObject;
        isAnykey = false;
        StartCoroutine(BlinkWarningPressKey());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            isAnykey = true;
            SceneManager.LoadScene("Map_v1");
        }
    }

    public IEnumerator BlinkWarningPressKey()
    {
        while (!isAnykey)
        {
            restartKeyText.SetActive(!restartKeyText.activeSelf);
            yield return new WaitForSeconds(.5f);
        }
        restartKeyText.SetActive(true);
    }
}
