using UnityEngine;

public class TrainWay : MonoBehaviour
{
    [Header("이동할 로컬 포지션 두 개")]
    public Transform localPosA;
    public Transform localPosB;

    [Header("이동에 걸리는 시간(초 단위)")]
    public float duration = 60f;

    [Header("도착 후 띄울 Canvas (활성화할 대상)")]
    public GameObject showCanvas;   // Inspector에서 할당

    private float elapsed = 0f;
    private bool isMoving = true;
    private bool finished = false;

    void Start()
    {
        // 시작 위치 A로 이동
        transform.position = localPosA.position;

        // Canvas는 시작 시 꺼두기
        if (showCanvas != null)
            showCanvas.SetActive(false);
    }

    void Update()
    {
        if (!isMoving) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        transform.position = Vector3.Lerp(localPosA.position, localPosB.position, t);

        // 이동 완료
        if (t >= 1f && !finished)
        {
            finished = true;
            isMoving = false;

            // Canvas 활성화
            if (showCanvas != null)
                showCanvas.SetActive(true);

            // ★★ 전체 게임 정지 ★★
            Time.timeScale = 0f;

            Debug.Log("[TrainWay] 목적지 도착 → Canvas 활성화 + Scene 정지");
        }
    }
}
