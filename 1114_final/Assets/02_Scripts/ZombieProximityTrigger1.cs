using UnityEngine;

public class ZombieProximityTrigger1 : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public GameObject zombieObject;
    public string animatorParamName = "isChasing";

    [Header("설정")]
    public float moveSpeed = 2.0f;

    private Animator zombieAnim;
    private BoxCollider boxCollider;
    private Transform playerTarget;

    private bool PlayerEntered = false;   // ★ 콜라이더로 진입 감지
    private bool isChasing = false;
    private bool triggered = false;

    void Start()
    {
        if (zombieObject != null)
            zombieAnim = zombieObject.GetComponent<Animator>();

        if (zombieObject != null)
            boxCollider = zombieObject.GetComponent<BoxCollider>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || zombieObject == null)
            return;

        // ★ PlayerEntered가 false면 아무 동작도 안 함
        if (!PlayerEntered)
            return;

        // 1. 추격 시작
        if (!triggered)
        {
            triggered = true;
            isChasing = true;
            playerTarget = player;

            if (zombieAnim != null)
                zombieAnim.SetBool(animatorParamName, true);

            Debug.Log("[ZombieProximityTrigger] 추격 시작");

            if (zombieObject.activeSelf == false)
            {
                zombieObject.SetActive(true);
                zombieAnim.CrossFade("Zombie Walk", 0.15f);
            }
        }

        // 2. 추격 이동
        if (isChasing && playerTarget != null)
        {
            if (boxCollider != null && boxCollider.enabled == false)
                boxCollider.enabled = true;

            Vector3 dir = playerTarget.position - zombieObject.transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude >= 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                zombieObject.transform.rotation =
                    Quaternion.Slerp(zombieObject.transform.rotation, targetRot, Time.deltaTime * 10f);

                zombieObject.transform.position +=
                    zombieObject.transform.forward * moveSpeed * Time.deltaTime;
            }
        }

        // 3. 플레이어가 좀비에게 물렸을 때
        if (WeaponKeyboardAim.PlayerBitten)
        {
            WeaponKeyboardAim.PlayerBitten = false;

            if (zombieAnim != null)
            {
                zombieAnim.SetBool(animatorParamName, false);
                zombieAnim.CrossFade("ZombieStanding", 0.15f);
            }

            Debug.Log("[ZombieProximityTrigger] 추격 중지 + Standing CrossFade");
            Destroy(this.gameObject, 5f);
            Destroy(zombieObject.gameObject);
        }

        // 4. Continue 버튼 눌렀을 때(현재 기능 비활성)
        if (WeaponKeyboardAim.ContinuePlay)
        {
            // 기능 주석 처리
        }

        // 5. 좀비가 플레이어에게 사망했을 때
        if (WeaponKeyboardAim.PlayerKilledZombie)
        {
            isChasing = false;
            playerTarget = null;

            if (boxCollider != null)
                boxCollider.enabled = false;

            WeaponKeyboardAim.PlayerKilledZombie = false;

            if (zombieAnim != null)
            {
                zombieAnim.SetBool(animatorParamName, false);
                zombieAnim.CrossFade("ZombieDying", 0.15f);
                Debug.Log("[ZombieProximityTrigger] ZombieDying 재생");
            }

            Destroy(zombieObject, 5f);
            Destroy(this.gameObject, 5f);
        }
    }

    // ★ Player가 이 Collider로 들어왔는지 체크
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEntered = true;
            Debug.Log("Player Entered Zombie Trigger");
        }
    }
}