using UnityEngine;

public class TrainWay : MonoBehaviour
{
    [Header("이동할 로컬 포지션 두 개")]
    public Transform localPosA;
    public Transform localPosB;

    [Header("이동에 걸리는 시간(초 단위)")]
    public float duration = 60f;   // 1분 = 60초

    private float elapsed = 0f;
    private bool isMoving = true;

    void Start()
    {
        // 시작 위치를 A로 세팅
        transform.position = localPosA.position;
    }

    void Update()
    {
        if (!isMoving) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);   // 0 → 1

        // 두 점 사이를 등속(직선 보간)으로 이동
        transform.position = Vector3.Lerp(localPosA.position, localPosB.position, t);

        // 다 도달했으면 멈춤
        if (t >= 1f)
        {
            isMoving = false;
        }
    }
}
