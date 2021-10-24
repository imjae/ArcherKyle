using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{

    Rigidbody rigid;
    Collider col;

    ArrowMovement arrowMovement;

    [SerializeField]
    private GameObject arrowExplosion;
    [SerializeField]
    private GameObject iceFieldEffect;

    // Arrow 클래스 상속받는 각 화살의 공격력을 가져오기 위한 변수
    private Arrow arrowScript;
    private float attackPoint;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;
    private ElementController elementController;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        arrowMovement = GetComponent<ArrowMovement>();
        arrowScript = GetComponent<Arrow>();
        elementController = GameObject.Find("Robot Kyle").GetComponent<ElementController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.CompareTag("Player"))
        {
            Debug.Log(name + " -> " + other.name);

            arrowMovement.isMovement = false;
            rigid.velocity = Vector3.zero;
            // Rigid,Collider 파괴
            Destroy(rigid);
            Destroy(col);
            // 맞은 대상 자식 오브젝트로 변경
            transform.SetParent(other.transform);

            // 공통적으로 화살이 터지는 이펙트는 존재한다.
            Instantiate(arrowExplosion, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Quaternion.identity);

            // 얼음, 불은 속성 범위공격
            // 번개 속성만 치명타, 기본공격 느낌
            if (elementController.currentElement.Equals(ElementController.ELEMENT.FIRE))
            {
                if (other.CompareTag("MonsterCriticalZone"))
                    MonsterCollisionBasicAction(GetRootObject(other.transform));
                if (other.CompareTag("Monster"))
                    MonsterCollisionBasicAction(other.gameObject);
            }
            else if (elementController.currentElement.Equals(ElementController.ELEMENT.LIGHTNING))
            {
                // Debug.Log("전기! => " + other.tag);

                if (other.CompareTag("MonsterCriticalZone"))
                {
                    MonsterCollisionBasicAction(GetRootObject(other.transform));

                    Monster monster = GetRootObject(other.transform).GetComponent<Monster>();
                    HealthSystem health = monster.gameObject.GetComponent<HealthSystem>();
                    // Debug.Log("크리티컬!! : " + health.maxHitPoint);
                    health.TakeDamage(health.maxHitPoint);

                }
                if (other.CompareTag("Monster"))
                {
                    MonsterCollisionBasicAction(other.gameObject);

                    // Debug.Log("몸통! : " + arrowScript.attackPoint);
                    GameObject monster = other.gameObject;
                    HealthSystem health = monster.GetComponent<HealthSystem>();
                    health.TakeDamage(arrowScript.attackPoint);
                }
            }
            else if (elementController.currentElement.Equals(ElementController.ELEMENT.ICE))
            {
                if (other.CompareTag("MonsterCriticalZone"))
                    MonsterCollisionBasicAction(GetRootObject(other.transform));
                if (other.CompareTag("Monster"))
                    MonsterCollisionBasicAction(other.gameObject);

                if (other.CompareTag("Monster") || other.CompareTag("MonsterCriticalZone"))
                {
                    Vector3 genePoint = other.transform.position;
                    // 크리티컬존에 맞았을 때 실제 몬스터의 아래에 생성
                    if (other.CompareTag("MonsterCriticalZone"))
                        genePoint = GetRootObject(other.transform).transform.position;

                    Instantiate(iceFieldEffect, genePoint, Quaternion.Euler(-90f, 0f, 90f));
                }

            }
        }
    }

    private void MonsterCollisionBasicAction(GameObject other)
    {
        monsterStatusController.ActiveMonsterStatus();

        // 몬스터 카메라 전부 OFF
        CameraManagement.Camera.AllFaceCameraOff();
        other.transform.Find("FaceCamera").GetComponent<Camera>().enabled = true;

        Monster monster = other.GetComponent<Monster>();
        RefreshMonsterStatus(monster);
    }

    private void RefreshMonsterStatus(Monster monster)
    {
        monsterStatusController.MonsterName = monster.MonsterName;
        monsterStatusController.MonsterHealth = $"{monster.Health.maxHitPoint}";
        monsterStatusController.MonsterAttackPoint = $"{monster.AttackValue}";
        monsterStatusController.MonsterRange = $"{monster.AttackRange}";
        monsterStatusController.MonsterWeak = monster.MonsterWeak;
    }

    private GameObject GetRootObject(Transform transform)
    {
        if (transform.name.Equals("root"))
        {
            Debug.Log(transform.parent.name);
            return transform.parent.gameObject;
        }
        else
        {
            return GetRootObject(transform.parent);
        }
    }
}
