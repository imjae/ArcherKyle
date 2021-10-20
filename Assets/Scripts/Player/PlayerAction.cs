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
    public WEAPON currentEquipWeapon;

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
    private ElementController elementController;

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

        cameraTransform = GameObject.Find("Camera").transform;
        cameraMovementScript = GameObject.Find("Camera").GetComponent<CameraMovement>();
        playerMovementScript = GameObject.Find("Robot Kyle").GetComponent<PlayerMovement>();
        elementController = GetComponent<ElementController>();

        UnActiveBow();
        UnActiveSword();
        UnActiveFireArrow();
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

    // RealSword�� ���̰� �ϴ� �Լ� (���⸦ ����� ��)
    private void ActiveSword()
    {
        Renderer backSwordRenderer = backSword.GetComponent<Renderer>();
        Renderer realSwordRenderer = realSword.GetComponent<Renderer>();

        backSwordRenderer.enabled = false;
        realSwordRenderer.enabled = true;

        ActiveSwordEffect();
    }
    // RealSword�� ���ߴ� �Լ� (���⸦ ���� ���� ��)
    private void UnActiveSword()
    {
        Renderer backSwordRenderer = backSword.GetComponent<Renderer>();
        Renderer realSwordRenderer = realSword.GetComponent<Renderer>();

        backSwordRenderer.enabled = true;
        realSwordRenderer.enabled = false;

        UnActiveSwordEffect();
    }

    // ������ ���õ� ������ ����Ʈ�� �Ѱ�, �������� ���� �Լ�
    public void ActiveSwordEffect()
    {
        Transform realSwordTransform = realSword.transform;
        string element = GetCurrentElementName();
        for (int i = 0; i < realSwordTransform.childCount; i++)
        {
            // ���õ� ���ҿ� �̸��� ���� ������Ʈ�� ��ƼŬ�� �Ѱ�, �������� ����.
            string name = realSwordTransform.GetChild(i).gameObject.name;
            if (name.Equals(element))
                ActiveSwordParticle(name);
            else
                UnActiveSwordParticle(name);
        }
    }

    // ������ ���õ� ������ ����Ʈ ��� ����
    public void UnActiveSwordEffect()
    {
        Transform realSwordTransform = realSword.transform;
        string element = GetCurrentElementName();
        for (int i = 0; i < realSwordTransform.childCount; i++)
        {
            // ���õ� ���ҿ� �̸��� ���� ������Ʈ�� ��ƼŬ�� �Ѱ�, �������� ����.
            string name = realSwordTransform.GetChild(i).gameObject.name;

            UnActiveSwordParticle(name);
        }
    }

    private void ActiveSwordParticle(string particleName)
    {
        Transform particle = realSword.transform.Find(particleName);
        particle.gameObject.SetActive(true);
        for (int i = 0; i < particle.childCount; i++)
        {
            particle.GetChild(i).gameObject.SetActive(true);
            if (i == particle.childCount - 1)
                particle.GetChild(i).GetComponent<ParticleSystem>().Stop();
            else
                particle.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
    }

    public void ActiveSwordFinalAttackEffect()
    {
        string name = GetCurrentElementName();
        Transform particle = realSword.transform.Find(name);

        // ������ �ڽ���ġ�� �ִ� ����Ʈ Play �����ش�.(��ġ ���Ѿ���)
        particle.GetChild(particle.childCount - 1).gameObject.SetActive(true);
        particle.GetChild(particle.childCount - 1).GetComponent<ParticleSystem>().Play();
    }

    private void UnActiveSwordParticle(string particleName)
    {
        Transform particle = realSword.transform.Find(particleName);
        particle.gameObject.SetActive(false);
        for (int i = 0; i < particle.childCount; i++)
        {
            particle.GetChild(i).gameObject.SetActive(false);
            particle.GetChild(i).GetComponent<ParticleSystem>().Stop();
        }
    }

    // ���� ���õ� ���� �̸� ��ȯ
    private string GetCurrentElementName()
    {
        string result = "";
        if (elementController.currentElement.Equals(ElementController.ELEMENT.FIRE))
            result = "FireParticle";
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.ELECTRIC))
            result = "ElectricParticle";
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.ICE))
            result = "IceParticle";

        return result;
    }
}
