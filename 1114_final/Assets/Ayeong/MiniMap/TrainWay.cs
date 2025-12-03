using UnityEngine;

public class TrainWay : MonoBehaviour
{
    [Header("이동할 로컬 포지션 두 개")]
    public Transform localPosA;
    public Transform localPosB;

    [Header("이동에 걸리는 총 시간(초) — Inspector에서 설정 가능")]
    public float duration = 60f;

    [Header("도착 후 띄울 Canvas (선택)")]
    public GameObject showCanvas;

    private float elapsed = 0f;
    private bool isMoving = true;
    private bool finished = false;

    [HideInInspector]
    public bool ignoreCanvasAndPause = false;

    void Start()
    {
        // 시작 위치 세팅
        if (localPosA != null)
            transform.position = localPosA.position;

        // Canvas 비활성화
        if (showCanvas != null)
            showCanvas.SetActive(false);

        elapsed = 0f;
        isMoving = true;
        finished = false;
        ignoreCanvasAndPause = false;
    }

    void Update()
    {
        if (!isMoving) return;

        // Inspector에서 설정한 duration 만큼만 이동
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        if (localPosA != null && localPosB != null)
            transform.position = Vector3.Lerp(localPosA.position, localPosB.position, t);

        // 목적지 도착 시
        if (t >= 1f && !finished)
        {
            finished = true;
            isMoving = false;

            if (!ignoreCanvasAndPause)
            {
                if (showCanvas != null)
                    showCanvas.SetActive(true);

                Debug.Log("[TrainWay] 도착 → Canvas 표시");
            }
            else
            {
                Debug.Log("[TrainWay] 도착 → Canvas/정지 생략");
            }
        }
    }
}
