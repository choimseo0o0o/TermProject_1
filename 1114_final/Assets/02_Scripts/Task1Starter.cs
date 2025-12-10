using UnityEngine;

public class Task1Starter : MonoBehaviour
{
    void Awake()
    {
        // 엔진 기본 상태 강제 복원
        Time.timeScale = 1f;

        // 만약 네 프로젝트에 GameManager, StageManager 같은 게 있다면,
        // 여기서 “무조건 시작 상태”로 만들어주면 된다.
        //
        // 예시 (있으면 주석 풀고 실제 이름에 맞게 바꿔서 사용):
        // GameManager.Instance.isPlaying = true;
        // GameManager.Instance.isPaused = false;
        // StageManager.Instance.started = true;
        // PlayerController.gameStarted = true;
    }
}
