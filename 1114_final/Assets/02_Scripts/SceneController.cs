using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            int current = SceneManager.GetActiveScene().buildIndex;
            int last    = SceneManager.sceneCountInBuildSettings - 1;

            // 다음 씬은 항상 정상 속도로 시작하게
            Time.timeScale = 1f;

            if (current < last)
            {
                int next = current + 1;

                string currentName = SceneManager.GetActiveScene().name;
                string nextPath    = SceneUtility.GetScenePathByBuildIndex(next);

                Debug.Log($"👉 CURRENT: {current} / {currentName}");
                Debug.Log($"👉 NEXT INDEX: {next}, PATH: {nextPath}");

                SceneManager.LoadScene(next);
            }
            // current == last면 아무것도 안 함
        }
    }
}
