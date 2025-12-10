using UnityEngine;

public class TrainWayTrigger : MonoBehaviour
{
    [Header("도착 시 Canvas/정지를 제어할 TrainWay")]
    public TrainWay trainWay;      // ← TrainWay가 붙은 오브젝트 드래그

    [Header("이 트리거를 지나가는 Player 태그")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && trainWay != null)
        {
            trainWay.ignoreCanvasAndPause = true;
            Debug.Log("[TrainWayTrigger] Player 트리거 통과 → ignoreCanvasAndPause = true");
        }
    }
}
