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

    public enum SWORD_COMBO
    {
        COMBO1,
        COMBO2,
        COMBO3,
        COMBOFINAL,
        NONE
    }

    public List<bool> isAttackSwordComboList;

    // ���� ���� ����
    private bool isEquipWeapon;
    // ���� ��ü�� ����
    private bool isSwitching;
    private WEAPON currentEquipWeapon;

    public Transform hip;
    public Transform leftWristJoint01;
    public Transform rightWristJoint01;

    // ���� ����� � �Ű� �ִ� ����
    public GameObject backBow;
    public GameObject realBow;
    public GameObject backSword;
    public GameObject realSword;
    // ���� Clone
    private GameObject cloneBackBow;
    private GameObject cloneRealBow;
    private GameObject cloneBackSword;
    private GameObject cloneRealSword;

    // Ȱ ���� ���� �ð�, ���� �߰� �ð�, �� �ð�
    private float drawArrowStartTime;
    private float drawArrowTime;
    private float drawArrowEndTime;

    // Į ���ݸ�� üũ(�޺� �̾���� ���� �ڷ�ƾ)
    private IEnumerator checkSwordCombo;
    private float swordButtonUpTime;
    private SWORD_COMBO currentSwordCombo;
    private int currentCombo;
    private int prevCombo;
    private bool isFirstCombo;

    bool isCombo;

    // ī�޶�� �÷��̾� �����ӿ� ���õ� ��ũ��Ʈ�� �����ϱ����� ����
    private CameraMovement cameraMovementScript;
    private PlayerMovement playerMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        // UnActiveRealBow();
        isEquipWeapon = false;
        isSwitching = false;
        drawArrowStartTime = 0f;
        drawArrowEndTime = 0f;
        _animator = GetComponent<Animator>();

        cloneBackBow = Instantiate(backBow, hip);
        cloneRealBow = Instantiate(realBow, leftWristJoint01);
        cloneBackSword = Instantiate(backSword, hip);
        cloneRealSword = Instantiate(realSword, rightWristJoint01);

        currentSwordCombo = PlayerAction.SWORD_COMBO.NONE;
        swordButtonUpTime = 0f;
        currentCombo = 0;
        prevCombo = 0;
        isFirstCombo = true;
        isAttackSwordComboList = new List<bool>() { false, false, false, false };

        cameraMovementScript = GameObject.Find("Camera").GetComponent<CameraMovement>();
        playerMovementScript = GameObject.Find("Robot Kyle").GetComponent<PlayerMovement>();

        StartCoroutine(SwordComboCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack"))
            isCombo = true;

        InputEquipWeapons();
        InputAttackWeapons();
    }

    private void FixedUpdate()
    {

    }

    private void InputAttackWeapons()
    {
        if (currentEquipWeapon.Equals(WEAPON.BOW))
        {
            if (Input.GetButtonDown("Attack"))
            {
                // 1��Ī ���� ��ȯ
                cameraMovementScript.curView = CameraMovement.VIEW.ONE;
                cameraMovementScript.TransCamersView();
                playerMovementScript.isAim = true;

                _animator.SetBool("IsAimed", playerMovementScript.isAim);
                drawArrowTime = 0f;
                drawArrowStartTime = GameManager.Instance.playeTime;
                _animator.SetTrigger("DrawArrowTrigger");
            }

            if (Input.GetButton("Attack"))
            {
                drawArrowTime = GameManager.Instance.playeTime - drawArrowStartTime;
                // _animator.SetTrigger("AimOverdrawTrigger");
            }

            if (Input.GetButtonUp("Attack"))
            {
                // 1��Ī ���� ��ȯ
                cameraMovementScript.curView = CameraMovement.VIEW.THIRD;
                cameraMovementScript.TransCamersView();
                playerMovementScript.isAim = false;

                _animator.SetBool("IsAimed", playerMovementScript.isAim);
                drawArrowTime = GameManager.Instance.playeTime - drawArrowStartTime;
                _animator.SetTrigger("AimRecoilTrigger");
                // Debug.Log(drawArrowEndTime - drawArrowStartTime);
            }
        }

        // if (currentEquipWeapon.Equals(WEAPON.SWORD))
        // {
        //     // ���� ��ư Ŭ��(���콺 ����)
        //     if (Input.GetButtonDown("Attack"))
        //     {
        //         if (GameManager.Instance.playeTime - swordButtonUpTime < 1f || currentCombo == 0)
        //         {
        //             if (currentCombo == 0 && isFirstCombo)
        //             {
        //                 isFirstCombo = false;
        //                 _animator.SetTrigger("AttackSwordTrigger");
        //             }


        //             if (currentCombo == 4)
        //             {
        //                 currentCombo = 0;
        //             }
        //         }
        //         else
        //         {
        //             isFirstCombo = true;
        //             currentCombo = 0;
        //             _animator.SetInteger("AttackSwordCombo", 0);
        //         }
        //     }

        //     if (Input.GetButtonUp("Attack"))
        //     {
        //         swordButtonUpTime = GameManager.Instance.playeTime;

        //     }
        // }

    }

    private void InputEquipWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isSwitching)
        {
            // ���� ���� ���϶�
            if (isEquipWeapon)
            {
                if (currentEquipWeapon.Equals(WEAPON.SWORD))
                {
                    _animator.SetTrigger("DontEquipSwordTrigger");
                    currentEquipWeapon = WEAPON.NONE;
                    isEquipWeapon = false;
                }
                else if (currentEquipWeapon.Equals(WEAPON.BOW))
                {
                    _animator.SetTrigger("EquipBow2SwordTrigger");
                    currentEquipWeapon = WEAPON.SWORD;
                    isEquipWeapon = true;
                }
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
        // Ȱ
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isSwitching)
        {
            // ���� ���� ���϶�
            if (isEquipWeapon)
            {
                if (currentEquipWeapon.Equals(WEAPON.SWORD))
                {
                    _animator.SetTrigger("EquipSword2BowTrigger");
                    currentEquipWeapon = WEAPON.BOW;
                    isEquipWeapon = true;
                }
                else if (currentEquipWeapon.Equals(WEAPON.BOW))
                {
                    _animator.SetTrigger("DontEquipBowTrigger");
                    currentEquipWeapon = WEAPON.NONE;
                    isEquipWeapon = false;
                }
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
    }

    private IEnumerator SwordComboCoroutine()
    {

        while (true)
        {
            if (currentEquipWeapon.Equals(WEAPON.SWORD))
            {

                if (Input.GetButtonDown("Attack"))
                {
                    if (isCombo)
                    {
                        isCombo = false;
                        prevCombo = this.currentCombo;
                        if (this.currentCombo == 0)
                            _animator.SetTrigger("AttackSwordTrigger");

                        this.currentCombo++;

                        if (this.currentCombo == 4)
                        {
                            this.currentCombo = 0;
                            _animator.SetInteger("AttackSwordCombo", this.currentCombo);
                        }
                        else
                        {
                            _animator.SetInteger("AttackSwordCombo", this.currentCombo);
                        }
                        yield return new WaitForSeconds(.9f);

                    }
                    else
                    {
                        //�ʱ�ȭȭ
                        this.currentCombo = 0;
                        _animator.SetInteger("AttackSwordCombo", 4);
                    }

                }
            }
            yield return null;
        }
    }

    private IEnumerator SwitchDelay()
    {
        isSwitching = true;
        yield return new WaitForSeconds(1f);
        isSwitching = false;
    }

    private int GetCurrentGetCombo()
    {
        for (int i = 0; i < isAttackSwordComboList.Count; i++)
        {
            if (isAttackSwordComboList[i])
            {
                return i;
            }
        }
        return -1;
    }
    private void SetSwordCombo(int combo)
    {
        _animator.SetInteger("AttackSwordCombo", combo);
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
