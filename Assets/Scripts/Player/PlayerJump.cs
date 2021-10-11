using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float speed;      // 캐릭터 움직임 스피드.
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.

    private Vector3 MoveDir;

    CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 캐릭터가 땅에 있는가?
        if (_controller.isGrounded)
        {
            // 위, 아래 움직임 셋팅. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= speed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeed;

        }

        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        _controller.Move(MoveDir * Time.deltaTime);
    }
}