using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.W)) PlayInputSound();
        if (Input.GetKeyDown(KeyCode.A)) PlayInputSound();
        if (Input.GetKeyDown(KeyCode.S)) PlayInputSound();
        if (Input.GetKeyDown(KeyCode.D)) PlayInputSound();

        // 방향키 (Arrow Keys)
        if (Input.GetKeyDown(KeyCode.UpArrow)) PlayInputSound();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PlayInputSound();
        if (Input.GetKeyDown(KeyCode.DownArrow)) PlayInputSound();
        if (Input.GetKeyDown(KeyCode.RightArrow)) PlayInputSound();
        // -------------------------------------------------- //

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
    }

    private void PlayInputSound()
    {
        if (inputAudioSource == null || inputSound == null)
            return;

        inputAudioSource.PlayOneShot(inputSound);
    }
}
