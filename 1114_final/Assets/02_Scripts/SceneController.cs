using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;   // 코루틴용
using static PublicControllerValue;

public class SceneController : MonoBehaviour
{
    [Header("엔딩 씬 설정")]
    [Tooltip("엔딩 씬의 이름(파일 이름)")]
    public string endingSceneName = "ending";   // 실제 엔딩 씬 이름으로 변경

    [Tooltip("3초 뒤에 나타날 Plane 오브젝트")]
    public GameObject endingPlane;

    [Tooltip("Plane이 나타나기까지의 지연 시간(초)")]
    public float planeDelay = 3f;

    [Header("페이드 설정")]
    [Tooltip("전체 화면을 덮는 CanvasGroup (검은 이미지가 붙어있는 오브젝트)")]
    public CanvasGroup fadeCanvasGroup;

    [Tooltip("페이드 인/아웃에 걸리는 시간(초)")]
    public float fadeDuration = 1f;

    private bool isTransitioning = false;   // 씬 전환 중인지

    void Start()
    {
        // 1) 페이드 인 (씬이 시작될 때)
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1f;                 // 시작은 완전 검정
            StartCoroutine(FadeIn());                   // 검정 → 화면
        }

        // 2) 엔딩 씬이라면 Plane 3초 뒤에 등장
        if (SceneManager.GetActiveScene().name == endingSceneName && endingPlane != null)
        {
            IsOkayToPressContB = true;
            endingPlane.SetActive(false);
            StartCoroutine(ShowPlaneAfterDelay());
        }
    }

    void Update()
    {
        if (RightContB && !isTransitioning)
        {
            RightContB = false;
            int current = SceneManager.GetActiveScene().buildIndex;
            int last = SceneManager.sceneCountInBuildSettings - 1;

            // 다음 씬은 항상 정상 속도로 시작하게
            Time.timeScale = 1f;

            if (current < last)
            {
                int next = current + 1;

                string currentName = SceneManager.GetActiveScene().name;
                string nextPath = SceneUtility.GetScenePathByBuildIndex(next);

                Debug.Log($"👉 CURRENT: {current} / {currentName}");
                Debug.Log($"👉 NEXT INDEX: {next}, PATH: {nextPath}");

                // 기존: 바로 LoadScene(next);
                // 변경: 페이드 아웃 후 씬 로드
                StartCoroutine(FadeOutAndLoad(next));
            }
            // current == last면 아무것도 안 함
        }
    }

    // ───────────────── 페이드 관련 ─────────────────

    private IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);

            if (fadeCanvasGroup != null)
                fadeCanvasGroup.alpha = 1f - t;   // 1 → 0

            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(false); // 완전히 투명해지면 꺼버려도 됨
        }
    }

    private IEnumerator FadeOutAndLoad(int nextBuildIndex)
    {
        isTransitioning = true;

        if (fadeCanvasGroup == null)
        {
            // 페이드용 CanvasGroup이 없으면 그냥 바로 전환
            SceneManager.LoadScene(nextBuildIndex);
            yield break;
        }

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;   // 화면에서 시작해서

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);

            fadeCanvasGroup.alpha = t;   // 0 → 1 (점점 검정)

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;   // 완전 검정 상태

        // 여기서 씬 로드
        SceneManager.LoadScene(nextBuildIndex);
    }

    // ───────────────── 엔딩 Plane 관련 ─────────────────

    private IEnumerator ShowPlaneAfterDelay()
    {
        // 설정한 시간만큼 대기
        yield return new WaitForSeconds(planeDelay);

        // Plane 나타나게
        if (endingPlane != null)
        {
            endingPlane.SetActive(true);
        }
    }
}
