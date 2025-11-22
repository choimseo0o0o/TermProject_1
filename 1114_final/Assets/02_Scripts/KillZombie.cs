using UnityEngine;
using TMPro;
using System.Collections;

public class ZombieKillDisplay : MonoBehaviour
{
    [Header("Zombie Counters")]
    public int totalZombie = 4;
    public int killedZombie = 0;

    [Header("UI Components")]
    public TextMeshProUGUI killText;       // TextMeshProUGUI 연결
    public GameObject textObject;          // 텍스트 오브젝트

    [Header("Fade Settings")]
    public float fadeDuration = 2f;        // 서서히 사라지는 시간

    public void KilledZombie()
    {
        killedZombie = Mathf.Clamp(killedZombie + 1, 0, totalZombie);

        killText.text = $"{killedZombie} / {totalZombie}";
        textObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color c = killText.color;
        c.a = 1f;
        killText.color = c;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            killText.color = new Color(c.r, c.g, c.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        killText.color = new Color(c.r, c.g, c.b, 0f);
        textObject.SetActive(false);
    }
}