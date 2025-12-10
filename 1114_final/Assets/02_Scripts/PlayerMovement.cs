using UnityEngine;
using static PublicControllerValue;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    [Header("입력 사운드 설정")]
    public AudioSource inputAudioSource;
    public AudioClip inputSound;

    [Header("이동 제한 (월드 좌표 기준)")]
    public bool useLimit = true;   // 제한을 켜고/끄고 싶을 때
    public float minX = -5f;
    public float maxX = 5f;
    public float minZ = -5f;
    public float maxZ = 5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!WeaponKeyboardAim.PlayerCanMove)
            return;

        // ----------- 입력 사운드 (WASD + 방향키) ----------- //
        // WASD
        // if (Input.GetKeyDown(KeyCode.W)) PlayInputSound();
        // if (Input.GetKeyDown(KeyCode.A)) PlayInputSound();
        // if (Input.GetKeyDown(KeyCode.S)) PlayInputSound();
        // if (Input.GetKeyDown(KeyCode.D)) PlayInputSound();

        // // 방향키 (Arrow Keys)
        // if (Input.GetKeyDown(KeyCode.UpArrow)) PlayInputSound();
        // if (Input.GetKeyDown(KeyCode.LeftArrow)) PlayInputSound();
        // if (Input.GetKeyDown(KeyCode.DownArrow)) PlayInputSound();
        // if (Input.GetKeyDown(KeyCode.RightArrow)) PlayInputSound();
        // -------------------------------------------------- //

        if (PlayerIsMoving)
            PlayInputSound();

        // 지면 체크
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // 이동
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ------------ 위치 제한 ------------
        if (useLimit)
        {
            Vector3 pos = transform.position;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

            transform.position = pos;
        }
        // ----------------------------------
    }

    public void PlayInputSound()
    {
        if (inputAudioSource == null || inputSound == null)
            return;

        inputAudioSource.PlayOneShot(inputSound);
    }
}
