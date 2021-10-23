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

    // Arrow Ŭ���� ��ӹ޴� �� ȭ���� ���ݷ��� �������� ���� ����
    private Arrow arrowScript;
    private float attackPoint;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

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
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            arrowMovement.isMovement = false;
            rigid.velocity = Vector3.zero;

            Destroy(rigid);
            Destroy(col);

            transform.SetParent(other.transform);

            Instantiate(arrowExplosion, transform.position, Quaternion.identity);

            if (other.CompareTag("Monster"))
            {
                monsterStatusController.ActiveMonsterStatus();

                // ���� ī�޶� ���� OFF
                CameraManagement.Camera.AllFaceCameraOff();
                other.transform.Find("FaceCamera").GetComponent<Camera>().enabled = true;

                Monster monster = other.GetComponent<Monster>();
                monsterStatusController.MonsterName = monster.MonsterName;
                monsterStatusController.MonsterHealth = $"{monster.Health.maxHitPoint}";
                monsterStatusController.MonsterAttackPoint = $"{monster.AttackValue}";
                monsterStatusController.MonsterRange = $"{monster.AttackRange}";
                monsterStatusController.MonsterWeak = monster.MonsterWeak;
            }

            if (other.CompareTag("MonsterCriticalZone"))
            {
                Monster monster = GetRootObject(other.transform).GetComponent<Monster>();
                HealthSystem health = monster.gameObject.GetComponent<HealthSystem>();
                Debug.Log("ũ��Ƽ��!! : " + health.maxHitPoint);
                health.TakeDamage(health.maxHitPoint);
            }
            else if (other.CompareTag("Monster"))
            {
                Debug.Log("����! : " + arrowScript.attackPoint);
                GameObject monster = other.gameObject;
                HealthSystem health = monster.GetComponent<HealthSystem>();
                health.TakeDamage(arrowScript.attackPoint);
            }
        }
    }

    private GameObject GetRootObject(Transform transform)
    {
        Debug.Log(transform.name);
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
