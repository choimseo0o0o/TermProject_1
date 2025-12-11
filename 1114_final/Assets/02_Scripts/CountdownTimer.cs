using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private int maxSeconds = 80;   // 인스펙터에서 설정할 최대 시간(초)

    private float currentTime;      // 실제 카운트다운용 (float)
    private TMP_Text timeText;      // "Time: xx" 를 표시하는 TextMeshPro

    void Awake()
    {
        // 같은 GameObject에 붙어있는 TMP_Text 자동으로 찾기
        timeText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        currentTime = maxSeconds;
        UpdateTimeText();
    }

    void Update()
    {
        if (currentTime <= 0f) return;

        // 매 프레임마다 시간 감소
        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
            currentTime = 0f;

        UpdateTimeText();
    }

    // 화면에 보이는 텍스트 갱신
    void UpdateTimeText()
    {
        int displayTime = Mathf.CeilToInt(currentTime);   // 20,19,18 처럼 정수로 표시
        timeText.text = $"Time : {displayTime}";
    }

    // 필요하면 외부에서 다시 리셋 가능
    public void ResetTimer()
    {
        currentTime = maxSeconds;
    }
}
