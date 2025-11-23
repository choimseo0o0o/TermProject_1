using UnityEngine;
using TMPro;

public class PlayerCollider : MonoBehaviour
{
    public TextMeshProUGUI Notice_Bitten;
    public GameObject RestartButton;
    private bool IsEnded;

    [Header("Life 오브젝트(앞에서부터 3개)")]
    public GameObject[] LifeObjects;   // 배열 3개 넣기

    [Header("게임 종료 UI")]
    public GameObject Notice_Ended;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            WeaponKeyboardAim.PlayerCanMove = false;   // ★ 플레이어 이동 불가
            HandleLifeSystem();   // ★ 목숨 처리


            if (!IsEnded)
            {
                Notice_Bitten.gameObject.SetActive(true);
                RestartButton.SetActive(true);
            }

            other.gameObject.SetActive(false);
            WeaponKeyboardAim.PlayerBitten = true;

            Debug.Log("Player has collided with a Zombie!");
        }
    }

    // ★ 목숨 관리 함수
    void HandleLifeSystem()
    {
        // 뒤에서부터 하나씩 제거
        for (int i = LifeObjects.Length - 1; i >= 0; i--)
        {
            if (LifeObjects[i] != null && LifeObjects[i].activeSelf)
            {
                LifeObjects[i].SetActive(false);
                return;   // 하나 지우고 종료
            }
        }

        // 여기까지 왔다는 것은 지울 Life가 없다 → 게임 종료
        if (Notice_Ended != null)
        {
            Notice_Ended.SetActive(true);
            IsEnded = true;
        }

    }
}