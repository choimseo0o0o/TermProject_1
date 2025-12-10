using UnityEngine;
using static PublicControllerValue;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject firstCanvas;    // 처음 활성화되는 캔버스
    public GameObject secondCanvas;   // C를 누르면 활성화할 캔버스

    void Start()
    {
        // 시작 시 첫 번째 캔버스 ON, 두 번째 캔버스 OFF
        firstCanvas.SetActive(true);
        secondCanvas.SetActive(false);
        Point_Continue = true;
    }

    void Update()
    {
        if (RightContA)
        {
            firstCanvas.SetActive(false);
            secondCanvas.SetActive(true);
            RightContA = false;
            IsOkayToPressContB = true;
        }
    }
}
