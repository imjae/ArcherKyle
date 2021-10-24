using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    Rigidbody _rigidbody;
    Collider _collider;

    // 걷기 기능 삭제
    public float walkSpeed;
    public float runSpeed;
    public float backSpeed;
    private float finalSpeed;

    // alt 눌렀을때 둘러보기 기능
    public bool toggleCameraRotation;
    public bool run;

    // 활 에임 상태인지
    public bool isAim;
    // 검 공격 상태인지
    public bool isSword;
    // 움직일 수 있는 상태인지
    public bool isMovement;

    public float smoothness = 10f;

    // 활든 왼쪽발 어깨 관절
    public Transform leftSholderJoint;

    // 2단점프 콤보
    public bool isGrounded = false;
    public int jumpCount = 2; //점프횟수    2를 3으로 바꾸면 3단 점프



    // 원소 컨트롤러
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

        // Alt로 둘러보기 활성/비활성화
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

    // 마우스 인풋을 받을 변수
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

        // finalSpeed가 에임상태이면 절반으로 줄어듬.
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
                //아래에 더블클릭에서 처리하고싶은 이벤트 작성
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
                //아래에 더블클릭에서 처리하고싶은 이벤트 작성
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
                //아래에 더블클릭에서 처리하고싶은 이벤트 작성
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
                //아래에 더블클릭에서 처리하고싶은 이벤트 작성
                _animator.Play("Standing Dodge Right");
                GetDashEffect("Right").GetComponent<ParticleSystem>().Play();
                _rigidbody.AddForce(transform.right * 2500f);
            }

        }
    }

    // 현재 선택된 원소에 맞는 대쉬 이펙트 반환
    // 인자로 대쉬의 방향을 받는다. (Foward, Back, Left, Right)
    private GameObject GetDashEffect(string direction)
    {
        GameObject result = null;
        Transform dashObject = transform.Find("Dash");
        Transform dashObjectOfDirection = dashObject.Find(direction);

        // 0 : FIRE
        // 1 : LIGHTNING
        // 2 : ICE
        int currentElementIndex = (int)elementController.currentElement;

        // 위 주석의 Enum의 인덱스에 맞게 유니티의 Hierarchy에 정렬되어 있음
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
