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

    // ���� ���� ����
    private bool isEquipWeapon;
    // ���� ��ü�� ����
    private bool isSwitching;
    private WEAPON currentEquipWeapon;

    // ���⸦ ã�� ������ Parent ��ü
    public Transform hip;
    public Transform leftWristJoint;
    public Transform rightWristJoint;

    // ���� ����� � �Ű� �ִ� ����
    private GameObject backBow;
    private GameObject realBow;
    private GameObject backSword;
    private GameObject realSword;
    // Fire Arrow
    private GameObject fireArrow;

    // Ȱ ���� ���� �ð�, ���� �߰� �ð�, �� �ð�
    private float drawArrowStartTime;
    private float drawArrowTime;
    private float drawArrowEndTime;

    // Į���� �޺�
    private bool comboPossible;
    int comboStep;

    // ī�޶�� �÷��̾� �����ӿ� ���õ� ��ũ��Ʈ�� �����ϱ����� ����
    private CameraMovement cameraMovementScript;
    private PlayerMovement playerMovementScript;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        // UnActiveRealBow();
        isEquipWeapon = false;
        isSwitching = false;
        drawArrowStartTime = 0f;
        drawArrowEndTime = 0f;
        _animator = GetComponent<Animator>();

        backBow = hip.Find("BackBow").gameObject;
        realBow = leftWristJoint.Find("RealBow").gameObject;
        backSword = hip.Find("BackSword").gameObject;
        realSword = rightWristJoint.Find("RealSword").gameObject;

        fireArrow = realBow.transform.GetChild(2).gameObject;

        UnActiveBow();
        UnActiveSword();
        UnActiveFireArrow();

        cameraTransform = GameObject.Find("Camera").transform;
        cameraMovementScript = GameObject.Find("Camera").GetComponent<CameraMovement>();
        playerMovementScript = GameObject.Find("Robot Kyle").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
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

                // Vector3 localToWorldPosition = transform.TransformPoint(fireArrow.transform.position);

                var clone = Instantiate(fireArrow, fireArrow.transform.position, Quaternion.identity);
                clone.transform.rotation = cameraTransform.rotation;

                Debug.DrawRay(cameraTransform.position, cameraTransform.position.normalized * 1000f, Color.red);

                var localScale = clone.transform.localScale;
                clone.transform.localScale = new Vector3(localScale.x * 9, localScale.y * 9, localScale.z * 9);

                // ȭ�� UnAtive�ϰ� Ŭ�е� ȭ�� �߽�
                UnActiveBow();
            }
        }

        else if (currentEquipWeapon.Equals(WEAPON.SWORD))
        {
            if (Input.GetButtonDown("Attack"))
                SwordAttack();
        }
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

    private IEnumerator SwitchDelay()
    {
        isSwitching = true;
        yield return new WaitForSeconds(1f);
        isSwitching = false;
    }

    public void SwordAttack()
    {
        if (comboStep == 0)
        {
            _animator.Play("SwordCombo1");
            comboStep = 1;
            return;
        }

        if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboStep == 4)
            _animator.Play("SwordComboFinal");
        else
            _animator.Play($"SwordCombo{comboStep}");
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }

    private void ActiveFireArrow()
    {
        fireArrow.GetComponent<ParticleSystem>().Play();
        fireArrow.SetActive(true);
    }

    private void UnActiveFireArrow()
    {
        fireArrow.GetComponent<ParticleSystem>().Stop();
        fireArrow.SetActive(false);
    }

    private void ActiveBow()
    {
        Renderer backBowRenderer = backBow.transform.GetChild(0).GetComponent<Renderer>();
        Renderer realBowRenderer = realBow.transform.GetChild(0).GetComponent<Renderer>();

        // ActiveFireArrow();

        backBowRenderer.enabled = false;
        realBowRenderer.enabled = true;
    }
    private void UnActiveBow()
    {
        Renderer backBowRenderer = backBow.transform.GetChild(0).GetComponent<Renderer>();
        Renderer realBowRenderer = realBow.transform.GetChild(0).GetComponent<Renderer>();

        UnActiveFireArrow();

        backBowRenderer.enabled = true;
        realBowRenderer.enabled = false;
    }
    private void ActiveSword()
    {
        Renderer backSwordRenderer = backSword.GetComponent<Renderer>();
        Renderer realSwordRenderer = realSword.GetComponent<Renderer>();

        realSword.transform.Find("FireEnchantment").GetComponent<ParticleSystem>().Play();
        realSword.transform.Find("FireEnchantment").gameObject.SetActive(true);
        realSword.transform.Find("SwordTrailFire").GetComponent<ParticleSystem>().Play();
        realSword.transform.Find("SwordTrailFire").gameObject.SetActive(true);

        backSwordRenderer.enabled = false;
        realSwordRenderer.enabled = true;
    }
    private void UnActiveSword()
    {
        Renderer backSwordRenderer = backSword.GetComponent<Renderer>();
        Renderer realSwordRenderer = realSword.GetComponent<Renderer>();

        realSword.transform.Find("FireEnchantment").GetComponent<ParticleSystem>().Stop();
        realSword.transform.Find("FireEnchantment").gameObject.SetActive(false);
        realSword.transform.Find("SwordTrailFire").GetComponent<ParticleSystem>().Stop();
        realSword.transform.Find("SwordTrailFire").gameObject.SetActive(false);

        backSwordRenderer.enabled = true;
        realSwordRenderer.enabled = false;
    }

    private void instantiateRedArrow()
    {
        // this.cloneFireArrow = Instantiate(fireArrow);
        // this.cloneFireArrow.transform.SetParent(rightWristJoint);
        // this.cloneFireArrow.transform.localPosition = new Vector3(0.555f, -0.074f, -0.08f);
        // this.cloneFireArrow.transform.localRotation = Quaternion.Euler(new Vector3(6.052f, 90f, 0f));
        // this.cloneFireArrow.transform.localScale = new Vector3(1.1f, 1.1f, 1.7f);
    }


}
