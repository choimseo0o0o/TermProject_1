using UnityEngine;
using static PublicControllerValue;

public class PlaneTrigger : MonoBehaviour
{
    [Header("화면 정중앙에 띄울 Plane")]
    public GameObject planeObject;     // 3D Plane (Inspector에서 드래그)
    public float planeDistance = 2f;   // 카메라 앞 거리

    private bool triggered = false;
    private Quaternion initialLocalRotation;   // Plane의 회전값 기억용

    void Start()
    {
        if (planeObject != null)
        {
            // 처음 Plane의 로컬 회전값을 기억해 둔다 (요청한 대로 회전 유지)
            initialLocalRotation = planeObject.transform.localRotation;
            // 시작할 때는 안 보이게
            planeObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player만 반응 + 한 번만 실행
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;
        AttachPlaneToCamera();
    }

    void AttachPlaneToCamera()
    {
        if (planeObject == null)
        {
            Debug.LogWarning("[PlaneTriggerFPS] planeObject가 인스펙터에 지정되지 않았음");
            return;
        }

        // 메인 카메라 찾기
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("[PlaneTriggerFPS] Main Camera를 찾을 수 없음");
            return;
        }

        planeObject.SetActive(true);
        IsOkayToPressContB = true;

        // ★ Plane을 카메라의 자식으로 붙인다 → FPS 화면에 고정
        planeObject.transform.SetParent(cam.transform, worldPositionStays: false);

        // 카메라 앞 정중앙으로 이동 (Z축 방향으로 planeDistance)
        planeObject.transform.localPosition = new Vector3(0f, 0f, planeDistance);

        // 회전은 Plane이 처음 가지고 있던 로컬 회전값 그대로 사용
        planeObject.transform.localRotation = initialLocalRotation;

        Debug.Log("[PlaneTriggerFPS] Player 트리거 진입 → Plane를 카메라 정중앙에 부착");
    }
}
