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

    // Ȱ ���� ��������
    public bool isAim;
    // �� ���� ��������
    public bool isSword;
    // ������ �� �ִ� ��������
    public bool isMovement;

    public float smoothness = 10f;

    // Ȱ�� ���ʹ� ��� ����
    public Transform leftSholderJoint;

    // 2������ �޺�
    public bool isGrounded = false;
    public int jumpCount = 2; //����Ƚ��    2�� 3���� �ٲٸ� 3�� ����



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
        IsGround();

        // Alt�� �ѷ����� Ȱ��/��Ȱ��ȭ
        toggleCameraRotation = (Input.GetKey(KeyCode.LeftAlt)) ? true : false;

        if (isAim)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(0, 1, 0));
            leftSholderJoint.localEulerAngles = new Vector3(0, _camera.transform.parent.rotation.eulerAngles.x * 1f, 0);
        }
    }

    private void LateUpdate()
    {
        InputDash();
        InputJump();
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


    void InputJump()
    {
        if (jumpCount > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    _animator.SetTrigger("OnJumpTrigger");
                    _rigidbody.AddForce(Vector3.up * 200f);
                }
                else if (!isGrounded)
                {
                    _animator.SetTrigger("FlipJumpTrigger");
                    _rigidbody.AddForce(Vector3.up * 250f);
                }
                jumpCount--;
                // Debug.Log(isGround);
            }
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

    float DoubleClickSecond_forward = 0.25f;
    bool IsOneClick_forward = false;
    double Timer_forward = 0;

    float DoubleClickSecond_left = 0.25f;
    bool IsOneClick_left = false;
    double Timer_left = 0;

    float DoubleClickSecond_back = 0.25f;
    bool IsOneClick_back = false;
    double Timer_back = 0;

    float DoubleClickSecond_right = 0.25f;
    bool IsOneClick_right = false;
    double Timer_right = 0;

    void InputDash()
    {

        if (IsOneClick_forward && ((Time.time - Timer_forward) > DoubleClickSecond_forward))
        {
            IsOneClick_forward = false;
        }
        if (IsOneClick_left && ((Time.time - Timer_left) > DoubleClickSecond_left))
        {
            IsOneClick_left = false;
        }
        if (IsOneClick_right && ((Time.time - Timer_right) > DoubleClickSecond_right))
        {
            IsOneClick_right = false;
        }
        if (IsOneClick_back && ((Time.time - Timer_back) > DoubleClickSecond_back))
        {
            IsOneClick_back = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!IsOneClick_forward) { Timer_forward = Time.time; IsOneClick_forward = true; }
            else if (IsOneClick_forward && ((Time.time - Timer_forward) < DoubleClickSecond_forward))
            {
                IsOneClick_forward = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Forward");
                GetDashEffect("Forward").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.forward * 2500f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (!IsOneClick_left) { Timer_left = Time.time; IsOneClick_left = true; }
            else if (IsOneClick_left && ((Time.time - Timer_left) < DoubleClickSecond_left))
            {
                IsOneClick_left = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Left");
                GetDashEffect("Left").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.right * -2500f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!IsOneClick_back) { Timer_back = Time.time; IsOneClick_back = true; }
            else if (IsOneClick_back && ((Time.time - Timer_back) < DoubleClickSecond_back))
            {
                IsOneClick_back = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Backward");
                GetDashEffect("Back").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.forward * -2500f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (!IsOneClick_right) { Timer_right = Time.time; IsOneClick_right = true; }
            else if (IsOneClick_right && ((Time.time - Timer_right) < DoubleClickSecond_right))
            {
                IsOneClick_right = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                _animator.Play("Standing Dodge Right");
                GetDashEffect("Right").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.right * 2500f);
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
        bool result = Physics.Raycast(transform.position, Vector3.down, 0.1f);
        isGrounded = result;
        if (isGrounded)
            jumpCount = 2;
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
