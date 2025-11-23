using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f; // 이동 속도
    public float gravity = -9.81f; // 중력 값 (점프 등을 위해 필요)

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        // Player 오브젝트에 부착된 CharacterController 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!WeaponKeyboardAim.PlayerCanMove)
            return;
        // 1. 지면에 닿아 있는지 확인 및 중력 초기화
        // isGrounded는 CharacterController가 제공하는 정보입니다.
        if (controller.isGrounded && velocity.y < 0)
        {
            // 지면에 닿았을 때 미끄러짐 방지
            velocity.y = -2f;
        }

        // 2. 입력 받기 (WASD)
        // Unity의 기본 Input Manager 설정(Horizontal, Vertical)을 사용합니다.
        float x = Input.GetAxis("Horizontal"); // A(-1) 또는 D(1)
        float z = Input.GetAxis("Vertical");   // S(-1) 또는 W(1)

        // 3. 이동 방향 계산 (플레이어의 현재 방향을 기준으로)
        // transform.right: 플레이어의 오른쪽 방향
        // transform.forward: 플레이어의 앞쪽 방향
        Vector3 move = transform.right * x + transform.forward * z;

        // 4. CharacterController를 사용하여 이동 실행
        // Time.deltaTime을 곱하여 프레임 속도와 관계없이 일정한 속도를 유지합니다.
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 5. 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}