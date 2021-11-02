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

    // 무기 장착 여부
    private bool isEquipWeapon;
    // 무기 교체중 여부
    private bool isSwitching;
    public WEAPON currentEquipWeapon;

    // 무기를 찾을 무기의 Parent 객체
    public Transform hip;
    public Transform leftWristJoint;
    public Transform rightWristJoint;

    // 실제 무기와 등에 매고 있는 무기
    private GameObject backBow;
    private GameObject realBow;
    private GameObject backSword;
    private GameObject realSword;
    
    private Transform arrowPoint;

    // 칼공격 콤보
    private bool comboPossible;
    int comboStep;
    // 카메라와 플레이어 움직임에 관련된 스크립트에 접근하기위한 변수
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
                    // 줌 시간 0으로 초기화
                    aimTime = 0f;

                    // 1인칭 시점 변환
                    cameraMovementScript.curView = CameraMovement.VIEW.ONE;
                    cameraMovementScript.TransCamersView();
                    playerMovementScript.isAim = true;

                    _animator.SetBool("IsAimed", playerMovementScript.isAim);

                    // Aim 생성
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
                {// 보라 번개 속성일때만 약점이 표시되어야 한다.
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

                    // 1인칭 시점 변환
                    cameraMovementScript.curView = CameraMovement.VIEW.THIRD;
                    cameraMovementScript.TransCamersView();
                    playerMovementScript.isAim = false;

                    _animator.SetBool("IsAimed", playerMovementScript.isAim);
                    _animator.SetTrigger("AimRecoilTrigger");

                    // Vector3 localToWorldPosition = transform.TransformPoint(fireArrow.transform.position);
                    GameObject clone;
                    //  TODO 이부분 수정해야함. 화살 종류에 따라
                    if(GetCurrentElementArrowName().Equals("ArrowMissileRed"))
                    {
                        clone = ObjectPooler.SpawnFromPool("FireArrow", arrowPoint.transform.position, cameraTransform.rotation);
                    } else {
                        clone = Instantiate(GetCurrentArrow(), GetCurrentArrow().transform.position, cameraTransform.rotation);
                    }

                     
                    // PoolableObject clone = ObjectPoolManager.GetInstance().arrowPool.PopObject();


                    Vector3 localScale = clone.transform.localScale;
                    clone.transform.localScale = new Vector3(localScale.x * 9, localScale.y * 9, localScale.z * 9);

                    // Aim 제거
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
            // 무기 장착 중일때
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
        // 활
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isSwitching)
        {
            // 무기 장착 중일때
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

    // 칼 공격 콤보 관련 함수
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

    // RealSword를 보이게 하는 함수 (무기를 들었을 때)
    private void ActiveSword()
    {
        Renderer backSwordRenderer = backSword.GetComponent<Renderer>();
        Renderer realSwordRenderer = realSword.GetComponent<Renderer>();

        backSwordRenderer.enabled = false;
        realSwordRenderer.enabled = true;

        ActiveSwordEffect();
    }
    // RealSword를 감추는 함수 (무기를 해제 했을 때)
    private void UnActiveSword()
    {
        Renderer backSwordRenderer = backSword.GetComponent<Renderer>();
        Renderer realSwordRenderer = realSword.GetComponent<Renderer>();

        backSwordRenderer.enabled = true;
        realSwordRenderer.enabled = false;

        UnActiveSwordEffect();
    }

    // 실제로 선택된 원소의 이펙트를 켜고, 나머지는 끄는 함수
    public void ActiveSwordEffect()
    {
        Transform realSwordTransform = realSword.transform;
        string element = GetCurrentElementName();
        for (int i = 0; i < realSwordTransform.childCount; i++)
        {
            // 선택된 원소와 이름이 같은 오브젝트의 파티클을 켜고, 나머지는 끈다.
            string name = realSwordTransform.GetChild(i).gameObject.name;

            // 파티클 관련 오브젝트에만 적용
            if (name.Contains("Particle"))
            {
                if (name.Equals(element))
                    ActiveSwordParticle(name);
                else
                    UnActiveSwordParticle(name);
            }

        }
    }

    // 실제로 선택된 원소의 이펙트 모두 종료
    public void UnActiveSwordEffect()
    {
        Transform realSwordTransform = realSword.transform;
        string element = GetCurrentElementName();
        for (int i = 0; i < realSwordTransform.childCount; i++)
        {
            // 선택된 원소와 이름이 같은 오브젝트의 파티클을 켜고, 나머지는 끈다.
            string name = realSwordTransform.GetChild(i).gameObject.name;
            // 파티클 관련 오브젝트에만 적용
            if (name.Contains("Particle"))
                UnActiveSwordParticle(name);
        }
    }

    public void ActiveSwordFinalAttackEffect()
    {
        string name = GetCurrentElementName();
        Transform particle = realSword.transform.Find(name);

        // 마지막 자식위치에 있는 이펙트 Play 시켜준다.(위치 지켜야함)
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

    // 현재 선택된 원소 이름 반환
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

    // 현재 선택된 원소에 따른 화살이름 반환환
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
