using UnityEngine;

public class WeaponKeyboardAim : MonoBehaviour
{
    [Header("조준 설정")]
    public float rotateSpeed = 60f;  // 1초에 몇 도 회전할지 (도/초)

    [Header("총구 & 탄 설정")]
    public Transform muzzle;           // 총구 끝(총알이 나가는 위치)
    public GameObject bulletPrefab;    // Sphere(총알) 프리팹
    public float bulletSpeed = 20f;    // 총알 속도
    public float bulletLifeTime = 5f;  // 총알 자동 삭제 시간

    void Update()
    {
        HandleAim();
        HandleShoot();
    }

    void HandleAim()
    {
        // Q : 왼쪽 회전, W : 오른쪽 회전
        float rotateDir = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            // 왼쪽 (보통 Y축 기준 반시계 회전이라고 가정)
            rotateDir = -1f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            // 오른쪽
            rotateDir = 1f;
        }

        if (rotateDir != 0f)
        {
            // pivot(현재 오브젝트의 위치)을 기준으로 Y축 회전
            float deltaAngle = rotateSpeed * rotateDir * Time.deltaTime;
            transform.Rotate(0f, deltaAngle, 0f, Space.Self);
        }
    }

    void HandleShoot()
    {
        // 스페이스바를 "딱 눌렀을 때" 한 번만 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (muzzle == null || bulletPrefab == null)
                return;

            // 1. 총알 생성 (총구 위치 & 방향)
            GameObject bullet = Instantiate(
                bulletPrefab,
                muzzle.position,
                muzzle.rotation
            );

            // 2. Rigidbody가 있으면 앞으로 날려보내기
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = muzzle.forward * bulletSpeed;
                // 또는 rb.AddForce(muzzle.forward * bulletSpeed, ForceMode.VelocityChange);
            }

            // 3. 일정 시간 후 자동 삭제
            Destroy(bullet, bulletLifeTime);
        }
    }
}
