using UnityEngine;

public class ZombieSpawnTrigger : MonoBehaviour
{
    [Header("Player 설정")]
    public string playerTag = "Player";   // 플레이어 오브젝트의 Tag

    [Header("등장시킬 좀비들 (5마리)")]
    public GameObject[] zombies;          // 미리 깔아둔 좀비 아바타들 (비활성화 상태 권장)

    [Header("재생할 애니메이션 상태 이름")]
    public string animationStateName = "ZombieWalk";   // Animator에 있는 상태 이름

    private bool triggered = false;


    private void OnTriggerEnter(Collider other)
    {
        //if (triggered) return;                       // 한 번만 실행
        //if (!other.CompareTag(playerTag)) return;    // Player가 아닐 때 무시

        triggered = true;

        foreach (GameObject zombie in zombies)
        {
            if (zombie == null) continue;

            // 비활성화 상태라면 먼저 켜기
            if (!zombie.activeSelf)
                zombie.SetActive(true);

            // Animator 찾아서 애니메이션 재생
            Animator anim = zombie.GetComponent<Animator>();
            if (anim != null && !string.IsNullOrEmpty(animationStateName))
            {
                anim.Play(animationStateName, 0, 0f);
            }
        }
    }
}
