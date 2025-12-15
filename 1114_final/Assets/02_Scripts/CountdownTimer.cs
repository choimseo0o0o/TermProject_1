using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class CountdownTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private int maxSeconds = 80;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Quit Settings")]
    [SerializeField] private float quitDelaySeconds = 2f; // 인스펙터에서 조절

    private float currentTime;
    private TMP_Text timeText;

    private bool isGameOver = false;
    private bool isGameOverDisabled = false;

    private void Awake()
    {
        timeText = GetComponent<TMP_Text>();
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    private void Start()
    {
        currentTime = maxSeconds;
        UpdateTimeText();
    }

    private void Update()
    {
        if (isGameOver) return;
        if (isGameOverDisabled) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimeText();
            TriggerGameOver();
            return;
        }

        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        int displayTime = Mathf.CeilToInt(currentTime);
        timeText.text = $"Time : {displayTime}";
    }

    private void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // UI 보일 시간 확보 후 종료
        StartCoroutine(QuitAfterDelay());
    }

    private IEnumerator QuitAfterDelay()
    {
        // 필요하면 게임 멈추고 UI만 보여주기
        Time.timeScale = 0f;

        // WaitForSeconds는 timeScale 영향을 받으니 Realtime 사용
        yield return new WaitForSecondsRealtime(quitDelaySeconds);

        QuitGame();
    }

    public void DisableGameOver(bool disable)
    {
        isGameOverDisabled = disable;
        if (disable && gameOverUI != null) gameOverUI.SetActive(false);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
