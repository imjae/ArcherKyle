using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    Rigidbody _rigidbody;
    Collider _collider;

    public float walkSpeed = 3.5f;
    public float runSpeed = 8f;
    public float finalSpeed;

    // alt 눌렀을때 둘러보기 기능
    public bool toggleCameraRotation;
    public bool run;
    // 땅에 닿아있는지
    private bool isGround;

    public float smoothness = 10f;
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _rigidbody = this.GetComponent<Rigidbody>();
        _collider = this.GetComponent<Collider>();
    }

    void Update()
    {
        // Alt로 둘러보기 활성/비활성화
        toggleCameraRotation = (Input.GetKey(KeyCode.LeftAlt)) ? true : false;
        IsGround();
    }

    private void LateUpdate()
    {
        

    }

    private void FixedUpdate()
    {
        if (!toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            // Debug.Log($"{_camera.transform.forward} / {new Vector3(1, 0, 1)} / { Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1))}");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        run = (Input.GetKey(KeyCode.LeftShift)) ? true : false;

        InputMovement();
        InputJump();
    }

    void InputMovement()
    {
        finalSpeed = (run) ? runSpeed : walkSpeed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _rigidbody.MovePosition(transform.position + moveDirection.normalized * finalSpeed * Time.deltaTime);

        float percent = ((run) ? 1f : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    void InputJump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            _rigidbody.AddForce(Vector3.up * 280f);
            Debug.Log(isGround);
        }
    }

    void IsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.05f, Color.red);
        isGround = Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
