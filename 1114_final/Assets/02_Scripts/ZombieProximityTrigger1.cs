using UnityEngine;

public class ZombieProximityTrigger1 : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public GameObject zombieObject;
    public string animatorParamName = "isChasing";

    [Header("설정")]
    public float moveSpeed = 2.0f;

    [Header("사운드 클립 설정 (AudioClip만 넣으면 됨)")]
    public AudioSource audioSource;    // 스피커 1개
    public AudioClip soundChase;       // A
    public AudioClip soundBitten;      // B
    public AudioClip soundZombieDie;   // C

    private Animator zombieAnim;
    private BoxCollider boxCollider;
    private Transform playerTarget;

    private bool PlayerEntered = false;
    private bool isChasing = false;
    private bool triggered = false;

    void Start()
    {
        if (zombieObject != null)
        {
            zombieAnim = zombieObject.GetComponent<Animator>();
            boxCollider = zombieObject.GetComponent<BoxCollider>();
        }

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

        if (!PlayerEntered)
            return;

        // A - 추격 시작
        if (!triggered)
        {
            triggered = true;
            isChasing = true;
            playerTarget = player;

            if (zombieAnim != null)
                zombieAnim.SetBool(animatorParamName, true);

            Debug.Log("[ZombieProximityTrigger] 추격 시작");

            Play(soundChase);

            if (!zombieObject.activeSelf)
            {
                zombieObject.SetActive(true);
                if (zombieAnim != null)
                    zombieAnim.CrossFade("Zombie Walk", 0.15f);
            }
        }

        // 추격 이동
        if (isChasing && playerTarget != null)
        {
            if (boxCollider != null && !boxCollider.enabled)
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

        // B - 플레이어가 물렸을 때
        if (WeaponKeyboardAim.PlayerBitten)
        {
            Debug.Log("[ZombieProximityTrigger] PlayerBitten 블록 진입");

            WeaponKeyboardAim.PlayerBitten = false;
            isChasing = false;

            if (zombieAnim != null)
            {
                zombieAnim.SetBool(animatorParamName, false);
                zombieAnim.CrossFade("ZombieStanding", 0.15f);
            }

            Debug.Log("[ZombieProximityTrigger] Bitten 사운드 재생 시도");
            Play(soundBitten);

            Destroy(zombieObject, 5f);
            Destroy(this.gameObject, 5f);
        }

        // C - 좀비가 죽었을 때
        if (WeaponKeyboardAim.PlayerKilledZombie)
        {
            if (WeaponKeyboardAim.ZombieName != zombieObject.name)
                return;

            isChasing = false;
            playerTarget = null;

            if (boxCollider != null)
                boxCollider.enabled = false;

            if (zombieAnim != null)
            {
                zombieAnim.SetBool(animatorParamName, false);
                zombieAnim.CrossFade("ZombieDying", 0.15f);
            }

            Debug.Log("[ZombieProximityTrigger] ZombieDie 사운드 재생 시도");
            Play(soundZombieDie);

            Destroy(zombieObject, 5f);
            Destroy(this.gameObject, 5f);

            WeaponKeyboardAim.PlayerKilledZombie = false;
        }
    }

    private void Play(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEntered = true;
        }
    }
}
