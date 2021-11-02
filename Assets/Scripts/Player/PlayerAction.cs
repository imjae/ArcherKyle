using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour
{
    Animator _animator;

    public enum WEAPON
    {
        NONE,
        BOW,
        SWORD
    }

    private PlayerHealthSystem healthSystem;
    private PlayerEnergySystem energySystem;

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
    
    private Transform arrowPoint;

    // Į���� �޺�
    private bool comboPossible;
    int comboStep;
    // ī�޶�� �÷��̾� �����ӿ� ���õ� ��ũ��Ʈ�� �����ϱ����� ����
    private CameraMovement cameraMovementScript;
    private PlayerMovement playerMovementScript;
    private ElementController elementController;
    private GameObject canvas;
    private GameObject aim;
    private Animator aimAnimator;

    private Transform cameraTransform;

    private float aimTime;
    private bool isWeak;
    private bool isEnergy;
    private bool isDie;
    GameObject[] weakPointArr;

    void Start()
    {
        healthSystem = GetComponent<PlayerHealthSystem>();
        energySystem = GetComponent<PlayerEnergySystem>();

        // UnActiveRealBow();
        isEquipWeapon = false;
        isSwitching = false;
        isEnergy = false;
        isDie = false;
        _animator = GetComponent<Animator>();

        backBow = hip.Find("BackBow").gameObject;
        realBow = leftWristJoint.Find("RealBow").gameObject;
        backSword = hip.Find("BackSword").gameObject;
        realSword = rightWristJoint.Find("RealSword").gameObject;

        arrowPoint = realBow.transform.Find("ArrowPoint");

        cameraTransform = GameObject.Find("Camera").transform;
        cameraMovementScript = GameObject.Find("Camera").GetComponent<CameraMovement>();
        playerMovementScript = GameObject.Find("Robot Kyle").GetComponent<PlayerMovement>();
        elementController = GetComponent<ElementController>();
        canvas = GameObject.Find("Canvas");
        aim = canvas.transform.Find("Aim").gameObject;
        aimAnimator = aim.GetComponent<Animator>();

        aimTime = 0f;
        isWeak = false;
        weakPointArr = null;

        UnActiveBow();
        UnActiveSword();
    }

    // Update is called once per frame
    void Update()
    {
        InputEquipWeapons();
        InputAttackWeapons();
        ObserveDie();
    }

    private void ObserveDie()
    {
        if (this.GetComponent<PlayerHealthSystem>().hitPoint <= 0 && !isDie)
        {
            isDie = true;
            // GameManager.Instance.SetTimeScale(0.5f);
            _animator.SetTrigger("DieTrigger");
            StartCoroutine(LoadGameoverScene());
        }
    }

    IEnumerator LoadGameoverScene()
    {
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene("Gameover");
    }

    private void InputAttackWeapons()
    {
        if (currentEquipWeapon.Equals(WEAPON.BOW))
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (energySystem.hitPoint > 25f)
                {
                    isEnergy = true;
                    // �� �ð� 0���� �ʱ�ȭ
                    aimTime = 0f;

                    // 1��Ī ���� ��ȯ
                    cameraMovementScript.curView = CameraMovement.VIEW.ONE;
                    cameraMovementScript.TransCamersView();
                    playerMovementScript.isAim = true;

                    _animator.SetBool("IsAimed", playerMovementScript.isAim);

                    // Aim ����
                    ActiveAim();
                    aimAnimator.SetTrigger("UnScaleAim");

                    _animator.SetTrigger("DrawArrowTrigger");
                }
                else
                {
                    isEnergy = false;
                    StartCoroutine(GameManager.Instance.BlinkWarningPanel());
                }
            }

            if (Input.GetButton("Attack"))
            {
                if (isEnergy)
                {// ���� ���� �Ӽ��϶��� ������ ǥ�õǾ�� �Ѵ�.
                    if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING))
                    {
                        aimTime += Time.unscaledDeltaTime;

                        if (aimTime > 2f)
                        {
                            GameManager.Instance.SetTimeScale(0.3f);
                        }

                        if (aimTime > 3f && !isWeak)
                        {
                            isWeak = true;
                            weakPointArr = GameObject.FindGameObjectsWithTag("WeakPoint");
                            foreach (var o in weakPointArr)
                            {
                                if (o != null)
                                    o.GetComponent<ParticleSystem>().Play();
                            }
                        }
                    }
                }
            }

            if (Input.GetButtonUp("Attack"))
            {
                GameManager.Instance.SetTimeScale(1f);

                if (isEnergy)
                {
                    if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING) && weakPointArr != null)
                    {
                        isWeak = false;
                        foreach (var o in weakPointArr)
                        {
                            if (o != null)
                                o.GetComponent<ParticleSystem>().Stop();
                        }
                    }

                    // 1��Ī ���� ��ȯ
                    cameraMovementScript.curView = CameraMovement.VIEW.THIRD;
                    cameraMovementScript.TransCamersView();
                    playerMovementScript.isAim = false;

                    _animator.SetBool("IsAimed", playerMovementScript.isAim);
                    _animator.SetTrigger("AimRecoilTrigger");

                    // Vector3 localToWorldPosition = transform.TransformPoint(fireArrow.transform.position);
                    GameObject clone;
                    //  TODO �̺κ� �����ؾ���. ȭ�� ������ ����
                    if(GetCurrentElementArrowName().Equals("ArrowMissileRed"))
                    {
                        clone = ObjectPooler.SpawnFromPool("FireArrow", arrowPoint.transform.position, cameraTransform.rotation);
                    } else {
                        clone = Instantiate(GetCurrentArrow(), GetCurrentArrow().transform.position, cameraTransform.rotation);
                    }

                     
                    // PoolableObject clone = ObjectPoolManager.GetInstance().arrowPool.PopObject();


                    Vector3 localScale = clone.transform.localScale;
                    clone.transform.localScale = new Vector3(localScale.x * 9, localScale.y * 9, localScale.z * 9);

                    // Aim ����
                    UnActiveAim();
                    UnActiveBowEffect();

                    energySystem.TakeDamage(25f);
                }
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

    // Į ���� �޺� ���� �Լ�
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

    private void ActiveBow()
    {
        Renderer backBowRenderer = backBow.transform.GetChild(0).GetComponent<Renderer>();
        Renderer realBowRenderer = realBow.transform.GetChild(0).GetComponent<Renderer>();

        backBowRenderer.enabled = false;
        realBowRenderer.enabled = true;
    }
    private void UnActiveBow()
    {
        Renderer backBowRenderer = backBow.transform.GetChild(0).GetComponent<Renderer>();
        Renderer realBowRenderer = realBow.transform.GetChild(0).GetComponent<Renderer>();

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

            // ��ƼŬ ���� ������Ʈ���� ����
            if (name.Contains("Particle"))
            {
                if (name.Equals(element))
                    ActiveSwordParticle(name);
                else
                    UnActiveSwordParticle(name);
            }

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
            // ��ƼŬ ���� ������Ʈ���� ����
            if (name.Contains("Particle"))
                UnActiveSwordParticle(name);
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

    private void ActiveSwordParticle(string particleName)
    {
        Transform particle = realSword.transform.Find(particleName);
        particle.gameObject.SetActive(true);
        for (int i = 0; i < particle.childCount; i++)
        {
            particle.GetChild(i).gameObject.SetActive(true);
            particle.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
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
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING))
            result = "LightningParticle";
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.ICE))
            result = "IceParticle";

        return result;
    }

    // ���� ���õ� ���ҿ� ���� ȭ���̸� ��ȯȯ
    private string GetCurrentElementArrowName()
    {
        string result = "";
        if (elementController.currentElement.Equals(ElementController.ELEMENT.FIRE))
            result = "ArrowMissileRed";
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING))
            result = "ArrowMissilePurple";
        else if (elementController.currentElement.Equals(ElementController.ELEMENT.ICE))
            result = "ArrowMissileBlue";

        return result;
    }

    private GameObject GetCurrentArrow()
    {
        string name = GetCurrentElementArrowName();
        for (int i = 0; i < realBow.transform.childCount; i++)
        {
            var bowChild = realBow.transform.GetChild(i).gameObject;
            if (bowChild.name.Equals(name))
            {
                return bowChild;
            }
        }
        return null;
    }


    private void ActiveAim()
    {
        aim.SetActive(true);
    }

    private void UnActiveAim()
    {
        aim.SetActive(false);
    }

    private void ActiveBowEffect()
    {
        var bowEffect = realBow.transform.Find("Effect");

        for (int i = 0; i < bowEffect.childCount; i++)
        {
            var effect = bowEffect.GetChild(i);

            if (elementController.currentElement.Equals(ElementController.ELEMENT.FIRE))
            {
                if (i == (int)ElementController.ELEMENT.FIRE)
                {
                    effect.gameObject.SetActive(true);
                    effect.gameObject.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    effect.gameObject.SetActive(false);
                    effect.gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
            else if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING))
            {
                if (i == (int)ElementController.ELEMENT.LIGHTNING)
                {
                    effect.gameObject.SetActive(true);
                    effect.gameObject.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    effect.gameObject.SetActive(false);
                    effect.gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
            else if (elementController.currentElement.Equals(ElementController.ELEMENT.ICE))
            {
                if (i == (int)ElementController.ELEMENT.ICE)
                {
                    effect.gameObject.SetActive(true);
                    effect.gameObject.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    effect.gameObject.SetActive(false);
                    effect.gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }

        }
    }

    private void UnActiveBowEffect()
    {
        var bowEffect = realBow.transform.Find("Effect");

        for (int i = 0; i < bowEffect.childCount; i++)
        {
            var effect = bowEffect.GetChild(i);

            effect.gameObject.SetActive(false);
            effect.gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterAttack"))
        {
            StartCoroutine(GameManager.Instance.BlinkWarningHit());

            healthSystem.TakeDamage(30);
        }
    }
}
