using UnityEngine;

public class WeaponKeyboardAim : MonoBehaviour
{
    [Header("조준 설정")]
    public float rotateSpeed = 60f;

    [Header("Ray 설정")]
    public float rayDistance = 100f;
    public Color defaultRayColor = Color.red;
    public Color firedRayColor = Color.green;
    public float cylinderRadius = 0.05f;

    [Header("씬에서 연결할 실린더 & 시작점")]
    public Transform rayCylinder;   // 실린더(자식 Cylinder)
    public Transform rayOrigin;     // 레이 출발 오브젝트 (예: Muzzle)
    public TMPro.TextMeshProUGUI Notice_Dead;
    public GameObject RestartButton;

    [Header("사운드 설정")]
    public AudioSource fireAudioSource;  // 총 소리용 AudioSource
    public AudioClip fireClip;           // 스페이스바(발사) 사운드

    private Color currentRayColor;
    private Renderer cylRenderer;
    private Material cylMat;
    public static bool PlayerCanMove;

    public static bool ContinuePlay, PlayerBitten, PlayerKilledZombie;

    public TMPro.TextMeshProUGUI killedText;
    private int killedCount = 0;

    public static string ZombieName;
    private string PreviousZombieName;

    void Awake()
    {
        currentRayColor = defaultRayColor;

        // 시작점이 비어 있으면 자기 자신 기준
        if (rayOrigin == null)
            rayOrigin = transform;

        // Cylinder 준비
        if (rayCylinder != null)
        {
            cylRenderer = rayCylinder.GetComponent<Renderer>();
            cylMat = new Material(Shader.Find("Unlit/Color"));
            cylRenderer.material = cylMat;
            cylMat.color = currentRayColor;
        }
        PlayerCanMove = true;

        if (killedText != null)
            killedText.text = "Killed : " + killedCount.ToString();
    }

    void Update()
    {
        HandleAim();

        // rayOrigin 기준으로 Ray 생성
        Vector3 originPos = rayOrigin.position;
        Vector3 originDir = rayOrigin.forward;

        Ray ray = new Ray(originPos, originDir);

        HandleRaycast(ray);
        UpdateCylinder(ray);
    }

    void HandleAim()
    {
        float dir = 0f;

        if (Input.GetKey(KeyCode.Q)) dir = -1f;
        else if (Input.GetKey(KeyCode.E)) dir = 1f;

        if (dir != 0f)
        {
            float delta = rotateSpeed * dir * Time.deltaTime;
            transform.Rotate(0f, delta, 0f, Space.Self);
        }
    }

    void HandleRaycast(Ray ray)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 🔊 스페이스바(발사) 사운드 재생
            PlayFireSound();

            currentRayColor = firedRayColor;

            int zombieLayer = LayerMask.NameToLayer("Zombie");
            int continueLayer = LayerMask.NameToLayer("Continue");

            int mask = (1 << zombieLayer) | (1 << continueLayer);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, mask))
            {
                if (hit.collider.gameObject.layer == continueLayer)
                {
                    ContinuePlay = true;
                    if (Notice_Dead != null)
                        Notice_Dead.gameObject.SetActive(false);
                    if (RestartButton != null)
                        RestartButton.SetActive(false);

                    Debug.Log("게임 재개 버튼 눌림");
                    PlayerCanMove = true;
                }

                if (hit.collider.gameObject.layer == zombieLayer)
                {
                    Debug.Log("좀비 맞음");
                    ZombieName = hit.collider.gameObject.name;
                    Debug.Log("ZombieName: " + ZombieName);
                    PlayerKilledZombie = true;

                    if (ZombieName == PreviousZombieName)
                        return;
                    else
                        PreviousZombieName = ZombieName;

                    Debug.Log("New ZombieName: " + ZombieName);
                    killedCount++;
                    Debug.Log("KillCount: " + killedCount);

                    if (killedText != null)
                        killedText.text = "Killed : " + killedCount.ToString();
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            currentRayColor = defaultRayColor;
        }
    }

    void UpdateCylinder(Ray ray)
    {
        if (rayCylinder == null || cylMat == null)
            return;

        Vector3 start = ray.origin;
        Vector3 end = ray.origin + ray.direction * rayDistance;
        Vector3 mid = (start + end) * 0.5f;

        // 위치: 시작점과 끝점의 가운데
        rayCylinder.position = mid;

        // 방향: Ray 방향을 따라가도록 (Cylinder는 Y축이 길이)
        rayCylinder.rotation =
            Quaternion.LookRotation(ray.direction) * Quaternion.Euler(90f, 0f, 0f);

        // 스케일: 길이/굵기
        rayCylinder.localScale = new Vector3(
            cylinderRadius * 2f,      // X 지름
            rayDistance * 0.5f,       // Y 길이 절반
            cylinderRadius * 2f       // Z 지름
        );

        cylMat.color = currentRayColor;

        // Scene 뷰 확인용
        Debug.DrawRay(start, ray.direction * rayDistance, currentRayColor);
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = Application.isPlaying ? currentRayColor : defaultRayColor;
        Gizmos.color = gizmoColor;

        Transform originT = (rayOrigin != null) ? rayOrigin : transform;
        Vector3 start = originT.position;
        Vector3 end = start + originT.forward * rayDistance;

        Gizmos.DrawLine(start, end);
    }

    // 🔊 발사 사운드 전용 함수
    private void PlayFireSound()
    {
        if (fireAudioSource == null || fireClip == null)
            return;

        // 같은 사운드를 연속 발사하고 싶으면 Stop()은 빼도 됨
        fireAudioSource.Stop();
        fireAudioSource.clip = fireClip;
        fireAudioSource.Play();
    }
}
