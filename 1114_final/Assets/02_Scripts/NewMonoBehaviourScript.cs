using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HudCornerLock : MonoBehaviour
{
    public enum Corner { TopLeft, TopRight, BottomLeft, BottomRight }
    public Corner corner = Corner.TopLeft;
    public Vector2 offset = new Vector2(40f, -40f); // 

    void Awake()
    {
        var rt = GetComponent<RectTransform>();

        Vector2 anchorMin, anchorMax, pivot;
        switch (corner)
        {
            case Corner.TopLeft:
                anchorMin = anchorMax = pivot = new Vector2(0f, 1f);
                break;
            case Corner.TopRight:
                anchorMin = anchorMax = pivot = new Vector2(1f, 1f);
                break;
            case Corner.BottomLeft:
                anchorMin = anchorMax = pivot = new Vector2(0f, 0f);
                break;
            default: // BottomRight
                anchorMin = anchorMax = pivot = new Vector2(1f, 0f);
                break;
        }

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = offset;
    }
}
