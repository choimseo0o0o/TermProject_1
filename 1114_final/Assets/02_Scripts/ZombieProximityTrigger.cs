using UnityEngine;

public class ZombieProximityTrigger : MonoBehaviour
{
    [Header("참조")]
    public Transform player;           // Player Transform
    public GameObject zombieObject;    // Animator 달린 좀비 오브젝트
    public string animatorParamName = "isChasing";

    [Header("설정")]
    public float triggerRadius = 5f;   // 이 반경 안에 들어오면 발동
    public float moveSpeed = 2.0f;     // 좀비 이동 속도

    private Animator zombieAnim;
    private Transform playerTarget;
    private bool isChasing = false;
    private bool triggered = false;

    void Start()
    {
        if (zombieObject != null)
            zombieAnim = zombieObject.GetComponent<Animator>();

        // 인스펙터에서 안 넣었다면 태그로 자동 찾기
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

        // 1. 아직 발동 안 했으면 거리 체크
        if (!triggered)
        {
            float sqrDist = (player.position - transform.position).sqrMagnitude;
            if (sqrDist <= triggerRadius * triggerRadius)
            {
                triggered = true;
                isChasing = true;
                playerTarget = player;

                if (zombieAnim != null)
                    zombieAnim.SetBool(animatorParamName, true);

                Debug.Log("[ZombieProximityTrigger] 추격 시작");
            }
        }

        // 2. 추격 중이면 좀비 이동
        if (isChasing && playerTarget != null)
        {
            Vector3 dir = playerTarget.position - zombieObject.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.01f) return;

            Quaternion targetRot = Quaternion.LookRotation(dir);
            zombieObject.transform.rotation =
                Quaternion.Slerp(zombieObject.transform.rotation, targetRot, Time.deltaTime * 10f);

            zombieObject.transform.position +=
                zombieObject.transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    // 씬 뷰에서 반경이 보이도록
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
