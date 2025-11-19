using UnityEngine;

public class GunAim : MonoBehaviour
{
    [Header("참조")]
    public Transform muzzle;       // 총구 끝 (MuzzlePoint)
    public Transform aimCircle;    // 조준점(동그라미 스피어)

    [Header("설정")]
    public float maxDistance = 200f;
    public LayerMask hitMask = ~0; // 맞출 레이어 (기본: 전부)

    void Update()
    {
        if (muzzle == null || aimCircle == null)
            return;

        // 1. 총구 기준 레이
        Ray ray = new Ray(muzzle.position, muzzle.forward);

        // 디버그용: 씬에서 레이 확인
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        // 2. 레이캐스트로 목표 지점 찾기
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, maxDistance, hitMask))
        {
            targetPoint = hit.point;
            // 필요하면 무슨 오브젝트 맞았는지 로그로 확인
            // Debug.Log($"[GunAim] Hit: {hit.collider.name} @ {hit.point}");
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxDistance;
        }

        // 3. 조준점(동그라미) 이동
        aimCircle.position = targetPoint;
        // 카메라를 바라보게 하고 싶으면 아래처럼 (cam Transform 있으면)
        // aimCircle.LookAt(Camera.main.transform);
    }
}
