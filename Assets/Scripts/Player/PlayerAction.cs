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

    public Transform hip;
    public Transform leftWristJoint01;
    public Transform rightWristJoint01;

    public GameObject backBow;
    public GameObject realBow;
    public GameObject backSword;
    public GameObject realSword;

    private GameObject cloneBackBow;
    private GameObject cloneRealBow;
    private GameObject cloneBackSword;
    private GameObject cloneRealSword;

    // Start is called before the first frame update
    void Start()
    {
        // UnActiveRealBow();
        isEquipWeapon = false;
        isSwitching = false;
        _animator = GetComponent<Animator>();

        cloneBackBow = Instantiate(backBow, hip);
        cloneRealBow = Instantiate(realBow, leftWristJoint01);
        cloneBackSword = Instantiate(backSword, hip);
        cloneRealSword = Instantiate(realSword, rightWristJoint01);
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
            // 무기 장착 중일때
            if (isEquipWeapon)
            {
                _animator.SetTrigger("DontEquipSwordTrigger");
                currentEquipWeapon = WEAPON.NONE;
                isEquipWeapon = false;
                StartCoroutine(SwitchDelay());
            }
            else
            {
                _animator.SetTrigger("EquipSwordTrigger");
                currentEquipWeapon = WEAPON.SWORD;
                isEquipWeapon = true;
                StartCoroutine(SwitchDelay());
            }
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            cloneBackBow.SetActive(!cloneBackBow.activeSelf);
            cloneRealBow.SetActive(!cloneRealBow.activeSelf);
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
        cloneBackBow.SetActive(false);
        cloneRealBow.SetActive(true);
    }
    private void UnActiveRealBow()
    {
        cloneBackBow.SetActive(true);
        cloneRealBow.SetActive(false);
    }
    private void ActiveRealSword()
    {
        cloneBackSword.SetActive(false);
        cloneRealSword.SetActive(true);
    }
    private void UnActiveRealSword()
    {
        cloneBackSword.SetActive(true);
        cloneRealSword.SetActive(false);
    }
}
