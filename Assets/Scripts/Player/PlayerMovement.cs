using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    CharacterController _controller;

    public float walkSpeed = 3.5f;
    public float runSpeed = 8f;
    public float finalSpped;

    // alt 눌렀을때 둘러보기 기능
    public bool toggleCameraRotation;
    public bool run;

    public float smoothness = 10f;
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            // 둘러보기 활성화
            toggleCameraRotation = true;
        }
        else
        {
            // 둘러보기 비활성화
            toggleCameraRotation = false;
        }
    }

    private void LateUpdate()
    {
        if (!toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        run = (Input.GetKey(KeyCode.LeftShift)) ? true : false;

        InputMovement();
    }

    void InputMovement()
    {
        finalSpped = (run) ? runSpeed : walkSpeed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _controller.Move(moveDirection.normalized * finalSpped * Time.deltaTime);

        float percent = ((run) ? 1f : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
