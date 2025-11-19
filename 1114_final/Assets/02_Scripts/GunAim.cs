using UnityEngine;

public class GunAim : MonoBehaviour
{
    [Header("참조")]
    public Transform muzzle;       // 총구 끝 (MuzzlePoint)
    public Transform aimCircle;    // 조준점(동그라미 스피어)
    public Camera cam;             // 메인 카메라

    [Header("설정")]
    public float maxDistance = 200f;              // 레이캐스트 최대 거리
    public LayerMask hitMask = ~0;                // 맞출 레이어 (기본: 전부)
    [Tooltip("카메라 쪽으로 얼마나 당길지 (단위: m)")]
    public float pullToCameraOffset = 0.05f;      // 항상 화면 앞에 보이게 살짝 당김

    [Tooltip("조준 스피어가 총구에서 최대 어느 정도까지 나갈 수 있는지 (기차 안 한계)")]
    public float maxSphereDistanceFromMuzzle = 18f; //총구 기준 최대 거리

    void Start()
    {
        // 인스펙터에서 안 넣어줬으면 자동으로 메인 카메라 할당
        if (cam == null)
            cam = Camera.main;
    }

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
            // Debug.Log($"[GunAim] Hit: {hit.collider.name} @ {hit.point}");
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxDistance;
        }

        // 🔵 2-1. 조준 스피어가 총구에서 너무 멀리 나가지 않도록 거리 제한
        //      (기차 안에서만 움직이게 하는 개념)
        Vector3 muzzleToTarget = targetPoint - muzzle.position;
        float distFromMuzzle = muzzleToTarget.magnitude;

        if (distFromMuzzle > maxSphereDistanceFromMuzzle)
        {
            Vector3 dir = muzzleToTarget.normalized;
            targetPoint = muzzle.position + dir * maxSphereDistanceFromMuzzle;
        }

        // 🔵 2-2. 카메라 쪽으로 살짝 당겨서 항상 화면 최전방에 보이게
        if (cam != null)
        {
            Vector3 camToTarget = targetPoint - cam.transform.position;
            float dist = camToTarget.magnitude;

            if (dist > pullToCameraOffset)
            {
                Vector3 dir = camToTarget.normalized;
                targetPoint = cam.transform.position + dir * (dist - pullToCameraOffset);
            }
        }

        // 3. 조준점(동그라미) 이동
        aimCircle.position = targetPoint;

        // 조준점이 카메라를 바라보게 하고 싶으면
        // if (cam != null)
        //     aimCircle.LookAt(cam.transform);
    }
}
