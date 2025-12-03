using UnityEngine;

public class ZombieClearChecker : MonoBehaviour
{
    [Header("Zombie 설정")]
    public string zombieTag = "Zombie";   // 좀비 Tag

    [Header("클리어 Plane 설정")]
    public GameObject clearPlane;         // 좀비 다 죽었을 때 띄울 Plane
    public float planeDistance = 2f;      // 카메라 앞 거리

    private bool planeShown = false;

    void Start()
    {
        Debug.Log("[ZombieClearChecker] Start 호출됨");

        // Plane 처음에는 꺼두고 싶으면 여기서 끈다
        if (clearPlane != null)
        {
            Debug.Log("[ZombieClearChecker] clearPlane 연결됨: " + clearPlane.name);
            clearPlane.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[ZombieClearChecker] clearPlane가 연결되지 않음 (Inspector 확인 필요)");
        }
    }

    void Update()
    {
        if (planeShown)
            return;

        CheckAllZombiesDisabled();
    }

    void CheckAllZombiesDisabled()
    {
        // 씬 안의 Zombie 태그 가진 애들 전부 찾기
        GameObject[] zombies = GameObject.FindGameObjectsWithTag(zombieTag);

        // 아예 좀비가 한 마리도 없는 상태(아직 생성 안 됐거나 세팅 안 됨)면 그냥 리턴
        if (zombies.Length == 0)
        {
            // Debug.Log("[ZombieClearChecker] Tag=Zombie 오브젝트가 하나도 없음");
            return;
        }

        int activeCount = 0;
        foreach (GameObject z in zombies)
        {
            if (z.activeInHierarchy)
                activeCount++;
        }

        Debug.Log($"[ZombieClearChecker] 좀비 전체: {zombies.Length}, 그중 활성: {activeCount}");

        // 한 마리라도 살아 있으면 아직 클리어 아님
        if (activeCount > 0)
            return;

        // 여기까지 왔으면 모든 Zombie 태그 오브젝트가 비활성화 상태
        ShowClearPlane();
    }

    void ShowClearPlane()
    {
        if (clearPlane == null)
        {
            Debug.LogWarning("[ZombieClearChecker] ShowClearPlane 호출됐지만 clearPlane가 null");
            return;
        }

        planeShown = true;
        clearPlane.SetActive(true);

        // 카메라 정면 앞에 배치
        Camera camObj = Camera.main;
        if (camObj != null)
        {
            Transform cam = camObj.transform;
            clearPlane.transform.position = cam.position + cam.forward * planeDistance;
            clearPlane.transform.rotation =
                Quaternion.LookRotation(clearPlane.transform.position - cam.position);
        }

        Debug.Log("[ZombieClearChecker] 모든 좀비 비활성화 → Plane 활성화 완료");
    }
}
