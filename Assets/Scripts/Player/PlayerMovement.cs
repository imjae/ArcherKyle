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
    // 땅에 닿아있는지
    private bool isGround;
    // 활 에임 상태인지
    public bool isAim;
    // 움직일 수 있는 상태인지
    public bool isMovement;

    public float smoothness = 10f;

    // 활든 왼쪽발 어깨 관절
    public Transform leftSholderJoint;
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _rigidbody = this.GetComponent<Rigidbody>();
        _collider = this.GetComponent<Collider>();

        isAim = false;
        isMovement = true;
    }

    void Update()
    {
        // Alt로 둘러보기 활성/비활성화
        toggleCameraRotation = (Input.GetKey(KeyCode.LeftAlt)) ? true : false;
        IsGround();
        if (isAim)
        {
            rotX += -(Input.GetAxis("Mouse Y")) * 100f * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, -23, 23);

            Debug.Log(rotX);
            // Quaternion rot = Quaternion.Euler(0, rotX, 0);
            // Debug.Log(rot);
            leftSholderJoint.localEulerAngles = new Vector3(0, rotX, 0);

            // Vector3 armRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            // // leftSholderJoint.localRotation = Quaternion.Slerp(leftSholderJoint.localRotation, Quaternion.LookRotation(armRotate), Time.deltaTime * smoothness);
            // Debug.Log($"{leftSholderJoint.localRotation} / {Quaternion.LookRotation(armRotate)}");
            // leftSholderJoint.localRotation = Quaternion.Euler(_camera.transform.);
        }
    }

    private void LateUpdate()
    {
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
        finalSpeed = (isAim) ? finalSpeed / 2 : finalSpeed;

        _animator.SetFloat("MovementBlend", percent, 0.1f, Time.deltaTime);

        _rigidbody.MovePosition(transform.position + moveDirection.normalized * finalSpeed * Time.deltaTime);

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

    void IsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.05f, Color.red);
        isGround = Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }



    public void PossibleMovement()
    {
        this.isMovement = true;
    }

    public void ImpossibleMovement()
    {
        this.isMovement = false;
    }
}
