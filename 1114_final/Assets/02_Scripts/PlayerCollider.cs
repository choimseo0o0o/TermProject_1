using UnityEngine;
using TMPro;
using static PublicControllerValue;

public class PlayerCollider : MonoBehaviour
{
    public TextMeshProUGUI Notice_Bitten;
    public GameObject RestartButton;
    private bool IsEnded;

    [Header("Life 오브젝트(앞에서부터 3개)")]
    public GameObject[] LifeObjects;   // Life 3개 넣기

    [Header("게임 종료 UI")]
    public GameObject Notice_Ended;

    [Header("컨티뉴 키 설정")]
    [SerializeField] private KeyCode continueKey = KeyCode.C;

    void Update()
    {
        // 이미 게임 종료면 컨티뉴 불가
        if (IsEnded)
            return;

        // 물렸다는 안내가 떠 있을 때만 C키로 컨티뉴 허용
        if (Notice_Bitten != null && Notice_Bitten.gameObject.activeSelf)
        {
            if (RightContA)
            {
                // 안내/버튼 숨기고 다시 움직이게
                Notice_Bitten.gameObject.SetActive(false);

                if (RestartButton != null)
                    RestartButton.SetActive(false);

                WeaponKeyboardAim.PlayerCanMove = true;   // 다시 이동 가능

                Debug.Log("[PlayerCollider] Continue 키 입력 - 게임 이어서 진행");
                RightContA = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Zombie"))
            return;

        Debug.Log("충돌 감지: " + other.gameObject.name);
        Point_Continue = true;

        // 좀비에 닿으면 일단 멈추고 Life 처리
        WeaponKeyboardAim.PlayerCanMove = false;
        HandleLifeSystem();

        // 마지막 목숨이 아니면 → “물림 안내 + 컨티뉴 버튼” 표시
        if (!IsEnded)
        {
            if (Notice_Bitten != null)
                Notice_Bitten.gameObject.SetActive(true);

            if (RestartButton != null)
                RestartButton.SetActive(true);
        }

        // 닿은 좀비 비활성화 + PlayerBitten 플래그
        other.gameObject.SetActive(false);
        WeaponKeyboardAim.PlayerBitten = true;
    }

    // ★ 목숨 관리 함수
    void HandleLifeSystem()
    {
        // 1) 뒤에서부터 활성된 Life 하나 끄기
        for (int i = LifeObjects.Length - 1; i >= 0; i--)
        {
            if (LifeObjects[i] != null && LifeObjects[i].activeSelf)
            {
                LifeObjects[i].SetActive(false);
                break;   // 하나만 끄고 나감
            }
        }

        // 2) 남은 Life가 하나도 없으면 → 이번 피격으로 마지막 Life가 사라진 것
        bool anyLifeLeft = false;

        for (int i = 0; i < LifeObjects.Length; i++)
        {
            if (LifeObjects[i] != null && LifeObjects[i].activeSelf)
            {
                anyLifeLeft = true;
                break;
            }
        }

        if (!anyLifeLeft)
        {
            if (Notice_Ended != null)
                Notice_Ended.SetActive(true);

            IsEnded = true;
            Debug.Log("[PlayerCollider] 모든 Life 소진 → Game Over");
        }
    }
}
