using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GameOverDisableTrigger : MonoBehaviour
{
    [Header("Inspector References")]
    [SerializeField] private CountdownTimer countdownTimer; // 타이머 연결
    [SerializeField] private string playerTag = "Player";   // 플레이어 태그

    [Header("Behavior")]
    [SerializeField] private bool disableGameOverOnEnter = true; // 들어오면 무효화
    [SerializeField] private bool oneShot = true;               // 1회만 발동

    private void Awake()
    {
        // 트리거로 강제 (인스펙터 실수 방지)
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (countdownTimer == null) return;

        if (disableGameOverOnEnter)
            countdownTimer.DisableGameOver(true);

        if (oneShot)
            gameObject.SetActive(false);
    }
}
