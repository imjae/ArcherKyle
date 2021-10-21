using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    Rigidbody _rigidbody;
    Collider _collider;

    // �ȱ� ��� ����
    public float walkSpeed;
    public float runSpeed;
    public float backSpeed;
    private float finalSpeed;

    // alt �������� �ѷ����� ���
    public bool toggleCameraRotation;
    public bool run;
    // ���� ����ִ���
    private bool isGround;
    // Ȱ ���� ��������
    public bool isAim;
    // �� ���� ��������
    public bool isSword;
    // ������ �� �ִ� ��������
    public bool isMovement;

    public float smoothness = 10f;

    // Ȱ�� ���ʹ� ��� ����
    public Transform leftSholderJoint;

    // ���� ��Ʈ�ѷ�
    private ElementController elementController;
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _rigidbody = this.GetComponent<Rigidbody>();
        _collider = this.GetComponent<Collider>();

        isAim = false;
        isMovement = true;
        isSword = false;

        elementController = this.GetComponent<ElementController>();
    }

    void Update()
    {
        // Alt�� �ѷ����� Ȱ��/��Ȱ��ȭ
        toggleCameraRotation = (Input.GetKey(KeyCode.LeftAlt)) ? true : false;
        IsGround();

        if (isAim)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(0, 1, 0));
            leftSholderJoint.localEulerAngles = new Vector3(0, _camera.transform.parent.rotation.eulerAngles.x * 1f, 0);
        }
    }

    private void LateUpdate()
    {
        InputJump();
        InputDash();
    }

    // ���콺 ��ǲ�� ���� ����
    private float rotX;
    private float rotY;

    private void FixedUpdate()
    {
        if (!toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            // Debug.Log($"{_camera.transform.forward} / {new Vector3(1, 0, 1)} / { Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1))}");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }


        // run = (Input.GetKey(KeyCode.LeftShift)) ? true : false;

        if (isMovement)
        {
            InputMovement();
        }
    }

    void InputMovement()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection =
                    forward * Input.GetAxisRaw("Vertical") +
                    right * Input.GetAxisRaw("Horizontal");

        float percent;

        // float percent = ((run) ? 1f : 0.5f) * moveDirection.magnitude;
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            percent = -1f * moveDirection.magnitude;
            finalSpeed = backSpeed;
        }
        else
        {
            percent = 1f * moveDirection.magnitude;
            finalSpeed = runSpeed;
        }

        // finalSpeed�� ���ӻ����̸� �������� �پ��.
        if (isAim)
            finalSpeed = finalSpeed / 2;
        else if (isSword)
            finalSpeed = finalSpeed / 4;

        _animator.SetFloat("MovementBlend", percent, 0.1f, Time.deltaTime);

        _rigidbody.AddForce(moveDirection.normalized * 200f);
        // _rigidbody.velocity = moveDirection.normalized * finalSpeed;

    }

    void InputJump()
    {
        if (Input.GetButtonDown("Jump") && this.isGround)
        {
            _animator.SetTrigger("OnJumpTrigger");
            _rigidbody.AddForce(Vector3.up * 200f);
            // Debug.Log(isGround);
        }
    }

    float m_DoubleClickSecond_forward = 0.25f;
    bool m_IsOneClick_forward = false;
    double m_Timer_forward = 0;

    float m_DoubleClickSecond_left = 0.25f;
    bool m_IsOneClick_left = false;
    double m_Timer_left = 0;

    float m_DoubleClickSecond_back = 0.25f;
    bool m_IsOneClick_back = false;
    double m_Timer_back = 0;

    float m_DoubleClickSecond_right = 0.25f;
    bool m_IsOneClick_right = false;
    double m_Timer_right = 0;

    void InputDash()
    {

        if (m_IsOneClick_forward && ((Time.time - m_Timer_forward) > m_DoubleClickSecond_forward))
        {
            m_IsOneClick_forward = false;
        }
        if (m_IsOneClick_left && ((Time.time - m_Timer_left) > m_DoubleClickSecond_left))
        {
            m_IsOneClick_left = false;
        }
        if (m_IsOneClick_right && ((Time.time - m_Timer_right) > m_DoubleClickSecond_right))
        {
            m_IsOneClick_right = false;
        }
        if (m_IsOneClick_back && ((Time.time - m_Timer_back) > m_DoubleClickSecond_back))
        {
            m_IsOneClick_back = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!m_IsOneClick_forward) { m_Timer_forward = Time.time; m_IsOneClick_forward = true; }
            else if (m_IsOneClick_forward && ((Time.time - m_Timer_forward) < m_DoubleClickSecond_forward))
            {
                m_IsOneClick_forward = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Forward");
                GetDashEffect("Forward").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.forward * 2000f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (!m_IsOneClick_left) { m_Timer_left = Time.time; m_IsOneClick_left = true; }
            else if (m_IsOneClick_left && ((Time.time - m_Timer_left) < m_DoubleClickSecond_left))
            {
                m_IsOneClick_left = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Left");
                GetDashEffect("Left").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.right * -2000f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!m_IsOneClick_back) { m_Timer_back = Time.time; m_IsOneClick_back = true; }
            else if (m_IsOneClick_back && ((Time.time - m_Timer_back) < m_DoubleClickSecond_back))
            {
                m_IsOneClick_back = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Backward");
                GetDashEffect("Back").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.forward * -2000f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (!m_IsOneClick_right) { m_Timer_right = Time.time; m_IsOneClick_right = true; }
            else if (m_IsOneClick_right && ((Time.time - m_Timer_right) < m_DoubleClickSecond_right))
            {
                m_IsOneClick_right = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Right");
                GetDashEffect("Right").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.right * 2000f);
            }

        }
    }

    // ���� ���õ� ���ҿ� �´� �뽬 ����Ʈ ��ȯ
    // ���ڷ� �뽬�� ������ �޴´�. (Foward, Back, Left, Right)
    private GameObject GetDashEffect(string direction)
    {
        GameObject result = null;
        Transform dashObject = transform.Find("Dash");
        Transform dashObjectOfDirection = dashObject.Find(direction);

        // 0 : FIRE
        // 1 : LIGHTNING
        // 2 : ICE
        int currentElementIndex = (int)elementController.currentElement;

        // �� �ּ��� Enum�� �ε����� �°� ����Ƽ�� Hierarchy�� ���ĵǾ� ����
        result = dashObjectOfDirection.GetChild(currentElementIndex).gameObject;

        return result;
    }


    void IsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.05f, Color.red);
        isGround = Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    public void SwordMovement()
    {
        this.isSword = true;
    }

    public void NotSwordMovement()
    {
        this.isSword = false;
    }
}
