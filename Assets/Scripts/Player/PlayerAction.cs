using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    Animator _animator;

    public enum WEAPON
    {
        NONE,
        BOW,
        SWORD
    }

    // 무기 장착 여부
    private bool isEquipWeapon;
    // 무기 교체중 여부
    private bool isSwitching;
    private WEAPON currentEquipWeapon;

    public GameObject backBow;
    public GameObject realBow;

    // Start is called before the first frame update
    void Start()
    {
        // UnActiveRealBow();
        isEquipWeapon = false;
        isSwitching = false;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isSwitching)
        {
            // 무기 장착 중일때
            if (isEquipWeapon)
            {
                _animator.SetTrigger("DontEquipBowTrigger");
                currentEquipWeapon = WEAPON.NONE;
                isEquipWeapon = false;
                StartCoroutine(SwitchDelay());
            }
            else
            {
                _animator.SetTrigger("EquipBowTrigger");
                currentEquipWeapon = WEAPON.BOW;
                isEquipWeapon = true;
                StartCoroutine(SwitchDelay());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isSwitching)
        {
            if (isEquipWeapon)
            {

            }
            else
            {

            }
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log($"backBow : {backBow.activeSelf} / realBow : {realBow.activeSelf}");
            backBow.SetActive(!backBow.activeSelf);
            realBow.SetActive(!realBow.activeSelf);
            Debug.Log($"backBow : {backBow.activeSelf} / realBow : {realBow.activeSelf}");
        }
    }

    private IEnumerator SwitchDelay()
    {
        isSwitching = true;
        yield return new WaitForSeconds(1f);
        isSwitching = false;
    }

    private void ActiveRealBow()
    {
        backBow.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false; // SetActive(false);
        realBow.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true; // realBow.SetActive(true);
    }
    private void UnActiveRealBow()
    {
        backBow.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true; // SetActive(false);
        realBow.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false; // realBow.SetActive(true);
    }
}
